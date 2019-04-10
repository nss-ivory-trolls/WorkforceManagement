// Author: Megan Cruzen

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonAPI.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int PaymentTypeId { get; set; }

        public List<Product> ProductList { get; set; } = new List<Product>();
        public List<Customer> Customer { get; set; } = new List<Customer>();

        //public Customer Customer { get; set; }
    }
}
