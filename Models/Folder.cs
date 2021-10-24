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
    public class Folder : IEntityBase
    {
        [OpenApiProperty(Description = "Gets or sets the folder id.")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [OpenApiProperty(Description = "Gets or sets the name.")]
        public string Name { get; set; }

        [OpenApiProperty(Description = "Gets or sets the foreign family ID.")]
        public Guid? FamilyId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the family.")]
        [JsonIgnore]
        public Family Family { get; set; }
        
        [OpenApiProperty(Description = "Gets or sets the assets.")]
        [JsonIgnore]
        public List<Asset> Assets { get; set; } = new List<Asset>();

        public Folder()
        {
        }

        public Folder(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        [OpenApiProperty(Description = "Gets or sets the partitionKey.")]
        public string PartitionKey { get; set; }
    }
}
