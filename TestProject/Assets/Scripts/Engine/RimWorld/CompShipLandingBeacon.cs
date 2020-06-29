using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class CompShipLandingBeacon : ThingComp
	{
		
		
		public CompProperties_ShipLandingBeacon Props
		{
			get
			{
				return (CompProperties_ShipLandingBeacon)this.props;
			}
		}

		
		
		public List<ShipLandingArea> LandingAreas
		{
			get
			{
				return this.landingAreas;
			}
		}

		
		
		public bool Active
		{
			get
			{
				CompPowerTrader comp = this.parent.GetComp<CompPowerTrader>();
				return comp == null || comp.PowerOn;
			}
		}

		
		private bool CanLinkTo(CompShipLandingBeacon other)
		{
			return other != this && ShipLandingBeaconUtility.CanLinkTo(this.parent.Position, other);
		}

		
		public void EstablishConnections()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			List<CompShipLandingBeacon> list = new List<CompShipLandingBeacon>();
			List<CompShipLandingBeacon> list2 = new List<CompShipLandingBeacon>();
			List<Thing> list3 = this.parent.Map.listerThings.ThingsOfDef(ThingDefOf.ShipLandingBeacon);
			foreach (Thing thing in list3)
			{
				CompShipLandingBeacon compShipLandingBeacon = thing.TryGetComp<CompShipLandingBeacon>();
				if (compShipLandingBeacon != null && this.CanLinkTo(compShipLandingBeacon))
				{
					if (this.parent.Position.x == compShipLandingBeacon.parent.Position.x)
					{
						list2.Add(compShipLandingBeacon);
					}
					else if (this.parent.Position.z == compShipLandingBeacon.parent.Position.z)
					{
						list.Add(compShipLandingBeacon);
					}
				}
			}
			List<CompShipLandingBeacon>.Enumerator enumerator2 = list.GetEnumerator();
			{
				while (enumerator2.MoveNext())
				{
					CompShipLandingBeacon h = enumerator2.Current;
					List<CompShipLandingBeacon>.Enumerator enumerator3 = list2.GetEnumerator();
					{
						while (enumerator3.MoveNext())
						{
							CompShipLandingBeacon v = enumerator3.Current;
							Thing thing2 = list3.FirstOrDefault((Thing x) => x.Position.x == h.parent.Position.x && x.Position.z == v.parent.Position.z);
							if (thing2 != null)
							{
								this.TryAddArea(new ShipLandingArea(CellRect.FromLimits(thing2.Position, this.parent.Position).ContractedBy(1), this.parent.Map)
								{
									beacons = new List<CompShipLandingBeacon>
									{
										this,
										thing2.TryGetComp<CompShipLandingBeacon>(),
										v,
										h
									}
								});
							}
						}
					}
				}
			}
			for (int i = this.landingAreas.Count - 1; i >= 0; i--)
			{
				List<CompShipLandingBeacon>.Enumerator enumerator2 = this.landingAreas[i].beacons.GetEnumerator();
				{
					while (enumerator2.MoveNext())
					{
						if (!enumerator2.Current.TryAddArea(this.landingAreas[i]))
						{
							this.RemoveArea(this.landingAreas[i]);
							break;
						}
					}
				}
			}
		}

		
		private void RemoveArea(ShipLandingArea area)
		{
			foreach (CompShipLandingBeacon compShipLandingBeacon in area.beacons)
			{
				if (compShipLandingBeacon.landingAreas.Contains(area))
				{
					compShipLandingBeacon.landingAreas.Remove(area);
				}
			}
			this.landingAreas.Remove(area);
		}

		
		public bool TryAddArea(ShipLandingArea newArea)
		{
			if (!this.landingAreas.Contains(newArea))
			{
				for (int i = this.landingAreas.Count - 1; i >= 0; i--)
				{
					if (this.landingAreas[i].MyRect.Overlaps(newArea.MyRect) && this.landingAreas[i].MyRect != newArea.MyRect)
					{
						if (this.landingAreas[i].MyRect.Area <= newArea.MyRect.Area)
						{
							return false;
						}
						this.RemoveArea(this.landingAreas[i]);
					}
				}
				this.landingAreas.Add(newArea);
			}
			return true;
		}

		
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			CompGlower compGlower = this.parent.TryGetComp<CompGlower>();
			if (compGlower != null)
			{
				this.fieldColor = compGlower.Props.glowColor.ToColor.ToOpaque();
			}
			this.EstablishConnections();
			foreach (ShipLandingArea shipLandingArea in this.landingAreas)
			{
				shipLandingArea.RecalculateBlockingThing();
			}
		}

		
		public override void PostDeSpawn(Map map)
		{
			for (int i = this.landingAreas.Count - 1; i >= 0; i--)
			{
				this.RemoveArea(this.landingAreas[i]);
			}
			foreach (Thing thing in map.listerThings.ThingsOfDef(ThingDefOf.ShipLandingBeacon))
			{
				CompShipLandingBeacon compShipLandingBeacon = thing.TryGetComp<CompShipLandingBeacon>();
				if (compShipLandingBeacon != null)
				{
					compShipLandingBeacon.EstablishConnections();
				}
			}
		}

		
		public override void CompTickRare()
		{
			foreach (ShipLandingArea shipLandingArea in this.landingAreas)
			{
				shipLandingArea.RecalculateBlockingThing();
			}
		}

		
		public override void PostDrawExtraSelectionOverlays()
		{
			foreach (ShipLandingArea shipLandingArea in this.landingAreas)
			{
				if (shipLandingArea.Active)
				{
					Color color = shipLandingArea.Clear ? this.fieldColor : Color.red;
					color.a = Pulser.PulseBrightness(1f, 0.6f);
					GenDraw.DrawFieldEdges(shipLandingArea.MyRect.ToList<IntVec3>(), color);
				}
				foreach (CompShipLandingBeacon compShipLandingBeacon in shipLandingArea.beacons)
				{
					if (this.CanLinkTo(compShipLandingBeacon))
					{
						GenDraw.DrawLineBetween(this.parent.TrueCenter(), compShipLandingBeacon.parent.TrueCenter(), SimpleColor.White);
					}
				}
			}
		}

		
		public override string CompInspectStringExtra()
		{
			if (!this.parent.Spawned)
			{
				return null;
			}
			string text = "";
			if (!this.Active)
			{
				text += "NotUsable".Translate() + ": " + "Unpowered".Translate().CapitalizeFirst();
			}
			int i = 0;
			while (i < this.landingAreas.Count)
			{
				if (!this.landingAreas[i].Clear)
				{
					if (!text.NullOrEmpty())
					{
						text += "\n";
					}
					text += "NotUsable".Translate() + ": ";
					if (this.landingAreas[i].BlockedByRoof)
					{
						text += "BlockedByRoof".Translate().CapitalizeFirst();
						break;
					}
					text += "BlockedBy".Translate(this.landingAreas[i].FirstBlockingThing).CapitalizeFirst();
					break;
				}
				else
				{
					i++;
				}
			}
			foreach (Thing thing in this.parent.Map.listerThings.ThingsOfDef(ThingDefOf.ShipLandingBeacon))
			{
				if (thing != this.parent && ShipLandingBeaconUtility.AlignedDistanceTooShort(this.parent.Position, thing.Position, this.Props.edgeLengthRange.min - 1f))
				{
					if (!text.NullOrEmpty())
					{
						text += "\n";
					}
					text += "NotUsable".Translate() + ": " + "TooCloseToOtherBeacon".Translate().CapitalizeFirst();
					break;
				}
			}
			return text;
		}

		
		private List<ShipLandingArea> landingAreas = new List<ShipLandingArea>();

		
		private Color fieldColor = Color.white;
	}
}
