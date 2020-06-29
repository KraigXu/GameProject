using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class RoyalTitleDef : Def
	{
		
		
		public bool Awardable
		{
			get
			{
				return this.favorCost > 0;
			}
		}

		
		
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

		
		public RoyalTitleInheritanceWorker GetInheritanceWorker(Faction faction)
		{
			if (this.inheritanceWorkerOverrideClass == null)
			{
				return faction.def.RoyalTitleInheritanceWorker;
			}
			return this.InheritanceWorkerOverride;
		}

		
		
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

		
		public string GetLabelFor(Pawn p)
		{
			if (p == null)
			{
				return this.GetLabelForBothGenders();
			}
			return this.GetLabelFor(p.gender);
		}

		
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

		
		public string GetLabelForBothGenders()
		{
			if (!string.IsNullOrEmpty(this.labelFemale))
			{
				return this.label + " / " + this.labelFemale;
			}
			return this.label;
		}

		
		public string GetLabelCapForBothGenders()
		{
			if (!string.IsNullOrEmpty(this.labelFemale))
			{
				return base.LabelCap + " / " + this.labelFemale.CapitalizeFirst();
			}
			return base.LabelCap;
		}

		
		public string GetLabelCapFor(Pawn p)
		{
			return this.GetLabelFor(p).CapitalizeFirst(this);
		}

		
		public IEnumerable<RoomRequirement> GetBedroomRequirements(Pawn p)
		{
			if (p.story.traits.HasTrait(TraitDefOf.Ascetic))
			{
				return null;
			}
			return this.bedroomRequirements;
		}

		
		public string GetReportText(Faction faction)
		{
			return this.description + "\n\n" + RoyalTitleUtility.GetTitleProgressionInfo(faction, null);
		}

		
		public bool JoyKindDisabled(JoyKindDef joyKind)
		{
			return this.disabledJoyKinds != null && this.disabledJoyKinds.Contains(joyKind);
		}

		
		private bool HasSameRoomRequirement(RoomRequirement otherReq, List<RoomRequirement> list)
		{
			if (list == null)
			{
				return false;
			}
			List<RoomRequirement>.Enumerator enumerator = list.GetEnumerator();
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

		
		public bool HasSameThroneroomRequirement(RoomRequirement otherReq)
		{
			return this.HasSameRoomRequirement(otherReq, this.throneRoomRequirements);
		}

		
		public bool HasSameBedroomRequirement(RoomRequirement otherReq)
		{
			return this.HasSameRoomRequirement(otherReq, this.bedroomRequirements);
		}

		
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

		
		private IEnumerable<string> RequiredApparelListForGender(Gender g)
		{
			//IEnumerable<RoyalTitleDef.ApparelRequirement> source = this.requiredApparel;
			
			//Func<RoyalTitleDef.ApparelRequirement, IEnumerable<ThingDef>> selector;
			//if ((selector ) == null)
			//{
			//	selector = ( ((RoyalTitleDef.ApparelRequirement r) => r.AllRequiredApparel(g)));
			//}
			//foreach (TaggedString taggedString in from a in source.SelectMany(selector)
			//select a.LabelCap)
			//{
			//	yield return taggedString;
			//}
			//IEnumerator<TaggedString> enumerator = null;
			//yield return "ApparelRequirementAnyPrestigeArmor".Translate();
			//yield return "ApparelRequirementAnyPsycasterApparel".Translate();
			//yield break;
			yield break;
		}

		
		public IEnumerable<DefHyperlink> GetHyperlinks(Faction faction)
		{
			IEnumerable<DefHyperlink> descriptionHyperlinks = this.descriptionHyperlinks;
			return descriptionHyperlinks ?? (from t in faction.def.RoyalTitlesAllInSeniorityOrderForReading
			where t != this
			select new DefHyperlink(t, faction));
		}

		
		public override IEnumerable<string> ConfigErrors()
		{

			//{
			//	
			//}
			//IEnumerator<string> enumerator = null;
			//if (!ModLister.RoyaltyInstalled)
			//{
			//	Log.ErrorOnce("Royal titles are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 1222185, false);
			//}
			//if (this.awardThought != null && !typeof(Thought_MemoryRoyalTitle).IsAssignableFrom(this.awardThought.thoughtClass))
			//{
			//	yield return string.Format("Royal title {0} has awardThought with thoughtClass {1} which is not deriving from Thought_MemoryRoyalTitle!", this.defName, this.awardThought.thoughtClass.FullName);
			//}
			//if (this.lostThought != null && !typeof(Thought_MemoryRoyalTitle).IsAssignableFrom(this.lostThought.thoughtClass))
			//{
			//	yield return string.Format("Royal title {0} has awardThought with thoughtClass {1} which is not deriving from Thought_MemoryRoyalTitle!", this.defName, this.awardThought.thoughtClass.FullName);
			//}
			//if (this.disabledJoyKinds != null)
			//{
			//	foreach (JoyKindDef joyKindDef in this.disabledJoyKinds)
			//	{
			//		if (joyKindDef.titleRequiredAny != null && joyKindDef.titleRequiredAny.Contains(this))
			//		{
			//			yield return string.Format("Royal title {0} disables joy kind {1} which requires the title!", this.defName, joyKindDef.defName);
			//		}
			//	}
			//	List<JoyKindDef>.Enumerator enumerator2 = default(List<JoyKindDef>.Enumerator);
			//}
			//if (this.Awardable && this.changeHeirQuestPoints < 0)
			//{
			//	yield return "undefined changeHeirQuestPoints, it's required for awardable titles";
			//}
			//if (!this.throneRoomRequirements.NullOrEmpty<RoomRequirement>())
			//{
			//	foreach (RoomRequirement req in this.throneRoomRequirements)
			//	{
			//		foreach (string arg in req.ConfigErrors())
			//		{
			//			yield return string.Format("Room requirement {0}: {1}", req.GetType().Name, arg);
			//		}
			//		enumerator = null;
			//		req = null;
			//	}
			//	List<RoomRequirement>.Enumerator enumerator3 = default(List<RoomRequirement>.Enumerator);
			//}
			//yield break;
			yield break;
		}

		
		public int seniority;

		
		public int favorCost;

		
		[MustTranslate]
		public string labelFemale;

		
		public int changeHeirQuestPoints = -1;

		
		public float commonality = 1f;

		
		public WorkTags disabledWorkTags;

		
		public Type inheritanceWorkerOverrideClass;

		
		public QualityCategory requiredMinimumApparelQuality;

		
		public List<RoyalTitleDef.ApparelRequirement> requiredApparel;

		
		public List<RoyalTitlePermitDef> permits;

		
		public ExpectationDef minExpectation;

		
		public List<JoyKindDef> disabledJoyKinds;

		
		[NoTranslate]
		public List<string> tags;

		
		public List<ThingDefCountClass> rewards;

		
		public bool suppressIdleAlert;

		
		public bool canBeInherited;

		
		public bool allowDignifiedMeditationFocus = true;

		
		public ThoughtDef awardThought;

		
		public ThoughtDef lostThought;

		
		public List<RoomRequirement> throneRoomRequirements;

		
		public List<RoomRequirement> bedroomRequirements;

		
		public float recruitmentDifficultyOffset;

		
		public float recruitmentResistanceFactor = 1f;

		
		public float recruitmentResistanceOffset;

		
		public RoyalTitleFoodRequirement foodRequirement;

		
		public RoyalTitleDef replaceOnRecruited;

		
		public float decreeMtbDays = -1f;

		
		public float decreeMinIntervalDays = 2f;

		
		public float decreeMentalBreakCommonality;

		
		public List<string> decreeTags;

		
		public List<AbilityDef> grantedAbilities = new List<AbilityDef>();

		
		public IntRange speechCooldown;

		
		public int maxPsylinkLevel;

		
		[Unsaved(false)]
		private List<ThingDef> satisfyingMealsCached;

		
		[Unsaved(false)]
		private List<ThingDef> satisfyingMealsNoDrugsCached;

		
		private RoyalTitleInheritanceWorker inheritanceWorkerOverride;

		
		public class ApparelRequirement
		{
			
			public IEnumerable<ThingDef> AllAllowedApparelForPawn(Pawn p, bool ignoreGender = false, bool includeWorn = false)
			{
				//List<ThingDef>.Enumerator enumerator = DefDatabase<ThingDef>.AllDefsListForReading.GetEnumerator();
				//{
				//	while (enumerator.MoveNext())
				//	{
				//		ThingDef apparel = enumerator.Current;
				//		if (apparel.IsApparel && apparel.apparel.tags != null && (ignoreGender || apparel.apparel.CorrectGenderForWearing(p.gender)) && apparel.apparel.tags.Any((string t) => this.requiredTags.Contains(t) || this.allowedTags.Contains(t)) && apparel.apparel.bodyPartGroups.Any((BodyPartGroupDef b) => this.bodyPartGroupsMatchAny.Contains(b)) && (includeWorn || !p.apparel.WornApparel.Any((Apparel w) => w.def == apparel)))
				//		{
				//			yield return apparel;
				//		}
				//	}
				//}
				//List<ThingDef>.Enumerator enumerator = default(List<ThingDef>.Enumerator);
				//yield break;
				yield break;
			}

			
			public IEnumerable<ThingDef> AllRequiredApparelForPawn(Pawn p, bool ignoreGender = false, bool includeWorn = false)
			{
				//List<ThingDef>.Enumerator enumerator = DefDatabase<ThingDef>.AllDefsListForReading.GetEnumerator();
				//{
				//	while (enumerator.MoveNext())
				//	{
				//		ThingDef apparel = enumerator.Current;
				//		if (apparel.IsApparel && apparel.apparel.tags != null && (ignoreGender || apparel.apparel.CorrectGenderForWearing(p.gender)) && apparel.apparel.tags.Any((string t) => this.requiredTags.Contains(t)) && apparel.apparel.bodyPartGroups.Any((BodyPartGroupDef b) => this.bodyPartGroupsMatchAny.Contains(b)) && (includeWorn || !p.apparel.WornApparel.Any((Apparel w) => w.def == apparel)))
				//		{
				//			yield return apparel;
				//		}
				//	}
				//}
				//List<ThingDef>.Enumerator enumerator = default(List<ThingDef>.Enumerator);
				//yield break;
				yield break;
			}

			
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

			
			public ThingDef RandomRequiredApparelForPawnInGeneration(Pawn p, Func<ThingDef, bool> validator)
			{
				ThingDef result = null;

				if (!DefDatabase<ThingDef>.AllDefsListForReading.Where(delegate(ThingDef a)
				{
					if (a.IsApparel && a.apparel.tags != null)
					{
						List<BodyPartGroupDef> bodyPartGroups = a.apparel.bodyPartGroups;
						Predicate<BodyPartGroupDef> predicate = ((BodyPartGroupDef b) => this.bodyPartGroupsMatchAny.Contains(b));

						if (bodyPartGroups.Any(predicate))
						{
							List<string> tags = a.apparel.tags;
							Predicate<string> predicate2 = ((string t) => this.requiredTags.Contains(t));

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

			
			public List<BodyPartGroupDef> bodyPartGroupsMatchAny;

			
			public List<string> requiredTags;

			
			public List<string> allowedTags;
		}
	}
}
