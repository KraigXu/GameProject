﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000295 RID: 661
	public static class PawnGenerator
	{
		// Token: 0x06001252 RID: 4690 RVA: 0x000688F8 File Offset: 0x00066AF8
		public static void Reset()
		{
			PawnGenerator.relationsGeneratableBlood = (from rel in DefDatabase<PawnRelationDef>.AllDefsListForReading
			where rel.familyByBloodRelation && rel.generationChanceFactor > 0f
			select rel).ToArray<PawnRelationDef>();
			PawnGenerator.relationsGeneratableNonblood = (from rel in DefDatabase<PawnRelationDef>.AllDefsListForReading
			where !rel.familyByBloodRelation && rel.generationChanceFactor > 0f
			select rel).ToArray<PawnRelationDef>();
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x0006896C File Offset: 0x00066B6C
		public static Pawn GeneratePawn(PawnKindDef kindDef, Faction faction = null)
		{
			return PawnGenerator.GeneratePawn(new PawnGenerationRequest(kindDef, faction, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null));
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x000689DC File Offset: 0x00066BDC
		public static Pawn GeneratePawn(PawnGenerationRequest request)
		{
			Pawn result;
			try
			{
				Pawn pawn = PawnGenerator.GenerateOrRedressPawnInternal(request);
				if (pawn != null && !request.AllowDead && pawn.health.hediffSet.hediffs.Any<Hediff>())
				{
					bool dead = pawn.Dead;
					bool downed = pawn.Downed;
					pawn.health.hediffSet.DirtyCache();
					pawn.health.CheckForStateChange(null, null);
					if (pawn.Dead)
					{
						Log.Error(string.Concat(new object[]
						{
							"Pawn was generated dead but the pawn generation request specified the pawn must be alive. This shouldn't ever happen even if we ran out of tries because null pawn should have been returned instead in this case. Resetting health...\npawn.Dead=",
							pawn.Dead.ToString(),
							" pawn.Downed=",
							pawn.Downed.ToString(),
							" deadBefore=",
							dead.ToString(),
							" downedBefore=",
							downed.ToString(),
							"\nrequest=",
							request
						}), false);
						pawn.health.Reset();
					}
				}
				if (pawn.Faction == Faction.OfPlayerSilentFail && !pawn.IsQuestLodger())
				{
					Find.StoryWatcher.watcherPopAdaptation.Notify_PawnEvent(pawn, PopAdaptationEvent.GainedColonist);
				}
				result = pawn;
			}
			catch (Exception arg)
			{
				Log.Error("Error while generating pawn. Rethrowing. Exception: " + arg, false);
				throw;
			}
			finally
			{
			}
			return result;
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x00068B5C File Offset: 0x00066D5C
		private static Pawn GenerateOrRedressPawnInternal(PawnGenerationRequest request)
		{
			Pawn pawn = null;
			if (!request.Newborn && !request.ForceGenerateNewPawn)
			{
				if (request.ForceRedressWorldPawnIfFormerColonist)
				{
					if ((from x in PawnGenerator.GetValidCandidatesToRedress(request)
					where PawnUtility.EverBeenColonistOrTameAnimal(x)
					select x).TryRandomElementByWeight((Pawn x) => PawnGenerator.WorldPawnSelectionWeight(x), out pawn))
					{
						PawnGenerator.RedressPawn(pawn, request);
						Find.WorldPawns.RemovePawn(pawn);
					}
				}
				if (pawn == null && request.Inhabitant && request.Tile != -1)
				{
					Settlement settlement = Find.WorldObjects.WorldObjectAt<Settlement>(request.Tile);
					if (settlement != null && settlement.previouslyGeneratedInhabitants.Any<Pawn>())
					{
						if ((from x in PawnGenerator.GetValidCandidatesToRedress(request)
						where settlement.previouslyGeneratedInhabitants.Contains(x)
						select x).TryRandomElementByWeight((Pawn x) => PawnGenerator.WorldPawnSelectionWeight(x), out pawn))
						{
							PawnGenerator.RedressPawn(pawn, request);
							Find.WorldPawns.RemovePawn(pawn);
						}
					}
				}
				if (pawn == null && Rand.Chance(PawnGenerator.ChanceToRedressAnyWorldPawn(request)))
				{
					if (PawnGenerator.GetValidCandidatesToRedress(request).TryRandomElementByWeight((Pawn x) => PawnGenerator.WorldPawnSelectionWeight(x), out pawn))
					{
						PawnGenerator.RedressPawn(pawn, request);
						Find.WorldPawns.RemovePawn(pawn);
					}
				}
			}
			bool redressed;
			if (pawn == null)
			{
				redressed = false;
				pawn = PawnGenerator.GenerateNewPawnInternal(ref request);
				if (pawn == null)
				{
					return null;
				}
				if (request.Inhabitant && request.Tile != -1)
				{
					Settlement settlement2 = Find.WorldObjects.WorldObjectAt<Settlement>(request.Tile);
					if (settlement2 != null)
					{
						settlement2.previouslyGeneratedInhabitants.Add(pawn);
					}
				}
			}
			else
			{
				redressed = true;
			}
			if (Find.Scenario != null)
			{
				Find.Scenario.Notify_PawnGenerated(pawn, request.Context, redressed);
			}
			return pawn;
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x00068D4C File Offset: 0x00066F4C
		public static void RedressPawn(Pawn pawn, PawnGenerationRequest request)
		{
			try
			{
				if (pawn.becameWorldPawnTickAbs != -1 && pawn.health != null)
				{
					float x = (GenTicks.TicksAbs - pawn.becameWorldPawnTickAbs).TicksToDays();
					List<Hediff> list = SimplePool<List<Hediff>>.Get();
					list.Clear();
					foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
					{
						if (Rand.Chance(hediff.def.removeOnRedressChanceByDaysCurve.Evaluate(x)))
						{
							list.Add(hediff);
						}
					}
					foreach (Hediff hediff2 in list)
					{
						pawn.health.RemoveHediff(hediff2);
					}
					list.Clear();
					SimplePool<List<Hediff>>.Return(list);
				}
				pawn.ChangeKind(request.KindDef);
				if (pawn.royalty != null)
				{
					pawn.royalty.allowRoomRequirements = pawn.kindDef.allowRoyalRoomRequirements;
					pawn.royalty.allowApparelRequirements = pawn.kindDef.allowRoyalApparelRequirements;
				}
				if (pawn.Faction != request.Faction)
				{
					pawn.SetFaction(request.Faction, null);
				}
				PawnGenerator.GenerateGearFor(pawn, request);
				if (pawn.guest != null)
				{
					pawn.guest.SetGuestStatus(null, false);
				}
				if (pawn.needs != null)
				{
					pawn.needs.SetInitialLevels();
				}
			}
			finally
			{
			}
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x00068F08 File Offset: 0x00067108
		public static bool IsBeingGenerated(Pawn pawn)
		{
			for (int i = 0; i < PawnGenerator.pawnsBeingGenerated.Count; i++)
			{
				if (PawnGenerator.pawnsBeingGenerated[i].Pawn == pawn)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x00068F44 File Offset: 0x00067144
		private static bool IsValidCandidateToRedress(Pawn pawn, PawnGenerationRequest request)
		{
			if (pawn.def != request.KindDef.race)
			{
				return false;
			}
			if (!request.WorldPawnFactionDoesntMatter && pawn.Faction != request.Faction)
			{
				return false;
			}
			if (!request.AllowDead && (pawn.Dead || pawn.Destroyed))
			{
				return false;
			}
			if (!request.AllowDowned && pawn.Downed)
			{
				return false;
			}
			if (pawn.health.hediffSet.BleedRateTotal > 0.001f)
			{
				return false;
			}
			if (!request.CanGeneratePawnRelations && pawn.RaceProps.IsFlesh && pawn.relations.RelatedToAnyoneOrAnyoneRelatedToMe)
			{
				return false;
			}
			if (!request.AllowGay && pawn.RaceProps.Humanlike && pawn.story.traits.HasTrait(TraitDefOf.Gay))
			{
				return false;
			}
			if (!request.AllowAddictions && AddictionUtility.AddictedToAnything(pawn))
			{
				return false;
			}
			if (request.ProhibitedTraits != null && request.ProhibitedTraits.Any((TraitDef t) => pawn.story.traits.HasTrait(t)))
			{
				return false;
			}
			List<SkillRange> skills = request.KindDef.skills;
			if (skills != null)
			{
				for (int i = 0; i < skills.Count; i++)
				{
					SkillRecord skill = pawn.skills.GetSkill(skills[i].Skill);
					if (skill.TotallyDisabled)
					{
						return false;
					}
					if (skill.Level < skills[i].Range.min || skill.Level > skills[i].Range.max)
					{
						return false;
					}
				}
			}
			if (request.ForcedTraits != null)
			{
				foreach (TraitDef tDef in request.ForcedTraits)
				{
					if (!pawn.story.traits.HasTrait(tDef))
					{
						return false;
					}
				}
			}
			if (request.ValidatorPreGear != null && !request.ValidatorPreGear(pawn))
			{
				return false;
			}
			if (request.ValidatorPostGear != null && !request.ValidatorPostGear(pawn))
			{
				return false;
			}
			if (request.FixedBiologicalAge != null)
			{
				float ageBiologicalYearsFloat = pawn.ageTracker.AgeBiologicalYearsFloat;
				float? num = request.FixedBiologicalAge;
				if (!(ageBiologicalYearsFloat == num.GetValueOrDefault() & num != null))
				{
					return false;
				}
			}
			if (request.FixedChronologicalAge != null)
			{
				float num2 = (float)pawn.ageTracker.AgeChronologicalYears;
				float? num = request.FixedChronologicalAge;
				if (!(num2 == num.GetValueOrDefault() & num != null))
				{
					return false;
				}
			}
			if (request.FixedGender != null)
			{
				Gender gender = pawn.gender;
				Gender? fixedGender = request.FixedGender;
				if (!(gender == fixedGender.GetValueOrDefault() & fixedGender != null))
				{
					return false;
				}
			}
			if (request.FixedLastName != null && (!(pawn.Name is NameTriple) || ((NameTriple)pawn.Name).Last != request.FixedLastName))
			{
				return false;
			}
			if (request.FixedMelanin != null && pawn.story != null)
			{
				float melanin = pawn.story.melanin;
				float? num = request.FixedMelanin;
				if (!(melanin == num.GetValueOrDefault() & num != null))
				{
					return false;
				}
			}
			if (request.FixedTitle != null && (pawn.royalty == null || !pawn.royalty.HasTitle(request.FixedTitle)))
			{
				return false;
			}
			if (request.KindDef.minTitleRequired != null)
			{
				if (pawn.royalty == null)
				{
					return false;
				}
				RoyalTitleDef royalTitleDef = pawn.royalty.MainTitle();
				if (royalTitleDef == null || royalTitleDef.seniority < request.KindDef.minTitleRequired.seniority)
				{
					return false;
				}
			}
			if (request.Context == PawnGenerationContext.PlayerStarter && Find.Scenario != null && !Find.Scenario.AllowPlayerStartingPawn(pawn, true, request))
			{
				return false;
			}
			if (request.MustBeCapableOfViolence)
			{
				if (pawn.WorkTagIsDisabled(WorkTags.Violent))
				{
					return false;
				}
				if (pawn.RaceProps.ToolUser && !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
				{
					return false;
				}
			}
			return (request.RedressValidator == null || request.RedressValidator(pawn)) && (request.KindDef.requiredWorkTags == WorkTags.None || pawn.kindDef == request.KindDef || (pawn.CombinedDisabledWorkTags & request.KindDef.requiredWorkTags) == WorkTags.None);
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x00069458 File Offset: 0x00067658
		private static Pawn GenerateNewPawnInternal(ref PawnGenerationRequest request)
		{
			Pawn pawn = null;
			string text = null;
			bool ignoreScenarioRequirements = false;
			bool ignoreValidator = false;
			for (int i = 0; i < 120; i++)
			{
				if (i == 70)
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not generate a pawn after ",
						70,
						" tries. Last error: ",
						text,
						" Ignoring scenario requirements."
					}), false);
					ignoreScenarioRequirements = true;
				}
				if (i == 100)
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not generate a pawn after ",
						100,
						" tries. Last error: ",
						text,
						" Ignoring validator."
					}), false);
					ignoreValidator = true;
				}
				PawnGenerationRequest pawnGenerationRequest = request;
				pawn = PawnGenerator.TryGenerateNewPawnInternal(ref pawnGenerationRequest, out text, ignoreScenarioRequirements, ignoreValidator);
				if (pawn != null)
				{
					request = pawnGenerationRequest;
					break;
				}
			}
			if (pawn == null)
			{
				Log.Error(string.Concat(new object[]
				{
					"Pawn generation error: ",
					text,
					" Too many tries (",
					120,
					"), returning null. Generation request: ",
					request
				}), false);
				return null;
			}
			return pawn;
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x00069570 File Offset: 0x00067770
		private static Pawn TryGenerateNewPawnInternal(ref PawnGenerationRequest request, out string error, bool ignoreScenarioRequirements, bool ignoreValidator)
		{
			error = null;
			Pawn pawn = (Pawn)ThingMaker.MakeThing(request.KindDef.race, null);
			PawnGenerator.pawnsBeingGenerated.Add(new PawnGenerator.PawnGenerationStatus(pawn, null));
			Pawn result;
			try
			{
				pawn.kindDef = request.KindDef;
				pawn.SetFactionDirect(request.Faction);
				PawnComponentsUtility.CreateInitialComponents(pawn);
				if (request.FixedGender != null)
				{
					pawn.gender = request.FixedGender.Value;
				}
				else if (pawn.RaceProps.hasGenders)
				{
					if (Rand.Value < 0.5f)
					{
						pawn.gender = Gender.Male;
					}
					else
					{
						pawn.gender = Gender.Female;
					}
				}
				else
				{
					pawn.gender = Gender.None;
				}
				PawnGenerator.GenerateRandomAge(pawn, request);
				pawn.needs.SetInitialLevels();
				if (!request.Newborn && request.CanGeneratePawnRelations)
				{
					PawnGenerator.GeneratePawnRelations(pawn, ref request);
				}
				if (pawn.RaceProps.Humanlike)
				{
					FactionDef def;
					Faction faction;
					if (request.Faction != null)
					{
						def = request.Faction.def;
					}
					else if (Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction, false, true, TechLevel.Undefined))
					{
						def = faction.def;
					}
					else
					{
						def = Faction.OfAncients.def;
					}
					pawn.story.melanin = ((request.FixedMelanin != null) ? request.FixedMelanin.Value : PawnSkinColors.RandomMelanin(request.Faction));
					pawn.story.crownType = ((Rand.Value < 0.5f) ? CrownType.Average : CrownType.Narrow);
					pawn.story.hairColor = PawnHairColors.RandomHairColor(pawn.story.SkinColor, pawn.ageTracker.AgeBiologicalYears);
					PawnBioAndNameGenerator.GiveAppropriateBioAndNameTo(pawn, request.FixedLastName, def);
					if (pawn.story != null)
					{
						if (request.FixedBirthName != null)
						{
							pawn.story.birthLastName = request.FixedBirthName;
						}
						else if (pawn.Name is NameTriple)
						{
							pawn.story.birthLastName = ((NameTriple)pawn.Name).Last;
						}
					}
					pawn.story.hairDef = PawnHairChooser.RandomHairDefFor(pawn, def);
					PawnGenerator.GenerateTraits(pawn, request);
					PawnGenerator.GenerateBodyType(pawn);
					PawnGenerator.GenerateSkills(pawn);
				}
				if (pawn.RaceProps.Animal && request.Faction != null && request.Faction.IsPlayer)
				{
					pawn.training.SetWantedRecursive(TrainableDefOf.Tameness, true);
					pawn.training.Train(TrainableDefOf.Tameness, null, true);
				}
				PawnGenerator.GenerateInitialHediffs(pawn, request);
				RoyalTitleDef royalTitleDef = request.FixedTitle;
				if (royalTitleDef == null)
				{
					if (request.KindDef.titleRequired != null)
					{
						royalTitleDef = request.KindDef.titleRequired;
					}
					else if (!request.KindDef.titleSelectOne.NullOrEmpty<RoyalTitleDef>() && Rand.Chance(request.KindDef.royalTitleChance))
					{
						royalTitleDef = request.KindDef.titleSelectOne.RandomElementByWeight((RoyalTitleDef t) => t.commonality);
					}
				}
				if (request.KindDef.minTitleRequired != null && (royalTitleDef == null || royalTitleDef.seniority < request.KindDef.minTitleRequired.seniority))
				{
					royalTitleDef = request.KindDef.minTitleRequired;
				}
				if (royalTitleDef != null)
				{
					Faction faction2 = (request.Faction != null && request.Faction.def.HasRoyalTitles) ? request.Faction : Find.FactionManager.RandomRoyalFaction(false, false, true, TechLevel.Undefined);
					pawn.royalty.SetTitle(faction2, royalTitleDef, false, false, true);
					int amount = 0;
					if (royalTitleDef.GetNextTitle(faction2) != null)
					{
						amount = Rand.Range(0, royalTitleDef.GetNextTitle(faction2).favorCost - 1);
					}
					pawn.royalty.SetFavor(faction2, amount);
					if (royalTitleDef.maxPsylinkLevel > 0)
					{
						Hediff_ImplantWithLevel hediff_ImplantWithLevel = HediffMaker.MakeHediff(HediffDefOf.PsychicAmplifier, pawn, pawn.health.hediffSet.GetBrain()) as Hediff_ImplantWithLevel;
						pawn.health.AddHediff(hediff_ImplantWithLevel, null, null, null);
						hediff_ImplantWithLevel.SetLevelTo(royalTitleDef.maxPsylinkLevel);
					}
				}
				if (pawn.royalty != null)
				{
					pawn.royalty.allowRoomRequirements = request.KindDef.allowRoyalRoomRequirements;
					pawn.royalty.allowApparelRequirements = request.KindDef.allowRoyalApparelRequirements;
				}
				if (pawn.workSettings != null && request.Faction != null && request.Faction.IsPlayer)
				{
					pawn.workSettings.EnableAndInitialize();
				}
				if (request.Faction != null && pawn.RaceProps.Animal)
				{
					pawn.GenerateNecessaryName();
				}
				if (Find.Scenario != null)
				{
					Find.Scenario.Notify_NewPawnGenerating(pawn, request.Context);
				}
				if (!request.AllowDead && (pawn.Dead || pawn.Destroyed))
				{
					PawnGenerator.DiscardGeneratedPawn(pawn);
					error = "Generated dead pawn.";
					result = null;
				}
				else if (!request.AllowDowned && pawn.Downed)
				{
					PawnGenerator.DiscardGeneratedPawn(pawn);
					error = "Generated downed pawn.";
					result = null;
				}
				else if (request.MustBeCapableOfViolence && ((pawn.story != null && pawn.WorkTagIsDisabled(WorkTags.Violent)) || (pawn.RaceProps.ToolUser && !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))))
				{
					PawnGenerator.DiscardGeneratedPawn(pawn);
					error = "Generated pawn incapable of violence.";
					result = null;
				}
				else
				{
					if (request.KindDef != null && !request.KindDef.skills.NullOrEmpty<SkillRange>())
					{
						List<SkillRange> skills = request.KindDef.skills;
						for (int i = 0; i < skills.Count; i++)
						{
							if (pawn.skills.GetSkill(skills[i].Skill).TotallyDisabled)
							{
								error = "Generated pawn incapable of required skill: " + skills[i].Skill.defName;
								return null;
							}
						}
					}
					if (request.KindDef.requiredWorkTags != WorkTags.None && (pawn.CombinedDisabledWorkTags & request.KindDef.requiredWorkTags) != WorkTags.None)
					{
						PawnGenerator.DiscardGeneratedPawn(pawn);
						error = "Generated pawn with disabled requiredWorkTags.";
						result = null;
					}
					else if (!ignoreScenarioRequirements && request.Context == PawnGenerationContext.PlayerStarter && Find.Scenario != null && !Find.Scenario.AllowPlayerStartingPawn(pawn, false, request))
					{
						PawnGenerator.DiscardGeneratedPawn(pawn);
						error = "Generated pawn doesn't meet scenario requirements.";
						result = null;
					}
					else if (!ignoreValidator && request.ValidatorPreGear != null && !request.ValidatorPreGear(pawn))
					{
						PawnGenerator.DiscardGeneratedPawn(pawn);
						error = "Generated pawn didn't pass validator check (pre-gear).";
						result = null;
					}
					else
					{
						if (!request.Newborn)
						{
							PawnGenerator.GenerateGearFor(pawn, request);
						}
						if (!ignoreValidator && request.ValidatorPostGear != null && !request.ValidatorPostGear(pawn))
						{
							PawnGenerator.DiscardGeneratedPawn(pawn);
							error = "Generated pawn didn't pass validator check (post-gear).";
							result = null;
						}
						else
						{
							for (int j = 0; j < PawnGenerator.pawnsBeingGenerated.Count - 1; j++)
							{
								if (PawnGenerator.pawnsBeingGenerated[j].PawnsGeneratedInTheMeantime == null)
								{
									PawnGenerator.pawnsBeingGenerated[j] = new PawnGenerator.PawnGenerationStatus(PawnGenerator.pawnsBeingGenerated[j].Pawn, new List<Pawn>());
								}
								PawnGenerator.pawnsBeingGenerated[j].PawnsGeneratedInTheMeantime.Add(pawn);
							}
							result = pawn;
						}
					}
				}
			}
			finally
			{
				PawnGenerator.pawnsBeingGenerated.RemoveLast<PawnGenerator.PawnGenerationStatus>();
			}
			return result;
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x00069CA0 File Offset: 0x00067EA0
		private static void DiscardGeneratedPawn(Pawn pawn)
		{
			if (Find.WorldPawns.Contains(pawn))
			{
				Find.WorldPawns.RemovePawn(pawn);
			}
			Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
			List<Pawn> pawnsGeneratedInTheMeantime = PawnGenerator.pawnsBeingGenerated.Last<PawnGenerator.PawnGenerationStatus>().PawnsGeneratedInTheMeantime;
			if (pawnsGeneratedInTheMeantime != null)
			{
				for (int i = 0; i < pawnsGeneratedInTheMeantime.Count; i++)
				{
					Pawn pawn2 = pawnsGeneratedInTheMeantime[i];
					if (Find.WorldPawns.Contains(pawn2))
					{
						Find.WorldPawns.RemovePawn(pawn2);
					}
					Find.WorldPawns.PassToWorld(pawn2, PawnDiscardDecideMode.Discard);
					for (int j = 0; j < PawnGenerator.pawnsBeingGenerated.Count; j++)
					{
						PawnGenerator.pawnsBeingGenerated[j].PawnsGeneratedInTheMeantime.Remove(pawn2);
					}
				}
			}
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x00069D58 File Offset: 0x00067F58
		private static IEnumerable<Pawn> GetValidCandidatesToRedress(PawnGenerationRequest request)
		{
			IEnumerable<Pawn> enumerable = Find.WorldPawns.GetPawnsBySituation(WorldPawnSituation.Free);
			if (request.KindDef.factionLeader)
			{
				enumerable = enumerable.Concat(Find.WorldPawns.GetPawnsBySituation(WorldPawnSituation.FactionLeader));
			}
			return from x in enumerable
			where PawnGenerator.IsValidCandidateToRedress(x, request)
			select x;
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x00069DB8 File Offset: 0x00067FB8
		private static float ChanceToRedressAnyWorldPawn(PawnGenerationRequest request)
		{
			int pawnsBySituationCount = Find.WorldPawns.GetPawnsBySituationCount(WorldPawnSituation.Free);
			float num = Mathf.Min(0.02f + 0.01f * ((float)pawnsBySituationCount / 10f), 0.8f);
			if (request.MinChanceToRedressWorldPawn != null)
			{
				num = Mathf.Max(num, request.MinChanceToRedressWorldPawn.Value);
			}
			return num;
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x00069E18 File Offset: 0x00068018
		private static float WorldPawnSelectionWeight(Pawn p)
		{
			if (p.RaceProps.IsFlesh && !p.relations.everSeenByPlayer && p.relations.RelatedToAnyoneOrAnyoneRelatedToMe)
			{
				return 0.1f;
			}
			return 1f;
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x00069E4C File Offset: 0x0006804C
		private static void GenerateGearFor(Pawn pawn, PawnGenerationRequest request)
		{
			PawnApparelGenerator.GenerateStartingApparelFor(pawn, request);
			PawnWeaponGenerator.TryGenerateWeaponFor(pawn, request);
			PawnInventoryGenerator.GenerateInventoryFor(pawn, request);
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x00069E64 File Offset: 0x00068064
		private static void GenerateInitialHediffs(Pawn pawn, PawnGenerationRequest request)
		{
			int num = 0;
			do
			{
				AgeInjuryUtility.GenerateRandomOldAgeInjuries(pawn, !request.AllowDead);
				PawnTechHediffsGenerator.GenerateTechHediffsFor(pawn);
				if (request.AllowAddictions)
				{
					PawnAddictionHediffsGenerator.GenerateAddictionsAndTolerancesFor(pawn);
				}
				if ((request.AllowDead && pawn.Dead) || request.AllowDowned || !pawn.Downed)
				{
					goto IL_C0;
				}
				pawn.health.Reset();
				num++;
			}
			while (num <= 80);
			Log.Warning(string.Concat(new object[]
			{
				"Could not generate old age injuries for ",
				pawn.ThingID,
				" of age ",
				pawn.ageTracker.AgeBiologicalYears,
				" that allow pawn to move after ",
				80,
				" tries. request=",
				request
			}), false);
			IL_C0:
			if (!pawn.Dead && (request.Faction == null || !request.Faction.IsPlayer))
			{
				int num2 = 0;
				while (pawn.health.HasHediffsNeedingTend(false))
				{
					num2++;
					if (num2 > 10000)
					{
						Log.Error("Too many iterations.", false);
						return;
					}
					TendUtility.DoTend(null, pawn, null);
				}
			}
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x00069F84 File Offset: 0x00068184
		private static void GenerateRandomAge(Pawn pawn, PawnGenerationRequest request)
		{
			if (request.FixedBiologicalAge != null && request.FixedChronologicalAge != null)
			{
				float? fixedBiologicalAge = request.FixedBiologicalAge;
				float? fixedChronologicalAge = request.FixedChronologicalAge;
				if (fixedBiologicalAge.GetValueOrDefault() > fixedChronologicalAge.GetValueOrDefault() & (fixedBiologicalAge != null & fixedChronologicalAge != null))
				{
					Log.Warning(string.Concat(new object[]
					{
						"Tried to generate age for pawn ",
						pawn,
						", but pawn generation request demands biological age (",
						request.FixedBiologicalAge,
						") to be greater than chronological age (",
						request.FixedChronologicalAge,
						")."
					}), false);
				}
			}
			if (request.Newborn)
			{
				pawn.ageTracker.AgeBiologicalTicks = 0L;
			}
			else if (request.FixedBiologicalAge != null)
			{
				pawn.ageTracker.AgeBiologicalTicks = (long)(request.FixedBiologicalAge.Value * 3600000f);
			}
			else
			{
				int num = 0;
				float num2;
				for (;;)
				{
					if (pawn.RaceProps.ageGenerationCurve != null)
					{
						num2 = (float)Mathf.RoundToInt(Rand.ByCurve(pawn.RaceProps.ageGenerationCurve));
					}
					else if (pawn.RaceProps.IsMechanoid)
					{
						num2 = Rand.Range(0f, 2500f);
					}
					else
					{
						num2 = Rand.ByCurve(PawnGenerator.DefaultAgeGenerationCurve) * pawn.RaceProps.lifeExpectancy;
					}
					num++;
					if (num > 300)
					{
						break;
					}
					if (num2 <= (float)pawn.kindDef.maxGenerationAge && num2 >= (float)pawn.kindDef.minGenerationAge)
					{
						goto IL_1A6;
					}
				}
				Log.Error("Tried 300 times to generate age for " + pawn, false);
				IL_1A6:
				pawn.ageTracker.AgeBiologicalTicks = (long)(num2 * 3600000f) + (long)Rand.Range(0, 3600000);
			}
			if (request.Newborn)
			{
				pawn.ageTracker.AgeChronologicalTicks = 0L;
			}
			else if (request.FixedChronologicalAge != null)
			{
				pawn.ageTracker.AgeChronologicalTicks = (long)(request.FixedChronologicalAge.Value * 3600000f);
			}
			else
			{
				int num3;
				if (request.CertainlyBeenInCryptosleep || Rand.Value < pawn.kindDef.backstoryCryptosleepCommonality)
				{
					float value = Rand.Value;
					if (value < 0.7f)
					{
						num3 = Rand.Range(0, 100);
					}
					else if (value < 0.95f)
					{
						num3 = Rand.Range(100, 1000);
					}
					else
					{
						int max = GenDate.Year((long)GenTicks.TicksAbs, 0f) - 2026 - pawn.ageTracker.AgeBiologicalYears;
						num3 = Rand.Range(1000, max);
					}
				}
				else
				{
					num3 = 0;
				}
				long num4 = (long)GenTicks.TicksAbs - pawn.ageTracker.AgeBiologicalTicks;
				num4 -= (long)num3 * 3600000L;
				pawn.ageTracker.BirthAbsTicks = num4;
			}
			if (pawn.ageTracker.AgeBiologicalTicks > pawn.ageTracker.AgeChronologicalTicks)
			{
				pawn.ageTracker.AgeChronologicalTicks = pawn.ageTracker.AgeBiologicalTicks;
			}
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x0006A290 File Offset: 0x00068490
		public static int RandomTraitDegree(TraitDef traitDef)
		{
			if (traitDef.degreeDatas.Count == 1)
			{
				return traitDef.degreeDatas[0].degree;
			}
			return traitDef.degreeDatas.RandomElementByWeight((TraitDegreeData dd) => dd.commonality).degree;
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x0006A2EC File Offset: 0x000684EC
		private static void GenerateTraits(Pawn pawn, PawnGenerationRequest request)
		{
			if (pawn.story == null)
			{
				return;
			}
			if (request.ForcedTraits != null)
			{
				foreach (TraitDef traitDef in request.ForcedTraits)
				{
					if (traitDef != null)
					{
						pawn.story.traits.GainTrait(new Trait(traitDef, 0, true));
					}
				}
			}
			if (pawn.story.childhood.forcedTraits != null)
			{
				List<TraitEntry> forcedTraits = pawn.story.childhood.forcedTraits;
				for (int i = 0; i < forcedTraits.Count; i++)
				{
					TraitEntry traitEntry = forcedTraits[i];
					if (traitEntry.def == null)
					{
						Log.Error("Null forced trait def on " + pawn.story.childhood, false);
					}
					else if ((request.KindDef.disallowedTraits == null || !request.KindDef.disallowedTraits.Contains(traitEntry.def)) && !pawn.story.traits.HasTrait(traitEntry.def) && (request.ProhibitedTraits == null || !request.ProhibitedTraits.Contains(traitEntry.def)))
					{
						pawn.story.traits.GainTrait(new Trait(traitEntry.def, traitEntry.degree, false));
					}
				}
			}
			if (pawn.story.adulthood != null && pawn.story.adulthood.forcedTraits != null)
			{
				List<TraitEntry> forcedTraits2 = pawn.story.adulthood.forcedTraits;
				for (int j = 0; j < forcedTraits2.Count; j++)
				{
					TraitEntry traitEntry2 = forcedTraits2[j];
					if (traitEntry2.def == null)
					{
						Log.Error("Null forced trait def on " + pawn.story.adulthood, false);
					}
					else if ((request.KindDef.disallowedTraits == null || !request.KindDef.disallowedTraits.Contains(traitEntry2.def)) && !pawn.story.traits.HasTrait(traitEntry2.def) && (request.ProhibitedTraits == null || !request.ProhibitedTraits.Contains(traitEntry2.def)))
					{
						pawn.story.traits.GainTrait(new Trait(traitEntry2.def, traitEntry2.degree, false));
					}
				}
			}
			int num = Rand.RangeInclusive(2, 3);
			if (request.AllowGay && (LovePartnerRelationUtility.HasAnyLovePartnerOfTheSameGender(pawn) || LovePartnerRelationUtility.HasAnyExLovePartnerOfTheSameGender(pawn)))
			{
				Trait trait = new Trait(TraitDefOf.Gay, PawnGenerator.RandomTraitDegree(TraitDefOf.Gay), false);
				pawn.story.traits.GainTrait(trait);
			}
			Func<TraitDef, float> <>9__0;
			Predicate<SkillDef> <>9__2;
			while (pawn.story.traits.allTraits.Count < num)
			{
				PawnGenerator.<>c__DisplayClass24_1 <>c__DisplayClass24_2 = new PawnGenerator.<>c__DisplayClass24_1();
				PawnGenerator.<>c__DisplayClass24_1 <>c__DisplayClass24_3 = <>c__DisplayClass24_2;
				IEnumerable<TraitDef> allDefsListForReading = DefDatabase<TraitDef>.AllDefsListForReading;
				Func<TraitDef, float> weightSelector;
				if ((weightSelector = <>9__0) == null)
				{
					weightSelector = (<>9__0 = ((TraitDef tr) => tr.GetGenderSpecificCommonality(pawn.gender)));
				}
				<>c__DisplayClass24_3.newTraitDef = allDefsListForReading.RandomElementByWeight(weightSelector);
				if (!pawn.story.traits.HasTrait(<>c__DisplayClass24_2.newTraitDef) && (request.KindDef.disallowedTraits == null || !request.KindDef.disallowedTraits.Contains(<>c__DisplayClass24_2.newTraitDef)) && (request.KindDef.requiredWorkTags == WorkTags.None || (<>c__DisplayClass24_2.newTraitDef.disabledWorkTags & request.KindDef.requiredWorkTags) == WorkTags.None) && (<>c__DisplayClass24_2.newTraitDef != TraitDefOf.Gay || (request.AllowGay && !LovePartnerRelationUtility.HasAnyLovePartnerOfTheOppositeGender(pawn) && !LovePartnerRelationUtility.HasAnyExLovePartnerOfTheOppositeGender(pawn))) && (request.ProhibitedTraits == null || !request.ProhibitedTraits.Contains(<>c__DisplayClass24_2.newTraitDef)) && (request.Faction == null || Faction.OfPlayerSilentFail == null || !request.Faction.HostileTo(Faction.OfPlayer) || <>c__DisplayClass24_2.newTraitDef.allowOnHostileSpawn) && !pawn.story.traits.allTraits.Any((Trait tr) => <>c__DisplayClass24_2.newTraitDef.ConflictsWith(tr)) && (<>c__DisplayClass24_2.newTraitDef.requiredWorkTypes == null || !pawn.OneOfWorkTypesIsDisabled(<>c__DisplayClass24_2.newTraitDef.requiredWorkTypes)) && !pawn.WorkTagIsDisabled(<>c__DisplayClass24_2.newTraitDef.requiredWorkTags))
				{
					if (<>c__DisplayClass24_2.newTraitDef.forcedPassions != null && pawn.workSettings != null)
					{
						List<SkillDef> forcedPassions = <>c__DisplayClass24_2.newTraitDef.forcedPassions;
						Predicate<SkillDef> predicate;
						if ((predicate = <>9__2) == null)
						{
							predicate = (<>9__2 = ((SkillDef p) => p.IsDisabled(pawn.story.DisabledWorkTagsBackstoryAndTraits, pawn.GetDisabledWorkTypes(true))));
						}
						if (forcedPassions.Any(predicate))
						{
							continue;
						}
					}
					int degree = PawnGenerator.RandomTraitDegree(<>c__DisplayClass24_2.newTraitDef);
					if (!pawn.story.childhood.DisallowsTrait(<>c__DisplayClass24_2.newTraitDef, degree) && (pawn.story.adulthood == null || !pawn.story.adulthood.DisallowsTrait(<>c__DisplayClass24_2.newTraitDef, degree)))
					{
						Trait trait2 = new Trait(<>c__DisplayClass24_2.newTraitDef, degree, false);
						if (pawn.mindState == null || pawn.mindState.mentalBreaker == null || (pawn.mindState.mentalBreaker.BreakThresholdMinor + trait2.OffsetOfStat(StatDefOf.MentalBreakThreshold)) * trait2.MultiplierOfStat(StatDefOf.MentalBreakThreshold) <= 0.5f)
						{
							pawn.story.traits.GainTrait(trait2);
						}
					}
				}
			}
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x0006A91C File Offset: 0x00068B1C
		private static void GenerateBodyType(Pawn pawn)
		{
			if (pawn.story.adulthood != null)
			{
				pawn.story.bodyType = pawn.story.adulthood.BodyTypeFor(pawn.gender);
				return;
			}
			if (Rand.Value < 0.5f)
			{
				pawn.story.bodyType = BodyTypeDefOf.Thin;
				return;
			}
			pawn.story.bodyType = ((pawn.gender == Gender.Female) ? BodyTypeDefOf.Female : BodyTypeDefOf.Male);
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x0006A998 File Offset: 0x00068B98
		private static void GenerateSkills(Pawn pawn)
		{
			List<SkillDef> allDefsListForReading = DefDatabase<SkillDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				SkillDef skillDef = allDefsListForReading[i];
				int num = PawnGenerator.FinalLevelOfSkill(pawn, skillDef);
				SkillRecord skill = pawn.skills.GetSkill(skillDef);
				skill.Level = num;
				if (!skill.TotallyDisabled)
				{
					bool flag = false;
					bool flag2 = false;
					foreach (Trait trait in pawn.story.traits.allTraits)
					{
						if (trait.def.ConflictsWithPassion(skillDef))
						{
							flag = true;
							flag2 = false;
							break;
						}
						if (trait.def.RequiresPassion(skillDef))
						{
							flag2 = true;
						}
					}
					if (!flag)
					{
						float num2 = (float)num * 0.11f;
						float value = Rand.Value;
						if (flag2 || value < num2)
						{
							if (value < num2 * 0.2f)
							{
								skill.passion = Passion.Major;
							}
							else
							{
								skill.passion = Passion.Minor;
							}
						}
					}
					skill.xpSinceLastLevel = Rand.Range(skill.XpRequiredForLevelUp * 0.1f, skill.XpRequiredForLevelUp * 0.9f);
				}
			}
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x0006AAD4 File Offset: 0x00068CD4
		private static int FinalLevelOfSkill(Pawn pawn, SkillDef sk)
		{
			float num;
			if (sk.usuallyDefinedInBackstories)
			{
				num = (float)Rand.RangeInclusive(0, 4);
			}
			else
			{
				num = Rand.ByCurve(PawnGenerator.LevelRandomCurve);
			}
			foreach (Backstory backstory in from bs in pawn.story.AllBackstories
			where bs != null
			select bs)
			{
				foreach (KeyValuePair<SkillDef, int> keyValuePair in backstory.skillGainsResolved)
				{
					if (keyValuePair.Key == sk)
					{
						num += (float)keyValuePair.Value * Rand.Range(1f, 1.4f);
					}
				}
			}
			for (int i = 0; i < pawn.story.traits.allTraits.Count; i++)
			{
				int num2 = 0;
				if (pawn.story.traits.allTraits[i].CurrentData.skillGains.TryGetValue(sk, out num2))
				{
					num += (float)num2;
				}
			}
			float num3 = Rand.Range(1f, PawnGenerator.AgeSkillMaxFactorCurve.Evaluate((float)pawn.ageTracker.AgeBiologicalYears));
			num *= num3;
			num = PawnGenerator.LevelFinalAdjustmentCurve.Evaluate(num);
			if (pawn.kindDef.skills != null)
			{
				foreach (SkillRange skillRange in pawn.kindDef.skills)
				{
					if (skillRange.Skill == sk)
					{
						if (num < (float)skillRange.Range.min || num > (float)skillRange.Range.max)
						{
							num = (float)skillRange.Range.RandomInRange;
							break;
						}
						break;
					}
				}
			}
			return Mathf.Clamp(Mathf.RoundToInt(num), 0, 20);
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x0006ACE4 File Offset: 0x00068EE4
		public static void PostProcessGeneratedGear(Thing gear, Pawn pawn)
		{
			CompQuality compQuality = gear.TryGetComp<CompQuality>();
			if (compQuality != null)
			{
				QualityCategory q = QualityUtility.GenerateQualityGeneratingPawn(pawn.kindDef);
				if (pawn.royalty != null && pawn.Faction != null)
				{
					RoyalTitleDef currentTitle = pawn.royalty.GetCurrentTitle(pawn.Faction);
					if (currentTitle != null)
					{
						q = (QualityCategory)Mathf.Clamp((int)QualityUtility.GenerateQualityGeneratingPawn(pawn.kindDef), (int)currentTitle.requiredMinimumApparelQuality, 6);
					}
				}
				compQuality.SetQuality(q, ArtGenerationContext.Outsider);
			}
			if (gear.def.useHitPoints)
			{
				float randomInRange = pawn.kindDef.gearHealthRange.RandomInRange;
				if (randomInRange < 1f)
				{
					int num = Mathf.RoundToInt(randomInRange * (float)gear.MaxHitPoints);
					num = Mathf.Max(1, num);
					gear.HitPoints = num;
				}
			}
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x0006AD98 File Offset: 0x00068F98
		private static void GeneratePawnRelations(Pawn pawn, ref PawnGenerationRequest request)
		{
			if (!pawn.RaceProps.Humanlike)
			{
				return;
			}
			Pawn[] array = (from x in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead
			where x.def == pawn.def
			select x).ToArray<Pawn>();
			if (array.Length == 0)
			{
				return;
			}
			int num = 0;
			foreach (Pawn pawn2 in array)
			{
				if (pawn2.Discarded)
				{
					Log.Warning(string.Concat(new object[]
					{
						"Warning during generating pawn relations for ",
						pawn,
						": Pawn ",
						pawn2,
						" is discarded, yet he was yielded by PawnUtility. Discarding a pawn means that he is no longer managed by anything."
					}), false);
				}
				else if (pawn2.Faction != null && pawn2.Faction.IsPlayer)
				{
					num++;
				}
			}
			float num2 = 45f;
			num2 += (float)num * 2.7f;
			PawnGenerationRequest localReq = request;
			Pair<Pawn, PawnRelationDef> pair = PawnGenerator.GenerateSamples(array, PawnGenerator.relationsGeneratableBlood, 40).RandomElementByWeightWithDefault((Pair<Pawn, PawnRelationDef> x) => x.Second.generationChanceFactor * x.Second.Worker.GenerationChance(pawn, x.First, localReq), num2 * 40f / (float)(array.Length * PawnGenerator.relationsGeneratableBlood.Length));
			if (pair.First != null)
			{
				pair.Second.Worker.CreateRelation(pawn, pair.First, ref request);
			}
			Pair<Pawn, PawnRelationDef> pair2 = PawnGenerator.GenerateSamples(array, PawnGenerator.relationsGeneratableNonblood, 40).RandomElementByWeightWithDefault((Pair<Pawn, PawnRelationDef> x) => x.Second.generationChanceFactor * x.Second.Worker.GenerationChance(pawn, x.First, localReq), num2 * 40f / (float)(array.Length * PawnGenerator.relationsGeneratableNonblood.Length));
			if (pair2.First != null)
			{
				pair2.Second.Worker.CreateRelation(pawn, pair2.First, ref request);
			}
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x0006AF38 File Offset: 0x00069138
		private static Pair<Pawn, PawnRelationDef>[] GenerateSamples(Pawn[] pawns, PawnRelationDef[] relations, int count)
		{
			Pair<Pawn, PawnRelationDef>[] array = new Pair<Pawn, PawnRelationDef>[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = new Pair<Pawn, PawnRelationDef>(pawns[Rand.Range(0, pawns.Length)], relations[Rand.Range(0, relations.Length)]);
			}
			return array;
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x0006AF7C File Offset: 0x0006917C
		[DebugOutput("Performance", false)]
		public static void PawnGenerationHistogram()
		{
			DebugHistogram debugHistogram = new DebugHistogram((from x in Enumerable.Range(1, 20)
			select (float)x * 10f).ToArray<float>());
			for (int i = 0; i < 100; i++)
			{
				long timestamp = Stopwatch.GetTimestamp();
				Thing thing = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Colonist, null, PawnGenerationContext.NonPlayer, -1, true, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null));
				debugHistogram.Add((float)((Stopwatch.GetTimestamp() - timestamp) * 1000L / Stopwatch.Frequency));
				thing.Destroy(DestroyMode.Vanish);
			}
			debugHistogram.Display();
		}

		// Token: 0x04000CBC RID: 3260
		private static List<PawnGenerator.PawnGenerationStatus> pawnsBeingGenerated = new List<PawnGenerator.PawnGenerationStatus>();

		// Token: 0x04000CBD RID: 3261
		private static PawnRelationDef[] relationsGeneratableBlood = (from rel in DefDatabase<PawnRelationDef>.AllDefsListForReading
		where rel.familyByBloodRelation && rel.generationChanceFactor > 0f
		select rel).ToArray<PawnRelationDef>();

		// Token: 0x04000CBE RID: 3262
		private static PawnRelationDef[] relationsGeneratableNonblood = (from rel in DefDatabase<PawnRelationDef>.AllDefsListForReading
		where !rel.familyByBloodRelation && rel.generationChanceFactor > 0f
		select rel).ToArray<PawnRelationDef>();

		// Token: 0x04000CBF RID: 3263
		public const float MaxStartMinorMentalBreakThreshold = 0.5f;

		// Token: 0x04000CC0 RID: 3264
		private static SimpleCurve DefaultAgeGenerationCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.05f, 0f),
				true
			},
			{
				new CurvePoint(0.1f, 100f),
				true
			},
			{
				new CurvePoint(0.675f, 100f),
				true
			},
			{
				new CurvePoint(0.75f, 30f),
				true
			},
			{
				new CurvePoint(0.875f, 18f),
				true
			},
			{
				new CurvePoint(1f, 10f),
				true
			},
			{
				new CurvePoint(1.125f, 3f),
				true
			},
			{
				new CurvePoint(1.25f, 0f),
				true
			}
		};

		// Token: 0x04000CC1 RID: 3265
		public const float MaxGeneratedMechanoidAge = 2500f;

		// Token: 0x04000CC2 RID: 3266
		private static readonly SimpleCurve AgeSkillMaxFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(10f, 0.7f),
				true
			},
			{
				new CurvePoint(35f, 1f),
				true
			},
			{
				new CurvePoint(60f, 1.6f),
				true
			}
		};

		// Token: 0x04000CC3 RID: 3267
		private static readonly SimpleCurve LevelFinalAdjustmentCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(10f, 10f),
				true
			},
			{
				new CurvePoint(20f, 16f),
				true
			},
			{
				new CurvePoint(27f, 20f),
				true
			}
		};

		// Token: 0x04000CC4 RID: 3268
		private static readonly SimpleCurve LevelRandomCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(0.5f, 150f),
				true
			},
			{
				new CurvePoint(4f, 150f),
				true
			},
			{
				new CurvePoint(5f, 25f),
				true
			},
			{
				new CurvePoint(10f, 5f),
				true
			},
			{
				new CurvePoint(15f, 0f),
				true
			}
		};

		// Token: 0x02001466 RID: 5222
		private struct PawnGenerationStatus
		{
			// Token: 0x170014A7 RID: 5287
			// (get) Token: 0x06007A73 RID: 31347 RVA: 0x00298B6D File Offset: 0x00296D6D
			// (set) Token: 0x06007A74 RID: 31348 RVA: 0x00298B75 File Offset: 0x00296D75
			public Pawn Pawn { get; private set; }

			// Token: 0x170014A8 RID: 5288
			// (get) Token: 0x06007A75 RID: 31349 RVA: 0x00298B7E File Offset: 0x00296D7E
			// (set) Token: 0x06007A76 RID: 31350 RVA: 0x00298B86 File Offset: 0x00296D86
			public List<Pawn> PawnsGeneratedInTheMeantime { get; private set; }

			// Token: 0x06007A77 RID: 31351 RVA: 0x00298B8F File Offset: 0x00296D8F
			public PawnGenerationStatus(Pawn pawn, List<Pawn> pawnsGeneratedInTheMeantime)
			{
				this = default(PawnGenerator.PawnGenerationStatus);
				this.Pawn = pawn;
				this.PawnsGeneratedInTheMeantime = pawnsGeneratedInTheMeantime;
			}
		}
	}
}
