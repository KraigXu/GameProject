using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D66 RID: 3430
	public class CompProperties_Techprint : CompProperties
	{
		// Token: 0x0600538B RID: 21387 RVA: 0x001BF0F5 File Offset: 0x001BD2F5
		public CompProperties_Techprint()
		{
			this.compClass = typeof(CompTechprint);
		}

		// Token: 0x0600538C RID: 21388 RVA: 0x001BF110 File Offset: 0x001BD310
		public override void ResolveReferences(ThingDef parentDef)
		{
			if (parentDef.descriptionHyperlinks == null)
			{
				parentDef.descriptionHyperlinks = new List<DefHyperlink>();
			}
			List<Def> unlockedDefs = this.project.UnlockedDefs;
			for (int i = 0; i < unlockedDefs.Count; i++)
			{
				ThingDef def;
				RecipeDef recipeDef;
				if ((def = (unlockedDefs[i] as ThingDef)) != null)
				{
					parentDef.descriptionHyperlinks.Add(def);
				}
				else if ((recipeDef = (unlockedDefs[i] as RecipeDef)) != null && !recipeDef.products.NullOrEmpty<ThingDefCountClass>())
				{
					for (int j = 0; j < recipeDef.products.Count; j++)
					{
						parentDef.descriptionHyperlinks.Add(recipeDef.products[j].thingDef);
					}
				}
			}
			parentDef.description += "\n\n" + "Unlocks".Translate() + ": " + (from x in this.project.UnlockedDefs
			select x.label).ToCommaList(false).CapitalizeFirst();
		}

		// Token: 0x04002E2A RID: 11818
		public ResearchProjectDef project;
	}
}
