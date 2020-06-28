using System;

namespace RimWorld
{
	// Token: 0x02000F4D RID: 3917
	[AttributeUsage(AttributeTargets.Field)]
	public class MayRequireAttribute : Attribute
	{
		// Token: 0x06006052 RID: 24658 RVA: 0x00216B07 File Offset: 0x00214D07
		public MayRequireAttribute(string modId)
		{
			this.modId = modId;
		}

		// Token: 0x04003432 RID: 13362
		public string modId;
	}
}
