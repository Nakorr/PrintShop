using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShopServiceDAL.ViewModel
{
   public class MessageInfoViewModel
    {
        public string MessageId { get; set; }
        public string CustomerName { get; set; }
        public DateTime DateDelivery { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
