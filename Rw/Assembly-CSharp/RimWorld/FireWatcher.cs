using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A39 RID: 2617
	public class FireWatcher
	{
		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x06003DD8 RID: 15832 RVA: 0x001463A5 File Offset: 0x001445A5
		public float FireDanger
		{
			get
			{
				return this.fireDanger;
			}
		}

		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x06003DD9 RID: 15833 RVA: 0x001463AD File Offset: 0x001445AD
		public bool LargeFireDangerPresent
		{
			get
			{
				if (this.fireDanger < 0f)
				{
					this.UpdateObservations();
				}
				return this.fireDanger > 90f;
			}
		}

		// Token: 0x06003DDA RID: 15834 RVA: 0x001463CF File Offset: 0x001445CF
		public FireWatcher(Map map)
		{
			this.map = map;
		}

		// Token: 0x06003DDB RID: 15835 RVA: 0x001463E9 File Offset: 0x001445E9
		public void FireWatcherTick()
		{
			if (Find.TickManager.TicksGame % 426 == 0)
			{
				this.UpdateObservations();
			}
		}

		// Token: 0x06003DDC RID: 15836 RVA: 0x00146404 File Offset: 0x00144604
		private void UpdateObservations()
		{
			this.fireDanger = 0f;
			List<Thing> list = this.map.listerThings.ThingsOfDef(ThingDefOf.Fire);
			for (int i = 0; i < list.Count; i++)
			{
				Fire fire = list[i] as Fire;
				this.fireDanger += 0.5f + fire.fireSize;
			}
		}

		// Token: 0x04002417 RID: 9239
		private Map map;

		// Token: 0x04002418 RID: 9240
		private float fireDanger = -1f;

		// Token: 0x04002419 RID: 9241
		private const int UpdateObservationsInterval = 426;

		// Token: 0x0400241A RID: 9242
		private const float BaseDangerPerFire = 0.5f;
	}
}
