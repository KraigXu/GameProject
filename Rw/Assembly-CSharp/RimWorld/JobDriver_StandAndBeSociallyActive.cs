using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000640 RID: 1600
	public class JobDriver_StandAndBeSociallyActive : JobDriver
	{
		// Token: 0x06002BCA RID: 11210 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06002BCB RID: 11211 RVA: 0x000FBBF8 File Offset: 0x000F9DF8
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return new Toil
			{
				tickAction = delegate
				{
					Pawn pawn = this.FindClosePawn();
					if (pawn != null)
					{
						this.pawn.rotationTracker.FaceCell(pawn.Position);
					}
					this.pawn.GainComfortFromCellIfPossible(false);
				},
				socialMode = RandomSocialMode.SuperActive,
				defaultCompleteMode = ToilCompleteMode.Never,
				handlingFacing = true
			};
			yield break;
		}

		// Token: 0x06002BCC RID: 11212 RVA: 0x000FBC08 File Offset: 0x000F9E08
		private Pawn FindClosePawn()
		{
			IntVec3 position = this.pawn.Position;
			for (int i = 0; i < 24; i++)
			{
				IntVec3 intVec = position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(base.Map))
				{
					Thing thing = intVec.GetThingList(base.Map).Find((Thing x) => x is Pawn);
					if (thing != null && thing != this.pawn && GenSight.LineOfSight(position, intVec, base.Map, false, null, 0, 0))
					{
						return (Pawn)thing;
					}
				}
			}
			return null;
		}
	}
}
