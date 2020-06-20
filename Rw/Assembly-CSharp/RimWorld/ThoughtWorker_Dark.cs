using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000829 RID: 2089
	public class ThoughtWorker_Dark : ThoughtWorker
	{
		// Token: 0x0600345F RID: 13407 RVA: 0x0011FA50 File Offset: 0x0011DC50
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.Awake() && p.needs.mood.recentMemory.TicksSinceLastLight > 240;
		}
	}
}
