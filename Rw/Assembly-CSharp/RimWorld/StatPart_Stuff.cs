using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200100E RID: 4110
	public class StatPart_Stuff : StatPart
	{
		// Token: 0x0600625A RID: 25178 RVA: 0x00221814 File Offset: 0x0021FA14
		public override string ExplanationPart(StatRequest req)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (req.BuildableDef.MadeFromStuff)
			{
				string t = (req.StuffDef != null) ? req.StuffDef.label : "None".TranslateSimple();
				string t2 = (req.StuffDef != null) ? req.StuffDef.GetStatValueAbstract(this.stuffPowerStat, null).ToStringByStyle(this.parentStat.ToStringStyleUnfinalized, ToStringNumberSense.Absolute) : "0";
				stringBuilder.AppendLine("StatsReport_Material".Translate() + " (" + t + "): " + t2);
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("StatsReport_StuffEffectMultiplier".Translate() + ": " + this.GetMultiplier(req).ToStringPercent("F0"));
				stringBuilder.AppendLine();
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0600625B RID: 25179 RVA: 0x00221914 File Offset: 0x0021FB14
		public override void TransformValue(StatRequest req, ref float value)
		{
			float num = (req.StuffDef != null) ? req.StuffDef.GetStatValueAbstract(this.stuffPowerStat, null) : 0f;
			value += this.GetMultiplier(req) * num;
		}

		// Token: 0x0600625C RID: 25180 RVA: 0x00221953 File Offset: 0x0021FB53
		private float GetMultiplier(StatRequest req)
		{
			if (req.HasThing)
			{
				return req.Thing.GetStatValue(this.multiplierStat, true);
			}
			return req.BuildableDef.GetStatValueAbstract(this.multiplierStat, null);
		}

		// Token: 0x0600625D RID: 25181 RVA: 0x00221985 File Offset: 0x0021FB85
		public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest req)
		{
			if (req.StuffDef != null)
			{
				yield return new Dialog_InfoCard.Hyperlink(req.StuffDef, -1);
			}
			yield break;
		}

		// Token: 0x04003BFC RID: 15356
		public StatDef stuffPowerStat;

		// Token: 0x04003BFD RID: 15357
		public StatDef multiplierStat;
	}
}
