using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001050 RID: 4176
	public class PlaceWorker_ReportWorkSpeedPenalties : PlaceWorker
	{
		// Token: 0x060063BB RID: 25531 RVA: 0x002296E8 File Offset: 0x002278E8
		public override void PostPlace(Map map, BuildableDef def, IntVec3 loc, Rot4 rot)
		{
			ThingDef thingDef = def as ThingDef;
			if (thingDef == null)
			{
				return;
			}
			bool flag = StatPart_WorkTableOutdoors.Applies(thingDef, map, loc);
			bool flag2 = StatPart_WorkTableTemperature.Applies(thingDef, map, loc);
			if (flag || flag2)
			{
				string str = "WillGetWorkSpeedPenalty".Translate(def.label).CapitalizeFirst() + ": ";
				string text = "";
				if (flag)
				{
					text += "Outdoors".Translate();
				}
				if (flag2)
				{
					if (!text.NullOrEmpty())
					{
						text += ", ";
					}
					text += "BadTemperature".Translate();
				}
				Messages.Message(str + text.CapitalizeFirst() + ".", new TargetInfo(loc, map, false), MessageTypeDefOf.CautionInput, false);
			}
		}
	}
}
