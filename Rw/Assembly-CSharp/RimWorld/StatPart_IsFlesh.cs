using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001003 RID: 4099
	public class StatPart_IsFlesh : StatPart
	{
		// Token: 0x0600622A RID: 25130 RVA: 0x00220C6C File Offset: 0x0021EE6C
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetIsFleshFactor(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x0600622B RID: 25131 RVA: 0x00220C8C File Offset: 0x0021EE8C
		public override string ExplanationPart(StatRequest req)
		{
			float num;
			if (this.TryGetIsFleshFactor(req, out num) && num != 1f)
			{
				return "StatsReport_NotFlesh".Translate() + ": x" + num.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0600622C RID: 25132 RVA: 0x00220CD4 File Offset: 0x0021EED4
		private bool TryGetIsFleshFactor(StatRequest req, out float bodySize)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, delegate(Pawn x)
			{
				if (!x.RaceProps.IsFlesh)
				{
					return 0f;
				}
				return 1f;
			}, delegate(ThingDef x)
			{
				if (!x.race.IsFlesh)
				{
					return 0f;
				}
				return 1f;
			}, out bodySize);
		}
	}
}
