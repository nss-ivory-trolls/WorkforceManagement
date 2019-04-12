using BangazonAPI.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWorkForceManagement.Models.ViewModels
{
    public class ComputerCreateViewModel
    {
        /*public string Make { get; set; }

        public string Manufacturer { get; set; }

        public DateTime PurchaseDate { get; set; }*/

        public ComputerCreateViewModel()
        {
            Employees = new List<Employee>();
        }

        public ComputerCreateViewModel(string connectionString)
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT id, firstname, lastname from Employee;";
                    SqlDataReader reader = cmd.ExecuteReader();

                    Employees = new List<Employee>();

                    while (reader.Read())
                    {
                        Employees.Add(new Employee
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FirstName = reader.GetString(reader.GetOrdinal("firstname")),
                            LastName = reader.GetString(reader.GetOrdinal("lastname"))
                        });
                    }
                    reader.Close();
                }
            }
        }


        public Computer Computer { get; set; }
        public List<Employee> Employees { get; set; }

       
        public List<SelectListItem> EmployeesOptions 
        {
        get
            {

                return Employees.Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.FullName
                }).ToList();
                

            }
        }
        
        
    }
}
