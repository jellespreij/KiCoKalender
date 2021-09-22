using KiCoKalender.Interfaces;
using KiCoKalender.Models;
using KiCoKalender.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KiCoKalender.Service
{
    class AppointmentService : IAppointmentService
    {
        private AppointmentRepository AppointmentRepository { get; }

        public AppointmentService(AppointmentRepository AppointmentRepository)
        {
            this.AppointmentRepository = AppointmentRepository;
        }

        public void AddAppointment(Appointment appointment)
        {
            AppointmentRepository.AddAppointment(appointment);
        }

        public void DeleteAppointment(Appointment appointment)
        {
            AppointmentRepository.DeleteAppointment(appointment);
        }

        public Task<List<Appointment>> FindByUserId(long? userId)
        {
            return AppointmentRepository.FindByUserId(userId);
        }

        public void UpdateAppointment(Appointment appointment)
        {
            AppointmentRepository.UpdateAppointment(appointment);
        }
    }
}
