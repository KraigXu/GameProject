using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EBC RID: 3772
	public class MainButtonWorker_ToggleResearchTab : MainButtonWorker_ToggleTab
	{
		// Token: 0x170010A3 RID: 4259
		// (get) Token: 0x06005C34 RID: 23604 RVA: 0x001FD8D0 File Offset: 0x001FBAD0
		public override float ButtonBarPercent
		{
			get
			{
				ResearchProjectDef currentProj = Find.ResearchManager.currentProj;
				if (currentProj == null)
				{
					return 0f;
				}
				return currentProj.ProgressPercent;
			}
		}
	}
}
