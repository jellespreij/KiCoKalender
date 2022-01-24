using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Newtonsoft.Json;
using System;

namespace Models
{
    public class TransactionDTO
    {
        [OpenApiProperty(Description = "Gets or sets the transaction ID.")]
        public Guid Id { get; set; }

        [OpenApiProperty(Description = "Gets or sets the transaction name.")]
        [JsonRequired]
        public string Name { get; set; }

        [OpenApiProperty(Description = "Gets or sets the file name.")]
        [JsonRequired]
        public string FileName { get; set; }

        [OpenApiProperty(Description = "Gets or sets the transaction description.")]
        public string Description { get; set; }

        [OpenApiProperty(Description = "Gets or sets the transaction date.")]
        [JsonRequired]
        public DateTime Date { get; set; }

        [OpenApiProperty(Description = "Gets or sets the transaction amount.")]
        [JsonRequired]
        public double Amount { get; set; }

        [OpenApiProperty(Description = "Gets or sets the transaction url.")]
        public string Url { get; set; }

        public TransactionDTO()
        {

        }
    }
}
