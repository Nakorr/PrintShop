using System;
using System.Collections.Generic;
using System.Linq;
using PrintShopServiceDAL.ViewModel;
using PrintShopServiceDAL.BindingModel;
using System.Text;
using System.Threading.Tasks;

namespace PrintShopServiceDAL.Interfaces
{
    public interface IMessageInfoService
    {
        List<MessageInfoViewModel> GetList();
        MessageInfoViewModel GetElement(int id);
        void AddElement(MessageInfoBindingModel model);
    }
}
