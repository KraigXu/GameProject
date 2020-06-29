using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class Alert_ShuttleLandingBeaconUnusable : Alert
	{
		
		// (get) Token: 0x060056C2 RID: 22210 RVA: 0x001CC48C File Offset: 0x001CA68C
		private List<GlobalTargetInfo> Targets
		{
			get
			{
				this.targets.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Thing> list = maps[i].listerThings.ThingsOfDef(ThingDefOf.ShipLandingBeacon);
					for (int j = 0; j < list.Count; j++)
					{
						CompShipLandingBeacon compShipLandingBeacon = list[j].TryGetComp<CompShipLandingBeacon>();
						if (compShipLandingBeacon != null && compShipLandingBeacon.Active && !compShipLandingBeacon.LandingAreas.Any<ShipLandingArea>())
						{
							this.targets.Add(list[j]);
						}
					}
				}
				return this.targets;
			}
		}

		
		public Alert_ShuttleLandingBeaconUnusable()
		{
			this.defaultLabel = "ShipLandingBeaconUnusable".Translate();
			this.defaultExplanation = "ShipLandingBeaconUnusableDesc".Translate();
		}

		
		public override AlertReport GetReport()
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.Targets);
		}

		
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
