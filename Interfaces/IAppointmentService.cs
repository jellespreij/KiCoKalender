using KiCoKalender.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiCoKalender.Interfaces
{
    interface IAppointmentService
    {
        Task<List<Appointment>> FindByUserId(long? userId);
        void AddAppointment(Appointment appointment);
        void DeleteAppointment(Appointment appointment);
        void UpdateAppointment(Appointment appointment);
    }
}
