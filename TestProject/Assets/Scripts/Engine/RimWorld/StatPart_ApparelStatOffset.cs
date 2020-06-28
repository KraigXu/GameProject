using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FF5 RID: 4085
	public class StatPart_ApparelStatOffset : StatPart
	{
		// Token: 0x060061F1 RID: 25073 RVA: 0x002201E4 File Offset: 0x0021E3E4
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && req.Thing != null)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.apparel != null)
				{
					for (int i = 0; i < pawn.apparel.WornApparel.Count; i++)
					{
						float statValue = pawn.apparel.WornApparel[i].GetStatValue(this.apparelStat, true);
						if (this.subtract)
						{
							val -= statValue;
						}
						else
						{
							val += statValue;
						}
					}
				}
			}
		}

		// Token: 0x060061F2 RID: 25074 RVA: 0x0022026C File Offset: 0x0021E46C
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && req.Thing != null)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && this.PawnWearingRelevantGear(pawn))
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("StatsReport_RelevantGear".Translate());
					if (pawn.apparel != null)
					{
						for (int i = 0; i < pawn.apparel.WornApparel.Count; i++)
						{
							Apparel gear = pawn.apparel.WornApparel[i];
							stringBuilder.AppendLine(this.InfoTextLineFrom(gear));
						}
					}
					return stringBuilder.ToString();
				}
			}
			return null;
		}

		// Token: 0x060061F3 RID: 25075 RVA: 0x00220310 File Offset: 0x0021E510
		private string InfoTextLineFrom(Thing gear)
		{
			float num = gear.GetStatValue(this.apparelStat, true);
			if (this.subtract)
			{
				num = -num;
			}
			return "    " + gear.LabelCap + ": " + num.ToStringByStyle(this.parentStat.toStringStyle, ToStringNumberSense.Offset);
		}

		// Token: 0x060061F4 RID: 25076 RVA: 0x00220360 File Offset: 0x0021E560
		private bool PawnWearingRelevantGear(Pawn pawn)
		{
			if (pawn.apparel != null)
			{
				for (int i = 0; i < pawn.apparel.WornApparel.Count; i++)
				{
					if (pawn.apparel.WornApparel[i].GetStatValue(this.apparelStat, true) != 0f)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060061F5 RID: 25077 RVA: 0x002203B7 File Offset: 0x0021E5B7
		public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest req)
		{
			Pawn pawn = req.Thing as Pawn;
			if (pawn != null && pawn.apparel != null)
			{
				int num;
				for (int i = 0; i < pawn.apparel.WornApparel.Count; i = num + 1)
				{
					Apparel thing = pawn.apparel.WornApparel[i];
					if (Mathf.Abs(thing.GetStatValue(this.apparelStat, true)) > 0f)
					{
						yield return new Dialog_InfoCard.Hyperlink(thing, -1);
					}
					num = i;
				}
			}
			yield break;
		}

		// Token: 0x04003BD6 RID: 15318
		private StatDef apparelStat;

		// Token: 0x04003BD7 RID: 15319
		private bool subtract;
	}
}
