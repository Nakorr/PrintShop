using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintShopServiceDAL.ViewModel;
using PrintShopServiceDAL.BindingModel;
using PrintShopServiceDAL.Attributies;

namespace PrintShopServiceDAL.Interfaces
{
    [CustomInterface("Интерфейс для работы с сотрудниками")]

    public interface IImplementerService
    {
        [CustomMethod("Метод получения списка сотруников")]
        List<ImplementerViewModel> GetList();
        [CustomMethod("Метод получения сотрудника по id")]
        ImplementerViewModel GetElement(int id);
        [CustomMethod("Метод добавления сотрудников")]
        void AddElement(ImplementerBindingModel model);
        [CustomMethod("Метод изменения данных по сотрудникам")]
        void UpdElement(ImplementerBindingModel model);
        [CustomMethod("Метод удаления сотрудника")]
        void DelElement(int id);
        [CustomMethod("Метод удаления сотрудника")]
        ImplementerViewModel GetFreeWorker();
    }
}
