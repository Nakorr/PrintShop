using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintShopServiceDAL.ViewModel;
using PrintShopServiceDAL.BindingModel;

namespace PrintShopServiceDAL.Interfaces
{
   public interface IReportService
    {
        void SavePrintPrice(ReportBindingModel model);
        List<StocksLoadViewModel> GetStocksLoad();
        void SaveStocksLoad(ReportBindingModel model);
        List<CustomerIndentsModel> GetCustomerIndents(ReportBindingModel model);
        void SaveCustomerIndents(ReportBindingModel model);

    }
}
