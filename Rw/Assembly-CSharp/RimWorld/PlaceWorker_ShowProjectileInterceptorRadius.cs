using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200106D RID: 4205
	public class PlaceWorker_ShowProjectileInterceptorRadius : PlaceWorker
	{
		// Token: 0x06006402 RID: 25602 RVA: 0x0022A910 File Offset: 0x00228B10
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			CompProperties_ProjectileInterceptor compProperties = def.GetCompProperties<CompProperties_ProjectileInterceptor>();
			if (compProperties != null)
			{
				GenDraw.DrawCircleOutline(center.ToVector3Shifted(), compProperties.radius);
			}
		}
	}
}
