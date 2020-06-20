using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001002 RID: 4098
	public class StatPart_IsCorpseFresh : StatPart
	{
		// Token: 0x06006226 RID: 25126 RVA: 0x00220BB4 File Offset: 0x0021EDB4
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetIsFreshFactor(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x06006227 RID: 25127 RVA: 0x00220BD4 File Offset: 0x0021EDD4
		public override string ExplanationPart(StatRequest req)
		{
			float num;
			if (this.TryGetIsFreshFactor(req, out num) && num != 1f)
			{
				return "StatsReport_NotFresh".Translate() + ": x" + num.ToStringPercent();
			}
			return null;
		}

		// Token: 0x06006228 RID: 25128 RVA: 0x00220C1C File Offset: 0x0021EE1C
		private bool TryGetIsFreshFactor(StatRequest req, out float factor)
		{
			if (!req.HasThing)
			{
				factor = 1f;
				return false;
			}
			Corpse corpse = req.Thing as Corpse;
			if (corpse == null)
			{
				factor = 1f;
				return false;
			}
			factor = ((corpse.GetRotStage() == RotStage.Fresh) ? 1f : 0f);
			return true;
		}
	}
}
