using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200063E RID: 1598
	public class JobDriver_MarryAdjacentPawn : JobDriver
	{
		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x06002BBB RID: 11195 RVA: 0x000FBA78 File Offset: 0x000F9C78
		private Pawn OtherFiance
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x06002BBC RID: 11196 RVA: 0x000FBA9E File Offset: 0x000F9C9E
		public int TicksLeftToMarry
		{
			get
			{
				return this.ticksLeftToMarry;
			}
		}

		// Token: 0x06002BBD RID: 11197 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06002BBE RID: 11198 RVA: 0x000FBAA6 File Offset: 0x000F9CA6
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

		// Token: 0x06002BBF RID: 11199 RVA: 0x000FBAB6 File Offset: 0x000F9CB6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeftToMarry, "ticksLeftToMarry", 0, false);
		}

		// Token: 0x040019AE RID: 6574
		private int ticksLeftToMarry = 2500;

		// Token: 0x040019AF RID: 6575
		private const TargetIndex OtherFianceInd = TargetIndex.A;

		// Token: 0x040019B0 RID: 6576
		private const int Duration = 2500;
	}
}
