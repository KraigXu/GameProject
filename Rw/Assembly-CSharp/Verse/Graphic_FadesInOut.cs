using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002E8 RID: 744
	public class Graphic_FadesInOut : Graphic_WithPropertyBlock
	{
		// Token: 0x06001518 RID: 5400 RVA: 0x0007C014 File Offset: 0x0007A214
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			CompFadesInOut compFadesInOut = thing.TryGetComp<CompFadesInOut>();
			if (compFadesInOut == null)
			{
				Log.ErrorOnce(thingDef.defName + ": Graphic_FadesInOut requires CompFadesInOut.", 5643893, false);
				return;
			}
			this.propertyBlock.SetColor(ShaderPropertyIDs.Color, new Color(this.color.r, this.color.g, this.color.b, this.color.a * compFadesInOut.Opacity()));
			base.DrawWorker(loc, rot, thingDef, thing, extraRotation);
		}
	}
}
