using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000634 RID: 1588
	public class JobDriver_PlaceNoCostFrame : JobDriver
	{
		// Token: 0x06002B7F RID: 11135 RVA: 0x000DDBC6 File Offset: 0x000DBDC6
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002B80 RID: 11136 RVA: 0x000FB45A File Offset: 0x000F965A
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_Goto.MoveOffTargetBlueprint(TargetIndex.A);
			yield return Toils_Construct.MakeSolidThingFromBlueprintIfNecessary(TargetIndex.A, TargetIndex.None);
			yield break;
		}
	}
}
