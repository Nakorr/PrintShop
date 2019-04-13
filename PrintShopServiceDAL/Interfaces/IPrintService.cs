using PrintShopServiceDAL.BindingModel;
using PrintShopServiceDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShopServiceDAL.Interfaces
{
    public interface IPrintService
    {
        List<PrintViewModel> GetList();
        PrintViewModel GetElement(int id);
        void AddElement(PrintBindingModel model);
        void UpdElement(PrintBindingModel model);
        void DelElement(int id);
    }
}
