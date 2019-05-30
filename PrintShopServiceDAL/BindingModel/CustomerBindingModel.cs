using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using PrintShopServiceDAL.ViewModel;

namespace PrintShopServiceDAL.BindingModel
{
    public class CustomerBindingModel
    {
        public int Id { get; set; }
        public string CustomerFIO { get; set; }
        public string Mail { get; set; }
        public List<MessageInfoViewModel> Messages { get; set; }
    }
}
