using System.Collections.Generic;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace KiCoKalender.Models
{
    [OpenApiExample(typeof(DummyAssetExample))]
    public class Asset
    {
        [OpenApiProperty(Description = "Gets or sets the asset id.")]
        public long? assetId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the user id.")]
        [JsonRequired]
        public long? userId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the description.")]
        [JsonRequired]
        public string assetDescription { get; set; }

        [OpenApiProperty(Description = "Gets or sets the created date.")]
        [JsonRequired]
        public DateTime assetCreatedDate { get; set; }

        [OpenApiProperty(Description = "Gets or sets the assets enum.")]
        [JsonRequired]
        public AssetsEnum assetsEnum { get; set; }

        [OpenApiProperty(Description = "Gets or sets the assets url.")]
        [JsonRequired]
        public string assetURL { get; set; }

        public Asset()
        {

        }

        public Asset(long? assetId, long? userId, string assetDescription, DateTime assetCreatedDate, AssetsEnum assetsEnum, string assetURL)
        {
            this.assetId = assetId;
            this.userId = userId;
            this.assetDescription = assetDescription;
            this.assetCreatedDate = assetCreatedDate;
            this.assetsEnum = assetsEnum;
            this.assetURL = assetURL;

            //PartitionKey = assetId;
            //RowKey = TBA;
        }
    }

    public class DummyAssetExample : OpenApiExample<Asset>
    {
        public override IOpenApiExample<Asset> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("Asset", "This is an asset summary", new Asset() { assetId = 1, userId = 101, assetDescription = "This is an asset", assetCreatedDate = DateTime.Now, assetsEnum = AssetsEnum.Picture, assetURL = "-url-" }, NamingStrategy));

            return this;
        }
    }
}
