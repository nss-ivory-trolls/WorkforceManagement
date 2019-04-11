using BangazonAPI.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWorkForceManagement.Models.Views
{
    public class EmployeeEditViewModel
    {
        public Employee Employee { get; set; }
        public List<Department> Departments { get; set; }
        //public List<TrainingProgram> TrainingPrograms { get; set; }
        public List<SelectListItem> DepartmentOptions
        {
            get
            {
                if (Departments == null)
                {
                    return null;
                }
                return Departments.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();
            }
        }

    }
}
