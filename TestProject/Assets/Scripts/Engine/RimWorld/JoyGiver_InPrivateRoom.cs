using System;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200070A RID: 1802
	public class JoyGiver_InPrivateRoom : JoyGiver
	{
		// Token: 0x06002F9C RID: 12188 RVA: 0x0010C3D8 File Offset: 0x0010A5D8
		public override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.ownership == null)
			{
				return null;
			}
			Room ownedRoom = pawn.ownership.OwnedRoom;
			if (ownedRoom == null)
			{
				return null;
			}
			IntVec3 c2;
			if (!(from c in ownedRoom.Cells
			where c.Standable(pawn.Map) && !c.IsForbidden(pawn) && pawn.CanReserveAndReach(c, PathEndMode.OnCell, Danger.None, 1, -1, null, false)
			select c).TryRandomElement(out c2))
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, c2);
		}

		// Token: 0x06002F9D RID: 12189 RVA: 0x0010C44F File Offset: 0x0010A64F
		public override Job TryGiveJobWhileInBed(Pawn pawn)
		{
			return JobMaker.MakeJob(this.def.jobDef, pawn.CurrentBed());
		}
	}
}
