using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001145 RID: 4421
	public class QuestNode_GetSiteThreatPoints : QuestNode
	{
		// Token: 0x06006736 RID: 26422 RVA: 0x0001028D File Offset: 0x0000E48D
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06006737 RID: 26423 RVA: 0x00241FA4 File Offset: 0x002401A4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.site.GetValue(slate) != null)
			{
				slate.Set<float>(this.storeAs.GetValue(slate), this.site.GetValue(slate).ActualThreatPoints, false);
				return;
			}
			float num = 0f;
			IEnumerable<SitePartDefWithParams> value = this.sitePartsParams.GetValue(slate);
			if (value != null)
			{
				foreach (SitePartDefWithParams sitePartDefWithParams in value)
				{
					num += sitePartDefWithParams.parms.threatPoints;
				}
			}
			slate.Set<float>(this.storeAs.GetValue(slate), num, false);
		}

		// Token: 0x04003F54 RID: 16212
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04003F55 RID: 16213
		public SlateRef<Site> site;

		// Token: 0x04003F56 RID: 16214
		public SlateRef<IEnumerable<SitePartDefWithParams>> sitePartsParams;
	}
}
