using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
namespace PrintShopServiceDAL.ViewModel
{
    public class IndentViewModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerFIO { get; set; }
        public int PrintId { get; set; }
        public string PrintName { get; set; }
        public int Count { get; set; }
        public decimal Sum { get; set; }
        public string Status { get; set; }
        public string DateCreate { get; set; }
        public string DateImplement { get; set; }
    }
}
