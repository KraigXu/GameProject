using System;

namespace Spirit
{
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class StaticConstructorOnStartup : Attribute
	{
	}
}
