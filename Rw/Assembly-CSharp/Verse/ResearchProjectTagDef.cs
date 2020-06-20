using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000DE RID: 222
	public class ResearchProjectTagDef : Def
	{
		// Token: 0x0600062C RID: 1580 RVA: 0x0001DA28 File Offset: 0x0001BC28
		public int CompletedProjects()
		{
			int num = 0;
			List<ResearchProjectDef> allDefsListForReading = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				ResearchProjectDef researchProjectDef = allDefsListForReading[i];
				if (researchProjectDef.IsFinished && researchProjectDef.HasTag(this))
				{
					num++;
				}
			}
			return num;
		}
	}
}
