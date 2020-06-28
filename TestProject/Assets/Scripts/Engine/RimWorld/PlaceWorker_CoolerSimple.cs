using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200105D RID: 4189
	public class PlaceWorker_CoolerSimple : PlaceWorker
	{
		// Token: 0x060063D7 RID: 25559 RVA: 0x00229C10 File Offset: 0x00227E10
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			Map currentMap = Find.CurrentMap;
			RoomGroup roomGroup = center.GetRoomGroup(currentMap);
			if (roomGroup != null && !roomGroup.UsesOutdoorTemperature)
			{
				GenDraw.DrawFieldEdges(roomGroup.Cells.ToList<IntVec3>(), GenTemperature.ColorRoomCold);
			}
		}
	}
}
