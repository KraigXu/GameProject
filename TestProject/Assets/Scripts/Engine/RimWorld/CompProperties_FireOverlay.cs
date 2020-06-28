using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200086E RID: 2158
	public class CompProperties_FireOverlay : CompProperties
	{
		// Token: 0x06003524 RID: 13604 RVA: 0x00122DFC File Offset: 0x00120FFC
		public CompProperties_FireOverlay()
		{
			this.compClass = typeof(CompFireOverlay);
		}

		// Token: 0x06003525 RID: 13605 RVA: 0x00122E1F File Offset: 0x0012101F
		public override void DrawGhost(IntVec3 center, Rot4 rot, ThingDef thingDef, Color ghostCol, AltitudeLayer drawAltitude, Thing thing = null)
		{
			GhostUtility.GhostGraphicFor(CompFireOverlay.FireGraphic, thingDef, ghostCol).DrawFromDef(center.ToVector3ShiftedWithAltitude(drawAltitude), rot, thingDef, 0f);
		}

		// Token: 0x04001C71 RID: 7281
		public float fireSize = 1f;

		// Token: 0x04001C72 RID: 7282
		public Vector3 offset;
	}
}
