using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintShopModel;
using PrintShopServiceDAL.BindingModel;
using PrintShopServiceDAL.Interfaces;
using PrintShopServiceDAL.ViewModel;

namespace PrintShopServiceImplementDataBase.Implementations
{
   public class PrintServiceDB : IPrintService
    {
        private PrintShopDbContext context;
        public PrintServiceDB(PrintShopDbContext context)
        {
            this.context = context;
        }
        public List<PrintViewModel> GetList()
        {
            List<PrintViewModel> result = context.Prints.Select(rec => new
           PrintViewModel
            {
                Id = rec.Id,
                PrintName = rec.PrintName,
                Price = rec.Price,
                PrintIngredients = context.PrintIngredients
            .Where(recPC => recPC.PrintId == rec.Id)
           .Select(recPC => new PrintIngredientViewModel
           {
               Id = recPC.Id,
               PrintId = recPC.PrintId,
               IngredientId = recPC.IngredientId,
               IngredientName = recPC.Ingredient.IngredientName,
               Count = recPC.Count
           })
           .ToList()
            })
            .ToList();
            return result;
        }
        public PrintViewModel GetElement(int id)
        {
            Print element = context.Prints.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new PrintViewModel
            {
                    Id = element.Id,
PrintName = element.PrintName,
Price = element.Price,
PrintIngredients = context.PrintIngredients
 .Where(recPC => recPC.PrintId == element.Id)
 .Select(recPC => new PrintIngredientViewModel
 {
     Id = recPC.Id,
     PrintId = recPC.PrintId,
     IngredientId = recPC.IngredientId,
     IngredientName = recPC.Ingredient.IngredientName,
     Count = recPC.Count
 })
 .ToList()
 };
            }
            throw new Exception("Элемент не найден");
        }
        public void AddElement(PrintBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Print element = context.Prints.FirstOrDefault(rec =>
                   rec.PrintName == model.PrintName);
                    if (element != null)
                    {
                        throw new Exception("Уже есть изделие с таким названием");
                    }
                    element = new Print
                    {
                        PrintName = model.PrintName,
                        Price = model.Price
                    };
                    context.Prints.Add(element);
                    context.SaveChanges();
                    // убираем дубли по компонентам
                    var groupIngredients = model.PrintIngredient
                     .GroupBy(rec => rec.IngredientId)
                    .Select(rec => new
                    {
                        IngredientId = rec.Key,
                        Count = rec.Sum(r => r.Count)
                    });
                    // добавляем компоненты
                    foreach (var groupIngredient in groupIngredients)
                    {
                        context.PrintIngredients.Add(new PrintIngredient
                        {
                            PrintId = element.Id,
                            IngredientId = groupIngredient.IngredientId,
                            Count = groupIngredient.Count
                        });
                        context.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                    
                }
            }
        }
        public void UpdElement(PrintBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Print element = context.Prints.FirstOrDefault(rec =>
                   rec.PrintName == model.PrintName && rec.Id != model.Id);
                    if (element != null)
                    {
                        throw new Exception("Уже есть изделие с таким названием");
                    }
                    element = context.Prints.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    element.PrintName = model.PrintName;
                    element.Price = model.Price;
                    context.SaveChanges();
                    // обновляем существуюущие компоненты
                    var compIds = model.PrintIngredient.Select(rec =>
                   rec.IngredientId).Distinct();
                    var updateIngredients = context.PrintIngredients.Where(rec =>
                   rec.PrintId == model.Id && compIds.Contains(rec.IngredientId));
                    foreach (var updateIngredient in updateIngredients)
                    {
                        updateIngredient.Count =
                       model.PrintIngredient.FirstOrDefault(rec => rec.Id == updateIngredient.Id).Count;
                    }
                    context.SaveChanges();
                    context.PrintIngredients.RemoveRange(context.PrintIngredients.Where(rec =>
                    rec.PrintId == model.Id && !compIds.Contains(rec.IngredientId)));
                    context.SaveChanges();
                    // новые записи
                    var groupIngredients = model.PrintIngredient
                    .Where(rec => rec.Id == 0)
                   .GroupBy(rec => rec.IngredientId)
                   .Select(rec => new
                   {
                       IngredientId = rec.Key,
                       Count = rec.Sum(r => r.Count)
                   });
                    foreach (var groupIngredient in groupIngredients)
                    {
                        PrintIngredient elementPC =
                       context.PrintIngredients.FirstOrDefault(rec => rec.PrintId == model.Id &&
                       rec.IngredientId == groupIngredient.IngredientId);
                        if (elementPC != null)
                        {
                            elementPC.Count += groupIngredient.Count;
                            context.SaveChanges();
                        }
                        else
                        {
                            context.PrintIngredients.Add(new PrintIngredient
                            {
                                PrintId = model.Id,
                                
                            IngredientId = groupIngredient.IngredientId,
                                Count = groupIngredient.Count
                            });
                            context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        public void DelElement(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Print element = context.Prints.FirstOrDefault(rec => rec.Id ==
                   id);
                    if (element != null)
                    {
                        // удаяем записи по компонентам при удалении изделия
                        context.PrintIngredients.RemoveRange(context.PrintIngredients.Where(rec =>
                        rec.PrintId == id));
                        context.Prints.Remove(element);
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Элемент не найден");
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
