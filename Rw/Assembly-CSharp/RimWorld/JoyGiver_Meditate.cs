using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000709 RID: 1801
	public class JoyGiver_Meditate : JoyGiver_InPrivateRoom
	{
		// Token: 0x06002F99 RID: 12185 RVA: 0x0010C39E File Offset: 0x0010A59E
		public override Job TryGiveJobWhileInBed(Pawn pawn)
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return base.TryGiveJobWhileInBed(pawn);
			}
			return MeditationUtility.GetMeditationJob(pawn, true);
		}

		// Token: 0x06002F9A RID: 12186 RVA: 0x0010C3B6 File Offset: 0x0010A5B6
		public override Job TryGiveJob(Pawn pawn)
		{
			if (ModsConfig.RoyaltyActive)
			{
				return MeditationUtility.GetMeditationJob(pawn, true);
			}
			return base.TryGiveJob(pawn);
		}
	}
}
