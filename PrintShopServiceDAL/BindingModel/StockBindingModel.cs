using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace PrintShopServiceDAL.BindingModel
{
    public class StockBindingModel
    {
        public int Id { get; set; }
        public string StockName { get; set; }
    }
}
