using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	
	public class ResearchProjectDef : Def
	{
		
		// (get) Token: 0x06000609 RID: 1545 RVA: 0x0001CFD5 File Offset: 0x0001B1D5
		public float ResearchViewX
		{
			get
			{
				return this.x;
			}
		}

		
		// (get) Token: 0x0600060A RID: 1546 RVA: 0x0001CFDD File Offset: 0x0001B1DD
		public float ResearchViewY
		{
			get
			{
				return this.y;
			}
		}

		
		// (get) Token: 0x0600060B RID: 1547 RVA: 0x0001CFE5 File Offset: 0x0001B1E5
		public float CostApparent
		{
			get
			{
				return this.baseCost * this.CostFactor(Faction.OfPlayer.def.techLevel);
			}
		}

		
		// (get) Token: 0x0600060C RID: 1548 RVA: 0x0001D003 File Offset: 0x0001B203
		public float ProgressReal
		{
			get
			{
				return Find.ResearchManager.GetProgress(this);
			}
		}

		
		// (get) Token: 0x0600060D RID: 1549 RVA: 0x0001D010 File Offset: 0x0001B210
		public float ProgressApparent
		{
			get
			{
				return this.ProgressReal * this.CostFactor(Faction.OfPlayer.def.techLevel);
			}
		}

		
		// (get) Token: 0x0600060E RID: 1550 RVA: 0x0001D02E File Offset: 0x0001B22E
		public float ProgressPercent
		{
			get
			{
				return Find.ResearchManager.GetProgress(this) / this.baseCost;
			}
		}

		
		// (get) Token: 0x0600060F RID: 1551 RVA: 0x0001D042 File Offset: 0x0001B242
		public bool IsFinished
		{
			get
			{
				return this.ProgressReal >= this.baseCost;
			}
		}

		
		// (get) Token: 0x06000610 RID: 1552 RVA: 0x0001D055 File Offset: 0x0001B255
		public bool CanStartNow
		{
			get
			{
				return !this.IsFinished && this.PrerequisitesCompleted && this.TechprintRequirementMet && (this.requiredResearchBuilding == null || this.PlayerHasAnyAppropriateResearchBench);
			}
		}

		
		// (get) Token: 0x06000611 RID: 1553 RVA: 0x0001D084 File Offset: 0x0001B284
		public bool PrerequisitesCompleted
		{
			get
			{
				if (this.prerequisites != null)
				{
					for (int i = 0; i < this.prerequisites.Count; i++)
					{
						if (!this.prerequisites[i].IsFinished)
						{
							return false;
						}
					}
				}
				if (this.hiddenPrerequisites != null)
				{
					for (int j = 0; j < this.hiddenPrerequisites.Count; j++)
					{
						if (!this.hiddenPrerequisites[j].IsFinished)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		
		// (get) Token: 0x06000612 RID: 1554 RVA: 0x0001D0F8 File Offset: 0x0001B2F8
		public int TechprintsApplied
		{
			get
			{
				return Find.ResearchManager.GetTechprints(this);
			}
		}

		
		// (get) Token: 0x06000613 RID: 1555 RVA: 0x0001D105 File Offset: 0x0001B305
		public bool TechprintRequirementMet
		{
			get
			{
				return this.techprintCount <= 0 || Find.ResearchManager.GetTechprints(this) >= this.techprintCount;
			}
		}

		
		// (get) Token: 0x06000614 RID: 1556 RVA: 0x0001D128 File Offset: 0x0001B328
		public ThingDef Techprint
		{
			get
			{
				if (this.techprintCount <= 0)
				{
					return null;
				}
				if (this.cachedTechprint == null)
				{
					this.cachedTechprint = DefDatabase<ThingDef>.AllDefs.FirstOrDefault(delegate(ThingDef x)
					{
						CompProperties_Techprint compProperties = x.GetCompProperties<CompProperties_Techprint>();
						return compProperties != null && compProperties.project == this;
					});
					if (this.cachedTechprint == null)
					{
						Log.ErrorOnce("Could not find techprint for research project " + this, (int)this.shortHash ^ 873231450, false);
					}
				}
				return this.cachedTechprint;
			}
		}

		
		// (get) Token: 0x06000615 RID: 1557 RVA: 0x0001D190 File Offset: 0x0001B390
		public List<Def> UnlockedDefs
		{
			get
			{
				if (this.cachedUnlockedDefs == null)
				{
					//this.cachedUnlockedDefs = (from x in (from x in DefDatabase<RecipeDef>.AllDefs
					//where x.researchPrerequisite == this || (x.researchPrerequisites != null && x.researchPrerequisites.Contains(this))
					//select x).SelectMany((RecipeDef x) => from y in x.products
					//select y.thingDef)
					//orderby x.label
					//select x).Concat(from x in DefDatabase<ThingDef>.AllDefs
					//where x.researchPrerequisites != null && x.researchPrerequisites.Contains(this)
					//orderby x.label
					//select x).Concat(from x in DefDatabase<ThingDef>.AllDefs
					//where x.plant != null && x.plant.sowResearchPrerequisites != null && x.plant.sowResearchPrerequisites.Contains(this)
					//orderby x.label
					//select x).Concat(from x in DefDatabase<TerrainDef>.AllDefs
					//where x.researchPrerequisites != null && x.researchPrerequisites.Contains(this)
					//orderby x.label
					//select x).Distinct<Def>().ToList<Def>();
				}
				return this.cachedUnlockedDefs;
			}
		}

		
		// (get) Token: 0x06000616 RID: 1558 RVA: 0x0001D2DC File Offset: 0x0001B4DC
		public List<Dialog_InfoCard.Hyperlink> InfoCardHyperlinks
		{
			get
			{
				if (this.cachedHyperlinks == null)
				{
					this.cachedHyperlinks = new List<Dialog_InfoCard.Hyperlink>();
					List<Def> unlockedDefs = this.UnlockedDefs;
					if (unlockedDefs != null)
					{
						for (int i = 0; i < unlockedDefs.Count; i++)
						{
							this.cachedHyperlinks.Add(new Dialog_InfoCard.Hyperlink(unlockedDefs[i], -1));
						}
					}
				}
				return this.cachedHyperlinks;
			}
		}

		
		// (get) Token: 0x06000617 RID: 1559 RVA: 0x0001D338 File Offset: 0x0001B538
		private bool PlayerHasAnyAppropriateResearchBench
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Building> allBuildingsColonist = maps[i].listerBuildings.allBuildingsColonist;
					for (int j = 0; j < allBuildingsColonist.Count; j++)
					{
						Building_ResearchBench building_ResearchBench = allBuildingsColonist[j] as Building_ResearchBench;
						if (building_ResearchBench != null && this.CanBeResearchedAt(building_ResearchBench, true))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		
		public override void ResolveReferences()
		{
			if (this.tab == null)
			{
				this.tab = ResearchTabDefOf.Main;
			}
		}

		
		public override IEnumerable<string> ConfigErrors()
		{

			IEnumerator<string> enumerator = null;
			if (this.techLevel == TechLevel.Undefined)
			{
				yield return "techLevel is Undefined";
			}
			if (this.ResearchViewX < 0f || this.ResearchViewY < 0f)
			{
				yield return "researchViewX and/or researchViewY not set";
			}
			if (this.techprintCount == 0 && !this.heldByFactionCategoryTags.NullOrEmpty<string>())
			{
				yield return "requires no techprints but has heldByFactionCategoryTags.";
			}
			if (this.techprintCount > 0 && this.heldByFactionCategoryTags.NullOrEmpty<string>())
			{
				yield return "requires techprints but has no heldByFactionCategoryTags.";
			}
			List<ResearchProjectDef> rpDefs = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
			int num;
			for (int i = 0; i < rpDefs.Count; i = num + 1)
			{
				if (rpDefs[i] != this && rpDefs[i].tab == this.tab && rpDefs[i].ResearchViewX == this.ResearchViewX && rpDefs[i].ResearchViewY == this.ResearchViewY)
				{
					yield return string.Concat(new object[]
					{
						"same research view coords and tab as ",
						rpDefs[i],
						": ",
						this.ResearchViewX,
						", ",
						this.ResearchViewY,
						"(",
						this.tab,
						")"
					});
				}
				num = i;
			}
			if (!ModLister.RoyaltyInstalled && this.techprintCount > 0)
			{
				yield return "defines techprintCount, but techprints are a Royalty-specific game system and only work with Royalty installed.";
			}
			yield break;
			yield break;
		}

		
		public override void PostLoad()
		{
			base.PostLoad();
			if (!ModLister.RoyaltyInstalled)
			{
				this.techprintCount = 0;
			}
		}

		
		public float CostFactor(TechLevel researcherTechLevel)
		{
			TechLevel techLevel = (TechLevel)Mathf.Min((int)this.techLevel, 4);
			if (researcherTechLevel >= techLevel)
			{
				return 1f;
			}
			int num = (int)(techLevel - researcherTechLevel);
			return 1f + (float)num * 0.5f;
		}

		
		public bool HasTag(ResearchProjectTagDef tag)
		{
			return this.tags != null && this.tags.Contains(tag);
		}

		
		public bool CanBeResearchedAt(Building_ResearchBench bench, bool ignoreResearchBenchPowerStatus)
		{
			if (this.requiredResearchBuilding != null && bench.def != this.requiredResearchBuilding)
			{
				return false;
			}
			if (!ignoreResearchBenchPowerStatus)
			{
				CompPowerTrader comp = bench.GetComp<CompPowerTrader>();
				if (comp != null && !comp.PowerOn)
				{
					return false;
				}
			}
			//if (!this.requiredResearchFacilities.NullOrEmpty<ThingDef>())
			//{
			//	ResearchProjectDef.c__DisplayClass63_0 c__DisplayClass63_ = new ResearchProjectDef.c__DisplayClass63_0();
			//	c__DisplayClass63_.4__this = this;
			//	c__DisplayClass63_.affectedByFacilities = bench.TryGetComp<CompAffectedByFacilities>();
			//	if (c__DisplayClass63_.affectedByFacilities == null)
			//	{
			//		return false;
			//	}
			//	List<Thing> linkedFacilitiesListForReading = c__DisplayClass63_.affectedByFacilities.LinkedFacilitiesListForReading;
			//	int j;
			//	int i;
			//	for (i = 0; i < this.requiredResearchFacilities.Count; i = j + 1)
			//	{
			//		if (linkedFacilitiesListForReading.Find((Thing x) => x.def == c__DisplayClass63_.4__this.requiredResearchFacilities[i] && c__DisplayClass63_.affectedByFacilities.IsFacilityActive(x)) == null)
			//		{
			//			return false;
			//		}
			//		j = i;
			//	}
			//}
			return true;
		}


		public void ReapplyAllMods()
		{
			if (this.researchMods != null)
			{
				for (int i = 0; i < this.researchMods.Count; i++)
				{
					try
					{
						this.researchMods[i].Apply();
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Exception applying research mod for project ",
							this,
							": ",
							ex.ToString()
						}), false);
					}
				}
			}
		}

		public static ResearchProjectDef Named(string defName)
		{
			return DefDatabase<ResearchProjectDef>.GetNamed(defName, true);
		}
		public static void GenerateNonOverlappingCoordinates()
		{
			foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefsListForReading)
			{
				researchProjectDef.x = researchProjectDef.researchViewX;
				researchProjectDef.y = researchProjectDef.researchViewY;
			}
			int num = 0;
			do
			{
				bool flag = false;
				foreach (ResearchProjectDef researchProjectDef2 in DefDatabase<ResearchProjectDef>.AllDefsListForReading)
				{
					foreach (ResearchProjectDef researchProjectDef3 in DefDatabase<ResearchProjectDef>.AllDefsListForReading)
					{
						if (researchProjectDef2 != researchProjectDef3 && researchProjectDef2.tab == researchProjectDef3.tab)
						{
							bool flag2 = Mathf.Abs(researchProjectDef2.x - researchProjectDef3.x) < 0.5f;
							bool flag3 = Mathf.Abs(researchProjectDef2.y - researchProjectDef3.y) < 0.25f;
							if (flag2 && flag3)
							{
								flag = true;
								if (researchProjectDef2.x <= researchProjectDef3.x)
								{
									researchProjectDef2.x -= 0.1f;
									researchProjectDef3.x += 0.1f;
								}
								else
								{
									researchProjectDef2.x += 0.1f;
									researchProjectDef3.x -= 0.1f;
								}
								if (researchProjectDef2.y <= researchProjectDef3.y)
								{
									researchProjectDef2.y -= 0.1f;
									researchProjectDef3.y += 0.1f;
								}
								else
								{
									researchProjectDef2.y += 0.1f;
									researchProjectDef3.y -= 0.1f;
								}
								researchProjectDef2.x += 0.001f;
								researchProjectDef2.y += 0.001f;
								researchProjectDef3.x -= 0.001f;
								researchProjectDef3.y -= 0.001f;
								ResearchProjectDef.ClampInCoordinateLimits(researchProjectDef2);
								ResearchProjectDef.ClampInCoordinateLimits(researchProjectDef3);
							}
						}
					}
				}
				if (!flag)
				{
					return;
				}
				num++;
			}
			while (num <= 200);
			Log.Error("Couldn't relax research project coordinates apart after " + 200 + " passes.", false);
		}

		
		private static void ClampInCoordinateLimits(ResearchProjectDef rp)
		{
			if (rp.x < 0f)
			{
				rp.x = 0f;
			}
			if (rp.y < 0f)
			{
				rp.y = 0f;
			}
			if (rp.y > 6.5f)
			{
				rp.y = 6.5f;
			}
		}

		
		public void Debug_ApplyPositionDelta(Vector2 delta)
		{
			bool flag = Mathf.Abs(delta.x) > 0.01f;
			bool flag2 = Mathf.Abs(delta.y) > 0.01f;
			if (flag)
			{
				this.x += delta.x;
			}
			if (flag2)
			{
				this.y += delta.y;
			}
			this.positionModified = true;
		}

		
		public void Debug_SnapPositionData()
		{
			this.x = Mathf.Round(this.x * 1f) / 1f;
			this.y = Mathf.Round(this.y * 20f) / 20f;
			ResearchProjectDef.ClampInCoordinateLimits(this);
		}

		
		public bool Debug_IsPositionModified()
		{
			return this.positionModified;
		}

		
		public float baseCost = 100f;

		
		public List<ResearchProjectDef> prerequisites;

		
		public List<ResearchProjectDef> hiddenPrerequisites;

		
		public TechLevel techLevel;

		
		public List<ResearchProjectDef> requiredByThis;

		
		private List<ResearchMod> researchMods;

		
		public ThingDef requiredResearchBuilding;

		
		public List<ThingDef> requiredResearchFacilities;

		
		public List<ResearchProjectTagDef> tags;

		
		public ResearchTabDef tab;

		
		public float researchViewX = 1f;

		
		public float researchViewY = 1f;

		
		[MustTranslate]
		public string discoveredLetterTitle;

		
		[MustTranslate]
		public string discoveredLetterText;

		
		public int discoveredLetterMinDifficulty;

		
		public bool unlockExtremeDifficulty;

		
		public int techprintCount;

		
		public float techprintCommonality = 1f;

		
		public float techprintMarketValue = 1000f;

		
		public List<string> heldByFactionCategoryTags;

		
		[Unsaved(false)]
		private float x = 1f;

		
		[Unsaved(false)]
		private float y = 1f;

		
		[Unsaved(false)]
		private bool positionModified;

		
		[Unsaved(false)]
		private ThingDef cachedTechprint;

		
		[Unsaved(false)]
		private List<Def> cachedUnlockedDefs;

		
		[Unsaved(false)]
		private List<Dialog_InfoCard.Hyperlink> cachedHyperlinks;

		
		public const TechLevel MaxEffectiveTechLevel = TechLevel.Industrial;

		
		private const float ResearchCostFactorPerTechLevelDiff = 0.5f;
	}
}
