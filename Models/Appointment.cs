using System.Collections.Generic;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace KiCoKalender.Models
{
    [OpenApiExample(typeof(DummyAppointmentExample))]
    public class Appointment
    {
        [OpenApiProperty(Description = "Gets or sets the appointment ID.")]
        public long? appointmentId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the user ID.")]
        public long? userId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the appointment name.")]
        [JsonRequired]
        public string appointmentName { get; set; }

        [OpenApiProperty(Description = "Gets or sets the appointment description.")]
        [JsonRequired]
        public string AppointmentDescription { get; set; }

        [OpenApiProperty(Description = "Gets or sets the appointment date.")]
        [JsonRequired]
        public DateTime AppointmentDate { get; set; }

        [OpenApiProperty(Description = "Gets or sets the appointment privacy.")]
        [JsonRequired]
        public bool AppointmentPrivate { get; set; }

        [OpenApiProperty(Description = "Gets or sets the appointment acception.")]
        [JsonRequired]
        public bool AppointmentAccepted { get; set; }

        public Appointment()
        {

        }

        public Appointment(long? appointmentId, long? userId, string appointmentName, string appointmentDescription, DateTime appointmentDate)
        {
            this.appointmentId = appointmentId;
            this.userId = userId;
            this.appointmentName = appointmentName;
            this.AppointmentDescription = appointmentDescription;
            this.AppointmentDate = appointmentDate;
        }
    }

    public class DummyAppointmentExample : OpenApiExample<User>
    {
        public override IOpenApiExample<User> Build(NamingStrategy NamingStrategy = null)
        {

            Examples.Add(OpenApiExampleResolver.Resolve("Appointment", "This is an appointment summary", new Appointment() { appointmentId = 1, userId = 2, appointmentName = "name", AppointmentDescription = "description", AppointmentDate = new DateTime(2000, 10, 10), AppointmentPrivate = false, AppointmentAccepted = true }, NamingStrategy));

            return this;
        }
    }
}

