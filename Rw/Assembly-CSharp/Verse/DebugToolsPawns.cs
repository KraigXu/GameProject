using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200033D RID: 829
	public static class DebugToolsPawns
	{
		// Token: 0x06001898 RID: 6296 RVA: 0x0008D9CC File Offset: 0x0008BBCC
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Resurrect()
		{
			foreach (Thing thing in UI.MouseCell().GetThingList(Find.CurrentMap).ToList<Thing>())
			{
				Corpse corpse = thing as Corpse;
				if (corpse != null)
				{
					ResurrectionUtility.Resurrect(corpse.InnerPawn);
				}
			}
		}

		// Token: 0x06001899 RID: 6297 RVA: 0x0008DA3C File Offset: 0x0008BC3C
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DamageUntilDown(Pawn p)
		{
			HealthUtility.DamageUntilDowned(p, true);
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x0008DA45 File Offset: 0x0008BC45
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DamageLegs(Pawn p)
		{
			HealthUtility.DamageLegsUntilIncapableOfMoving(p, true);
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x0008DA4E File Offset: 0x0008BC4E
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DamageToDeath(Pawn p)
		{
			HealthUtility.DamageUntilDead(p);
		}

		// Token: 0x0600189C RID: 6300 RVA: 0x0008DA56 File Offset: 0x0008BC56
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void CarriedDamageToDeath(Pawn p)
		{
			HealthUtility.DamageUntilDead(p.carryTracker.CarriedThing as Pawn);
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x0008DA70 File Offset: 0x0008BC70
		[DebugAction("Pawns", "10 damage until dead", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Do10DamageUntilDead()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				for (int i = 0; i < 1000; i++)
				{
					thing.TakeDamage(new DamageInfo(DamageDefOf.Crush, 10f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
					if (thing.Destroyed)
					{
						string str = "Took " + (i + 1) + " hits";
						Pawn pawn = thing as Pawn;
						if (pawn != null)
						{
							if (pawn.health.ShouldBeDeadFromLethalDamageThreshold())
							{
								str = str + " (reached lethal damage threshold of " + pawn.health.LethalDamageThreshold.ToString("0.#") + ")";
							}
							else if (PawnCapacityUtility.CalculatePartEfficiency(pawn.health.hediffSet, pawn.RaceProps.body.corePart, false, null) <= 0.0001f)
							{
								str += " (core part hp reached 0)";
							}
							else
							{
								PawnCapacityDef pawnCapacityDef = pawn.health.ShouldBeDeadFromRequiredCapacity();
								if (pawnCapacityDef != null)
								{
									str = str + " (incapable of " + pawnCapacityDef.defName + ")";
								}
							}
						}
						Log.Message(str + ".", false);
						break;
					}
				}
			}
		}

		// Token: 0x0600189E RID: 6302 RVA: 0x0008DC00 File Offset: 0x0008BE00
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DamageHeldPawnToDeath()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && pawn.carryTracker.CarriedThing != null && pawn.carryTracker.CarriedThing is Pawn)
				{
					HealthUtility.DamageUntilDead((Pawn)pawn.carryTracker.CarriedThing);
				}
			}
		}

		// Token: 0x0600189F RID: 6303 RVA: 0x0008DC9C File Offset: 0x0008BE9C
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SurgeryFailMinor(Pawn p)
		{
			BodyPartRecord bodyPartRecord = (from x in p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where !x.def.conceptual
			select x).RandomElement<BodyPartRecord>();
			Log.Message("part is " + bodyPartRecord, false);
			HealthUtility.GiveInjuriesOperationFailureMinor(p, bodyPartRecord);
		}

		// Token: 0x060018A0 RID: 6304 RVA: 0x0008DD00 File Offset: 0x0008BF00
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SurgeryFailCatastrophic(Pawn p)
		{
			BodyPartRecord bodyPartRecord = (from x in p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where !x.def.conceptual
			select x).RandomElement<BodyPartRecord>();
			Log.Message("part is " + bodyPartRecord, false);
			HealthUtility.GiveInjuriesOperationFailureCatastrophic(p, bodyPartRecord);
		}

		// Token: 0x060018A1 RID: 6305 RVA: 0x0008DD63 File Offset: 0x0008BF63
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SurgeryFailRidiculous(Pawn p)
		{
			HealthUtility.GiveInjuriesOperationFailureRidiculous(p);
		}

		// Token: 0x060018A2 RID: 6306 RVA: 0x0008DD6B File Offset: 0x0008BF6B
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RestoreBodyPart(Pawn p)
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_RestorePart(p)));
		}

		// Token: 0x060018A3 RID: 6307 RVA: 0x0008DD82 File Offset: 0x0008BF82
		[DebugAction("Pawns", "Apply damage...", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ApplyDamage()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_ApplyDamage()));
		}

		// Token: 0x060018A4 RID: 6308 RVA: 0x0008DD98 File Offset: 0x0008BF98
		[DebugAction("Pawns", "Add Hediff...", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddHediff()
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_AddHediff()));
		}

		// Token: 0x060018A5 RID: 6309 RVA: 0x0008DDAE File Offset: 0x0008BFAE
		[DebugAction("Pawns", "Remove Hediff...", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RemoveHediff(Pawn p)
		{
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_RemoveHediff(p)));
		}

		// Token: 0x060018A6 RID: 6310 RVA: 0x0008DDC8 File Offset: 0x0008BFC8
		[DebugAction("Pawns", "Heal random injury (10)", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void HealRandomInjury10(Pawn p)
		{
			Hediff_Injury hediff_Injury;
			if ((from x in p.health.hediffSet.GetHediffs<Hediff_Injury>()
			where x.CanHealNaturally() || x.CanHealFromTending()
			select x).TryRandomElement(out hediff_Injury))
			{
				hediff_Injury.Heal(10f);
			}
		}

		// Token: 0x060018A7 RID: 6311 RVA: 0x0008DE20 File Offset: 0x0008C020
		[DebugAction("Pawns", "Activate HediffGiver", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ActivateHediffGiver(Pawn p)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			if (p.RaceProps.hediffGiverSets != null)
			{
				foreach (HediffGiver localHdg2 in p.RaceProps.hediffGiverSets.SelectMany((HediffGiverSetDef set) => set.hediffGivers))
				{
					HediffGiver localHdg = localHdg2;
					list.Add(new FloatMenuOption(localHdg.hediff.defName, delegate
					{
						localHdg.TryApply(p, null);
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
			}
			if (list.Any<FloatMenuOption>())
			{
				Find.WindowStack.Add(new FloatMenu(list));
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x060018A8 RID: 6312 RVA: 0x0008DF2C File Offset: 0x0008C12C
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DiscoverHediffs(Pawn p)
		{
			foreach (Hediff hediff in p.health.hediffSet.hediffs)
			{
				if (!hediff.Visible)
				{
					hediff.Severity = Mathf.Max(hediff.Severity, hediff.def.stages.First((HediffStage s) => s.becomeVisible).minSeverity);
				}
			}
		}

		// Token: 0x060018A9 RID: 6313 RVA: 0x0008DFD0 File Offset: 0x0008C1D0
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GrantImmunities(Pawn p)
		{
			foreach (Hediff hediff in p.health.hediffSet.hediffs)
			{
				ImmunityRecord immunityRecord = p.health.immunity.GetImmunityRecord(hediff.def);
				if (immunityRecord != null)
				{
					immunityRecord.immunity = 1f;
				}
			}
		}

		// Token: 0x060018AA RID: 6314 RVA: 0x0008E04C File Offset: 0x0008C24C
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GiveBirth(Pawn p)
		{
			Hediff_Pregnant.DoBirthSpawn(p, null);
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x060018AB RID: 6315 RVA: 0x0008E05C File Offset: 0x0008C25C
		[DebugAction("Pawns", "Resistance -1", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ResistanceMinus1(Pawn p)
		{
			if (p.guest != null && p.guest.resistance > 0f)
			{
				p.guest.resistance = Mathf.Max(0f, p.guest.resistance - 1f);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x060018AC RID: 6316 RVA: 0x0008E0B0 File Offset: 0x0008C2B0
		[DebugAction("Pawns", "Resistance -10", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ResistanceMinus10(Pawn p)
		{
			if (p.guest != null && p.guest.resistance > 0f)
			{
				p.guest.resistance = Mathf.Max(0f, p.guest.resistance - 10f);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x060018AD RID: 6317 RVA: 0x0008E104 File Offset: 0x0008C304
		[DebugAction("Pawns", "+20 neural heat", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddPsychicEntropy()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				if (thing is Pawn)
				{
					((Pawn)thing).psychicEntropy.TryAddEntropy(20f, null, true, false);
				}
			}
		}

		// Token: 0x060018AE RID: 6318 RVA: 0x0008E184 File Offset: 0x0008C384
		[DebugAction("Pawns", "-20 neural heat", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ReducePsychicEntropy()
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).ToList<Thing>())
			{
				if (thing is Pawn)
				{
					((Pawn)thing).psychicEntropy.TryAddEntropy(-20f, null, true, false);
				}
			}
		}

		// Token: 0x060018AF RID: 6319 RVA: 0x0008E204 File Offset: 0x0008C404
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ListMeleeVerbs(Pawn p)
		{
			DebugToolsPawns.<>c__DisplayClass23_0 <>c__DisplayClass23_ = new DebugToolsPawns.<>c__DisplayClass23_0();
			<>c__DisplayClass23_.p = p;
			<>c__DisplayClass23_.allMeleeVerbs = (from x in <>c__DisplayClass23_.p.meleeVerbs.GetUpdatedAvailableVerbsList(false)
			select x.verb).ToList<Verb>();
			<>c__DisplayClass23_.highestWeight = 0f;
			foreach (Verb v2 in <>c__DisplayClass23_.allMeleeVerbs)
			{
				float num = VerbUtility.InitialVerbWeight(v2, <>c__DisplayClass23_.p);
				if (num > <>c__DisplayClass23_.highestWeight)
				{
					<>c__DisplayClass23_.highestWeight = num;
				}
			}
			<>c__DisplayClass23_.totalSelectionWeight = 0f;
			foreach (Verb verb in <>c__DisplayClass23_.allMeleeVerbs)
			{
				<>c__DisplayClass23_.totalSelectionWeight += VerbUtility.FinalSelectionWeight(verb, <>c__DisplayClass23_.p, <>c__DisplayClass23_.allMeleeVerbs, <>c__DisplayClass23_.highestWeight);
			}
			<>c__DisplayClass23_.allMeleeVerbs.SortBy((Verb x) => -VerbUtility.InitialVerbWeight(x, <>c__DisplayClass23_.p));
			List<TableDataGetter<Verb>> list = new List<TableDataGetter<Verb>>();
			list.Add(new TableDataGetter<Verb>("verb", (Verb v) => v.ToString().Split(new char[]
			{
				'/'
			})[1].TrimEnd(new char[]
			{
				')'
			})));
			list.Add(new TableDataGetter<Verb>("source", delegate(Verb v)
			{
				if (v.HediffSource != null)
				{
					return v.HediffSource.Label;
				}
				if (v.tool != null)
				{
					return v.tool.label;
				}
				return "";
			}));
			list.Add(new TableDataGetter<Verb>("damage", (Verb v) => v.verbProps.AdjustedMeleeDamageAmount(v, <>c__DisplayClass23_.p)));
			list.Add(new TableDataGetter<Verb>("cooldown", (Verb v) => v.verbProps.AdjustedCooldown(v, <>c__DisplayClass23_.p) + "s"));
			list.Add(new TableDataGetter<Verb>("dmg/sec", (Verb v) => VerbUtility.DPS(v, <>c__DisplayClass23_.p)));
			list.Add(new TableDataGetter<Verb>("armor pen", (Verb v) => v.verbProps.AdjustedArmorPenetration(v, <>c__DisplayClass23_.p)));
			list.Add(new TableDataGetter<Verb>("hediff", delegate(Verb v)
			{
				string text = "";
				if (v.verbProps.meleeDamageDef != null && !v.verbProps.meleeDamageDef.additionalHediffs.NullOrEmpty<DamageDefAdditionalHediff>())
				{
					foreach (DamageDefAdditionalHediff damageDefAdditionalHediff in v.verbProps.meleeDamageDef.additionalHediffs)
					{
						text = text + damageDefAdditionalHediff.hediff.label + " ";
					}
				}
				return text;
			}));
			list.Add(new TableDataGetter<Verb>("weight", (Verb v) => VerbUtility.InitialVerbWeight(v, <>c__DisplayClass23_.p)));
			list.Add(new TableDataGetter<Verb>("category", delegate(Verb v)
			{
				VerbSelectionCategory selectionCategory = v.GetSelectionCategory(<>c__DisplayClass23_.p, <>c__DisplayClass23_.highestWeight);
				if (selectionCategory == VerbSelectionCategory.Best)
				{
					return "Best".Colorize(Color.green);
				}
				if (selectionCategory != VerbSelectionCategory.Worst)
				{
					return "Mid";
				}
				return "Worst".Colorize(Color.grey);
			}));
			list.Add(new TableDataGetter<Verb>("sel %", (Verb v) => <>c__DisplayClass23_.<ListMeleeVerbs>g__GetSelectionPercent|1(v).ToStringPercent("F2")));
			List<TableDataGetter<Verb>> list2 = list;
			DebugTables.MakeTablesDialog<Verb>(<>c__DisplayClass23_.allMeleeVerbs, list2.ToArray());
		}

		// Token: 0x060018B0 RID: 6320 RVA: 0x0008E4B0 File Offset: 0x0008C6B0
		[DebugAction("Pawns", "Add/remove pawn relation", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddRemovePawnRelation(Pawn p)
		{
			if (!p.RaceProps.IsFlesh)
			{
				return;
			}
			Func<Pawn, bool> <>9__5;
			Action<bool> act = delegate(bool add)
			{
				if (add)
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					foreach (PawnRelationDef pawnRelationDef in DefDatabase<PawnRelationDef>.AllDefs)
					{
						if (!pawnRelationDef.implied)
						{
							PawnRelationDef defLocal = pawnRelationDef;
							list2.Add(new DebugMenuOption(defLocal.defName, DebugMenuOptionMode.Action, delegate
							{
								List<DebugMenuOption> list4 = new List<DebugMenuOption>();
								IEnumerable<Pawn> source = from x in PawnsFinder.AllMapsWorldAndTemporary_Alive
								where x.RaceProps.IsFlesh
								select x;
								Func<Pawn, bool> keySelector;
								if ((keySelector = <>9__5) == null)
								{
									keySelector = (<>9__5 = ((Pawn x) => x.def == p.def));
								}
								foreach (Pawn pawn in source.OrderByDescending(keySelector).ThenBy((Pawn x) => x.IsWorldPawn()))
								{
									if (p != pawn && (!defLocal.familyByBloodRelation || pawn.def == p.def) && !p.relations.DirectRelationExists(defLocal, pawn))
									{
										Pawn otherLocal = pawn;
										list4.Add(new DebugMenuOption(otherLocal.LabelShort + " (" + otherLocal.KindLabel + ")", DebugMenuOptionMode.Action, delegate
										{
											p.relations.AddDirectRelation(defLocal, otherLocal);
										}));
									}
								}
								Find.WindowStack.Add(new Dialog_DebugOptionListLister(list4));
							}));
						}
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
					return;
				}
				List<DebugMenuOption> list3 = new List<DebugMenuOption>();
				List<DirectPawnRelation> directRelations = p.relations.DirectRelations;
				for (int i = 0; i < directRelations.Count; i++)
				{
					DirectPawnRelation rel = directRelations[i];
					list3.Add(new DebugMenuOption(rel.def.defName + " - " + rel.otherPawn.LabelShort, DebugMenuOptionMode.Action, delegate
					{
						p.relations.RemoveDirectRelation(rel);
					}));
				}
				Find.WindowStack.Add(new Dialog_DebugOptionListLister(list3));
			};
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("Add", DebugMenuOptionMode.Action, delegate
			{
				act(true);
			}));
			list.Add(new DebugMenuOption("Remove", DebugMenuOptionMode.Action, delegate
			{
				act(false);
			}));
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060018B1 RID: 6321 RVA: 0x0008E540 File Offset: 0x0008C740
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void AddOpinionTalksAbout(Pawn p)
		{
			if (!p.RaceProps.Humanlike)
			{
				return;
			}
			Action<bool> act = delegate(bool good)
			{
				Func<ThoughtDef, bool> <>9__4;
				foreach (Pawn pawn in from x in p.Map.mapPawns.AllPawnsSpawned
				where x.RaceProps.Humanlike
				select x)
				{
					if (p != pawn)
					{
						IEnumerable<ThoughtDef> allDefs = DefDatabase<ThoughtDef>.AllDefs;
						Func<ThoughtDef, bool> predicate;
						if ((predicate = <>9__4) == null)
						{
							predicate = (<>9__4 = ((ThoughtDef x) => typeof(Thought_MemorySocial).IsAssignableFrom(x.thoughtClass) && ((good && x.stages[0].baseOpinionOffset > 0f) || (!good && x.stages[0].baseOpinionOffset < 0f))));
						}
						IEnumerable<ThoughtDef> source = allDefs.Where(predicate);
						if (source.Any<ThoughtDef>())
						{
							int num = Rand.Range(2, 5);
							for (int i = 0; i < num; i++)
							{
								ThoughtDef def = source.RandomElement<ThoughtDef>();
								pawn.needs.mood.thoughts.memories.TryGainMemory(def, p);
							}
						}
					}
				}
			};
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("Good", DebugMenuOptionMode.Action, delegate
			{
				act(true);
			}));
			list.Add(new DebugMenuOption("Bad", DebugMenuOptionMode.Action, delegate
			{
				act(false);
			}));
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060018B2 RID: 6322 RVA: 0x0008E5D0 File Offset: 0x0008C7D0
		[DebugAction("Pawns", "Force vomit...", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceVomit(Pawn p)
		{
			p.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Vomit), JobCondition.InterruptForced, null, true, true, null, null, false, false);
		}

		// Token: 0x060018B3 RID: 6323 RVA: 0x0008E602 File Offset: 0x0008C802
		[DebugAction("Pawns", "Psyfocus +20%", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void OffsetPsyfocusPositive20(Pawn p)
		{
			Pawn_PsychicEntropyTracker psychicEntropy = p.psychicEntropy;
			if (psychicEntropy == null)
			{
				return;
			}
			psychicEntropy.OffsetPsyfocusDirectly(0.2f);
		}

		// Token: 0x060018B4 RID: 6324 RVA: 0x0008E619 File Offset: 0x0008C819
		[DebugAction("Pawns", "Psyfocus -20%", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void OffsetPsyfocusNegative20(Pawn p)
		{
			Pawn_PsychicEntropyTracker psychicEntropy = p.psychicEntropy;
			if (psychicEntropy == null)
			{
				return;
			}
			psychicEntropy.OffsetPsyfocusDirectly(-0.2f);
		}

		// Token: 0x060018B5 RID: 6325 RVA: 0x0008E630 File Offset: 0x0008C830
		[DebugAction("Pawns", "Food -20%", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void OffsetFoodNegative20()
		{
			DebugToolsPawns.OffsetNeed(NeedDefOf.Food, -0.2f);
		}

		// Token: 0x060018B6 RID: 6326 RVA: 0x0008E641 File Offset: 0x0008C841
		[DebugAction("Pawns", "Rest -20%", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void OffsetRestNegative20()
		{
			DebugToolsPawns.OffsetNeed(NeedDefOf.Rest, -0.2f);
		}

		// Token: 0x060018B7 RID: 6327 RVA: 0x0008E652 File Offset: 0x0008C852
		[DebugAction("Pawns", "Joy -20%", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void OffsetJoyNegative20()
		{
			DebugToolsPawns.OffsetNeed(NeedDefOf.Joy, -0.2f);
		}

		// Token: 0x060018B8 RID: 6328 RVA: 0x0008E664 File Offset: 0x0008C864
		[DebugAction("Pawns", "Chemical -20%", actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void OffsetChemicalNegative20()
		{
			List<NeedDef> allDefsListForReading = DefDatabase<NeedDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (typeof(Need_Chemical).IsAssignableFrom(allDefsListForReading[i].needClass))
				{
					DebugToolsPawns.OffsetNeed(allDefsListForReading[i], -0.2f);
				}
			}
		}

		// Token: 0x060018B9 RID: 6329 RVA: 0x0008E6B8 File Offset: 0x0008C8B8
		[DebugAction("Pawns", null, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SetSkill()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (SkillDef localDef2 in DefDatabase<SkillDef>.AllDefs)
			{
				SkillDef localDef = localDef2;
				list.Add(new DebugMenuOption(localDef.defName, DebugMenuOptionMode.Action, delegate
				{
					List<DebugMenuOption> list2 = new List<DebugMenuOption>();
					for (int i = 0; i <= 20; i++)
					{
						int level = i;
						list2.Add(new DebugMenuOption(level.ToString(), DebugMenuOptionMode.Tool, delegate
						{
							Pawn pawn = (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
							where t is Pawn
							select t).Cast<Pawn>().FirstOrDefault<Pawn>();
							if (pawn == null)
							{
								return;
							}
							SkillRecord skill = pawn.skills.GetSkill(localDef);
							skill.Level = level;
							skill.xpSinceLastLevel = skill.XpRequiredForLevelUp / 2f;
							DebugActionsUtility.DustPuffFrom(pawn);
						}));
					}
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060018BA RID: 6330 RVA: 0x0008E744 File Offset: 0x0008C944
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MaxSkill(Pawn p)
		{
			if (p.skills != null)
			{
				foreach (SkillDef sDef in DefDatabase<SkillDef>.AllDefs)
				{
					p.skills.Learn(sDef, 1E+08f, false);
				}
				DebugActionsUtility.DustPuffFrom(p);
			}
			if (p.training != null)
			{
				foreach (TrainableDef td in DefDatabase<TrainableDef>.AllDefs)
				{
					Pawn trainer = p.Map.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>();
					bool flag;
					if (p.training.CanAssignToTrain(td, out flag).Accepted)
					{
						p.training.Train(td, trainer, false);
					}
				}
			}
		}

		// Token: 0x060018BB RID: 6331 RVA: 0x0008E824 File Offset: 0x0008CA24
		[DebugAction("Pawns", "Mental break...", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MentalBreak()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("(log possibles)", DebugMenuOptionMode.Tool, delegate
			{
				foreach (Pawn pawn in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
				where t is Pawn
				select t).Cast<Pawn>())
				{
					pawn.mindState.mentalBreaker.LogPossibleMentalBreaks();
					DebugActionsUtility.DustPuffFrom(pawn);
				}
			}));
			list.Add(new DebugMenuOption("(natural mood break)", DebugMenuOptionMode.Tool, delegate
			{
				foreach (Pawn pawn in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
				where t is Pawn
				select t).Cast<Pawn>())
				{
					pawn.mindState.mentalBreaker.TryDoRandomMoodCausedMentalBreak();
					DebugActionsUtility.DustPuffFrom(pawn);
				}
			}));
			foreach (MentalBreakDef locBrDef2 in from x in DefDatabase<MentalBreakDef>.AllDefs
			orderby x.intensity descending
			select x)
			{
				MentalBreakDef locBrDef = locBrDef2;
				string text = locBrDef.defName;
				if (!Find.CurrentMap.mapPawns.FreeColonists.Any((Pawn x) => locBrDef.Worker.BreakCanOccur(x)))
				{
					text += " [NO]";
				}
				list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Tool, delegate
				{
					foreach (Pawn pawn in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).Where((Thing t) => t is Pawn).Cast<Pawn>())
					{
						locBrDef.Worker.TryStart(pawn, null, false);
						DebugActionsUtility.DustPuffFrom(pawn);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060018BC RID: 6332 RVA: 0x0008E968 File Offset: 0x0008CB68
		[DebugAction("Pawns", "Mental state...", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void MentalState()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (MentalStateDef locBrDef2 in DefDatabase<MentalStateDef>.AllDefs)
			{
				MentalStateDef locBrDef = locBrDef2;
				string text = locBrDef.defName;
				if (!Find.CurrentMap.mapPawns.FreeColonists.Any((Pawn x) => locBrDef.Worker.StateCanOccur(x)))
				{
					text += " [NO]";
				}
				list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Tool, delegate
				{
					foreach (Pawn locP2 in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
					where t is Pawn
					select t).Cast<Pawn>())
					{
						Pawn locP = locP2;
						if (locBrDef != MentalStateDefOf.SocialFighting)
						{
							locP.mindState.mentalStateHandler.TryStartMentalState(locBrDef, null, true, false, null, false);
							DebugActionsUtility.DustPuffFrom(locP);
						}
						else
						{
							DebugTools.curTool = new DebugTool("...with", delegate
							{
								Pawn pawn = (Pawn)(from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
								where t is Pawn
								select t).FirstOrDefault<Thing>();
								if (pawn != null)
								{
									locP.interactions.StartSocialFight(pawn);
									DebugTools.curTool = null;
								}
							}, null);
						}
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060018BD RID: 6333 RVA: 0x0008EA28 File Offset: 0x0008CC28
		[DebugAction("Pawns", "Inspiration...", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Inspiration()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (InspirationDef localDef2 in DefDatabase<InspirationDef>.AllDefs)
			{
				InspirationDef localDef = localDef2;
				string text = localDef.defName;
				if (!Find.CurrentMap.mapPawns.FreeColonists.Any((Pawn x) => localDef.Worker.InspirationCanOccur(x)))
				{
					text += " [NO]";
				}
				list.Add(new DebugMenuOption(text, DebugMenuOptionMode.Tool, delegate
				{
					foreach (Pawn pawn in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).OfType<Pawn>())
					{
						pawn.mindState.inspirationHandler.TryStartInspiration_NewTemp(localDef, "Debug gain");
						DebugActionsUtility.DustPuffFrom(pawn);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060018BE RID: 6334 RVA: 0x0008EAE8 File Offset: 0x0008CCE8
		[DebugAction("Pawns", "Give trait...", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GiveTrait()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (TraitDef traitDef in DefDatabase<TraitDef>.AllDefs)
			{
				TraitDef trDef = traitDef;
				for (int j = 0; j < traitDef.degreeDatas.Count; j++)
				{
					int i = j;
					list.Add(new DebugMenuOption(string.Concat(new object[]
					{
						trDef.degreeDatas[i].label,
						" (",
						trDef.degreeDatas[j].degree,
						")"
					}), DebugMenuOptionMode.Tool, delegate
					{
						foreach (Pawn pawn in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
						where t is Pawn
						select t).Cast<Pawn>())
						{
							if (pawn.story != null)
							{
								Trait trait = new Trait(trDef, trDef.degreeDatas[i].degree, false);
								pawn.story.traits.GainTrait(trait);
								DebugActionsUtility.DustPuffFrom(pawn);
							}
						}
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060018BF RID: 6335 RVA: 0x0008EC0C File Offset: 0x0008CE0C
		[DebugAction("Pawns", "Set backstory...", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void SetBackstory()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("adulthood", DebugMenuOptionMode.Action, delegate
			{
				DebugToolsPawns.<SetBackstory>g__AddBackstoryOption|39_0(BackstorySlot.Adulthood);
			}));
			list.Add(new DebugMenuOption("childhood", DebugMenuOptionMode.Action, delegate
			{
				DebugToolsPawns.<SetBackstory>g__AddBackstoryOption|39_0(BackstorySlot.Childhood);
			}));
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060018C0 RID: 6336 RVA: 0x0008EC90 File Offset: 0x0008CE90
		[DebugAction("Pawns", "Give ability...", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GiveAbility()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("*All", DebugMenuOptionMode.Tool, delegate
			{
				foreach (Pawn pawn in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
				where t is Pawn
				select t).Cast<Pawn>())
				{
					foreach (AbilityDef def in DefDatabase<AbilityDef>.AllDefs)
					{
						pawn.abilities.GainAbility(def);
					}
				}
			}));
			foreach (AbilityDef abilityDef in DefDatabase<AbilityDef>.AllDefs)
			{
				AbilityDef localAb = abilityDef;
				list.Add(new DebugMenuOption(abilityDef.label, DebugMenuOptionMode.Tool, delegate
				{
					foreach (Pawn pawn in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
					where t is Pawn
					select t).Cast<Pawn>())
					{
						pawn.abilities.GainAbility(localAb);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060018C1 RID: 6337 RVA: 0x0008ED48 File Offset: 0x0008CF48
		[DebugAction("Pawns", "Give Psylink...", actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GivePsylink()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			for (int i = 1; i <= 6; i++)
			{
				int level = i;
				list.Add(new DebugMenuOption("Level".Translate() + ": " + i, DebugMenuOptionMode.Tool, delegate
				{
					foreach (Pawn pawn in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
					where t is Pawn
					select t).Cast<Pawn>())
					{
						Hediff_ImplantWithLevel hediff_ImplantWithLevel = pawn.GetMainPsylinkSource();
						if (hediff_ImplantWithLevel == null)
						{
							hediff_ImplantWithLevel = (HediffMaker.MakeHediff(HediffDefOf.PsychicAmplifier, pawn, pawn.health.hediffSet.GetBrain()) as Hediff_ImplantWithLevel);
							pawn.health.AddHediff(hediff_ImplantWithLevel, null, null, null);
						}
						hediff_ImplantWithLevel.ChangeLevel(level - hediff_ImplantWithLevel.level);
					}
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060018C2 RID: 6338 RVA: 0x0008EDC0 File Offset: 0x0008CFC0
		[DebugAction("Pawns", "Remove neural heat", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void RemovePsychicEntropy(Pawn p)
		{
			if (p.psychicEntropy != null)
			{
				p.psychicEntropy.TryAddEntropy(-1000f, null, true, false);
			}
		}

		// Token: 0x060018C3 RID: 6339 RVA: 0x0008EDDE File Offset: 0x0008CFDE
		[DebugAction("Pawns", "Give good thought...", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GiveGoodThought(Pawn p)
		{
			if (p.needs.mood != null)
			{
				p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DebugGood, null);
			}
		}

		// Token: 0x060018C4 RID: 6340 RVA: 0x0008EE0D File Offset: 0x0008D00D
		[DebugAction("Pawns", "Give bad thought...", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void GiveBadThought(Pawn p)
		{
			if (p.needs.mood != null)
			{
				p.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DebugBad, null);
			}
		}

		// Token: 0x060018C5 RID: 6341 RVA: 0x0008EE3C File Offset: 0x0008D03C
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ClearBoundUnfinishedThings()
		{
			foreach (Building_WorkTable building_WorkTable in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
			where t is Building_WorkTable
			select t).Cast<Building_WorkTable>())
			{
				foreach (Bill bill in building_WorkTable.BillStack)
				{
					Bill_ProductionWithUft bill_ProductionWithUft = bill as Bill_ProductionWithUft;
					if (bill_ProductionWithUft != null)
					{
						bill_ProductionWithUft.ClearBoundUft();
					}
				}
			}
		}

		// Token: 0x060018C6 RID: 6342 RVA: 0x0008EEFC File Offset: 0x0008D0FC
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceBirthday(Pawn p)
		{
			p.ageTracker.AgeBiologicalTicks = (long)((p.ageTracker.AgeBiologicalYears + 1) * 3600000 + 1);
			p.ageTracker.DebugForceBirthdayBiological();
		}

		// Token: 0x060018C7 RID: 6343 RVA: 0x0008EF2C File Offset: 0x0008D12C
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Recruit(Pawn p)
		{
			if (p.Faction != Faction.OfPlayer && p.RaceProps.Humanlike)
			{
				InteractionWorker_RecruitAttempt.DoRecruit(p.Map.mapPawns.FreeColonists.RandomElement<Pawn>(), p, 1f, true);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x060018C8 RID: 6344 RVA: 0x0008EF7C File Offset: 0x0008D17C
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void DamageApparel(Pawn p)
		{
			if (p.apparel != null && p.apparel.WornApparelCount > 0)
			{
				p.apparel.WornApparel.RandomElement<Apparel>().TakeDamage(new DamageInfo(DamageDefOf.Deterioration, 30f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x060018C9 RID: 6345 RVA: 0x0008EFD9 File Offset: 0x0008D1D9
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TameAnimal(Pawn p)
		{
			if (p.AnimalOrWildMan() && p.Faction != Faction.OfPlayer)
			{
				InteractionWorker_RecruitAttempt.DoRecruit(p.Map.mapPawns.FreeColonists.FirstOrDefault<Pawn>(), p, 1f, true);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x060018CA RID: 6346 RVA: 0x0008F018 File Offset: 0x0008D218
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TrainAnimal(Pawn p)
		{
			if (p.RaceProps.Animal && p.Faction == Faction.OfPlayer && p.training != null)
			{
				DebugActionsUtility.DustPuffFrom(p);
				bool flag = false;
				foreach (TrainableDef td in DefDatabase<TrainableDef>.AllDefs)
				{
					if (p.training.GetWanted(td))
					{
						p.training.Train(td, null, true);
						flag = true;
					}
				}
				if (!flag)
				{
					foreach (TrainableDef td2 in DefDatabase<TrainableDef>.AllDefs)
					{
						if (p.training.CanAssignToTrain(td2).Accepted)
						{
							p.training.Train(td2, null, true);
						}
					}
				}
			}
		}

		// Token: 0x060018CB RID: 6347 RVA: 0x0008F10C File Offset: 0x0008D30C
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void NameAnimalByNuzzling(Pawn p)
		{
			if ((p.Name == null || p.Name.Numerical) && p.RaceProps.Animal)
			{
				PawnUtility.GiveNameBecauseOfNuzzle(p.Map.mapPawns.FreeColonists.First<Pawn>(), p);
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x060018CC RID: 6348 RVA: 0x0008F15C File Offset: 0x0008D35C
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryDevelopBoundRelation(Pawn p)
		{
			if (p.Faction == null)
			{
				return;
			}
			if (p.RaceProps.Humanlike)
			{
				IEnumerable<Pawn> source = from x in p.Map.mapPawns.AllPawnsSpawned
				where x.RaceProps.Animal && x.Faction == p.Faction
				select x;
				if (source.Any<Pawn>())
				{
					RelationsUtility.TryDevelopBondRelation(p, source.RandomElement<Pawn>(), 999999f);
					return;
				}
			}
			else if (p.RaceProps.Animal)
			{
				IEnumerable<Pawn> source2 = from x in p.Map.mapPawns.AllPawnsSpawned
				where x.RaceProps.Humanlike && x.Faction == p.Faction
				select x;
				if (source2.Any<Pawn>())
				{
					RelationsUtility.TryDevelopBondRelation(source2.RandomElement<Pawn>(), p, 999999f);
				}
			}
		}

		// Token: 0x060018CD RID: 6349 RVA: 0x0008F235 File Offset: 0x0008D435
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void QueueTrainingDecay(Pawn p)
		{
			if (p.RaceProps.Animal && p.Faction == Faction.OfPlayer && p.training != null)
			{
				p.training.Debug_MakeDegradeHappenSoon();
				DebugActionsUtility.DustPuffFrom(p);
			}
		}

		// Token: 0x060018CE RID: 6350 RVA: 0x0008F26C File Offset: 0x0008D46C
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void StartMarriageCeremony(Pawn p)
		{
			if (!p.RaceProps.Humanlike)
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Pawn pawn in from x in p.Map.mapPawns.AllPawnsSpawned
			where x.RaceProps.Humanlike
			select x)
			{
				if (p != pawn)
				{
					Pawn otherLocal = pawn;
					list.Add(new DebugMenuOption(otherLocal.LabelShort + " (" + otherLocal.KindLabel + ")", DebugMenuOptionMode.Action, delegate
					{
						if (!p.relations.DirectRelationExists(PawnRelationDefOf.Fiance, otherLocal))
						{
							p.relations.TryRemoveDirectRelation(PawnRelationDefOf.Lover, otherLocal);
							p.relations.TryRemoveDirectRelation(PawnRelationDefOf.Spouse, otherLocal);
							p.relations.AddDirectRelation(PawnRelationDefOf.Fiance, otherLocal);
							Messages.Message("Dev: Auto added fiance relation.", p, MessageTypeDefOf.TaskCompletion, false);
						}
						if (!p.Map.lordsStarter.TryStartMarriageCeremony(p, otherLocal))
						{
							Messages.Message("Could not find any valid marriage site.", MessageTypeDefOf.RejectInput, false);
						}
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060018CF RID: 6351 RVA: 0x0008F384 File Offset: 0x0008D584
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ForceInteraction(Pawn p)
		{
			if (p.Faction == null)
			{
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Pawn pawn in p.Map.mapPawns.SpawnedPawnsInFaction(p.Faction))
			{
				if (pawn != p)
				{
					Pawn otherLocal = pawn;
					list.Add(new DebugMenuOption(otherLocal.LabelShort + " (" + otherLocal.KindLabel + ")", DebugMenuOptionMode.Action, delegate
					{
						List<DebugMenuOption> list2 = new List<DebugMenuOption>();
						foreach (InteractionDef interactionLocal2 in DefDatabase<InteractionDef>.AllDefsListForReading)
						{
							InteractionDef interactionLocal = interactionLocal2;
							list2.Add(new DebugMenuOption(interactionLocal.label, DebugMenuOptionMode.Action, delegate
							{
								p.interactions.TryInteractWith(otherLocal, interactionLocal);
							}));
						}
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(list2));
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060018D0 RID: 6352 RVA: 0x0008F484 File Offset: 0x0008D684
		[DebugAction("Pawns", null, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void StartRandomGathering()
		{
			if (!Find.CurrentMap.lordsStarter.TryStartRandomGathering(true))
			{
				Messages.Message("Could not find any valid gathering spot or organizer.", MessageTypeDefOf.RejectInput, false);
			}
		}

		// Token: 0x060018D1 RID: 6353 RVA: 0x0008F4A8 File Offset: 0x0008D6A8
		[DebugAction("Pawns", null, actionType = DebugActionType.Action, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void StartGathering()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (List<GatheringDef>.Enumerator enumerator = DefDatabase<GatheringDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GatheringDef gatheringDef = enumerator.Current;
					list.Add(new DebugMenuOption(gatheringDef.LabelCap + " (" + (gatheringDef.Worker.CanExecute(Find.CurrentMap, null) ? "Yes" : "No") + ")", DebugMenuOptionMode.Action, delegate
					{
						gatheringDef.Worker.TryExecute(Find.CurrentMap, null);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060018D2 RID: 6354 RVA: 0x0008F57C File Offset: 0x0008D77C
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void StartPrisonBreak(Pawn p)
		{
			if (!p.IsPrisoner)
			{
				return;
			}
			PrisonBreakUtility.StartPrisonBreak(p);
		}

		// Token: 0x060018D3 RID: 6355 RVA: 0x0008F58D File Offset: 0x0008D78D
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void PassToWorld(Pawn p)
		{
			p.DeSpawn(DestroyMode.Vanish);
			Find.WorldPawns.PassToWorld(p, PawnDiscardDecideMode.KeepForever);
		}

		// Token: 0x060018D4 RID: 6356 RVA: 0x0008F5A2 File Offset: 0x0008D7A2
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Make1YearOlder(Pawn p)
		{
			p.ageTracker.DebugMake1YearOlder();
		}

		// Token: 0x060018D5 RID: 6357 RVA: 0x0008F5B0 File Offset: 0x0008D7B0
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryJobGiver(Pawn p)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Type localType2 in typeof(ThinkNode_JobGiver).AllSubclasses())
			{
				Type localType = localType2;
				list.Add(new DebugMenuOption(localType.Name, DebugMenuOptionMode.Action, delegate
				{
					ThinkNode_JobGiver thinkNode_JobGiver = (ThinkNode_JobGiver)Activator.CreateInstance(localType);
					thinkNode_JobGiver.ResolveReferences();
					ThinkResult thinkResult = thinkNode_JobGiver.TryIssueJobPackage(p, default(JobIssueParams));
					if (thinkResult.Job != null)
					{
						p.jobs.StartJob(thinkResult.Job, JobCondition.None, null, false, true, null, null, false, false);
						return;
					}
					Messages.Message("Failed to give job", MessageTypeDefOf.RejectInput, false);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060018D6 RID: 6358 RVA: 0x0008F660 File Offset: 0x0008D860
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void TryJoyGiver(Pawn p)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (List<JoyGiverDef>.Enumerator enumerator = DefDatabase<JoyGiverDef>.AllDefsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					JoyGiverDef def = enumerator.Current;
					list.Add(new DebugMenuOption(def.Worker.CanBeGivenTo(p) ? def.defName : (def.defName + " [NO]"), DebugMenuOptionMode.Action, delegate
					{
						Job job = def.Worker.TryGiveJob(p);
						if (job != null)
						{
							p.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false);
							return;
						}
						Messages.Message("Failed to give job", MessageTypeDefOf.RejectInput, false);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list));
		}

		// Token: 0x060018D7 RID: 6359 RVA: 0x0008F738 File Offset: 0x0008D938
		[DebugAction("Pawns", "EndCurrentJob(InterruptForced)", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void EndCurrentJobInterruptForced(Pawn p)
		{
			p.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x060018D8 RID: 6360 RVA: 0x0008F74E File Offset: 0x0008D94E
		[DebugAction("Pawns", "CheckForJobOverride", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void CheckForJobOverride(Pawn p)
		{
			p.jobs.CheckForJobOverride();
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x060018D9 RID: 6361 RVA: 0x0008F764 File Offset: 0x0008D964
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ToggleJobLogging(Pawn p)
		{
			p.jobs.debugLog = !p.jobs.debugLog;
			DebugActionsUtility.DustPuffFrom(p);
			MoteMaker.ThrowText(p.DrawPos, p.Map, p.LabelShort + "\n" + (p.jobs.debugLog ? "ON" : "OFF"), -1f);
		}

		// Token: 0x060018DA RID: 6362 RVA: 0x0008F7CF File Offset: 0x0008D9CF
		[DebugAction("Pawns", null, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void ToggleStanceLogging(Pawn p)
		{
			p.stances.debugLog = !p.stances.debugLog;
			DebugActionsUtility.DustPuffFrom(p);
		}

		// Token: 0x060018DB RID: 6363 RVA: 0x0008F7F0 File Offset: 0x0008D9F0
		private static void OffsetNeed(NeedDef nd, float offsetPct)
		{
			foreach (Pawn pawn in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
			where t is Pawn
			select t).Cast<Pawn>())
			{
				Need need = pawn.needs.TryGetNeed(nd);
				if (need != null)
				{
					need.CurLevel += offsetPct * need.MaxLevel;
					DebugActionsUtility.DustPuffFrom(pawn);
				}
			}
		}

		// Token: 0x060018DC RID: 6364 RVA: 0x0008F898 File Offset: 0x0008DA98
		[DebugAction("Pawns", "Kidnap colonist", actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap)]
		private static void Kidnap(Pawn p)
		{
			if (p.IsColonist)
			{
				Faction faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
				if (faction != null)
				{
					faction.kidnapped.Kidnap(p, faction.leader);
				}
			}
		}
	}
}
