using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebsiteBanRuouVang.Models
{
    public class Brand
    {
        [Key]
        [Required]

        public int BrandId { get; set; }
        [Required]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}