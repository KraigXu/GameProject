using System;

namespace RimWorld
{
	
	[AttributeUsage(AttributeTargets.Field)]
	public class MayRequireAttribute : Attribute
	{
		
		public MayRequireAttribute(string modId)
		{
			this.modId = modId;
		}

		
		public string modId;
	}
}
