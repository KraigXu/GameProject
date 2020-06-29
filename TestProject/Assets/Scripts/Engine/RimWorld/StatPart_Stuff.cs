using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	
	public class StatPart_Stuff : StatPart
	{
		
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

		
		public override void TransformValue(StatRequest req, ref float value)
		{
			float num = (req.StuffDef != null) ? req.StuffDef.GetStatValueAbstract(this.stuffPowerStat, null) : 0f;
			value += this.GetMultiplier(req) * num;
		}

		
		private float GetMultiplier(StatRequest req)
		{
			if (req.HasThing)
			{
				return req.Thing.GetStatValue(this.multiplierStat, true);
			}
			return req.BuildableDef.GetStatValueAbstract(this.multiplierStat, null);
		}

		
		public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest req)
		{
			if (req.StuffDef != null)
			{
				yield return new Dialog_InfoCard.Hyperlink(req.StuffDef, -1);
			}
			yield break;
		}

		
		public StatDef stuffPowerStat;

		
		public StatDef multiplierStat;
	}
}
