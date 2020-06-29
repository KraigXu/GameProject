using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_GetSiteDisturbanceFactor : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		
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

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		public SlateRef<IEnumerable<SitePartDef>> sitePartDefs;
	}
}
