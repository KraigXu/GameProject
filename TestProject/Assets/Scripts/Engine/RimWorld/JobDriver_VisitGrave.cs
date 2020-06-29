using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_VisitGrave : JobDriver_VisitJoyThing
	{
		
		// (get) Token: 0x06002C21 RID: 11297 RVA: 0x000FCD70 File Offset: 0x000FAF70
		private Building_Grave Grave
		{
			get
			{
				return (Building_Grave)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		
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
