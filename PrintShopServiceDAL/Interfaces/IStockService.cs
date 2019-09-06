using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintShopServiceDAL.BindingModel;
using PrintShopServiceDAL.ViewModel;
using PrintShopServiceDAL.Attributies;


namespace PrintShopServiceDAL.Interfaces
{
    [CustomInterface("Интерфейс для работы с складами")]

    public interface IStockService
    {
        [CustomMethod("Метод получения списка складов")]

        List<StockViewModel> GetList();
        [CustomMethod("Метод получения склада по id")]

        StockViewModel GetElement(int id);
        [CustomMethod("Метод получения склада по id")]

        void AddElement(StockBindingModel model);
        [CustomMethod("Метод изменения данных по складу")]

        void UpdElement(StockBindingModel model);
        [CustomMethod("Метод удаления склада")]

        void DelElement(int id);
    }
}
