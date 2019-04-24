using PrintShopServiceDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PrintShopServiceDAL.ViewModel
{
    public class PrintViewModel
    {
        public int Id { get; set; }
        public string PrintName { get; set; }
        public decimal Price { get; set; }
        public List<PrintIngredientViewModel> PrintIngredients{ get; set; }
    }
}
