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
    [CustomInterface("Интерфейс для работы с отчётами")]

    public interface IReportService
    {
        [CustomMethod("Метод добавления отчёта")]

        void SavePrintPrice(ReportBindingModel model);
        [CustomMethod("Метод получения списка отчётов")]

        List<StocksLoadViewModel> GetStocksLoad();
        [CustomMethod("Метод получения отчёта по id")]

        void SaveStocksLoad(ReportBindingModel model);
        [CustomMethod("Метод получения списка отчётов")]

        List<CustomerIndentsModel> GetCustomerIndents(ReportBindingModel model);
        [CustomMethod("Метод получения отчёта по id")]

        void SaveCustomerIndents(ReportBindingModel model);

    }
}
