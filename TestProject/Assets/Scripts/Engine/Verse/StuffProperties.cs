using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public class StuffProperties
	{
		
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

		
		public void ResolveReferencesSpecial()
		{
			if (this.appearance == null)
			{
				this.appearance = StuffAppearanceDefOf.Smooth;
			}
		}

		
		[Unsaved(false)]
		public ThingDef parent;

		
		public string stuffAdjective;

		
		public float commonality = 1f;

		
		public List<StuffCategoryDef> categories = new List<StuffCategoryDef>();

		
		public List<StatModifier> statOffsets;

		
		public List<StatModifier> statFactors;

		
		public Color color = new Color(0.8f, 0.8f, 0.8f);

		
		public EffecterDef constructEffect;

		
		public StuffAppearanceDef appearance;

		
		public bool allowColorGenerators;

		
		public SoundDef soundImpactStuff;

		
		public SoundDef soundMeleeHitSharp;

		
		public SoundDef soundMeleeHitBlunt;

		
		[Unsaved(false)]
		private bool sourceNaturalRockCached;

		
		[Unsaved(false)]
		private ThingDef sourceNaturalRockCachedValue;
	}
}
