using System.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShopServiceDAL.ViewModel
{
    public class StocksLoadViewModel
    {
        public string StockName { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<Tuple<string, int>> Ingredients { get; set; }
    }
}
