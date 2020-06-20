using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EBD RID: 3773
	public class MainButtonWorker_ToggleTab : MainButtonWorker
	{
		// Token: 0x06005C36 RID: 23606 RVA: 0x001FD8FF File Offset: 0x001FBAFF
		public override void Activate()
		{
			Find.MainTabsRoot.ToggleTab(this.def, true);
		}
	}
}
