using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IAppointmentService
    {
        IEnumerable<Appointment> FindAppointmentByUserId(Guid userId);
        IEnumerable<Appointment> FindAppointmentByFamilyId(Guid familyId);
        void AddAppointment(Appointment appointment);
        void UpdateAppointment(Appointment appointment);
        void DeleteAppointment(Appointment appointment);
    }
}
