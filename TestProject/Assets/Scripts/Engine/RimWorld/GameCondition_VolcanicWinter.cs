using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009BD RID: 2493
	public class GameCondition_VolcanicWinter : GameCondition
	{
		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x06003B7C RID: 15228 RVA: 0x0013A4DD File Offset: 0x001386DD
		public override int TransitionTicks
		{
			get
			{
				return 50000;
			}
		}

		// Token: 0x06003B7D RID: 15229 RVA: 0x0013A4E4 File Offset: 0x001386E4
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 0.3f);
		}

		// Token: 0x06003B7E RID: 15230 RVA: 0x0013A4F8 File Offset: 0x001386F8
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(0.55f, this.VolcanicWinterColors, 1f, 1f));
		}

		// Token: 0x06003B7F RID: 15231 RVA: 0x0013A519 File Offset: 0x00138719
		public override float TemperatureOffset()
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, this.MaxTempOffset);
		}

		// Token: 0x06003B80 RID: 15232 RVA: 0x0013A52E File Offset: 0x0013872E
		public override float AnimalDensityFactor(Map map)
		{
			return 1f - GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 0.5f);
		}

		// Token: 0x06003B81 RID: 15233 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool AllowEnjoyableOutsideNow(Map map)
		{
			return false;
		}

		// Token: 0x04002329 RID: 9001
		private float MaxTempOffset = -7f;

		// Token: 0x0400232A RID: 9002
		private const float AnimalDensityImpact = 0.5f;

		// Token: 0x0400232B RID: 9003
		private const float SkyGlow = 0.55f;

		// Token: 0x0400232C RID: 9004
		private const float MaxSkyLerpFactor = 0.3f;

		// Token: 0x0400232D RID: 9005
		private SkyColorSet VolcanicWinterColors = new SkyColorSet(new ColorInt(0, 0, 0).ToColor, Color.white, new Color(0.6f, 0.6f, 0.6f), 0.65f);
	}
}
