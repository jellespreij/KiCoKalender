using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json;
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

        [OpenApiProperty(Description = "Gets or sets the users.")]
        public List<User> Users { get; set; } = new List<User>();

        [OpenApiProperty(Description = "Gets or sets the users.")]
        public List<Folder> Folders { get; set; } = new List<Folder>();

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

        public Family(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class DummyFamilyExample : OpenApiExample<Family>
    {
        public override IOpenApiExample<Family> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("Family", "This is a family summary", new Family() { Name = "family name" }, NamingStrategy));

            return this;
        }
    }
}
