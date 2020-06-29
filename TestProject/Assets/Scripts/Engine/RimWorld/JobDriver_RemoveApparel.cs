using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_RemoveApparel : JobDriver
	{
		
		
		private Apparel Apparel
		{
			get
			{
				return (Apparel)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
		}

		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.duration = (int)(this.Apparel.GetStatValue(StatDefOf.EquipDelay, true) * 60f);
		}

		
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

		
		private int duration;

		
		private const TargetIndex ApparelInd = TargetIndex.A;
	}
}
