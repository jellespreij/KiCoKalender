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
    [OpenApiExample(typeof(DummyUserExample))]
    public class User : IEntityBase
    {
        [OpenApiProperty(Description = "Gets or sets the ID.")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [OpenApiProperty(Description = "Gets or sets the foreign family ID.")]
        public Guid? FamilyId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the family.")]
        [JsonIgnore]
        public Family Family { get; set; }

        [OpenApiProperty(Description = "Gets or sets the email.")]
        [JsonRequired]
        public string Email { get; set; }

        [OpenApiProperty(Description = "Password for the user logging in.")]
        [JsonRequired]
        public string Password { get; set; }

        [OpenApiProperty(Description = "Gets or sets the first name.")]
        [JsonRequired]
        public string FirstName { get; set; }
        
        [OpenApiProperty(Description = "Gets or sets the last name.")]
        [JsonRequired]
        public string LastName { get; set; }

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
        public DateTime Created { get; set; }

        public User()
        {

        }

        public User(Guid id, string firstName, string lastName, string email, string password, Role role, DateTime age, string address, string postcode)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Role = role;
            Age = age;
            Address = address;
            Postcode = postcode;
            Created = DateTime.Now;
        }
    }

    public class DummyUserExample : OpenApiExample<User>
    {
        public override IOpenApiExample<User> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("Kim",
                                            new User()
                                            {
                                                Email = "inlog.email@gmail.com",
                                                Password = "SuperSecretPassword123!!",
                                                FirstName = "Kim",
                                                LastName = "Kim",
                                                Role = Role.Parent,
                                                Age = new DateTime(2000, 10, 10),
                                                Address = "street",
                                                Postcode = "1234AB",
                                                Created = DateTime.Now
                                            },
                                            NamingStrategy));
            return this;
        }
    }
}
