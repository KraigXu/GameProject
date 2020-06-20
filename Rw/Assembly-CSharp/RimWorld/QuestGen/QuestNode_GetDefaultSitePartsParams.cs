using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001122 RID: 4386
	public class QuestNode_GetDefaultSitePartsParams : QuestNode
	{
		// Token: 0x0600669D RID: 26269 RVA: 0x0023ECDB File Offset: 0x0023CEDB
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600669E RID: 26270 RVA: 0x0023ECE5 File Offset: 0x0023CEE5
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600669F RID: 26271 RVA: 0x0023ECF4 File Offset: 0x0023CEF4
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

		// Token: 0x04003EC0 RID: 16064
		public SlateRef<int> tile;

		// Token: 0x04003EC1 RID: 16065
		public SlateRef<Faction> faction;

		// Token: 0x04003EC2 RID: 16066
		public SlateRef<IEnumerable<SitePartDef>> sitePartDefs;

		// Token: 0x04003EC3 RID: 16067
		[NoTranslate]
		public SlateRef<string> storeSitePartsParamsAs;
	}
}
