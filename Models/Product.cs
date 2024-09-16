using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebsiteBanRuouVang.Models
{
    public class Product
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int Price { get; set; }

        public string Detail { get; set; }

        public string Image { get; set; }
        public int DepId { get; set; }
        public int BrandId { get; set; }
        public virtual Department Department { get; set; }

        public virtual Brand Brand { get; set; }
    }
}