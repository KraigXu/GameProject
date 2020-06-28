using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D52 RID: 3410
	public class ShipLandingArea
	{
		// Token: 0x17000EB4 RID: 3764
		// (get) Token: 0x060052E8 RID: 21224 RVA: 0x001BAE9C File Offset: 0x001B909C
		public IntVec3 CenterCell
		{
			get
			{
				return this.rect.CenterCell;
			}
		}

		// Token: 0x17000EB5 RID: 3765
		// (get) Token: 0x060052E9 RID: 21225 RVA: 0x001BAEA9 File Offset: 0x001B90A9
		public CellRect MyRect
		{
			get
			{
				return this.rect;
			}
		}

		// Token: 0x17000EB6 RID: 3766
		// (get) Token: 0x060052EA RID: 21226 RVA: 0x001BAEB1 File Offset: 0x001B90B1
		public bool Clear
		{
			get
			{
				return this.firstBlockingThing == null && !this.blockedByRoof;
			}
		}

		// Token: 0x17000EB7 RID: 3767
		// (get) Token: 0x060052EB RID: 21227 RVA: 0x001BAEC6 File Offset: 0x001B90C6
		public bool BlockedByRoof
		{
			get
			{
				return this.blockedByRoof;
			}
		}

		// Token: 0x17000EB8 RID: 3768
		// (get) Token: 0x060052EC RID: 21228 RVA: 0x001BAECE File Offset: 0x001B90CE
		public Thing FirstBlockingThing
		{
			get
			{
				return this.firstBlockingThing;
			}
		}

		// Token: 0x17000EB9 RID: 3769
		// (get) Token: 0x060052ED RID: 21229 RVA: 0x001BAED8 File Offset: 0x001B90D8
		public bool Active
		{
			get
			{
				for (int i = 0; i < this.beacons.Count; i++)
				{
					if (!this.beacons[i].Active)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x060052EE RID: 21230 RVA: 0x001BAF11 File Offset: 0x001B9111
		public ShipLandingArea(CellRect rect, Map map)
		{
			this.rect = rect;
			this.map = map;
		}

		// Token: 0x060052EF RID: 21231 RVA: 0x001BAF34 File Offset: 0x001B9134
		public void RecalculateBlockingThing()
		{
			this.blockedByRoof = false;
			foreach (IntVec3 c in this.rect)
			{
				if (c.Roofed(this.map))
				{
					this.blockedByRoof = true;
					break;
				}
				List<Thing> thingList = c.GetThingList(this.map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (!(thingList[i] is Pawn) && (thingList[i].def.Fillage != FillCategory.None || thingList[i].def.IsEdifice() || thingList[i] is Skyfaller))
					{
						this.firstBlockingThing = thingList[i];
						return;
					}
				}
			}
			this.firstBlockingThing = null;
		}

		// Token: 0x04002DC0 RID: 11712
		private CellRect rect;

		// Token: 0x04002DC1 RID: 11713
		private Map map;

		// Token: 0x04002DC2 RID: 11714
		private Thing firstBlockingThing;

		// Token: 0x04002DC3 RID: 11715
		private bool blockedByRoof;

		// Token: 0x04002DC4 RID: 11716
		public List<CompShipLandingBeacon> beacons = new List<CompShipLandingBeacon>();
	}
}
