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
    [OpenApiExample(typeof(DummyAssetExample))]
    public class Asset : IEntityBase
    {
        [OpenApiProperty(Description = "Gets or sets the asset id.")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [OpenApiProperty(Description = "Gets or sets the asset name.")]
        [JsonRequired]
        public string Name { get; set; }

        [OpenApiProperty(Description = "Gets or sets the description.")]
        public string Description { get; set; }

        [OpenApiProperty(Description = "Gets or sets the created date.")]
        [JsonRequired]
        public DateTime CreatedDate { get; set; }

        [OpenApiProperty(Description = "Gets or sets the foreign folder ID.")]
        public Guid? FolderId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the folder.")]
        [JsonIgnore]
        public Folder Folder { get; set; }

        [OpenApiProperty(Description = "Gets or sets the assets url.")]
        public string Url { get; set; }

        [OpenApiProperty(Description = "Gets or sets the partitionKey.")]
        public string PartitionKey { get; set; }

        public Asset()
        {

        }

        public Asset(Guid id, string name, string description, DateTime createdDate, string url, string partitionKey)
        {
            Id = id;
            Name = name;
            Description = description;
            CreatedDate = createdDate;
            Url = url;
            PartitionKey = partitionKey;
        }
    }

    public class DummyAssetExample : OpenApiExample<Asset>
    {
        public override IOpenApiExample<Asset> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("Asset",
                                new Asset()
                                {
                                    Name = "name",
                                    Description = "This is an asset",
                                    CreatedDate = DateTime.Now,
                                    Url = "-url-",
                                },
                                NamingStrategy));
            return this;
        }
    }
}
