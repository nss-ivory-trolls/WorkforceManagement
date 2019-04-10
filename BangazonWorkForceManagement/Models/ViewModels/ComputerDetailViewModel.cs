using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWorkForceManagement.Models.ViewModels
{
    public class ComputerDetailViewModel
    {
        public int Id { get; set; }

     
        public DateTime PurchaseDate { get; set; }

        
        public DateTime DecommissionDate { get; set; }

       
        public string Make { get; set; }

       
        public string Manufacturer { get; set; }
    }
}
