using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000695 RID: 1685
	public class JobDriver_RemoveApparel : JobDriver
	{
		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x06002DD5 RID: 11733 RVA: 0x001020F4 File Offset: 0x001002F4
		private Apparel Apparel
		{
			get
			{
				return (Apparel)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06002DD6 RID: 11734 RVA: 0x0010211A File Offset: 0x0010031A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
		}

		// Token: 0x06002DD7 RID: 11735 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06002DD8 RID: 11736 RVA: 0x00102134 File Offset: 0x00100334
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.duration = (int)(this.Apparel.GetStatValue(StatDefOf.EquipDelay, true) * 60f);
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x0010215A File Offset: 0x0010035A
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			yield return Toils_General.Wait(this.duration, TargetIndex.None).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return Toils_General.Do(delegate
			{
				if (!this.pawn.apparel.WornApparel.Contains(this.Apparel))
				{
					base.EndJobWith(JobCondition.Incompletable);
					return;
				}
				Apparel apparel;
				if (!this.pawn.apparel.TryDrop(this.Apparel, out apparel))
				{
					base.EndJobWith(JobCondition.Incompletable);
					return;
				}
				this.job.targetA = apparel;
				if (!this.job.haulDroppedApparel)
				{
					base.EndJobWith(JobCondition.Succeeded);
					return;
				}
				apparel.SetForbidden(false, false);
				StoragePriority currentPriority = StoreUtility.CurrentStoragePriorityOf(apparel);
				IntVec3 c;
				if (StoreUtility.TryFindBestBetterStoreCellFor(apparel, this.pawn, base.Map, currentPriority, this.pawn.Faction, out c, true))
				{
					this.job.count = apparel.stackCount;
					this.job.targetB = c;
					return;
				}
				base.EndJobWith(JobCondition.Incompletable);
			});
			if (this.job.haulDroppedApparel)
			{
				yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null);
				yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null);
				yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false).FailOn(() => !this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation));
				Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B);
				yield return carryToCell;
				yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, true, false);
				carryToCell = null;
			}
			yield break;
		}

		// Token: 0x04001A41 RID: 6721
		private int duration;

		// Token: 0x04001A42 RID: 6722
		private const TargetIndex ApparelInd = TargetIndex.A;
	}
}
