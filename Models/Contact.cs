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
    [OpenApiExample(typeof(DummyContactExample))]
    public class Contact : IEntityBase
    {
        [OpenApiProperty(Description = "Gets or sets the ID.")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [OpenApiProperty(Description = "Gets or sets the familyId.")]
        [JsonRequired]
        public Guid FamilyId { get; set; }

        [OpenApiProperty(Description = "Gets or sets the phoneNumber.")]
        public string PhoneNumber { get; set; }

        [OpenApiProperty(Description = "Gets or sets the name.")]
        [JsonRequired]
        public string Name { get; set; }
        
        [OpenApiProperty(Description = "Gets or sets the name.")]
        public string LastName { get; set; }

        [OpenApiProperty(Description = "Gets or sets the adres.")]
        public string City { get; set; }
        
        [OpenApiProperty(Description = "Gets or sets the adres.")]
        [JsonRequired]
        public string Address { get; set; }

        [OpenApiProperty(Description = "Gets or sets the postcode.")]
        [JsonRequired]
        public string Postcode { get; set; }
        
        [OpenApiProperty(Description = "Gets or sets the postcode.")]
        public string Email { get; set; }

        [OpenApiProperty(Description = "Gets or sets the contactType.")]
        [JsonRequired]
        public ContactType ContactType { get; set; }

        [OpenApiProperty(Description = "Gets or sets created date.")]
        [JsonRequired]
        public DateTime Created { get; set; }

        [OpenApiProperty(Description = "Gets or sets the partitionKey.")]
        public string PartitionKey { get; set; }

        public Contact()
        {

        }

        public Contact(Guid id, Guid familyId, string phoneNumber, ContactType contactType, string name, string city, string address, string postcode, string partitionKey)
        {
            Id = id;
            FamilyId = familyId;
            PhoneNumber = phoneNumber;
            Name = name;
            ContactType = contactType;
            City = city;
            Address = address;
            Postcode = postcode;
            Created = DateTime.Now;
            PartitionKey = partitionKey;
        }
    }

    public class DummyContactExample : OpenApiExample<Contact>
    {
        public override IOpenApiExample<Contact> Build(NamingStrategy NamingStrategy = null)
        {
            Guid guidFamily = Guid.NewGuid();

            Examples.Add(OpenApiExampleResolver.Resolve("Address",
                                new Contact()
                                {
                                    FamilyId = guidFamily,
                                    ContactType = ContactType.family,
                                    PhoneNumber = "02012345678",
                                    Name = "Dirk Dirksma's huis",
                                    Address = "street123",
                                    Postcode = "AB1234",
                                    City = "city",
                                },
                                NamingStrategy));
            return this;
        }
    }
}

