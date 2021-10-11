using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum Role
	{

		[EnumMember(Value = "Parent")]
		Parent = 1,


		[EnumMember(Value = "Child")]
		Child = 2,


		[EnumMember(Value = "Mediator")]
		Mediator = 3
	}
}
