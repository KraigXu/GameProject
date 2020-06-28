using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000842 RID: 2114
	public class ThoughtWorker_HasAddedBodyPart : ThoughtWorker
	{
		// Token: 0x06003494 RID: 13460 RVA: 0x001203C8 File Offset: 0x0011E5C8
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			int num = p.health.hediffSet.CountAddedAndImplantedParts();
			if (num > 0)
			{
				return ThoughtState.ActiveAtStage(num - 1);
			}
			return false;
		}
	}
}
