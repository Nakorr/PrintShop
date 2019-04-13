using PrintShopModel;
using PrintShopServiceDAL.BindingModel;
using PrintShopServiceDAL.Interfaces;
using PrintShopServiceDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShopServiceImplementDataBase.Implementations
{
    public class StockServiceDB : IStockService
    {
        private PrintShopDbContext context;
        public StockServiceDB(PrintShopDbContext context)
        {
            this.context = context;
        }
        public List<StockViewModel> GetList()
        {
            List<StockViewModel> result = context.Stocks.Select(rec => new StockViewModel
            {
                Id = rec.Id,
                StockName = rec.StockName
            })
            .ToList();
            return result;
        }
        public StockViewModel GetElement(int id)
        {
            Stock element = context.Stocks.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new StockViewModel
                {
                    Id = element.Id,
                    StockName = element.StockName
                };
            }
            throw new Exception("Элемент не найден");
        }
        public void AddElement(StockBindingModel model)
        {
            Stock element = context.Stocks.FirstOrDefault(rec => rec.StockName == model.StockName);
            if (element != null)
            {
                throw new Exception("Уже есть клиент с таким ФИО");
            }
            context.Stocks.Add(new Stock
            {
                StockName = model.StockName
            });
            context.SaveChanges();
        }
        public void UpdElement(StockBindingModel model)
        {
            Stock element = context.Stocks.FirstOrDefault(rec => rec.StockName == model.StockName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }
            element = context.Stocks.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.StockName = model.StockName;
            context.SaveChanges();
        }
        public void DelElement(int id)
        {
            Stock element = context.Stocks.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                context.Stocks.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}