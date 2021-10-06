using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace Models
{
    [OpenApiExample(typeof(DummyUserContextExample))]
    public class UserContext : IEntityBase
    {
        [OpenApiProperty(Description = "Gets or sets the ID.")]
        [JsonRequired]
        public long Id { get; set; }

        [OpenApiProperty(Description = "Gets or sets the userId.")]
        [JsonRequired]
        public long UserId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the name.")]
        [JsonRequired]
        public string Name { get; set; }

        [OpenApiProperty(Description = "Gets or sets the email.")]
        [JsonRequired]
        public string Email { get; set; }

        [OpenApiProperty(Description = "Gets or sets the age.")]
        [JsonRequired]
        public DateTime Age { get; set; }

        [OpenApiProperty(Description = "Gets or sets the Role for the user.")]
        [JsonRequired]
        public Role Role { get; set; }

        [OpenApiProperty(Description = "Gets or sets the adres.")]
        [JsonRequired]
        public string Address { get; set; }

        [OpenApiProperty(Description = "Gets or sets the postcode.")]
        [JsonRequired]
        public string Postcode { get; set; }

        [OpenApiProperty(Description = "Gets or sets created date.")]
        [JsonRequired]
        public DateTime Created { get; set; }

        [OpenApiProperty(Description = "Gets or sets the partitionKey.")]
        [JsonRequired]
        public string PartitionKey { get; set; }

        public UserContext()
        {

        }

        public UserContext(long id, long userId, string name, string email, Role role, DateTime age, string address, string postcode, string partitionKey)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Email = email;
            Role = role;
            Age = age;
            Address = address;
            Postcode = postcode;
            Created = DateTime.Now;
            PartitionKey = partitionKey;
        }
    }

    public class DummyUserContextExample : OpenApiExample<UserContext>
    {
        public override IOpenApiExample<UserContext> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("User", "This is a user summary", new UserContext() { Id = 1, UserId = 1, Name = "Dirk Dirksma", Role = Role.Parent, Email = "-email-", Age = new DateTime(2000, 10, 10), Address = "street", Postcode = "1234AB", Created = DateTime.Now, PartitionKey = "1" }, NamingStrategy));

            return this;
        }
    }
}
