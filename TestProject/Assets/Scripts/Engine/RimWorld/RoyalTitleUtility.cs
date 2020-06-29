using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public static class RoyalTitleUtility
	{
		
		public static void FindLostAndGainedPermits(RoyalTitleDef currentTitle, RoyalTitleDef newTitle, out List<RoyalTitlePermitDef> gainedPermits, out List<RoyalTitlePermitDef> lostPermits)
		{
			gainedPermits = new List<RoyalTitlePermitDef>();
			lostPermits = new List<RoyalTitlePermitDef>();
			if (newTitle != null && newTitle.permits != null)
			{
				foreach (RoyalTitlePermitDef item in newTitle.permits)
				{
					if (currentTitle == null || currentTitle.permits == null || !currentTitle.permits.Contains(item))
					{
						gainedPermits.Add(item);
					}
				}
			}
			if (currentTitle != null && currentTitle.permits != null)
			{
				foreach (RoyalTitlePermitDef item2 in currentTitle.permits)
				{
					if (newTitle == null || newTitle.permits == null || !newTitle.permits.Contains(item2))
					{
						lostPermits.Add(item2);
					}
				}
			}
		}

		
		public static string BuildDifferenceExplanationText(RoyalTitleDef currentTitle, RoyalTitleDef newTitle, Faction faction, Pawn pawn)
		{
			//RoyalTitleUtility.c__DisplayClass1_0 c__DisplayClass1_ = new RoyalTitleUtility.c__DisplayClass1_0();
			//c__DisplayClass1_.faction = faction;
			//c__DisplayClass1_.pawn = pawn;
			//c__DisplayClass1_.newTitle = newTitle;
			//StringBuilder stringBuilder = new StringBuilder();
			//bool flag = RoyalTitleUtility.ShouldBecomeConceitedOnNewTitle(c__DisplayClass1_.pawn);
			//List<WorkTags> list = c__DisplayClass1_.pawn.story.DisabledWorkTagsBackstoryAndTraits.GetAllSelectedItems<WorkTags>().ToList<WorkTags>();
			//List<WorkTags> list2 = (c__DisplayClass1_.newTitle == null) ? new List<WorkTags>() : c__DisplayClass1_.newTitle.disabledWorkTags.GetAllSelectedItems<WorkTags>().ToList<WorkTags>();
			//List<WorkTags> list3 = new List<WorkTags>();
			//foreach (WorkTags item in list2)
			//{
			//	if (!list.Contains(item))
			//	{
			//		list3.Add(item);
			//	}
			//}
			//int num = (c__DisplayClass1_.newTitle != null) ? c__DisplayClass1_.faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(c__DisplayClass1_.newTitle) : -1;
			//if (c__DisplayClass1_.newTitle != null && flag)
			//{
			//	stringBuilder.AppendLine("LetterRoyalTitleConceitedTrait".Translate(c__DisplayClass1_.pawn.Named("PAWN"), (from t in RoyalTitleUtility.GetConceitedTraits(c__DisplayClass1_.pawn)
			//	select t.CurrentData.label).ToCommaList(true)));
			//	stringBuilder.AppendLine();
			//	if (c__DisplayClass1_.newTitle.minExpectation != null)
			//	{
			//		stringBuilder.AppendLine("LetterRoyalTitleExpectation".Translate(c__DisplayClass1_.pawn.Named("PAWN"), c__DisplayClass1_.newTitle.minExpectation.label).CapitalizeFirst());
			//		stringBuilder.AppendLine();
			//	}
			//}
			//if (c__DisplayClass1_.newTitle != null)
			//{
			//	if (c__DisplayClass1_.newTitle.canBeInherited)
			//	{
			//		Pawn heir = c__DisplayClass1_.pawn.royalty.GetHeir(c__DisplayClass1_.faction);
			//		TaggedString taggedString = (heir != null) ? "LetterRoyalTitleHeir".Translate(c__DisplayClass1_.pawn.Named("PAWN"), heir.Named("HEIR")) : "LetterRoyalTitleNoHeir".Translate(c__DisplayClass1_.pawn.Named("PAWN"));
			//		stringBuilder.Append(taggedString);
			//		if (heir != null && heir.Faction != Faction.OfPlayer)
			//		{
			//			stringBuilder.Append(" " + "LetterRoyalTitleHeirFactionWarning".Translate(heir.Named("PAWN"), c__DisplayClass1_.faction.Named("FACTION")));
			//		}
			//		stringBuilder.AppendLine(" " + "LetterRoyalTitleChangingHeir".Translate(c__DisplayClass1_.faction.Named("FACTION")));
			//	}
			//	else
			//	{
			//		stringBuilder.Append("LetterRoyalTitleCantBeInherited".Translate(c__DisplayClass1_.newTitle.Named("TITLE")).CapitalizeFirst());
			//		stringBuilder.Append(" " + "LetterRoyalTitleNoHeir".Translate(c__DisplayClass1_.pawn.Named("PAWN")));
			//		stringBuilder.AppendLine();
			//	}
			//	stringBuilder.AppendLine();
			//}
			//if (flag && list3.Count > 0)
			//{
			//	stringBuilder.AppendLine("LetterRoyalTitleDisabledWorkTag".Translate(c__DisplayClass1_.pawn.Named("PAWN"), (from t in list3
			//	orderby c__DisplayClass1_.<BuildDifferenceExplanationText>g__FirstTitleDisablingWorkTags|1(t).seniority
			//	select string.Format("{0} ({1})", t.LabelTranslated(), c__DisplayClass1_.<BuildDifferenceExplanationText>g__FirstTitleDisablingWorkTags|1(t).GetLabelFor(c__DisplayClass1_.pawn))).ToLineList("- ", false)).CapitalizeFirst());
			//	stringBuilder.AppendLine();
			//}
			//if (c__DisplayClass1_.newTitle != null)
			//{
			//	if (c__DisplayClass1_.newTitle.requiredMinimumApparelQuality > QualityCategory.Awful)
			//	{
			//		stringBuilder.AppendLine("LetterRoyalTitleApparelQualityRequirement".Translate(c__DisplayClass1_.pawn.Named("PAWN"), c__DisplayClass1_.newTitle.requiredMinimumApparelQuality.GetLabel().ToLower()).CapitalizeFirst());
			//		stringBuilder.AppendLine();
			//	}
			//	if (c__DisplayClass1_.newTitle.requiredApparel != null && c__DisplayClass1_.newTitle.requiredApparel.Count > 0)
			//	{
			//		stringBuilder.AppendLine("LetterRoyalTitleApparelRequirement".Translate(c__DisplayClass1_.pawn.Named("PAWN")).CapitalizeFirst());
			//		foreach (RoyalTitleDef.ApparelRequirement apparelRequirement in c__DisplayClass1_.newTitle.requiredApparel)
			//		{
			//			int i = 0;
			//			stringBuilder.Append("- ");
			//			stringBuilder.AppendLine(string.Join(", ", apparelRequirement.AllRequiredApparelForPawn(c__DisplayClass1_.pawn, false, true).Select(delegate(ThingDef a)
			//			{
			//				int i;
			//				string result = (i == 0) ? a.LabelCap.Resolve() : a.label;
			//				i = i;
			//				i++;
			//				return result;
			//			}).ToArray<string>()));
			//		}
			//		stringBuilder.AppendLine("- " + "ApparelRequirementAnyPrestigeArmor".Translate());
			//		stringBuilder.AppendLine("- " + "ApparelRequirementAnyPsycasterApparel".Translate());
			//		stringBuilder.AppendLine();
			//	}
			//	if (!c__DisplayClass1_.newTitle.throneRoomRequirements.NullOrEmpty<RoomRequirement>())
			//	{
			//		stringBuilder.AppendLine("LetterRoyalTitleThroneroomRequirements".Translate(c__DisplayClass1_.pawn.Named("PAWN"), "\n" + (from r in c__DisplayClass1_.newTitle.throneRoomRequirements
			//		select r.LabelCap(null)).ToLineList("- ", false)).CapitalizeFirst());
			//		stringBuilder.AppendLine();
			//	}
			//	if (!c__DisplayClass1_.newTitle.GetBedroomRequirements(c__DisplayClass1_.pawn).EnumerableNullOrEmpty<RoomRequirement>())
			//	{
			//		stringBuilder.AppendLine("LetterRoyalTitleBedroomRequirements".Translate(c__DisplayClass1_.pawn.Named("PAWN"), "\n" + (from r in c__DisplayClass1_.newTitle.GetBedroomRequirements(c__DisplayClass1_.pawn)
			//		select r.LabelCap(null)).ToLineList("- ", false)).CapitalizeFirst());
			//		stringBuilder.AppendLine();
			//	}
			//	if (flag && c__DisplayClass1_.newTitle.foodRequirement.Defined && c__DisplayClass1_.newTitle.SatisfyingMeals(true).Any<ThingDef>() && (c__DisplayClass1_.pawn.story == null || !c__DisplayClass1_.pawn.story.traits.HasTrait(TraitDefOf.Ascetic)))
			//	{
			//		stringBuilder.AppendLine("LetterRoyalTitleFoodRequirements".Translate(c__DisplayClass1_.pawn.Named("PAWN"), "\n" + (from m in c__DisplayClass1_.newTitle.SatisfyingMeals(false)
			//		select m.LabelCap.Resolve()).ToLineList("- ", false)).CapitalizeFirst());
			//		stringBuilder.AppendLine();
			//	}
			//}
			//List<RoyalTitlePermitDef> list4;
			//List<RoyalTitlePermitDef> list5;
			//RoyalTitleUtility.FindLostAndGainedPermits(currentTitle, c__DisplayClass1_.newTitle, out list4, out list5);
			//if (c__DisplayClass1_.newTitle != null && c__DisplayClass1_.newTitle.permits != null)
			//{
			//	stringBuilder.AppendLine("LetterRoyalTitlePermits".Translate(c__DisplayClass1_.pawn.Named("PAWN")).CapitalizeFirst());
			//	IEnumerable<RoyalTitlePermitDef> permits = c__DisplayClass1_.newTitle.permits;
			//	Func<RoyalTitlePermitDef, int?> keySelector;
			//	if ((keySelector = c__DisplayClass1_.9__11) == null)
			//	{
			//		keySelector = (c__DisplayClass1_.9__11 = delegate(RoyalTitlePermitDef p)
			//		{
			//			RoyalTitleDef royalTitleDef2 = c__DisplayClass1_.<BuildDifferenceExplanationText>g__FirstTitleWithPermit|7(p);
			//			if (royalTitleDef2 == null)
			//			{
			//				return null;
			//			}
			//			return new int?(royalTitleDef2.seniority);
			//		});
			//	}
			//	foreach (RoyalTitlePermitDef royalTitlePermitDef in permits.OrderBy(keySelector))
			//	{
			//		RoyalTitleDef royalTitleDef = c__DisplayClass1_.<BuildDifferenceExplanationText>g__FirstTitleWithPermit|7(royalTitlePermitDef);
			//		if (royalTitleDef != null)
			//		{
			//			stringBuilder.AppendLine("- " + royalTitlePermitDef.LabelCap + " (" + royalTitleDef.GetLabelFor(c__DisplayClass1_.pawn) + ")");
			//		}
			//	}
			//	stringBuilder.AppendLine();
			//}
			//if (list5.Count > 0)
			//{
			//	stringBuilder.AppendLine("LetterRoyalTitleLostPermits".Translate(c__DisplayClass1_.pawn.Named("PAWN")).CapitalizeFirst());
			//	foreach (RoyalTitlePermitDef royalTitlePermitDef2 in list5)
			//	{
			//		stringBuilder.AppendLine("- " + royalTitlePermitDef2.LabelCap);
			//	}
			//	stringBuilder.AppendLine();
			//}
			//if (c__DisplayClass1_.newTitle != null)
			//{
			//	if (c__DisplayClass1_.newTitle.grantedAbilities.Contains(AbilityDefOf.Speech) && (currentTitle == null || !currentTitle.grantedAbilities.Contains(AbilityDefOf.Speech)))
			//	{
			//		stringBuilder.AppendLine("LetterRoyalTitleSpeechAbilityGained".Translate(c__DisplayClass1_.pawn.Named("PAWN")).CapitalizeFirst());
			//		stringBuilder.AppendLine();
			//	}
			//	List<JoyKindDef> list6 = (from def in DefDatabase<JoyKindDef>.AllDefsListForReading
			//	where def.titleRequiredAny != null && def.titleRequiredAny.Contains(c__DisplayClass1_.newTitle)
			//	select def).ToList<JoyKindDef>();
			//	if (list6.Count > 0)
			//	{
			//		stringBuilder.AppendLine("LetterRoyalTitleEnabledJoyKind".Translate(c__DisplayClass1_.pawn.Named("PAWN")).CapitalizeFirst());
			//		foreach (JoyKindDef joyKindDef in list6)
			//		{
			//			stringBuilder.AppendLine("- " + joyKindDef.LabelCap);
			//		}
			//		stringBuilder.AppendLine();
			//	}
			//	if (flag && !c__DisplayClass1_.newTitle.disabledJoyKinds.NullOrEmpty<JoyKindDef>())
			//	{
			//		stringBuilder.AppendLine("LetterRoyalTitleDisabledJoyKind".Translate(c__DisplayClass1_.pawn.Named("PAWN")).CapitalizeFirst());
			//		foreach (JoyKindDef joyKindDef2 in c__DisplayClass1_.newTitle.disabledJoyKinds)
			//		{
			//			stringBuilder.AppendLine("- " + joyKindDef2.LabelCap);
			//		}
			//		stringBuilder.AppendLine();
			//	}
			//	if (c__DisplayClass1_.faction.def.royalImplantRules != null)
			//	{
			//		List<RoyalImplantRule> list7 = new List<RoyalImplantRule>();
			//		foreach (RoyalImplantRule royalImplantRule in c__DisplayClass1_.faction.def.royalImplantRules)
			//		{
			//			RoyalTitleDef minTitleForImplant = c__DisplayClass1_.faction.GetMinTitleForImplant(royalImplantRule.implantHediff, 0);
			//			int num2 = c__DisplayClass1_.faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(minTitleForImplant);
			//			if (num >= num2)
			//			{
			//				if (royalImplantRule.maxLevel == 0)
			//				{
			//					list7.Add(royalImplantRule);
			//				}
			//				else
			//				{
			//					list7.AddDistinct(c__DisplayClass1_.faction.GetMaxAllowedImplantLevel(royalImplantRule.implantHediff, c__DisplayClass1_.newTitle));
			//				}
			//			}
			//		}
			//		if (list7.Count > 0)
			//		{
			//			stringBuilder.AppendLine("LetterRoyalTitleAllowedImplants".Translate(c__DisplayClass1_.pawn.Named("PAWN"), "\n" + list7.Select(delegate(RoyalImplantRule i)
			//			{
			//				if (i.maxLevel == 0)
			//				{
			//					return string.Format("{0} ({1})", i.implantHediff.LabelCap, c__DisplayClass1_.faction.GetMinTitleForImplant(i.implantHediff, 0).GetLabelFor(c__DisplayClass1_.pawn));
			//				}
			//				return string.Format("{0}({1}x) ({2})", i.implantHediff.LabelCap, i.maxLevel, i.minTitle.GetLabelFor(c__DisplayClass1_.pawn));
			//			}).ToLineList("- ", false)).CapitalizeFirst());
			//			stringBuilder.AppendLine();
			//		}
			//	}
			//	if (currentTitle != null && c__DisplayClass1_.newTitle.seniority < currentTitle.seniority)
			//	{
			//		List<Hediff> list8 = new List<Hediff>();
			//		if (c__DisplayClass1_.pawn.health != null && c__DisplayClass1_.pawn.health.hediffSet != null)
			//		{
			//			foreach (Hediff hediff in c__DisplayClass1_.pawn.health.hediffSet.hediffs)
			//			{
			//				if (hediff.def.HasComp(typeof(HediffComp_RoyalImplant)))
			//				{
			//					RoyalTitleDef minTitleForImplant2 = c__DisplayClass1_.faction.GetMinTitleForImplant(hediff.def, HediffComp_RoyalImplant.GetImplantLevel(hediff));
			//					if (c__DisplayClass1_.faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(minTitleForImplant2) > num)
			//					{
			//						list8.Add(hediff);
			//					}
			//				}
			//			}
			//		}
			//		if (list8.Count > 0)
			//		{
			//			stringBuilder.AppendLine("LetterRoyalTitleImplantsMustBeRemoved".Translate(c__DisplayClass1_.pawn.Named("PAWN"), "\n" + (from i in list8
			//			select i.LabelCap).ToLineList("- ", false)).Resolve());
			//			stringBuilder.AppendLine("LetterRoyalTitleImplantGracePeriod".Translate());
			//			stringBuilder.AppendLine();
			//		}
			//	}
			//	if (c__DisplayClass1_.pawn.royalty.NewHighestTitle(c__DisplayClass1_.faction, c__DisplayClass1_.newTitle) && !c__DisplayClass1_.newTitle.rewards.NullOrEmpty<ThingDefCountClass>())
			//	{
			//		stringBuilder.AppendLine("LetterRoyalTitleRewardGranted".Translate(c__DisplayClass1_.pawn.Named("PAWN"), "\n" + (from r in c__DisplayClass1_.newTitle.rewards
			//		select r.Label).ToLineList("- ", false)).CapitalizeFirst());
			//		stringBuilder.AppendLine();
			//	}
			//}
			//return stringBuilder.ToString().TrimEndNewlines();
			return "";
		}

		
		public static RoyalTitleDef GetCurrentTitleIn(this Pawn p, Faction faction)
		{
			if (p.royalty != null)
			{
				return p.royalty.GetCurrentTitle(faction);
			}
			return null;
		}

		
		public static int GetCurrentTitleSeniorityIn(this Pawn p, Faction faction)
		{
			RoyalTitleDef currentTitleIn = p.GetCurrentTitleIn(faction);
			if (currentTitleIn != null)
			{
				return currentTitleIn.seniority;
			}
			return 0;
		}

		
		public static string GetTitleProgressionInfo(Faction faction, Pawn pawn = null)
		{
			TaggedString t = "RoyalTitleTooltipTitlesEarnable".Translate(faction.Named("FACTION")) + ":";
			int num = 0;
			foreach (RoyalTitleDef royalTitleDef in faction.def.RoyalTitlesAwardableInSeniorityOrderForReading)
			{
				num += royalTitleDef.favorCost;
				t += "\n  - " + ((pawn != null) ? royalTitleDef.GetLabelCapFor(pawn) : royalTitleDef.GetLabelCapForBothGenders()) + ": " + "RoyalTitleTooltipRoyalFavorAmount".Translate(royalTitleDef.favorCost, faction.def.royalFavorLabel) + " (" + "RoyalTitleTooltipRoyalFavorTotal".Translate(num.ToString()) + ")";
			}
			t += "\n\n" + "RoyalTitleTooltipTitlesNonEarnable".Translate(faction.Named("FACTION")) + ":";
			foreach (RoyalTitleDef royalTitleDef2 in from tit in faction.def.RoyalTitlesAllInSeniorityOrderForReading
			where !tit.Awardable
			select tit)
			{
				t += "\n  - " + royalTitleDef2.GetLabelCapForBothGenders();
			}
			return t.Resolve();
		}

		
		public static Building_Throne FindBestUnassignedThrone(Pawn pawn)
		{
			float num = float.PositiveInfinity;
			Building_Throne result = null;
			foreach (Thing thing in pawn.Map.listerThings.ThingsOfDef(ThingDefOf.Throne))
			{
				Building_Throne building_Throne = thing as Building_Throne;
				if (building_Throne != null && building_Throne.CompAssignableToPawn.HasFreeSlot && building_Throne.Spawned && !building_Throne.IsForbidden(pawn) && pawn.CanReserveAndReach(building_Throne, PathEndMode.InteractionCell, pawn.NormalMaxDanger(), 1, -1, null, false) && RoomRoleWorker_ThroneRoom.Validate(building_Throne.GetRoom(RegionType.Set_Passable)) == null)
				{
					PawnPath pawnPath = pawn.Map.pathFinder.FindPath(pawn.Position, building_Throne, pawn, PathEndMode.InteractionCell);
					float num2 = pawnPath.Found ? pawnPath.TotalCost : float.PositiveInfinity;
					pawnPath.ReleaseToPool();
					if (num > num2)
					{
						num = num2;
						result = building_Throne;
					}
				}
			}
			if (num == float.PositiveInfinity)
			{
				return null;
			}
			return result;
		}

		
		public static Building_Throne FindBestUsableThrone(Pawn pawn)
		{
			Building_Throne building_Throne = pawn.ownership.AssignedThrone;
			if (building_Throne != null)
			{
				if (!building_Throne.Spawned || building_Throne.IsForbidden(pawn) || !pawn.CanReserveAndReach(building_Throne, PathEndMode.InteractionCell, pawn.NormalMaxDanger(), 1, -1, null, false))
				{
					return null;
				}
				if (RoomRoleWorker_ThroneRoom.Validate(building_Throne.GetRoom(RegionType.Set_Passable)) != null)
				{
					return null;
				}
			}
			else
			{
				building_Throne = RoyalTitleUtility.FindBestUnassignedThrone(pawn);
				if (building_Throne == null)
				{
					return null;
				}
				pawn.ownership.ClaimThrone(building_Throne);
			}
			return building_Throne;
		}

		
		public static bool BedroomSatisfiesRequirements(Room room, RoyalTitle title)
		{
			List<RoomRequirement>.Enumerator enumerator = title.def.bedroomRequirements.GetEnumerator();
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Met(room, null))
					{
						return false;
					}
				}
			}
			return true;
		}

		
		public static bool IsPawnConceited(Pawn p)
		{
			Pawn_StoryTracker story = p.story;
			TraitSet traitSet = (story != null) ? story.traits : null;
			return (traitSet == null || !traitSet.HasTrait(TraitDefOf.Ascetic)) && (!p.Faction.IsPlayer || p.IsQuestLodger() || (traitSet != null && (traitSet.HasTrait(TraitDefOf.Abrasive) || traitSet.HasTrait(TraitDefOf.Greedy) || traitSet.HasTrait(TraitDefOf.Jealous))));
		}

		
		public static IEnumerable<Trait> GetConceitedTraits(Pawn p)
		{
			Pawn_StoryTracker story = p.story;
			TraitSet traits = (story != null) ? story.traits : null;
			if (traits != null)
			{
				int num;
				for (int i = 0; i < RoyalTitleUtility.ConceitedTraits.Count; i = num + 1)
				{
					Trait trait = traits.GetTrait(RoyalTitleUtility.ConceitedTraits[i]);
					if (trait != null)
					{
						yield return trait;
					}
					num = i;
				}
			}
			yield break;
		}

		
		public static IEnumerable<Trait> GetTraitsAffectingPsylinkNegatively(Pawn p)
		{
			if (p.story == null || p.story.traits == null || p.story.traits.allTraits.NullOrEmpty<Trait>())
			{
				yield break;
			}
			foreach (Trait trait in p.story.traits.allTraits)
			{
				TraitDegreeData traitDegreeData = trait.def.DataAtDegree(trait.Degree);
				if (traitDegreeData.statFactors != null)
				{
					if (traitDegreeData.statFactors.Any((StatModifier f) => f.stat == StatDefOf.PsychicSensitivity && f.value < 1f))
					{
						goto IL_114;
					}
				}
				if (traitDegreeData.statOffsets == null)
				{
					continue;
				}
				if (!traitDegreeData.statOffsets.Any((StatModifier f) => f.stat == StatDefOf.PsychicSensitivity && f.value < 0f))
				{
					continue;
				}
				IL_114:
				yield return trait;
			}
			List<Trait>.Enumerator enumerator = default(List<Trait>.Enumerator);
			yield break;
			yield break;
		}

		
		public static TaggedString GetPsylinkAffectedByTraitsNegativelyWarning(Pawn p)
		{
			if (p.HasPsylink || !RoyalTitleUtility.GetTraitsAffectingPsylinkNegatively(p).Any<Trait>())
			{
				return null;
			}
			return "RoyalWithTraitAffectingPsylinkNegatively".Translate(p.Named("PAWN"), p.Faction.Named("FACTION"), (from t in RoyalTitleUtility.GetTraitsAffectingPsylinkNegatively(p)
			select t.Label).ToCommaList(true));
		}

		
		public static bool ShouldBecomeConceitedOnNewTitle(Pawn p)
		{
			Pawn_StoryTracker story = p.story;
			TraitSet traitSet = (story != null) ? story.traits : null;
			return (traitSet == null || !traitSet.HasTrait(TraitDefOf.Ascetic)) && (p.Faction == null || !p.Faction.IsPlayer || p.IsQuestLodger() || RoyalTitleUtility.GetConceitedTraits(p).Any<Trait>());
		}

		
		public static void ResetStaticData()
		{
			RoyalTitleUtility.ConceitedTraits = new List<TraitDef>
			{
				TraitDefOf.Abrasive,
				TraitDefOf.Greedy,
				TraitDefOf.Jealous
			};
		}

		
		public static void DoTable_IngestibleMaxSatisfiedTitle()
		{
			List<TableDataGetter<ThingDef>> list = new List<TableDataGetter<ThingDef>>();
			list.Add(new TableDataGetter<ThingDef>("name", (ThingDef f) => f.LabelCap));
			list.Add(new TableDataGetter<ThingDef>("max satisfied title", delegate(ThingDef t)
			{
				RoyalTitleDef royalTitleDef = t.ingestible.MaxSatisfiedTitle();
				if (royalTitleDef == null)
				{
					return "-";
				}
				return royalTitleDef.LabelCap;
			}));
			DebugTables.MakeTablesDialog<ThingDef>(from t in DefDatabase<ThingDef>.AllDefsListForReading
			where t.ingestible != null && !t.IsCorpse && t.ingestible.HumanEdible
			select t, list.ToArray());
		}

		
		private static List<TraitDef> ConceitedTraits;
	}
}
