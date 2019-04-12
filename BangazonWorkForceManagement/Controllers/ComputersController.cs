using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using BangazonAPI.Models;
using BangazonWorkForceManagement.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BangazonWorkForceManagement.Controllers
{
    public class ComputersController : Controller
    {
        private readonly IConfiguration _configuration;

        public ComputersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            }
        }

        // GET: Computers
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Id as ComputerId,
                                               c.PurchaseDate, c.DecomissionDate, 
                                               c.Make, c.Manufacturer, e.FirstName, e.LastName
                                          FROM Computer c LEFT JOIN ComputerEmployee ce on ce.ComputerId = c.Id LEFT JOIN Employee e on e.id = ce.EmployeeId ";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Computer> computers = new List<Computer>();

                    while (reader.Read())
                    {
                        Computer computer = new Computer
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("ComputerId")),
                            Make = reader.GetString(reader.GetOrdinal("Make")),
                            Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer")),
                            PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate")),
                            DecomissionDate = reader.IsDBNull(reader.GetOrdinal("DecomissionDate")) ? (DateTime?)null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("DecomissionDate")),
                            Employee = new Employee() {
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName"))
                            }
                        };

                        computers.Add(computer);
                    }

                    reader.Close();
                    return View(computers);
                }
            }
        }

        // GET: Computers/Details/5
        public ActionResult Details(int id)
        {
            Computer computer = GetComputerById(id);
            if (computer == null)
            {
                return NotFound();
            }

            ComputerDetailViewModel viewModel = new ComputerDetailViewModel
            {
                Id = id,
                Make = computer.Make,
                Manufacturer = computer.Manufacturer,
                PurchaseDate = computer.PurchaseDate,
                DecomissionDate = computer.DecomissionDate
            };

            return View(viewModel);
        }

        // GET: Computers/Create
        public ActionResult Create()
        {
            ComputerCreateViewModel viewModel =
                new ComputerCreateViewModel(_configuration.GetConnectionString("DefaultConnection"));
            return View(viewModel);
        }

        // POST: Computers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ComputerCreateViewModel viewModel)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        try
                        {
                            cmd.CommandText = @"insert into Computer (PurchaseDate, DecomissionDate, Make, Manufacturer) OUTPUT Inserted.Id values (@PurchaseDate, @DecomissionDate, @Make, @Manufacturer);";
                            cmd.Parameters.Add(new SqlParameter("@PurchaseDate", viewModel.Computer.PurchaseDate));
                            cmd.Parameters.Add(new SqlParameter("@DecomissionDate", SqlDateTime.Null));
                            cmd.Parameters.Add(new SqlParameter("@Make", viewModel.Computer.Make));
                            cmd.Parameters.Add(new SqlParameter("@Manufacturer", viewModel.Computer.Manufacturer));
                            int insertedID = Convert.ToInt32(cmd.ExecuteScalar());

                            cmd.CommandText = @"insert into ComputerEmployee (ComputerId, EmployeeId, AssignDate) values(@ComputerId, @EmployeeId, @AssignDate)";
                            cmd.Parameters.Add(new SqlParameter("@ComputerId", insertedID));
                            cmd.Parameters.Add(new SqlParameter("@EmployeeId", viewModel.Computer.Employee.Id));
                            cmd.Parameters.Add(new SqlParameter("@AssignDate", DateTime.Now));
                            cmd.ExecuteNonQuery();
                            return RedirectToAction(nameof(Index));
                        } catch
                        {
                            //cmd.CommandText = @"insert into Computer (PurchaseDate, DecomissionDate, Make, Manufacturer) OUTPUT Inserted.Id values (@PurchaseDate, @DecomissionDate, @Make, @Manufacturer);";
                            //cmd.Parameters.Add(new SqlParameter("@PurchaseDate", viewModel.Computer.PurchaseDate));
                            //cmd.Parameters.Add(new SqlParameter("@DecomissionDate", SqlDateTime.Null));
                            //cmd.Parameters.Add(new SqlParameter("@Make", viewModel.Computer.Make));
                            //cmd.Parameters.Add(new SqlParameter("@Manufacturer", viewModel.Computer.Manufacturer));
                            //int insertedID = Convert.ToInt32(cmd.ExecuteScalar());
                            //cmd.ExecuteNonQuery();
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
            }
            catch 
            {
                return View();
            }
        }

        // GET: Computers/Delete/5
        public ActionResult Delete(int id)
        {
            Computer computer = GetComputerById(id);
            if (computer == null)
            {
                return NotFound();
            }

            using (SqlConnection conn = Connection)
            {
                int? assignedcomputer = null;
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Id AS ComputerId,
                                        c.PurchaseDate, c. DecomissionDate,
                                        c.Make, c.Manufacturer, ce.ComputerId as ComputerEmployeeCID
                                        FROM Computer c LEFT JOIN ComputerEmployee ce on c.id = ce.ComputerId
                                        WHERE c.Id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    cmd.ExecuteNonQuery();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())

                    assignedcomputer = reader.IsDBNull(reader.GetOrdinal("ComputerEmployeeCID")) ? (int?)null : (int?)reader.GetInt32(reader.GetOrdinal("ComputerEmployeeCID"));

                    

                            ComputerDeleteViewModel viewModel = new ComputerDeleteViewModel
                            {
                                Id = id,
                                Make = computer.Make,
                                Manufacturer = computer.Manufacturer,
                                PurchaseDate = computer.PurchaseDate,
                                ShouldDisplayDelete = assignedcomputer == null
                    };

                        reader.Close();
                        return View(viewModel);

                       
                }
            }
        }


        // POST: Computers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "DELETE FROM computer WHERE id = @id;";
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        cmd.ExecuteNonQuery();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }


        private Computer GetComputerById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id,
                                               PurchaseDate, DecomissionDate, 
                                               Make, Manufacturer
                                          FROM Computer
                                            WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Computer computer = null;

                    if (reader.Read())
                    {
                        computer = new Computer
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Make = reader.GetString(reader.GetOrdinal("Make")),
                            Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer")),
                            PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate")),
                            DecomissionDate = reader.IsDBNull(reader.GetOrdinal("DecomissionDate")) ? (DateTime?)null : (DateTime?)reader.GetDateTime(reader.GetOrdinal("DecomissionDate"))
                        };
                    }
                    reader.Close();
                    return computer;
                }
            }

        }
    }
}