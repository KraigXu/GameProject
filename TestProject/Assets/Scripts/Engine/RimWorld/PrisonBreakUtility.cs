using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000B80 RID: 2944
	public static class PrisonBreakUtility
	{
		// Token: 0x06004503 RID: 17667 RVA: 0x00174F10 File Offset: 0x00173110
		public static float InitiatePrisonBreakMtbDays(Pawn pawn)
		{
			if (!pawn.Awake())
			{
				return -1f;
			}
			if (!PrisonBreakUtility.CanParticipateInPrisonBreak(pawn))
			{
				return -1f;
			}
			Room room = pawn.GetRoom(RegionType.Set_Passable);
			if (room == null || !room.isPrisonCell)
			{
				return -1f;
			}
			float num = 60f;
			num /= Mathf.Clamp(pawn.health.capacities.GetLevel(PawnCapacityDefOf.Moving), 0.01f, 1f);
			if (pawn.guest.everParticipatedInPrisonBreak)
			{
				float x = (float)(Find.TickManager.TicksGame - pawn.guest.lastPrisonBreakTicks) / 60000f;
				num *= PrisonBreakUtility.PrisonBreakMTBFactorForDaysSincePrisonBreak.Evaluate(x);
			}
			return num;
		}

		// Token: 0x06004504 RID: 17668 RVA: 0x00174FB9 File Offset: 0x001731B9
		public static bool CanParticipateInPrisonBreak(Pawn pawn)
		{
			return !pawn.Downed && pawn.IsPrisoner && !PrisonBreakUtility.IsPrisonBreaking(pawn);
		}

		// Token: 0x06004505 RID: 17669 RVA: 0x00174FDC File Offset: 0x001731DC
		public static bool IsPrisonBreaking(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			return lord != null && lord.LordJob is LordJob_PrisonBreak;
		}

		// Token: 0x06004506 RID: 17670 RVA: 0x00175004 File Offset: 0x00173204
		public static void StartPrisonBreak(Pawn initiator)
		{
			string str;
			string str2;
			LetterDef textLetterDef;
			PrisonBreakUtility.StartPrisonBreak(initiator, out str, out str2, out textLetterDef);
			if (!str.NullOrEmpty())
			{
				Find.LetterStack.ReceiveLetter(str2, str, textLetterDef, initiator, null, null, null, null);
			}
		}

		// Token: 0x06004507 RID: 17671 RVA: 0x00175048 File Offset: 0x00173248
		public static void StartPrisonBreak(Pawn initiator, out string letterText, out string letterLabel, out LetterDef letterDef)
		{
			PrisonBreakUtility.participatingRooms.Clear();
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(initiator.Position, 20f, true))
			{
				if (intVec.InBounds(initiator.Map))
				{
					Room room = intVec.GetRoom(initiator.Map, RegionType.Set_Passable);
					if (room != null && PrisonBreakUtility.IsOrCanBePrisonCell(room))
					{
						PrisonBreakUtility.participatingRooms.Add(room);
					}
				}
			}
			PrisonBreakUtility.RemoveRandomRooms(PrisonBreakUtility.participatingRooms, initiator);
			int sapperThingID = -1;
			if (Rand.Value < 0.5f)
			{
				sapperThingID = initiator.thingIDNumber;
			}
			PrisonBreakUtility.allEscapingPrisoners.Clear();
			foreach (Room room2 in PrisonBreakUtility.participatingRooms)
			{
				PrisonBreakUtility.StartPrisonBreakIn(room2, PrisonBreakUtility.allEscapingPrisoners, sapperThingID, PrisonBreakUtility.participatingRooms);
			}
			PrisonBreakUtility.participatingRooms.Clear();
			if (PrisonBreakUtility.allEscapingPrisoners.Any<Pawn>())
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < PrisonBreakUtility.allEscapingPrisoners.Count; i++)
				{
					stringBuilder.AppendLine("  - " + PrisonBreakUtility.allEscapingPrisoners[i].NameShortColored.Resolve());
				}
				letterText = "LetterPrisonBreak".Translate(stringBuilder.ToString().TrimEndNewlines());
				letterLabel = "LetterLabelPrisonBreak".Translate();
				letterDef = LetterDefOf.ThreatBig;
				PrisonBreakUtility.allEscapingPrisoners.Clear();
			}
			else
			{
				letterText = null;
				letterLabel = null;
				letterDef = null;
			}
			Find.TickManager.slower.SignalForceNormalSpeed();
		}

		// Token: 0x06004508 RID: 17672 RVA: 0x00175210 File Offset: 0x00173410
		private static bool IsOrCanBePrisonCell(Room room)
		{
			if (room.isPrisonCell)
			{
				return true;
			}
			if (room.TouchesMapEdge)
			{
				return false;
			}
			bool result = false;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Pawn pawn = containedAndAdjacentThings[i] as Pawn;
				if (pawn != null && pawn.IsPrisoner)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x06004509 RID: 17673 RVA: 0x00175268 File Offset: 0x00173468
		private static void RemoveRandomRooms(HashSet<Room> participatingRooms, Pawn initiator)
		{
			Room room = initiator.GetRoom(RegionType.Set_Passable);
			PrisonBreakUtility.tmpToRemove.Clear();
			foreach (Room room2 in participatingRooms)
			{
				if (room2 != room && Rand.Value >= 0.5f)
				{
					PrisonBreakUtility.tmpToRemove.Add(room2);
				}
			}
			for (int i = 0; i < PrisonBreakUtility.tmpToRemove.Count; i++)
			{
				participatingRooms.Remove(PrisonBreakUtility.tmpToRemove[i]);
			}
			PrisonBreakUtility.tmpToRemove.Clear();
		}

		// Token: 0x0600450A RID: 17674 RVA: 0x00175310 File Offset: 0x00173510
		private static void StartPrisonBreakIn(Room room, List<Pawn> outAllEscapingPrisoners, int sapperThingID, HashSet<Room> participatingRooms)
		{
			PrisonBreakUtility.escapingPrisonersGroup.Clear();
			PrisonBreakUtility.AddPrisonersFrom(room, PrisonBreakUtility.escapingPrisonersGroup);
			if (!PrisonBreakUtility.escapingPrisonersGroup.Any<Pawn>())
			{
				return;
			}
			foreach (Room room2 in participatingRooms)
			{
				if (room2 != room && PrisonBreakUtility.RoomsAreCloseToEachOther(room, room2))
				{
					PrisonBreakUtility.AddPrisonersFrom(room2, PrisonBreakUtility.escapingPrisonersGroup);
				}
			}
			IntVec3 exitPoint;
			if (!RCellFinder.TryFindRandomExitSpot(PrisonBreakUtility.escapingPrisonersGroup[0], out exitPoint, TraverseMode.PassDoors))
			{
				return;
			}
			IntVec3 groupUpLoc;
			if (!PrisonBreakUtility.TryFindGroupUpLoc(PrisonBreakUtility.escapingPrisonersGroup, exitPoint, out groupUpLoc))
			{
				return;
			}
			LordMaker.MakeNewLord(PrisonBreakUtility.escapingPrisonersGroup[0].Faction, new LordJob_PrisonBreak(groupUpLoc, exitPoint, sapperThingID), room.Map, PrisonBreakUtility.escapingPrisonersGroup);
			for (int i = 0; i < PrisonBreakUtility.escapingPrisonersGroup.Count; i++)
			{
				Pawn pawn = PrisonBreakUtility.escapingPrisonersGroup[i];
				if (pawn.CurJob != null && pawn.GetPosture().Laying())
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
				else
				{
					pawn.jobs.CheckForJobOverride();
				}
				pawn.guest.everParticipatedInPrisonBreak = true;
				pawn.guest.lastPrisonBreakTicks = Find.TickManager.TicksGame;
				outAllEscapingPrisoners.Add(pawn);
			}
			PrisonBreakUtility.escapingPrisonersGroup.Clear();
		}

		// Token: 0x0600450B RID: 17675 RVA: 0x00175474 File Offset: 0x00173674
		private static void AddPrisonersFrom(Room room, List<Pawn> outEscapingPrisoners)
		{
			foreach (Thing thing in room.ContainedAndAdjacentThings)
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && PrisonBreakUtility.CanParticipateInPrisonBreak(pawn) && !outEscapingPrisoners.Contains(pawn))
				{
					outEscapingPrisoners.Add(pawn);
				}
			}
		}

		// Token: 0x0600450C RID: 17676 RVA: 0x001754E0 File Offset: 0x001736E0
		private static bool TryFindGroupUpLoc(List<Pawn> escapingPrisoners, IntVec3 exitPoint, out IntVec3 groupUpLoc)
		{
			groupUpLoc = IntVec3.Invalid;
			Map map = escapingPrisoners[0].Map;
			using (PawnPath pawnPath = map.pathFinder.FindPath(escapingPrisoners[0].Position, exitPoint, TraverseParms.For(escapingPrisoners[0], Danger.Deadly, TraverseMode.PassDoors, false), PathEndMode.OnCell))
			{
				if (!pawnPath.Found)
				{
					Log.Warning("Prison break: could not find path for prisoner " + escapingPrisoners[0] + " to the exit point.", false);
					return false;
				}
				Func<IntVec3, bool> <>9__0;
				for (int i = 0; i < pawnPath.NodesLeftCount; i++)
				{
					IntVec3 intVec = pawnPath.Peek(pawnPath.NodesLeftCount - i - 1);
					Room room = intVec.GetRoom(map, RegionType.Set_Passable);
					if (room != null && !room.isPrisonCell)
					{
						if (!room.TouchesMapEdge && !room.IsHuge)
						{
							IEnumerable<IntVec3> cells = room.Cells;
							Func<IntVec3, bool> predicate;
							if ((predicate = <>9__0) == null)
							{
								predicate = (<>9__0 = ((IntVec3 x) => x.Standable(map)));
							}
							if (cells.Count(predicate) < 5)
							{
								goto IL_10E;
							}
						}
						groupUpLoc = CellFinder.RandomClosewalkCellNear(intVec, map, 3, null);
					}
					IL_10E:;
				}
			}
			if (!groupUpLoc.IsValid)
			{
				groupUpLoc = escapingPrisoners[0].Position;
			}
			return true;
		}

		// Token: 0x0600450D RID: 17677 RVA: 0x00175644 File Offset: 0x00173844
		private static bool RoomsAreCloseToEachOther(Room a, Room b)
		{
			IntVec3 anyCell = a.Regions[0].AnyCell;
			IntVec3 anyCell2 = b.Regions[0].AnyCell;
			if (a.Map != b.Map)
			{
				return false;
			}
			if (!anyCell.WithinRegions(anyCell2, a.Map, 18, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false), RegionType.Set_Passable))
			{
				return false;
			}
			bool result;
			using (PawnPath pawnPath = a.Map.pathFinder.FindPath(anyCell, anyCell2, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false), PathEndMode.OnCell))
			{
				if (!pawnPath.Found)
				{
					result = false;
				}
				else
				{
					result = (pawnPath.NodesLeftCount < 24);
				}
			}
			return result;
		}

		// Token: 0x04002772 RID: 10098
		private const float BaseInitiatePrisonBreakMtbDays = 60f;

		// Token: 0x04002773 RID: 10099
		private const float DistanceToJoinPrisonBreak = 20f;

		// Token: 0x04002774 RID: 10100
		private const float ChanceForRoomToJoinPrisonBreak = 0.5f;

		// Token: 0x04002775 RID: 10101
		private const float SapperChance = 0.5f;

		// Token: 0x04002776 RID: 10102
		private static readonly SimpleCurve PrisonBreakMTBFactorForDaysSincePrisonBreak = new SimpleCurve
		{
			{
				new CurvePoint(0f, 20f),
				true
			},
			{
				new CurvePoint(5f, 1.5f),
				true
			},
			{
				new CurvePoint(10f, 1f),
				true
			}
		};

		// Token: 0x04002777 RID: 10103
		private static HashSet<Room> participatingRooms = new HashSet<Room>();

		// Token: 0x04002778 RID: 10104
		private static List<Pawn> allEscapingPrisoners = new List<Pawn>();

		// Token: 0x04002779 RID: 10105
		private static List<Room> tmpToRemove = new List<Room>();

		// Token: 0x0400277A RID: 10106
		private static List<Pawn> escapingPrisonersGroup = new List<Pawn>();
	}
}
