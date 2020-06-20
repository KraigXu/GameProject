using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001256 RID: 4694
	public static class SettlementNameGenerator
	{
		// Token: 0x06006D96 RID: 28054 RVA: 0x00265354 File Offset: 0x00263554
		public static string GenerateSettlementName(Settlement factionBase, RulePackDef rulePack = null)
		{
			if (rulePack == null)
			{
				if (factionBase.Faction == null || factionBase.Faction.def.settlementNameMaker == null)
				{
					return factionBase.def.label;
				}
				rulePack = factionBase.Faction.def.settlementNameMaker;
			}
			SettlementNameGenerator.usedNames.Clear();
			List<Settlement> settlements = Find.WorldObjects.Settlements;
			for (int i = 0; i < settlements.Count; i++)
			{
				Settlement settlement = settlements[i];
				if (settlement.Name != null)
				{
					SettlementNameGenerator.usedNames.Add(settlement.Name);
				}
			}
			return NameGenerator.GenerateName(rulePack, SettlementNameGenerator.usedNames, true, null);
		}

		// Token: 0x040043EB RID: 17387
		private static List<string> usedNames = new List<string>();
	}
}
