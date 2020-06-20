using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000800 RID: 2048
	public class ThoughtWorker_NeedRoomSize : ThoughtWorker
	{
		// Token: 0x06003407 RID: 13319 RVA: 0x0011E788 File Offset: 0x0011C988
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.needs.roomsize == null)
			{
				return ThoughtState.Inactive;
			}
			Room room = p.GetRoom(RegionType.Set_Passable);
			if (room == null || room.PsychologicallyOutdoors)
			{
				return ThoughtState.Inactive;
			}
			switch (p.needs.roomsize.CurCategory)
			{
			case RoomSizeCategory.VeryCramped:
				return ThoughtState.ActiveAtStage(0);
			case RoomSizeCategory.Cramped:
				return ThoughtState.ActiveAtStage(1);
			case RoomSizeCategory.Normal:
				return ThoughtState.Inactive;
			case RoomSizeCategory.Spacious:
				return ThoughtState.ActiveAtStage(2);
			default:
				throw new InvalidOperationException("Unknown RoomSizeCategory");
			}
		}
	}
}
