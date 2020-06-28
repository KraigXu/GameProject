using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001014 RID: 4116
	public class StatPart_WorkTableUnpowered : StatPart
	{
		// Token: 0x06006274 RID: 25204 RVA: 0x00221D06 File Offset: 0x0021FF06
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && StatPart_WorkTableUnpowered.Applies(req.Thing))
			{
				val *= req.Thing.def.building.unpoweredWorkTableWorkSpeedFactor;
			}
		}

		// Token: 0x06006275 RID: 25205 RVA: 0x00221D3C File Offset: 0x0021FF3C
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && StatPart_WorkTableUnpowered.Applies(req.Thing))
			{
				float unpoweredWorkTableWorkSpeedFactor = req.Thing.def.building.unpoweredWorkTableWorkSpeedFactor;
				return "NoPower".Translate() + ": x" + unpoweredWorkTableWorkSpeedFactor.ToStringPercent();
			}
			return null;
		}

		// Token: 0x06006276 RID: 25206 RVA: 0x00221DA0 File Offset: 0x0021FFA0
		public static bool Applies(Thing th)
		{
			if (th.def.building.unpoweredWorkTableWorkSpeedFactor == 0f)
			{
				return false;
			}
			CompPowerTrader compPowerTrader = th.TryGetComp<CompPowerTrader>();
			return compPowerTrader != null && !compPowerTrader.PowerOn;
		}
	}
}
