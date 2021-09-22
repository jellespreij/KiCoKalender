using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace KiCoKalender.Models
{
    [OpenApiExample(typeof(DummyUserExample))]
    public class User
    {
        [OpenApiProperty(Description = "Gets or sets the ID.")]
        public long? userId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the name.")]
        [JsonRequired]
        public string userName { get; set; }

        [OpenApiProperty(Description = "Gets or sets the age.")]
        [JsonRequired]
        public DateTime userAge { get; set; }

        [OpenApiProperty(Description = "Gets or sets the Role for the user.")]
        [JsonRequired]
        public Role userRole { get; set; }

        [OpenApiProperty(Description = "Gets or sets the adres.")]
        [JsonRequired]
        public string userAddress { get; set; }

        [OpenApiProperty(Description = "Gets or sets the postcode.")]
        [JsonRequired]
        public string userPostcode { get; set; }

        [OpenApiProperty(Description = "Gets or sets created date.")]
        [JsonRequired]
        public DateTime userCreated { get; set; }

        public User()
        {

        }

        public User(long? userId, string userFirstName, string userLastName, Role userRole)
        {
            this.userId = userId;
            this.userName = userName;
            this.userRole = userRole;

            //PartitionKey = userId;
            //RowKey = userLastName + userFirstName;
        }
    }

    public class DummyUserExample : OpenApiExample<User>
    {
        public override IOpenApiExample<User> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("Dirk", "This is Dirk's summary", new User() { userId = 101, userName = "Dirk Dirksma", userRole = Role.Parent, userAge = new DateTime(2000, 10, 10), userAddress = "street", userPostcode = "1234AB", userCreated = DateTime.Now }, NamingStrategy));

            return this;
        }
    }
}
