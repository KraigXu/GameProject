using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001001 RID: 4097
	public class StatPart_Health : StatPart_Curve
	{
		// Token: 0x06006222 RID: 25122 RVA: 0x00220B0D File Offset: 0x0021ED0D
		protected override bool AppliesTo(StatRequest req)
		{
			return req.HasThing && req.Thing.def.useHitPoints && req.Thing.def.healthAffectsPrice;
		}

		// Token: 0x06006223 RID: 25123 RVA: 0x00220B3E File Offset: 0x0021ED3E
		protected override float CurveXGetter(StatRequest req)
		{
			return (float)req.Thing.HitPoints / (float)req.Thing.MaxHitPoints;
		}

		// Token: 0x06006224 RID: 25124 RVA: 0x00220B5C File Offset: 0x0021ED5C
		protected override string ExplanationLabel(StatRequest req)
		{
			return "StatsReport_HealthMultiplier".Translate(req.Thing.HitPoints + " / " + req.Thing.MaxHitPoints);
		}
	}
}
