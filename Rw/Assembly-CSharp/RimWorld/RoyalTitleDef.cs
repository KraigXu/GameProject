using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200103C RID: 4156
	public class RoyalTitleDef : Def
	{
		// Token: 0x17001144 RID: 4420
		// (get) Token: 0x06006344 RID: 25412 RVA: 0x00227F63 File Offset: 0x00226163
		public bool Awardable
		{
			get
			{
				return this.favorCost > 0;
			}
		}

		// Token: 0x17001145 RID: 4421
		// (get) Token: 0x06006345 RID: 25413 RVA: 0x00227F6E File Offset: 0x0022616E
		public IEnumerable<WorkTypeDef> DisabledWorkTypes
		{
			get
			{
				List<WorkTypeDef> list = DefDatabase<WorkTypeDef>.AllDefsListForReading;
				int num;
				for (int i = 0; i < list.Count; i = num + 1)
				{
					if ((this.disabledWorkTags & list[i].workTags) != WorkTags.None)
					{
						yield return list[i];
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x17001146 RID: 4422
		// (get) Token: 0x06006346 RID: 25414 RVA: 0x00227F7E File Offset: 0x0022617E
		public RoyalTitleInheritanceWorker InheritanceWorkerOverride
		{
			get
			{
				if (this.inheritanceWorkerOverride == null && this.inheritanceWorkerOverrideClass != null)
				{
					this.inheritanceWorkerOverride = (RoyalTitleInheritanceWorker)Activator.CreateInstance(this.inheritanceWorkerOverrideClass);
				}
				return this.inheritanceWorkerOverride;
			}
		}

		// Token: 0x06006347 RID: 25415 RVA: 0x00227FB2 File Offset: 0x002261B2
		public RoyalTitleInheritanceWorker GetInheritanceWorker(Faction faction)
		{
			if (this.inheritanceWorkerOverrideClass == null)
			{
				return faction.def.RoyalTitleInheritanceWorker;
			}
			return this.InheritanceWorkerOverride;
		}

		// Token: 0x17001147 RID: 4423
		// (get) Token: 0x06006348 RID: 25416 RVA: 0x00227FD4 File Offset: 0x002261D4
		public float MinThroneRoomImpressiveness
		{
			get
			{
				if (this.throneRoomRequirements.NullOrEmpty<RoomRequirement>())
				{
					return 0f;
				}
				RoomRequirement_Impressiveness roomRequirement_Impressiveness = this.throneRoomRequirements.OfType<RoomRequirement_Impressiveness>().FirstOrDefault<RoomRequirement_Impressiveness>();
				if (roomRequirement_Impressiveness == null)
				{
					return 0f;
				}
				return (float)roomRequirement_Impressiveness.impressiveness;
			}
		}

		// Token: 0x06006349 RID: 25417 RVA: 0x00228015 File Offset: 0x00226215
		public string GetLabelFor(Pawn p)
		{
			if (p == null)
			{
				return this.GetLabelForBothGenders();
			}
			return this.GetLabelFor(p.gender);
		}

		// Token: 0x0600634A RID: 25418 RVA: 0x0022802D File Offset: 0x0022622D
		public string GetLabelFor(Gender g)
		{
			if (g != Gender.Female)
			{
				return this.label;
			}
			if (string.IsNullOrEmpty(this.labelFemale))
			{
				return this.label;
			}
			return this.labelFemale;
		}

		// Token: 0x0600634B RID: 25419 RVA: 0x00228054 File Offset: 0x00226254
		public string GetLabelForBothGenders()
		{
			if (!string.IsNullOrEmpty(this.labelFemale))
			{
				return this.label + " / " + this.labelFemale;
			}
			return this.label;
		}

		// Token: 0x0600634C RID: 25420 RVA: 0x00228080 File Offset: 0x00226280
		public string GetLabelCapForBothGenders()
		{
			if (!string.IsNullOrEmpty(this.labelFemale))
			{
				return base.LabelCap + " / " + this.labelFemale.CapitalizeFirst();
			}
			return base.LabelCap;
		}

		// Token: 0x0600634D RID: 25421 RVA: 0x002280C0 File Offset: 0x002262C0
		public string GetLabelCapFor(Pawn p)
		{
			return this.GetLabelFor(p).CapitalizeFirst(this);
		}

		// Token: 0x0600634E RID: 25422 RVA: 0x002280CF File Offset: 0x002262CF
		public IEnumerable<RoomRequirement> GetBedroomRequirements(Pawn p)
		{
			if (p.story.traits.HasTrait(TraitDefOf.Ascetic))
			{
				return null;
			}
			return this.bedroomRequirements;
		}

		// Token: 0x0600634F RID: 25423 RVA: 0x002280F0 File Offset: 0x002262F0
		public string GetReportText(Faction faction)
		{
			return this.description + "\n\n" + RoyalTitleUtility.GetTitleProgressionInfo(faction, null);
		}

		// Token: 0x06006350 RID: 25424 RVA: 0x00228109 File Offset: 0x00226309
		public bool JoyKindDisabled(JoyKindDef joyKind)
		{
			return this.disabledJoyKinds != null && this.disabledJoyKinds.Contains(joyKind);
		}

		// Token: 0x06006351 RID: 25425 RVA: 0x00228124 File Offset: 0x00226324
		private bool HasSameRoomRequirement(RoomRequirement otherReq, List<RoomRequirement> list)
		{
			if (list == null)
			{
				return false;
			}
			using (List<RoomRequirement>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.SameOrSubsetOf(otherReq))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006352 RID: 25426 RVA: 0x00228180 File Offset: 0x00226380
		public bool HasSameThroneroomRequirement(RoomRequirement otherReq)
		{
			return this.HasSameRoomRequirement(otherReq, this.throneRoomRequirements);
		}

		// Token: 0x06006353 RID: 25427 RVA: 0x0022818F File Offset: 0x0022638F
		public bool HasSameBedroomRequirement(RoomRequirement otherReq)
		{
			return this.HasSameRoomRequirement(otherReq, this.bedroomRequirements);
		}

		// Token: 0x06006354 RID: 25428 RVA: 0x002281A0 File Offset: 0x002263A0
		public int MaxAllowedPsylinkLevel(FactionDef faction)
		{
			int result = 0;
			for (int i = 0; i < faction.royalImplantRules.Count; i++)
			{
				RoyalImplantRule royalImplantRule = faction.royalImplantRules[i];
				if (royalImplantRule.implantHediff == HediffDefOf.PsychicAmplifier && royalImplantRule.minTitle.Awardable && royalImplantRule.minTitle.seniority <= this.seniority)
				{
					result = royalImplantRule.maxLevel;
				}
			}
			return result;
		}

		// Token: 0x06006355 RID: 25429 RVA: 0x00228208 File Offset: 0x00226408
		public IEnumerable<ThingDef> SatisfyingMeals(bool includeDrugs = true)
		{
			if (includeDrugs)
			{
				if (this.satisfyingMealsCached == null)
				{
					this.satisfyingMealsCached = (from t in DefDatabase<ThingDef>.AllDefsListForReading
					where this.foodRequirement.Acceptable(t)
					orderby t.ingestible.preferability descending
					select t).ToList<ThingDef>();
				}
			}
			else if (this.satisfyingMealsNoDrugsCached == null)
			{
				this.satisfyingMealsNoDrugsCached = (from t in DefDatabase<ThingDef>.AllDefsListForReading
				where this.foodRequirement.Acceptable(t) && !t.IsDrug
				orderby t.ingestible.preferability descending
				select t).ToList<ThingDef>();
			}
			if (!includeDrugs)
			{
				return this.satisfyingMealsNoDrugsCached;
			}
			return this.satisfyingMealsCached;
		}

		// Token: 0x06006356 RID: 25430 RVA: 0x002282C7 File Offset: 0x002264C7
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			if (!this.permits.NullOrEmpty<RoyalTitlePermitDef>())
			{
				TaggedString taggedString = "RoyalTitleTooltipPermits".Translate();
				string valueString = (from r in this.permits
				select r.label).ToCommaList(false).CapitalizeFirst();
				string reportText = (from r in this.permits
				select r.LabelCap.ToString()).ToLineList("  - ", true);
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, taggedString, valueString, reportText, 99999, null, null, false);
			}
			if (this.requiredMinimumApparelQuality > QualityCategory.Awful)
			{
				TaggedString taggedString2 = "RoyalTitleTooltipRequiredApparelQuality".Translate();
				string text = this.requiredMinimumApparelQuality.GetLabel().CapitalizeFirst();
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, taggedString2, text, text, 99998, null, null, false);
			}
			if (!this.requiredApparel.NullOrEmpty<RoyalTitleDef.ApparelRequirement>())
			{
				TaggedString taggedString3 = "RoyalTitleTooltipRequiredApparel".Translate();
				TaggedString t2 = "Male".Translate().CapitalizeFirst() + ":\n" + this.RequiredApparelListForGender(Gender.Male).ToLineList("  - ", false) + "\n\n" + "Female".Translate().CapitalizeFirst() + ":\n" + this.RequiredApparelListForGender(Gender.Female).ToLineList("  - ", false);
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, taggedString3, "", "RoyalTitleRequiredApparelStatDescription".Translate() + ":\n\n" + t2, 99998, null, null, false);
			}
			if (!this.bedroomRequirements.NullOrEmpty<RoomRequirement>())
			{
				TaggedString taggedString4 = "RoyalTitleTooltipBedroomRequirements".Translate();
				string valueString2 = (from r in this.bedroomRequirements
				select r.Label(null)).ToCommaList(false).CapitalizeFirst();
				string reportText2 = (from r in this.bedroomRequirements
				select r.LabelCap(null)).ToLineList("  - ", false);
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, taggedString4, valueString2, reportText2, 99997, null, null, false);
			}
			if (!this.throneRoomRequirements.NullOrEmpty<RoomRequirement>())
			{
				TaggedString taggedString5 = "RoyalTitleTooltipThroneroomRequirements".Translate();
				string valueString3 = (from r in this.throneRoomRequirements
				select r.Label(null)).ToCommaList(false).CapitalizeFirst();
				string reportText3 = (from r in this.throneRoomRequirements
				select r.LabelCap(null)).ToArray<string>().ToLineList("  - ");
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, taggedString5, valueString3, reportText3, 99997, null, null, false);
			}
			IEnumerable<string> enumerable = from t in this.disabledWorkTags.GetAllSelectedItems<WorkTags>()
			where t > WorkTags.None
			select t into w
			select w.LabelTranslated();
			if (enumerable.Any<string>())
			{
				TaggedString taggedString6 = "DisabledWorkTypes".Translate();
				string valueString4 = enumerable.ToCommaList(false).CapitalizeFirst();
				string reportText4 = enumerable.ToLineList(" -  ", true);
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, taggedString6, valueString4, reportText4, 99994, null, null, false);
			}
			if (this.foodRequirement.Defined && this.SatisfyingMeals(true).Any<ThingDef>())
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "RoyalTitleRequiredMeals".Translate(), (from m in this.SatisfyingMeals(true)
				select m.label).ToCommaList(false).CapitalizeFirst(), "RoyalTitleRequiredMealsDesc".Translate(), 99995, null, null, false);
			}
			yield break;
		}

		// Token: 0x06006357 RID: 25431 RVA: 0x002282D7 File Offset: 0x002264D7
		private IEnumerable<string> RequiredApparelListForGender(Gender g)
		{
			IEnumerable<RoyalTitleDef.ApparelRequirement> source = this.requiredApparel;
			Func<RoyalTitleDef.ApparelRequirement, IEnumerable<ThingDef>> <>9__0;
			Func<RoyalTitleDef.ApparelRequirement, IEnumerable<ThingDef>> selector;
			if ((selector = <>9__0) == null)
			{
				selector = (<>9__0 = ((RoyalTitleDef.ApparelRequirement r) => r.AllRequiredApparel(g)));
			}
			foreach (TaggedString taggedString in from a in source.SelectMany(selector)
			select a.LabelCap)
			{
				yield return taggedString;
			}
			IEnumerator<TaggedString> enumerator = null;
			yield return "ApparelRequirementAnyPrestigeArmor".Translate();
			yield return "ApparelRequirementAnyPsycasterApparel".Translate();
			yield break;
			yield break;
		}

		// Token: 0x06006358 RID: 25432 RVA: 0x002282F0 File Offset: 0x002264F0
		public IEnumerable<DefHyperlink> GetHyperlinks(Faction faction)
		{
			IEnumerable<DefHyperlink> descriptionHyperlinks = this.descriptionHyperlinks;
			return descriptionHyperlinks ?? (from t in faction.def.RoyalTitlesAllInSeniorityOrderForReading
			where t != this
			select new DefHyperlink(t, faction));
		}

		// Token: 0x06006359 RID: 25433 RVA: 0x0022834F File Offset: 0x0022654F
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Royal titles are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 1222185, false);
			}
			if (this.awardThought != null && !typeof(Thought_MemoryRoyalTitle).IsAssignableFrom(this.awardThought.thoughtClass))
			{
				yield return string.Format("Royal title {0} has awardThought with thoughtClass {1} which is not deriving from Thought_MemoryRoyalTitle!", this.defName, this.awardThought.thoughtClass.FullName);
			}
			if (this.lostThought != null && !typeof(Thought_MemoryRoyalTitle).IsAssignableFrom(this.lostThought.thoughtClass))
			{
				yield return string.Format("Royal title {0} has awardThought with thoughtClass {1} which is not deriving from Thought_MemoryRoyalTitle!", this.defName, this.awardThought.thoughtClass.FullName);
			}
			if (this.disabledJoyKinds != null)
			{
				foreach (JoyKindDef joyKindDef in this.disabledJoyKinds)
				{
					if (joyKindDef.titleRequiredAny != null && joyKindDef.titleRequiredAny.Contains(this))
					{
						yield return string.Format("Royal title {0} disables joy kind {1} which requires the title!", this.defName, joyKindDef.defName);
					}
				}
				List<JoyKindDef>.Enumerator enumerator2 = default(List<JoyKindDef>.Enumerator);
			}
			if (this.Awardable && this.changeHeirQuestPoints < 0)
			{
				yield return "undefined changeHeirQuestPoints, it's required for awardable titles";
			}
			if (!this.throneRoomRequirements.NullOrEmpty<RoomRequirement>())
			{
				foreach (RoomRequirement req in this.throneRoomRequirements)
				{
					foreach (string arg in req.ConfigErrors())
					{
						yield return string.Format("Room requirement {0}: {1}", req.GetType().Name, arg);
					}
					enumerator = null;
					req = null;
				}
				List<RoomRequirement>.Enumerator enumerator3 = default(List<RoomRequirement>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x04003C68 RID: 15464
		public int seniority;

		// Token: 0x04003C69 RID: 15465
		public int favorCost;

		// Token: 0x04003C6A RID: 15466
		[MustTranslate]
		public string labelFemale;

		// Token: 0x04003C6B RID: 15467
		public int changeHeirQuestPoints = -1;

		// Token: 0x04003C6C RID: 15468
		public float commonality = 1f;

		// Token: 0x04003C6D RID: 15469
		public WorkTags disabledWorkTags;

		// Token: 0x04003C6E RID: 15470
		public Type inheritanceWorkerOverrideClass;

		// Token: 0x04003C6F RID: 15471
		public QualityCategory requiredMinimumApparelQuality;

		// Token: 0x04003C70 RID: 15472
		public List<RoyalTitleDef.ApparelRequirement> requiredApparel;

		// Token: 0x04003C71 RID: 15473
		public List<RoyalTitlePermitDef> permits;

		// Token: 0x04003C72 RID: 15474
		public ExpectationDef minExpectation;

		// Token: 0x04003C73 RID: 15475
		public List<JoyKindDef> disabledJoyKinds;

		// Token: 0x04003C74 RID: 15476
		[NoTranslate]
		public List<string> tags;

		// Token: 0x04003C75 RID: 15477
		public List<ThingDefCountClass> rewards;

		// Token: 0x04003C76 RID: 15478
		public bool suppressIdleAlert;

		// Token: 0x04003C77 RID: 15479
		public bool canBeInherited;

		// Token: 0x04003C78 RID: 15480
		public bool allowDignifiedMeditationFocus = true;

		// Token: 0x04003C79 RID: 15481
		public ThoughtDef awardThought;

		// Token: 0x04003C7A RID: 15482
		public ThoughtDef lostThought;

		// Token: 0x04003C7B RID: 15483
		public List<RoomRequirement> throneRoomRequirements;

		// Token: 0x04003C7C RID: 15484
		public List<RoomRequirement> bedroomRequirements;

		// Token: 0x04003C7D RID: 15485
		public float recruitmentDifficultyOffset;

		// Token: 0x04003C7E RID: 15486
		public float recruitmentResistanceFactor = 1f;

		// Token: 0x04003C7F RID: 15487
		public float recruitmentResistanceOffset;

		// Token: 0x04003C80 RID: 15488
		public RoyalTitleFoodRequirement foodRequirement;

		// Token: 0x04003C81 RID: 15489
		public RoyalTitleDef replaceOnRecruited;

		// Token: 0x04003C82 RID: 15490
		public float decreeMtbDays = -1f;

		// Token: 0x04003C83 RID: 15491
		public float decreeMinIntervalDays = 2f;

		// Token: 0x04003C84 RID: 15492
		public float decreeMentalBreakCommonality;

		// Token: 0x04003C85 RID: 15493
		public List<string> decreeTags;

		// Token: 0x04003C86 RID: 15494
		public List<AbilityDef> grantedAbilities = new List<AbilityDef>();

		// Token: 0x04003C87 RID: 15495
		public IntRange speechCooldown;

		// Token: 0x04003C88 RID: 15496
		public int maxPsylinkLevel;

		// Token: 0x04003C89 RID: 15497
		[Unsaved(false)]
		private List<ThingDef> satisfyingMealsCached;

		// Token: 0x04003C8A RID: 15498
		[Unsaved(false)]
		private List<ThingDef> satisfyingMealsNoDrugsCached;

		// Token: 0x04003C8B RID: 15499
		private RoyalTitleInheritanceWorker inheritanceWorkerOverride;

		// Token: 0x02001EB1 RID: 7857
		public class ApparelRequirement
		{
			// Token: 0x0600AAA0 RID: 43680 RVA: 0x0031CD83 File Offset: 0x0031AF83
			public IEnumerable<ThingDef> AllAllowedApparelForPawn(Pawn p, bool ignoreGender = false, bool includeWorn = false)
			{
				using (List<ThingDef>.Enumerator enumerator = DefDatabase<ThingDef>.AllDefsListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ThingDef apparel = enumerator.Current;
						if (apparel.IsApparel && apparel.apparel.tags != null && (ignoreGender || apparel.apparel.CorrectGenderForWearing(p.gender)) && apparel.apparel.tags.Any((string t) => this.requiredTags.Contains(t) || this.allowedTags.Contains(t)) && apparel.apparel.bodyPartGroups.Any((BodyPartGroupDef b) => this.bodyPartGroupsMatchAny.Contains(b)) && (includeWorn || !p.apparel.WornApparel.Any((Apparel w) => w.def == apparel)))
						{
							yield return apparel;
						}
					}
				}
				List<ThingDef>.Enumerator enumerator = default(List<ThingDef>.Enumerator);
				yield break;
				yield break;
			}

			// Token: 0x0600AAA1 RID: 43681 RVA: 0x0031CDA8 File Offset: 0x0031AFA8
			public IEnumerable<ThingDef> AllRequiredApparelForPawn(Pawn p, bool ignoreGender = false, bool includeWorn = false)
			{
				using (List<ThingDef>.Enumerator enumerator = DefDatabase<ThingDef>.AllDefsListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ThingDef apparel = enumerator.Current;
						if (apparel.IsApparel && apparel.apparel.tags != null && (ignoreGender || apparel.apparel.CorrectGenderForWearing(p.gender)) && apparel.apparel.tags.Any((string t) => this.requiredTags.Contains(t)) && apparel.apparel.bodyPartGroups.Any((BodyPartGroupDef b) => this.bodyPartGroupsMatchAny.Contains(b)) && (includeWorn || !p.apparel.WornApparel.Any((Apparel w) => w.def == apparel)))
						{
							yield return apparel;
						}
					}
				}
				List<ThingDef>.Enumerator enumerator = default(List<ThingDef>.Enumerator);
				yield break;
				yield break;
			}

			// Token: 0x0600AAA2 RID: 43682 RVA: 0x0031CDCD File Offset: 0x0031AFCD
			public IEnumerable<ThingDef> AllRequiredApparel(Gender gender = Gender.None)
			{
				foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
				{
					if (thingDef.IsApparel && thingDef.apparel.tags != null && thingDef.apparel.tags.Any((string t) => this.requiredTags.Contains(t)) && thingDef.apparel.bodyPartGroups.Any((BodyPartGroupDef b) => this.bodyPartGroupsMatchAny.Contains(b)) && (gender == Gender.None || thingDef.apparel.CorrectGenderForWearing(gender)))
					{
						yield return thingDef;
					}
				}
				List<ThingDef>.Enumerator enumerator = default(List<ThingDef>.Enumerator);
				yield break;
				yield break;
			}

			// Token: 0x0600AAA3 RID: 43683 RVA: 0x0031CDE4 File Offset: 0x0031AFE4
			public bool ApparelMeetsRequirement(ThingDef thingDef, bool allowUnmatched = true)
			{
				bool flag = false;
				for (int i = 0; i < this.bodyPartGroupsMatchAny.Count; i++)
				{
					if (thingDef.apparel.bodyPartGroups.Contains(this.bodyPartGroupsMatchAny[i]))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					for (int j = 0; j < this.requiredTags.Count; j++)
					{
						if (thingDef.apparel.tags.Contains(this.requiredTags[j]))
						{
							return true;
						}
					}
					if (this.allowedTags != null)
					{
						for (int k = 0; k < this.allowedTags.Count; k++)
						{
							if (thingDef.apparel.tags.Contains(this.allowedTags[k]))
							{
								return true;
							}
						}
					}
					return false;
				}
				return allowUnmatched;
			}

			// Token: 0x0600AAA4 RID: 43684 RVA: 0x0031CEA8 File Offset: 0x0031B0A8
			public bool IsMet(Pawn p)
			{
				foreach (Apparel apparel in p.apparel.WornApparel)
				{
					bool flag = false;
					for (int i = 0; i < this.bodyPartGroupsMatchAny.Count; i++)
					{
						if (apparel.def.apparel.bodyPartGroups.Contains(this.bodyPartGroupsMatchAny[i]))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						for (int j = 0; j < this.requiredTags.Count; j++)
						{
							if (apparel.def.apparel.tags.Contains(this.requiredTags[j]))
							{
								return true;
							}
						}
						if (this.allowedTags != null)
						{
							for (int k = 0; k < this.allowedTags.Count; k++)
							{
								if (apparel.def.apparel.tags.Contains(this.allowedTags[k]))
								{
									return true;
								}
							}
						}
					}
				}
				return false;
			}

			// Token: 0x0600AAA5 RID: 43685 RVA: 0x0031CFD8 File Offset: 0x0031B1D8
			public ThingDef RandomRequiredApparelForPawnInGeneration(Pawn p, Func<ThingDef, bool> validator)
			{
				ThingDef result = null;
				Predicate<BodyPartGroupDef> <>9__2;
				Predicate<string> <>9__3;
				if (!DefDatabase<ThingDef>.AllDefsListForReading.Where(delegate(ThingDef a)
				{
					if (a.IsApparel && a.apparel.tags != null)
					{
						List<BodyPartGroupDef> bodyPartGroups = a.apparel.bodyPartGroups;
						Predicate<BodyPartGroupDef> predicate;
						if ((predicate = <>9__2) == null)
						{
							predicate = (<>9__2 = ((BodyPartGroupDef b) => this.bodyPartGroupsMatchAny.Contains(b)));
						}
						if (bodyPartGroups.Any(predicate))
						{
							List<string> tags = a.apparel.tags;
							Predicate<string> predicate2;
							if ((predicate2 = <>9__3) == null)
							{
								predicate2 = (<>9__3 = ((string t) => this.requiredTags.Contains(t)));
							}
							if (tags.Any(predicate2) && a.apparel.CorrectGenderForWearing(p.gender))
							{
								return validator == null || validator(a);
							}
						}
					}
					return false;
				}).TryRandomElementByWeight((ThingDef a) => a.generateCommonality, out result))
				{
					return null;
				}
				return result;
			}

			// Token: 0x0600AAA6 RID: 43686 RVA: 0x0031D044 File Offset: 0x0031B244
			public override string ToString()
			{
				if (this.allowedTags == null)
				{
					return string.Format("({0}) -> {1}", string.Join(",", (from a in this.bodyPartGroupsMatchAny
					select a.defName).ToArray<string>()), string.Join(",", this.requiredTags.ToArray()));
				}
				return string.Format("({0}) -> {1}|{2}", string.Join(",", (from a in this.bodyPartGroupsMatchAny
				select a.defName).ToArray<string>()), string.Join(",", this.requiredTags.ToArray()), string.Join(",", this.allowedTags.ToArray()));
			}

			// Token: 0x04007398 RID: 29592
			public List<BodyPartGroupDef> bodyPartGroupsMatchAny;

			// Token: 0x04007399 RID: 29593
			public List<string> requiredTags;

			// Token: 0x0400739A RID: 29594
			public List<string> allowedTags;
		}
	}
}
