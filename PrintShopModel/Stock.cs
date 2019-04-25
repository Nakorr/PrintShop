using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShopModel
{
    /// <summary>
    /// Хранилиище компонентов в магазине
    /// </summary>
    public class Stock
    {
        public int Id { get; set; }
        public string StockName { get; set; }
        public virtual List<StockIngredient> StockIngredients { get; set; }
    }
}