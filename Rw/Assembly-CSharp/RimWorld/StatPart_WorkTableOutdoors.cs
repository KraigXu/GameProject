using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001012 RID: 4114
	public class StatPart_WorkTableOutdoors : StatPart
	{
		// Token: 0x0600626A RID: 25194 RVA: 0x00221B74 File Offset: 0x0021FD74
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && StatPart_WorkTableOutdoors.Applies(req.Thing))
			{
				val *= 0.9f;
			}
		}

		// Token: 0x0600626B RID: 25195 RVA: 0x00221B98 File Offset: 0x0021FD98
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && StatPart_WorkTableOutdoors.Applies(req.Thing))
			{
				return "Outdoors".Translate() + ": x" + 0.9f.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600626C RID: 25196 RVA: 0x00221BE6 File Offset: 0x0021FDE6
		public static bool Applies(Thing t)
		{
			return StatPart_WorkTableOutdoors.Applies(t.def, t.Map, t.Position);
		}

		// Token: 0x0600626D RID: 25197 RVA: 0x00221C00 File Offset: 0x0021FE00
		public static bool Applies(ThingDef def, Map map, IntVec3 c)
		{
			if (def.building == null)
			{
				return false;
			}
			if (map == null)
			{
				return false;
			}
			Room room = c.GetRoom(map, RegionType.Set_All);
			return room != null && room.PsychologicallyOutdoors;
		}

		// Token: 0x04003C00 RID: 15360
		public const float WorkRateFactor = 0.9f;
	}
}
