using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;
using System;

namespace Models
{
    public class TransactionUpdateDTO
    {
        [OpenApiProperty(Description = "Gets or sets the transaction name.")]
        [JsonRequired]
        public string Name { get; set; }

        [OpenApiProperty(Description = "Gets or sets the file name.")]
        [JsonRequired]
        public string FileName { get; set; }

        [OpenApiProperty(Description = "Gets or sets the transaction description.")]
        public string Description { get; set; }

        [OpenApiProperty(Description = "Gets or sets the transaction amount.")]
        [JsonRequired]
        public double Amount { get; set; }

        public TransactionUpdateDTO()
        {

        }
    }
}
