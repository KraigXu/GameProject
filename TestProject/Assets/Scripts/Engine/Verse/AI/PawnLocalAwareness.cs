using System;

namespace Verse.AI
{
	// Token: 0x02000582 RID: 1410
	public static class PawnLocalAwareness
	{
		// Token: 0x06002820 RID: 10272 RVA: 0x000ED1C4 File Offset: 0x000EB3C4
		public static bool AnimalAwareOf(this Pawn p, Thing t)
		{
			return p.RaceProps.ToolUser || p.Faction != null || ((float)(p.Position - t.Position).LengthHorizontalSquared <= 900f && p.GetRoom(RegionType.Set_Passable) == t.GetRoom(RegionType.Set_Passable) && GenSight.LineOfSight(p.Position, t.Position, p.Map, false, null, 0, 0));
		}

		// Token: 0x04001834 RID: 6196
		private const float SightRadius = 30f;
	}
}
