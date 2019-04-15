using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BangazonAPI.Models;
using BangazonWorkForceManagement.Models.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BangazonWorkForceManagement.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IConfiguration _config;

        public EmployeesController(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        // GET: Employees
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT e.FirstName AS EmployeeFirstName,
                                               e.Id AS EmployeeId,
                                               e.IsSupervisor AS IsSupervisor,
	                                           e.LastName AS EmployeeLastName,
                                               d.Budget AS DepartmentBudget,
	                                           d.Name AS DepartmentName,
                                               d.Id as DepartmentId
                                        FROM Employee e
                                        JOIN Department AS d on d.Id = e.DepartmentId";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Employee> employees = new List<Employee>();
                    while (reader.Read())
                    {
                        Employee employee = new Employee
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                            FirstName = reader.GetString(reader.GetOrdinal("EmployeeFirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("EmployeeLastName")),
                            IsSuperVisor = reader.GetBoolean(reader.GetOrdinal("IsSupervisor")),
                            DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                            Department = new Department
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                                Name = reader.GetString(reader.GetOrdinal("DepartmentName")),
                                Budget = reader.GetInt32(reader.GetOrdinal("DepartmentBudget"))
                            }
                        };

                        employees.Add(employee);
                    }
                    reader.Close();
                    return View(employees);
                }
            }
        }

        // GET: Employees/Details/5
        public ActionResult Details(int id)
        {

            Employee employee2 = GetEmployeeById(id);
            using (SqlConnection conn = Connection)

            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT 
                                        e.FirstName AS EmployeeFirstName,
                                        e.Id AS EmployeeId,
                                        e.IsSupervisor AS IsSupervisor,
                                        e.LastName AS EmployeeLastName,
                                        d.Budget AS DepartmentBudget,
                                        d.Name AS DepartmentName,
                                        d.Id as DepartmentId,
                                        c.Make as ComputerMake,
                                        c.Id as ComputerId,
                                        c.PurchaseDate as ComputerPurchaseDate,
                                        c.DecomissionDate as ComputerDecomissionDate,
                                        c.Manufacturer as ComputerManufacturer,
                                        tp.Id as TrainingProgramId,
                                        tp.Name as TrainingProgramName,
                                        tp.StartDate as TrainingProgramStartDate,
                                        tp.EndDate as TrainingProgramEndDate,
                                        tp.MaxAttendees as TrainingProgramMaxAtendees
                                        FROM Employee e
                                        JOIN Department AS d on d.Id = e.DepartmentId
                                        LEFT JOIN ComputerEmployee AS ce on ce.EmployeeId = e.Id
                                        LEFT JOIN Computer AS c on c.Id = ce.ComputerId AND ce.UnAssignDate IS NULL 
                                        LEFT JOIN EmployeeTraining AS et on et.EmployeeId = e.Id
                                        LEFT JOIN TrainingProgram AS tp on tp.Id = et.TrainingProgramId
                                        WHERE e.Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@Id", id));
                    SqlDataReader reader = cmd.ExecuteReader();
                    EmployeeDetailViewModel employee = null;
                    while (reader.Read())
                    {
                        if (employee == null)
                        {
                            employee = new EmployeeDetailViewModel
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                                FirstName = reader.GetString(reader.GetOrdinal("EmployeeFirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("EmployeeLastName")),
                                IsSuperVisor = reader.GetBoolean(reader.GetOrdinal("IsSupervisor")),
                                DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                                Department = new Department
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                                    Name = reader.GetString(reader.GetOrdinal("DepartmentName")),
                                    Budget = reader.GetInt32(reader.GetOrdinal("DepartmentBudget"))
                                },
                                Computer = new Computer()
                            };
                        }
                        if (!reader.IsDBNull(reader.GetOrdinal("ComputerId")) && !reader.IsDBNull(reader.GetOrdinal("ComputerMake")) && !reader.IsDBNull(reader.GetOrdinal("ComputerPurchaseDate")) && !reader.IsDBNull(reader.GetOrdinal("ComputerManufacturer")) && !reader.IsDBNull(reader.GetOrdinal("ComputerDecomissionDate")))
                        {
                            employee.Computer = new Computer
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ComputerId")),
                                PurchaseDate = reader.GetDateTime(reader.GetOrdinal("ComputerPurchaseDate")),
                                DecommissionDate = reader.GetDateTime(reader.GetOrdinal("ComputerDecomissionDate")),
                                Make = reader.GetString(reader.GetOrdinal("ComputerMake")),
                                Manufacturer = reader.GetString(reader.GetOrdinal("ComputerManufacturer"))
                            };
                        };
                            if (!reader.IsDBNull(reader.GetOrdinal("TrainingProgramId")))
                            {

                                if (!employee.TrainingProgramList.Exists(x => x.Id == reader.GetInt32(reader.GetOrdinal("TrainingProgramId"))))
                                {
                                    employee.TrainingProgramList.Add(
                                new TrainingProgram
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("TrainingProgramId")),
                                    Name = reader.GetString(reader.GetOrdinal("TrainingProgramName")),
                                    StartDate = reader.GetDateTime(reader.GetOrdinal("TrainingProgramStartDate")),
                                    EndDate = reader.GetDateTime(reader.GetOrdinal("TrainingProgramEndDate")),
                                    MaxAttendees = reader.GetInt32(reader.GetOrdinal("TrainingProgramMaxAtendees"))
                                });
                                }
                            } 
                        }                    
                    reader.Close();
                    return View(employee);
                }
            }
        }


        // GET: Employees/Create
        public ActionResult Create()
        {
            {
                EmployeeCreateViewModel viewModel =
                    new EmployeeCreateViewModel(_config.GetConnectionString("DefaultConnection"));
                return View(viewModel);
            }
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeCreateViewModel viewModel)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO Employee(FirstName, LastName, DepartmentId, IsSuperVisor)
                                             VALUES (@FirstName, @LastName, @DepartmentId, @IsSupervisor)";

                        cmd.Parameters.Add(new SqlParameter("@FirstName", viewModel.Employee.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@LastName", viewModel.Employee.LastName));
                        cmd.Parameters.Add(new SqlParameter("@DepartmentId", viewModel.Employee.DepartmentId));
                        cmd.Parameters.Add(new SqlParameter("@IsSupervisor", viewModel.Employee.IsSuperVisor));

                        cmd.ExecuteNonQuery();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int id)
        {
            Employee employee = GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }

            EmployeeEditViewModel viewModel = new EmployeeEditViewModel
            {             
                Departments = GetAllDepartments(),
                Employee = GetEmployeeById(id),
                UnassignedComputers = GetAllUnAssignedComputers(id),
                AttendingTP = GetAttendingTP(id),
                NotAttendingTP = GetNotAttendingTP(id),
                NowAttendingTP = null,
                NowNotAttendingTP = null
            };

            return View(viewModel);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EmployeeEditViewModel ViewModel)
        {
             Employee Employee = GetEmployeeById(id);
            using (SqlConnection conn = Connection)
            {
                    conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (Employee.Computer == null)
                    {
                        cmd.CommandText = @"UPDATE Employee
                                           SET LastName = @lastName,
                                               FirstName = @firstname,
                                               DepartmentId = @DepartmentId
                                            WHERE id = @id;

                                           INSERT INTO ComputerEmployee(EmployeeId, ComputerId, AssignDate)
                                           OUTPUT Inserted.Id
                                           VALUES(@EmployeeId, @ComputerId, GETDATE());
                                           SELECT MAX(Id)
                                           FROM ComputerEmployee;";
                        cmd.Parameters.Add(new SqlParameter("@EmployeeId", Employee.Id));
                        cmd.Parameters.Add(new SqlParameter("@ComputerId", int.Parse(ViewModel.NewComputerId)));
                        cmd.Parameters.Add(new SqlParameter("@LastName", ViewModel.Employee.LastName));
                        cmd.Parameters.Add(new SqlParameter("@FirstName", ViewModel.Employee.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@DepartmentId", ViewModel.Employee.DepartmentId));
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        cmd.ExecuteNonQuery();

                        UpdateEmployeeTrainingPrograms(id, ViewModel);
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {

                        int NewComputerId = int.Parse(ViewModel.NewComputerId);
                        int OldCompId = Employee.Computer.Id;

                        cmd.CommandText = @"UPDATE Employee
                                           SET LastName = @lastName,
                                               FirstName = @firstname,
                                               DepartmentId = @DepartmentId
                                            WHERE id = @id;";
                        cmd.Parameters.Add(new SqlParameter("@LastName", ViewModel.Employee.LastName));
                        cmd.Parameters.Add(new SqlParameter("@FirstName", ViewModel.Employee.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@DepartmentId", ViewModel.Employee.DepartmentId));
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        cmd.ExecuteNonQuery();

                        if (NewComputerId != 0 && NewComputerId != OldCompId)
                        {
                            UpdateEmployeeComputer(id, NewComputerId, OldCompId);
                        }

                        UpdateEmployeeTrainingPrograms(id, ViewModel);
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
        }
          
        private void UpdateEmployeeTrainingPrograms(int id, EmployeeEditViewModel ViewModel)
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    if (ViewModel.NowAttendingTP !=null)
                    {
                        foreach (var item in ViewModel.NowAttendingTP)
                        {                         
                            cmd.CommandText = $@"DELETE FROM EmployeeTraining
                                                 WHERE TrainingProgramId = @id
                                                 AND EmployeeId = @item";
                            cmd.Parameters.Add(new SqlParameter("@id", id));
                            cmd.Parameters.Add(new SqlParameter("@item", int.Parse(item)));
                            cmd.ExecuteNonQuery();
                        }
                    }

                    if (ViewModel.NowNotAttendingTP != null) {
                        foreach (var item in ViewModel.NowNotAttendingTP)
                        {                           
                            cmd.CommandText = $@"INSERT INTO EmployeeTraining (EmployeeId, TrainingProgramId)
                                                OUTPUT INSERTED.Id
                                                VALUES (@id2, @item2);
                                                SELECT MAX(Id)
                                                FROM EmployeeTraining;";
                            cmd.Parameters.Add(new SqlParameter("@id2", id));
                            cmd.Parameters.Add(new SqlParameter("@item2", int.Parse(item)));
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private List<TrainingProgram> GetAttendingTP (int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $@"SELECT tp.Id AS TPId, 
                                                tp.Name AS TPName
                                         FROM EmployeeTraining et
                                         LEFT JOIN TrainingProgram tp ON tp.Id = et.TrainingProgramId
                                         WHERE et.EmployeeId = @id AND StartDate >= GETDATE()";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<TrainingProgram> TrainingPrograms = new List<TrainingProgram>();

                    while (reader.Read())
                    {
                        TrainingPrograms.Add(new TrainingProgram
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("TPId")),
                            Name = reader.GetString(reader.GetOrdinal("TPName"))
                        });
                    }
                    reader.Close();
                    return TrainingPrograms;
                }
            }
        } 

        private List<TrainingProgram> GetNotAttendingTP (int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $@"SELECT AttendeeCount, 
                                        TrainingProgramId AS TPId, 
                                        tp.Name AS TPName,
                                        tp.MaxAttendees
                                        FROM(
                                        SELECT Count(EmployeeId) AS AttendeeCount, TrainingProgramId
                                        FROM EmployeeTraining
                                        GROUP BY TrainingProgramId
                                        ) AS Counts
                                        LEFT JOIN TrainingProgram tp on tp.Id= Counts.TrainingProgramId
                                        WHERE Counts.AttendeeCount < tp.MaxAttendees 
                                        AND tp.Name NOT IN (
                                        SELECT tp.Name 
                                        FROM EmployeeTraining et
                                        LEFT JOIN TrainingProgram tp ON tp.Id = et.TrainingProgramId
                                        WHERE et.EmployeeId = @id
                                        AND StartDate >= GETDATE())";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<TrainingProgram> TrainingPrograms = new List<TrainingProgram>();

                    while (reader.Read())
                    {
                        TrainingPrograms.Add(new TrainingProgram
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("TPId")),
                            Name = reader.GetString(reader.GetOrdinal("TPName"))
                        });
                    }
                        reader.Close();
                        return TrainingPrograms;
                }
            }
        }

        private int AddEmployeeComputer (int EmployeeId, int NewComputerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    int ComputerId = 0;
                    cmd.CommandText = @"INSERT INTO ComputerEmployee(EmployeeId, ComputerId, AssignDate)
                                        OUTPUT Inserted.Id
                                        VALUES (@EmployeeId, @ComputerId, GETDATE());
                                        SELECT MAX(Id)
                                        FROM ComputerEmployee;";
                    cmd.Parameters.Add(new SqlParameter("@EmployeeId", EmployeeId));
                    cmd.Parameters.Add(new SqlParameter("@ComputerId", NewComputerId));

                    ComputerId = (Int32)cmd.ExecuteScalar();
                    return ComputerId; 
                }
            }
        }
        

        private void UpdateEmployeeComputer(int EmployeeId, int NewComputerId, int OldCompId)
        {          
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO ComputerEmployee(EmployeeId, ComputerId, AssignDate)
                                        OUTPUT Inserted.Id
                                        VALUES (@EmployeeId, @ComputerId, GETDATE());
                                        SELECT MAX(Id)
                                        FROM ComputerEmployee;";
                    cmd.Parameters.Add(new SqlParameter("@EmployeeId", EmployeeId));
                    cmd.Parameters.Add(new SqlParameter("@ComputerId", NewComputerId));
                    

                    cmd.ExecuteNonQuery();

                    cmd.CommandText = @"UPDATE ComputerEmployee
                                        SET EmployeeId = @EmployeeId,
                                            ComputerId = @OldCompId,
                                            UnassignDate = GETDATE()
                                        WHERE EmployeeId = @EmployeeId AND ComputerId= @OldCompId;";

                    cmd.Parameters.Add(new SqlParameter("@OldCompId", OldCompId));
                    cmd.ExecuteNonQuery();

                }
            }
        }

        private Employee GetEmployeeById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT e.FirstName AS EmployeeFirstName,
                                               e.Id AS EmployeeId,                                             
	                                           e.LastName AS EmployeeLastName,
                                               e.IsSupervisor AS IsSupervisor,
	                                           d.Name AS DepartmentName,
                                               d.Id as DepartmentId,
											   c.Make as ComputerMake,
                                               c.Id as ComputerId,
											   c.PurchaseDate as PurchaseDate,
											   c.DecomissionDate as DecomissionDate,
											   c.Manufacturer as Manufacturer
                                        FROM Employee e
                                        JOIN Department d on d.Id = e.DepartmentId
										LEFT JOIN ComputerEmployee AS ce on ce.EmployeeId = e.Id
                                        AND ce.UnAssignDate IS NULL
										LEFT JOIN Computer AS c on c.Id = ce.ComputerId								
										WHERE e.Id= @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Employee employee = null;

                    if (reader.Read())
                    {

                        employee = new Employee
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                            FirstName = reader.GetString(reader.GetOrdinal("EmployeeFirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("EmployeeLastName")),
                            IsSuperVisor = reader.GetBoolean(reader.GetOrdinal("IsSupervisor")),
                            DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId"))
                        };
                    if (!reader.IsDBNull(reader.GetOrdinal("ComputerId")) && !reader.IsDBNull(reader.GetOrdinal("ComputerMake")) && !reader.IsDBNull(reader.GetOrdinal("PurchaseDate")) && !reader.IsDBNull(reader.GetOrdinal("Manufacturer")) && !reader.IsDBNull(reader.GetOrdinal("DecomissionDate")))
                        {

                            employee.Computer = new Computer
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ComputerId")),
                                PurchaseDate = reader.GetDateTime(reader.GetOrdinal("PurchaseDate")),
                                DecommissionDate = reader.GetDateTime(reader.GetOrdinal("DecomissionDate")),
                                Make = reader.GetString(reader.GetOrdinal("ComputerMake")),
                                Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer"))
                            };

                            }                 
                        }
                        reader.Close();
                        return employee;
                    }
                }
            }

        


        private List<Department> GetAllDepartments()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, 
                                               Name 
                                        FROM Department;";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Department> departments = new List<Department>();

                    while (reader.Read())
                    {
                        departments.Add(new Department
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        });
                    }
                    reader.Close();

                    return departments;
                }
            }
        }

        private List<TrainingProgram> GetAllTrainingPrograms()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, 
                                               Name, 
                                               StartDate, 
                                               EndDate, 
                                               MaxAttendees 
                                       FROM TrainingProgram;";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<TrainingProgram> trainingPrograms = new List<TrainingProgram>();

                    while (reader.Read())
                    {
                        trainingPrograms.Add(new TrainingProgram
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                            EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate"))
                        });
                    }
                    reader.Close();

                    return trainingPrograms;
                }
            }
        }

        private List<Computer> GetAllUnAssignedComputers(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    List<Computer> UnAssignedComputers = new List<Computer>();

                    cmd.CommandText = @"SELECT com.Id, com.Make, com.Manufacturer
                                        FROM Computer com
                                        LEFT JOIN (SELECT c.id, count(*) AS CountNulls
			                            FROM Computer c
		                                LEFT JOIN ComputerEmployee ce ON c.Id = ce.ComputerId
			                            WHERE UnassignDate IS NULL
		                                GROUP BY c.Id) cc ON com.Id = cc.Id
                                        WHERE cc.CountNulls IS NULL;";
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        UnAssignedComputers.Add(new Computer
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Make = reader.GetString(reader.GetOrdinal("Make")),
                            Manufacturer = reader.GetString(reader.GetOrdinal("Manufacturer"))
                        });
                    }

                    reader.Close();
                    return UnAssignedComputers;
                }
            }
        }
    }
}


