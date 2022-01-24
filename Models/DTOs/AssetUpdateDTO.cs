using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;
using System;

namespace Models
{
    public class AssetUpdateDTO
    {
        [OpenApiProperty(Description = "Gets or sets the asset name.")]
        [JsonRequired]
        public string Name { get; set; }

        [OpenApiProperty(Description = "Gets or sets the description.")]
        public string Description { get; set; }

        public AssetUpdateDTO()
        {

        }
    }

}