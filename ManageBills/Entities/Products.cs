using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebAPI.Entities
{
    public class Products
    {
        [Key]
        public int ProductId { set; get; }
        public string? ProductName { set; get; }

    }
}
