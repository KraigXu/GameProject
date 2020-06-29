using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld.Planet
{
	
	[StaticConstructorOnStartup]
	public static class CaravanFormingUtility
	{
		
		public static void FormAndCreateCaravan(IEnumerable<Pawn> pawns, Faction faction, int exitFromTile, int directionTile, int destinationTile)
		{
			CaravanExitMapUtility.ExitMapAndCreateCaravan(pawns, faction, exitFromTile, directionTile, destinationTile, true);
		}

		
		public static void StartFormingCaravan(List<Pawn> pawns, List<Pawn> downedPawns, Faction faction, List<TransferableOneWay> transferables, IntVec3 meetingPoint, IntVec3 exitSpot, int startingTile, int destinationTile)
		{
			if (startingTile < 0)
			{
				Log.Error("Can't start forming caravan because startingTile is invalid.", false);
				return;
			}
			if (!pawns.Any<Pawn>())
			{
				Log.Error("Can't start forming caravan with 0 pawns.", false);
				return;
			}
			if (pawns.Any((Pawn x) => x.Downed))
			{
				Log.Warning("Forming a caravan with a downed pawn. This shouldn't happen because we have to create a Lord.", false);
			}
			List<TransferableOneWay> list = transferables.ToList<TransferableOneWay>();
			list.RemoveAll((TransferableOneWay x) => x.CountToTransfer <= 0 || !x.HasAnyThing || x.AnyThing is Pawn);
			for (int i = 0; i < pawns.Count; i++)
			{
				Lord lord = pawns[i].GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(pawns[i], PawnLostCondition.ForcedToJoinOtherLord, null);
				}
			}
			LordJob_FormAndSendCaravan lordJob = new LordJob_FormAndSendCaravan(list, downedPawns, meetingPoint, exitSpot, startingTile, destinationTile);
			LordMaker.MakeNewLord(Faction.OfPlayer, lordJob, pawns[0].MapHeld, pawns);
			for (int j = 0; j < pawns.Count; j++)
			{
				Pawn pawn = pawns[j];
				if (pawn.Spawned)
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}

		
		public static void StopFormingCaravan(Lord lord)
		{
			CaravanFormingUtility.SetToUnloadEverything(lord);
			lord.lordManager.RemoveLord(lord);
		}

		
		public static void RemovePawnFromCaravan(Pawn pawn, Lord lord, bool removeFromDowned = true)
		{
			bool flag = false;
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				Pawn pawn2 = lord.ownedPawns[i];
				if (pawn2 != pawn && CaravanUtility.IsOwner(pawn2, Faction.OfPlayer))
				{
					flag = true;
					break;
				}
			}
			bool flag2 = true;
			TaggedString taggedString = "MessagePawnLostWhileFormingCaravan".Translate(pawn).CapitalizeFirst();
			if (!flag)
			{
				CaravanFormingUtility.StopFormingCaravan(lord);
				taggedString += " " + "MessagePawnLostWhileFormingCaravan_AllLost".Translate();
			}
			else
			{
				pawn.inventory.UnloadEverything = true;
				if (lord.ownedPawns.Contains(pawn))
				{
					lord.Notify_PawnLost(pawn, PawnLostCondition.ForcedByPlayerAction, null);
					flag2 = false;
				}
				LordJob_FormAndSendCaravan lordJob_FormAndSendCaravan = lord.LordJob as LordJob_FormAndSendCaravan;
				if (lordJob_FormAndSendCaravan != null && lordJob_FormAndSendCaravan.downedPawns.Contains(pawn))
				{
					if (!removeFromDowned)
					{
						flag2 = false;
					}
					else
					{
						lordJob_FormAndSendCaravan.downedPawns.Remove(pawn);
					}
				}
				if (pawn.jobs != null)
				{
					pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				}
			}
			if (flag2)
			{
				Messages.Message(taggedString, pawn, MessageTypeDefOf.NegativeEvent, true);
			}
		}

		
		public static void Notify_FormAndSendCaravanLordFailed(Lord lord)
		{
			CaravanFormingUtility.SetToUnloadEverything(lord);
		}

		
		private static void SetToUnloadEverything(Lord lord)
		{
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				lord.ownedPawns[i].inventory.UnloadEverything = true;
			}
		}

		
		public static List<Thing> AllReachableColonyItems(Map map, bool allowEvenIfOutsideHomeArea = false, bool allowEvenIfReserved = false, bool canMinify = false)
		{
			List<Thing> list = new List<Thing>();
			List<Thing> allThings = map.listerThings.AllThings;
			for (int i = 0; i < allThings.Count; i++)
			{
				Thing thing = allThings[i];
				bool flag = canMinify && thing.def.Minifiable;
				if ((flag || thing.def.category == ThingCategory.Item) && (allowEvenIfOutsideHomeArea || map.areaManager.Home[thing.Position] || thing.IsInAnyStorage()) && !thing.Position.Fogged(thing.Map) && (allowEvenIfReserved || !map.reservationManager.IsReservedByAnyoneOf(thing, Faction.OfPlayer)) && (flag || thing.def.EverHaulable) && thing.GetInnerIfMinified().def.canLoadIntoCaravan)
				{
					list.Add(thing);
				}
			}
			return list;
		}

		
		public static List<Pawn> AllSendablePawns(Map map, bool allowEvenIfDowned = false, bool allowEvenIfInMentalState = false, bool allowEvenIfPrisonerNotSecure = false, bool allowCapturableDownedPawns = false, bool allowLodgers = false, int allowLoadAndEnterTransportersLordForGroupID = -1)
		{
			List<Pawn> list = new List<Pawn>();
			List<Pawn> allPawnsSpawned = map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				Pawn pawn = allPawnsSpawned[i];
				if ((allowEvenIfDowned || !pawn.Downed) && (allowEvenIfInMentalState || !pawn.InMentalState) && (pawn.Faction == Faction.OfPlayer || pawn.IsPrisonerOfColony || (allowCapturableDownedPawns && CaravanFormingUtility.CanListAsAutoCapturable(pawn))) && !pawn.IsQuestHelper() && (!pawn.IsQuestLodger() || allowLodgers) && (allowEvenIfPrisonerNotSecure || !pawn.IsPrisoner || pawn.guest.PrisonerIsSecure) && (pawn.GetLord() == null || pawn.GetLord().LordJob is LordJob_VoluntarilyJoinable || (allowLoadAndEnterTransportersLordForGroupID >= 0 && pawn.GetLord().LordJob is LordJob_LoadAndEnterTransporters && ((LordJob_LoadAndEnterTransporters)pawn.GetLord().LordJob).transportersGroup == allowLoadAndEnterTransportersLordForGroupID)))
				{
					list.Add(pawn);
				}
			}
			return list;
		}

		
		private static bool CanListAsAutoCapturable(Pawn p)
		{
			return p.Downed && !p.mindState.WillJoinColonyIfRescued && CaravanUtility.ShouldAutoCapture(p, Faction.OfPlayer);
		}

		
		public static IEnumerable<Gizmo> GetGizmos(Pawn pawn)
		{
			//CaravanFormingUtility.c__DisplayClass11_0 c__DisplayClass11_ = new CaravanFormingUtility.c__DisplayClass11_0();
			//c__DisplayClass11_.pawn = pawn;
			//if (CaravanFormingUtility.IsFormingCaravanOrDownedPawnToBeTakenByCaravan(c__DisplayClass11_.pawn))
			//{
			//	CaravanFormingUtility.c__DisplayClass11_1 c__DisplayClass11_2 = new CaravanFormingUtility.c__DisplayClass11_1();
			//	c__DisplayClass11_2.CS$8__locals1 = c__DisplayClass11_;
			//	c__DisplayClass11_2.lord = CaravanFormingUtility.GetFormAndSendCaravanLord(c__DisplayClass11_2.CS$8__locals1.pawn);
			//	yield return new Command_Action
			//	{
			//		defaultLabel = "CommandCancelFormingCaravan".Translate(),
			//		defaultDesc = "CommandCancelFormingCaravanDesc".Translate(),
			//		icon = TexCommand.ClearPrioritizedWork,
			//		activateSound = SoundDefOf.Tick_Low,
			//		action = delegate
			//		{
			//			CaravanFormingUtility.StopFormingCaravan(c__DisplayClass11_2.lord);
			//		},
			//		hotKey = KeyBindingDefOf.Designator_Cancel
			//	};
			//	yield return new Command_Action
			//	{
			//		defaultLabel = "CommandRemoveFromCaravan".Translate(),
			//		defaultDesc = "CommandRemoveFromCaravanDesc".Translate(),
			//		icon = CaravanFormingUtility.RemoveFromCaravanCommand,
			//		action = delegate
			//		{
			//			CaravanFormingUtility.RemovePawnFromCaravan(c__DisplayClass11_2.CS$8__locals1.pawn, c__DisplayClass11_2.lord, true);
			//		},
			//		hotKey = KeyBindingDefOf.Misc6
			//	};
			//	c__DisplayClass11_2 = null;
			//}
			//else if (c__DisplayClass11_.pawn.Spawned)
			//{
			//	bool flag = false;
			//	for (int i = 0; i < c__DisplayClass11_.pawn.Map.lordManager.lords.Count; i++)
			//	{
			//		Lord lord = c__DisplayClass11_.pawn.Map.lordManager.lords[i];
			//		if (lord.faction == Faction.OfPlayer && lord.LordJob is LordJob_FormAndSendCaravan)
			//		{
			//			flag = true;
			//			break;
			//		}
			//	}
			//	if (flag && Dialog_FormCaravan.AllSendablePawns(c__DisplayClass11_.pawn.Map, false).Contains(c__DisplayClass11_.pawn))
			//	{
			//		yield return new Command_Action
			//		{
			//			defaultLabel = "CommandAddToCaravan".Translate(),
			//			defaultDesc = "CommandAddToCaravanDesc".Translate(),
			//			icon = CaravanFormingUtility.AddToCaravanCommand,
			//			action = delegate
			//			{
			//				List<Lord> list = new List<Lord>();
			//				for (int j = 0; j < c__DisplayClass11_.pawn.Map.lordManager.lords.Count; j++)
			//				{
			//					Lord lord2 = c__DisplayClass11_.pawn.Map.lordManager.lords[j];
			//					if (lord2.faction == Faction.OfPlayer && lord2.LordJob is LordJob_FormAndSendCaravan)
			//					{
			//						list.Add(lord2);
			//					}
			//				}
			//				if (list.Count == 0)
			//				{
			//					return;
			//				}
			//				if (list.Count == 1)
			//				{
			//					CaravanFormingUtility.LateJoinFormingCaravan(c__DisplayClass11_.pawn, list[0]);
			//					SoundDefOf.Click.PlayOneShotOnCamera(null);
			//					return;
			//				}
			//				List<FloatMenuOption> list2 = new List<FloatMenuOption>();
			//				for (int k = 0; k < list.Count; k++)
			//				{
			//					Lord caravanLocal = list[k];
			//					string label = "Caravan".Translate() + " " + (k + 1);
			//					list2.Add(new FloatMenuOption(label, delegate
			//					{
			//						if (c__DisplayClass11_.pawn.Spawned && c__DisplayClass11_.pawn.Map.lordManager.lords.Contains(caravanLocal) && Dialog_FormCaravan.AllSendablePawns(c__DisplayClass11_.pawn.Map, false).Contains(c__DisplayClass11_.pawn))
			//						{
			//							CaravanFormingUtility.LateJoinFormingCaravan(c__DisplayClass11_.pawn, caravanLocal);
			//						}
			//					}, MenuOptionPriority.Default, null, null, 0f, null, null));
			//				}
			//				Find.WindowStack.Add(new FloatMenu(list2));
			//			},
			//			hotKey = KeyBindingDefOf.Misc7
			//		};
			//	}
			//}
			yield break;
		}

		
		private static void LateJoinFormingCaravan(Pawn pawn, Lord lord)
		{
			Lord lord2 = pawn.GetLord();
			if (lord2 != null)
			{
				lord2.Notify_PawnLost(pawn, PawnLostCondition.ForcedToJoinOtherLord, null);
			}
			if (pawn.Downed)
			{
				((LordJob_FormAndSendCaravan)lord.LordJob).downedPawns.Add(pawn);
			}
			else
			{
				lord.AddPawn(pawn);
			}
			if (pawn.Spawned)
			{
				pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		
		public static bool IsFormingCaravan(this Pawn p)
		{
			Lord lord = p.GetLord();
			return lord != null && lord.LordJob is LordJob_FormAndSendCaravan;
		}

		
		public static bool IsFormingCaravanOrDownedPawnToBeTakenByCaravan(Pawn p)
		{
			return CaravanFormingUtility.GetFormAndSendCaravanLord(p) != null;
		}

		
		public static Lord GetFormAndSendCaravanLord(Pawn p)
		{
			if (p.IsFormingCaravan())
			{
				return p.GetLord();
			}
			if (p.Spawned)
			{
				List<Lord> lords = p.Map.lordManager.lords;
				for (int i = 0; i < lords.Count; i++)
				{
					LordJob_FormAndSendCaravan lordJob_FormAndSendCaravan = lords[i].LordJob as LordJob_FormAndSendCaravan;
					if (lordJob_FormAndSendCaravan != null && lordJob_FormAndSendCaravan.downedPawns.Contains(p))
					{
						return lords[i];
					}
				}
			}
			return null;
		}

		
		public static float CapacityLeft(LordJob_FormAndSendCaravan lordJob)
		{
			float num = CollectionsMassCalculator.MassUsageTransferables(lordJob.transferables, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, false, false);
			CaravanFormingUtility.tmpCaravanPawns.Clear();
			for (int i = 0; i < lordJob.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = lordJob.lord.ownedPawns[i];
				CaravanFormingUtility.tmpCaravanPawns.Add(new ThingCount(pawn, pawn.stackCount));
			}
			num += CollectionsMassCalculator.MassUsage(CaravanFormingUtility.tmpCaravanPawns, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, false, false);
			float num2 = CollectionsMassCalculator.Capacity(CaravanFormingUtility.tmpCaravanPawns, null);
			CaravanFormingUtility.tmpCaravanPawns.Clear();
			return num2 - num;
		}

		
		public static string AppendOverweightInfo(string text, float capacityLeft)
		{
			if (capacityLeft < 0f)
			{
				text += " (" + "OverweightLower".Translate() + ")";
			}
			return text;
		}

		
		private static readonly Texture2D RemoveFromCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/RemoveFromCaravan", true);

		
		private static readonly Texture2D AddToCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/AddToCaravan", true);

		
		private static List<ThingCount> tmpCaravanPawns = new List<ThingCount>();
	}
}
