using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShopModel
{
    public class PrintIngredient
    {
        public int Id { get; set; }
        public int PrintId { get; set; }
        public int IngredientId { get; set; }
        public int Count { get; set; }
        public virtual Print Print { get; set; }
        public virtual Ingredient Ingredient { get; set; }
    }
}
