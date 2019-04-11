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
                    cmd.CommandText = @"SELECT d.id, d.[name], d.budget, e.id AS employeeId, e.firstname, e.lastname, e.issupervisor, e.departmentId
                                        FROM Department d 
                                        LEFT JOIN Employee e
                                        ON d.id = e.departmentid";
                    SqlDataReader reader = cmd.ExecuteReader();

                    Dictionary<int, Department> departments = new Dictionary<int, Department>();
                    Dictionary<int, Employee> employeesSort = new Dictionary<int, Employee>();
                    while (reader.Read())
                    {
                        int deptId = reader.GetInt32(reader.GetOrdinal("id"));
                        if (!departments.ContainsKey(deptId))
                        {
                            Department newDepartment = new Department
                            {
                                Id = deptId,
                                Name = reader.GetString(reader.GetOrdinal("name")),
                                Budget = reader.GetInt32(reader.GetOrdinal("budget"))
                            };
                            departments.Add(deptId, newDepartment);
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("employeeId")))
                        {
                            int employeeId = reader.GetInt32(reader.GetOrdinal("employeeId"));
                            if (!employeesSort.ContainsKey(employeeId))
                            {
                                Employee newEmployee = new Employee
                                {
                                    Id = employeeId,
                                    FirstName = reader.GetString(reader.GetOrdinal("firstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("lastName")),
                                    IsSuperVisor = false,
                                    DepartmentId = reader.GetInt32(reader.GetOrdinal("departmentId"))
                                };
                                employeesSort.Add(employeeId, newEmployee);

                                Department currentDepartment = departments[deptId];
                                currentDepartment.Employees.Add(
                                    new Employee
                                    {
                                        Id = employeeId,
                                        FirstName = reader.GetString(reader.GetOrdinal("firstName")),
                                        LastName = reader.GetString(reader.GetOrdinal("lastName")),
                                        IsSuperVisor = reader.GetBoolean(reader.GetOrdinal("issupervisor")),
                                        DepartmentId = reader.GetInt32(reader.GetOrdinal("departmentId"))
                                    }
                                    );
                            }
                        }
                    }
                    reader.Close();
                    return View(departments.Values.ToList());
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
                    cmd.CommandText = $@"SELECT d.id, d.[name], d.budget, e.id AS employeeId, e.firstname, e.lastname, e.issupervisor, e.departmentId 
                                         FROM Department d 
                                         LEFT JOIN Employee e
                                         ON d.id = e.departmentid
                                         WHERE d.id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Department department = null;

                    while (reader.Read())
                    {
                        if (department == null)
                        {
                            department = new Department
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Name = reader.GetString(reader.GetOrdinal("name")),
                                Budget = reader.GetInt32(reader.GetOrdinal("budget"))
                            };
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("employeeId")))
                        {
                            int employeeId = reader.GetInt32(reader.GetOrdinal("employeeId"));
                            if (!department.Employees.Any(e => e.Id == employeeId))
                            {
                                Employee employee = new Employee
                                {
                                    Id = employeeId,
                                    FirstName = reader.GetString(reader.GetOrdinal("firstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("lastName")),
                                    IsSuperVisor = reader.GetBoolean(reader.GetOrdinal("issupervisor")),
                                    DepartmentId = reader.GetInt32(reader.GetOrdinal("departmentId"))
                                };
                                department.Employees.Add(employee);
                            }
                        }
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
    }
}