using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200083F RID: 2111
	public class ThoughtWorker_Greedy : ThoughtWorker
	{
		// Token: 0x0600348E RID: 13454 RVA: 0x001201F0 File Offset: 0x0011E3F0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.IsColonist)
			{
				return false;
			}
			Room ownedRoom = p.ownership.OwnedRoom;
			if (ownedRoom == null)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			int num = RoomStatDefOf.Impressiveness.GetScoreStageIndex(ownedRoom.GetStat(RoomStatDefOf.Impressiveness)) + 1;
			if (this.def.stages[num] != null)
			{
				return ThoughtState.ActiveAtStage(num);
			}
			return ThoughtState.Inactive;
		}
	}
}
