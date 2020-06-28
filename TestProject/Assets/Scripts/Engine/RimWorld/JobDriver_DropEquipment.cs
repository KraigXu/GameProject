using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000694 RID: 1684
	public class JobDriver_DropEquipment : JobDriver
	{
		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x06002DCF RID: 11727 RVA: 0x00102088 File Offset: 0x00100288
		private ThingWithComps TargetEquipment
		{
			get
			{
				return (ThingWithComps)base.TargetA.Thing;
			}
		}

		// Token: 0x06002DD0 RID: 11728 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06002DD1 RID: 11729 RVA: 0x001020A8 File Offset: 0x001002A8
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			yield return new Toil
			{
				initAction = delegate
				{
					this.pawn.pather.StopDead();
				},
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = 30
			};
			yield return new Toil
			{
				initAction = delegate
				{
					ThingWithComps thingWithComps;
					if (!this.pawn.equipment.TryDropEquipment(this.TargetEquipment, out thingWithComps, this.pawn.Position, true))
					{
						base.EndJobWith(JobCondition.Incompletable);
					}
				}
			};
			yield break;
		}

		// Token: 0x04001A40 RID: 6720
		private const int DurationTicks = 30;
	}
}
