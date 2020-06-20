using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001143 RID: 4419
	public class QuestNode_GetSiteDisturbanceFactor : QuestNode
	{
		// Token: 0x0600672D RID: 26413 RVA: 0x00241CD9 File Offset: 0x0023FED9
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600672E RID: 26414 RVA: 0x00241CE3 File Offset: 0x0023FEE3
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600672F RID: 26415 RVA: 0x00241CF0 File Offset: 0x0023FEF0
		private void SetVars(Slate slate)
		{
			float num = 1f;
			IEnumerable<SitePartDef> value = this.sitePartDefs.GetValue(slate);
			if (value != null)
			{
				foreach (SitePartDef sitePartDef in value)
				{
					num *= sitePartDef.activeThreatDisturbanceFactor;
				}
			}
			slate.Set<float>(this.storeAs.GetValue(slate), num, false);
		}

		// Token: 0x04003F4D RID: 16205
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003F4E RID: 16206
		public SlateRef<IEnumerable<SitePartDef>> sitePartDefs;
	}
}
