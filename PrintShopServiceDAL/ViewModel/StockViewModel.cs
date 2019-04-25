using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.Serialization;


namespace PrintShopServiceDAL.ViewModel
{
    public class StockViewModel
    {
        public int Id { get; set; }
        [DisplayName("Название склада")]
        public string StockName { get; set; }
        public List<StockIngredientViewModel> StockIngredients { get; set; }
    }
}
