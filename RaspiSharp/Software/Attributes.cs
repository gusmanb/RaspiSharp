using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RaspiSharp.Software
{

	[AttributeUsage(AttributeTargets.Class, Inherited=true)]
	public class RaspElementCategoryAttribute : Attribute { public string Category { get; set; } }

	[AttributeUsage(AttributeTargets.Method, Inherited = true)]
	public class RaspInputAttribute : Attribute { public IOType InputType { get; set; } }

	[AttributeUsage(AttributeTargets.Event, Inherited = true)]
	public class RaspOutputAttribute : Attribute { public IOType OutputType { get; set; } }

	[AttributeUsage(AttributeTargets.Method, Inherited = true)]
	public class RaspMethodAttribute : Attribute { }

	[AttributeUsage(AttributeTargets.Property, Inherited = true)]
	public class RaspPropertyAttribute : Attribute { }

	public enum IOType
	{

		Signal,
		Byte,
		Buffer,
        Integer

	}
}
