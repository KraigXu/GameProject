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

		
		// (get) Token: 0x060051D9 RID: 20953 RVA: 0x001B60E5 File Offset: 0x001B42E5
		protected override string ExplanationKey
		{
			get
			{
				return "StatsReport_RoomImpressiveness";
			}
		}
	}
}
