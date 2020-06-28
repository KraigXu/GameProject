using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001010 RID: 4112
	public class StatPart_WildManOffset : StatPart
	{
		// Token: 0x06006263 RID: 25187 RVA: 0x00221A6B File Offset: 0x0021FC6B
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.IsWildMan(req))
			{
				val += this.offset;
			}
		}

		// Token: 0x06006264 RID: 25188 RVA: 0x00221A81 File Offset: 0x0021FC81
		public override string ExplanationPart(StatRequest req)
		{
			if (this.IsWildMan(req))
			{
				return "StatsReport_WildMan".Translate() + ": " + this.offset.ToStringWithSign("0.##");
			}
			return null;
		}

		// Token: 0x06006265 RID: 25189 RVA: 0x00221ABC File Offset: 0x0021FCBC
		private bool IsWildMan(StatRequest req)
		{
			Pawn pawn = req.Thing as Pawn;
			return pawn != null && pawn.IsWildMan();
		}

		// Token: 0x04003BFE RID: 15358
		public float offset;
	}
}
