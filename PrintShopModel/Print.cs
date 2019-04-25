using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShopModel
{
    public class Print
    {
        public int Id { get; set; }
        public string PrintName { get; set; }
        public decimal Price { get; set; }
        public virtual List<PrintIngredient> PrintIngredients { get; set; }
        public virtual List<Indent> Indents { get; set; }
    }
}
