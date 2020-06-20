using System;

namespace RimWorld
{
	// Token: 0x02000FA5 RID: 4005
	[DefOf]
	public static class WorkGiverDefOf
	{
		// Token: 0x060060AC RID: 24748 RVA: 0x002172CC File Offset: 0x002154CC
		static WorkGiverDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(WorkGiverDefOf));
		}

		// Token: 0x04003AAA RID: 15018
		public static WorkGiverDef Refuel;

		// Token: 0x04003AAB RID: 15019
		public static WorkGiverDef Repair;
	}
}
