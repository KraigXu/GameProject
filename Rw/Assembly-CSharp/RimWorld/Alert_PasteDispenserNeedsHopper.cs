using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DEF RID: 3567
	public class Alert_PasteDispenserNeedsHopper : Alert
	{
		// Token: 0x17000F69 RID: 3945
		// (get) Token: 0x0600566D RID: 22125 RVA: 0x001CA774 File Offset: 0x001C8974
		private List<Thing> BadDispensers
		{
			get
			{
				this.badDispensersResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					foreach (Thing thing in maps[i].listerThings.ThingsInGroup(ThingRequestGroup.FoodDispenser))
					{
						bool flag = false;
						ThingDef hopper = ThingDefOf.Hopper;
						foreach (IntVec3 c in ((Building_NutrientPasteDispenser)thing).AdjCellsCardinalInBounds)
						{
							Thing edifice = c.GetEdifice(thing.Map);
							if (edifice != null && edifice.def == hopper)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							this.badDispensersResult.Add(thing);
						}
					}
				}
				return this.badDispensersResult;
			}
		}

		// Token: 0x0600566E RID: 22126 RVA: 0x001CA878 File Offset: 0x001C8A78
		public Alert_PasteDispenserNeedsHopper()
		{
			this.defaultLabel = "NeedFoodHopper".Translate();
			this.defaultExplanation = "NeedFoodHopperDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x0600566F RID: 22127 RVA: 0x001CA8C7 File Offset: 0x001C8AC7
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadDispensers);
		}

		// Token: 0x04002F2E RID: 12078
		private List<Thing> badDispensersResult = new List<Thing>();
	}
}
