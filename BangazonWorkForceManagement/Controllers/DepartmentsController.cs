using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BangazonAPI.Models;
using BangazonWorkForceManagement.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BangazonWorkForceManagement.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IConfiguration _configuration;

        public DepartmentsController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            }
        }


        // GET: Departments
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT id, [name], budget FROM Department";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Department> departments = new List<Department>();

                    while (reader.Read())
                    {
                        Department department = new Department
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            Budget = reader.GetInt32(reader.GetOrdinal("budget")),
                            Employees = GetDeptEmployee(reader.GetInt32(reader.GetOrdinal("id")))
                        };
                        departments.Add(department);

                    }
                        reader.Close();
                        return View(departments);
                }
            }
        }

        // GET: Departments/Details/5
        public ActionResult Details(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $@"SELECT id, [name], budget FROM Department WHERE id = {id}";
                    SqlDataReader reader = cmd.ExecuteReader();

                    Department department = null;

                    if (reader.Read())
                    {
                        department = new Department
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            Budget = reader.GetInt32(reader.GetOrdinal("budget")),
                            
                            
                        };
                        
                    }
                    reader.Close();
                    return View(department);
                }
            }
        }

        // GET: Departments/Create
        public ActionResult Create()
        {
            DepartmentCreateViewModel viewModel = new DepartmentCreateViewModel();
            return View(viewModel);
        }

        // POST: Departments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DepartmentCreateViewModel viewModel)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO Department ([name], budget)
                                            VALUES (@name, @budget)";
                        cmd.Parameters.Add(new SqlParameter("@name", viewModel.Department.Name));
                        cmd.Parameters.Add(new SqlParameter("@budget", viewModel.Department.Budget));

                        cmd.ExecuteReader();

                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Departments/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Departments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Departments/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public List<Employee> GetDeptEmployee(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $@"select id, firstName, LastName, departmentId FROM employee WHERE departmentid = {id}";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Employee> employees = new List<Employee>();

                    while (reader.Read())
                    {
                        employees.Add(new Employee
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            FirstName = reader.GetString(reader.GetOrdinal("firstName")),
                            LastName = reader.GetString(reader.GetOrdinal("lastName")),
                            DepartmentId = reader.GetInt32(reader.GetOrdinal("departmentId"))
                        });
                    }
                    reader.Close();
                    return employees;


                }
            }
        }
    }
}