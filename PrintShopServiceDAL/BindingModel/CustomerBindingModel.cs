using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace PrintShopServiceDAL.BindingModel
{
    public class CustomerBindingModel
    {
        public int Id { get; set; }
        public string CustomerFIO { get; set; }
        public string Mail { get; set; }
    }
}
