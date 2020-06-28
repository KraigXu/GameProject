using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000841 RID: 2113
	public class ThoughtWorker_Ascetic : ThoughtWorker
	{
		// Token: 0x06003492 RID: 13458 RVA: 0x00120360 File Offset: 0x0011E560
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.IsColonist)
			{
				return false;
			}
			Room ownedRoom = p.ownership.OwnedRoom;
			if (ownedRoom == null)
			{
				return false;
			}
			int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(ownedRoom.GetStat(RoomStatDefOf.Impressiveness));
			if (this.def.stages[scoreStageIndex] != null)
			{
				return ThoughtState.ActiveAtStage(scoreStageIndex);
			}
			return ThoughtState.Inactive;
		}
	}
}
