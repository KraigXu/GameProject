using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E03 RID: 3587
	public class Alert_CannotBeUsedRoofed : Alert
	{
		// Token: 0x17000F78 RID: 3960
		// (get) Token: 0x060056B9 RID: 22201 RVA: 0x001CC138 File Offset: 0x001CA338
		private List<Thing> UnusableBuildings
		{
			get
			{
				this.unusableBuildingsResult.Clear();
				if (this.thingDefsToCheck == null)
				{
					this.thingDefsToCheck = new List<ThingDef>();
					foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
					{
						if (!thingDef.canBeUsedUnderRoof)
						{
							this.thingDefsToCheck.Add(thingDef);
						}
					}
				}
				List<Map> maps = Find.Maps;
				Faction ofPlayer = Faction.OfPlayer;
				for (int i = 0; i < this.thingDefsToCheck.Count; i++)
				{
					for (int j = 0; j < maps.Count; j++)
					{
						List<Thing> list = maps[j].listerThings.ThingsOfDef(this.thingDefsToCheck[i]);
						for (int k = 0; k < list.Count; k++)
						{
							if (list[k].Faction == ofPlayer && RoofUtility.IsAnyCellUnderRoof(list[k]))
							{
								this.unusableBuildingsResult.Add(list[k]);
							}
						}
					}
				}
				return this.unusableBuildingsResult;
			}
		}

		// Token: 0x060056BA RID: 22202 RVA: 0x001CC268 File Offset: 0x001CA468
		public Alert_CannotBeUsedRoofed()
		{
			this.defaultLabel = "BuildingCantBeUsedRoofed".Translate();
			this.defaultExplanation = "BuildingCantBeUsedRoofedDesc".Translate();
		}

		// Token: 0x060056BB RID: 22203 RVA: 0x001CC2A5 File Offset: 0x001CA4A5
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.UnusableBuildings);
		}

		// Token: 0x04002F40 RID: 12096
		private List<ThingDef> thingDefsToCheck;

		// Token: 0x04002F41 RID: 12097
		private List<Thing> unusableBuildingsResult = new List<Thing>();
	}
}
