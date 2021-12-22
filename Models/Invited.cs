using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum Invited
	{
		[EnumMember(Value = "Everyone")]
		everyone = 1,


		[EnumMember(Value = "Parents")]
		parents = 2,


		[EnumMember(Value = "Children")]
		children = 3
	}
}
