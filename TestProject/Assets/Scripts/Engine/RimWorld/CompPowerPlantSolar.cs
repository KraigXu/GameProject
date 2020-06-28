using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A7A RID: 2682
	[StaticConstructorOnStartup]
	public class CompPowerPlantSolar : CompPowerPlant
	{
		// Token: 0x17000B3C RID: 2876
		// (get) Token: 0x06003F52 RID: 16210 RVA: 0x0015088C File Offset: 0x0014EA8C
		protected override float DesiredPowerOutput
		{
			get
			{
				return Mathf.Lerp(0f, 1700f, this.parent.Map.skyManager.CurSkyGlow) * this.RoofedPowerOutputFactor;
			}
		}

		// Token: 0x17000B3D RID: 2877
		// (get) Token: 0x06003F53 RID: 16211 RVA: 0x001508BC File Offset: 0x0014EABC
		private float RoofedPowerOutputFactor
		{
			get
			{
				int num = 0;
				int num2 = 0;
				foreach (IntVec3 c in this.parent.OccupiedRect())
				{
					num++;
					if (this.parent.Map.roofGrid.Roofed(c))
					{
						num2++;
					}
				}
				return (float)(num - num2) / (float)num;
			}
		}

		// Token: 0x06003F54 RID: 16212 RVA: 0x00150940 File Offset: 0x0014EB40
		public override void PostDraw()
		{
			base.PostDraw();
			GenDraw.FillableBarRequest r = default(GenDraw.FillableBarRequest);
			r.center = this.parent.DrawPos + Vector3.up * 0.1f;
			r.size = CompPowerPlantSolar.BarSize;
			r.fillPercent = base.PowerOutput / 1700f;
			r.filledMat = CompPowerPlantSolar.PowerPlantSolarBarFilledMat;
			r.unfilledMat = CompPowerPlantSolar.PowerPlantSolarBarUnfilledMat;
			r.margin = 0.15f;
			Rot4 rotation = this.parent.Rotation;
			rotation.Rotate(RotationDirection.Clockwise);
			r.rotation = rotation;
			GenDraw.DrawFillableBar(r);
		}

		// Token: 0x040024CC RID: 9420
		private const float FullSunPower = 1700f;

		// Token: 0x040024CD RID: 9421
		private const float NightPower = 0f;

		// Token: 0x040024CE RID: 9422
		private static readonly Vector2 BarSize = new Vector2(2.3f, 0.14f);

		// Token: 0x040024CF RID: 9423
		private static readonly Material PowerPlantSolarBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.5f, 0.475f, 0.1f), false);

		// Token: 0x040024D0 RID: 9424
		private static readonly Material PowerPlantSolarBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.15f, 0.15f, 0.15f), false);
	}
}
