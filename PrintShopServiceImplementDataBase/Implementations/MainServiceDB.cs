using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintShopModel;
using PrintShopServiceDAL.BindingModel;
using PrintShopServiceDAL.Interfaces;
using PrintShopServiceDAL.ViewModel;
using System.Data.Entity;
using System.Data.Entity.SqlServer;


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
                PrintName = rec.Print.PrintName
            })
            .ToList();
            return result;
        }
        public void CreateIndent(IndentBindingModel model)
        {
            context.Indents.Add(new Indent
            {
                CustomerId = model.CustomerId,
                PrintId = model.PrintId,
                DateCreate = DateTime.Now,
                Count = model.Count,
                Sum = model.Sum,
                Status = IndentStatus.Принят
            });
            context.SaveChanges();
        }
        public void TakeIndentInWork(IndentBindingModel model)
        {
            
        using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Indent element = context.Indents.FirstOrDefault(rec => rec.Id ==
                   model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    if (element.Status != IndentStatus.Принят)
                    {
                        throw new Exception("Заказ не в статусе \"Принят\"");
                    }
                    var PrintIngredients = context.PrintIngredients.Include(rec =>
                    rec.Ingredient).Where(rec => rec.PrintId == element.PrintId);
                    // списываем
                    foreach (var PrintIngredient in PrintIngredients)
                    {
                        int countOnStocks = PrintIngredient.Count * element.Count;
                        var stockIngredients = context.StockIngredients.Where(rec =>
                        rec.IngredientId == PrintIngredient.IngredientId);
                        foreach (var stockIngredient in stockIngredients)
                        {
                            // компонентов на одном слкаде может не хватать
                            if (stockIngredient.Count >= countOnStocks)
                            {
                                stockIngredient.Count -= countOnStocks;
                                countOnStocks = 0;
                                context.SaveChanges();
                                break;
                            }
                            else
                            {
                                countOnStocks -= stockIngredient.Count;
                                stockIngredient.Count = 0;
                                context.SaveChanges();
                            }
                        }
                        if (countOnStocks > 0)
                        {
                            throw new Exception("Не достаточно компонента " +
                           PrintIngredient.Ingredient.IngredientName + " требуется " + PrintIngredient.Count + ", не хватает " + countOnStocks);
                         }
                    }
                    element.DateImplement = DateTime.Now;
                    element.Status = IndentStatus.Выполняется;
                    context.SaveChanges();
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
    }
}
