using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;
using System;

namespace Models
{
    public class ContactDTO
    {
        [OpenApiProperty(Description = "Gets or sets the ID.")]
        public Guid Id { get; set; }

        [OpenApiProperty(Description = "Gets or sets the phoneNumber.")]
        public string PhoneNumber { get; set; }

        [OpenApiProperty(Description = "Gets or sets the name.")]
        [JsonRequired]
        public string Name { get; set; }

        [OpenApiProperty(Description = "Gets or sets the name.")]
        public string LastName { get; set; }

        [OpenApiProperty(Description = "Gets or sets the adres.")]
        public string City { get; set; }

        [OpenApiProperty(Description = "Gets or sets the adres.")]
        [JsonRequired]
        public string Address { get; set; }

        [OpenApiProperty(Description = "Gets or sets the postcode.")]
        [JsonRequired]
        public string Postcode { get; set; }

        [OpenApiProperty(Description = "Gets or sets the postcode.")]
        public string Email { get; set; }

        [OpenApiProperty(Description = "Gets or sets the contactType.")]
        [JsonRequired]
        public ContactType ContactType { get; set; }

        [OpenApiProperty(Description = "Gets or sets created date.")]
        [JsonRequired]
        public DateTime Created { get; set; }

        public ContactDTO()
        {

        }
    }

}