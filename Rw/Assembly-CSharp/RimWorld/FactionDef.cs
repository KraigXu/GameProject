using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008C0 RID: 2240
	public class FactionDef : Def
	{
		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x060035ED RID: 13805 RVA: 0x00124EC4 File Offset: 0x001230C4
		public List<RoyalTitleDef> RoyalTitlesAwardableInSeniorityOrderForReading
		{
			get
			{
				if (this.royalTitlesAwardableInSeniorityOrderForReading == null)
				{
					this.royalTitlesAwardableInSeniorityOrderForReading = new List<RoyalTitleDef>();
					if (this.royalTitleTags != null && ModLister.RoyaltyInstalled)
					{
						foreach (RoyalTitleDef royalTitleDef in DefDatabase<RoyalTitleDef>.AllDefsListForReading)
						{
							if (royalTitleDef.Awardable && royalTitleDef.tags.SharesElementWith(this.royalTitleTags))
							{
								this.royalTitlesAwardableInSeniorityOrderForReading.Add(royalTitleDef);
							}
						}
						this.royalTitlesAwardableInSeniorityOrderForReading.SortBy((RoyalTitleDef t) => t.seniority);
					}
				}
				return this.royalTitlesAwardableInSeniorityOrderForReading;
			}
		}

		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x060035EE RID: 13806 RVA: 0x00124F90 File Offset: 0x00123190
		public List<RoyalTitleDef> RoyalTitlesAllInSeniorityOrderForReading
		{
			get
			{
				if (this.royalTitlesAllInSeniorityOrderForReading == null)
				{
					this.royalTitlesAllInSeniorityOrderForReading = new List<RoyalTitleDef>();
					if (this.royalTitleTags != null && ModLister.RoyaltyInstalled)
					{
						foreach (RoyalTitleDef royalTitleDef in DefDatabase<RoyalTitleDef>.AllDefsListForReading)
						{
							if (royalTitleDef.tags.SharesElementWith(this.royalTitleTags))
							{
								this.royalTitlesAllInSeniorityOrderForReading.Add(royalTitleDef);
							}
						}
						this.royalTitlesAllInSeniorityOrderForReading.SortBy((RoyalTitleDef t) => t.seniority);
					}
				}
				return this.royalTitlesAllInSeniorityOrderForReading;
			}
		}

		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x060035EF RID: 13807 RVA: 0x00125050 File Offset: 0x00123250
		public RoyalTitleInheritanceWorker RoyalTitleInheritanceWorker
		{
			get
			{
				if (this.royalTitleInheritanceWorker == null && this.royalTitleInheritanceWorkerClass != null)
				{
					this.royalTitleInheritanceWorker = (RoyalTitleInheritanceWorker)Activator.CreateInstance(this.royalTitleInheritanceWorkerClass);
				}
				return this.royalTitleInheritanceWorker;
			}
		}

		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x060035F0 RID: 13808 RVA: 0x00125084 File Offset: 0x00123284
		public bool CanEverBeNonHostile
		{
			get
			{
				return !this.permanentEnemy;
			}
		}

		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x060035F1 RID: 13809 RVA: 0x00125090 File Offset: 0x00123290
		public Texture2D SettlementTexture
		{
			get
			{
				if (this.settlementTexture == null)
				{
					if (!this.settlementTexturePath.NullOrEmpty())
					{
						this.settlementTexture = ContentFinder<Texture2D>.Get(this.settlementTexturePath, true);
					}
					else
					{
						this.settlementTexture = BaseContent.BadTex;
					}
				}
				return this.settlementTexture;
			}
		}

		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x060035F2 RID: 13810 RVA: 0x001250E0 File Offset: 0x001232E0
		public Texture2D FactionIcon
		{
			get
			{
				if (this.factionIcon == null)
				{
					if (!this.factionIconPath.NullOrEmpty())
					{
						this.factionIcon = ContentFinder<Texture2D>.Get(this.factionIconPath, true);
					}
					else
					{
						this.factionIcon = BaseContent.BadTex;
					}
				}
				return this.factionIcon;
			}
		}

		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x060035F3 RID: 13811 RVA: 0x0012512D File Offset: 0x0012332D
		public Texture2D RoyalFavorIcon
		{
			get
			{
				if (this.royalFavorIcon == null && !this.royalFavorIconPath.NullOrEmpty())
				{
					this.royalFavorIcon = ContentFinder<Texture2D>.Get(this.royalFavorIconPath, true);
				}
				return this.royalFavorIcon;
			}
		}

		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x060035F4 RID: 13812 RVA: 0x00125162 File Offset: 0x00123362
		public bool HasRoyalTitles
		{
			get
			{
				return this.RoyalTitlesAwardableInSeniorityOrderForReading.Count > 0;
			}
		}

		// Token: 0x060035F5 RID: 13813 RVA: 0x00125174 File Offset: 0x00123374
		public float MinPointsToGeneratePawnGroup(PawnGroupKindDef groupKind)
		{
			if (this.pawnGroupMakers == null)
			{
				return 0f;
			}
			IEnumerable<PawnGroupMaker> source = from x in this.pawnGroupMakers
			where x.kindDef == groupKind
			select x;
			if (!source.Any<PawnGroupMaker>())
			{
				return 0f;
			}
			return source.Min((PawnGroupMaker pgm) => pgm.MinPointsToGenerateAnything);
		}

		// Token: 0x060035F6 RID: 13814 RVA: 0x001251E7 File Offset: 0x001233E7
		public bool CanUseStuffForApparel(ThingDef stuffDef)
		{
			return this.apparelStuffFilter == null || this.apparelStuffFilter.Allows(stuffDef);
		}

		// Token: 0x060035F7 RID: 13815 RVA: 0x001251FF File Offset: 0x001233FF
		public float RaidCommonalityFromPoints(float points)
		{
			if (points < 0f || this.raidCommonalityFromPointsCurve == null)
			{
				return 1f;
			}
			return this.raidCommonalityFromPointsCurve.Evaluate(points);
		}

		// Token: 0x060035F8 RID: 13816 RVA: 0x00125223 File Offset: 0x00123423
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.apparelStuffFilter != null)
			{
				this.apparelStuffFilter.ResolveReferences();
			}
		}

		// Token: 0x060035F9 RID: 13817 RVA: 0x00125240 File Offset: 0x00123440
		public override void PostLoad()
		{
			if (this.backstoryCategories != null && this.backstoryCategories.Count > 0)
			{
				if (this.backstoryFilters == null)
				{
					this.backstoryFilters = new List<BackstoryCategoryFilter>();
				}
				this.backstoryFilters.Add(new BackstoryCategoryFilter
				{
					categories = this.backstoryCategories
				});
			}
		}

		// Token: 0x060035FA RID: 13818 RVA: 0x00125292 File Offset: 0x00123492
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.pawnGroupMakers != null && this.maxPawnCostPerTotalPointsCurve == null)
			{
				yield return "has pawnGroupMakers but missing maxPawnCostPerTotalPointsCurve";
			}
			if (!this.isPlayer && this.factionNameMaker == null && this.fixedName == null)
			{
				yield return "FactionTypeDef " + this.defName + " lacks a factionNameMaker and a fixedName.";
			}
			if (this.techLevel == TechLevel.Undefined)
			{
				yield return this.defName + " has no tech level.";
			}
			if (this.humanlikeFaction)
			{
				if (this.backstoryFilters.NullOrEmpty<BackstoryCategoryFilter>())
				{
					yield return this.defName + " is humanlikeFaction but has no backstory categories.";
				}
				if (this.hairTags.Count == 0)
				{
					yield return this.defName + " is humanlikeFaction but has no hairTags.";
				}
			}
			if (this.isPlayer)
			{
				if (this.settlementNameMaker == null)
				{
					yield return "isPlayer is true but settlementNameMaker is null";
				}
				if (this.factionNameMaker == null)
				{
					yield return "isPlayer is true but factionNameMaker is null";
				}
				if (this.playerInitialSettlementNameMaker == null)
				{
					yield return "isPlayer is true but playerInitialSettlementNameMaker is null";
				}
			}
			if (this.permanentEnemy)
			{
				if (this.mustStartOneEnemy)
				{
					yield return "permanentEnemy has mustStartOneEnemy = true, which is redundant";
				}
				if (this.goodwillDailyFall != 0f || this.goodwillDailyGain != 0f)
				{
					yield return "permanentEnemy has a goodwillDailyFall or goodwillDailyGain";
				}
				if (this.startingGoodwill != IntRange.zero)
				{
					yield return "permanentEnemy has a startingGoodwill defined";
				}
				if (this.naturalColonyGoodwill != IntRange.zero)
				{
					yield return "permanentEnemy has a naturalColonyGoodwill defined";
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x060035FB RID: 13819 RVA: 0x001252A2 File Offset: 0x001234A2
		public static FactionDef Named(string defName)
		{
			return DefDatabase<FactionDef>.GetNamed(defName, true);
		}

		// Token: 0x060035FC RID: 13820 RVA: 0x001252AB File Offset: 0x001234AB
		public RulePackDef GetNameMaker(Gender gender)
		{
			if (gender == Gender.Female && this.pawnNameMakerFemale != null)
			{
				return this.pawnNameMakerFemale;
			}
			return this.pawnNameMaker;
		}

		// Token: 0x04001DDA RID: 7642
		public bool isPlayer;

		// Token: 0x04001DDB RID: 7643
		public RulePackDef factionNameMaker;

		// Token: 0x04001DDC RID: 7644
		public RulePackDef settlementNameMaker;

		// Token: 0x04001DDD RID: 7645
		public RulePackDef playerInitialSettlementNameMaker;

		// Token: 0x04001DDE RID: 7646
		[MustTranslate]
		public string fixedName;

		// Token: 0x04001DDF RID: 7647
		public bool humanlikeFaction = true;

		// Token: 0x04001DE0 RID: 7648
		public bool hidden;

		// Token: 0x04001DE1 RID: 7649
		public float listOrderPriority;

		// Token: 0x04001DE2 RID: 7650
		public List<PawnGroupMaker> pawnGroupMakers;

		// Token: 0x04001DE3 RID: 7651
		public SimpleCurve raidCommonalityFromPointsCurve;

		// Token: 0x04001DE4 RID: 7652
		public bool autoFlee = true;

		// Token: 0x04001DE5 RID: 7653
		public FloatRange attackersDownPercentageRangeForAutoFlee = new FloatRange(0.4f, 0.7f);

		// Token: 0x04001DE6 RID: 7654
		public bool canSiege;

		// Token: 0x04001DE7 RID: 7655
		public bool canStageAttacks;

		// Token: 0x04001DE8 RID: 7656
		public bool canUseAvoidGrid = true;

		// Token: 0x04001DE9 RID: 7657
		public float earliestRaidDays;

		// Token: 0x04001DEA RID: 7658
		public FloatRange allowedArrivalTemperatureRange = new FloatRange(-1000f, 1000f);

		// Token: 0x04001DEB RID: 7659
		public PawnKindDef basicMemberKind;

		// Token: 0x04001DEC RID: 7660
		public List<ResearchProjectTagDef> startingResearchTags;

		// Token: 0x04001DED RID: 7661
		public List<ResearchProjectTagDef> startingTechprintsResearchTags;

		// Token: 0x04001DEE RID: 7662
		[NoTranslate]
		public List<string> recipePrerequisiteTags;

		// Token: 0x04001DEF RID: 7663
		public bool rescueesCanJoin;

		// Token: 0x04001DF0 RID: 7664
		[MustTranslate]
		public string pawnSingular = "member";

		// Token: 0x04001DF1 RID: 7665
		[MustTranslate]
		public string pawnsPlural = "members";

		// Token: 0x04001DF2 RID: 7666
		public string leaderTitle = "leader";

		// Token: 0x04001DF3 RID: 7667
		public string leaderTitleFemale;

		// Token: 0x04001DF4 RID: 7668
		[MustTranslate]
		public string royalFavorLabel;

		// Token: 0x04001DF5 RID: 7669
		[NoTranslate]
		public string royalFavorIconPath;

		// Token: 0x04001DF6 RID: 7670
		public List<PawnKindDef> fixedLeaderKinds;

		// Token: 0x04001DF7 RID: 7671
		public bool leaderForceGenerateNewPawn;

		// Token: 0x04001DF8 RID: 7672
		public float forageabilityFactor = 1f;

		// Token: 0x04001DF9 RID: 7673
		public SimpleCurve maxPawnCostPerTotalPointsCurve;

		// Token: 0x04001DFA RID: 7674
		public List<string> royalTitleTags;

		// Token: 0x04001DFB RID: 7675
		public string categoryTag;

		// Token: 0x04001DFC RID: 7676
		public bool hostileToFactionlessHumanlikes;

		// Token: 0x04001DFD RID: 7677
		public int requiredCountAtGameStart;

		// Token: 0x04001DFE RID: 7678
		public int maxCountAtGameStart = 9999;

		// Token: 0x04001DFF RID: 7679
		public bool canMakeRandomly;

		// Token: 0x04001E00 RID: 7680
		public float settlementGenerationWeight;

		// Token: 0x04001E01 RID: 7681
		public RulePackDef pawnNameMaker;

		// Token: 0x04001E02 RID: 7682
		public RulePackDef pawnNameMakerFemale;

		// Token: 0x04001E03 RID: 7683
		public TechLevel techLevel;

		// Token: 0x04001E04 RID: 7684
		[NoTranslate]
		public List<BackstoryCategoryFilter> backstoryFilters;

		// Token: 0x04001E05 RID: 7685
		[NoTranslate]
		private List<string> backstoryCategories;

		// Token: 0x04001E06 RID: 7686
		[NoTranslate]
		public List<string> hairTags = new List<string>();

		// Token: 0x04001E07 RID: 7687
		public ThingFilter apparelStuffFilter;

		// Token: 0x04001E08 RID: 7688
		public List<TraderKindDef> caravanTraderKinds = new List<TraderKindDef>();

		// Token: 0x04001E09 RID: 7689
		public List<TraderKindDef> visitorTraderKinds = new List<TraderKindDef>();

		// Token: 0x04001E0A RID: 7690
		public List<TraderKindDef> baseTraderKinds = new List<TraderKindDef>();

		// Token: 0x04001E0B RID: 7691
		public float geneticVariance = 1f;

		// Token: 0x04001E0C RID: 7692
		public IntRange startingGoodwill = IntRange.zero;

		// Token: 0x04001E0D RID: 7693
		public bool mustStartOneEnemy;

		// Token: 0x04001E0E RID: 7694
		public IntRange naturalColonyGoodwill = IntRange.zero;

		// Token: 0x04001E0F RID: 7695
		public float goodwillDailyGain;

		// Token: 0x04001E10 RID: 7696
		public float goodwillDailyFall;

		// Token: 0x04001E11 RID: 7697
		public bool permanentEnemy;

		// Token: 0x04001E12 RID: 7698
		public bool permanentEnemyToEveryoneExceptPlayer;

		// Token: 0x04001E13 RID: 7699
		[NoTranslate]
		public string settlementTexturePath;

		// Token: 0x04001E14 RID: 7700
		[NoTranslate]
		public string factionIconPath;

		// Token: 0x04001E15 RID: 7701
		public List<Color> colorSpectrum;

		// Token: 0x04001E16 RID: 7702
		public List<PawnRelationDef> royalTitleInheritanceRelations;

		// Token: 0x04001E17 RID: 7703
		public Type royalTitleInheritanceWorkerClass;

		// Token: 0x04001E18 RID: 7704
		public List<RoyalImplantRule> royalImplantRules;

		// Token: 0x04001E19 RID: 7705
		[Obsolete("Will be removed in the future")]
		public RoyalTitleDef minTitleForBladelinkWeapons;

		// Token: 0x04001E1A RID: 7706
		public string renounceTitleMessage;

		// Token: 0x04001E1B RID: 7707
		[Unsaved(false)]
		private Texture2D factionIcon;

		// Token: 0x04001E1C RID: 7708
		[Unsaved(false)]
		private Texture2D settlementTexture;

		// Token: 0x04001E1D RID: 7709
		[Unsaved(false)]
		private Texture2D royalFavorIcon;

		// Token: 0x04001E1E RID: 7710
		[Unsaved(false)]
		private List<RoyalTitleDef> royalTitlesAwardableInSeniorityOrderForReading;

		// Token: 0x04001E1F RID: 7711
		[Unsaved(false)]
		private List<RoyalTitleDef> royalTitlesAllInSeniorityOrderForReading;

		// Token: 0x04001E20 RID: 7712
		[Unsaved(false)]
		private RoyalTitleInheritanceWorker royalTitleInheritanceWorker;
	}
}
