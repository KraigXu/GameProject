using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001066 RID: 4198
	public class PlaceWorker_ShowExplosionRadius : PlaceWorker
	{
		// Token: 0x060063ED RID: 25581 RVA: 0x0022A280 File Offset: 0x00228480
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			CompProperties_Explosive compProperties = def.GetCompProperties<CompProperties_Explosive>();
			if (compProperties == null)
			{
				return;
			}
			GenDraw.DrawRadiusRing(center, compProperties.explosiveRadius);
		}
	}
}
