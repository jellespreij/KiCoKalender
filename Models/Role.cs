using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KiCoKalender.Models
{
	/// <summary>
	/// This specifices the Role of the User.
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	public enum Role
	{
		/// <summary>
		/// Identifies as "Parent".
		/// </summary>
		[EnumMember(Value = "Parent")]
		Parent = 1,

		/// <summary>
		/// Identifies as "Child".
		/// </summary>
		[EnumMember(Value = "Child")]
		Child = 2,

		/// <summary>
		/// Identifies as "Mediator".
		/// </summary>
		[EnumMember(Value = "Mediator")]
		Mediator = 3
	}
}
