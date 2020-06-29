using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;

namespace Verse
{
	
	public class RecipeDef : Def
	{
		
		// (get) Token: 0x060005E2 RID: 1506 RVA: 0x0001C5DB File Offset: 0x0001A7DB
		public RecipeWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RecipeWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.recipe = this;
				}
				return this.workerInt;
			}
		}

		
		// (get) Token: 0x060005E3 RID: 1507 RVA: 0x0001C60D File Offset: 0x0001A80D
		public RecipeWorkerCounter WorkerCounter
		{
			get
			{
				if (this.workerCounterInt == null)
				{
					this.workerCounterInt = (RecipeWorkerCounter)Activator.CreateInstance(this.workerCounterClass);
					this.workerCounterInt.recipe = this;
				}
				return this.workerCounterInt;
			}
		}

		
		// (get) Token: 0x060005E4 RID: 1508 RVA: 0x0001C63F File Offset: 0x0001A83F
		public IngredientValueGetter IngredientValueGetter
		{
			get
			{
				if (this.ingredientValueGetterInt == null)
				{
					this.ingredientValueGetterInt = (IngredientValueGetter)Activator.CreateInstance(this.ingredientValueGetterClass);
				}
				return this.ingredientValueGetterInt;
			}
		}

		
		// (get) Token: 0x060005E5 RID: 1509 RVA: 0x0001C668 File Offset: 0x0001A868
		public bool AvailableNow
		{
			get
			{
				if (this.researchPrerequisite != null && !this.researchPrerequisite.IsFinished)
				{
					return false;
				}
				if (this.researchPrerequisites != null)
				{
					if (this.researchPrerequisites.Any((ResearchProjectDef r) => !r.IsFinished))
					{
						return false;
					}
				}
				if (this.factionPrerequisiteTags != null)
				{
					if (this.factionPrerequisiteTags.Any((string tag) => Faction.OfPlayer.def.recipePrerequisiteTags == null || !Faction.OfPlayer.def.recipePrerequisiteTags.Contains(tag)))
					{
						return false;
					}
				}
				return true;
			}
		}

		
		// (get) Token: 0x060005E6 RID: 1510 RVA: 0x0001C6FC File Offset: 0x0001A8FC
		public string MinSkillString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = false;
				if (this.skillRequirements != null)
				{
					for (int i = 0; i < this.skillRequirements.Count; i++)
					{
						SkillRequirement skillRequirement = this.skillRequirements[i];
						stringBuilder.AppendLine(string.Concat(new object[]
						{
							"   ",
							skillRequirement.skill.skillLabel.CapitalizeFirst(),
							": ",
							skillRequirement.minLevel
						}));
						flag = true;
					}
				}
				if (!flag)
				{
					stringBuilder.AppendLine("   (" + "NoneLower".Translate() + ")");
				}
				return stringBuilder.ToString();
			}
		}

		
		// (get) Token: 0x060005E7 RID: 1511 RVA: 0x0001C7B4 File Offset: 0x0001A9B4
		public IEnumerable<ThingDef> AllRecipeUsers
		{
			get
			{
				int num;
				if (this.recipeUsers != null)
				{
					for (int i = 0; i < this.recipeUsers.Count; i = num + 1)
					{
						yield return this.recipeUsers[i];
						num = i;
					}
				}
				List<ThingDef> thingDefs = DefDatabase<ThingDef>.AllDefsListForReading;
				for (int i = 0; i < thingDefs.Count; i = num + 1)
				{
					if (thingDefs[i].recipes != null && thingDefs[i].recipes.Contains(this))
					{
						yield return thingDefs[i];
					}
					num = i;
				}
				yield break;
			}
		}

		
		// (get) Token: 0x060005E8 RID: 1512 RVA: 0x0001C7C4 File Offset: 0x0001A9C4
		public bool UsesUnfinishedThing
		{
			get
			{
				return this.unfinishedThingDef != null;
			}
		}

		
		// (get) Token: 0x060005E9 RID: 1513 RVA: 0x0001C7D0 File Offset: 0x0001A9D0
		public bool IsSurgery
		{
			get
			{
				if (this.isSurgeryCached == null)
				{
					this.isSurgeryCached = new bool?(false);
					using (IEnumerator<ThingDef> enumerator = this.AllRecipeUsers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.category == ThingCategory.Pawn)
							{
								this.isSurgeryCached = new bool?(true);
								break;
							}
						}
					}
				}
				return this.isSurgeryCached.Value;
			}
		}

		
		// (get) Token: 0x060005EA RID: 1514 RVA: 0x0001C850 File Offset: 0x0001AA50
		public ThingDef ProducedThingDef
		{
			get
			{
				if (this.specialProducts != null)
				{
					return null;
				}
				if (this.products == null || this.products.Count != 1)
				{
					return null;
				}
				return this.products[0].thingDef;
			}
		}

		
		public bool AvailableOnNow(Thing thing)
		{
			return this.Worker.AvailableOnNow(thing);
		}

		
		public float WorkAmountTotal(ThingDef stuffDef)
		{
			if (this.workAmount >= 0f)
			{
				return this.workAmount;
			}
			return this.products[0].thingDef.GetStatValueAbstract(StatDefOf.WorkToMake, stuffDef);
		}

		
		public IEnumerable<ThingDef> PotentiallyMissingIngredients(Pawn billDoer, Map map)
		{
			int num;
			for (int i = 0; i < this.ingredients.Count; i = num + 1)
			{
				IngredientCount ingredientCount = this.ingredients[i];
				bool flag = false;
				List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.HaulableEver);
				for (int j = 0; j < list.Count; j++)
				{
					Thing thing = list[j];
					if ((billDoer == null || !thing.IsForbidden(billDoer)) && !thing.Position.Fogged(map) && (ingredientCount.IsFixedIngredient || this.fixedIngredientFilter.Allows(thing)) && ingredientCount.filter.Allows(thing))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					if (ingredientCount.IsFixedIngredient)
					{
						yield return ingredientCount.filter.AllowedThingDefs.First<ThingDef>();
					}
					else
					{
						ThingDef thingDef = (from x in ingredientCount.filter.AllowedThingDefs
						orderby x.BaseMarketValue
						select x).FirstOrDefault((ThingDef x) => this.fixedIngredientFilter.Allows(x));
						if (thingDef != null)
						{
							yield return thingDef;
						}
					}
				}
				num = i;
			}
			yield break;
		}

		
		public bool IsIngredient(ThingDef th)
		{
			for (int i = 0; i < this.ingredients.Count; i++)
			{
				if (this.ingredients[i].filter.Allows(th) && (this.ingredients[i].IsFixedIngredient || this.fixedIngredientFilter.Allows(th)))
				{
					return true;
				}
			}
			return false;
		}

		
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.workerClass == null)
			{
				yield return "workerClass is null.";
			}
			yield break;
			yield break;
		}

		
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			DeepProfiler.Start("Stat refs");
			try
			{
				if (this.workTableSpeedStat == null)
				{
					this.workTableSpeedStat = StatDefOf.WorkTableWorkSpeedFactor;
				}
				if (this.workTableEfficiencyStat == null)
				{
					this.workTableEfficiencyStat = StatDefOf.WorkTableEfficiencyFactor;
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("ingredients reference resolve");
			try
			{
				for (int i = 0; i < this.ingredients.Count; i++)
				{
					this.ingredients[i].ResolveReferences();
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("fixedIngredientFilter.ResolveReferences()");
			try
			{
				if (this.fixedIngredientFilter != null)
				{
					this.fixedIngredientFilter.ResolveReferences();
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("defaultIngredientFilter setup");
			try
			{
				if (this.defaultIngredientFilter == null)
				{
					this.defaultIngredientFilter = new ThingFilter();
					if (this.fixedIngredientFilter != null)
					{
						this.defaultIngredientFilter.CopyAllowancesFrom(this.fixedIngredientFilter);
					}
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("defaultIngredientFilter.ResolveReferences()");
			try
			{
				this.defaultIngredientFilter.ResolveReferences();
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		
		public bool CompatibleWithHediff(HediffDef hediffDef)
		{
			if (this.incompatibleWithHediffTags.NullOrEmpty<string>() || hediffDef.tags.NullOrEmpty<string>())
			{
				return true;
			}
			for (int i = 0; i < this.incompatibleWithHediffTags.Count; i++)
			{
				for (int j = 0; j < hediffDef.tags.Count; j++)
				{
					if (this.incompatibleWithHediffTags[i].Equals(hediffDef.tags[j], StringComparison.InvariantCultureIgnoreCase))
					{
						return false;
					}
				}
			}
			return true;
		}

		
		public bool PawnSatisfiesSkillRequirements(Pawn pawn)
		{
			return this.FirstSkillRequirementPawnDoesntSatisfy(pawn) == null;
		}

		
		public SkillRequirement FirstSkillRequirementPawnDoesntSatisfy(Pawn pawn)
		{
			if (this.skillRequirements == null)
			{
				return null;
			}
			for (int i = 0; i < this.skillRequirements.Count; i++)
			{
				if (!this.skillRequirements[i].PawnSatisfies(pawn))
				{
					return this.skillRequirements[i];
				}
			}
			return null;
		}

		
		public List<ThingDef> GetPremultipliedSmallIngredients()
		{
			if (this.premultipliedSmallIngredients != null)
			{
				return this.premultipliedSmallIngredients;
			}
			this.premultipliedSmallIngredients = (from td in this.ingredients.SelectMany((IngredientCount ingredient) => ingredient.filter.AllowedThingDefs)
			where td.smallVolume
			select td).Distinct<ThingDef>().ToList<ThingDef>();
			bool flag = true;
			while (flag)
			{
				flag = false;
				for (int i = 0; i < this.ingredients.Count; i++)
				{
					if (this.ingredients[i].filter.AllowedThingDefs.Any((ThingDef td) => !this.premultipliedSmallIngredients.Contains(td)))
					{
						foreach (ThingDef item in this.ingredients[i].filter.AllowedThingDefs)
						{
							flag |= this.premultipliedSmallIngredients.Remove(item);
						}
					}
				}
			}
			return this.premultipliedSmallIngredients;
		}

		
		private IEnumerable<Dialog_InfoCard.Hyperlink> GetIngredientsHyperlinks()
		{
			return Dialog_InfoCard.DefsToHyperlinks(from i in this.ingredients
			where i.IsFixedIngredient
			select i.FixedIngredient into i
			where i != null
			select i);
		}

		
		private IEnumerable<Dialog_InfoCard.Hyperlink> GetProductsHyperlinks()
		{
			return Dialog_InfoCard.DefsToHyperlinks(from i in this.products
			select i.thingDef);
		}

		
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			if (this.workSkill != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Skill".Translate(), this.workSkill.LabelCap, "Stat_Recipe_Skill_Desc".Translate(), 4404, null, null, false);
			}
			if (this.ingredients != null && this.ingredients.Count > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Ingredients".Translate(), (from ic in this.ingredients
				select ic.Summary).ToCommaList(false), "Stat_Recipe_Ingredients_Desc".Translate(), 4405, null, this.GetIngredientsHyperlinks(), false);
			}
			if (this.skillRequirements != null && this.skillRequirements.Count > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "SkillRequirements".Translate(), (from sr in this.skillRequirements
				select sr.Summary).ToCommaList(false), "Stat_Recipe_SkillRequirements_Desc".Translate(), 4403, null, null, false);
			}
			if (this.products != null && this.products.Count > 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Products".Translate(), (from pr in this.products
				select pr.Summary).ToCommaList(false), "Stat_Recipe_Products_Desc".Translate(), 4405, null, this.GetProductsHyperlinks(), false);
			}
			if (this.workSpeedStat != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "WorkSpeedStat".Translate(), this.workSpeedStat.LabelCap, "Stat_Recipe_WorkSpeedStat_Desc".Translate(), 4402, null, null, false);
			}
			if (this.efficiencyStat != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "EfficiencyStat".Translate(), this.efficiencyStat.LabelCap, "Stat_Recipe_EfficiencyStat_Desc".Translate(), 4401, null, null, false);
			}
			if (this.IsSurgery)
			{
				if (this.surgerySuccessChanceFactor >= 99999f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgerySuccessChanceFactor".Translate(), "Stat_Thing_Surgery_SuccessChanceFactor_CantFail".Translate(), "Stat_Thing_Surgery_SuccessChanceFactor_CantFail_Desc".Translate(), 4102, null, null, false);
				}
				else
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgerySuccessChanceFactor".Translate(), this.surgerySuccessChanceFactor.ToStringPercent(), "Stat_Thing_Surgery_SuccessChanceFactor_Desc".Translate(), 4102, null, null, false);
					if (this.deathOnFailedSurgeryChance >= 99999f)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgeryDeathOnFailChance".Translate(), "100%", "Stat_Thing_Surgery_DeathOnFailChance_Desc".Translate(), 4101, null, null, false);
					}
					else
					{
						yield return new StatDrawEntry(StatCategoryDefOf.Surgery, "SurgeryDeathOnFailChance".Translate(), this.deathOnFailedSurgeryChance.ToStringPercent(), "Stat_Thing_Surgery_DeathOnFailChance_Desc".Translate(), 4101, null, null, false);
					}
				}
			}
			yield break;
		}

		
		public Type workerClass = typeof(RecipeWorker);

		
		public Type workerCounterClass = typeof(RecipeWorkerCounter);

		
		[MustTranslate]
		public string jobString = "Doing an unknown recipe.";

		
		public WorkTypeDef requiredGiverWorkType;

		
		public float workAmount = -1f;

		
		public StatDef workSpeedStat;

		
		public StatDef efficiencyStat;

		
		public StatDef workTableEfficiencyStat;

		
		public StatDef workTableSpeedStat;

		
		public List<IngredientCount> ingredients = new List<IngredientCount>();

		
		public ThingFilter fixedIngredientFilter = new ThingFilter();

		
		public ThingFilter defaultIngredientFilter;

		
		public bool allowMixingIngredients;

		
		public bool ignoreIngredientCountTakeEntireStacks;

		
		private Type ingredientValueGetterClass = typeof(IngredientValueGetter_Volume);

		
		public List<SpecialThingFilterDef> forceHiddenSpecialFilters;

		
		public bool autoStripCorpses = true;

		
		public bool interruptIfIngredientIsRotting;

		
		public List<ThingDefCountClass> products = new List<ThingDefCountClass>();

		
		public List<SpecialProductType> specialProducts;

		
		public bool productHasIngredientStuff;

		
		public int targetCountAdjustment = 1;

		
		public ThingDef unfinishedThingDef;

		
		public List<SkillRequirement> skillRequirements;

		
		public SkillDef workSkill;

		
		public float workSkillLearnFactor = 1f;

		
		public EffecterDef effectWorking;

		
		public SoundDef soundWorking;

		
		public List<ThingDef> recipeUsers;

		
		public List<BodyPartDef> appliedOnFixedBodyParts = new List<BodyPartDef>();

		
		public List<BodyPartGroupDef> appliedOnFixedBodyPartGroups = new List<BodyPartGroupDef>();

		
		public HediffDef addsHediff;

		
		public HediffDef removesHediff;

		
		public HediffDef changesHediffLevel;

		
		public List<string> incompatibleWithHediffTags;

		
		public int hediffLevelOffset;

		
		public bool hideBodyPartNames;

		
		public bool isViolation;

		
		[MustTranslate]
		public string successfullyRemovedHediffMessage;

		
		public float surgerySuccessChanceFactor = 1f;

		
		public float deathOnFailedSurgeryChance;

		
		public bool targetsBodyPart = true;

		
		public bool anesthetize = true;

		
		public ResearchProjectDef researchPrerequisite;

		
		public List<ResearchProjectDef> researchPrerequisites;

		
		[NoTranslate]
		public List<string> factionPrerequisiteTags;

		
		public ConceptDef conceptLearned;

		
		public bool dontShowIfAnyIngredientMissing;

		
		[Unsaved(false)]
		private RecipeWorker workerInt;

		
		[Unsaved(false)]
		private RecipeWorkerCounter workerCounterInt;

		
		[Unsaved(false)]
		private IngredientValueGetter ingredientValueGetterInt;

		
		[Unsaved(false)]
		private List<ThingDef> premultipliedSmallIngredients;

		
		private bool? isSurgeryCached;
	}
}
