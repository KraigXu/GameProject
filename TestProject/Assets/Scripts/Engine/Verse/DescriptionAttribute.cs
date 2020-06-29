using System;

namespace Verse
{
	
	[AttributeUsage(AttributeTargets.Field)]
	public class DescriptionAttribute : Attribute
	{
		
		public DescriptionAttribute(string description)
		{
			this.description = description;
		}

		
		public string description;
	}
}
