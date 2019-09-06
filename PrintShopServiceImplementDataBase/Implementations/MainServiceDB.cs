using PrintShopModel;
using PrintShopServiceDAL.BindingModel;
using PrintShopServiceDAL.Interfaces;
using PrintShopServiceDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Mail;
using System.Net;

namespace PrintShopServiceImplementDataBase.Implementations
{
    public class MainServiceDB : IMainService
    {
        private PrintShopDbContext context;
        public MainServiceDB(PrintShopDbContext context)
        {
            this.context = context;
        }
        public List<IndentViewModel> GetList()
        {
            List<IndentViewModel> result = context.Indents.Select(rec => new IndentViewModel
            {
                Id = rec.Id,
                CustomerId = rec.CustomerId,
                PrintId = rec.PrintId,
                DateCreate = SqlFunctions.DateName("dd", rec.DateCreate) + " " +
            SqlFunctions.DateName("mm", rec.DateCreate) + " " +
            SqlFunctions.DateName("yyyy", rec.DateCreate),
                DateImplement = rec.DateImplement == null ? "" :
            SqlFunctions.DateName("dd",
           rec.DateImplement.Value) + " " +
            SqlFunctions.DateName("mm",
           rec.DateImplement.Value) + " " +
            SqlFunctions.DateName("yyyy",
           rec.DateImplement.Value),
                Status = rec.Status.ToString(),
                Count = rec.Count,
                Sum = rec.Sum,
                CustomerFIO = rec.Customer.CustomerFIO,
                PrintName = rec.Print.PrintName,
                ImplementerId = rec.Implementer.Id,
                ImplementerName = rec.Implementer.ImplementerFIO
            })
            .ToList();
            return result;
        }
        public void CreateIndent(IndentBindingModel model)
        {
            var indent = new Indent
            {
                CustomerId = model.CustomerId,
                PrintId = model.PrintId,
                DateCreate = DateTime.Now,
                Count = model.Count,
                Sum = model.Sum,
                Status = IndentStatus.Принят
            };
            context.Indents.Add(indent);
            context.SaveChanges();

            var customer = context.Customers.FirstOrDefault(x => x.Id == model.CustomerId);
            SendEmail(customer.Mail, "Оповещение по заказам", $"Заказ №{indent.Id} от {indent.DateCreate.ToShortDateString()} создан успешно");
        }
        public void TakeIndentInWork(IndentBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Indent element = context.Indents.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    if (element.Status != IndentStatus.Принят)
                    {
                        throw new Exception("Заказ не в статусе \"Принят\"");
                    }
                    var pizzaIngredients = context.PrintIngredients.Include(rec => rec.Ingredient).Where(rec => rec.PrintId == element.PrintId);
                    // списываем
                    foreach (var pizzaIngredient in pizzaIngredients)
                    {
                        int countOnStocks = pizzaIngredient.Count * element.Count;
                        var storageIngredients = context.StockIngredients.Where(rec => rec.IngredientId == pizzaIngredient.IngredientId);
                        foreach (var storageIngredient in storageIngredients)
                        {
                            // компонентов на одном слкаде может не хватать
                            if (storageIngredient.Count >= countOnStocks)
                            {
                                storageIngredient.Count -= countOnStocks;
                                countOnStocks = 0;
                                context.SaveChanges();
                                break;
                            }
                            else
                            {
                                countOnStocks -= storageIngredient.Count;
                                storageIngredient.Count = 0;
                                context.SaveChanges();
                            }
                        }
                        if (countOnStocks > 0)
                        {
                            throw new Exception("Не достаточно компонента " +
                           pizzaIngredient.Ingredient.IngredientName + " требуется " + pizzaIngredient.Count + ", не хватает " + countOnStocks);
                        }
                    }
                    element.ImplementerId = model.ImplementerId;
                    element.DateImplement = DateTime.Now;
                    element.Status = IndentStatus.Выполняется;
                    context.SaveChanges();
                    SendEmail(element.Customer.Mail, "Оповещение по заказам", $"Заказ №{element.Id} от {element.DateCreate.ToShortDateString()} передеан в работу");
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        public void FinishIndent(IndentBindingModel model)
        {
            Indent element = context.Indents.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            if (element.Status != IndentStatus.Выполняется)
            {
                throw new Exception("Заказ не в статусе \"Выполняется\"");
            }
            element.Status = IndentStatus.Готов;
            context.SaveChanges();
            SendEmail(element.Customer.Mail, "Оповещение по заказам", $"Заказ №{element.Id} от {element.DateCreate.ToShortDateString()} передан на оплату");
        }

        public void PayIndent(IndentBindingModel model)
        {
            Indent element = context.Indents.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            if (element.Status != IndentStatus.Готов)
            {
                throw new Exception("Заказ не в статусе \"Готов\"");
            }
            element.Status = IndentStatus.Оплачен;
            context.SaveChanges();
            SendEmail(element.Customer.Mail, "Оповещение по заказам", $"Заказ №{element.Id} от {element.DateCreate.ToShortDateString()} оплачен успешно");
        }

        public void PutIngredientOnStock(StockIngredientBindingModel model)
        {
            StockIngredient element = context.StockIngredients.FirstOrDefault(rec =>
           rec.StockId == model.StockId && rec.IngredientId == model.IngredientId);
            if (element != null)
            {
                element.Count += model.Count;
            }
            else
            {
                context.StockIngredients.Add(new StockIngredient
                {
                    StockId = model.StockId,
                    IngredientId = model.IngredientId,
                    Count = model.Count
                });
            }
            context.SaveChanges();
        }

        public List<IndentViewModel> GetFreeIndents()
        {
            List<IndentViewModel> result = context.Indents
            .Where(x => x.Status == IndentStatus.Принят || x.Status ==
           IndentStatus.НедостаточноРесурсов)
            .Select(rec => new IndentViewModel
            {
                Id = rec.Id
            })
            .ToList();
            return result;
        }

        private void SendEmail(string mailAddress, string subject, string text)
        {
            MailMessage objMailMessage = new MailMessage();
            SmtpClient objSmtpClient = null;
            try
            {
                string login = ConfigurationManager.AppSettings["MailLogin"];
                objMailMessage.From = new
               MailAddress(login);
                objMailMessage.To.Add(new MailAddress(mailAddress));
                objMailMessage.Subject = subject;
                objMailMessage.Body = text;
                objMailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                objMailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                objSmtpClient = new SmtpClient("smtp.gmail.com", 587);
                objSmtpClient.UseDefaultCredentials = false;
                objSmtpClient.EnableSsl = true;
                objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                objSmtpClient.Credentials = new
               NetworkCredential(ConfigurationManager.AppSettings["MailLogin"],
               ConfigurationManager.AppSettings["MailPassword"]);
                objSmtpClient.Send(objMailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objMailMessage = null;
                objSmtpClient = null;
            }
        }
    }
}