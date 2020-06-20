using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B8 RID: 2488
	public class GameCondition_NoSunlight : GameCondition
	{
		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x06003B5B RID: 15195 RVA: 0x000FAF75 File Offset: 0x000F9175
		public override int TransitionTicks
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x06003B5C RID: 15196 RVA: 0x001396D2 File Offset: 0x001378D2
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 1f);
		}

		// Token: 0x06003B5D RID: 15197 RVA: 0x00139D40 File Offset: 0x00137F40
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(0f, this.EclipseSkyColors, 1f, 0f));
		}

		// Token: 0x0400231B RID: 8987
		private SkyColorSet EclipseSkyColors = new SkyColorSet(new Color(0.482f, 0.603f, 0.682f), Color.white, new Color(0.6f, 0.6f, 0.6f), 1f);
	}
}
