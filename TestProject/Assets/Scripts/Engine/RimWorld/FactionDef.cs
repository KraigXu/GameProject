using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class FactionDef : Def
	{
		
		
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

		
		
		public bool CanEverBeNonHostile
		{
			get
			{
				return !this.permanentEnemy;
			}
		}

		
		
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

		
		
		public bool HasRoyalTitles
		{
			get
			{
				return this.RoyalTitlesAwardableInSeniorityOrderForReading.Count > 0;
			}
		}

		
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

		
		public bool CanUseStuffForApparel(ThingDef stuffDef)
		{
			return this.apparelStuffFilter == null || this.apparelStuffFilter.Allows(stuffDef);
		}

		
		public float RaidCommonalityFromPoints(float points)
		{
			if (points < 0f || this.raidCommonalityFromPointsCurve == null)
			{
				return 1f;
			}
			return this.raidCommonalityFromPointsCurve.Evaluate(points);
		}

		
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.apparelStuffFilter != null)
			{
				this.apparelStuffFilter.ResolveReferences();
			}
		}

		
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

		
		public override IEnumerable<string> ConfigErrors()
		{

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

		
		public static FactionDef Named(string defName)
		{
			return DefDatabase<FactionDef>.GetNamed(defName, true);
		}

		
		public RulePackDef GetNameMaker(Gender gender)
		{
			if (gender == Gender.Female && this.pawnNameMakerFemale != null)
			{
				return this.pawnNameMakerFemale;
			}
			return this.pawnNameMaker;
		}

		
		public bool isPlayer;

		
		public RulePackDef factionNameMaker;

		
		public RulePackDef settlementNameMaker;

		
		public RulePackDef playerInitialSettlementNameMaker;

		
		[MustTranslate]
		public string fixedName;

		
		public bool humanlikeFaction = true;

		
		public bool hidden;

		
		public float listOrderPriority;

		
		public List<PawnGroupMaker> pawnGroupMakers;

		
		public SimpleCurve raidCommonalityFromPointsCurve;

		
		public bool autoFlee = true;

		
		public FloatRange attackersDownPercentageRangeForAutoFlee = new FloatRange(0.4f, 0.7f);

		
		public bool canSiege;

		
		public bool canStageAttacks;

		
		public bool canUseAvoidGrid = true;

		
		public float earliestRaidDays;

		
		public FloatRange allowedArrivalTemperatureRange = new FloatRange(-1000f, 1000f);

		
		public PawnKindDef basicMemberKind;

		
		public List<ResearchProjectTagDef> startingResearchTags;

		
		public List<ResearchProjectTagDef> startingTechprintsResearchTags;

		
		[NoTranslate]
		public List<string> recipePrerequisiteTags;

		
		public bool rescueesCanJoin;

		
		[MustTranslate]
		public string pawnSingular = "member";

		
		[MustTranslate]
		public string pawnsPlural = "members";

		
		public string leaderTitle = "leader";

		
		public string leaderTitleFemale;

		
		[MustTranslate]
		public string royalFavorLabel;

		
		[NoTranslate]
		public string royalFavorIconPath;

		
		public List<PawnKindDef> fixedLeaderKinds;

		
		public bool leaderForceGenerateNewPawn;

		
		public float forageabilityFactor = 1f;

		
		public SimpleCurve maxPawnCostPerTotalPointsCurve;

		
		public List<string> royalTitleTags;

		
		public string categoryTag;

		
		public bool hostileToFactionlessHumanlikes;

		
		public int requiredCountAtGameStart;

		
		public int maxCountAtGameStart = 9999;

		
		public bool canMakeRandomly;

		
		public float settlementGenerationWeight;

		
		public RulePackDef pawnNameMaker;

		
		public RulePackDef pawnNameMakerFemale;

		
		public TechLevel techLevel;

		
		[NoTranslate]
		public List<BackstoryCategoryFilter> backstoryFilters;

		
		[NoTranslate]
		private List<string> backstoryCategories;

		
		[NoTranslate]
		public List<string> hairTags = new List<string>();

		
		public ThingFilter apparelStuffFilter;

		
		public List<TraderKindDef> caravanTraderKinds = new List<TraderKindDef>();

		
		public List<TraderKindDef> visitorTraderKinds = new List<TraderKindDef>();

		
		public List<TraderKindDef> baseTraderKinds = new List<TraderKindDef>();

		
		public float geneticVariance = 1f;

		
		public IntRange startingGoodwill = IntRange.zero;

		
		public bool mustStartOneEnemy;

		
		public IntRange naturalColonyGoodwill = IntRange.zero;

		
		public float goodwillDailyGain;

		
		public float goodwillDailyFall;

		
		public bool permanentEnemy;

		
		public bool permanentEnemyToEveryoneExceptPlayer;

		
		[NoTranslate]
		public string settlementTexturePath;

		
		[NoTranslate]
		public string factionIconPath;

		
		public List<Color> colorSpectrum;

		
		public List<PawnRelationDef> royalTitleInheritanceRelations;

		
		public Type royalTitleInheritanceWorkerClass;

		
		public List<RoyalImplantRule> royalImplantRules;

		
		[Obsolete("Will be removed in the future")]
		public RoyalTitleDef minTitleForBladelinkWeapons;

		
		public string renounceTitleMessage;

		
		[Unsaved(false)]
		private Texture2D factionIcon;

		
		[Unsaved(false)]
		private Texture2D settlementTexture;

		
		[Unsaved(false)]
		private Texture2D royalFavorIcon;

		
		[Unsaved(false)]
		private List<RoyalTitleDef> royalTitlesAwardableInSeniorityOrderForReading;

		
		[Unsaved(false)]
		private List<RoyalTitleDef> royalTitlesAllInSeniorityOrderForReading;

		
		[Unsaved(false)]
		private RoyalTitleInheritanceWorker royalTitleInheritanceWorker;
	}
}
