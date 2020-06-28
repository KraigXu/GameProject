using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001056 RID: 4182
	public class PlaceWorker_WindTurbine : PlaceWorker
	{
		// Token: 0x060063C8 RID: 25544 RVA: 0x002299BF File Offset: 0x00227BBF
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			GenDraw.DrawFieldEdges(WindTurbineUtility.CalculateWindCells(center, rot, def.size).ToList<IntVec3>());
		}
	}
}
