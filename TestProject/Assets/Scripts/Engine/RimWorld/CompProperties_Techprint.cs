using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_Techprint : CompProperties
	{
		
		public CompProperties_Techprint()
		{
			this.compClass = typeof(CompTechprint);
		}

		
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

		
		public ResearchProjectDef project;
	}
}
