using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebAPI.Entities
{
    public class ProductDetails
    {
        [Key]
        public int ProductDetailId { set; get; }
        public string? ProductPropertyName { set; get; }
        public int? Quantity { set; get; }
        public double? Price { set; get; }
        public double? ShellPrice { set; get; }
        public int? ParentId { set; get; }
    }
}
