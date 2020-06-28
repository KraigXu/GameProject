using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000837 RID: 2103
	public class ThoughtWorker_Pain : ThoughtWorker
	{
		// Token: 0x0600347D RID: 13437 RVA: 0x0011FFEC File Offset: 0x0011E1EC
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			float painTotal = p.health.hediffSet.PainTotal;
			if (painTotal < 0.0001f)
			{
				return ThoughtState.Inactive;
			}
			if (painTotal < 0.15f)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			if (painTotal < 0.4f)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (painTotal < 0.8f)
			{
				return ThoughtState.ActiveAtStage(2);
			}
			return ThoughtState.ActiveAtStage(3);
		}
	}
}
