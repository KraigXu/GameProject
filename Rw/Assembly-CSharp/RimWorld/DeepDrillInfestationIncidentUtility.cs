using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020009DB RID: 2523
	public static class DeepDrillInfestationIncidentUtility
	{
		// Token: 0x06003C36 RID: 15414 RVA: 0x0013DF8C File Offset: 0x0013C18C
		public static void GetUsableDeepDrills(Map map, List<Thing> outDrills)
		{
			outDrills.Clear();
			List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.CreatesInfestations);
			Faction ofPlayer = Faction.OfPlayer;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Faction == ofPlayer && list[i].TryGetComp<CompCreatesInfestations>().CanCreateInfestationNow)
				{
					outDrills.Add(list[i]);
				}
			}
		}
	}
}
