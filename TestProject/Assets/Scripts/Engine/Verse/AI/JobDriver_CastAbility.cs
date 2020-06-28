using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000520 RID: 1312
	public class JobDriver_CastAbility : JobDriver_CastVerbOnce
	{
		// Token: 0x06002579 RID: 9593 RVA: 0x000DE4D9 File Offset: 0x000DC6D9
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return new Toil
			{
				initAction = delegate
				{
					this.pawn.pather.StopDead();
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield return Toils_Combat.CastVerb(TargetIndex.A, TargetIndex.B, false);
			yield break;
		}
	}
}
