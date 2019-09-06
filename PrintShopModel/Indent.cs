using System;
using System.Collections.Generic;
using System.Text;

namespace PrintShopModel
{
    public class Indent
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int PrintId { get; set; }
        public int? ImplementerId { get; set; }
        public int Count { get; set; }
        public decimal Sum { get; set; }
        public IndentStatus Status { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateImplement { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Print Print { get; set; }
        public virtual Implementer Implementer { get; set; }

    }
}
