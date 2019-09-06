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
    [CustomInterface("Интерфейс для работы с заказами")]

    public interface IMainService
    {
        [CustomMethod("Метод получения списка заказов")]

        List<IndentViewModel> GetList();
        [CustomMethod("Метод получения списка свободных заказов")]

        List<IndentViewModel> GetFreeIndents();
        [CustomMethod("Метод добавления заказа")]

        void CreateIndent(IndentBindingModel model);
        [CustomMethod("Метод отправления заказа в работу")]

        void TakeIndentInWork(IndentBindingModel model);
        [CustomMethod("Метод выполнения заказа")]

        void FinishIndent(IndentBindingModel model);
        [CustomMethod("Метод получения оплаты заказа")]

        void PayIndent(IndentBindingModel model);
        [CustomMethod("Метод добавления ингредиентов на склад")]

        void PutIngredientOnStock(StockIngredientBindingModel model);
    }
}
