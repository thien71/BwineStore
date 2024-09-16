using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebsiteBanRuouVang.Models
{
    public class Cart
    {
        [Key]
        [Required]
        public int CartId { get; set; }
        [Required]
        public int Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        public virtual Product Products { get; set; }
    }
}