using BangazonAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWorkForceManagement.Models.ViewModels
{
    public class TrainingProgramIndexViewModel
    {
        public TrainingProgram TrainingProgram { get; set; }

        public Boolean UpcomingEvents
        {
            get {
                if (TrainingProgram.StartDate < DateTime.Today)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }

        }
    }
}
