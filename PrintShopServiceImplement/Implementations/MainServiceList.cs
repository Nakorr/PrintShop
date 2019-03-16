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
    public class MainServiceList : IMainService
    {
        private DataListSingleton source;
        public MainServiceList()
        {
            source = DataListSingleton.GetInstance();
        }
        public List<IndentViewModel> GetList()
        {
            List<IndentViewModel> result = source.Indents
            .Select(rec => new IndentViewModel
            {
                Id = rec.Id,
                CustomerId = rec.CustomerId,
                PrintId = rec.PrintId,
                DateCreate = rec.DateCreate.ToLongDateString(),
                DateImplement = rec.DateImplement?.ToLongDateString(),
                Status = rec.Status.ToString(),
                Count = rec.Count,
                Sum = rec.Sum,
                CustomerFIO = source.Customers.FirstOrDefault(recC => recC.Id ==
     rec.CustomerId)?.CustomerFIO,
                PrintName = source.Prints.FirstOrDefault(recP => recP.Id ==
    rec.PrintId)?.PrintName,
            })
            .ToList();
            return result;
        }
        public void CreateIndent(IndentBindingModel model)
        {
            int maxId = source.Indents.Count > 0 ? source.Indents.Max(rec => rec.Id) : 0;
            source.Indents.Add(new Indent
            {
                Id = maxId + 1,
                CustomerId = model.CustomerId,
                PrintId = model.PrintId,
                DateCreate = DateTime.Now,
                Count = model.Count,
                Sum = model.Sum,
                Status = IndentStatus.Принят
            });
        }
        public void TakeIndentInWork(IndentBindingModel model)
        {
            Indent element = source.Indents.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            if (element.Status != IndentStatus.Принят)
            {
                throw new Exception("Заказ не в статусе \"Принят\"");
            }
            // смотрим по количеству компонентов на складах
            var PrintIngredients = source.PrintIngredient.Where(rec => rec.PrintId
           == element.PrintId);
            
 foreach (var PrintIngredient in PrintIngredients)
            {
                int countOnStocks = source.StockIngredient
                .Where(rec => rec.IngredientId ==
               PrintIngredient.IngredientId)
               .Sum(rec => rec.Count);
                if (countOnStocks < PrintIngredient.Count * element.Count)
                {
                    var IngredientName = source.Ingredients.FirstOrDefault(rec => rec.Id ==
                   PrintIngredient.IngredientId);
                    throw new Exception("Не достаточно ингредиента " +
                   IngredientName?.IngredientName + " требуется " + (PrintIngredient.Count * element.Count) +
                   ", в наличии " + countOnStocks);
                }
            }
            // списываем
            foreach (var PrintIngredient in PrintIngredients)
            {
                int countOnStocks = PrintIngredient.Count * element.Count;
                var StockIngredients = source.StockIngredient.Where(rec => rec.IngredientId
               == PrintIngredient.IngredientId);
                foreach (var stockIngredient in StockIngredients)
                {
                    // компонентов на одном слкаде может не хватать
                    if (stockIngredient.Count >= countOnStocks)
                    {
                        stockIngredient.Count -= countOnStocks;
                        break;
                    }
                    else
                    {
                        countOnStocks -= stockIngredient.Count;
                        stockIngredient.Count = 0;
                    }
                }
            }
            element.DateImplement = DateTime.Now;
            element.Status = IndentStatus.Выполняется;
        }
        public void FinishIndent(IndentBindingModel model)
        {
            Indent element = source.Indents.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
                
            }
            if (element.Status != IndentStatus.Выполняется)
            {
                throw new Exception("Заказ не в статусе \"Выполняется\"");
            }
            element.Status = IndentStatus.Готов;
        }
        public void PayIndent(IndentBindingModel model)
        {
            Indent element = source.Indents.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            if (element.Status != IndentStatus.Готов)
            {
                throw new Exception("Заказ не в статусе \"Готов\"");
            }
            element.Status = IndentStatus.Оплачен;
        }
        public void PutIngredientOnStock(StockIngredientBindingModel model)
        {
            StockIngredient element = source.StockIngredient.FirstOrDefault(rec =>
           rec.StockId == model.StockId && rec.IngredientId == model.IngredientId);
            if (element != null)
            {
                element.Count += model.Count;
            }
            else
            {
                int maxId = source.StockIngredient.Count > 0 ?
               source.StockIngredient.Max(rec => rec.Id) : 0;
                source.StockIngredient.Add(new StockIngredient
                {
                    Id = ++maxId,
                    StockId = model.StockId,
                    IngredientId = model.IngredientId,
                    Count = model.Count
                });
            }
        }
    }
}
