using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace Models
{
    [OpenApiExample(typeof(DummyAppointmentExample))]
    public class Appointment : IEntityBase
    {
        [OpenApiProperty(Description = "Gets or sets the appointment ID.")]
        public long Id { get; set; }

        [OpenApiProperty(Description = "Gets or sets the user ID of the creator of the appointment.")]
        public long UserId { get; set; }
        
        [OpenApiProperty(Description = "Gets or sets the family ID.")]
        public long FamilyId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the appointment name.")]
        [JsonRequired]
        public string Name { get; set; }

        [OpenApiProperty(Description = "Gets or sets the appointment description.")]
        [JsonRequired]
        public string Description { get; set; }

        [OpenApiProperty(Description = "Gets or sets the appointment date.")]
        [JsonRequired]
        public DateTime Date { get; set; }

        [OpenApiProperty(Description = "Gets or sets the appointment privacy.")]
        [JsonRequired]
        public bool Private { get; set; }

        [OpenApiProperty(Description = "Gets or sets the appointment acception.")]
        [JsonRequired]
        public bool Accepted { get; set; }

        [OpenApiProperty(Description = "Gets or sets the partitionKey.")]
        [JsonRequired]
        public string PartitionKey { get; set; }

        public Appointment()
        {

        }

        public Appointment(long appointmentId, long userId, long familyId, string name, string description, DateTime date, string partitionKey)
        {
            Id = appointmentId;
            UserId = userId;
            FamilyId = familyId;
            Name = name;
            Description = description;
            Date = date;
            PartitionKey = partitionKey;
        }
    }

    public class DummyAppointmentExample : OpenApiExample<Appointment>
    {
        public override IOpenApiExample<Appointment> Build(NamingStrategy NamingStrategy = null)
        {

            Examples.Add(OpenApiExampleResolver.Resolve("Appointment", "This is an appointment summary", new Appointment() { Id = 1, UserId = 2, FamilyId = 1, Name = "name", Description = "description", Date = new DateTime(2000, 10, 10), Private = false, Accepted = true, PartitionKey = "2" }, NamingStrategy));

            return this;
        }
    }
}
