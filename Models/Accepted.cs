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
	public enum Accepted
	{

		[EnumMember(Value = "Accepted")]
		accepted = 1,


		[EnumMember(Value = "Declined")]
		declined = 2,


		[EnumMember(Value = "Pending")]
		pending = 3
	}	
}

