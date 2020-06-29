using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public static class SiegeBlueprintPlacer
	{
		
		public static IEnumerable<Blueprint_Build> PlaceBlueprints(IntVec3 placeCenter, Map map, Faction placeFaction, float points)
		{
			SiegeBlueprintPlacer.center = placeCenter;
			SiegeBlueprintPlacer.faction = placeFaction;
			foreach (Blueprint_Build blueprint_Build in SiegeBlueprintPlacer.PlaceCoverBlueprints(map))
			{
				yield return blueprint_Build;
			}
			IEnumerator<Blueprint_Build> enumerator = null;
			foreach (Blueprint_Build blueprint_Build2 in SiegeBlueprintPlacer.PlaceArtilleryBlueprints(points, map))
			{
				yield return blueprint_Build2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		
		private static bool CanPlaceBlueprintAt(IntVec3 root, Rot4 rot, ThingDef buildingDef, Map map, ThingDef stuffDef)
		{
			return GenConstruct.CanPlaceBlueprintAt(buildingDef, root, rot, map, false, null, null, stuffDef).Accepted;
		}

		
		private static IEnumerable<Blueprint_Build> PlaceCoverBlueprints(Map map)
		{
			SiegeBlueprintPlacer.placedCoverLocs.Clear();
			ThingDef coverThing;
			ThingDef coverStuff;
			if (Rand.Chance(0.5f))
			{
				coverThing = ThingDefOf.Sandbags;
				coverStuff = ThingDefOf.Cloth;
			}
			else
			{
				coverThing = ThingDefOf.Barricade;
				coverStuff = (Rand.Chance(0.5f) ? ThingDefOf.Steel : ThingDefOf.WoodLog);
			}
			int numCover = SiegeBlueprintPlacer.NumCoverRange.RandomInRange;
			int num;
			for (int i = 0; i < numCover; i = num + 1)
			{
				IntVec3 bagRoot = SiegeBlueprintPlacer.FindCoverRoot(map, coverThing, coverStuff);
				if (!bagRoot.IsValid)
				{
					yield break;
				}
				Rot4 growDir;
				if (bagRoot.x > SiegeBlueprintPlacer.center.x)
				{
					growDir = Rot4.West;
				}
				else
				{
					growDir = Rot4.East;
				}
				Rot4 growDirB;
				if (bagRoot.z > SiegeBlueprintPlacer.center.z)
				{
					growDirB = Rot4.South;
				}
				else
				{
					growDirB = Rot4.North;
				}
				foreach (Blueprint_Build blueprint_Build in SiegeBlueprintPlacer.MakeCoverLine(bagRoot, map, growDir, SiegeBlueprintPlacer.CoverLengthRange.RandomInRange, coverThing, coverStuff))
				{
					yield return blueprint_Build;
				}
				IEnumerator<Blueprint_Build> enumerator = null;
				bagRoot += growDirB.FacingCell;
				foreach (Blueprint_Build blueprint_Build2 in SiegeBlueprintPlacer.MakeCoverLine(bagRoot, map, growDirB, SiegeBlueprintPlacer.CoverLengthRange.RandomInRange, coverThing, coverStuff))
				{
					yield return blueprint_Build2;
				}
				enumerator = null;
				num = i;
			}
			yield break;
			yield break;
		}

		
		private static IEnumerable<Blueprint_Build> MakeCoverLine(IntVec3 root, Map map, Rot4 growDir, int maxLength, ThingDef coverThing, ThingDef coverStuff)
		{
			IntVec3 cur = root;
			int i = 0;
			while (i < maxLength && SiegeBlueprintPlacer.CanPlaceBlueprintAt(cur, Rot4.North, coverThing, map, coverStuff))
			{
				yield return GenConstruct.PlaceBlueprintForBuild(coverThing, cur, map, Rot4.North, SiegeBlueprintPlacer.faction, coverStuff);
				SiegeBlueprintPlacer.placedCoverLocs.Add(cur);
				cur += growDir.FacingCell;
				int num = i;
				i = num + 1;
			}
			yield break;
		}

		
		private static IEnumerable<Blueprint_Build> PlaceArtilleryBlueprints(float points, Map map)
		{
			IEnumerable<ThingDef> artyDefs = from def in DefDatabase<ThingDef>.AllDefs
			where def.building != null && def.building.buildingTags.Contains("Artillery_BaseDestroyer")
			select def;
			int numArtillery = Mathf.RoundToInt(points / 60f);
			numArtillery = Mathf.Clamp(numArtillery, 1, 2);
			int num;
			for (int i = 0; i < numArtillery; i = num + 1)
			{
				Rot4 random = Rot4.Random;
				ThingDef thingDef = artyDefs.RandomElement<ThingDef>();
				IntVec3 intVec = SiegeBlueprintPlacer.FindArtySpot(thingDef, random, map);
				if (!intVec.IsValid)
				{
					yield break;
				}
				yield return GenConstruct.PlaceBlueprintForBuild(thingDef, intVec, map, random, SiegeBlueprintPlacer.faction, ThingDefOf.Steel);
				points -= 60f;
				num = i;
			}
			yield break;
		}

		
		private static IntVec3 FindCoverRoot(Map map, ThingDef coverThing, ThingDef coverStuff)
		{
			CellRect cellRect = CellRect.CenteredOn(SiegeBlueprintPlacer.center, 13);
			cellRect.ClipInsideMap(map);
			CellRect cellRect2 = CellRect.CenteredOn(SiegeBlueprintPlacer.center, 8);
			cellRect2.ClipInsideMap(map);
			int num = 0;
			for (;;)
			{
				num++;
				if (num > 200)
				{
					break;
				}
				IntVec3 randomCell = cellRect.RandomCell;
				if (!cellRect2.Contains(randomCell) && map.reachability.CanReach(randomCell, SiegeBlueprintPlacer.center, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.Deadly) && SiegeBlueprintPlacer.CanPlaceBlueprintAt(randomCell, Rot4.North, coverThing, map, coverStuff))
				{
					bool flag = false;
					for (int i = 0; i < SiegeBlueprintPlacer.placedCoverLocs.Count; i++)
					{
						if ((float)(SiegeBlueprintPlacer.placedCoverLocs[i] - randomCell).LengthHorizontalSquared < 36f)
						{
							flag = true;
						}
					}
					if (!flag)
					{
						return randomCell;
					}
				}
			}
			return IntVec3.Invalid;
		}

		
		private static IntVec3 FindArtySpot(ThingDef artyDef, Rot4 rot, Map map)
		{
			CellRect cellRect = CellRect.CenteredOn(SiegeBlueprintPlacer.center, 8);
			cellRect.ClipInsideMap(map);
			int num = 0;
			for (;;)
			{
				num++;
				if (num > 200)
				{
					break;
				}
				IntVec3 randomCell = cellRect.RandomCell;
				if (map.reachability.CanReach(randomCell, SiegeBlueprintPlacer.center, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.Deadly) && !randomCell.Roofed(map) && SiegeBlueprintPlacer.CanPlaceBlueprintAt(randomCell, rot, artyDef, map, ThingDefOf.Steel))
				{
					return randomCell;
				}
			}
			return IntVec3.Invalid;
		}

		
		private static IntVec3 center;

		
		private static Faction faction;

		
		private static List<IntVec3> placedCoverLocs = new List<IntVec3>();

		
		private const int MaxArtyCount = 2;

		
		public const float ArtyCost = 60f;

		
		private const int MinCoverDistSquared = 36;

		
		private static readonly IntRange NumCoverRange = new IntRange(2, 4);

		
		private static readonly IntRange CoverLengthRange = new IntRange(2, 7);
	}
}
