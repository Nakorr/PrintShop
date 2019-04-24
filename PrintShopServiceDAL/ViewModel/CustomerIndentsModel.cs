using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShopServiceDAL.ViewModel
{
   public class CustomerIndentsModel
    {
        public string CustomerName { get; set; }
        public string DateCreate { get; set; }
        public string PrintName { get; set; }
        public int Count { get; set; }
        public decimal Sum { get; set; }
        public string Status { get; set; }
    }
}
