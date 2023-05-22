using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebAPI.Entities
{
    public class Properties
    {
        [Key]
        public int PropertyId { set; get; }
        public int ProductId { set; get; }
        public Products Products { set; get; }
        public string? PropertyName { set; get; }
        public int? PropertySort { set; get; }
    }
}
