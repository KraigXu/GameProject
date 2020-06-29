using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class Alert_BilliardsTableOnWall : Alert
	{
		
		// (get) Token: 0x060056B6 RID: 22198 RVA: 0x001CC054 File Offset: 0x001CA254
		private List<Thing> BadTables
		{
			get
			{
				this.badTablesResult.Clear();
				List<Map> maps = Find.Maps;
				Faction ofPlayer = Faction.OfPlayer;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Thing> list = maps[i].listerThings.ThingsOfDef(ThingDefOf.BilliardsTable);
					for (int j = 0; j < list.Count; j++)
					{
						if (list[j].Faction == ofPlayer && !JoyGiver_PlayBilliards.ThingHasStandableSpaceOnAllSides(list[j]))
						{
							this.badTablesResult.Add(list[j]);
						}
					}
				}
				return this.badTablesResult;
			}
		}

		
		public Alert_BilliardsTableOnWall()
		{
			this.defaultLabel = "BilliardsNeedsSpace".Translate();
			this.defaultExplanation = "BilliardsNeedsSpaceDesc".Translate();
		}

		
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadTables);
		}

		
		private List<Thing> badTablesResult = new List<Thing>();
	}
}
