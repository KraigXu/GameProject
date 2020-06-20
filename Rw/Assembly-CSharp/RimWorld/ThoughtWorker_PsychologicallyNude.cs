using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000833 RID: 2099
	public class ThoughtWorker_PsychologicallyNude : ThoughtWorker
	{
		// Token: 0x06003475 RID: 13429 RVA: 0x0011FEEC File Offset: 0x0011E0EC
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return p.apparel.PsychologicallyNude;
		}
	}
}
