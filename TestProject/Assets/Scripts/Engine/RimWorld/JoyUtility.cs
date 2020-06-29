﻿using System;
using System.Collections.Generic;
using System.Text;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	
	public static class JoyUtility
	{
		
		public static bool EnjoyableOutsideNow(Map map, StringBuilder outFailReason = null)
		{
			if (map.weatherManager.RainRate >= 0.25f)
			{
				if (outFailReason != null)
				{
					outFailReason.Append(map.weatherManager.curWeather.label);
				}
				return false;
			}
			GameConditionDef gameConditionDef;
			if (!map.gameConditionManager.AllowEnjoyableOutsideNow(map, out gameConditionDef))
			{
				if (outFailReason != null)
				{
					outFailReason.Append(gameConditionDef.label);
				}
				return false;
			}
			return true;
		}

		
		public static bool EnjoyableOutsideNow(Pawn pawn, StringBuilder outFailReason = null)
		{
			Map mapHeld = pawn.MapHeld;
			if (mapHeld == null)
			{
				return true;
			}
			if (!JoyUtility.EnjoyableOutsideNow(mapHeld, outFailReason))
			{
				return false;
			}
			if (!pawn.ComfortableTemperatureRange().Includes(mapHeld.mapTemperature.OutdoorTemp))
			{
				if (outFailReason != null)
				{
					outFailReason.Append("NotEnjoyableOutsideTemperature".Translate());
				}
				return false;
			}
			return true;
		}

		
		public static void JoyTickCheckEnd(Pawn pawn, JoyTickFullJoyAction fullJoyAction = JoyTickFullJoyAction.EndJob, float extraJoyGainFactor = 1f, Building joySource = null)
		{
			Job curJob = pawn.CurJob;
			if (curJob.def.joyKind == null)
			{
				Log.Warning("This method can only be called for jobs with joyKind.", false);
				return;
			}
			if (joySource != null)
			{
				if (joySource.def.building.joyKind != null && pawn.CurJob.def.joyKind != joySource.def.building.joyKind)
				{
					Log.ErrorOnce("Joy source joyKind and jobDef.joyKind are not the same. building=" + joySource.ToStringSafe<Building>() + ", jobDef=" + pawn.CurJob.def.ToStringSafe<JobDef>(), joySource.thingIDNumber ^ 876598732, false);
				}
				extraJoyGainFactor *= joySource.GetStatValue(StatDefOf.JoyGainFactor, true);
			}
			if (pawn.needs.joy == null)
			{
				pawn.jobs.curDriver.EndJobWith(JobCondition.InterruptForced);
				return;
			}
			pawn.needs.joy.GainJoy(extraJoyGainFactor * curJob.def.joyGainRate * 0.36f / 2500f, curJob.def.joyKind);
			if (curJob.def.joySkill != null)
			{
				pawn.skills.GetSkill(curJob.def.joySkill).Learn(curJob.def.joyXpPerTick, false);
			}
			if (!curJob.ignoreJoyTimeAssignment && !pawn.GetTimeAssignment().allowJoy)
			{
				pawn.jobs.curDriver.EndJobWith(JobCondition.InterruptForced);
			}
			if (pawn.needs.joy.CurLevel > 0.9999f)
			{
				if (fullJoyAction == JoyTickFullJoyAction.EndJob)
				{
					pawn.jobs.curDriver.EndJobWith(JobCondition.Succeeded);
					return;
				}
				if (fullJoyAction == JoyTickFullJoyAction.GoToNextToil)
				{
					pawn.jobs.curDriver.ReadyForNextToil();
				}
			}
		}

		
		public static void TryGainRecRoomThought(Pawn pawn)
		{
			Room room = pawn.GetRoom(RegionType.Set_Passable);
			if (room != null)
			{
				int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(room.GetStat(RoomStatDefOf.Impressiveness));
				if (pawn.needs.mood != null && ThoughtDefOf.AteInImpressiveDiningRoom.stages[scoreStageIndex] != null)
				{
					pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(ThoughtDefOf.JoyActivityInImpressiveRecRoom, scoreStageIndex), null);
				}
			}
		}

		
		public static bool LordPreventsGettingJoy(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			return lord != null && !lord.CurLordToil.AllowSatisfyLongNeeds;
		}

		
		public static bool TimetablePreventsGettingJoy(Pawn pawn)
		{
			return !((pawn.timetable == null) ? TimeAssignmentDefOf.Anything : pawn.timetable.CurrentAssignment).allowJoy;
		}

		
		public static int JoyKindsOnMapCount(Map map)
		{
			List<JoyKindDef> list = JoyUtility.JoyKindsOnMapTempList(map);
			int count = list.Count;
			list.Clear();
			return count;
		}

		
		public static List<JoyKindDef> JoyKindsOnMapTempList(Map map)
		{
			for (int i = 0; i < DefDatabase<JoyKindDef>.AllDefsListForReading.Count; i++)
			{
				JoyKindDef joyKindDef = DefDatabase<JoyKindDef>.AllDefsListForReading[i];
				if (!joyKindDef.needsThing)
				{
					JoyUtility.tempKindList.Add(joyKindDef);
				}
			}
			foreach (Building building in map.listerBuildings.allBuildingsColonist)
			{
				if (building.def.building.joyKind != null && !JoyUtility.tempKindList.Contains(building.def.building.joyKind))
				{
					JoyUtility.tempKindList.Add(building.def.building.joyKind);
				}
			}
			foreach (Thing thing in map.listerThings.ThingsInGroup(ThingRequestGroup.Drug))
			{
				if (thing.def.IsIngestible && thing.def.ingestible.joyKind != null && !JoyUtility.tempKindList.Contains(thing.def.ingestible.joyKind) && !thing.Position.Fogged(map))
				{
					JoyUtility.tempKindList.Add(thing.def.ingestible.joyKind);
				}
			}
			foreach (Thing thing2 in map.listerThings.ThingsInGroup(ThingRequestGroup.FoodSourceNotPlantOrTree))
			{
				if (thing2.def.IsIngestible && thing2.def.ingestible.joyKind != null && !JoyUtility.tempKindList.Contains(thing2.def.ingestible.joyKind) && !thing2.Position.Fogged(map))
				{
					JoyUtility.tempKindList.Add(thing2.def.ingestible.joyKind);
				}
			}
			return JoyUtility.tempKindList;
		}

		
		public static string JoyKindsOnMapString(Map map)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < DefDatabase<JoyKindDef>.AllDefsListForReading.Count; i++)
			{
				JoyKindDef joyKindDef = DefDatabase<JoyKindDef>.AllDefsListForReading[i];
				if (!joyKindDef.needsThing)
				{
					JoyUtility.CheckAppendJoyKind(stringBuilder, null, joyKindDef, map);
				}
			}
			foreach (Building building in map.listerBuildings.allBuildingsColonist)
			{
				if (building.def.building.joyKind != null)
				{
					JoyUtility.CheckAppendJoyKind(stringBuilder, building, building.def.building.joyKind, map);
				}
			}
			foreach (Thing thing in map.listerThings.ThingsInGroup(ThingRequestGroup.Drug))
			{
				if (thing.def.IsIngestible && thing.def.ingestible.joyKind != null)
				{
					JoyUtility.CheckAppendJoyKind(stringBuilder, thing, thing.def.ingestible.joyKind, map);
				}
			}
			foreach (Thing thing2 in map.listerThings.ThingsInGroup(ThingRequestGroup.FoodSourceNotPlantOrTree))
			{
				if (thing2.def.IsIngestible && thing2.def.ingestible.joyKind != null)
				{
					JoyUtility.CheckAppendJoyKind(stringBuilder, thing2, thing2.def.ingestible.joyKind, map);
				}
			}
			JoyUtility.listedJoyKinds.Clear();
			return stringBuilder.ToString().TrimEndNewlines();
		}

		
		private static void CheckAppendJoyKind(StringBuilder sb, Thing t, JoyKindDef kind, Map map)
		{
			if (JoyUtility.listedJoyKinds.Contains(kind))
			{
				return;
			}
			if (t == null)
			{
				sb.AppendLine("   " + kind.LabelCap);
			}
			else
			{
				if (t.def.category == ThingCategory.Item && t.Position.Fogged(map))
				{
					return;
				}
				sb.AppendLine("   " + kind.LabelCap + " (" + t.def.label + ")");
			}
			JoyUtility.listedJoyKinds.Add(kind);
		}

		
		public static string JoyKindsNotOnMapString(Map map)
		{
			List<JoyKindDef> allDefsListForReading = DefDatabase<JoyKindDef>.AllDefsListForReading;
			List<JoyKindDef> list = JoyUtility.JoyKindsOnMapTempList(map);
			if (allDefsListForReading.Count == list.Count)
			{
				return "(" + "None".Translate() + ")";
			}
			string text = "";
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				JoyKindDef joyKindDef = allDefsListForReading[i];
				if (!list.Contains(joyKindDef))
				{
					text += "   " + joyKindDef.LabelCap + "\n";
				}
			}
			list.Clear();
			return text.TrimEndNewlines();
		}

		
		private static List<JoyKindDef> tempKindList = new List<JoyKindDef>();

		
		private static List<JoyKindDef> listedJoyKinds = new List<JoyKindDef>();
	}
}
