using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000802 RID: 2050
	public abstract class ThoughtWorker_RoomImpressiveness : ThoughtWorker
	{
		// Token: 0x0600340B RID: 13323 RVA: 0x0011E8A4 File Offset: 0x0011CAA4
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.story.traits.HasTrait(TraitDefOf.Ascetic))
			{
				return ThoughtState.Inactive;
			}
			Room room = p.GetRoom(RegionType.Set_Passable);
			if (room == null)
			{
				return ThoughtState.Inactive;
			}
			int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(room.GetStat(RoomStatDefOf.Impressiveness));
			if (this.def.stages[scoreStageIndex] == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(scoreStageIndex);
		}
	}
}
