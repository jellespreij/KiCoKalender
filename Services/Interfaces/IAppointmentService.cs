using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAppointmentService
    {
        IEnumerable<Appointment> FindAppointmentByFamilyIdAndUserId(Guid familyId, Guid userId);
        Appointment AddAppointment(Appointment appointment);
        Appointment UpdateAppointment(Appointment appointment, Guid id);
        Appointment DeleteAppointment(Guid id);
    }
}
