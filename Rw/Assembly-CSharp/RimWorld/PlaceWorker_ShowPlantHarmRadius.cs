using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001065 RID: 4197
	public class PlaceWorker_ShowPlantHarmRadius : PlaceWorker
	{
		// Token: 0x060063EB RID: 25579 RVA: 0x0022A24C File Offset: 0x0022844C
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			if (thing == null)
			{
				return;
			}
			CompPlantHarmRadius compPlantHarmRadius = thing.TryGetComp<CompPlantHarmRadius>();
			if (compPlantHarmRadius == null)
			{
				return;
			}
			float currentRadius = compPlantHarmRadius.CurrentRadius;
			if (currentRadius < 50f)
			{
				GenDraw.DrawRadiusRing(center, currentRadius);
			}
		}
	}
}
