using Models;
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

        public Appointment UpdateAppointment(Appointment appointment, Guid id)
        {
            return _appointmentRepository.Update(appointment, id).Result;
        }
    }
}
