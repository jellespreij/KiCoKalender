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

namespace KiCoKalender.Models
{
    [OpenApiExample(typeof(DummyFamilyExample))]
    public class Family
    {
        [OpenApiProperty(Description = "Gets or sets the family ID.")]
        public long? familyId { get; set; }

        [OpenApiProperty(Description = "Gets or sets parent IDs.")]
        [JsonRequired]
        public string familyName { get; set; }

        [OpenApiProperty(Description = "Gets or sets parent IDs.")]
        [JsonRequired]
        public long? parentIds { get; set; }

        [OpenApiProperty(Description = "Gets or sets children IDs.")]
        [JsonRequired]
        public long? childrenIds { get; set; }

        public Family()
        {

        }

        public Family(long? familyId, string familyName, long? parentIds, long? childrenIds)
        {
            this.familyId = familyId;
            this.parentIds = parentIds;
            this.childrenIds = childrenIds;
            this.familyName = familyName;

            //PartitionKey = userId;
            //RowKey = userLastName + userFirstName;
        }
    }

    public class DummyFamilyExample : OpenApiExample<Family>
    {
        public override IOpenApiExample<Family> Build(NamingStrategy NamingStrategy = null)
        {
            Examples.Add(OpenApiExampleResolver.Resolve("family", "This is a family summary", new Family() { familyId = 1, familyName = "family name", parentIds = 1, childrenIds = 3 }, NamingStrategy));

            return this;
        }
    }
}
