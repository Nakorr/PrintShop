using PrintShopServiceDAL.ViewModel;
using PrintShopServiceDAL.BindingModel;
using PrintShopServiceDAL.Attributies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShopServiceDAL.Interfaces
{
    [CustomInterface("Интерфейс для работы с клиентами")]

    public interface ICustomerService
    {
        [CustomMethod("Метод получения списка клиентов")]
        List<CustomerViewModel> GetList();
        [CustomMethod("Метод получения клиента по id")]
        CustomerViewModel GetElement(int id);
        [CustomMethod("Метод добавления клиента")]
        void AddElement(CustomerBindingModel model);
        [CustomMethod("Метод изменения данных по клиенту")]
        void UpdElement(CustomerBindingModel model);
        [CustomMethod("Метод удаления клиента")]
        void DelElement(int id);
    }
}
