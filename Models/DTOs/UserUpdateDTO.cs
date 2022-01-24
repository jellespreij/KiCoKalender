using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;

namespace Models
{
    public class UserUpdateDTO
    {
        [OpenApiProperty(Description = "Gets or sets the email.")]
        [JsonRequired]
        public string Email { get; set; }

        [OpenApiProperty(Description = "Gets or sets the adres.")]
        [JsonRequired]
        public string Address { get; set; }

        [OpenApiProperty(Description = "Gets or sets the phonenumber.")]
        [JsonRequired]
        public string PhoneNumber { get; set; }

        [OpenApiProperty(Description = "Gets or sets the zipcode.")]
        [JsonRequired]
        public string Zipcode { get; set; }

        public UserUpdateDTO()
        {

        }
    }

}