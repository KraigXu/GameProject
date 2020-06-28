using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001011 RID: 4113
	public class StatPart_WornByCorpse : StatPart
	{
		// Token: 0x06006267 RID: 25191 RVA: 0x00221AE4 File Offset: 0x0021FCE4
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Apparel apparel = req.Thing as Apparel;
				if (apparel != null && apparel.WornByCorpse)
				{
					val *= 0.1f;
				}
			}
		}

		// Token: 0x06006268 RID: 25192 RVA: 0x00221B1C File Offset: 0x0021FD1C
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Apparel apparel = req.Thing as Apparel;
				if (apparel != null && apparel.WornByCorpse)
				{
					return "StatsReport_WornByCorpse".Translate() + ": x" + 0.1f.ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x04003BFF RID: 15359
		private const float Factor = 0.1f;
	}
}
