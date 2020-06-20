using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000805 RID: 2053
	public class ThoughtWorker_HospitalPatientRoomStats : ThoughtWorker
	{
		// Token: 0x06003411 RID: 13329 RVA: 0x0011E9AC File Offset: 0x0011CBAC
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Building_Bed building_Bed = p.CurrentBed();
			if (building_Bed == null || !building_Bed.Medical)
			{
				return ThoughtState.Inactive;
			}
			Room room = p.GetRoom(RegionType.Set_Passable);
			if (room == null || room.Role != RoomRoleDefOf.Hospital)
			{
				return ThoughtState.Inactive;
			}
			int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(room.GetStat(RoomStatDefOf.Impressiveness));
			if (this.def.stages[scoreStageIndex] != null)
			{
				return ThoughtState.ActiveAtStage(scoreStageIndex);
			}
			return ThoughtState.Inactive;
		}
	}
}
