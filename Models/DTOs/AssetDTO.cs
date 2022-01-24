using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;
using System;

namespace Models
{
    public class AssetDTO
    {
        [OpenApiProperty(Description = "Gets or sets the asset id.")]
        public Guid Id { get; set; }

        [OpenApiProperty(Description = "Gets or sets the asset name.")]
        [JsonRequired]
        public string Name { get; set; }

        [OpenApiProperty(Description = "Gets or sets the description.")]
        public string Description { get; set; }

        [OpenApiProperty(Description = "Gets or sets the created date.")]
        [JsonRequired]
        public DateTime CreatedDate { get; set; }

        [OpenApiProperty(Description = "Gets or sets the assets url.")]
        public string Url { get; set; }

        public AssetDTO()
        {

        }
    }

}