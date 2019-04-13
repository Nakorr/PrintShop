using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintShopServiceDAL.BindingModel;
using PrintShopServiceDAL.ViewModel;

namespace PrintShopServiceDAL.Interfaces
{
    public interface IStockService 
    {
        List<StockViewModel> GetList();
        StockViewModel GetElement(int id);
        void AddElement(StockBindingModel model);
        void UpdElement(StockBindingModel model);
        void DelElement(int id);
    }
}
