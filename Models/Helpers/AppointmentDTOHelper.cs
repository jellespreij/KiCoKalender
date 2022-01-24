using System.Collections.Generic;
using System.Linq;

namespace Models.Helpers
{
    public static class AppointmentDTOHelper
    {
        public static IEnumerable<AppointmentDTO> ToDTO(IEnumerable<Appointment> appointments)
        {
            List<AppointmentDTO> appointmentDTOs = new();

            foreach (Appointment appointment in appointments)
            {
                appointmentDTOs.Add(new AppointmentDTO
                {
                    Id = appointment.Id,
                   Invited = appointment.Invited,
                   Name = appointment.Name,
                   Description = appointment.Description,
                   LocationPickupId = appointment.LocationPickupId,
                   LocationDropofId = appointment.LocationDropofId,
                   LocationId = appointment.LocationId,
                   StartTime = appointment.StartTime,
                   EndTime = appointment.EndTime,
                   Date = appointment.Date,
                   Privacy = appointment.Privacy,
                   Accepted = appointment.Accepted
                });
            }

            return appointmentDTOs.AsEnumerable();
        }
    }
}