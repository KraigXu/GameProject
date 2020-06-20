using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E02 RID: 3586
	public class Alert_BilliardsTableOnWall : Alert
	{
		// Token: 0x17000F77 RID: 3959
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

		// Token: 0x060056B7 RID: 22199 RVA: 0x001CC0ED File Offset: 0x001CA2ED
		public Alert_BilliardsTableOnWall()
		{
			this.defaultLabel = "BilliardsNeedsSpace".Translate();
			this.defaultExplanation = "BilliardsNeedsSpaceDesc".Translate();
		}

		// Token: 0x060056B8 RID: 22200 RVA: 0x001CC12A File Offset: 0x001CA32A
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadTables);
		}

		// Token: 0x04002F3F RID: 12095
		private List<Thing> badTablesResult = new List<Thing>();
	}
}
