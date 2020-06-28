using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200047E RID: 1150
	public class ThingFilter : IExposable
	{
		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x060021E8 RID: 8680 RVA: 0x000CE6EC File Offset: 0x000CC8EC
		public string Summary
		{
			get
			{
				if (!this.customSummary.NullOrEmpty())
				{
					return this.customSummary;
				}
				if (this.thingDefs != null && this.thingDefs.Count == 1 && this.categories.NullOrEmpty<string>() && this.tradeTagsToAllow.NullOrEmpty<string>() && this.tradeTagsToDisallow.NullOrEmpty<string>() && this.thingSetMakerTagsToAllow.NullOrEmpty<string>() && this.thingSetMakerTagsToDisallow.NullOrEmpty<string>() && this.disallowedCategories.NullOrEmpty<string>() && this.specialFiltersToAllow.NullOrEmpty<string>() && this.specialFiltersToDisallow.NullOrEmpty<string>() && this.stuffCategoriesToAllow.NullOrEmpty<StuffCategoryDef>() && this.allowAllWhoCanMake.NullOrEmpty<ThingDef>() && this.disallowWorsePreferability == FoodPreferability.Undefined && !this.disallowInedibleByHuman && this.allowWithComp == null && this.disallowWithComp == null && this.disallowCheaperThan == -3.40282347E+38f && this.disallowedThingDefs.NullOrEmpty<ThingDef>())
				{
					return this.thingDefs[0].label;
				}
				if (this.thingDefs.NullOrEmpty<ThingDef>() && this.categories != null && this.categories.Count == 1 && this.tradeTagsToAllow.NullOrEmpty<string>() && this.tradeTagsToDisallow.NullOrEmpty<string>() && this.thingSetMakerTagsToAllow.NullOrEmpty<string>() && this.thingSetMakerTagsToDisallow.NullOrEmpty<string>() && this.disallowedCategories.NullOrEmpty<string>() && this.specialFiltersToAllow.NullOrEmpty<string>() && this.specialFiltersToDisallow.NullOrEmpty<string>() && this.stuffCategoriesToAllow.NullOrEmpty<StuffCategoryDef>() && this.allowAllWhoCanMake.NullOrEmpty<ThingDef>() && this.disallowWorsePreferability == FoodPreferability.Undefined && !this.disallowInedibleByHuman && this.allowWithComp == null && this.disallowWithComp == null && this.disallowCheaperThan == -3.40282347E+38f && this.disallowedThingDefs.NullOrEmpty<ThingDef>())
				{
					return DefDatabase<ThingCategoryDef>.GetNamed(this.categories[0], true).label;
				}
				if (this.allowedDefs.Count == 1)
				{
					return this.allowedDefs.First<ThingDef>().label;
				}
				return "UsableIngredients".Translate();
			}
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x060021E9 RID: 8681 RVA: 0x000CE954 File Offset: 0x000CCB54
		public ThingRequest BestThingRequest
		{
			get
			{
				if (this.allowedDefs.Count == 1)
				{
					return ThingRequest.ForDef(this.allowedDefs.First<ThingDef>());
				}
				bool flag = true;
				bool flag2 = true;
				foreach (ThingDef thingDef in this.allowedDefs)
				{
					if (!thingDef.EverHaulable)
					{
						flag = false;
					}
					if (thingDef.category != ThingCategory.Pawn)
					{
						flag2 = false;
					}
				}
				if (flag)
				{
					return ThingRequest.ForGroup(ThingRequestGroup.HaulableEver);
				}
				if (flag2)
				{
					return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
				}
				return ThingRequest.ForGroup(ThingRequestGroup.Everything);
			}
		}

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x060021EA RID: 8682 RVA: 0x000CE9F4 File Offset: 0x000CCBF4
		public ThingDef AnyAllowedDef
		{
			get
			{
				return this.allowedDefs.FirstOrDefault<ThingDef>();
			}
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x060021EB RID: 8683 RVA: 0x000CEA01 File Offset: 0x000CCC01
		public IEnumerable<ThingDef> AllowedThingDefs
		{
			get
			{
				return this.allowedDefs;
			}
		}

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x060021EC RID: 8684 RVA: 0x000CEA09 File Offset: 0x000CCC09
		private static IEnumerable<ThingDef> AllStorableThingDefs
		{
			get
			{
				return from def in DefDatabase<ThingDef>.AllDefs
				where def.EverStorable(true)
				select def;
			}
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x060021ED RID: 8685 RVA: 0x000CEA34 File Offset: 0x000CCC34
		public int AllowedDefCount
		{
			get
			{
				return this.allowedDefs.Count;
			}
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x060021EE RID: 8686 RVA: 0x000CEA41 File Offset: 0x000CCC41
		// (set) Token: 0x060021EF RID: 8687 RVA: 0x000CEA49 File Offset: 0x000CCC49
		public FloatRange AllowedHitPointsPercents
		{
			get
			{
				return this.allowedHitPointsPercents;
			}
			set
			{
				if (this.allowedHitPointsPercents == value)
				{
					return;
				}
				this.allowedHitPointsPercents = value;
				if (this.settingsChangedCallback != null)
				{
					this.settingsChangedCallback();
				}
			}
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x060021F0 RID: 8688 RVA: 0x000CEA74 File Offset: 0x000CCC74
		// (set) Token: 0x060021F1 RID: 8689 RVA: 0x000CEA7C File Offset: 0x000CCC7C
		public QualityRange AllowedQualityLevels
		{
			get
			{
				return this.allowedQualities;
			}
			set
			{
				if (this.allowedQualities == value)
				{
					return;
				}
				this.allowedQualities = value;
				if (this.settingsChangedCallback != null)
				{
					this.settingsChangedCallback();
				}
			}
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x060021F2 RID: 8690 RVA: 0x000CEAA7 File Offset: 0x000CCCA7
		// (set) Token: 0x060021F3 RID: 8691 RVA: 0x000CEACB File Offset: 0x000CCCCB
		public TreeNode_ThingCategory DisplayRootCategory
		{
			get
			{
				if (this.displayRootCategoryInt == null)
				{
					this.RecalculateDisplayRootCategory();
				}
				if (this.displayRootCategoryInt == null)
				{
					return ThingCategoryNodeDatabase.RootNode;
				}
				return this.displayRootCategoryInt;
			}
			set
			{
				if (value == this.displayRootCategoryInt)
				{
					return;
				}
				this.displayRootCategoryInt = value;
				this.RecalculateSpecialFilterConfigurability();
			}
		}

		// Token: 0x060021F4 RID: 8692 RVA: 0x000CEAE4 File Offset: 0x000CCCE4
		public ThingFilter()
		{
		}

		// Token: 0x060021F5 RID: 8693 RVA: 0x000CEB3C File Offset: 0x000CCD3C
		public ThingFilter(Action settingsChangedCallback)
		{
			this.settingsChangedCallback = settingsChangedCallback;
		}

		// Token: 0x060021F6 RID: 8694 RVA: 0x000CEB9C File Offset: 0x000CCD9C
		public virtual void ExposeData()
		{
			Scribe_Collections.Look<SpecialThingFilterDef>(ref this.disallowedSpecialFilters, "disallowedSpecialFilters", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<ThingDef>(ref this.allowedDefs, "allowedDefs", LookMode.Undefined);
			Scribe_Values.Look<FloatRange>(ref this.allowedHitPointsPercents, "allowedHitPointsPercents", default(FloatRange), false);
			Scribe_Values.Look<QualityRange>(ref this.allowedQualities, "allowedQualityLevels", default(QualityRange), false);
		}

		// Token: 0x060021F7 RID: 8695 RVA: 0x000CEC04 File Offset: 0x000CCE04
		public void ResolveReferences()
		{
			for (int i = 0; i < DefDatabase<SpecialThingFilterDef>.AllDefsListForReading.Count; i++)
			{
				SpecialThingFilterDef specialThingFilterDef = DefDatabase<SpecialThingFilterDef>.AllDefsListForReading[i];
				if (!specialThingFilterDef.allowedByDefault)
				{
					this.SetAllow(specialThingFilterDef, false);
				}
			}
			if (this.thingDefs != null)
			{
				for (int j = 0; j < this.thingDefs.Count; j++)
				{
					if (this.thingDefs[j] != null)
					{
						this.SetAllow(this.thingDefs[j], true);
					}
					else
					{
						Log.Error("ThingFilter could not find thing def named " + this.thingDefs[j], false);
					}
				}
			}
			if (this.categories != null)
			{
				for (int k = 0; k < this.categories.Count; k++)
				{
					ThingCategoryDef named = DefDatabase<ThingCategoryDef>.GetNamed(this.categories[k], true);
					if (named != null)
					{
						this.SetAllow(named, true, null, null);
					}
				}
			}
			if (this.tradeTagsToAllow != null)
			{
				for (int l = 0; l < this.tradeTagsToAllow.Count; l++)
				{
					List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
					for (int m = 0; m < allDefsListForReading.Count; m++)
					{
						ThingDef thingDef = allDefsListForReading[m];
						if (thingDef.tradeTags != null && thingDef.tradeTags.Contains(this.tradeTagsToAllow[l]))
						{
							this.SetAllow(thingDef, true);
						}
					}
				}
			}
			if (this.tradeTagsToDisallow != null)
			{
				for (int n = 0; n < this.tradeTagsToDisallow.Count; n++)
				{
					List<ThingDef> allDefsListForReading2 = DefDatabase<ThingDef>.AllDefsListForReading;
					for (int num = 0; num < allDefsListForReading2.Count; num++)
					{
						ThingDef thingDef2 = allDefsListForReading2[num];
						if (thingDef2.tradeTags != null && thingDef2.tradeTags.Contains(this.tradeTagsToDisallow[n]))
						{
							this.SetAllow(thingDef2, false);
						}
					}
				}
			}
			if (this.thingSetMakerTagsToAllow != null)
			{
				for (int num2 = 0; num2 < this.thingSetMakerTagsToAllow.Count; num2++)
				{
					List<ThingDef> allDefsListForReading3 = DefDatabase<ThingDef>.AllDefsListForReading;
					for (int num3 = 0; num3 < allDefsListForReading3.Count; num3++)
					{
						ThingDef thingDef3 = allDefsListForReading3[num3];
						if (thingDef3.thingSetMakerTags != null && thingDef3.thingSetMakerTags.Contains(this.thingSetMakerTagsToAllow[num2]))
						{
							this.SetAllow(thingDef3, true);
						}
					}
				}
			}
			if (this.thingSetMakerTagsToDisallow != null)
			{
				for (int num4 = 0; num4 < this.thingSetMakerTagsToDisallow.Count; num4++)
				{
					List<ThingDef> allDefsListForReading4 = DefDatabase<ThingDef>.AllDefsListForReading;
					for (int num5 = 0; num5 < allDefsListForReading4.Count; num5++)
					{
						ThingDef thingDef4 = allDefsListForReading4[num5];
						if (thingDef4.thingSetMakerTags != null && thingDef4.thingSetMakerTags.Contains(this.thingSetMakerTagsToDisallow[num4]))
						{
							this.SetAllow(thingDef4, false);
						}
					}
				}
			}
			if (this.disallowedCategories != null)
			{
				for (int num6 = 0; num6 < this.disallowedCategories.Count; num6++)
				{
					ThingCategoryDef named2 = DefDatabase<ThingCategoryDef>.GetNamed(this.disallowedCategories[num6], true);
					if (named2 != null)
					{
						this.SetAllow(named2, false, null, null);
					}
				}
			}
			if (this.specialFiltersToAllow != null)
			{
				for (int num7 = 0; num7 < this.specialFiltersToAllow.Count; num7++)
				{
					this.SetAllow(SpecialThingFilterDef.Named(this.specialFiltersToAllow[num7]), true);
				}
			}
			if (this.specialFiltersToDisallow != null)
			{
				for (int num8 = 0; num8 < this.specialFiltersToDisallow.Count; num8++)
				{
					this.SetAllow(SpecialThingFilterDef.Named(this.specialFiltersToDisallow[num8]), false);
				}
			}
			if (this.stuffCategoriesToAllow != null)
			{
				for (int num9 = 0; num9 < this.stuffCategoriesToAllow.Count; num9++)
				{
					this.SetAllow(this.stuffCategoriesToAllow[num9], true);
				}
			}
			if (this.allowAllWhoCanMake != null)
			{
				for (int num10 = 0; num10 < this.allowAllWhoCanMake.Count; num10++)
				{
					this.SetAllowAllWhoCanMake(this.allowAllWhoCanMake[num10]);
				}
			}
			if (this.disallowWorsePreferability != FoodPreferability.Undefined)
			{
				List<ThingDef> allDefsListForReading5 = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int num11 = 0; num11 < allDefsListForReading5.Count; num11++)
				{
					ThingDef thingDef5 = allDefsListForReading5[num11];
					if (thingDef5.IsIngestible && thingDef5.ingestible.preferability != FoodPreferability.Undefined && thingDef5.ingestible.preferability < this.disallowWorsePreferability)
					{
						this.SetAllow(thingDef5, false);
					}
				}
			}
			if (this.disallowInedibleByHuman)
			{
				List<ThingDef> allDefsListForReading6 = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int num12 = 0; num12 < allDefsListForReading6.Count; num12++)
				{
					ThingDef thingDef6 = allDefsListForReading6[num12];
					if (thingDef6.IsIngestible && !ThingDefOf.Human.race.CanEverEat(thingDef6))
					{
						this.SetAllow(thingDef6, false);
					}
				}
			}
			if (this.allowWithComp != null)
			{
				List<ThingDef> allDefsListForReading7 = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int num13 = 0; num13 < allDefsListForReading7.Count; num13++)
				{
					ThingDef thingDef7 = allDefsListForReading7[num13];
					if (thingDef7.HasComp(this.allowWithComp))
					{
						this.SetAllow(thingDef7, true);
					}
				}
			}
			if (this.disallowWithComp != null)
			{
				List<ThingDef> allDefsListForReading8 = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int num14 = 0; num14 < allDefsListForReading8.Count; num14++)
				{
					ThingDef thingDef8 = allDefsListForReading8[num14];
					if (thingDef8.HasComp(this.disallowWithComp))
					{
						this.SetAllow(thingDef8, false);
					}
				}
			}
			if (this.disallowCheaperThan != -3.40282347E+38f)
			{
				List<ThingDef> list = new List<ThingDef>();
				foreach (ThingDef thingDef9 in this.allowedDefs)
				{
					if (thingDef9.BaseMarketValue < this.disallowCheaperThan)
					{
						list.Add(thingDef9);
					}
				}
				for (int num15 = 0; num15 < list.Count; num15++)
				{
					this.SetAllow(list[num15], false);
				}
			}
			if (this.disallowedThingDefs != null)
			{
				for (int num16 = 0; num16 < this.disallowedThingDefs.Count; num16++)
				{
					if (this.disallowedThingDefs[num16] != null)
					{
						this.SetAllow(this.disallowedThingDefs[num16], false);
					}
					else
					{
						Log.Error("ThingFilter could not find excepted thing def named " + this.disallowedThingDefs[num16], false);
					}
				}
			}
		}

		// Token: 0x060021F8 RID: 8696 RVA: 0x000CF230 File Offset: 0x000CD430
		public void RecalculateDisplayRootCategory()
		{
			if (ThingCategoryNodeDatabase.allThingCategoryNodes == null)
			{
				this.DisplayRootCategory = ThingCategoryNodeDatabase.RootNode;
				return;
			}
			int lastFoundCategory = -1;
			object lockObject = new object();
			GenThreading.ParallelFor(0, ThingCategoryNodeDatabase.allThingCategoryNodes.Count, delegate(int index)
			{
				TreeNode_ThingCategory treeNode_ThingCategory = ThingCategoryNodeDatabase.allThingCategoryNodes[index];
				bool flag = false;
				bool flag2 = false;
				foreach (ThingDef thingDef in this.allowedDefs)
				{
					if (treeNode_ThingCategory.catDef.ContainedInThisOrDescendant(thingDef))
					{
						flag2 = true;
					}
					else
					{
						flag = true;
					}
				}
				if (!flag && flag2)
				{
					object lockObject = lockObject;
					lock (lockObject)
					{
						if (index > lastFoundCategory)
						{
							lastFoundCategory = index;
						}
					}
				}
			}, -1);
			if (lastFoundCategory == -1)
			{
				this.DisplayRootCategory = ThingCategoryNodeDatabase.RootNode;
				return;
			}
			this.DisplayRootCategory = ThingCategoryNodeDatabase.allThingCategoryNodes[lastFoundCategory];
		}

		// Token: 0x060021F9 RID: 8697 RVA: 0x000CF2B8 File Offset: 0x000CD4B8
		private void RecalculateSpecialFilterConfigurability()
		{
			if (this.DisplayRootCategory == null)
			{
				this.allowedHitPointsConfigurable = true;
				this.allowedQualitiesConfigurable = true;
				return;
			}
			this.allowedHitPointsConfigurable = false;
			this.allowedQualitiesConfigurable = false;
			foreach (ThingDef thingDef in this.DisplayRootCategory.catDef.DescendantThingDefs)
			{
				if (thingDef.useHitPoints)
				{
					this.allowedHitPointsConfigurable = true;
				}
				if (thingDef.HasComp(typeof(CompQuality)))
				{
					this.allowedQualitiesConfigurable = true;
				}
				if (this.allowedHitPointsConfigurable && this.allowedQualitiesConfigurable)
				{
					break;
				}
			}
		}

		// Token: 0x060021FA RID: 8698 RVA: 0x000CF368 File Offset: 0x000CD568
		public bool IsAlwaysDisallowedDueToSpecialFilters(ThingDef def)
		{
			for (int i = 0; i < this.disallowedSpecialFilters.Count; i++)
			{
				if (this.disallowedSpecialFilters[i].Worker.AlwaysMatches(def))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060021FB RID: 8699 RVA: 0x000CF3A8 File Offset: 0x000CD5A8
		public virtual void CopyAllowancesFrom(ThingFilter other)
		{
			this.allowedDefs.Clear();
			foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefs)
			{
				this.SetAllow(thingDef, other.Allows(thingDef));
			}
			this.disallowedSpecialFilters = other.disallowedSpecialFilters.ListFullCopyOrNull<SpecialThingFilterDef>();
			this.allowedHitPointsPercents = other.allowedHitPointsPercents;
			this.allowedHitPointsConfigurable = other.allowedHitPointsConfigurable;
			this.allowedQualities = other.allowedQualities;
			this.allowedQualitiesConfigurable = other.allowedQualitiesConfigurable;
			this.thingDefs = other.thingDefs.ListFullCopyOrNull<ThingDef>();
			this.categories = other.categories.ListFullCopyOrNull<string>();
			this.tradeTagsToAllow = other.tradeTagsToAllow.ListFullCopyOrNull<string>();
			this.tradeTagsToDisallow = other.tradeTagsToDisallow.ListFullCopyOrNull<string>();
			this.thingSetMakerTagsToAllow = other.thingSetMakerTagsToAllow.ListFullCopyOrNull<string>();
			this.thingSetMakerTagsToDisallow = other.thingSetMakerTagsToDisallow.ListFullCopyOrNull<string>();
			this.disallowedCategories = other.disallowedCategories.ListFullCopyOrNull<string>();
			this.specialFiltersToAllow = other.specialFiltersToAllow.ListFullCopyOrNull<string>();
			this.specialFiltersToDisallow = other.specialFiltersToDisallow.ListFullCopyOrNull<string>();
			this.stuffCategoriesToAllow = other.stuffCategoriesToAllow.ListFullCopyOrNull<StuffCategoryDef>();
			this.allowAllWhoCanMake = other.allowAllWhoCanMake.ListFullCopyOrNull<ThingDef>();
			this.disallowWorsePreferability = other.disallowWorsePreferability;
			this.disallowInedibleByHuman = other.disallowInedibleByHuman;
			this.allowWithComp = other.allowWithComp;
			this.disallowWithComp = other.disallowWithComp;
			this.disallowCheaperThan = other.disallowCheaperThan;
			this.disallowedThingDefs = other.disallowedThingDefs.ListFullCopyOrNull<ThingDef>();
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
		}

		// Token: 0x060021FC RID: 8700 RVA: 0x000CF564 File Offset: 0x000CD764
		public void SetAllow(ThingDef thingDef, bool allow)
		{
			if (allow == this.Allows(thingDef))
			{
				return;
			}
			if (allow)
			{
				this.allowedDefs.Add(thingDef);
			}
			else
			{
				this.allowedDefs.Remove(thingDef);
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x060021FD RID: 8701 RVA: 0x000CF5B8 File Offset: 0x000CD7B8
		public void SetAllow(SpecialThingFilterDef sfDef, bool allow)
		{
			if (!sfDef.configurable)
			{
				return;
			}
			if (allow == this.Allows(sfDef))
			{
				return;
			}
			if (allow)
			{
				if (this.disallowedSpecialFilters.Contains(sfDef))
				{
					this.disallowedSpecialFilters.Remove(sfDef);
				}
			}
			else if (!this.disallowedSpecialFilters.Contains(sfDef))
			{
				this.disallowedSpecialFilters.Add(sfDef);
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x060021FE RID: 8702 RVA: 0x000CF630 File Offset: 0x000CD830
		public void SetAllow(ThingCategoryDef categoryDef, bool allow, IEnumerable<ThingDef> exceptedDefs = null, IEnumerable<SpecialThingFilterDef> exceptedFilters = null)
		{
			if (!ThingCategoryNodeDatabase.initialized)
			{
				Log.Error("SetAllow categories won't work before ThingCategoryDatabase is initialized.", false);
			}
			foreach (ThingDef thingDef in categoryDef.DescendantThingDefs)
			{
				if (exceptedDefs == null || !exceptedDefs.Contains(thingDef))
				{
					this.SetAllow(thingDef, allow);
				}
			}
			foreach (SpecialThingFilterDef specialThingFilterDef in categoryDef.DescendantSpecialThingFilterDefs)
			{
				if (exceptedFilters == null || !exceptedFilters.Contains(specialThingFilterDef))
				{
					this.SetAllow(specialThingFilterDef, allow);
				}
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x060021FF RID: 8703 RVA: 0x000CF704 File Offset: 0x000CD904
		public void SetAllow(StuffCategoryDef cat, bool allow)
		{
			for (int i = 0; i < DefDatabase<ThingDef>.AllDefsListForReading.Count; i++)
			{
				ThingDef thingDef = DefDatabase<ThingDef>.AllDefsListForReading[i];
				if (thingDef.IsStuff && thingDef.stuffProps.categories.Contains(cat))
				{
					this.SetAllow(thingDef, allow);
				}
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x06002200 RID: 8704 RVA: 0x000CF770 File Offset: 0x000CD970
		public void SetAllowAllWhoCanMake(ThingDef thing)
		{
			for (int i = 0; i < DefDatabase<ThingDef>.AllDefsListForReading.Count; i++)
			{
				ThingDef thingDef = DefDatabase<ThingDef>.AllDefsListForReading[i];
				if (thingDef.IsStuff && thingDef.stuffProps.CanMake(thing))
				{
					this.SetAllow(thingDef, true);
				}
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x06002201 RID: 8705 RVA: 0x000CF7D8 File Offset: 0x000CD9D8
		public void SetFromPreset(StorageSettingsPreset preset)
		{
			if (preset == StorageSettingsPreset.DefaultStockpile)
			{
				this.SetAllow(ThingCategoryDefOf.Foods, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Manufactured, true, null, null);
				this.SetAllow(ThingCategoryDefOf.ResourcesRaw, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Items, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Buildings, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Weapons, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Apparel, true, null, null);
				this.SetAllow(ThingCategoryDefOf.BodyParts, true, null, null);
			}
			if (preset == StorageSettingsPreset.DumpingStockpile)
			{
				this.SetAllow(ThingCategoryDefOf.Corpses, true, null, null);
				this.SetAllow(ThingCategoryDefOf.Chunks, true, null, null);
			}
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x06002202 RID: 8706 RVA: 0x000CF894 File Offset: 0x000CDA94
		public void SetDisallowAll(IEnumerable<ThingDef> exceptedDefs = null, IEnumerable<SpecialThingFilterDef> exceptedFilters = null)
		{
			this.allowedDefs.RemoveWhere((ThingDef d) => exceptedDefs == null || !exceptedDefs.Contains(d));
			this.disallowedSpecialFilters.RemoveAll((SpecialThingFilterDef sf) => sf.configurable && (exceptedFilters == null || !exceptedFilters.Contains(sf)));
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x06002203 RID: 8707 RVA: 0x000CF900 File Offset: 0x000CDB00
		public void SetAllowAll(ThingFilter parentFilter, bool includeNonStorable = false)
		{
			this.allowedDefs.Clear();
			if (parentFilter != null)
			{
				using (HashSet<ThingDef>.Enumerator enumerator = parentFilter.allowedDefs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ThingDef item = enumerator.Current;
						this.allowedDefs.Add(item);
					}
					goto IL_B9;
				}
			}
			if (includeNonStorable)
			{
				using (IEnumerator<ThingDef> enumerator2 = DefDatabase<ThingDef>.AllDefs.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						ThingDef item2 = enumerator2.Current;
						this.allowedDefs.Add(item2);
					}
					goto IL_B9;
				}
			}
			foreach (ThingDef item3 in ThingFilter.AllStorableThingDefs)
			{
				this.allowedDefs.Add(item3);
			}
			IL_B9:
			this.disallowedSpecialFilters.RemoveAll((SpecialThingFilterDef sf) => sf.configurable);
			if (this.settingsChangedCallback != null)
			{
				this.settingsChangedCallback();
			}
			this.displayRootCategoryInt = null;
		}

		// Token: 0x06002204 RID: 8708 RVA: 0x000CFA34 File Offset: 0x000CDC34
		public virtual bool Allows(Thing t)
		{
			t = t.GetInnerIfMinified();
			if (!this.Allows(t.def))
			{
				return false;
			}
			if (t.def.useHitPoints)
			{
				float num = (float)t.HitPoints / (float)t.MaxHitPoints;
				num = GenMath.RoundedHundredth(num);
				if (!this.allowedHitPointsPercents.IncludesEpsilon(Mathf.Clamp01(num)))
				{
					return false;
				}
			}
			if (this.allowedQualities != QualityRange.All && t.def.FollowQualityThingFilter())
			{
				QualityCategory p;
				if (!t.TryGetQuality(out p))
				{
					p = QualityCategory.Normal;
				}
				if (!this.allowedQualities.Includes(p))
				{
					return false;
				}
			}
			for (int i = 0; i < this.disallowedSpecialFilters.Count; i++)
			{
				if (this.disallowedSpecialFilters[i].Worker.Matches(t) && t.def.IsWithinCategory(this.disallowedSpecialFilters[i].parentCategory))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002205 RID: 8709 RVA: 0x000CFB1D File Offset: 0x000CDD1D
		public bool Allows(ThingDef def)
		{
			return this.allowedDefs.Contains(def);
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x000CFB2B File Offset: 0x000CDD2B
		public bool Allows(SpecialThingFilterDef sf)
		{
			return !this.disallowedSpecialFilters.Contains(sf);
		}

		// Token: 0x06002207 RID: 8711 RVA: 0x000CFB3C File Offset: 0x000CDD3C
		public ThingRequest GetThingRequest()
		{
			if (this.AllowedThingDefs.Any((ThingDef def) => !def.alwaysHaulable))
			{
				return ThingRequest.ForGroup(ThingRequestGroup.HaulableEver);
			}
			return ThingRequest.ForGroup(ThingRequestGroup.HaulableAlways);
		}

		// Token: 0x06002208 RID: 8712 RVA: 0x000CFB77 File Offset: 0x000CDD77
		public override string ToString()
		{
			return this.Summary;
		}

		// Token: 0x040014D9 RID: 5337
		[Unsaved(false)]
		private Action settingsChangedCallback;

		// Token: 0x040014DA RID: 5338
		[Unsaved(false)]
		private TreeNode_ThingCategory displayRootCategoryInt;

		// Token: 0x040014DB RID: 5339
		[Unsaved(false)]
		private HashSet<ThingDef> allowedDefs = new HashSet<ThingDef>();

		// Token: 0x040014DC RID: 5340
		[Unsaved(false)]
		private List<SpecialThingFilterDef> disallowedSpecialFilters = new List<SpecialThingFilterDef>();

		// Token: 0x040014DD RID: 5341
		private FloatRange allowedHitPointsPercents = FloatRange.ZeroToOne;

		// Token: 0x040014DE RID: 5342
		public bool allowedHitPointsConfigurable = true;

		// Token: 0x040014DF RID: 5343
		private QualityRange allowedQualities = QualityRange.All;

		// Token: 0x040014E0 RID: 5344
		public bool allowedQualitiesConfigurable = true;

		// Token: 0x040014E1 RID: 5345
		[MustTranslate]
		public string customSummary;

		// Token: 0x040014E2 RID: 5346
		private List<ThingDef> thingDefs;

		// Token: 0x040014E3 RID: 5347
		[NoTranslate]
		private List<string> categories;

		// Token: 0x040014E4 RID: 5348
		[NoTranslate]
		private List<string> tradeTagsToAllow;

		// Token: 0x040014E5 RID: 5349
		[NoTranslate]
		private List<string> tradeTagsToDisallow;

		// Token: 0x040014E6 RID: 5350
		[NoTranslate]
		private List<string> thingSetMakerTagsToAllow;

		// Token: 0x040014E7 RID: 5351
		[NoTranslate]
		private List<string> thingSetMakerTagsToDisallow;

		// Token: 0x040014E8 RID: 5352
		[NoTranslate]
		private List<string> disallowedCategories;

		// Token: 0x040014E9 RID: 5353
		[NoTranslate]
		private List<string> specialFiltersToAllow;

		// Token: 0x040014EA RID: 5354
		[NoTranslate]
		private List<string> specialFiltersToDisallow;

		// Token: 0x040014EB RID: 5355
		private List<StuffCategoryDef> stuffCategoriesToAllow;

		// Token: 0x040014EC RID: 5356
		private List<ThingDef> allowAllWhoCanMake;

		// Token: 0x040014ED RID: 5357
		private FoodPreferability disallowWorsePreferability;

		// Token: 0x040014EE RID: 5358
		private bool disallowInedibleByHuman;

		// Token: 0x040014EF RID: 5359
		private Type allowWithComp;

		// Token: 0x040014F0 RID: 5360
		private Type disallowWithComp;

		// Token: 0x040014F1 RID: 5361
		private float disallowCheaperThan = float.MinValue;

		// Token: 0x040014F2 RID: 5362
		private List<ThingDef> disallowedThingDefs;
	}
}
