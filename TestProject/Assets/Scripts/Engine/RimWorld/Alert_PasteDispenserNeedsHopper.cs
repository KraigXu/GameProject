using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class Alert_PasteDispenserNeedsHopper : Alert
	{
		
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

		
		public Alert_PasteDispenserNeedsHopper()
		{
			this.defaultLabel = "NeedFoodHopper".Translate();
			this.defaultExplanation = "NeedFoodHopperDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadDispensers);
		}

		
		private List<Thing> badDispensersResult = new List<Thing>();
	}
}
