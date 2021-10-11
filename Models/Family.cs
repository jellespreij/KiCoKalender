using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [OpenApiExample(typeof(DummyFamilyExample))]
    public class Family : IEntityBase
    {
        [OpenApiProperty(Description = "Gets or sets the family id.")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [OpenApiProperty(Description = "Gets or sets the family parentIds.")]
        public virtual List<UserContext> Parents { get; set; }

        [OpenApiProperty(Description = "Gets or sets the family childrenIds.")]
        public virtual List<UserContext> Children { get; set; }

        [OpenApiProperty(Description = "Gets or sets the name.")]
        public string Name { get; set; }

        [OpenApiProperty(Description = "Gets or sets the partitionKey.")]
        public string PartitionKey
        {
            get => Id.ToString();
            set => Id = Guid.Parse(value);
        }

        public Family() 
        {
        
        }

        public Family(Guid id, List<UserContext> parents, List<UserContext> children, string name, string partitionKey)
        {
            Id = id;
            Parents = parents;
            Children = children;
            Name = name;
            PartitionKey = partitionKey;
        }
    }

    public class DummyFamilyExample : OpenApiExample<Family>
    {
        public override IOpenApiExample<Family> Build(NamingStrategy NamingStrategy = null)
        {
            List < UserContext > parents = new();
            Examples.Add(OpenApiExampleResolver.Resolve("Jelle",
                                            new UserContext()
                                            {
                                                Name = "Jelle Spreij",
                                                Address = "straat1234",
                                                Email = "-email-",
                                                Age = DateTime.Now,
                                                Created = DateTime.Now,
                                                Postcode = "AB1234",
                                                Role = Role.Parent
                                            },
                                            NamingStrategy));

            List<UserContext> children = new();
            Examples.Add(OpenApiExampleResolver.Resolve("Baas",
                                            new UserContext()
                                            {
                                                Name = "Baas b",
                                                Address = "straat4321",
                                                Email = "-email-",
                                                Age = DateTime.Now,
                                                Created = DateTime.Now,
                                                Postcode = "AB1234",
                                                Role = Role.Child
                                            },
                                            NamingStrategy));

            // List<UserContext> parents = new();
            // parents.Add(new UserContext() { Id = Guid.NewGuid(), Name = "Jelle Spreij", Address = "straat1234", Email = "-email-", Age = DateTime.Now, Created = DateTime.Now, Postcode = "AB1234", Role = Role.Parent, PartitionKey = "1"});

            //List<UserContext> children = new();
            //children.Add(new UserContext() { Id = Guid.NewGuid(), Name = "Baas b", Address = "straat4321", Email = "-email-", Age = DateTime.Now, Created = DateTime.Now, Postcode = "AB1234", Role = Role.Child, PartitionKey = "3" });


            Examples.Add(OpenApiExampleResolver.Resolve("Family", "This is a family summary", new Family() { Parents = parents, Children = children, Name = "family name" }, NamingStrategy));

            return this;
        }
    }
}
