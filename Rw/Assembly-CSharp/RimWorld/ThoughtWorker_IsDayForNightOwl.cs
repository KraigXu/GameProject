using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200083D RID: 2109
	public class ThoughtWorker_IsDayForNightOwl : ThoughtWorker
	{
		// Token: 0x0600348A RID: 13450 RVA: 0x0012019C File Offset: 0x0011E39C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && GenLocalDate.HourInteger(p) >= 11 && GenLocalDate.HourInteger(p) <= 17;
		}
	}
}
