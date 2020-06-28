using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001067 RID: 4199
	public class PlaceWorker_WatchArea : PlaceWorker
	{
		// Token: 0x060063EF RID: 25583 RVA: 0x0022A2A4 File Offset: 0x002284A4
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			GenDraw.DrawFieldEdges(WatchBuildingUtility.CalculateWatchCells(def, center, rot, currentMap).ToList<IntVec3>());
		}
	}
}
