using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D25 RID: 3365
	public class FocusStrengthOffset_RoomImpressiveness : FocusStrengthOffset_Curve
	{
		// Token: 0x060051D8 RID: 20952 RVA: 0x001B60BC File Offset: 0x001B42BC
		protected override float SourceValue(Thing parent)
		{
			Room room = parent.GetRoom(RegionType.Set_Passable);
			if (room == null)
			{
				return 0f;
			}
			return room.GetStat(RoomStatDefOf.Impressiveness);
		}

		// Token: 0x17000E70 RID: 3696
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
