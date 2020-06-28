using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002F3 RID: 755
	public class Graphic_Shadow : Graphic
	{
		// Token: 0x06001559 RID: 5465 RVA: 0x0007D1BF File Offset: 0x0007B3BF
		public Graphic_Shadow(ShadowData shadowInfo)
		{
			this.shadowInfo = shadowInfo;
			if (shadowInfo == null)
			{
				throw new ArgumentNullException("shadowInfo");
			}
			this.shadowMesh = ShadowMeshPool.GetShadowMesh(shadowInfo);
		}

		// Token: 0x0600155A RID: 5466 RVA: 0x0007D1E8 File Offset: 0x0007B3E8
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			if (this.shadowMesh != null && thingDef != null && this.shadowInfo != null && (Find.CurrentMap == null || !loc.ToIntVec3().InBounds(Find.CurrentMap) || !Find.CurrentMap.roofGrid.Roofed(loc.ToIntVec3())) && DebugViewSettings.drawShadows)
			{
				Vector3 position = loc + this.shadowInfo.offset;
				position.y = AltitudeLayer.Shadows.AltitudeFor();
				Graphics.DrawMesh(this.shadowMesh, position, rot.AsQuat, MatBases.SunShadowFade, 0);
			}
		}

		// Token: 0x0600155B RID: 5467 RVA: 0x0007D280 File Offset: 0x0007B480
		public override void Print(SectionLayer layer, Thing thing)
		{
			Vector3 center = thing.TrueCenter() + (this.shadowInfo.offset + new Vector3(Graphic_Shadow.GlobalShadowPosOffsetX, 0f, Graphic_Shadow.GlobalShadowPosOffsetZ)).RotatedBy(thing.Rotation);
			center.y = AltitudeLayer.Shadows.AltitudeFor();
			Printer_Shadow.PrintShadow(layer, center, this.shadowInfo, thing.Rotation);
		}

		// Token: 0x0600155C RID: 5468 RVA: 0x0007D2E9 File Offset: 0x0007B4E9
		public override string ToString()
		{
			return "Graphic_Shadow(" + this.shadowInfo + ")";
		}

		// Token: 0x04000E05 RID: 3589
		private Mesh shadowMesh;

		// Token: 0x04000E06 RID: 3590
		private ShadowData shadowInfo;

		// Token: 0x04000E07 RID: 3591
		[TweakValue("Graphics_Shadow", -5f, 5f)]
		private static float GlobalShadowPosOffsetX;

		// Token: 0x04000E08 RID: 3592
		[TweakValue("Graphics_Shadow", -5f, 5f)]
		private static float GlobalShadowPosOffsetZ;
	}
}
