using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebAPI.Entities
{
    public class ProductDetailPropertyDetails
    {
        [Key]
        public int ProductDetailPropertyDetailId { set; get; }
        public int ProductDetailId { set; get; }
        public ProductDetails ProductDetails { set; get; }
        public int PropertyDetailId { set; get; }
        public PropertyDetails PropertyDetails { set; get; }
        public int ProductId { set; get; }
        public Products Products { set; get; }
    }
}
