using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace PrintShopServiceDAL.BindingModel
{
    public class IndentBindingModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int PrintId { get; set; }
        public int Count { get; set; }
        public decimal Sum { get; set; }
    }
}
