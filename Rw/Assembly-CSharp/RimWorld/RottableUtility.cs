using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D47 RID: 3399
	public static class RottableUtility
	{
		// Token: 0x060052A4 RID: 21156 RVA: 0x001B9DD8 File Offset: 0x001B7FD8
		public static bool IsNotFresh(this Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			return compRottable != null && compRottable.Stage > RotStage.Fresh;
		}

		// Token: 0x060052A5 RID: 21157 RVA: 0x001B9DFC File Offset: 0x001B7FFC
		public static bool IsDessicated(this Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			return compRottable != null && compRottable.Stage == RotStage.Dessicated;
		}

		// Token: 0x060052A6 RID: 21158 RVA: 0x001B9E20 File Offset: 0x001B8020
		public static RotStage GetRotStage(this Thing t)
		{
			CompRottable compRottable = t.TryGetComp<CompRottable>();
			if (compRottable == null)
			{
				return RotStage.Fresh;
			}
			return compRottable.Stage;
		}
	}
}
