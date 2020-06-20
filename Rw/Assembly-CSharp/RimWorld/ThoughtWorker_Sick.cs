using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200082E RID: 2094
	public class ThoughtWorker_Sick : ThoughtWorker
	{
		// Token: 0x0600346B RID: 13419 RVA: 0x0011FCEC File Offset: 0x0011DEEC
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.health.hediffSet.AnyHediffMakesSickThought;
		}
	}
}
