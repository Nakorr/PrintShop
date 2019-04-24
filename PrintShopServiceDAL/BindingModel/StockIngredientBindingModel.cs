using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace PrintShopServiceDAL.BindingModel
{
    public class StockIngredientBindingModel
    {
        public int Id { get; set; }
        public int StockId { get; set; }
        public int IngredientId { get; set; }
        public int Count { get; set; }
    }

}
