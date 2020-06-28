using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000836 RID: 2102
	public class ThoughtWorker_ColonistLeftUnburied : ThoughtWorker
	{
		// Token: 0x0600347B RID: 13435 RVA: 0x0011FF70 File Offset: 0x0011E170
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.Faction != Faction.OfPlayer)
			{
				return false;
			}
			List<Thing> list = p.Map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.Corpse));
			for (int i = 0; i < list.Count; i++)
			{
				Corpse corpse = (Corpse)list[i];
				if ((float)corpse.Age > 90000f && Alert_ColonistLeftUnburied.IsCorpseOfColonist(corpse))
				{
					return true;
				}
			}
			return false;
		}
	}
}
