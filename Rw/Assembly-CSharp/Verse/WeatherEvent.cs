using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001E4 RID: 484
	public abstract class WeatherEvent
	{
		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000DA7 RID: 3495
		public abstract bool Expired { get; }

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000DA8 RID: 3496 RVA: 0x0004E466 File Offset: 0x0004C666
		public bool CurrentlyAffectsSky
		{
			get
			{
				return this.SkyTargetLerpFactor > 0f;
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000DA9 RID: 3497 RVA: 0x000255BF File Offset: 0x000237BF
		public virtual SkyTarget SkyTarget
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000DAA RID: 3498 RVA: 0x0004E475 File Offset: 0x0004C675
		public virtual float SkyTargetLerpFactor
		{
			get
			{
				return -1f;
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000DAB RID: 3499 RVA: 0x0004E47C File Offset: 0x0004C67C
		public virtual Vector2? OverrideShadowVector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x0004E492 File Offset: 0x0004C692
		public WeatherEvent(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000DAD RID: 3501
		public abstract void FireEvent();

		// Token: 0x06000DAE RID: 3502
		public abstract void WeatherEventTick();

		// Token: 0x06000DAF RID: 3503 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void WeatherEventDraw()
		{
		}

		// Token: 0x04000A7A RID: 2682
		protected Map map;
	}
}
