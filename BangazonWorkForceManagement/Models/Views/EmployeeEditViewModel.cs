using BangazonAPI.Models;
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
                List<SelectListItem> UC = UnassignedComputers.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Make
                }).ToList();
               
                    UC.Insert(0, new SelectListItem { Value = "0", Text = "Do Not Assign Computer", Selected = true });
                    return UC;
                
              
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

        public List<TrainingProgram> AttendingTP { get; set; } = new List<TrainingProgram>();
        public List<TrainingProgram> NotAttendingTP { get; set; } = new List<TrainingProgram>();
        public List<string> NowAttendingTP { get; set; } = new List<string>();
        public List<string> NowNotAttendingTP { get; set; } = new List<string>();

        public List<SelectListItem> AttendingTPOptions
        {
            get
            {
                return AttendingTP.Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name
                }).ToList();
            }
        }

        public List<SelectListItem> NotAttendingTPOptions
        {
            get
            {
                return NotAttendingTP.Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name
                }).ToList();
            }
        }
    }
}
    

