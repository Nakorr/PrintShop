using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShopModel
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string IngredientName { get; set; }
        public virtual List<PrintIngredient> PrintIngredients { get; set; }
        public virtual List<StockIngredient> StockIngredients { get; set; }
    }
}
