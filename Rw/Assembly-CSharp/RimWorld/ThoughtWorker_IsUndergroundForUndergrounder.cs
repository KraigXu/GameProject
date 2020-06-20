using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200083B RID: 2107
	public class ThoughtWorker_IsUndergroundForUndergrounder : ThoughtWorker
	{
		// Token: 0x06003486 RID: 13446 RVA: 0x00120144 File Offset: 0x0011E344
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			bool flag;
			return ThoughtWorker_IsIndoorsForUndergrounder.IsAwakeAndIndoors(p, out flag) && flag;
		}
	}
}
