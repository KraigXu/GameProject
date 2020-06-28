using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C92 RID: 3218
	public class PlaceWorker_MonumentMarker : PlaceWorker
	{
		// Token: 0x06004DA3 RID: 19875 RVA: 0x001A14EC File Offset: 0x0019F6EC
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			MonumentMarker monumentMarker = thing as MonumentMarker;
			if (monumentMarker != null)
			{
				monumentMarker.DrawGhost_NewTmp(center, true, rot);
			}
		}

		// Token: 0x06004DA4 RID: 19876 RVA: 0x001A1510 File Offset: 0x0019F710
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			MonumentMarker monumentMarker = thing as MonumentMarker;
			if (monumentMarker != null)
			{
				CellRect rect = monumentMarker.sketch.OccupiedRect.MovedBy(loc);
				Blueprint_Install thingToIgnore2 = monumentMarker.FindMyBlueprint(rect, map);
				foreach (SketchEntity sketchEntity in monumentMarker.sketch.Entities)
				{
					CellRect cellRect = sketchEntity.OccupiedRect.MovedBy(loc);
					if (!cellRect.InBounds(map))
					{
						return false;
					}
					if (cellRect.InNoBuildEdgeArea(map))
					{
						return "TooCloseToMapEdge".Translate();
					}
					foreach (IntVec3 at in cellRect)
					{
						if (!sketchEntity.CanBuildOnTerrain(at, map))
						{
							return "MonumentBadTerrain".Translate();
						}
					}
					if (sketchEntity.IsSpawningBlockedPermanently(loc + sketchEntity.pos, map, thingToIgnore2, false))
					{
						return "MonumentBlockedPermanently".Translate();
					}
				}
				PlaceWorker_MonumentMarker.tmpMonumentThings.Clear();
				foreach (SketchBuildable sketchBuildable in monumentMarker.sketch.Buildables)
				{
					Thing spawnedBlueprintOrFrame = sketchBuildable.GetSpawnedBlueprintOrFrame(loc + sketchBuildable.pos, map);
					SketchThing sketchThing;
					if (spawnedBlueprintOrFrame != null)
					{
						PlaceWorker_MonumentMarker.tmpMonumentThings.Add(spawnedBlueprintOrFrame);
					}
					else if ((sketchThing = (sketchBuildable as SketchThing)) != null)
					{
						Thing sameSpawned = sketchThing.GetSameSpawned(loc + sketchThing.pos, map);
						if (sameSpawned != null)
						{
							PlaceWorker_MonumentMarker.tmpMonumentThings.Add(sameSpawned);
						}
					}
				}
				foreach (SketchEntity sketchEntity2 in monumentMarker.sketch.Entities)
				{
					if (!sketchEntity2.IsSameSpawnedOrBlueprintOrFrame(loc + sketchEntity2.pos, map))
					{
						foreach (IntVec3 c in sketchEntity2.OccupiedRect.MovedBy(loc))
						{
							if (c.InBounds(map))
							{
								Building firstBuilding = c.GetFirstBuilding(map);
								if (firstBuilding != null && !PlaceWorker_MonumentMarker.tmpMonumentThings.Contains(firstBuilding))
								{
									PlaceWorker_MonumentMarker.tmpMonumentThings.Clear();
									return "MonumentOverlapsBuilding".Translate();
								}
							}
						}
					}
				}
				foreach (SketchEntity sketchEntity3 in monumentMarker.sketch.Entities)
				{
					if (!sketchEntity3.IsSameSpawnedOrBlueprintOrFrame(loc + sketchEntity3.pos, map))
					{
						foreach (IntVec3 c2 in sketchEntity3.OccupiedRect.MovedBy(loc).ExpandedBy(1).EdgeCells)
						{
							if (c2.InBounds(map))
							{
								Building firstBuilding2 = c2.GetFirstBuilding(map);
								if (firstBuilding2 != null && !PlaceWorker_MonumentMarker.tmpMonumentThings.Contains(firstBuilding2) && (firstBuilding2.Faction == null || firstBuilding2.Faction == Faction.OfPlayer))
								{
									PlaceWorker_MonumentMarker.tmpMonumentThings.Clear();
									return "MonumentAdjacentToBuilding".Translate();
								}
							}
						}
					}
				}
				PlaceWorker_MonumentMarker.tmpMonumentThings.Clear();
			}
			return true;
		}

		// Token: 0x04002B70 RID: 11120
		private static List<Thing> tmpMonumentThings = new List<Thing>();
	}
}
