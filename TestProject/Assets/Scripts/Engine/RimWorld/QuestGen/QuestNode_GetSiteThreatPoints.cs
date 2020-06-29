using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	
	public class QuestNode_GetSiteThreatPoints : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		
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

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		public SlateRef<Site> site;

		
		public SlateRef<IEnumerable<SitePartDefWithParams>> sitePartsParams;
	}
}
