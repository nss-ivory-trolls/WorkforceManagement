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
    public class TrainingProgramsController : Controller
    {
        private readonly IConfiguration _configuration;

        public TrainingProgramsController(IConfiguration configuration)
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

        // GET: TrainingPrograms
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT * 
                                        FROM TrainingProgram 
                                        WHERE StartDate >= getdate();";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<TrainingProgram> trainingPrograms = new List<TrainingProgram>();

                    while (reader.Read())
                    {
                        TrainingProgram trainingProgram = new TrainingProgram
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            StartDate = reader.GetDateTime(reader.GetOrdinal("startDate")),
                            EndDate = reader.GetDateTime(reader.GetOrdinal("endDate")),
                            MaxAttendees = reader.GetInt32(reader.GetOrdinal("maxAttendees"))
                        };
                        trainingPrograms.Add(trainingProgram);
                    }
                    reader.Close();
                    return View(trainingPrograms);
                }
            }
        }

        // GET: TrainingPrograms/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TrainingPrograms/Create
        public ActionResult Create()
        {
            TrainingProgramCreateViewModel viewModel = new TrainingProgramCreateViewModel();
            return View(viewModel);
        }

        // POST: TrainingPrograms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TrainingProgramCreateViewModel viewModel)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO TrainingProgram (name, StartDate, EndDate, MaxAttendees)
                                            VALUES (@name, @startDate, @endDate, @max);";
                        cmd.Parameters.Add(new SqlParameter("@name", viewModel.TrainingProgram.Name));
                        cmd.Parameters.Add(new SqlParameter("@startDate", viewModel.TrainingProgram.StartDate));
                        cmd.Parameters.Add(new SqlParameter("@endDate", viewModel.TrainingProgram.EndDate));
                        cmd.Parameters.Add(new SqlParameter("@max", viewModel.TrainingProgram.MaxAttendees));

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

        // GET: TrainingPrograms/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TrainingPrograms/Edit/5
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

        // GET: TrainingPrograms/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TrainingPrograms/Delete/5
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