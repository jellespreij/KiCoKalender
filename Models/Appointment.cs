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
    [OpenApiExample(typeof(DummyUserExample))]
    public class Appointment : TableEntity
    {
        [OpenApiProperty(Description = "Gets or sets the appointment ID.")]
        public long? appointmentId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the appointment name.")]
        [JsonRequired]
        public string appointmentName { get; set; }

        [OpenApiProperty(Description = "Gets or sets the appointment description.")]
        [JsonRequired]
        public string AppointmentDescription { get; set; }

        [OpenApiProperty(Description = "Gets or sets the appointment date.")]
        [JsonRequired]
        public DateTime AppointmentDate { get; set; }

        public Appointment()
        {

        }

        public Appointment(long? appointmentId, string appointmentName, string appointmentDescription, DateTime appointmentDate)
        {
            this.appointmentId = appointmentId;
            this.appointmentName = appointmentName;
            this.AppointmentDescription = appointmentDescription;
            this.AppointmentDate = appointmentDate;

            //PartitionKey = userId;
            //RowKey = userLastName + userFirstName;
        }
    }
}

