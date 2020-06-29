using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class PlantProperties
	{
		
		// (get) Token: 0x0600356C RID: 13676 RVA: 0x001238A7 File Offset: 0x00121AA7
		public bool Sowable
		{
			get
			{
				return !this.sowTags.NullOrEmpty<string>();
			}
		}

		
		// (get) Token: 0x0600356D RID: 13677 RVA: 0x001238B7 File Offset: 0x00121AB7
		public bool Harvestable
		{
			get
			{
				return this.harvestYield > 0.001f;
			}
		}

		
		// (get) Token: 0x0600356E RID: 13678 RVA: 0x001238C6 File Offset: 0x00121AC6
		public bool HarvestDestroys
		{
			get
			{
				return this.harvestAfterGrowth <= 0f;
			}
		}

		
		// (get) Token: 0x0600356F RID: 13679 RVA: 0x001238D8 File Offset: 0x00121AD8
		public bool IsTree
		{
			get
			{
				return this.harvestTag == "Wood";
			}
		}

		
		// (get) Token: 0x06003570 RID: 13680 RVA: 0x001238EA File Offset: 0x00121AEA
		public float LifespanDays
		{
			get
			{
				return this.growDays * this.lifespanDaysPerGrowDays;
			}
		}

		
		// (get) Token: 0x06003571 RID: 13681 RVA: 0x001238F9 File Offset: 0x00121AF9
		public int LifespanTicks
		{
			get
			{
				return (int)(this.LifespanDays * 60000f);
			}
		}

		
		// (get) Token: 0x06003572 RID: 13682 RVA: 0x00123908 File Offset: 0x00121B08
		public bool LimitedLifespan
		{
			get
			{
				return this.lifespanDaysPerGrowDays > 0f;
			}
		}

		
		// (get) Token: 0x06003573 RID: 13683 RVA: 0x00123917 File Offset: 0x00121B17
		public bool Blightable
		{
			get
			{
				return this.Sowable && this.Harvestable && !this.neverBlightable;
			}
		}

		
		// (get) Token: 0x06003574 RID: 13684 RVA: 0x00123934 File Offset: 0x00121B34
		public bool GrowsInClusters
		{
			get
			{
				return this.wildClusterRadius > 0;
			}
		}

		
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

		
		public IEnumerable<string> ConfigErrors()
		{
			if (this.maxMeshCount > 25)
			{
				yield return "maxMeshCount > MaxMaxMeshCount";
			}
			yield break;
		}

		
		private IEnumerable<Dialog_InfoCard.Hyperlink> GetHarvestYieldHyperlinks()
		{
			yield return new Dialog_InfoCard.Hyperlink(this.harvestedThingDef, -1);
			yield break;
		}

		
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

		
		public List<PlantBiomeRecord> wildBiomes;

		
		public int wildClusterRadius = -1;

		
		public float wildClusterWeight = 15f;

		
		public float wildOrder = 2f;

		
		public bool wildEqualLocalDistribution = true;

		
		public bool cavePlant;

		
		public float cavePlantWeight = 1f;

		
		[NoTranslate]
		public List<string> sowTags = new List<string>();

		
		public float sowWork = 10f;

		
		public int sowMinSkill;

		
		public bool blockAdjacentSow;

		
		public List<ResearchProjectDef> sowResearchPrerequisites;

		
		public bool mustBeWildToSow;

		
		public float harvestWork = 10f;

		
		public float harvestYield;

		
		public ThingDef harvestedThingDef;

		
		[NoTranslate]
		public string harvestTag;

		
		public float harvestMinGrowth = 0.65f;

		
		public float harvestAfterGrowth;

		
		public bool harvestFailable = true;

		
		public SoundDef soundHarvesting;

		
		public SoundDef soundHarvestFinish;

		
		public float growDays = 2f;

		
		public float lifespanDaysPerGrowDays = 8f;

		
		public float growMinGlow = 0.51f;

		
		public float growOptimalGlow = 1f;

		
		public float fertilityMin = 0.9f;

		
		public float fertilitySensitivity = 0.5f;

		
		public bool dieIfLeafless;

		
		public bool neverBlightable;

		
		public bool interferesWithRoof;

		
		public bool dieIfNoSunlight = true;

		
		public bool dieFromToxicFallout = true;

		
		public PlantPurpose purpose = PlantPurpose.Misc;

		
		public float topWindExposure = 0.25f;

		
		public int maxMeshCount = 1;

		
		public FloatRange visualSizeRange = new FloatRange(0.9f, 1.1f);

		
		[NoTranslate]
		private string leaflessGraphicPath;

		
		[Unsaved(false)]
		public Graphic leaflessGraphic;

		
		[NoTranslate]
		private string immatureGraphicPath;

		
		[Unsaved(false)]
		public Graphic immatureGraphic;

		
		public bool dropLeaves;

		
		public const int MaxMaxMeshCount = 25;
	}
}
