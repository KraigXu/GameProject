using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D5D RID: 3421
	public class CompProperties_SpawnerItems : CompProperties
	{
		// Token: 0x17000ECD RID: 3789
		// (get) Token: 0x0600534E RID: 21326 RVA: 0x001BE039 File Offset: 0x001BC239
		public IEnumerable<ThingDef> MatchingItems
		{
			get
			{
				return from def in DefDatabase<ThingDef>.AllDefsListForReading
				where def.BaseMarketValue <= this.approxMarketValuePerDay && ((def.IsStuff && this.stuffCategories.Any((StuffCategoryDef c) => def.stuffProps.categories.Contains(c))) || this.categories.Any((ThingCategoryDef c) => def.IsWithinCategory(c)))
				select def;
			}
		}

		// Token: 0x0600534F RID: 21327 RVA: 0x001BE051 File Offset: 0x001BC251
		public CompProperties_SpawnerItems()
		{
			this.compClass = typeof(CompSpawnerItems);
		}

		// Token: 0x06005350 RID: 21328 RVA: 0x001BE08A File Offset: 0x001BC28A
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.MatchingItems.Count<ThingDef>() <= 0)
			{
				yield return "Could not find any item that would be spawnable by " + parentDef.defName + " (CompSpawnerItems)!";
			}
			yield break;
		}

		// Token: 0x04002E00 RID: 11776
		public float approxMarketValuePerDay;

		// Token: 0x04002E01 RID: 11777
		public int spawnInterval = 60000;

		// Token: 0x04002E02 RID: 11778
		public List<StuffCategoryDef> stuffCategories = new List<StuffCategoryDef>();

		// Token: 0x04002E03 RID: 11779
		public List<ThingCategoryDef> categories = new List<ThingCategoryDef>();
	}
}
