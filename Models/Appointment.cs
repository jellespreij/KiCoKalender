using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [OpenApiExample(typeof(DummyAppointmentExample))]
    public class Appointment : IEntityBase
    {
        [OpenApiProperty(Description = "Gets or sets the appointment ID.")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [OpenApiProperty(Description = "Gets or sets the user ID of the creator of the appointment.")]
        public Guid UserId { get; set; }
        
        [OpenApiProperty(Description = "Gets or sets the family ID.")]
        public Guid FamilyId { get; set; }

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
        public string PartitionKey
        {
            get => Id.ToString();
            set => Id = Guid.Parse(value);
        }

        public Appointment()
        {

        }

        public Appointment(Guid appointmentId, Guid userId, Guid familyId, string name, string description, DateTime date, string partitionKey)
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
            Guid guid = Guid.NewGuid();
            Examples.Add(OpenApiExampleResolver.Resolve("Appointment",
                    new Appointment()
                    {
                        UserId = guid,
                        FamilyId = guid,
                        Name = "name",
                        Description = "description",
                        Date = new DateTime(2000, 10, 10),
                        Private = false,
                        Accepted = true
                    },
                    NamingStrategy));
            return this;
        }
    }
}
