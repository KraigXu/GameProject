using System;
using UnityEngine;

namespace Verse
{
	
	public abstract class WeatherEvent
	{
		
		// (get) Token: 0x06000DA7 RID: 3495
		public abstract bool Expired { get; }

		
		// (get) Token: 0x06000DA8 RID: 3496 RVA: 0x0004E466 File Offset: 0x0004C666
		public bool CurrentlyAffectsSky
		{
			get
			{
				return this.SkyTargetLerpFactor > 0f;
			}
		}

		
		// (get) Token: 0x06000DA9 RID: 3497 RVA: 0x000255BF File Offset: 0x000237BF
		public virtual SkyTarget SkyTarget
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		
		// (get) Token: 0x06000DAA RID: 3498 RVA: 0x0004E475 File Offset: 0x0004C675
		public virtual float SkyTargetLerpFactor
		{
			get
			{
				return -1f;
			}
		}

		
		// (get) Token: 0x06000DAB RID: 3499 RVA: 0x0004E47C File Offset: 0x0004C67C
		public virtual Vector2? OverrideShadowVector
		{
			get
			{
				return null;
			}
		}

		
		public WeatherEvent(Map map)
		{
			this.map = map;
		}

		
		public abstract void FireEvent();

		
		public abstract void WeatherEventTick();

		
		public virtual void WeatherEventDraw()
		{
		}

		
		protected Map map;
	}
}
