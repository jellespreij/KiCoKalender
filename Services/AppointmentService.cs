using Models;
using Models.Helpers;
using Repositories;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AppointmentService : IAppointmentService
    {
        private IAppointmentRepository _appointmentRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository) 
        {
            _appointmentRepository = appointmentRepository;   
        }

        public Appointment FindAppointmentByAppointmentId(Guid appointmentId)
        {
            return _appointmentRepository.GetSingle(appointmentId);
        }

        public Appointment AddAppointment(Appointment appointment)
        {
            return _appointmentRepository.Add(appointment).Result;
        }

        public Appointment DeleteAppointment(Guid id)
        {
            return _appointmentRepository.Delete(id).Result;
        }

        public IEnumerable<Appointment> FindAppointmentByFamilyIdAndUserId(Guid familyId, Guid userId)
        {
            return _appointmentRepository.FindBy(e => e.FamilyId == familyId && (e.Privacy == false || (e.UserId == userId && e.Privacy == true)));
        }
        
        public IEnumerable<AppointmentDTO> FindAppointmentDTOByFamilyIdAndUserId(Guid familyId, Guid userId)
        {
            IEnumerable<Appointment> appointments = FindAppointmentByFamilyIdAndUserId(familyId, userId);

            return AppointmentDTOHelper.ToDTO(appointments);
        }

        public Appointment UpdateAppointment(AppointmentUpdateDTO appointmentUpdate, Guid id)
        {
            Appointment appointmentToUpdate = FindAppointmentByAppointmentId(id);

            appointmentToUpdate.Invited = appointmentUpdate.Invited;
            appointmentToUpdate.Name = appointmentUpdate.Name;
            appointmentToUpdate.Description = appointmentUpdate.Description;
            appointmentToUpdate.LocationPickupId = appointmentUpdate.LocationPickupId;
            appointmentToUpdate.LocationDropofId = appointmentUpdate.LocationDropofId;
            appointmentToUpdate.LocationId = appointmentUpdate.LocationId;
            appointmentToUpdate.StartTime = appointmentUpdate.StartTime;
            appointmentToUpdate.EndTime = appointmentUpdate.EndTime;
            appointmentToUpdate.Date = appointmentUpdate.Date;
            appointmentToUpdate.Privacy = appointmentUpdate.Privacy;
           
            return _appointmentRepository.Update(appointmentToUpdate, id).Result;
        }
    }
}
