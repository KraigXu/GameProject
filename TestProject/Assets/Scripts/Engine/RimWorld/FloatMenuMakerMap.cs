using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	
	public static class FloatMenuMakerMap
	{
		
		private static bool CanTakeOrder(Pawn pawn)
		{
			return pawn.IsColonistPlayerControlled;
		}

		
		public static void TryMakeFloatMenu(Pawn pawn)
		{
			if (!FloatMenuMakerMap.CanTakeOrder(pawn))
			{
				return;
			}
			if (pawn.Downed)
			{
				Messages.Message("IsIncapped".Translate(pawn.LabelCap, pawn), pawn, MessageTypeDefOf.RejectInput, false);
				return;
			}
			if (pawn.Map != Find.CurrentMap)
			{
				return;
			}
			List<FloatMenuOption> list = FloatMenuMakerMap.ChoicesAtFor(UI.MouseMapPosition(), pawn);
			if (list.Count == 0)
			{
				return;
			}
			bool flag = true;
			FloatMenuOption floatMenuOption = null;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Disabled || !list[i].autoTakeable)
				{
					flag = false;
					break;
				}
				if (floatMenuOption == null || list[i].autoTakeablePriority > floatMenuOption.autoTakeablePriority)
				{
					floatMenuOption = list[i];
				}
			}
			if (flag && floatMenuOption != null)
			{
				floatMenuOption.Chosen(true, null);
				return;
			}
			FloatMenuMap floatMenuMap = new FloatMenuMap(list, pawn.LabelCap, UI.MouseMapPosition());
			floatMenuMap.givesColonistOrders = true;
			Find.WindowStack.Add(floatMenuMap);
		}

		
		public static List<FloatMenuOption> ChoicesAtFor(Vector3 clickPos, Pawn pawn)
		{
			IntVec3 intVec = IntVec3.FromVector3(clickPos);
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			if (!intVec.InBounds(pawn.Map) || !FloatMenuMakerMap.CanTakeOrder(pawn))
			{
				return list;
			}
			if (pawn.Map != Find.CurrentMap)
			{
				return list;
			}
			FloatMenuMakerMap.makingFor = pawn;
			try
			{
				if (intVec.Fogged(pawn.Map))
				{
					if (pawn.Drafted)
					{
						FloatMenuOption floatMenuOption = FloatMenuMakerMap.GotoLocationOption(intVec, pawn);
						if (floatMenuOption != null && !floatMenuOption.Disabled)
						{
							list.Add(floatMenuOption);
						}
					}
				}
				else
				{
					if (pawn.Drafted)
					{
						FloatMenuMakerMap.AddDraftedOrders(clickPos, pawn, list);
					}
					if (pawn.RaceProps.Humanlike)
					{
						FloatMenuMakerMap.AddHumanlikeOrders(clickPos, pawn, list);
					}
					if (!pawn.Drafted)
					{
						FloatMenuMakerMap.AddUndraftedOrders(clickPos, pawn, list);
					}
					foreach (FloatMenuOption item in pawn.GetExtraFloatMenuOptionsFor(intVec))
					{
						list.Add(item);
					}
				}
			}
			finally
			{
				FloatMenuMakerMap.makingFor = null;
			}
			return list;
		}

		
		private static void AddDraftedOrders(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts)
		{
			//IntVec3 clickCell = IntVec3.FromVector3(clickPos);
			//foreach (LocalTargetInfo attackTarg in GenUI.TargetsAt(clickPos, TargetingParameters.ForAttackHostile(), true))
			//{
			//	FloatMenuMakerMap.c__DisplayClass4_0 c__DisplayClass4_ = new FloatMenuMakerMap.c__DisplayClass4_0();
			//	c__DisplayClass4_.attackTarg = attackTarg;
			//	if (pawn.equipment.Primary != null && !pawn.equipment.PrimaryEq.PrimaryVerb.verbProps.IsMeleeAttack)
			//	{
			//		string str;
			//		Action rangedAct = FloatMenuUtility.GetRangedAttackAction(pawn, c__DisplayClass4_.attackTarg, out str);
			//		string text = "FireAt".Translate(c__DisplayClass4_.attackTarg.Thing.Label, c__DisplayClass4_.attackTarg.Thing);
			//		FloatMenuOption floatMenuOption = new FloatMenuOption("", null, MenuOptionPriority.High, null, attackTarg.Thing, 0f, null, null);
			//		if (rangedAct == null)
			//		{
			//			text = text + ": " + str;
			//		}
			//		else
			//		{
			//			floatMenuOption.autoTakeable = (!c__DisplayClass4_.attackTarg.HasThing || c__DisplayClass4_.attackTarg.Thing.HostileTo(Faction.OfPlayer));
			//			floatMenuOption.autoTakeablePriority = 40f;
			//			floatMenuOption.action = delegate
			//			{
			//				MoteMaker.MakeStaticMote(c__DisplayClass4_.attackTarg.Thing.DrawPos, c__DisplayClass4_.attackTarg.Thing.Map, ThingDefOf.Mote_FeedbackShoot, 1f);
			//				rangedAct();
			//			};
			//		}
			//		floatMenuOption.Label = text;
			//		opts.Add(floatMenuOption);
			//	}
			//	string str2;
			//	Action meleeAct = FloatMenuUtility.GetMeleeAttackAction(pawn, c__DisplayClass4_.attackTarg, out str2);
			//	Pawn pawn2 = c__DisplayClass4_.attackTarg.Thing as Pawn;
			//	string text2;
			//	if (pawn2 != null && pawn2.Downed)
			//	{
			//		text2 = "MeleeAttackToDeath".Translate(c__DisplayClass4_.attackTarg.Thing.Label, c__DisplayClass4_.attackTarg.Thing);
			//	}
			//	else
			//	{
			//		text2 = "MeleeAttack".Translate(c__DisplayClass4_.attackTarg.Thing.Label, c__DisplayClass4_.attackTarg.Thing);
			//	}
			//	MenuOptionPriority priority = (c__DisplayClass4_.attackTarg.HasThing && pawn.HostileTo(c__DisplayClass4_.attackTarg.Thing)) ? MenuOptionPriority.AttackEnemy : MenuOptionPriority.VeryLow;
			//	FloatMenuOption floatMenuOption2 = new FloatMenuOption("", null, priority, null, c__DisplayClass4_.attackTarg.Thing, 0f, null, null);
			//	if (meleeAct == null)
			//	{
			//		text2 = text2 + ": " + str2.CapitalizeFirst();
			//	}
			//	else
			//	{
			//		floatMenuOption2.autoTakeable = (!c__DisplayClass4_.attackTarg.HasThing || c__DisplayClass4_.attackTarg.Thing.HostileTo(Faction.OfPlayer));
			//		floatMenuOption2.autoTakeablePriority = 30f;
			//		floatMenuOption2.action = delegate
			//		{
			//			MoteMaker.MakeStaticMote(c__DisplayClass4_.attackTarg.Thing.DrawPos, c__DisplayClass4_.attackTarg.Thing.Map, ThingDefOf.Mote_FeedbackMelee, 1f);
			//			meleeAct();
			//		};
			//	}
			//	floatMenuOption2.Label = text2;
			//	opts.Add(floatMenuOption2);
			//}
			//FloatMenuMakerMap.AddJobGiverWorkOrders(clickCell, pawn, opts, true);
			//FloatMenuOption floatMenuOption3 = FloatMenuMakerMap.GotoLocationOption(clickCell, pawn);
			//if (floatMenuOption3 != null)
			//{
			//	opts.Add(floatMenuOption3);
			//}
		}

		private static void AddHumanlikeOrders(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts)
		{
		}

		//	
		//	private static void AddHumanlikeOrders(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts)
		//{
		//	FloatMenuMakerMap.c__DisplayClass5_0 c__DisplayClass5_ = new FloatMenuMakerMap.c__DisplayClass5_0();
		//	c__DisplayClass5_.pawn = pawn;
		//	IntVec3 c = IntVec3.FromVector3(clickPos);
		//	if (c__DisplayClass5_.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
		//	{
		//		foreach (LocalTargetInfo dest in GenUI.TargetsAt(clickPos, TargetingParameters.ForArrest(c__DisplayClass5_.pawn), true))
		//		{
		//			bool flag = dest.HasThing && dest.Thing is Pawn && ((Pawn)dest.Thing).IsWildMan();
		//			if (c__DisplayClass5_.pawn.Drafted || flag)
		//			{
		//				if (!c__DisplayClass5_.pawn.CanReach(dest, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
		//				{
		//					opts.Add(new FloatMenuOption("CannotArrest".Translate() + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
		//				}
		//				else
		//				{
		//					Pawn pTarg = (Pawn)dest.Thing;
		//					Action action = delegate
		//					{
		//						Building_Bed building_Bed = RestUtility.FindBedFor(pTarg, c__DisplayClass5_.pawn, true, false, false);
		//						if (building_Bed == null)
		//						{
		//							building_Bed = RestUtility.FindBedFor(pTarg, c__DisplayClass5_.pawn, true, false, true);
		//						}
		//						if (building_Bed == null)
		//						{
		//							Messages.Message("CannotArrest".Translate() + ": " + "NoPrisonerBed".Translate(), pTarg, MessageTypeDefOf.RejectInput, false);
		//							return;
		//						}
		//						Job job = JobMaker.MakeJob(JobDefOf.Arrest, pTarg, building_Bed);
		//						job.count = 1;
		//						c__DisplayClass5_.pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		//						if (pTarg.Faction != null && ((pTarg.Faction != Faction.OfPlayer && !pTarg.Faction.def.hidden) || pTarg.IsQuestLodger()))
		//						{
		//							TutorUtility.DoModalDialogIfNotKnown(ConceptDefOf.ArrestingCreatesEnemies, Array.Empty<string>());
		//						}
		//					};
		//					opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("TryToArrest".Translate(dest.Thing.LabelCap, dest.Thing, pTarg.GetAcceptArrestChance(c__DisplayClass5_.pawn).ToStringPercent()), action, MenuOptionPriority.High, null, dest.Thing, 0f, null, null), c__DisplayClass5_.pawn, pTarg, "ReservedBy"));
		//				}
		//			}
		//		}
		//	}
		//	foreach (Thing t4 in c.GetThingList(c__DisplayClass5_.pawn.Map))
		//	{
		//		Thing t = t4;
		//		if (t.def.ingestible != null && c__DisplayClass5_.pawn.RaceProps.CanEverEat(t) && t.IngestibleNow)
		//		{
		//			string text;
		//			if (t.def.ingestible.ingestCommandString.NullOrEmpty())
		//			{
		//				text = "ConsumeThing".Translate(t.LabelShort, t);
		//			}
		//			else
		//			{
		//				text = string.Format(t.def.ingestible.ingestCommandString, t.LabelShort);
		//			}
		//			if (!t.IsSociallyProper(c__DisplayClass5_.pawn))
		//			{
		//				text = text + ": " + "ReservedForPrisoners".Translate().CapitalizeFirst();
		//			}
		//			FloatMenuOption floatMenuOption;
		//			if (t.def.IsNonMedicalDrug && c__DisplayClass5_.pawn.IsTeetotaler())
		//			{
		//				floatMenuOption = new FloatMenuOption(text + ": " + TraitDefOf.DrugDesire.DataAtDegree(-1).LabelCap, null, MenuOptionPriority.Default, null, null, 0f, null, null);
		//			}
		//			else if (FoodUtility.InappropriateForTitle(t.def, c__DisplayClass5_.pawn, true))
		//			{
		//				floatMenuOption = new FloatMenuOption(text + ": " + "FoodBelowTitleRequirements".Translate(c__DisplayClass5_.pawn.royalty.MostSeniorTitle.def.GetLabelFor(c__DisplayClass5_.pawn)), null, MenuOptionPriority.Default, null, null, 0f, null, null);
		//			}
		//			else if (!c__DisplayClass5_.pawn.CanReach(t, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
		//			{
		//				floatMenuOption = new FloatMenuOption(text + ": " + "NoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
		//			}
		//			else
		//			{
		//				MenuOptionPriority priority = (t is Corpse) ? MenuOptionPriority.Low : MenuOptionPriority.Default;
		//				bool maxAmountToPickup = FoodUtility.GetMaxAmountToPickup(t, c__DisplayClass5_.pawn, FoodUtility.WillIngestStackCountOf(c__DisplayClass5_.pawn, t.def, t.GetStatValue(StatDefOf.Nutrition, true))) != 0;
		//				floatMenuOption = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, delegate
		//				{
		//					int maxAmountToPickup2 = FoodUtility.GetMaxAmountToPickup(t, c__DisplayClass5_.pawn, FoodUtility.WillIngestStackCountOf(c__DisplayClass5_.pawn, t.def, t.GetStatValue(StatDefOf.Nutrition, true)));
		//					if (maxAmountToPickup2 == 0)
		//					{
		//						return;
		//					}
		//					t.SetForbidden(false, true);
		//					Job job = JobMaker.MakeJob(JobDefOf.Ingest, t);
		//					job.count = maxAmountToPickup2;
		//					c__DisplayClass5_.pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		//				}, priority, null, null, 0f, null, null), c__DisplayClass5_.pawn, t, "ReservedBy");
		//				if (!maxAmountToPickup)
		//				{
		//					floatMenuOption.action = null;
		//				}
		//			}
		//			opts.Add(floatMenuOption);
		//		}
		//	}
		//	foreach (LocalTargetInfo dest2 in GenUI.TargetsAt(clickPos, TargetingParameters.ForQuestPawnsWhoWillJoinColony(c__DisplayClass5_.pawn), true))
		//	{
		//		Pawn toHelpPawn = (Pawn)dest2.Thing;
		//		FloatMenuOption item6;
		//		if (!c__DisplayClass5_.pawn.CanReach(dest2, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
		//		{
		//			item6 = new FloatMenuOption("CannotGoNoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
		//		}
		//		else
		//		{
		//			item6 = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(toHelpPawn.IsPrisoner ? "FreePrisoner".Translate() : "OfferHelp".Translate(), delegate
		//			{
		//				c__DisplayClass5_.pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.OfferHelp, toHelpPawn), JobTag.Misc);
		//			}, MenuOptionPriority.RescueOrCapture, null, toHelpPawn, 0f, null, null), c__DisplayClass5_.pawn, toHelpPawn, "ReservedBy");
		//		}
		//		opts.Add(item6);
		//	}
		//	if (c__DisplayClass5_.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
		//	{
		//		foreach (Thing thing3 in c.GetThingList(c__DisplayClass5_.pawn.Map))
		//		{
		//			Corpse corpse = thing3 as Corpse;
		//			if (corpse != null && corpse.IsInValidStorage())
		//			{
		//				StoragePriority priority2 = StoreUtility.CurrentHaulDestinationOf(corpse).GetStoreSettings().Priority;
		//				IHaulDestination haulDestination;
		//				if (StoreUtility.TryFindBestBetterNonSlotGroupStorageFor(corpse, c__DisplayClass5_.pawn, c__DisplayClass5_.pawn.Map, priority2, Faction.OfPlayer, out haulDestination, true) && haulDestination.GetStoreSettings().Priority == priority2 && haulDestination is Building_Grave)
		//				{
		//					Building_Grave grave = haulDestination as Building_Grave;
		//					string label = "PrioritizeGeneric".Translate("Burying".Translate(), corpse.Label);
		//					opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, delegate
		//					{
		//						c__DisplayClass5_.pawn.jobs.TryTakeOrderedJob(HaulAIUtility.HaulToContainerJob(c__DisplayClass5_.pawn, corpse, grave), JobTag.Misc);
		//					}, MenuOptionPriority.Default, null, null, 0f, null, null), c__DisplayClass5_.pawn, new LocalTargetInfo(corpse), "ReservedBy"));
		//				}
		//			}
		//		}
		//		foreach (LocalTargetInfo localTargetInfo in GenUI.TargetsAt(clickPos, TargetingParameters.ForRescue(c__DisplayClass5_.pawn), true))
		//		{
		//			Pawn victim = (Pawn)localTargetInfo.Thing;
		//			if (!victim.InBed() && c__DisplayClass5_.pawn.CanReserveAndReach(victim, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, true) && !victim.mindState.WillJoinColonyIfRescued)
		//			{
		//				if (!victim.IsPrisonerOfColony && (!victim.InMentalState || victim.health.hediffSet.HasHediff(HediffDefOf.Scaria, false)) && (victim.Faction == Faction.OfPlayer || victim.Faction == null || !victim.Faction.HostileTo(Faction.OfPlayer)))
		//				{
		//					opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Rescue".Translate(victim.LabelCap, victim), delegate
		//					{
		//						Building_Bed building_Bed = RestUtility.FindBedFor(victim, c__DisplayClass5_.pawn, false, false, false);
		//						if (building_Bed == null)
		//						{
		//							building_Bed = RestUtility.FindBedFor(victim, c__DisplayClass5_.pawn, false, false, true);
		//						}
		//						if (building_Bed == null)
		//						{
		//							string t5;
		//							if (victim.RaceProps.Animal)
		//							{
		//								t5 = "NoAnimalBed".Translate();
		//							}
		//							else
		//							{
		//								t5 = "NoNonPrisonerBed".Translate();
		//							}
		//							Messages.Message("CannotRescue".Translate() + ": " + t5, victim, MessageTypeDefOf.RejectInput, false);
		//							return;
		//						}
		//						Job job = JobMaker.MakeJob(JobDefOf.Rescue, victim, building_Bed);
		//						job.count = 1;
		//						c__DisplayClass5_.pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		//						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Rescuing, KnowledgeAmount.Total);
		//					}, MenuOptionPriority.RescueOrCapture, null, victim, 0f, null, null), c__DisplayClass5_.pawn, victim, "ReservedBy"));
		//				}
		//				if (victim.RaceProps.Humanlike && (victim.InMentalState || victim.Faction != Faction.OfPlayer || (victim.Downed && (victim.guilt.IsGuilty || victim.IsPrisonerOfColony))))
		//				{
		//					TaggedString taggedString = "Capture".Translate(victim.LabelCap, victim);
		//					if (victim.Faction != null && victim.Faction != Faction.OfPlayer && !victim.Faction.def.hidden && !victim.Faction.HostileTo(Faction.OfPlayer) && !victim.IsPrisonerOfColony)
		//					{
		//						taggedString += ": " + "AngersFaction".Translate().CapitalizeFirst();
		//					}
		//					opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString, delegate
		//					{
		//						Building_Bed building_Bed = RestUtility.FindBedFor(victim, c__DisplayClass5_.pawn, true, false, false);
		//						if (building_Bed == null)
		//						{
		//							building_Bed = RestUtility.FindBedFor(victim, c__DisplayClass5_.pawn, true, false, true);
		//						}
		//						if (building_Bed == null)
		//						{
		//							Messages.Message("CannotCapture".Translate() + ": " + "NoPrisonerBed".Translate(), victim, MessageTypeDefOf.RejectInput, false);
		//							return;
		//						}
		//						Job job = JobMaker.MakeJob(JobDefOf.Capture, victim, building_Bed);
		//						job.count = 1;
		//						c__DisplayClass5_.pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		//						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Capturing, KnowledgeAmount.Total);
		//						if (victim.Faction != null && victim.Faction != Faction.OfPlayer && !victim.Faction.def.hidden && !victim.Faction.HostileTo(Faction.OfPlayer) && !victim.IsPrisonerOfColony)
		//						{
		//							Messages.Message("MessageCapturingWillAngerFaction".Translate(victim.Named("PAWN")).AdjustedFor(victim, "PAWN", true), victim, MessageTypeDefOf.CautionInput, false);
		//						}
		//					}, MenuOptionPriority.RescueOrCapture, null, victim, 0f, null, null), c__DisplayClass5_.pawn, victim, "ReservedBy"));
		//				}
		//			}
		//		}
		//		foreach (LocalTargetInfo localTargetInfo2 in GenUI.TargetsAt(clickPos, TargetingParameters.ForRescue(c__DisplayClass5_.pawn), true))
		//		{
		//			LocalTargetInfo localTargetInfo3 = localTargetInfo2;
		//			Pawn victim = (Pawn)localTargetInfo3.Thing;
		//			if (victim.Downed && c__DisplayClass5_.pawn.CanReserveAndReach(victim, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, true) && Building_CryptosleepCasket.FindCryptosleepCasketFor(victim, c__DisplayClass5_.pawn, true) != null)
		//			{
		//				string text2 = "CarryToCryptosleepCasket".Translate(localTargetInfo3.Thing.LabelCap, localTargetInfo3.Thing);
		//				JobDef jDef = JobDefOf.CarryToCryptosleepCasket;
		//				Action action2 = delegate
		//				{
		//					Building_CryptosleepCasket building_CryptosleepCasket = Building_CryptosleepCasket.FindCryptosleepCasketFor(victim, c__DisplayClass5_.pawn, false);
		//					if (building_CryptosleepCasket == null)
		//					{
		//						building_CryptosleepCasket = Building_CryptosleepCasket.FindCryptosleepCasketFor(victim, c__DisplayClass5_.pawn, true);
		//					}
		//					if (building_CryptosleepCasket == null)
		//					{
		//						Messages.Message("CannotCarryToCryptosleepCasket".Translate() + ": " + "NoCryptosleepCasket".Translate(), victim, MessageTypeDefOf.RejectInput, false);
		//						return;
		//					}
		//					Job job = JobMaker.MakeJob(jDef, victim, building_CryptosleepCasket);
		//					job.count = 1;
		//					c__DisplayClass5_.pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		//				};
		//				if (victim.IsQuestLodger())
		//				{
		//					text2 += " (" + "CryptosleepCasketGuestsNotAllowed".Translate() + ")";
		//					opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text2, null, MenuOptionPriority.Default, null, victim, 0f, null, null), c__DisplayClass5_.pawn, victim, "ReservedBy"));
		//				}
		//				else if (victim.GetExtraHostFaction(null) != null)
		//				{
		//					text2 += " (" + "CryptosleepCasketGuestPrisonersNotAllowed".Translate() + ")";
		//					opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text2, null, MenuOptionPriority.Default, null, victim, 0f, null, null), c__DisplayClass5_.pawn, victim, "ReservedBy"));
		//				}
		//				else
		//				{
		//					opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text2, action2, MenuOptionPriority.Default, null, victim, 0f, null, null), c__DisplayClass5_.pawn, victim, "ReservedBy"));
		//				}
		//			}
		//		}
		//		if (ModsConfig.RoyaltyActive)
		//		{
		//			foreach (LocalTargetInfo localTargetInfo4 in GenUI.TargetsAt(clickPos, TargetingParameters.ForShuttle(c__DisplayClass5_.pawn), true))
		//			{
		//				LocalTargetInfo localTargetInfo5 = localTargetInfo4;
		//				Pawn victim = (Pawn)localTargetInfo5.Thing;
		//				Predicate<Thing> validator = delegate(Thing thing)
		//				{
		//					CompShuttle compShuttle = thing.TryGetComp<CompShuttle>();
		//					return compShuttle != null && compShuttle.IsAllowed(victim);
		//				};
		//				Thing shuttleThing = GenClosest.ClosestThingReachable(victim.Position, victim.Map, ThingRequest.ForDef(ThingDefOf.Shuttle), PathEndMode.ClosestTouch, TraverseParms.For(c__DisplayClass5_.pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
		//				if (shuttleThing != null && c__DisplayClass5_.pawn.CanReserveAndReach(victim, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, true) && !c__DisplayClass5_.pawn.WorkTypeIsDisabled(WorkTypeDefOf.Hauling))
		//				{
		//					string label2 = "CarryToShuttle".Translate(localTargetInfo5.Thing);
		//					Action action3 = delegate
		//					{
		//						CompShuttle compShuttle = shuttleThing.TryGetComp<CompShuttle>();
		//						if (!compShuttle.LoadingInProgressOrReadyToLaunch)
		//						{
		//							TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(compShuttle.Transporter));
		//						}
		//						Job job = JobMaker.MakeJob(JobDefOf.HaulToTransporter, victim, shuttleThing);
		//						job.ignoreForbidden = true;
		//						job.count = 1;
		//						c__DisplayClass5_.pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		//					};
		//					opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label2, action3, MenuOptionPriority.Default, null, null, 0f, null, null), c__DisplayClass5_.pawn, victim, "ReservedBy"));
		//				}
		//			}
		//		}
		//	}
		//	foreach (LocalTargetInfo stripTarg2 in GenUI.TargetsAt(clickPos, TargetingParameters.ForStrip(c__DisplayClass5_.pawn), true))
		//	{
		//		LocalTargetInfo stripTarg = stripTarg2;
		//		FloatMenuOption item2;
		//		if (!c__DisplayClass5_.pawn.CanReach(stripTarg, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
		//		{
		//			item2 = new FloatMenuOption("CannotStrip".Translate(stripTarg.Thing.LabelCap, stripTarg.Thing) + ": " + "NoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
		//		}
		//		else if (stripTarg.Pawn != null && stripTarg.Pawn.HasExtraHomeFaction(null))
		//		{
		//			item2 = new FloatMenuOption("CannotStrip".Translate(stripTarg.Thing.LabelCap, stripTarg.Thing) + ": " + "QuestRelated".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
		//		}
		//		else
		//		{
		//			item2 = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Strip".Translate(stripTarg.Thing.LabelCap, stripTarg.Thing), delegate
		//			{
		//				stripTarg.Thing.SetForbidden(false, false);
		//				c__DisplayClass5_.pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Strip, stripTarg), JobTag.Misc);
		//				StrippableUtility.CheckSendStrippingImpactsGoodwillMessage(stripTarg.Thing);
		//			}, MenuOptionPriority.Default, null, null, 0f, null, null), c__DisplayClass5_.pawn, stripTarg, "ReservedBy");
		//		}
		//		opts.Add(item2);
		//	}
		//	if (c__DisplayClass5_.pawn.equipment != null)
		//	{
		//		FloatMenuMakerMap.c__DisplayClass5_11 c__DisplayClass5_12 = new FloatMenuMakerMap.c__DisplayClass5_11();
		//		c__DisplayClass5_12.CS$8__locals11 = c__DisplayClass5_;
		//		c__DisplayClass5_12.equipment = null;
		//		List<Thing> thingList = c.GetThingList(c__DisplayClass5_12.CS$8__locals11.pawn.Map);
		//		for (int i = 0; i < thingList.Count; i++)
		//		{
		//			if (thingList[i].TryGetComp<CompEquippable>() != null)
		//			{
		//				c__DisplayClass5_12.equipment = (ThingWithComps)thingList[i];
		//				break;
		//			}
		//		}
		//		if (c__DisplayClass5_12.equipment != null)
		//		{
		//			string labelShort = c__DisplayClass5_12.equipment.LabelShort;
		//			FloatMenuOption item3;
		//			string str;
		//			if (c__DisplayClass5_12.equipment.def.IsWeapon && c__DisplayClass5_12.CS$8__locals11.pawn.WorkTagIsDisabled(WorkTags.Violent))
		//			{
		//				item3 = new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "IsIncapableOfViolenceLower".Translate(c__DisplayClass5_12.CS$8__locals11.pawn.LabelShort, c__DisplayClass5_12.CS$8__locals11.pawn), null, MenuOptionPriority.Default, null, null, 0f, null, null);
		//			}
		//			else if (!c__DisplayClass5_12.CS$8__locals11.pawn.CanReach(c__DisplayClass5_12.equipment, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
		//			{
		//				item3 = new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "NoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
		//			}
		//			else if (!c__DisplayClass5_12.CS$8__locals11.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
		//			{
		//				item3 = new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "Incapable".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
		//			}
		//			else if (c__DisplayClass5_12.equipment.IsBurning())
		//			{
		//				item3 = new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "BurningLower".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
		//			}
		//			else if (c__DisplayClass5_12.CS$8__locals11.pawn.IsQuestLodger() && !EquipmentUtility.QuestLodgerCanEquip(c__DisplayClass5_12.equipment, c__DisplayClass5_12.CS$8__locals11.pawn))
		//			{
		//				item3 = new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + "QuestRelated".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
		//			}
		//			else if (!EquipmentUtility.CanEquip(c__DisplayClass5_12.equipment, c__DisplayClass5_12.CS$8__locals11.pawn, out str))
		//			{
		//				item3 = new FloatMenuOption("CannotEquip".Translate(labelShort) + ": " + str.CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
		//			}
		//			else
		//			{
		//				string text3 = "Equip".Translate(labelShort);
		//				if (c__DisplayClass5_12.equipment.def.IsRangedWeapon && c__DisplayClass5_12.CS$8__locals11.pawn.story != null && c__DisplayClass5_12.CS$8__locals11.pawn.story.traits.HasTrait(TraitDefOf.Brawler))
		//				{
		//					text3 += " " + "EquipWarningBrawler".Translate();
		//				}
		//				item3 = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text3, delegate
		//				{
		//					string text4 = "";
		//					CompBladelinkWeapon compBladelinkWeapon = c__DisplayClass5_12.equipment.TryGetComp<CompBladelinkWeapon>();
		//					if (compBladelinkWeapon != null && compBladelinkWeapon.bondedPawn != c__DisplayClass5_12.CS$8__locals11.pawn)
		//					{
		//						if (!text4.NullOrEmpty())
		//						{
		//							text4 += "\n\n";
		//						}
		//						text4 += "BladelinkEquipWarning".Translate();
		//					}
		//					if (!text4.NullOrEmpty())
		//					{
		//						text4 += "\n\n" + "RoyalWeaponEquipConfirmation".Translate();
		//						WindowStack windowStack = Find.WindowStack;
		//						TaggedString text5 = text4;
		//						string buttonAText = "Yes".Translate();
		//						Action buttonAAction;
		//						if ((buttonAAction = c__DisplayClass5_12.9__12) == null)
		//						{
		//							buttonAAction = (c__DisplayClass5_12.9__12 = delegate
		//							{
		//								c__DisplayClass5_12.<AddHumanlikeOrders>g__Equip|11();
		//							});
		//						}
		//						windowStack.Add(new Dialog_MessageBox(text5, buttonAText, buttonAAction, "No".Translate(), null, null, false, null, null));
		//						return;
		//					}
		//					c__DisplayClass5_12.<AddHumanlikeOrders>g__Equip|11();
		//				}, MenuOptionPriority.High, null, null, 0f, null, null), c__DisplayClass5_12.CS$8__locals11.pawn, c__DisplayClass5_12.equipment, "ReservedBy");
		//			}
		//			opts.Add(item3);
		//		}
		//	}
		//	if (c__DisplayClass5_.pawn.apparel != null)
		//	{
		//		Apparel apparel = c__DisplayClass5_.pawn.Map.thingGrid.ThingAt<Apparel>(c);
		//		if (apparel != null)
		//		{
		//			FloatMenuOption item4;
		//			string t2;
		//			if (!c__DisplayClass5_.pawn.CanReach(apparel, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
		//			{
		//				item4 = new FloatMenuOption("CannotWear".Translate(apparel.Label, apparel) + ": " + "NoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
		//			}
		//			else if (apparel.IsBurning())
		//			{
		//				item4 = new FloatMenuOption("CannotWear".Translate(apparel.Label, apparel) + ": " + "Burning".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
		//			}
		//			else if (c__DisplayClass5_.pawn.apparel.WouldReplaceLockedApparel(apparel))
		//			{
		//				item4 = new FloatMenuOption("CannotWear".Translate(apparel.Label, apparel) + ": " + "WouldReplaceLockedApparel".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
		//			}
		//			else if (!ApparelUtility.HasPartsToWear(c__DisplayClass5_.pawn, apparel.def))
		//			{
		//				item4 = new FloatMenuOption("CannotWear".Translate(apparel.Label, apparel) + ": " + "CannotWearBecauseOfMissingBodyParts".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
		//			}
		//			else if (!EquipmentUtility.CanEquip(apparel, c__DisplayClass5_.pawn, out t2))
		//			{
		//				item4 = new FloatMenuOption("CannotWear".Translate(apparel.Label, apparel) + ": " + t2, null, MenuOptionPriority.Default, null, null, 0f, null, null);
		//			}
		//			else
		//			{
		//				item4 = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("ForceWear".Translate(apparel.LabelShort, apparel), delegate
		//				{
		//					apparel.SetForbidden(false, true);
		//					Job job = JobMaker.MakeJob(JobDefOf.Wear, apparel);
		//					c__DisplayClass5_.pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		//				}, MenuOptionPriority.High, null, null, 0f, null, null), c__DisplayClass5_.pawn, apparel, "ReservedBy");
		//			}
		//			opts.Add(item4);
		//		}
		//	}
		//	if (c__DisplayClass5_.pawn.IsFormingCaravan())
		//	{
		//		Thing item = c.GetFirstItem(c__DisplayClass5_.pawn.Map);
		//		if (item != null && item.def.EverHaulable)
		//		{
		//			Pawn packTarget = GiveToPackAnimalUtility.UsablePackAnimalWithTheMostFreeSpace(c__DisplayClass5_.pawn) ?? c__DisplayClass5_.pawn;
		//			JobDef jobDef = (packTarget == c__DisplayClass5_.pawn) ? JobDefOf.TakeInventory : JobDefOf.GiveToPackAnimal;
		//			if (!c__DisplayClass5_.pawn.CanReach(item, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
		//			{
		//				opts.Add(new FloatMenuOption("CannotLoadIntoCaravan".Translate(item.Label, item) + ": " + "NoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
		//			}
		//			else if (MassUtility.WillBeOverEncumberedAfterPickingUp(packTarget, item, 1))
		//			{
		//				opts.Add(new FloatMenuOption("CannotLoadIntoCaravan".Translate(item.Label, item) + ": " + "TooHeavy".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
		//			}
		//			else
		//			{
		//				LordJob_FormAndSendCaravan lordJob = (LordJob_FormAndSendCaravan)c__DisplayClass5_.pawn.GetLord().LordJob;
		//				float capacityLeft = CaravanFormingUtility.CapacityLeft(lordJob);
		//				if (item.stackCount == 1)
		//				{
		//					float capacityLeft4 = capacityLeft - item.GetStatValue(StatDefOf.Mass, true);
		//					opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(CaravanFormingUtility.AppendOverweightInfo("LoadIntoCaravan".Translate(item.Label, item), capacityLeft4), delegate
		//					{
		//						item.SetForbidden(false, false);
		//						Job job = JobMaker.MakeJob(jobDef, item);
		//						job.count = 1;
		//						job.checkEncumbrance = (packTarget == c__DisplayClass5_.pawn);
		//						c__DisplayClass5_.pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		//					}, MenuOptionPriority.High, null, null, 0f, null, null), c__DisplayClass5_.pawn, item, "ReservedBy"));
		//				}
		//				else
		//				{
		//					if (MassUtility.WillBeOverEncumberedAfterPickingUp(packTarget, item, item.stackCount))
		//					{
		//						opts.Add(new FloatMenuOption("CannotLoadIntoCaravanAll".Translate(item.Label, item) + ": " + "TooHeavy".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
		//					}
		//					else
		//					{
		//						float capacityLeft2 = capacityLeft - (float)item.stackCount * item.GetStatValue(StatDefOf.Mass, true);
		//						opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(CaravanFormingUtility.AppendOverweightInfo("LoadIntoCaravanAll".Translate(item.Label, item), capacityLeft2), delegate
		//						{
		//							item.SetForbidden(false, false);
		//							Job job = JobMaker.MakeJob(jobDef, item);
		//							job.count = item.stackCount;
		//							job.checkEncumbrance = (packTarget == c__DisplayClass5_.pawn);
		//							c__DisplayClass5_.pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		//						}, MenuOptionPriority.High, null, null, 0f, null, null), c__DisplayClass5_.pawn, item, "ReservedBy"));
		//					}
		//					Action<int> 9__18;
		//					Func<int, string> 9__17;
		//					opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("LoadIntoCaravanSome".Translate(item.LabelNoCount, item), delegate
		//					{
		//						int num = Mathf.Min(MassUtility.CountToPickUpUntilOverEncumbered(packTarget, item), item.stackCount);
		//						Func<int, string> textGetter;
		//						if ((textGetter 7) == null)
		//						{
		//							textGetter = (9__17 = delegate(int val)
		//							{
		//								float capacityLeft3 = capacityLeft - (float)val * item.GetStatValue(StatDefOf.Mass, true);
		//								return CaravanFormingUtility.AppendOverweightInfo(string.Format("LoadIntoCaravanCount".Translate(item.LabelNoCount, item), val), capacityLeft3);
		//							});
		//						}
		//						int from = 1;
		//						int to = num;
		//						Action<int> confirmAction;
		//						if ((confirmAction 8) == null)
		//						{
		//							confirmAction = (9__18 = delegate(int count)
		//							{
		//								item.SetForbidden(false, false);
		//								Job job = JobMaker.MakeJob(jobDef, item);
		//								job.count = count;
		//								job.checkEncumbrance = (packTarget == c__DisplayClass5_.pawn);
		//								c__DisplayClass5_.pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		//							});
		//						}
		//						Dialog_Slider window = new Dialog_Slider(textGetter, from, to, confirmAction, int.MinValue);
		//						Find.WindowStack.Add(window);
		//					}, MenuOptionPriority.High, null, null, 0f, null, null), c__DisplayClass5_.pawn, item, "ReservedBy"));
		//				}
		//			}
		//		}
		//	}
		//	if (!c__DisplayClass5_.pawn.Map.IsPlayerHome && !c__DisplayClass5_.pawn.IsFormingCaravan())
		//	{
		//		Thing item = c.GetFirstItem(c__DisplayClass5_.pawn.Map);
		//		if (item != null && item.def.EverHaulable)
		//		{
		//			if (!c__DisplayClass5_.pawn.CanReach(item, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
		//			{
		//				opts.Add(new FloatMenuOption("CannotPickUp".Translate(item.Label, item) + ": " + "NoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
		//			}
		//			else if (MassUtility.WillBeOverEncumberedAfterPickingUp(c__DisplayClass5_.pawn, item, 1))
		//			{
		//				opts.Add(new FloatMenuOption("CannotPickUp".Translate(item.Label, item) + ": " + "TooHeavy".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
		//			}
		//			else if (item.stackCount == 1)
		//			{
		//				opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PickUp".Translate(item.Label, item), delegate
		//				{
		//					item.SetForbidden(false, false);
		//					Job job = JobMaker.MakeJob(JobDefOf.TakeInventory, item);
		//					job.count = 1;
		//					job.checkEncumbrance = true;
		//					c__DisplayClass5_.pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		//				}, MenuOptionPriority.High, null, null, 0f, null, null), c__DisplayClass5_.pawn, item, "ReservedBy"));
		//			}
		//			else
		//			{
		//				if (MassUtility.WillBeOverEncumberedAfterPickingUp(c__DisplayClass5_.pawn, item, item.stackCount))
		//				{
		//					opts.Add(new FloatMenuOption("CannotPickUpAll".Translate(item.Label, item) + ": " + "TooHeavy".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
		//				}
		//				else
		//				{
		//					opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PickUpAll".Translate(item.Label, item), delegate
		//					{
		//						item.SetForbidden(false, false);
		//						Job job = JobMaker.MakeJob(JobDefOf.TakeInventory, item);
		//						job.count = item.stackCount;
		//						job.checkEncumbrance = true;
		//						c__DisplayClass5_.pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		//					}, MenuOptionPriority.High, null, null, 0f, null, null), c__DisplayClass5_.pawn, item, "ReservedBy"));
		//				}
		//				Action<int> 9__22;
		//				opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PickUpSome".Translate(item.LabelNoCount, item), delegate
		//				{
		//					int num = Mathf.Min(MassUtility.CountToPickUpUntilOverEncumbered(c__DisplayClass5_.pawn, item), item.stackCount);
		//					string text4 = "PickUpCount".Translate(item.LabelNoCount, item);
		//					int from = 1;
		//					int to = num;
		//					Action<int> confirmAction;
		//					if ((confirmAction 2) == null)
		//					{
		//						confirmAction = (9__22 = delegate(int count)
		//						{
		//							item.SetForbidden(false, false);
		//							Job job = JobMaker.MakeJob(JobDefOf.TakeInventory, item);
		//							job.count = count;
		//							job.checkEncumbrance = true;
		//							c__DisplayClass5_.pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		//						});
		//					}
		//					Dialog_Slider window = new Dialog_Slider(text4, from, to, confirmAction, int.MinValue);
		//					Find.WindowStack.Add(window);
		//				}, MenuOptionPriority.High, null, null, 0f, null, null), c__DisplayClass5_.pawn, item, "ReservedBy"));
		//			}
		//		}
		//	}
		//	if (!c__DisplayClass5_.pawn.Map.IsPlayerHome && !c__DisplayClass5_.pawn.IsFormingCaravan())
		//	{
		//		Thing item = c.GetFirstItem(c__DisplayClass5_.pawn.Map);
		//		if (item != null && item.def.EverHaulable)
		//		{
		//			Pawn bestPackAnimal = GiveToPackAnimalUtility.UsablePackAnimalWithTheMostFreeSpace(c__DisplayClass5_.pawn);
		//			if (bestPackAnimal != null)
		//			{
		//				if (!c__DisplayClass5_.pawn.CanReach(item, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
		//				{
		//					opts.Add(new FloatMenuOption("CannotGiveToPackAnimal".Translate(item.Label, item) + ": " + "NoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
		//				}
		//				else if (MassUtility.WillBeOverEncumberedAfterPickingUp(bestPackAnimal, item, 1))
		//				{
		//					opts.Add(new FloatMenuOption("CannotGiveToPackAnimal".Translate(item.Label, item) + ": " + "TooHeavy".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
		//				}
		//				else if (item.stackCount == 1)
		//				{
		//					opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("GiveToPackAnimal".Translate(item.Label, item), delegate
		//					{
		//						item.SetForbidden(false, false);
		//						Job job = JobMaker.MakeJob(JobDefOf.GiveToPackAnimal, item);
		//						job.count = 1;
		//						c__DisplayClass5_.pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		//					}, MenuOptionPriority.High, null, null, 0f, null, null), c__DisplayClass5_.pawn, item, "ReservedBy"));
		//				}
		//				else
		//				{
		//					if (MassUtility.WillBeOverEncumberedAfterPickingUp(bestPackAnimal, item, item.stackCount))
		//					{
		//						opts.Add(new FloatMenuOption("CannotGiveToPackAnimalAll".Translate(item.Label, item) + ": " + "TooHeavy".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
		//					}
		//					else
		//					{
		//						opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("GiveToPackAnimalAll".Translate(item.Label, item), delegate
		//						{
		//							item.SetForbidden(false, false);
		//							Job job = JobMaker.MakeJob(JobDefOf.GiveToPackAnimal, item);
		//							job.count = item.stackCount;
		//							c__DisplayClass5_.pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		//						}, MenuOptionPriority.High, null, null, 0f, null, null), c__DisplayClass5_.pawn, item, "ReservedBy"));
		//					}
		//					Action<int> 9__26;
		//					opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("GiveToPackAnimalSome".Translate(item.LabelNoCount, item), delegate
		//					{
		//						int num = Mathf.Min(MassUtility.CountToPickUpUntilOverEncumbered(bestPackAnimal, item), item.stackCount);
		//						string text4 = "GiveToPackAnimalCount".Translate(item.LabelNoCount, item);
		//						int from = 1;
		//						int to = num;
		//						Action<int> confirmAction;
		//						if ((confirmAction 6) == null)
		//						{
		//							confirmAction = (9__26 = delegate(int count)
		//							{
		//								item.SetForbidden(false, false);
		//								Job job = JobMaker.MakeJob(JobDefOf.GiveToPackAnimal, item);
		//								job.count = count;
		//								c__DisplayClass5_.pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		//							});
		//						}
		//						Dialog_Slider window = new Dialog_Slider(text4, from, to, confirmAction, int.MinValue);
		//						Find.WindowStack.Add(window);
		//					}, MenuOptionPriority.High, null, null, 0f, null, null), c__DisplayClass5_.pawn, item, "ReservedBy"));
		//				}
		//			}
		//		}
		//	}
		//	if (!c__DisplayClass5_.pawn.Map.IsPlayerHome && c__DisplayClass5_.pawn.Map.exitMapGrid.MapUsesExitGrid)
		//	{
		//		foreach (LocalTargetInfo target in GenUI.TargetsAt(clickPos, TargetingParameters.ForRescue(c__DisplayClass5_.pawn), true))
		//		{
		//			Pawn p = (Pawn)target.Thing;
		//			if (p.Faction == Faction.OfPlayer || p.IsPrisonerOfColony || CaravanUtility.ShouldAutoCapture(p, Faction.OfPlayer))
		//			{
		//				if (!c__DisplayClass5_.pawn.CanReach(p, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
		//				{
		//					opts.Add(new FloatMenuOption("CannotCarryToExit".Translate(p.Label, p) + ": " + "NoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
		//				}
		//				else
		//				{
		//					IntVec3 exitSpot;
		//					if (!RCellFinder.TryFindBestExitSpot(c__DisplayClass5_.pawn, out exitSpot, TraverseMode.ByPawn))
		//					{
		//						opts.Add(new FloatMenuOption("CannotCarryToExit".Translate(p.Label, p) + ": " + "NoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
		//					}
		//					else
		//					{
		//						TaggedString taggedString2 = (p.Faction == Faction.OfPlayer || p.IsPrisonerOfColony) ? "CarryToExit".Translate(p.Label, p) : "CarryToExitAndCapture".Translate(p.Label, p);
		//						opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString2, delegate
		//						{
		//							Job job = JobMaker.MakeJob(JobDefOf.CarryDownedPawnToExit, p, exitSpot);
		//							job.count = 1;
		//							job.failIfCantJoinOrCreateCaravan = true;
		//							c__DisplayClass5_.pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		//						}, MenuOptionPriority.High, null, null, 0f, null, null), c__DisplayClass5_.pawn, target, "ReservedBy"));
		//					}
		//				}
		//			}
		//		}
		//	}
		//	if (c__DisplayClass5_.pawn.equipment != null && c__DisplayClass5_.pawn.equipment.Primary != null && GenUI.TargetsAt(clickPos, TargetingParameters.ForSelf(c__DisplayClass5_.pawn), true).Any<LocalTargetInfo>())
		//	{
		//		if (c__DisplayClass5_.pawn.IsQuestLodger())
		//		{
		//			opts.Add(new FloatMenuOption("CannotDrop".Translate(c__DisplayClass5_.pawn.equipment.Primary.Label, c__DisplayClass5_.pawn.equipment.Primary) + ": " + "QuestRelated".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
		//		}
		//		else
		//		{
		//			Action action4 = delegate
		//			{
		//				c__DisplayClass5_.pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.DropEquipment, c__DisplayClass5_.pawn.equipment.Primary), JobTag.Misc);
		//			};
		//			opts.Add(new FloatMenuOption("Drop".Translate(c__DisplayClass5_.pawn.equipment.Primary.Label, c__DisplayClass5_.pawn.equipment.Primary), action4, MenuOptionPriority.Default, null, c__DisplayClass5_.pawn, 0f, null, null));
		//		}
		//	}
		//	foreach (LocalTargetInfo dest3 in GenUI.TargetsAt(clickPos, TargetingParameters.ForTrade(), true))
		//	{
		//		if (!c__DisplayClass5_.pawn.CanReach(dest3, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
		//		{
		//			opts.Add(new FloatMenuOption("CannotTrade".Translate() + ": " + "NoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
		//		}
		//		else if (c__DisplayClass5_.pawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled)
		//		{
		//			opts.Add(new FloatMenuOption("CannotPrioritizeWorkTypeDisabled".Translate(SkillDefOf.Social.LabelCap), null, MenuOptionPriority.Default, null, null, 0f, null, null));
		//		}
		//		else if (!c__DisplayClass5_.pawn.CanTradeWith(((Pawn)dest3.Thing).Faction, ((Pawn)dest3.Thing).TraderKind))
		//		{
		//			opts.Add(new FloatMenuOption("CannotTradeMissingTitleAbility".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
		//		}
		//		else
		//		{
		//			Pawn pTarg = (Pawn)dest3.Thing;
		//			Action action5 = delegate
		//			{
		//				Job job = JobMaker.MakeJob(JobDefOf.TradeWithPawn, pTarg);
		//				job.playerForced = true;
		//				c__DisplayClass5_.pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		//				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.InteractingWithTraders, KnowledgeAmount.Total);
		//			};
		//			string t3 = "";
		//			if (pTarg.Faction != null)
		//			{
		//				t3 = " (" + pTarg.Faction.Name + ")";
		//			}
		//			opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("TradeWith".Translate(pTarg.LabelShort + ", " + pTarg.TraderKind.label) + t3, action5, MenuOptionPriority.InitiateSocial, null, dest3.Thing, 0f, null, null), c__DisplayClass5_.pawn, pTarg, "ReservedBy"));
		//		}
		//	}
		//	IEnumerator<LocalTargetInfo> enumerator = GenUI.TargetsAt(clickPos, TargetingParameters.ForOpen(c__DisplayClass5_.pawn), true).GetEnumerator();
		//	{
		//		while (enumerator.MoveNext())
		//		{
		//			LocalTargetInfo casket = enumerator.Current;
		//			if (!c__DisplayClass5_.pawn.CanReach(casket, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
		//			{
		//				opts.Add(new FloatMenuOption("CannotOpen".Translate(casket.Thing) + ": " + "NoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
		//			}
		//			else if (!c__DisplayClass5_.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
		//			{
		//				opts.Add(new FloatMenuOption("CannotOpen".Translate(casket.Thing) + ": " + "Incapable".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
		//			}
		//			else if (casket.Thing.Map.designationManager.DesignationOn(casket.Thing, DesignationDefOf.Open) == null)
		//			{
		//				opts.Add(FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Open".Translate(casket.Thing), delegate
		//				{
		//					Job job = JobMaker.MakeJob(JobDefOf.Open, casket.Thing);
		//					job.ignoreDesignations = true;
		//					c__DisplayClass5_.pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		//				}, MenuOptionPriority.High, null, null, 0f, null, null), c__DisplayClass5_.pawn, casket.Thing, "ReservedBy"));
		//			}
		//		}
		//	}
		//	foreach (Thing thing2 in c__DisplayClass5_.pawn.Map.thingGrid.ThingsAt(c))
		//	{
		//		foreach (FloatMenuOption item5 in thing2.GetFloatMenuOptions(c__DisplayClass5_.pawn))
		//		{
		//			opts.Add(item5);
		//		}
		//	}
		//}

		
		private static void AddUndraftedOrders(Vector3 clickPos, Pawn pawn, List<FloatMenuOption> opts)
		{
			if (FloatMenuMakerMap.equivalenceGroupTempStorage == null || FloatMenuMakerMap.equivalenceGroupTempStorage.Length != DefDatabase<WorkGiverEquivalenceGroupDef>.DefCount)
			{
				FloatMenuMakerMap.equivalenceGroupTempStorage = new FloatMenuOption[DefDatabase<WorkGiverEquivalenceGroupDef>.DefCount];
			}
			IntVec3 intVec = IntVec3.FromVector3(clickPos);
			bool flag = false;
			bool flag2 = false;
			foreach (Thing t in pawn.Map.thingGrid.ThingsAt(intVec))
			{
				flag2 = true;
				if (pawn.CanReach(t, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					flag = true;
					break;
				}
			}
			if (flag2 && !flag)
			{
				return;
			}
			FloatMenuMakerMap.AddJobGiverWorkOrders(intVec, pawn, opts, false);
		}

		
		private static void AddJobGiverWorkOrders(IntVec3 clickCell, Pawn pawn, List<FloatMenuOption> opts, bool drafted)
		{
			if (pawn.thinker.TryGetMainTreeThinkNode<JobGiver_Work>() == null)
			{
				return;
			}
			foreach (Thing thing in pawn.Map.thingGrid.ThingsAt(clickCell))
			{
				bool flag = false;
				foreach (WorkTypeDef workTypeDef in DefDatabase<WorkTypeDef>.AllDefsListForReading)
				{
					for (int i = 0; i < workTypeDef.workGiversByPriority.Count; i++)
					{
						WorkGiverDef workGiver = workTypeDef.workGiversByPriority[i];
						if (!drafted || workGiver.canBeDoneWhileDrafted)
						{
							WorkGiver_Scanner workGiver_Scanner = workGiver.Worker as WorkGiver_Scanner;
							if (workGiver_Scanner != null && workGiver_Scanner.def.directOrderable && !workGiver_Scanner.ShouldSkip(pawn, true))
							{
								JobFailReason.Clear();
								if (workGiver_Scanner.PotentialWorkThingRequest.Accepts(thing) || (workGiver_Scanner.PotentialWorkThingsGlobal(pawn) != null && workGiver_Scanner.PotentialWorkThingsGlobal(pawn).Contains(thing)))
								{
									Action action = null;
									PawnCapacityDef pawnCapacityDef = workGiver_Scanner.MissingRequiredCapacity(pawn);
									string text;
									if (pawnCapacityDef != null)
									{
										text = "CannotMissingHealthActivities".Translate(pawnCapacityDef.label);
									}
									else
									{
										Job job;
										if (!workGiver_Scanner.HasJobOnThing(pawn, thing, true))
										{
											job = null;
										}
										else
										{
											job = workGiver_Scanner.JobOnThing(pawn, thing, true);
										}
										if (job == null)
										{
											if (JobFailReason.HaveReason)
											{
												if (!JobFailReason.CustomJobString.NullOrEmpty())
												{
													text = "CannotGenericWorkCustom".Translate(JobFailReason.CustomJobString);
												}
												else
												{
													text = "CannotGenericWork".Translate(workGiver_Scanner.def.verb, thing.LabelShort, thing);
												}
												text = text + ": " + JobFailReason.Reason.CapitalizeFirst();
											}
											else
											{
												if (!thing.IsForbidden(pawn))
												{
													goto IL_6DF;
												}
												if (!thing.Position.InAllowedArea(pawn))
												{
													text = "CannotPrioritizeForbiddenOutsideAllowedArea".Translate() + ": " + pawn.playerSettings.EffectiveAreaRestriction.Label;
												}
												else
												{
													text = "CannotPrioritizeForbidden".Translate(thing.Label, thing);
												}
											}
										}
										else
										{
											WorkTypeDef workType = workGiver_Scanner.def.workType;
											if (pawn.WorkTagIsDisabled(workGiver_Scanner.def.workTags))
											{
												text = "CannotPrioritizeWorkGiverDisabled".Translate(workGiver_Scanner.def.label);
											}
											else if (pawn.jobs.curJob != null && pawn.jobs.curJob.JobIsSameAs(job))
											{
												text = "CannotGenericAlreadyAm".Translate(workGiver_Scanner.PostProcessedGerund(job), thing.LabelShort, thing);
											}
											else if (pawn.workSettings.GetPriority(workType) == 0)
											{
												if (pawn.WorkTypeIsDisabled(workType))
												{
													text = "CannotPrioritizeWorkTypeDisabled".Translate(workType.gerundLabel);
												}
												else if ("CannotPrioritizeNotAssignedToWorkType".CanTranslate())
												{
													text = "CannotPrioritizeNotAssignedToWorkType".Translate(workType.gerundLabel);
												}
												else
												{
													text = "CannotPrioritizeWorkTypeDisabled".Translate(workType.pawnLabel);
												}
											}
											else if (job.def == JobDefOf.Research && thing is Building_ResearchBench)
											{
												text = "CannotPrioritizeResearch".Translate();
											}
											else if (thing.IsForbidden(pawn))
											{
												if (!thing.Position.InAllowedArea(pawn))
												{
													text = "CannotPrioritizeForbiddenOutsideAllowedArea".Translate() + ": " + pawn.playerSettings.EffectiveAreaRestriction.Label;
												}
												else
												{
													text = "CannotPrioritizeForbidden".Translate(thing.Label, thing);
												}
											}
											else if (!pawn.CanReach(thing, workGiver_Scanner.PathEndMode, Danger.Deadly, false, TraverseMode.ByPawn))
											{
												text = (thing.Label + ": " + "NoPath".Translate()).CapitalizeFirst();
											}
											else
											{
												text = "PrioritizeGeneric".Translate(workGiver_Scanner.PostProcessedGerund(job), thing.Label);
												Job localJob = job;
												WorkGiver_Scanner localScanner = workGiver_Scanner;
												job.workGiverDef = workGiver_Scanner.def;
												action = delegate
												{
													if (pawn.jobs.TryTakeOrderedJobPrioritizedWork(localJob, localScanner, clickCell) && workGiver.forceMote != null)
													{
														MoteMaker.MakeStaticMote(clickCell, pawn.Map, workGiver.forceMote, 1f);
													}
												};
											}
										}
									}
									if (DebugViewSettings.showFloatMenuWorkGivers)
									{
										text += string.Format(" (from {0})", workGiver.defName);
									}
									FloatMenuOption menuOption = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, action, MenuOptionPriority.Default, null, null, 0f, null, null), pawn, thing, "ReservedBy");
									if (drafted && workGiver.autoTakeablePriorityDrafted != -1)
									{
										menuOption.autoTakeable = true;
										menuOption.autoTakeablePriority = (float)workGiver.autoTakeablePriorityDrafted;
									}
									if (!opts.Any((FloatMenuOption op) => op.Label == menuOption.Label))
									{
										if (workGiver.equivalenceGroup != null)
										{
											if (FloatMenuMakerMap.equivalenceGroupTempStorage[(int)workGiver.equivalenceGroup.index] == null || (FloatMenuMakerMap.equivalenceGroupTempStorage[(int)workGiver.equivalenceGroup.index].Disabled && !menuOption.Disabled))
											{
												FloatMenuMakerMap.equivalenceGroupTempStorage[(int)workGiver.equivalenceGroup.index] = menuOption;
												flag = true;
											}
										}
										else
										{
											opts.Add(menuOption);
										}
									}
								}
							}
						}
						IL_6DF:;
					}
				}
				if (flag)
				{
					for (int j = 0; j < FloatMenuMakerMap.equivalenceGroupTempStorage.Length; j++)
					{
						if (FloatMenuMakerMap.equivalenceGroupTempStorage[j] != null)
						{
							opts.Add(FloatMenuMakerMap.equivalenceGroupTempStorage[j]);
							FloatMenuMakerMap.equivalenceGroupTempStorage[j] = null;
						}
					}
				}
			}
			foreach (WorkTypeDef workTypeDef2 in DefDatabase<WorkTypeDef>.AllDefsListForReading)
			{
				for (int k = 0; k < workTypeDef2.workGiversByPriority.Count; k++)
				{
					WorkGiverDef workGiver = workTypeDef2.workGiversByPriority[k];
					if (!drafted || workGiver.canBeDoneWhileDrafted)
					{
						WorkGiver_Scanner workGiver_Scanner2 = workGiver.Worker as WorkGiver_Scanner;
						if (workGiver_Scanner2 != null && workGiver_Scanner2.def.directOrderable && !workGiver_Scanner2.ShouldSkip(pawn, true))
						{
							JobFailReason.Clear();
							if (workGiver_Scanner2.PotentialWorkCellsGlobal(pawn).Contains(clickCell))
							{
								Action action2 = null;
								string label = null;
								PawnCapacityDef pawnCapacityDef2 = workGiver_Scanner2.MissingRequiredCapacity(pawn);
								if (pawnCapacityDef2 != null)
								{
									label = "CannotMissingHealthActivities".Translate(pawnCapacityDef2.label);
								}
								else
								{
									Job job2;
									if (!workGiver_Scanner2.HasJobOnCell(pawn, clickCell, true))
									{
										job2 = null;
									}
									else
									{
										job2 = workGiver_Scanner2.JobOnCell(pawn, clickCell, true);
									}
									if (job2 == null)
									{
										if (JobFailReason.HaveReason)
										{
											if (!JobFailReason.CustomJobString.NullOrEmpty())
											{
												label = "CannotGenericWorkCustom".Translate(JobFailReason.CustomJobString);
											}
											else
											{
												label = "CannotGenericWork".Translate(workGiver_Scanner2.def.verb, "AreaLower".Translate());
											}
											label = label + ": " + JobFailReason.Reason.CapitalizeFirst();
										}
										else
										{
											if (!clickCell.IsForbidden(pawn))
											{
												goto IL_D1E;
											}
											if (!clickCell.InAllowedArea(pawn))
											{
												label = "CannotPrioritizeForbiddenOutsideAllowedArea".Translate() + ": " + pawn.playerSettings.EffectiveAreaRestriction.Label;
											}
											else
											{
												label = "CannotPrioritizeCellForbidden".Translate();
											}
										}
									}
									else
									{
										WorkTypeDef workType2 = workGiver_Scanner2.def.workType;
										if (pawn.jobs.curJob != null && pawn.jobs.curJob.JobIsSameAs(job2))
										{
											label = "CannotGenericAlreadyAmCustom".Translate(workGiver_Scanner2.PostProcessedGerund(job2));
										}
										else if (pawn.workSettings.GetPriority(workType2) == 0)
										{
											if (pawn.WorkTypeIsDisabled(workType2))
											{
												label = "CannotPrioritizeWorkTypeDisabled".Translate(workType2.gerundLabel);
											}
											else if ("CannotPrioritizeNotAssignedToWorkType".CanTranslate())
											{
												label = "CannotPrioritizeNotAssignedToWorkType".Translate(workType2.gerundLabel);
											}
											else
											{
												label = "CannotPrioritizeWorkTypeDisabled".Translate(workType2.pawnLabel);
											}
										}
										else if (clickCell.IsForbidden(pawn))
										{
											if (!clickCell.InAllowedArea(pawn))
											{
												label = "CannotPrioritizeForbiddenOutsideAllowedArea".Translate() + ": " + pawn.playerSettings.EffectiveAreaRestriction.Label;
											}
											else
											{
												label = "CannotPrioritizeCellForbidden".Translate();
											}
										}
										else if (!pawn.CanReach(clickCell, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
										{
											label = "AreaLower".Translate().CapitalizeFirst() + ": " + "NoPath".Translate();
										}
										else
										{
											label = "PrioritizeGeneric".Translate(workGiver_Scanner2.PostProcessedGerund(job2), "AreaLower".Translate());
											Job localJob = job2;
											WorkGiver_Scanner localScanner = workGiver_Scanner2;
											job2.workGiverDef = workGiver_Scanner2.def;
											action2 = delegate
											{
												if (pawn.jobs.TryTakeOrderedJobPrioritizedWork(localJob, localScanner, clickCell) && workGiver.forceMote != null)
												{
													MoteMaker.MakeStaticMote(clickCell, pawn.Map, workGiver.forceMote, 1f);
												}
											};
										}
									}
								}
								if (!opts.Any((FloatMenuOption op) => op.Label == label.TrimEnd(Array.Empty<char>())))
								{
									FloatMenuOption floatMenuOption = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, action2, MenuOptionPriority.Default, null, null, 0f, null, null), pawn, clickCell, "ReservedBy");
									if (drafted && workGiver.autoTakeablePriorityDrafted != -1)
									{
										floatMenuOption.autoTakeable = true;
										floatMenuOption.autoTakeablePriority = (float)workGiver.autoTakeablePriorityDrafted;
									}
									opts.Add(floatMenuOption);
								}
							}
						}
					}
					IL_D1E:;
				}
			}
		}

		
		private static FloatMenuOption GotoLocationOption(IntVec3 clickCell, Pawn pawn)
		{
			int num = GenRadial.NumCellsInRadius(2.9f);
			int i = 0;
			IntVec3 curLoc;
			//Action 9__0;
			//while (i < num)
			//{
			//	curLoc = GenRadial.RadialPattern[i] + clickCell;
			//	if (curLoc.Standable(pawn.Map))
			//	{
			//		if (!(curLoc != pawn.Position))
			//		{
			//			return null;
			//		}
			//		if (!pawn.CanReach(curLoc, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
			//		{
			//			return new FloatMenuOption("CannotGoNoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
			//		}
			//		Action action;
			//		if ((action ) == null)
			//		{
			//			action = ( delegate
			//			{
			//				IntVec3 intVec = RCellFinder.BestOrderedGotoDestNear(curLoc, pawn);
			//				Job job = JobMaker.MakeJob(JobDefOf.Goto, intVec);
			//				if (pawn.Map.exitMapGrid.IsExitCell(UI.MouseCell()))
			//				{
			//					job.exitMapOnArrival = true;
			//				}
			//				else if (!pawn.Map.IsPlayerHome && !pawn.Map.exitMapGrid.MapUsesExitGrid && CellRect.WholeMap(pawn.Map).IsOnEdge(UI.MouseCell(), 3) && pawn.Map.Parent.GetComponent<FormCaravanComp>() != null && MessagesRepeatAvoider.MessageShowAllowed("MessagePlayerTriedToLeaveMapViaExitGrid-" + pawn.Map.uniqueID, 60f))
			//				{
			//					if (pawn.Map.Parent.GetComponent<FormCaravanComp>().CanFormOrReformCaravanNow)
			//					{
			//						Messages.Message("MessagePlayerTriedToLeaveMapViaExitGrid_CanReform".Translate(), pawn.Map.Parent, MessageTypeDefOf.RejectInput, false);
			//					}
			//					else
			//					{
			//						Messages.Message("MessagePlayerTriedToLeaveMapViaExitGrid_CantReform".Translate(), pawn.Map.Parent, MessageTypeDefOf.RejectInput, false);
			//					}
			//				}
			//				if (pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc))
			//				{
			//					MoteMaker.MakeStaticMote(intVec, pawn.Map, ThingDefOf.Mote_FeedbackGoto, 1f);
			//				}
			//			});
			//		}
			//		Action action2 = action;
			//		return new FloatMenuOption("GoHere".Translate(), action2, MenuOptionPriority.GoHere, null, null, 0f, null, null)
			//		{
			//			autoTakeable = true,
			//			autoTakeablePriority = 10f
			//		};
			//	}
			//	else
			//	{
			//		i++;
			//	}
			//}
			return null;
		}

		
		public static Pawn makingFor;

		
		private static FloatMenuOption[] equivalenceGroupTempStorage;
	}
}
