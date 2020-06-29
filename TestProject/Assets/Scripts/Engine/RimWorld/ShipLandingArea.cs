using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class ShipLandingArea
	{
		
		
		public IntVec3 CenterCell
		{
			get
			{
				return this.rect.CenterCell;
			}
		}

		
		
		public CellRect MyRect
		{
			get
			{
				return this.rect;
			}
		}

		
		
		public bool Clear
		{
			get
			{
				return this.firstBlockingThing == null && !this.blockedByRoof;
			}
		}

		
		
		public bool BlockedByRoof
		{
			get
			{
				return this.blockedByRoof;
			}
		}

		
		
		public Thing FirstBlockingThing
		{
			get
			{
				return this.firstBlockingThing;
			}
		}

		
		
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
