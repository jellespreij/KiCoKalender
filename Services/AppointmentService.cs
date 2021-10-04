using Models;
using Repositories;
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

        public void AddAppointment(Appointment appointment)
        {
            _appointmentRepository.Add(appointment);
        }

        public void DeleteAppointment(Appointment appointment)
        {
            _appointmentRepository.Delete(appointment);
        }

        public IEnumerable<Appointment> FindAppointmentByFamilyId(long familyId)
        {
            return _appointmentRepository.FindBy(e => e.Id == familyId);
        }

        public IEnumerable<Appointment> FindAppointmentByUserId(long userId)
        {
            return _appointmentRepository.FindBy(e => e.UserId == userId);
        }

        public void UpdateAppointment(Appointment appointment)
        {
            _appointmentRepository.Update(appointment);
        }
    }
}
