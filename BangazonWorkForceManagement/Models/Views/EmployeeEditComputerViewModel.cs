using BangazonAPI.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWorkForceManagement.Models.Views
{
    public class EmployeeEditComputerViewModel
    {
        public Employee Employee { get; set; }
        public Computer Computer { get; set; }
        public List<Computer> UnassignedComputers { get; set; }
        public List<SelectListItem> ComputerOptions
        {
            get
            {
                if (UnassignedComputers == null)
                {
                    return null;
                }
                return UnassignedComputers.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Make
                }).ToList();
            }
        }
    }
}
