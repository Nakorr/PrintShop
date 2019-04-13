using PrintShopModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShopServiceImplement
{
    class DataListSingleton
    {
        private static DataListSingleton instance;
        public List<Customer> Customers { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<Indent> Indents { get; set; }
        public List<Print> Prints { get; set; }
        public List<PrintIngredient> PrintIngredient { get; set; }
        public List<Stock> Stock { get; set; }
        public List<StockIngredient> StockIngredient { get; set; }
        private DataListSingleton()
        {
            Customers = new List<Customer>();
            Ingredients = new List<Ingredient>();
            Indents = new List<Indent>();
            Prints = new List<Print>();
            PrintIngredient = new List<PrintIngredient>();
            Stock = new List<Stock>();
            StockIngredient = new List<StockIngredient>();
            
        }
        public static DataListSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new DataListSingleton();
            }

            return instance;
        }
    }
}
