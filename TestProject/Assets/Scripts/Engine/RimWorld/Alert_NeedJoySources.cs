using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DFA RID: 3578
	public class Alert_NeedJoySources : Alert
	{
		// Token: 0x06005698 RID: 22168 RVA: 0x001CB62A File Offset: 0x001C982A
		public Alert_NeedJoySources()
		{
			this.defaultLabel = "NeedJoySource".Translate();
		}

		// Token: 0x06005699 RID: 22169 RVA: 0x001CB648 File Offset: 0x001C9848
		public override TaggedString GetExplanation()
		{
			Map map = this.BadMap();
			int value = JoyUtility.JoyKindsOnMapCount(map);
			string label = map.info.parent.Label;
			ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(map);
			int joyKindsNeeded = expectationDef.joyKindsNeeded;
			string value2 = "AvailableRecreationTypes".Translate() + ":\n\n" + JoyUtility.JoyKindsOnMapString(map);
			string value3 = "MissingRecreationTypes".Translate() + ":\n\n" + JoyUtility.JoyKindsNotOnMapString(map);
			return "NeedJoySourceDesc".Translate(value, label, expectationDef.label, joyKindsNeeded, value2, value3);
		}

		// Token: 0x0600569A RID: 22170 RVA: 0x001CB705 File Offset: 0x001C9905
		public override AlertReport GetReport()
		{
			return this.BadMap() != null;
		}

		// Token: 0x0600569B RID: 22171 RVA: 0x001CB718 File Offset: 0x001C9918
		private Map BadMap()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (this.NeedJoySource(maps[i]))
				{
					return maps[i];
				}
			}
			return null;
		}

		// Token: 0x0600569C RID: 22172 RVA: 0x001CB754 File Offset: 0x001C9954
		private bool NeedJoySource(Map map)
		{
			if (!map.IsPlayerHome)
			{
				return false;
			}
			if (!map.mapPawns.AnyColonistSpawned)
			{
				return false;
			}
			int num = JoyUtility.JoyKindsOnMapCount(map);
			int joyKindsNeeded = ExpectationsUtility.CurrentExpectationFor(map).joyKindsNeeded;
			return num < joyKindsNeeded;
		}
	}
}
