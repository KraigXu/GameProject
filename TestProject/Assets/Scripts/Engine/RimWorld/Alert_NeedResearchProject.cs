using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DFD RID: 3581
	public class Alert_NeedResearchProject : Alert
	{
		// Token: 0x060056A4 RID: 22180 RVA: 0x001CBA3A File Offset: 0x001C9C3A
		public Alert_NeedResearchProject()
		{
			this.defaultLabel = "NeedResearchProject".Translate();
			this.defaultExplanation = "NeedResearchProjectDesc".Translate();
		}

		// Token: 0x060056A5 RID: 22181 RVA: 0x001CBA6C File Offset: 0x001C9C6C
		public override AlertReport GetReport()
		{
			if (Find.AnyPlayerHomeMap == null)
			{
				return false;
			}
			if (Find.ResearchManager.currentProj != null)
			{
				return false;
			}
			bool flag = false;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (maps[i].IsPlayerHome && maps[i].listerBuildings.ColonistsHaveResearchBench())
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
			if (!Find.ResearchManager.AnyProjectIsAvailable)
			{
				return false;
			}
			return true;
		}
	}
}
