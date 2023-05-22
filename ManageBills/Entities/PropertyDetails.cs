using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebAPI.Entities
{
    public class PropertyDetails
    {
        [Key]
        public int PropertyDetailId { set; get; }
        public int PropertyId { set; get; }
        public Properties Properties { set; get; }
        public string? PropertyDetailCode { set; get; }
        public string? PropertyDetailDetail { set; get; }
        
    }
}
