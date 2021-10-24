using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ContactType
    {
        [EnumMember(Value = "Family")]
        family = 1,

        [EnumMember(Value = "Friends")]
        friends = 2,

        [EnumMember(Value = "Mediator")]
        mediator = 3,

        [EnumMember(Value = "School")]
        school = 4,

        [EnumMember(Value = "Sport")]
        sport = 5
    }
}
