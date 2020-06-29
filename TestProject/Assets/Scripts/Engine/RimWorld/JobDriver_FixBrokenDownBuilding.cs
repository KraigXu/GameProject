using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_FixBrokenDownBuilding : JobDriver
	{
		
		
		private Building Building
		{
			get
			{
				return (Building)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		
		
		private Thing Components
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Building, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.Components, this.job, 1, -1, null, errorOnFailed);
		}

		
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

		
		private const TargetIndex BuildingInd = TargetIndex.A;

		
		private const TargetIndex ComponentInd = TargetIndex.B;

		
		private const int TicksDuration = 1000;
	}
}
