using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000655 RID: 1621
	public class JobDriver_ApplyTechprint : JobDriver
	{
		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x06002C3A RID: 11322 RVA: 0x000FD228 File Offset: 0x000FB428
		protected Building_ResearchBench ResearchBench
		{
			get
			{
				return (Building_ResearchBench)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x06002C3B RID: 11323 RVA: 0x000FD250 File Offset: 0x000FB450
		protected Thing Techprint
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x06002C3C RID: 11324 RVA: 0x000FD271 File Offset: 0x000FB471
		protected CompTechprint TechprintComp
		{
			get
			{
				return this.Techprint.TryGetComp<CompTechprint>();
			}
		}

		// Token: 0x06002C3D RID: 11325 RVA: 0x000FD280 File Offset: 0x000FB480
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.ResearchBench, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.Techprint, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002C3E RID: 11326 RVA: 0x000FD2D1 File Offset: 0x000FB4D1
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.A);
			yield return Toils_General.DoAtomic(delegate
			{
				this.job.count = 1;
			});
			Toil toil = Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
			yield return toil;
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
			yield return Toils_Haul.StartCarryThing(TargetIndex.B, false, true, false).FailOnDestroyedNullOrForbidden(TargetIndex.B);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, null, false, false);
			yield return Toils_General.Wait(600, TargetIndex.None).FailOnDestroyedNullOrForbidden(TargetIndex.B).FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return new Toil
			{
				initAction = delegate
				{
					Find.ResearchManager.ApplyTechprint(this.TechprintComp.Props.project, this.pawn);
					this.Techprint.Destroy(DestroyMode.Vanish);
					SoundDefOf.TechprintApplied.PlayOneShotOnCamera(null);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
			yield break;
		}

		// Token: 0x040019CC RID: 6604
		private const TargetIndex ResearchBenchInd = TargetIndex.A;

		// Token: 0x040019CD RID: 6605
		private const TargetIndex TechprintInd = TargetIndex.B;

		// Token: 0x040019CE RID: 6606
		private const TargetIndex HaulCell = TargetIndex.C;

		// Token: 0x040019CF RID: 6607
		private const int Duration = 600;
	}
}
