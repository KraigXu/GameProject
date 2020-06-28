using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000633 RID: 1587
	public class JobDriver_FixBrokenDownBuilding : JobDriver
	{
		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x06002B79 RID: 11129 RVA: 0x000FB324 File Offset: 0x000F9524
		private Building Building
		{
			get
			{
				return (Building)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x06002B7A RID: 11130 RVA: 0x000FB34C File Offset: 0x000F954C
		private Thing Components
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06002B7B RID: 11131 RVA: 0x000FB370 File Offset: 0x000F9570
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Building, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.Components, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002B7C RID: 11132 RVA: 0x000FB3C1 File Offset: 0x000F95C1
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, false, false);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
			Toil toil = Toils_General.Wait(1000, TargetIndex.None);
			toil.FailOnDespawnedOrNull(TargetIndex.A);
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			toil.WithEffect(this.Building.def.repairEffect, TargetIndex.A);
			toil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return toil;
			yield return new Toil
			{
				initAction = delegate
				{
					this.Components.Destroy(DestroyMode.Vanish);
					if (Rand.Value > this.pawn.GetStatValue(StatDefOf.FixBrokenDownBuildingSuccessChance, true))
					{
						MoteMaker.ThrowText((this.pawn.DrawPos + this.Building.DrawPos) / 2f, base.Map, "TextMote_FixBrokenDownBuildingFail".Translate(), 3.65f);
						return;
					}
					this.Building.GetComp<CompBreakdownable>().Notify_Repaired();
				}
			};
			yield break;
		}

		// Token: 0x040019A0 RID: 6560
		private const TargetIndex BuildingInd = TargetIndex.A;

		// Token: 0x040019A1 RID: 6561
		private const TargetIndex ComponentInd = TargetIndex.B;

		// Token: 0x040019A2 RID: 6562
		private const int TicksDuration = 1000;
	}
}
