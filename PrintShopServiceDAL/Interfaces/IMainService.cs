using PrintShopServiceDAL.BindingModel;
using PrintShopServiceDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShopServiceDAL.Interfaces
{
    public interface IMainService
    {
        List<IndentViewModel> GetList();
        void CreateIndent(IndentBindingModel model);
        void TakeIndentInWork(IndentBindingModel model);
        void FinishIndent(IndentBindingModel model);
        void PayIndent(IndentBindingModel model);
        void PutIngredientOnStock(StockIngredientBindingModel model);
    }
}
