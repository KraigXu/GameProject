using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001013 RID: 4115
	public class StatPart_WorkTableTemperature : StatPart
	{
		// Token: 0x0600626F RID: 25199 RVA: 0x00221C30 File Offset: 0x0021FE30
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && StatPart_WorkTableTemperature.Applies(req.Thing))
			{
				val *= 0.7f;
			}
		}

		// Token: 0x06006270 RID: 25200 RVA: 0x00221C54 File Offset: 0x0021FE54
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && StatPart_WorkTableTemperature.Applies(req.Thing))
			{
				return "BadTemperature".Translate().CapitalizeFirst() + ": x" + 0.7f.ToStringPercent();
			}
			return null;
		}

		// Token: 0x06006271 RID: 25201 RVA: 0x00221CAA File Offset: 0x0021FEAA
		public static bool Applies(Thing t)
		{
			return t.Spawned && StatPart_WorkTableTemperature.Applies(t.def, t.Map, t.Position);
		}

		// Token: 0x06006272 RID: 25202 RVA: 0x00221CD0 File Offset: 0x0021FED0
		public static bool Applies(ThingDef tDef, Map map, IntVec3 c)
		{
			if (map == null)
			{
				return false;
			}
			if (tDef.building == null)
			{
				return false;
			}
			float temperatureForCell = GenTemperature.GetTemperatureForCell(c, map);
			return temperatureForCell < 9f || temperatureForCell > 35f;
		}

		// Token: 0x04003C01 RID: 15361
		public const float WorkRateFactor = 0.7f;

		// Token: 0x04003C02 RID: 15362
		public const float MinTemp = 9f;

		// Token: 0x04003C03 RID: 15363
		public const float MaxTemp = 35f;
	}
}
