using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_UnloadInventory : JobDriver
	{
		
		
		private Pawn OtherPawn
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.OtherPawn, this.job, 1, -1, null, errorOnFailed);
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			yield return Toils_General.Wait(10, TargetIndex.None);
			yield return new Toil
			{
				initAction = delegate
				{
					Pawn otherPawn = this.OtherPawn;
					if (!otherPawn.inventory.UnloadEverything)
					{
						base.EndJobWith(JobCondition.Succeeded);
						return;
					}
					ThingCount firstUnloadableThing = otherPawn.inventory.FirstUnloadableThing;
					IntVec3 c;
					if (!firstUnloadableThing.Thing.def.EverStorable(false) || !this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) || !StoreUtility.TryFindStoreCellNearColonyDesperate(firstUnloadableThing.Thing, this.pawn, out c))
					{
						Thing thing;
						otherPawn.inventory.innerContainer.TryDrop(firstUnloadableThing.Thing, ThingPlaceMode.Near, firstUnloadableThing.Count, out thing, null, null);
						base.EndJobWith(JobCondition.Succeeded);
						if (thing != null)
						{
							thing.SetForbidden(false, false);
							return;
						}
					}
					else
					{
						Thing thing2;
						otherPawn.inventory.innerContainer.TryTransferToContainer(firstUnloadableThing.Thing, this.pawn.carryTracker.innerContainer, firstUnloadableThing.Count, out thing2, true);
						this.job.count = thing2.stackCount;
						this.job.SetTarget(TargetIndex.B, thing2);
						this.job.SetTarget(TargetIndex.C, c);
						firstUnloadableThing.Thing.SetForbidden(false, false);
					}
				}
			};
			yield return Toils_Reserve.Reserve(TargetIndex.C, 1, -1, null);
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.C);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, carryToCell, true, false);
			yield break;
		}

		
		private const TargetIndex OtherPawnInd = TargetIndex.A;

		
		private const TargetIndex ItemToHaulInd = TargetIndex.B;

		
		private const TargetIndex StoreCellInd = TargetIndex.C;

		
		private const int UnloadDuration = 10;
	}
}
