using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001064 RID: 4196
	public class PlaceWorker_ShowCompSendSignalOnPawnProximityRadius : PlaceWorker
	{
		// Token: 0x060063E9 RID: 25577 RVA: 0x0022A228 File Offset: 0x00228428
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			CompProperties_SendSignalOnPawnProximity compProperties = def.GetCompProperties<CompProperties_SendSignalOnPawnProximity>();
			if (compProperties == null)
			{
				return;
			}
			GenDraw.DrawRadiusRing(center, compProperties.radius);
		}
	}
}
