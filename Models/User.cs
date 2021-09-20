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
    public class User : TableEntity
    {
        [OpenApiProperty(Description = "Gets or sets the parent ID.")]
        public long? userId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the first name.")]
        [JsonRequired]
        public string userFirstName { get; set; }

        [OpenApiProperty(Description = "Gets or sets the last name.")]
        [JsonRequired]
        public string userLastName { get; set; }

        [OpenApiProperty(Description = "Gets or sets the Role for the user.")]
        [JsonRequired]
        public Role userRole { get; set; }

        public User() 
        {
        
        }

        public User(long? userId, string userFirstName, string userLastName, Role userRole)
        {
            this.userId = userId;
            this.userFirstName = userFirstName;
            this.userLastName = userLastName;
            this.userRole = userRole;

            //PartitionKey = userId;
            //RowKey = userLastName + userFirstName;
        }
    }

    public class DummyUserExample : OpenApiExample<User>
    {
        public override IOpenApiExample<User> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("Dirk", "This is Dirk's summary",new User() { userId = 33, userFirstName = "Dirk", userLastName = "Dirksma", userRole = Role.Parent}, NamingStrategy));
            Examples.Add(OpenApiExampleResolver.Resolve("Jan", "This is Jan's summary", new User() { userId = 34, userFirstName = "Jan", userLastName = "Jansma", userRole = Role.Parent }, NamingStrategy));
    
            return this;
        }
    }
}
