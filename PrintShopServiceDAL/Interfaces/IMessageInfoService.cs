using System;
using System.Collections.Generic;
using System.Linq;
using PrintShopServiceDAL.ViewModel;
using PrintShopServiceDAL.BindingModel;
using PrintShopServiceDAL.Attributies;
using System.Text;
using System.Threading.Tasks;

namespace PrintShopServiceDAL.Interfaces
{
    [CustomInterface("Интерфейс для работы с письмами")]

    public interface IMessageInfoService
    {
        [CustomMethod("Метод получения списка писем")]

        List<MessageInfoViewModel> GetList();
        [CustomMethod("Метод получения письма по id")]

        MessageInfoViewModel GetElement(int id);
        [CustomMethod("Метод добавления письма")]

        void AddElement(MessageInfoBindingModel model);
    }
}
