using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintShopModel;
using PrintShopServiceDAL.BindingModel;
using PrintShopServiceDAL.Interfaces;
using PrintShopServiceDAL.ViewModel;

namespace PrintShopServiceImplement.Implementations
{
    public class StockServiceList : IStockService
    {
        private DataListSingleton source;
        public StockServiceList()
        {
            source = DataListSingleton.GetInstance();
        }
        public List<StockViewModel> GetList()
        {
            List<StockViewModel> result = source.Stock
            .Select(rec => new StockViewModel
            {
                Id = rec.Id,
                StockName = rec.StockName,
                StockIngredients = source.StockIngredient
            .Where(recPC => recPC.StockId == rec.Id)
           .Select(recPC => new StockIngredientViewModel
           {
               Id = recPC.Id,
               StockId = recPC.StockId,
               IngredientId = recPC.IngredientId,
               IngredientName = source.Ingredients
            .FirstOrDefault(recC => recC.Id ==
           recPC.IngredientId)?.IngredientName,
               Count = recPC.Count
           })
           .ToList()
            })
            .ToList();
            return result;
        }
        public StockViewModel GetElement(int id)
        {
            Stock element = source.Stock.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new StockViewModel
                {
                    Id = element.Id,
                    StockName = element.StockName,
                    StockIngredients = source.StockIngredient
                .Where(recPC => recPC.StockId == element.Id)
               .Select(recPC => new StockIngredientViewModel
               {
                   Id = recPC.Id,
                   StockId = recPC.StockId,
                   IngredientId = recPC.IngredientId,
                   IngredientName = source.Ingredients
                .FirstOrDefault(recC => recC.Id ==
               recPC.IngredientId)?.IngredientName,
                   Count = recPC.Count
               })
               .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }
        public void AddElement(StockBindingModel model)
        {
            Stock element = source.Stock.FirstOrDefault(rec => rec.StockName ==
           model.StockName);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            int maxId = source.Stock.Count > 0 ? source.Stock.Max(rec => rec.Id) : 0;
            source.Stock.Add(new Stock
            {
                Id = maxId + 1,
                StockName = model.StockName
            });
        }
        public void UpdElement(StockBindingModel model)
        {
            Stock element = source.Stock.FirstOrDefault(rec =>
            rec.StockName == model.StockName && rec.Id !=
           model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            element = source.Stock.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.StockName = model.StockName;
        }
        public void DelElement(int id)
        {
            Stock element = source.Stock.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                // при удалении удаляем все записи о компонентах на удаляемом складе
                source.StockIngredient.RemoveAll(rec => rec.StockId == id);
                source.Stock.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}

