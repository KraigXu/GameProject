using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001281 RID: 4737
	public static class TradeRequestUtility
	{
		// Token: 0x06006F1D RID: 28445 RVA: 0x0026B160 File Offset: 0x00269360
		public static string RequestedThingLabel(ThingDef def, int count)
		{
			string text = GenLabel.ThingLabel(def, null, count);
			if (def.HasComp(typeof(CompQuality)))
			{
				text += " (" + "NormalQualityOrBetter".Translate() + ")";
			}
			if (def.IsApparel)
			{
				text += " (" + "NotTainted".Translate() + ")";
			}
			return text;
		}
	}
}
