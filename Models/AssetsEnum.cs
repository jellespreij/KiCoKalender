using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KiCoKalender.Models
{
	/// <summary>
	/// This specifices the Role of the User.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum AssetsEnum
	{
		/// <summary>
		/// Identifies as "picture".
		/// </summary>
		[EnumMember(Value = "picture")]
		Picture = 1,

		/// <summary>
		/// Identifies as "medical".
		/// </summary>
		[EnumMember(Value = "medical")]
		Medical = 2,

		/// <summary>
		/// Identifies as "finance".
		/// </summary>
		[EnumMember(Value = "finance")]
		Finance = 3,

		/// <summary>
		/// Identifies as "school".
		/// </summary>
		[EnumMember(Value = "school")]
		School = 4,

		/// <summary>
		/// Identifies as "other".
		/// </summary>
		[EnumMember(Value = "other")]
		Other = 5
	}
}
