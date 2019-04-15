using BangazonAPI.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWorkForceManagement.Models.Views
{
    public class EmployeeCreateViewModel
    {
        public EmployeeCreateViewModel()
        {
            Departments = new List<Department>();
        }

        public EmployeeCreateViewModel(string connectionString)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT id, name FROM Department";
                    SqlDataReader reader = cmd.ExecuteReader();

                    Departments = new List<Department>();

                    while (reader.Read())
                    {
                        Departments.Add(new Department
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Name = reader.GetString(reader.GetOrdinal("name"))
                        });
                    }
                    reader.Close();
                }
            }
        }
        
        public List<Department> Departments { get; set; }
        public Employee Employee { get; set; }

        public List<SelectListItem> DepartmentOptions
        {
            get
            {
                return Departments.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                }).ToList();
            }
        }
    }
}
