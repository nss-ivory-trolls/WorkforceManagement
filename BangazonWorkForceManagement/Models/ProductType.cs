// Author: Megan Cruzen

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class ProductType
    {
        public int Id { get; set; }

        [Required]
        [StringLength(55, MinimumLength = 2)]
        public string Name { get; set; }
    }
}
