using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace BangazonAPI.Models
{

    public class Department
    {
        public int Id { get; set; }

        [Required]
        [StringLength(55, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        public int Budget { get; set; } 


        public string Currency
        {
            get { return Budget.ToString("C", CultureInfo.CurrentCulture); }
        }
        
        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
}
