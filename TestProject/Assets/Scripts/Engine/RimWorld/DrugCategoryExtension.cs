using System;

namespace RimWorld
{
	// Token: 0x02000890 RID: 2192
	public static class DrugCategoryExtension
	{
		// Token: 0x06003551 RID: 13649 RVA: 0x00123333 File Offset: 0x00121533
		public static bool IncludedIn(this DrugCategory lhs, DrugCategory rhs)
		{
			return lhs <= rhs;
		}
	}
}
