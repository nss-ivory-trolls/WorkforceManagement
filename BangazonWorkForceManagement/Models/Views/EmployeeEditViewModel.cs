﻿using BangazonAPI.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWorkForceManagement.Models.Views
{
    public class EmployeeEditViewModel
    {

     
        public Employee Employee { get; set; }
        public List<Department> Departments { get; set; }
        public List<Computer> UnassignedComputers { get; set; }
        public Computer Computer { get; set; }
        [Display(Name = "Computer")]
        public string NewComputerId { get; set; }

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
