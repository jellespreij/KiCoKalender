using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace Models
{
    [OpenApiExample(typeof(DummyUserExample))]
    public class User : IEntityBase
    {
        [OpenApiProperty(Description = "Gets or sets the ID.")]
        [JsonRequired]
        public long Id { get; set; }

        [OpenApiProperty(Description = "Gets or sets the name.")]
        [JsonRequired]
        public string Name { get; set; }

        [OpenApiProperty(Description = "Gets or sets the email.")]
        [JsonRequired]
        public string Email { get; set; }

        [OpenApiProperty(Description = "Gets or sets the password.")]
        [JsonRequired]
        public string Password { get; set; }

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

        public User()
        {

        }

        public User(long id, string name, string email, string password, Role role, DateTime age, string address, string postcode, string partitionKey)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
            Role = role;
            Age = age;
            Address = address;
            Postcode = postcode;
            Created = DateTime.Now;
            PartitionKey = partitionKey;
        }
    }

    public class DummyUserExample : OpenApiExample<User>
    {
        public override IOpenApiExample<User> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("User", "This is a user summary", new User() { Id = 1, Name = "Dirk Dirksma", Role = Role.Parent, Email = "-email-", Password = "DitIsEenWachtwoord", Age = new DateTime(2000, 10, 10), Address = "street", Postcode = "1234AB", Created = DateTime.Now, PartitionKey = "1" }, NamingStrategy));

            return this;
        }
    }
}
