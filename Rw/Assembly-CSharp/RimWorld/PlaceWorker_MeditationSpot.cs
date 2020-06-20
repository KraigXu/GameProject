using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200106F RID: 4207
	public class PlaceWorker_MeditationSpot : PlaceWorker
	{
		// Token: 0x06006406 RID: 25606 RVA: 0x0022A96C File Offset: 0x00228B6C
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			MeditationUtility.DrawMeditationSpotOverlay(center, Find.CurrentMap);
		}
	}
}
