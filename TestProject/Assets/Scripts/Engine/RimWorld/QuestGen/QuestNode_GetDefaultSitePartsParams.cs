using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_GetDefaultSitePartsParams : QuestNode
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
			List<SitePartDefWithParams> list;
			SiteMakerHelper.GenerateDefaultParams(slate.Get<float>("points", 0f, false), this.tile.GetValue(slate), this.faction.GetValue(slate), this.sitePartDefs.GetValue(slate), out list);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def == SitePartDefOf.PreciousLump)
				{
					list[i].parms.preciousLumpResources = slate.Get<ThingDef>("targetMineable", null, false);
				}
			}
			slate.Set<List<SitePartDefWithParams>>(this.storeSitePartsParamsAs.GetValue(slate), list, false);
		}

		
		public SlateRef<int> tile;

		
		public SlateRef<Faction> faction;

		
		public SlateRef<IEnumerable<SitePartDef>> sitePartDefs;

		
		[NoTranslate]
		public SlateRef<string> storeSitePartsParamsAs;
	}
}
