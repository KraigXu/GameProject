using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class CompProperties_SpawnerItems : CompProperties
	{
		
		
		public IEnumerable<ThingDef> MatchingItems
		{
			get
			{
				return from def in DefDatabase<ThingDef>.AllDefsListForReading
				where def.BaseMarketValue <= this.approxMarketValuePerDay && ((def.IsStuff && this.stuffCategories.Any((StuffCategoryDef c) => def.stuffProps.categories.Contains(c))) || this.categories.Any((ThingCategoryDef c) => def.IsWithinCategory(c)))
				select def;
			}
		}

		
		public CompProperties_SpawnerItems()
		{
			this.compClass = typeof(CompSpawnerItems);
		}

		
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.MatchingItems.Count<ThingDef>() <= 0)
			{
				yield return "Could not find any item that would be spawnable by " + parentDef.defName + " (CompSpawnerItems)!";
			}
			yield break;
		}

		
		public float approxMarketValuePerDay;

		
		public int spawnInterval = 60000;

		
		public List<StuffCategoryDef> stuffCategories = new List<StuffCategoryDef>();

		
		public List<ThingCategoryDef> categories = new List<ThingCategoryDef>();
	}
}
