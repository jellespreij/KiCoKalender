using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Folder
    {
		[EnumMember(Value = "Picture")]
        Picture = 1,

		[EnumMember(Value = "Medical")]
        Medical = 2,

		[EnumMember(Value = "Finance")]
        Finance = 3,

        [EnumMember(Value = "School")]
        School = 4,

        [EnumMember(Value = "Other")]
        Other = 5
    }
}
