using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DDB RID: 3547
	public class Alert_DisallowedBuildingInsideMonument : Alert_Critical
	{
		// Token: 0x17000F5E RID: 3934
		// (get) Token: 0x06005617 RID: 22039 RVA: 0x001C8B24 File Offset: 0x001C6D24
		private List<Thing> DisallowedBuildings
		{
			get
			{
				this.disallowedBuildingsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Thing> list = maps[i].listerThings.ThingsOfDef(ThingDefOf.MonumentMarker);
					for (int j = 0; j < list.Count; j++)
					{
						MonumentMarker monumentMarker = (MonumentMarker)list[j];
						if (monumentMarker.AllDone)
						{
							Thing firstDisallowedBuilding = monumentMarker.FirstDisallowedBuilding;
							if (firstDisallowedBuilding != null)
							{
								this.disallowedBuildingsResult.Add(firstDisallowedBuilding);
							}
						}
					}
				}
				return this.disallowedBuildingsResult;
			}
		}

		// Token: 0x17000F5F RID: 3935
		// (get) Token: 0x06005618 RID: 22040 RVA: 0x001C8BB4 File Offset: 0x001C6DB4
		private int MinTicksLeft
		{
			get
			{
				int num = int.MaxValue;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Thing> list = maps[i].listerThings.ThingsOfDef(ThingDefOf.MonumentMarker);
					for (int j = 0; j < list.Count; j++)
					{
						MonumentMarker monumentMarker = (MonumentMarker)list[j];
						if (monumentMarker.AllDone && monumentMarker.AnyDisallowedBuilding)
						{
							num = Mathf.Min(num, 60000 - monumentMarker.ticksSinceDisallowedBuilding);
						}
					}
				}
				return num;
			}
		}

		// Token: 0x06005619 RID: 22041 RVA: 0x001C8C40 File Offset: 0x001C6E40
		public Alert_DisallowedBuildingInsideMonument()
		{
			this.defaultLabel = "DisallowedBuildingInsideMonument".Translate();
		}

		// Token: 0x0600561A RID: 22042 RVA: 0x001C8C68 File Offset: 0x001C6E68
		public override AlertReport GetReport()
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.DisallowedBuildings);
		}

		// Token: 0x0600561B RID: 22043 RVA: 0x001C8C83 File Offset: 0x001C6E83
		public override TaggedString GetExplanation()
		{
			return "DisallowedBuildingInsideMonumentDesc".Translate(this.MinTicksLeft.ToStringTicksToPeriod(true, false, true, true));
		}

		// Token: 0x04002F14 RID: 12052
		private List<Thing> disallowedBuildingsResult = new List<Thing>();
	}
}
