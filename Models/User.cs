using System.Collections.Generic;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace KiCoKalender.Models
{
    [OpenApiExample(typeof(DummyUserExample))]
    public class User
    {
        [OpenApiProperty(Description = "Gets or sets the parent ID.")]
        public long? userId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the name.")]
        [JsonRequired]
        public string Name { get; set; }

        [OpenApiProperty(Description = "Gets or sets the Role for the user.")]
        [JsonRequired]
        public Role role { get; set; }
    }

    public class DummyUserExample : OpenApiExample<User>
    {
        public override IOpenApiExample<User> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("Dirk", "This is Dirk's summary",new User() { userId = 33, Name = "Dirk", role = Role.Parent}, NamingStrategy));
            Examples.Add(OpenApiExampleResolver.Resolve("Jan", "This is Jan's summary", new User() { userId = 34, Name = "Jan", role = Role.Parent }, NamingStrategy));
    
            return this;
        }
    }
}
