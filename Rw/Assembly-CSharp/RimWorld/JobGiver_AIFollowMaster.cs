using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006B3 RID: 1715
	public class JobGiver_AIFollowMaster : JobGiver_AIFollowPawn
	{
		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x06002E54 RID: 11860 RVA: 0x000FAF75 File Offset: 0x000F9175
		protected override int FollowJobExpireInterval
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x06002E55 RID: 11861 RVA: 0x00104474 File Offset: 0x00102674
		protected override Pawn GetFollowee(Pawn pawn)
		{
			if (pawn.playerSettings == null)
			{
				return null;
			}
			return pawn.playerSettings.Master;
		}

		// Token: 0x06002E56 RID: 11862 RVA: 0x0010448B File Offset: 0x0010268B
		protected override float GetRadius(Pawn pawn)
		{
			if (pawn.playerSettings.Master.playerSettings.animalsReleased && pawn.training.HasLearned(TrainableDefOf.Release))
			{
				return 50f;
			}
			return 3f;
		}

		// Token: 0x04001A64 RID: 6756
		public const float RadiusUnreleased = 3f;

		// Token: 0x04001A65 RID: 6757
		public const float RadiusReleased = 50f;
	}
}
