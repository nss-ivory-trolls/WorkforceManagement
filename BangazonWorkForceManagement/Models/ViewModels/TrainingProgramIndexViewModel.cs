using BangazonAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWorkForceManagement.Models.ViewModels
{
    public class TrainingProgramIndexViewModel
    {
        public List<TrainingProgram> TrainingPrograms { get; set; } = new List<TrainingProgram>();

        public bool PastProgram { get; set; } = false;

    }
}
