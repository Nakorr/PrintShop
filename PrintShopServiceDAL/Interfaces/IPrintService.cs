using PrintShopServiceDAL.BindingModel;
using PrintShopServiceDAL.ViewModel;
using PrintShopServiceDAL.Attributies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShopServiceDAL.Interfaces
{
    [CustomInterface("Интерфейс для работы с книгами")]

    public interface IPrintService
    {
        [CustomMethod("Метод получения списка книгами")]

        List<PrintViewModel> GetList();
        [CustomMethod("Метод получения книг по id")]

        PrintViewModel GetElement(int id);
        [CustomMethod("Метод добавления книг")]

        void AddElement(PrintBindingModel model);
        [CustomMethod("Метод изменения данных по книге")]

        void UpdElement(PrintBindingModel model);
        [CustomMethod("Метод удаления книг")]

        void DelElement(int id);
    }
}
