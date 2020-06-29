using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class ShipLandingArea
	{
		
		// (get) Token: 0x060052E8 RID: 21224 RVA: 0x001BAE9C File Offset: 0x001B909C
		public IntVec3 CenterCell
		{
			get
			{
				return this.rect.CenterCell;
			}
		}

		
		// (get) Token: 0x060052E9 RID: 21225 RVA: 0x001BAEA9 File Offset: 0x001B90A9
		public CellRect MyRect
		{
			get
			{
				return this.rect;
			}
		}

		
		// (get) Token: 0x060052EA RID: 21226 RVA: 0x001BAEB1 File Offset: 0x001B90B1
		public bool Clear
		{
			get
			{
				return this.firstBlockingThing == null && !this.blockedByRoof;
			}
		}

		
		// (get) Token: 0x060052EB RID: 21227 RVA: 0x001BAEC6 File Offset: 0x001B90C6
		public bool BlockedByRoof
		{
			get
			{
				return this.blockedByRoof;
			}
		}

		
		// (get) Token: 0x060052EC RID: 21228 RVA: 0x001BAECE File Offset: 0x001B90CE
		public Thing FirstBlockingThing
		{
			get
			{
				return this.firstBlockingThing;
			}
		}

		
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

		
		public ShipLandingArea(CellRect rect, Map map)
		{
			this.rect = rect;
			this.map = map;
		}

		
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

		
		private CellRect rect;

		
		private Map map;

		
		private Thing firstBlockingThing;

		
		private bool blockedByRoof;

		
		public List<CompShipLandingBeacon> beacons = new List<CompShipLandingBeacon>();
	}
}
