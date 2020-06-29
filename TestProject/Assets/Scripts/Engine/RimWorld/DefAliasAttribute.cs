using System;

namespace RimWorld
{
	
	[AttributeUsage(AttributeTargets.Field)]
	public class DefAliasAttribute : Attribute
	{
		
		public DefAliasAttribute(string defName)
		{
			this.defName = defName;
		}

		
		public string defName;
	}
}
