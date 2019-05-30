using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrintShopModel
{
    public class Customer
    {
        public int Id { get; set; }
        public string CustomerFIO { get; set; }
        [ForeignKey("CustomerId")]
        public virtual List<Indent> Indents { get; set; }
        public string Mail { get; set; }
        [ForeignKey("CustomerId")]
        public virtual List<MessageInfo> MessageInfos { get; set; }
    }
}
