using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;
using System;

namespace Models
{
    public class ContactUpdateDTO
    {
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

        public ContactUpdateDTO()
        {

        }
    }

}