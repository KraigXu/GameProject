using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200089A RID: 2202
	public class PlantProperties
	{
		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x0600356C RID: 13676 RVA: 0x001238A7 File Offset: 0x00121AA7
		public bool Sowable
		{
			get
			{
				return !this.sowTags.NullOrEmpty<string>();
			}
		}

		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x0600356D RID: 13677 RVA: 0x001238B7 File Offset: 0x00121AB7
		public bool Harvestable
		{
			get
			{
				return this.harvestYield > 0.001f;
			}
		}

		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x0600356E RID: 13678 RVA: 0x001238C6 File Offset: 0x00121AC6
		public bool HarvestDestroys
		{
			get
			{
				return this.harvestAfterGrowth <= 0f;
			}
		}

		// Token: 0x17000980 RID: 2432
		// (get) Token: 0x0600356F RID: 13679 RVA: 0x001238D8 File Offset: 0x00121AD8
		public bool IsTree
		{
			get
			{
				return this.harvestTag == "Wood";
			}
		}

		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x06003570 RID: 13680 RVA: 0x001238EA File Offset: 0x00121AEA
		public float LifespanDays
		{
			get
			{
				return this.growDays * this.lifespanDaysPerGrowDays;
			}
		}

		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x06003571 RID: 13681 RVA: 0x001238F9 File Offset: 0x00121AF9
		public int LifespanTicks
		{
			get
			{
				return (int)(this.LifespanDays * 60000f);
			}
		}

		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x06003572 RID: 13682 RVA: 0x00123908 File Offset: 0x00121B08
		public bool LimitedLifespan
		{
			get
			{
				return this.lifespanDaysPerGrowDays > 0f;
			}
		}

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x06003573 RID: 13683 RVA: 0x00123917 File Offset: 0x00121B17
		public bool Blightable
		{
			get
			{
				return this.Sowable && this.Harvestable && !this.neverBlightable;
			}
		}

		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x06003574 RID: 13684 RVA: 0x00123934 File Offset: 0x00121B34
		public bool GrowsInClusters
		{
			get
			{
				return this.wildClusterRadius > 0;
			}
		}

		// Token: 0x06003575 RID: 13685 RVA: 0x00123940 File Offset: 0x00121B40
		public void PostLoadSpecial(ThingDef parentDef)
		{
			if (!this.leaflessGraphicPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.leaflessGraphic = GraphicDatabase.Get(parentDef.graphicData.graphicClass, this.leaflessGraphicPath, parentDef.graphic.Shader, parentDef.graphicData.drawSize, parentDef.graphicData.color, parentDef.graphicData.colorTwo);
				});
			}
			if (!this.immatureGraphicPath.NullOrEmpty())
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.immatureGraphic = GraphicDatabase.Get(parentDef.graphicData.graphicClass, this.immatureGraphicPath, parentDef.graphic.Shader, parentDef.graphicData.drawSize, parentDef.graphicData.color, parentDef.graphicData.colorTwo);
				});
			}
		}

		// Token: 0x06003576 RID: 13686 RVA: 0x0012399D File Offset: 0x00121B9D
		public IEnumerable<string> ConfigErrors()
		{
			if (this.maxMeshCount > 25)
			{
				yield return "maxMeshCount > MaxMaxMeshCount";
			}
			yield break;
		}

		// Token: 0x06003577 RID: 13687 RVA: 0x001239AD File Offset: 0x00121BAD
		private IEnumerable<Dialog_InfoCard.Hyperlink> GetHarvestYieldHyperlinks()
		{
			yield return new Dialog_InfoCard.Hyperlink(this.harvestedThingDef, -1);
			yield break;
		}

		// Token: 0x06003578 RID: 13688 RVA: 0x001239BD File Offset: 0x00121BBD
		internal IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.sowMinSkill > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MinGrowingSkillToSow".Translate(), this.sowMinSkill.ToString(), "Stat_Thing_Plant_MinGrowingSkillToSow_Desc".Translate(), 4151, null, null, false);
			}
			string attributes = "";
			if (this.Harvestable)
			{
				string text = "Harvestable".Translate();
				if (!attributes.NullOrEmpty())
				{
					attributes += ", ";
					text = text.UncapitalizeFirst();
				}
				attributes += text;
			}
			if (this.LimitedLifespan)
			{
				string text2 = "LimitedLifespan".Translate();
				if (!attributes.NullOrEmpty())
				{
					attributes += ", ";
					text2 = text2.UncapitalizeFirst();
				}
				attributes += text2;
			}
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "GrowingTime".Translate(), this.growDays.ToString("0.##") + " " + "Days".Translate(), "GrowingTimeDesc".Translate(), 4158, null, null, false);
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "FertilityRequirement".Translate(), this.fertilityMin.ToStringPercent(), "Stat_Thing_Plant_FertilityRequirement_Desc".Translate(), 4156, null, null, false);
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "FertilitySensitivity".Translate(), this.fertilitySensitivity.ToStringPercent(), "Stat_Thing_Plant_FertilitySensitivity_Desc".Translate(), 4155, null, null, false);
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "LightRequirement".Translate(), this.growMinGlow.ToStringPercent(), "Stat_Thing_Plant_LightRequirement_Desc".Translate(), 4154, null, null, false);
			if (!attributes.NullOrEmpty())
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Attributes".Translate(), attributes, "Stat_Thing_Plant_Attributes_Desc".Translate(), 4157, null, null, false);
			}
			if (this.LimitedLifespan)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "LifeSpan".Translate(), this.LifespanDays.ToString("0.##") + " " + "Days".Translate(), "Stat_Thing_Plant_LifeSpan_Desc".Translate(), 4150, null, null, false);
			}
			if (this.harvestYield > 0f)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Stat_Thing_Plant_HarvestYield_Desc".Translate());
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("StatsReport_DifficultyMultiplier".Translate(Find.Storyteller.difficulty.label) + ": " + Find.Storyteller.difficulty.cropYieldFactor.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor));
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "HarvestYield".Translate(), Mathf.CeilToInt(this.harvestYield * Find.Storyteller.difficulty.cropYieldFactor).ToString("F0"), stringBuilder.ToString(), 4150, null, this.GetHarvestYieldHyperlinks(), false);
			}
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MinGrowthTemperature".Translate(), 0f.ToStringTemperature("F1"), "Stat_Thing_Plant_MinGrowthTemperature_Desc".Translate(), 4152, null, null, false);
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "MaxGrowthTemperature".Translate(), 58f.ToStringTemperature("F1"), "Stat_Thing_Plant_MaxGrowthTemperature_Desc".Translate(), 4153, null, null, false);
			yield break;
		}

		// Token: 0x04001D20 RID: 7456
		public List<PlantBiomeRecord> wildBiomes;

		// Token: 0x04001D21 RID: 7457
		public int wildClusterRadius = -1;

		// Token: 0x04001D22 RID: 7458
		public float wildClusterWeight = 15f;

		// Token: 0x04001D23 RID: 7459
		public float wildOrder = 2f;

		// Token: 0x04001D24 RID: 7460
		public bool wildEqualLocalDistribution = true;

		// Token: 0x04001D25 RID: 7461
		public bool cavePlant;

		// Token: 0x04001D26 RID: 7462
		public float cavePlantWeight = 1f;

		// Token: 0x04001D27 RID: 7463
		[NoTranslate]
		public List<string> sowTags = new List<string>();

		// Token: 0x04001D28 RID: 7464
		public float sowWork = 10f;

		// Token: 0x04001D29 RID: 7465
		public int sowMinSkill;

		// Token: 0x04001D2A RID: 7466
		public bool blockAdjacentSow;

		// Token: 0x04001D2B RID: 7467
		public List<ResearchProjectDef> sowResearchPrerequisites;

		// Token: 0x04001D2C RID: 7468
		public bool mustBeWildToSow;

		// Token: 0x04001D2D RID: 7469
		public float harvestWork = 10f;

		// Token: 0x04001D2E RID: 7470
		public float harvestYield;

		// Token: 0x04001D2F RID: 7471
		public ThingDef harvestedThingDef;

		// Token: 0x04001D30 RID: 7472
		[NoTranslate]
		public string harvestTag;

		// Token: 0x04001D31 RID: 7473
		public float harvestMinGrowth = 0.65f;

		// Token: 0x04001D32 RID: 7474
		public float harvestAfterGrowth;

		// Token: 0x04001D33 RID: 7475
		public bool harvestFailable = true;

		// Token: 0x04001D34 RID: 7476
		public SoundDef soundHarvesting;

		// Token: 0x04001D35 RID: 7477
		public SoundDef soundHarvestFinish;

		// Token: 0x04001D36 RID: 7478
		public float growDays = 2f;

		// Token: 0x04001D37 RID: 7479
		public float lifespanDaysPerGrowDays = 8f;

		// Token: 0x04001D38 RID: 7480
		public float growMinGlow = 0.51f;

		// Token: 0x04001D39 RID: 7481
		public float growOptimalGlow = 1f;

		// Token: 0x04001D3A RID: 7482
		public float fertilityMin = 0.9f;

		// Token: 0x04001D3B RID: 7483
		public float fertilitySensitivity = 0.5f;

		// Token: 0x04001D3C RID: 7484
		public bool dieIfLeafless;

		// Token: 0x04001D3D RID: 7485
		public bool neverBlightable;

		// Token: 0x04001D3E RID: 7486
		public bool interferesWithRoof;

		// Token: 0x04001D3F RID: 7487
		public bool dieIfNoSunlight = true;

		// Token: 0x04001D40 RID: 7488
		public bool dieFromToxicFallout = true;

		// Token: 0x04001D41 RID: 7489
		public PlantPurpose purpose = PlantPurpose.Misc;

		// Token: 0x04001D42 RID: 7490
		public float topWindExposure = 0.25f;

		// Token: 0x04001D43 RID: 7491
		public int maxMeshCount = 1;

		// Token: 0x04001D44 RID: 7492
		public FloatRange visualSizeRange = new FloatRange(0.9f, 1.1f);

		// Token: 0x04001D45 RID: 7493
		[NoTranslate]
		private string leaflessGraphicPath;

		// Token: 0x04001D46 RID: 7494
		[Unsaved(false)]
		public Graphic leaflessGraphic;

		// Token: 0x04001D47 RID: 7495
		[NoTranslate]
		private string immatureGraphicPath;

		// Token: 0x04001D48 RID: 7496
		[Unsaved(false)]
		public Graphic immatureGraphic;

		// Token: 0x04001D49 RID: 7497
		public bool dropLeaves;

		// Token: 0x04001D4A RID: 7498
		public const int MaxMaxMeshCount = 25;
	}
}
