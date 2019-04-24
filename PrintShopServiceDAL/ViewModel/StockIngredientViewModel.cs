using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;using System.Runtime.Serialization;

namespace PrintShopServiceDAL.ViewModel
{
    public class StockIngredientViewModel
    {
        public int Id { get; set; }
        public int StockId { get; set; }
        public int IngredientId { get; set; }
        [DisplayName("Название ингредиента")]
        public string IngredientName { get; set; }
        [DisplayName("Количество")]
        public int Count { get; set; }
    }
}
