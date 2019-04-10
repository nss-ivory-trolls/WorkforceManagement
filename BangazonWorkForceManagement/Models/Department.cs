using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BangazonAPI.Models
{

    public class Department
    {


        public int Id { get; set; }

        [Required]
        [StringLength(55, MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        public int Budget { get; set; }

        public List<Employee> Employees { get; set; }
    }
}
