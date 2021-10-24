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
    [OpenApiExample(typeof(DummyTransactionExample))]
    public class Transaction : IEntityBase
    {
        [OpenApiProperty(Description = "Gets or sets the transaction ID.")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [OpenApiProperty(Description = "Gets or sets the user ID of the creator of the transaction.")]
        public Guid UserId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the family ID.")]
        public Guid FamilyId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the transaction name.")]
        [JsonRequired]
        public string Name { get; set; }

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

        [OpenApiProperty(Description = "Gets or sets the partitionKey.")]
        public string PartitionKey { get; set; }

        public Transaction()
        {

        }

        public Transaction(Guid id, Guid userId, Guid familyId, string name, string url, double amount, string description, DateTime date, string partitionKey)
        {
            Id = id;
            UserId = userId;
            FamilyId = familyId;
            Name = name;
            Amount = amount;
            Description = description;
            Date = date;
            Url = url;
            PartitionKey = partitionKey;
        }
    }

    public class DummyTransactionExample : OpenApiExample<Transaction>
    {
        public override IOpenApiExample<Transaction> Build(NamingStrategy NamingStrategy = null)
        {
            Guid guidUser = Guid.NewGuid();
            Guid guidFamily = Guid.NewGuid();

            Examples.Add(OpenApiExampleResolver.Resolve("Transaction",
                    new Transaction()
                    {
                        UserId = guidUser,
                        FamilyId = guidFamily,
                        Name = "name",
                        Date = new DateTime(2000, 10, 10),
                        PartitionKey = guidFamily.ToString()
                    },
                    NamingStrategy));
            return this;
        }
    }
}
