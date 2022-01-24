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
        Appointment FindAppointmentByAppointmentId(Guid appointmentId);
        IEnumerable<Appointment> FindAppointmentByFamilyIdAndUserId(Guid familyId, Guid userId);
        IEnumerable<AppointmentDTO> FindAppointmentDTOByFamilyIdAndUserId(Guid familyId, Guid userId);
        Appointment AddAppointment(Appointment appointment);
        Appointment UpdateAppointment(AppointmentUpdateDTO appointmentUpdate, Guid id);
        Appointment DeleteAppointment(Guid id);
    }
}
