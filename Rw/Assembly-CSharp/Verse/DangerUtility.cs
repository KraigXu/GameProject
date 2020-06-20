using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000053 RID: 83
	public static class DangerUtility
	{
		// Token: 0x060003B7 RID: 951 RVA: 0x00013554 File Offset: 0x00011754
		public static Danger NormalMaxDanger(this Pawn p)
		{
			if (p.CurJob != null && p.CurJob.playerForced)
			{
				return Danger.Deadly;
			}
			if (FloatMenuMakerMap.makingFor == p)
			{
				return Danger.Deadly;
			}
			if (p.Faction != Faction.OfPlayer)
			{
				return Danger.Some;
			}
			if (p.health.hediffSet.HasTemperatureInjury(TemperatureInjuryStage.Minor) && GenTemperature.FactionOwnsPassableRoomInTemperatureRange(p.Faction, p.SafeTemperatureRange(), p.MapHeld))
			{
				return Danger.None;
			}
			return Danger.Some;
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x000135C0 File Offset: 0x000117C0
		public static Danger GetDangerFor(this IntVec3 c, Pawn p, Map map)
		{
			Map mapHeld = p.MapHeld;
			if (mapHeld == null || mapHeld != map)
			{
				return Danger.None;
			}
			Region region = c.GetRegion(mapHeld, RegionType.Set_All);
			if (region == null)
			{
				return Danger.None;
			}
			return region.DangerFor(p);
		}
	}
}
