using System;
using System.Collections.Generic;
using System.Text;

namespace PrintShopModel
{
    public class Customer
    {
        public int Id { get; set; }
        public string CustomerFIO { get; set; }
        public virtual List<Indent> Indents { get; set; }
    }
}
