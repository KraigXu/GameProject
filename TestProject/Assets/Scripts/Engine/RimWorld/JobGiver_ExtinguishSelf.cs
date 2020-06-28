using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020006DB RID: 1755
	public class JobGiver_ExtinguishSelf : ThinkNode_JobGiver
	{
		// Token: 0x06002ECF RID: 11983 RVA: 0x00106F20 File Offset: 0x00105120
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (Rand.Value < 0.1f)
			{
				Fire fire = (Fire)pawn.GetAttachment(ThingDefOf.Fire);
				if (fire != null)
				{
					return JobMaker.MakeJob(JobDefOf.ExtinguishSelf, fire);
				}
			}
			return null;
		}

		// Token: 0x04001A8B RID: 6795
		private const float ActivateChance = 0.1f;
	}
}
