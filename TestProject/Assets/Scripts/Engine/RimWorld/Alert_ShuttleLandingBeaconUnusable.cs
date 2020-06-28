using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E06 RID: 3590
	public class Alert_ShuttleLandingBeaconUnusable : Alert
	{
		// Token: 0x17000F7B RID: 3963
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

		// Token: 0x060056C3 RID: 22211 RVA: 0x001CC529 File Offset: 0x001CA729
		public Alert_ShuttleLandingBeaconUnusable()
		{
			this.defaultLabel = "ShipLandingBeaconUnusable".Translate();
			this.defaultExplanation = "ShipLandingBeaconUnusableDesc".Translate();
		}

		// Token: 0x060056C4 RID: 22212 RVA: 0x001CC566 File Offset: 0x001CA766
		public override AlertReport GetReport()
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x04002F44 RID: 12100
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
