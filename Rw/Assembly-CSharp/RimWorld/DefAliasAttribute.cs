using System;

namespace RimWorld
{
	// Token: 0x02000F4C RID: 3916
	[AttributeUsage(AttributeTargets.Field)]
	public class DefAliasAttribute : Attribute
	{
		// Token: 0x06006051 RID: 24657 RVA: 0x00216AF8 File Offset: 0x00214CF8
		public DefAliasAttribute(string defName)
		{
			this.defName = defName;
		}

		// Token: 0x04003431 RID: 13361
		public string defName;
	}
}
