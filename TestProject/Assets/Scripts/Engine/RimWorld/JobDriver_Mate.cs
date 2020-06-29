using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_Mate : JobDriver
	{
		
		
		private Pawn Female
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOnDowned(TargetIndex.A);
			this.FailOnNotCasualInterruptible(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
			Toil toil = Toils_General.WaitWith(TargetIndex.A, 500, false, false);
			toil.tickAction = delegate
			{
				if (this.pawn.IsHashIntervalTick(100))
				{
					MoteMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, ThingDefOf.Mote_Heart);
				}
				if (this.Female.IsHashIntervalTick(100))
				{
					MoteMaker.ThrowMetaIcon(this.Female.Position, this.pawn.Map, ThingDefOf.Mote_Heart);
				}
			};
			yield return toil;
			yield return Toils_General.Do(delegate
			{
				PawnUtility.Mated(this.pawn, this.Female);
			});
			yield break;
		}

		
		private const int MateDuration = 500;

		
		private const TargetIndex FemInd = TargetIndex.A;

		
		private const int TicksBetweenHeartMotes = 100;
	}
}
