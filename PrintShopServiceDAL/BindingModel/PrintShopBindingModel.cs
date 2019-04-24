using PrintShopServiceDAL;
using PrintShopServiceDAL.BindingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace PrintShopServiceDAL.BindingModel
{
    public class PrintBindingModel
    {
        public int Id { get; set; }
        public string PrintName { get; set; }
        public decimal Price { get; set; }
        public List<PrintIngredientBindingModel> PrintIngredient { get; set; }
    }
}
