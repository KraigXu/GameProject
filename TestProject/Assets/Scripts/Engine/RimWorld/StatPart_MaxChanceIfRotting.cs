using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001004 RID: 4100
	public class StatPart_MaxChanceIfRotting : StatPart
	{
		// Token: 0x0600622E RID: 25134 RVA: 0x00220D26 File Offset: 0x0021EF26
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.IsRotting(req))
			{
				val = 1f;
			}
		}

		// Token: 0x0600622F RID: 25135 RVA: 0x00220D38 File Offset: 0x0021EF38
		public override string ExplanationPart(StatRequest req)
		{
			if (this.IsRotting(req))
			{
				return "StatsReport_NotFresh".Translate() + ": " + 1f.ToStringPercent();
			}
			return null;
		}

		// Token: 0x06006230 RID: 25136 RVA: 0x00220D6D File Offset: 0x0021EF6D
		private bool IsRotting(StatRequest req)
		{
			return req.HasThing && req.Thing.GetRotStage() > RotStage.Fresh;
		}
	}
}
