using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class Alert_ColonistLeftUnburied : Alert
	{
		
		public static bool IsCorpseOfColonist(Corpse corpse)
		{
			return corpse.InnerPawn.Faction == Faction.OfPlayer && corpse.InnerPawn.def.race.Humanlike && !corpse.InnerPawn.IsQuestLodger() && !corpse.IsInAnyStorage();
		}

		
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

		
		public Alert_ColonistLeftUnburied()
		{
			this.defaultLabel = "AlertColonistLeftUnburied".Translate();
			this.defaultExplanation = "AlertColonistLeftUnburiedDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.UnburiedColonistCorpses);
		}

		
		private List<Thing> unburiedColonistCorpsesResult = new List<Thing>();
	}
}
