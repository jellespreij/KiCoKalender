using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [OpenApiExample(typeof(DummyFamilyExample))]
    public class Family : IEntityBase
    {
        [OpenApiProperty(Description = "Gets or sets the family id.")]
        public long Id { get; set; }

        [OpenApiProperty(Description = "Gets or sets the family parentIds.")]
        public virtual List<User> Parents { get; set; }

        [OpenApiProperty(Description = "Gets or sets the family childrenIds.")]
        public virtual List<User> Children { get; set; }

        [OpenApiProperty(Description = "Gets or sets the name.")]
        public string Name { get; set; }

        [OpenApiProperty(Description = "Gets or sets the partitionKey.")]
        public string PartitionKey { get; set; }

        public Family() 
        {
        
        }

        public Family(long id, List<User> parents, List<User> children, string name, string partitionKey)
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
            List<User> parents = new();
            parents.Add(new User() { Id = 1, Name = "Jelle Spreij", Address = "straat1234", Email = "-email-", Password = "password", Age = DateTime.Now, Created = DateTime.Now, Postcode = "AB1234", Role = Role.Parent, PartitionKey = "1"});

            List<User> children = new();
            children.Add(new User() { Id = 3, Name = "Baas b", Address = "straat4321", Email = "-email-", Password = "password", Age = DateTime.Now, Created = DateTime.Now, Postcode = "AB1234", Role = Role.Child, PartitionKey = "3" });


            Examples.Add(OpenApiExampleResolver.Resolve("Family", "This is a family summary", new Family() { Id = 1, Parents = parents, Children = children, Name = "family name", PartitionKey = "1" }, NamingStrategy));

            return this;
        }
    }
}
