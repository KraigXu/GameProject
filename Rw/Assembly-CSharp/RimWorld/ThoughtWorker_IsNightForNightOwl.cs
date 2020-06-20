using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200083E RID: 2110
	public class ThoughtWorker_IsNightForNightOwl : ThoughtWorker
	{
		// Token: 0x0600348C RID: 13452 RVA: 0x001201C5 File Offset: 0x0011E3C5
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && (GenLocalDate.HourInteger(p) >= 23 || GenLocalDate.HourInteger(p) <= 5);
		}
	}
}
