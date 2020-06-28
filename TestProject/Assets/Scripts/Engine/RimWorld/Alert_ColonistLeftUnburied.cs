using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DE9 RID: 3561
	public class Alert_ColonistLeftUnburied : Alert
	{
		// Token: 0x06005656 RID: 22102 RVA: 0x001CA00C File Offset: 0x001C820C
		public static bool IsCorpseOfColonist(Corpse corpse)
		{
			return corpse.InnerPawn.Faction == Faction.OfPlayer && corpse.InnerPawn.def.race.Humanlike && !corpse.InnerPawn.IsQuestLodger() && !corpse.IsInAnyStorage();
		}

		// Token: 0x17000F63 RID: 3939
		// (get) Token: 0x06005657 RID: 22103 RVA: 0x001CA05C File Offset: 0x001C825C
		private List<Thing> UnburiedColonistCorpses
		{
			get
			{
				this.unburiedColonistCorpsesResult.Clear();
				foreach (Map map in Find.Maps)
				{
					if (map.mapPawns.AnyFreeColonistSpawned)
					{
						List<Thing> list = map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.Corpse));
						for (int i = 0; i < list.Count; i++)
						{
							Corpse corpse = (Corpse)list[i];
							if (Alert_ColonistLeftUnburied.IsCorpseOfColonist(corpse))
							{
								this.unburiedColonistCorpsesResult.Add(corpse);
							}
						}
					}
				}
				return this.unburiedColonistCorpsesResult;
			}
		}

		// Token: 0x06005658 RID: 22104 RVA: 0x001CA10C File Offset: 0x001C830C
		public Alert_ColonistLeftUnburied()
		{
			this.defaultLabel = "AlertColonistLeftUnburied".Translate();
			this.defaultExplanation = "AlertColonistLeftUnburiedDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06005659 RID: 22105 RVA: 0x001CA15B File Offset: 0x001C835B
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.UnburiedColonistCorpses);
		}

		// Token: 0x04002F28 RID: 12072
		private List<Thing> unburiedColonistCorpsesResult = new List<Thing>();
	}
}
