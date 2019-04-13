using PrintShopModel;
using PrintShopServiceDAL.BindingModel;
using PrintShopServiceDAL.Interfaces;
using PrintShopServiceDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShopServiceImplement.Implementations
{
    public class PrintServiceList : IPrintService
    {
        private DataListSingleton source;
        public PrintServiceList()
        {
            source = DataListSingleton.GetInstance();
        }
        public List<PrintViewModel> GetList()
        {
            List<PrintViewModel> result = source.Prints
    .Select(rec => new PrintViewModel
    {
        Id = rec.Id,
        PrintName = rec.PrintName,
        Price = rec.Price,
        PrintIngredients = source.PrintIngredient
    .Where(recPC => recPC.PrintId == rec.Id)
   .Select(recPC => new PrintIngredientViewModel
   {
       Id = recPC.Id,
       PrintId = recPC.PrintId,
       IngredientId = recPC.IngredientId,
       IngredientName = source.Ingredients.FirstOrDefault(recC =>
    recC.Id == recPC.IngredientId)?.IngredientName,
       Count = recPC.Count
   })
   .ToList()
    })
    .ToList();
            
 return result;
        }
        public PrintViewModel GetElement(int id)
        {
            Print element = source.Prints.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new PrintViewModel
                {
                    Id = element.Id,
                    PrintName = element.PrintName,
                    Price = element.Price,
                    PrintIngredients = source.PrintIngredient
                .Where(recPC => recPC.PrintId == element.Id)
                .Select(recPC => new PrintIngredientViewModel
                {
                    Id = recPC.Id,
                    PrintId = recPC.PrintId,
                    IngredientId = recPC.IngredientId,
                    IngredientName = source.Ingredients.FirstOrDefault(recC =>
     recC.Id == recPC.IngredientId)?.IngredientName,
                    Count = recPC.Count
                })
               .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }
        public void AddElement(PrintBindingModel model)
        {
            Print element = source.Prints.FirstOrDefault(rec => rec.PrintName ==
           model.PrintName);
            if (element != null)
            {
                throw new Exception("Уже есть книга с таким названием");
            }
            int maxId = source.Prints.Count > 0 ? source.Prints.Max(rec => rec.Id) :
           0;
            source.Prints.Add(new Print
            {
                Id = maxId + 1,
                PrintName = model.PrintName,
                Price = model.Price
            });
            // компоненты для изделия
            int maxPCId = source.PrintIngredient.Count > 0 ?
           source.PrintIngredient.Max(rec => rec.Id) : 0;
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
                source.PrintIngredient.Add(new PrintIngredient
                {
                    Id = ++maxPCId,
                    PrintId = maxId + 1,
                    
                IngredientId = groupIngredient.IngredientId,
                    Count = groupIngredient.Count
                });
            }
        }
        public void UpdElement(PrintBindingModel model)
        {
            Print element = source.Prints.FirstOrDefault(rec => rec.PrintName ==
           model.PrintName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть книга с таким названием");
            }
            element = source.Prints.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.PrintName = model.PrintName;
            element.Price = model.Price;
            int maxPCId = source.PrintIngredient.Count > 0 ?
           source.PrintIngredient.Max(rec => rec.Id) : 0;
            // обновляем существуюущие компоненты
            var compIds = model.PrintIngredient.Select(rec =>
           rec.IngredientId).Distinct();
            var updateIngredients = source.PrintIngredient.Where(rec => rec.PrintId ==
           model.Id && compIds.Contains(rec.IngredientId));
            foreach (var updateIngredient in updateIngredients)
            {
                updateIngredient.Count = model.PrintIngredient.FirstOrDefault(rec =>
               rec.Id == updateIngredient.Id).Count;
            }
            source.PrintIngredient.RemoveAll(rec => rec.PrintId == model.Id &&
           !compIds.Contains(rec.IngredientId));
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
                PrintIngredient elementPC = source.PrintIngredient.FirstOrDefault(rec
               => rec.PrintId == model.Id && rec.IngredientId == groupIngredient.IngredientId);
                if (elementPC != null)
                {
                    elementPC.Count += groupIngredient.Count;
                }
                else
                {
                    source.PrintIngredient.Add(new PrintIngredient
                    {
                        Id = ++maxPCId,
                        PrintId = model.Id,
                        IngredientId = groupIngredient.IngredientId,
                        Count = groupIngredient.Count
                    });
                }
            }
            
        }
        public void DelElement(int id)
        {
            Print element = source.Prints.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                // удаяем записи по компонентам при удалении изделия
                source.PrintIngredient.RemoveAll(rec => rec.PrintId == id);
                source.Prints.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}

