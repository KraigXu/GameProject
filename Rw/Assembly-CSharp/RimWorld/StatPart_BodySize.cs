using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FF8 RID: 4088
	public class StatPart_BodySize : StatPart
	{
		// Token: 0x060061FE RID: 25086 RVA: 0x002204C0 File Offset: 0x0021E6C0
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetBodySize(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x060061FF RID: 25087 RVA: 0x002204E0 File Offset: 0x0021E6E0
		public override string ExplanationPart(StatRequest req)
		{
			float f;
			if (this.TryGetBodySize(req, out f))
			{
				return "StatsReport_BodySize".Translate(f.ToString("F2")) + ": x" + f.ToStringPercent();
			}
			return null;
		}

		// Token: 0x06006200 RID: 25088 RVA: 0x00220530 File Offset: 0x0021E730
		private bool TryGetBodySize(StatRequest req, out float bodySize)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => x.BodySize, (ThingDef x) => x.race.baseBodySize, out bodySize);
		}
	}
}
