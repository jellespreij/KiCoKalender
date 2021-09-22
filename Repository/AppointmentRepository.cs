using KiCoKalender.Controllers;
using KiCoKalender.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiCoKalender.Repository
{
    class AppointmentRepository
    {
        ILogger Logger { get; }

        public AppointmentRepository(ILogger<AppointmentHttpTrigger> Logger)
        {
            this.Logger = Logger;
        }

        public async Task<List<Appointment>> FindByUserId(long? userId)
        {
            List<Appointment> appointments = new()
            {
                new Appointment() { appointmentId = 1, appointmentName = "name", AppointmentDescription = "description", AppointmentDate = DateTime.Now }
            };

            Logger.LogInformation("Found assets by id: ", userId);

            return appointments;
        }

        public void AddAppointment(Appointment appointment)
        {
            Logger.LogInformation("Added appointment");
        }

        public void DeleteAppointment(Appointment appointment)
        {
            Logger.LogInformation("Deleted appointment");
        }

        public void UpdateAppointment(Appointment appointment)
        {
            Logger.LogInformation("Updated appointment");
        }
    }
}
