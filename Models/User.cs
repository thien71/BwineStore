using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebsiteBanRuouVang.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }
        [Required]
        public string password { get; set; }

        public string Email { get; set; }
        public string role { get; set; }
    }
}