using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x020000D8 RID: 216
	public class RecipeDef : Def
	{
		// Token: 0x1700010F RID: 271
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

		// Token: 0x17000110 RID: 272
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

		// Token: 0x17000111 RID: 273
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

		// Token: 0x17000112 RID: 274
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

		// Token: 0x17000113 RID: 275
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

		// Token: 0x17000114 RID: 276
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

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060005E8 RID: 1512 RVA: 0x0001C7C4 File Offset: 0x0001A9C4
		public bool UsesUnfinishedThing
		{
			get
			{
				return this.unfinishedThingDef != null;
			}
		}

		// Token: 0x17000116 RID: 278
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

		// Token: 0x17000117 RID: 279
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

		// Token: 0x060005EB RID: 1515 RVA: 0x0001C885 File Offset: 0x0001AA85
		public bool AvailableOnNow(Thing thing)
		{
			return this.Worker.AvailableOnNow(thing);
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x0001C893 File Offset: 0x0001AA93
		public float WorkAmountTotal(ThingDef stuffDef)
		{
			if (this.workAmount >= 0f)
			{
				return this.workAmount;
			}
			return this.products[0].thingDef.GetStatValueAbstract(StatDefOf.WorkToMake, stuffDef);
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x0001C8C5 File Offset: 0x0001AAC5
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

		// Token: 0x060005EE RID: 1518 RVA: 0x0001C8E4 File Offset: 0x0001AAE4
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

		// Token: 0x060005EF RID: 1519 RVA: 0x0001C944 File Offset: 0x0001AB44
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
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

		// Token: 0x060005F0 RID: 1520 RVA: 0x0001C954 File Offset: 0x0001AB54
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

		// Token: 0x060005F1 RID: 1521 RVA: 0x0001CA98 File Offset: 0x0001AC98
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

		// Token: 0x060005F2 RID: 1522 RVA: 0x0001CB10 File Offset: 0x0001AD10
		public bool PawnSatisfiesSkillRequirements(Pawn pawn)
		{
			return this.FirstSkillRequirementPawnDoesntSatisfy(pawn) == null;
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x0001CB1C File Offset: 0x0001AD1C
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

		// Token: 0x060005F4 RID: 1524 RVA: 0x0001CB6C File Offset: 0x0001AD6C
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

		// Token: 0x060005F5 RID: 1525 RVA: 0x0001CC98 File Offset: 0x0001AE98
		private IEnumerable<Dialog_InfoCard.Hyperlink> GetIngredientsHyperlinks()
		{
			return Dialog_InfoCard.DefsToHyperlinks(from i in this.ingredients
			where i.IsFixedIngredient
			select i.FixedIngredient into i
			where i != null
			select i);
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x0001CD1C File Offset: 0x0001AF1C
		private IEnumerable<Dialog_InfoCard.Hyperlink> GetProductsHyperlinks()
		{
			return Dialog_InfoCard.DefsToHyperlinks(from i in this.products
			select i.thingDef);
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0001CD4D File Offset: 0x0001AF4D
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

		// Token: 0x040004F3 RID: 1267
		public Type workerClass = typeof(RecipeWorker);

		// Token: 0x040004F4 RID: 1268
		public Type workerCounterClass = typeof(RecipeWorkerCounter);

		// Token: 0x040004F5 RID: 1269
		[MustTranslate]
		public string jobString = "Doing an unknown recipe.";

		// Token: 0x040004F6 RID: 1270
		public WorkTypeDef requiredGiverWorkType;

		// Token: 0x040004F7 RID: 1271
		public float workAmount = -1f;

		// Token: 0x040004F8 RID: 1272
		public StatDef workSpeedStat;

		// Token: 0x040004F9 RID: 1273
		public StatDef efficiencyStat;

		// Token: 0x040004FA RID: 1274
		public StatDef workTableEfficiencyStat;

		// Token: 0x040004FB RID: 1275
		public StatDef workTableSpeedStat;

		// Token: 0x040004FC RID: 1276
		public List<IngredientCount> ingredients = new List<IngredientCount>();

		// Token: 0x040004FD RID: 1277
		public ThingFilter fixedIngredientFilter = new ThingFilter();

		// Token: 0x040004FE RID: 1278
		public ThingFilter defaultIngredientFilter;

		// Token: 0x040004FF RID: 1279
		public bool allowMixingIngredients;

		// Token: 0x04000500 RID: 1280
		public bool ignoreIngredientCountTakeEntireStacks;

		// Token: 0x04000501 RID: 1281
		private Type ingredientValueGetterClass = typeof(IngredientValueGetter_Volume);

		// Token: 0x04000502 RID: 1282
		public List<SpecialThingFilterDef> forceHiddenSpecialFilters;

		// Token: 0x04000503 RID: 1283
		public bool autoStripCorpses = true;

		// Token: 0x04000504 RID: 1284
		public bool interruptIfIngredientIsRotting;

		// Token: 0x04000505 RID: 1285
		public List<ThingDefCountClass> products = new List<ThingDefCountClass>();

		// Token: 0x04000506 RID: 1286
		public List<SpecialProductType> specialProducts;

		// Token: 0x04000507 RID: 1287
		public bool productHasIngredientStuff;

		// Token: 0x04000508 RID: 1288
		public int targetCountAdjustment = 1;

		// Token: 0x04000509 RID: 1289
		public ThingDef unfinishedThingDef;

		// Token: 0x0400050A RID: 1290
		public List<SkillRequirement> skillRequirements;

		// Token: 0x0400050B RID: 1291
		public SkillDef workSkill;

		// Token: 0x0400050C RID: 1292
		public float workSkillLearnFactor = 1f;

		// Token: 0x0400050D RID: 1293
		public EffecterDef effectWorking;

		// Token: 0x0400050E RID: 1294
		public SoundDef soundWorking;

		// Token: 0x0400050F RID: 1295
		public List<ThingDef> recipeUsers;

		// Token: 0x04000510 RID: 1296
		public List<BodyPartDef> appliedOnFixedBodyParts = new List<BodyPartDef>();

		// Token: 0x04000511 RID: 1297
		public List<BodyPartGroupDef> appliedOnFixedBodyPartGroups = new List<BodyPartGroupDef>();

		// Token: 0x04000512 RID: 1298
		public HediffDef addsHediff;

		// Token: 0x04000513 RID: 1299
		public HediffDef removesHediff;

		// Token: 0x04000514 RID: 1300
		public HediffDef changesHediffLevel;

		// Token: 0x04000515 RID: 1301
		public List<string> incompatibleWithHediffTags;

		// Token: 0x04000516 RID: 1302
		public int hediffLevelOffset;

		// Token: 0x04000517 RID: 1303
		public bool hideBodyPartNames;

		// Token: 0x04000518 RID: 1304
		public bool isViolation;

		// Token: 0x04000519 RID: 1305
		[MustTranslate]
		public string successfullyRemovedHediffMessage;

		// Token: 0x0400051A RID: 1306
		public float surgerySuccessChanceFactor = 1f;

		// Token: 0x0400051B RID: 1307
		public float deathOnFailedSurgeryChance;

		// Token: 0x0400051C RID: 1308
		public bool targetsBodyPart = true;

		// Token: 0x0400051D RID: 1309
		public bool anesthetize = true;

		// Token: 0x0400051E RID: 1310
		public ResearchProjectDef researchPrerequisite;

		// Token: 0x0400051F RID: 1311
		public List<ResearchProjectDef> researchPrerequisites;

		// Token: 0x04000520 RID: 1312
		[NoTranslate]
		public List<string> factionPrerequisiteTags;

		// Token: 0x04000521 RID: 1313
		public ConceptDef conceptLearned;

		// Token: 0x04000522 RID: 1314
		public bool dontShowIfAnyIngredientMissing;

		// Token: 0x04000523 RID: 1315
		[Unsaved(false)]
		private RecipeWorker workerInt;

		// Token: 0x04000524 RID: 1316
		[Unsaved(false)]
		private RecipeWorkerCounter workerCounterInt;

		// Token: 0x04000525 RID: 1317
		[Unsaved(false)]
		private IngredientValueGetter ingredientValueGetterInt;

		// Token: 0x04000526 RID: 1318
		[Unsaved(false)]
		private List<ThingDef> premultipliedSmallIngredients;

		// Token: 0x04000527 RID: 1319
		private bool? isSurgeryCached;
	}
}
