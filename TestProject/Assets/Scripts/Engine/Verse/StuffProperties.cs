using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000A5 RID: 165
	public class StuffProperties
	{
		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000531 RID: 1329 RVA: 0x0001A310 File Offset: 0x00018510
		public ThingDef SourceNaturalRock
		{
			get
			{
				if (!this.sourceNaturalRockCached)
				{
					List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
					List<RecipeDef> allDefsListForReading2 = DefDatabase<RecipeDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (allDefsListForReading[i].category == ThingCategory.Building && allDefsListForReading[i].building.isNaturalRock && allDefsListForReading[i].building.mineableThing != null && !allDefsListForReading[i].IsSmoothed)
						{
							if (allDefsListForReading[i].building.mineableThing == this.parent)
							{
								this.sourceNaturalRockCachedValue = allDefsListForReading[i];
								break;
							}
							for (int j = 0; j < allDefsListForReading2.Count; j++)
							{
								if (allDefsListForReading2[j].IsIngredient(allDefsListForReading[i].building.mineableThing))
								{
									bool flag = false;
									for (int k = 0; k < allDefsListForReading2[j].products.Count; k++)
									{
										if (allDefsListForReading2[j].products[k].thingDef == this.parent)
										{
											flag = true;
											break;
										}
									}
									if (flag)
									{
										this.sourceNaturalRockCachedValue = allDefsListForReading[i];
										break;
									}
								}
							}
						}
					}
					this.sourceNaturalRockCached = true;
				}
				return this.sourceNaturalRockCachedValue;
			}
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0001A460 File Offset: 0x00018660
		public bool CanMake(BuildableDef t)
		{
			if (!t.MadeFromStuff)
			{
				return false;
			}
			for (int i = 0; i < t.stuffCategories.Count; i++)
			{
				for (int j = 0; j < this.categories.Count; j++)
				{
					if (t.stuffCategories[i] == this.categories[j])
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0001A4C0 File Offset: 0x000186C0
		public void ResolveReferencesSpecial()
		{
			if (this.appearance == null)
			{
				this.appearance = StuffAppearanceDefOf.Smooth;
			}
		}

		// Token: 0x04000325 RID: 805
		[Unsaved(false)]
		public ThingDef parent;

		// Token: 0x04000326 RID: 806
		public string stuffAdjective;

		// Token: 0x04000327 RID: 807
		public float commonality = 1f;

		// Token: 0x04000328 RID: 808
		public List<StuffCategoryDef> categories = new List<StuffCategoryDef>();

		// Token: 0x04000329 RID: 809
		public List<StatModifier> statOffsets;

		// Token: 0x0400032A RID: 810
		public List<StatModifier> statFactors;

		// Token: 0x0400032B RID: 811
		public Color color = new Color(0.8f, 0.8f, 0.8f);

		// Token: 0x0400032C RID: 812
		public EffecterDef constructEffect;

		// Token: 0x0400032D RID: 813
		public StuffAppearanceDef appearance;

		// Token: 0x0400032E RID: 814
		public bool allowColorGenerators;

		// Token: 0x0400032F RID: 815
		public SoundDef soundImpactStuff;

		// Token: 0x04000330 RID: 816
		public SoundDef soundMeleeHitSharp;

		// Token: 0x04000331 RID: 817
		public SoundDef soundMeleeHitBlunt;

		// Token: 0x04000332 RID: 818
		[Unsaved(false)]
		private bool sourceNaturalRockCached;

		// Token: 0x04000333 RID: 819
		[Unsaved(false)]
		private ThingDef sourceNaturalRockCachedValue;
	}
}
