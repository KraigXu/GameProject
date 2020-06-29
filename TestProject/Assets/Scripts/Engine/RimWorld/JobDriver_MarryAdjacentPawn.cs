using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_MarryAdjacentPawn : JobDriver
	{
		
		
		private Pawn OtherFiance
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		
		
		public int TicksLeftToMarry
		{
			get
			{
				return this.ticksLeftToMarry;
			}
		}

		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOn(() => this.OtherFiance.Drafted || !this.pawn.Position.AdjacentTo8WayOrInside(this.OtherFiance));
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				this.ticksLeftToMarry = 2500;
			};
			toil.tickAction = delegate
			{
				this.ticksLeftToMarry--;
				if (this.ticksLeftToMarry <= 0)
				{
					this.ticksLeftToMarry = 0;
					base.ReadyForNextToil();
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			toil.FailOn(() => !this.pawn.relations.DirectRelationExists(PawnRelationDefOf.Fiance, this.OtherFiance));
			yield return toil;
			yield return new Toil
			{
				defaultCompleteMode = ToilCompleteMode.Instant,
				initAction = delegate
				{
					if (this.pawn.thingIDNumber < this.OtherFiance.thingIDNumber)
					{
						MarriageCeremonyUtility.Married(this.pawn, this.OtherFiance);
					}
				}
			};
			yield break;
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeftToMarry, "ticksLeftToMarry", 0, false);
		}

		
		private int ticksLeftToMarry = 2500;

		
		private const TargetIndex OtherFianceInd = TargetIndex.A;

		
		private const int Duration = 2500;
	}
}
