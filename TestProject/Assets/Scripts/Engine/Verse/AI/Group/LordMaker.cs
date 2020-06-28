using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020005CD RID: 1485
	public static class LordMaker
	{
		// Token: 0x06002973 RID: 10611 RVA: 0x000F4434 File Offset: 0x000F2634
		public static Lord MakeNewLord(Faction faction, LordJob lordJob, Map map, IEnumerable<Pawn> startingPawns = null)
		{
			if (map == null)
			{
				Log.Warning("Tried to create a lord with null map.", false);
				return null;
			}
			Lord lord = new Lord();
			lord.loadID = Find.UniqueIDsManager.GetNextLordID();
			lord.faction = faction;
			map.lordManager.AddLord(lord);
			lord.SetJob(lordJob);
			lord.GotoToil(lord.Graph.StartingToil);
			if (startingPawns != null)
			{
				foreach (Pawn p in startingPawns)
				{
					lord.AddPawn(p);
				}
			}
			return lord;
		}
	}
}
