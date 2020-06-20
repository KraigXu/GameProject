using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A89 RID: 2697
	public sealed class ResourceCounter
	{
		// Token: 0x17000B4A RID: 2890
		// (get) Token: 0x06003FC8 RID: 16328 RVA: 0x001538E4 File Offset: 0x00151AE4
		public int Silver
		{
			get
			{
				return this.GetCount(ThingDefOf.Silver);
			}
		}

		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x06003FC9 RID: 16329 RVA: 0x001538F4 File Offset: 0x00151AF4
		public float TotalHumanEdibleNutrition
		{
			get
			{
				float num = 0f;
				foreach (KeyValuePair<ThingDef, int> keyValuePair in this.countedAmounts)
				{
					if (keyValuePair.Key.IsNutritionGivingIngestible && keyValuePair.Key.ingestible.HumanEdible)
					{
						num += keyValuePair.Key.GetStatValueAbstract(StatDefOf.Nutrition, null) * (float)keyValuePair.Value;
					}
				}
				return num;
			}
		}

		// Token: 0x17000B4C RID: 2892
		// (get) Token: 0x06003FCA RID: 16330 RVA: 0x00153988 File Offset: 0x00151B88
		public Dictionary<ThingDef, int> AllCountedAmounts
		{
			get
			{
				return this.countedAmounts;
			}
		}

		// Token: 0x06003FCB RID: 16331 RVA: 0x00153990 File Offset: 0x00151B90
		public static void ResetDefs()
		{
			ResourceCounter.resources.Clear();
			ResourceCounter.resources.AddRange(from def in DefDatabase<ThingDef>.AllDefs
			where def.CountAsResource
			orderby def.resourceReadoutPriority descending
			select def);
		}

		// Token: 0x06003FCC RID: 16332 RVA: 0x001539FE File Offset: 0x00151BFE
		public ResourceCounter(Map map)
		{
			this.map = map;
			this.ResetResourceCounts();
		}

		// Token: 0x06003FCD RID: 16333 RVA: 0x00153A20 File Offset: 0x00151C20
		public void ResetResourceCounts()
		{
			this.countedAmounts.Clear();
			for (int i = 0; i < ResourceCounter.resources.Count; i++)
			{
				this.countedAmounts.Add(ResourceCounter.resources[i], 0);
			}
		}

		// Token: 0x06003FCE RID: 16334 RVA: 0x00153A64 File Offset: 0x00151C64
		public int GetCount(ThingDef rDef)
		{
			if (rDef.resourceReadoutPriority == ResourceCountPriority.Uncounted)
			{
				return 0;
			}
			int result;
			if (this.countedAmounts.TryGetValue(rDef, out result))
			{
				return result;
			}
			Log.Error("Looked for nonexistent key " + rDef + " in counted resources.", false);
			this.countedAmounts.Add(rDef, 0);
			return 0;
		}

		// Token: 0x06003FCF RID: 16335 RVA: 0x00153AB4 File Offset: 0x00151CB4
		public int GetCountIn(ThingRequestGroup group)
		{
			int num = 0;
			foreach (KeyValuePair<ThingDef, int> keyValuePair in this.countedAmounts)
			{
				if (group.Includes(keyValuePair.Key))
				{
					num += keyValuePair.Value;
				}
			}
			return num;
		}

		// Token: 0x06003FD0 RID: 16336 RVA: 0x00153B1C File Offset: 0x00151D1C
		public int GetCountIn(ThingCategoryDef cat)
		{
			int num = 0;
			for (int i = 0; i < cat.childThingDefs.Count; i++)
			{
				num += this.GetCount(cat.childThingDefs[i]);
			}
			for (int j = 0; j < cat.childCategories.Count; j++)
			{
				if (!cat.childCategories[j].resourceReadoutRoot)
				{
					num += this.GetCountIn(cat.childCategories[j]);
				}
			}
			return num;
		}

		// Token: 0x06003FD1 RID: 16337 RVA: 0x00153B95 File Offset: 0x00151D95
		public void ResourceCounterTick()
		{
			if (Find.TickManager.TicksGame % 204 == 0)
			{
				this.UpdateResourceCounts();
			}
		}

		// Token: 0x06003FD2 RID: 16338 RVA: 0x00153BB0 File Offset: 0x00151DB0
		public void UpdateResourceCounts()
		{
			this.ResetResourceCounts();
			List<SlotGroup> allGroupsListForReading = this.map.haulDestinationManager.AllGroupsListForReading;
			for (int i = 0; i < allGroupsListForReading.Count; i++)
			{
				foreach (Thing outerThing in allGroupsListForReading[i].HeldThings)
				{
					Thing innerIfMinified = outerThing.GetInnerIfMinified();
					if (innerIfMinified.def.CountAsResource && this.ShouldCount(innerIfMinified))
					{
						Dictionary<ThingDef, int> dictionary = this.countedAmounts;
						ThingDef def = innerIfMinified.def;
						dictionary[def] += innerIfMinified.stackCount;
					}
				}
			}
		}

		// Token: 0x06003FD3 RID: 16339 RVA: 0x00153C68 File Offset: 0x00151E68
		private bool ShouldCount(Thing t)
		{
			return !t.IsNotFresh();
		}

		// Token: 0x04002522 RID: 9506
		private Map map;

		// Token: 0x04002523 RID: 9507
		private Dictionary<ThingDef, int> countedAmounts = new Dictionary<ThingDef, int>();

		// Token: 0x04002524 RID: 9508
		private static List<ThingDef> resources = new List<ThingDef>();
	}
}
