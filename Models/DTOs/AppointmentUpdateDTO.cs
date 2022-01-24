using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;
using System;

namespace Models
{
    public class AppointmentUpdateDTO
    {
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

        public AppointmentUpdateDTO()
        {

        }
    }

}