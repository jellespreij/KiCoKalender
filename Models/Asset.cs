using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [OpenApiExample(typeof(DummyAssetExample))]
    public class Asset : IEntityBase
    {
        [OpenApiProperty(Description = "Gets or sets the asset id.")]
        public long Id { get; set; }

        [OpenApiProperty(Description = "Gets or sets the user id.")]
        [JsonRequired]
        public long UserId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the family id.")]
        [JsonRequired]
        public long FamilyId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the description.")]
        [JsonRequired]
        public string Description { get; set; }

        [OpenApiProperty(Description = "Gets or sets the created date.")]
        [JsonRequired]
        public DateTime CreatedDate { get; set; }

        [OpenApiProperty(Description = "Gets or sets the assets enum.")]
        [JsonRequired]
        public Folder Folder { get; set; }

        [OpenApiProperty(Description = "Gets or sets the assets url.")]
        [JsonRequired]
        public string Url { get; set; }

        [OpenApiProperty(Description = "Gets or sets the partitionKey.")]
        [JsonRequired]
        public string PartitionKey { get; set; }

        public Asset()
        {

        }

        public Asset(long id, long userId, long familyId, string description, DateTime createdDate, Folder folder, string url, string partitionKey)
        {
            Id = id;
            UserId = userId;
            FamilyId = familyId;
            Description = description;
            CreatedDate = createdDate;
            Folder = folder;
            Url = url;
            PartitionKey = partitionKey;
        }
    }

    public class DummyAssetExample : OpenApiExample<Asset>
    {
        public override IOpenApiExample<Asset> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("Asset", "This is an asset summary", new Asset() { Id = 1, UserId = 101, FamilyId = 1, Description = "This is an asset", CreatedDate = DateTime.Now, Folder = Folder.Picture, Url = "-url-", PartitionKey = "1" }, NamingStrategy));

            return this;
        }
    }
}
