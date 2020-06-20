using System;

namespace Verse
{
	// Token: 0x02000373 RID: 883
	[AttributeUsage(AttributeTargets.Field)]
	public class DescriptionAttribute : Attribute
	{
		// Token: 0x06001A46 RID: 6726 RVA: 0x000A1989 File Offset: 0x0009FB89
		public DescriptionAttribute(string description)
		{
			this.description = description;
		}

		// Token: 0x04000F53 RID: 3923
		public string description;
	}
}
