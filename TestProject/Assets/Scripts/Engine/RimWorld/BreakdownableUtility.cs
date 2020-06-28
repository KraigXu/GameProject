using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CF8 RID: 3320
	public static class BreakdownableUtility
	{
		// Token: 0x060050B5 RID: 20661 RVA: 0x001B1C3C File Offset: 0x001AFE3C
		public static bool IsBrokenDown(this Thing t)
		{
			CompBreakdownable compBreakdownable = t.TryGetComp<CompBreakdownable>();
			return compBreakdownable != null && compBreakdownable.BrokenDown;
		}
	}
}
