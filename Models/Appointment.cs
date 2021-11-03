using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Microsoft.OpenApi.Models;
using Models.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
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
        [JsonRequired]
        public Guid UserId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the family ID.")]
        [JsonRequired]
        public Guid FamilyId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the invited users.")]
        public Invited Invited { get; set; }

        [OpenApiProperty(Description = "Gets or sets the appointment name.")]
        [JsonRequired]
        public string Name { get; set; }

        [OpenApiProperty(Description = "Gets or sets the appointment description.")]
        public string Description { get; set; }

        [OpenApiProperty(Description = "Gets or sets the appointment pick up location.")]
        public Guid LocationPickupId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the appointment drop of location.")]
        public Guid LocationDropofId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the appointment location.")]
        [JsonRequired]
        public Guid LocationId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the appointment start time.")]
        [JsonRequired]
        public DateTime StartTime { get; set; }

        [OpenApiProperty(Description = "Gets or sets the appointment end time.")]
        public DateTime EndTime { get; set; }

        [OpenApiProperty(Description = "Gets or sets the appointment date.")]
        [JsonRequired]
        public DateTime Date { get; set; }

        [OpenApiProperty(Description = "Gets or sets the appointment privacy.")]
        [JsonRequired]
        public bool Privacy { get; set; }

        [OpenApiProperty(Description = "Gets or sets the appointment acception.")]
        [JsonRequired]
        public Accepted Accepted { get; set; }

        [OpenApiProperty(Description = "Gets or sets the partitionKey.")]
        public string PartitionKey { get; set; }

        public Appointment()
        {

        }

        public Appointment(Guid id, Guid userId, Guid familyId, Invited invited, Accepted accepted, string name, bool privacy, string description, DateTime date, string partitionKey)
        {
            Id = id;
            UserId = userId;
            FamilyId = familyId;
            Invited = invited;
            Name = name;
            Accepted = accepted;
            Privacy = privacy;
            Description = description;
            Date = date;
            PartitionKey = partitionKey;
        }
    }

    public class DummyAppointmentExample : OpenApiExample<Appointment>
    {
        public override IOpenApiExample<Appointment> Build(NamingStrategy NamingStrategy = null)
        {
            Guid guidUser = Guid.NewGuid();
            Guid guidFamily = Guid.NewGuid();
            Guid guidLocation = Guid.NewGuid();

            Examples.Add(OpenApiExampleResolver.Resolve("Appointment",
                    new Appointment()
                    {
                        UserId = guidUser,
                        FamilyId = guidFamily,
                        LocationId = guidLocation,
                        Name = "name",
                        Date = new DateTime(2000, 10, 10),
                        Privacy = false,
                        Accepted = Accepted.pending,
                    },
                    NamingStrategy));
            return this;
        }
    }
}
