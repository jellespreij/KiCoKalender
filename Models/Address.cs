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
    [OpenApiExample(typeof(DummyAddressExample))]
    public class Address : IEntityBase
    {
        [OpenApiProperty(Description = "Gets or sets the ID.")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [OpenApiProperty(Description = "Gets or sets the userId.")]
        [JsonRequired]
        public long UserId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the familyId.")]
        [JsonRequired]
        public long FamilyId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the name.")]
        [JsonRequired]
        public string Name { get; set; }

        [OpenApiProperty(Description = "Gets or sets the adres.")]
        [JsonRequired]
        public string Location { get; set; }

        [OpenApiProperty(Description = "Gets or sets the postcode.")]
        [JsonRequired]
        public string Postcode { get; set; }

        [OpenApiProperty(Description = "Gets or sets created date.")]
        [JsonRequired]
        public DateTime Created { get; set; }


        [OpenApiProperty(Description = "Gets or sets the partitionKey.")]
        public string PartitionKey
        {
            get => Id.ToString();
            set => Id = Guid.Parse(value);
        }

        public Address()
        {

        }

        public Address(Guid id, long userId, long familyId, string name, string location, string postcode, string partitionKey)
        {
            Id = id;
            UserId = userId;
            FamilyId = familyId;
            Name = name;
            Location = location;
            Postcode = postcode;
            Created = DateTime.Now;
            PartitionKey = partitionKey;
        }
    }

    public class DummyAddressExample : OpenApiExample<Address>
    {
        public override IOpenApiExample<Address> Build(NamingStrategy NamingStrategy = null)
        {
            Guid guid = Guid.NewGuid();
            Examples.Add(OpenApiExampleResolver.Resolve("Address",
                                new Address()
                                {
                                    UserId = 101,
                                    FamilyId = 1,
                                    Name = "Dirk Dirksma's huis",
                                    Location = "street123",
                                    Postcode = "AB1234"
                                },
                                NamingStrategy));
            return this;
        }
    }
}

