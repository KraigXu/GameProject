using System;
using Verse;

namespace RimWorld
{
	
	public class MainButtonWorker_ToggleResearchTab : MainButtonWorker_ToggleTab
	{
		
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
