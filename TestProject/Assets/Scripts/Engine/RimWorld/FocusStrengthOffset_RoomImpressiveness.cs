using System;
using Verse;

namespace RimWorld
{
	
	public class FocusStrengthOffset_RoomImpressiveness : FocusStrengthOffset_Curve
	{
		
		protected override float SourceValue(Thing parent)
		{
			Room room = parent.GetRoom(RegionType.Set_Passable);
			if (room == null)
			{
				return 0f;
			}
			return room.GetStat(RoomStatDefOf.Impressiveness);
		}

		
		
		protected override string ExplanationKey
		{
			get
			{
				return "StatsReport_RoomImpressiveness";
			}
		}
	}
}
