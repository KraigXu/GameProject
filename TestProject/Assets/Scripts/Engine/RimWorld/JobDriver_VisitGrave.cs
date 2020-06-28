using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000650 RID: 1616
	public class JobDriver_VisitGrave : JobDriver_VisitJoyThing
	{
		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x06002C21 RID: 11297 RVA: 0x000FCD70 File Offset: 0x000FAF70
		private Building_Grave Grave
		{
			get
			{
				return (Building_Grave)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06002C22 RID: 11298 RVA: 0x000FCD98 File Offset: 0x000FAF98
		protected override void WaitTickAction()
		{
			float num = 1f;
			Room room = this.pawn.GetRoom(RegionType.Set_Passable);
			if (room != null)
			{
				num *= room.GetStat(RoomStatDefOf.GraveVisitingJoyGainFactor);
			}
			this.pawn.GainComfortFromCellIfPossible(false);
			JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.EndJob, num, this.Grave);
		}

		// Token: 0x06002C23 RID: 11299 RVA: 0x000FCDE8 File Offset: 0x000FAFE8
		public override object[] TaleParameters()
		{
			return new object[]
			{
				this.pawn,
				(this.Grave.Corpse != null) ? this.Grave.Corpse.InnerPawn : null
			};
		}
	}
}
