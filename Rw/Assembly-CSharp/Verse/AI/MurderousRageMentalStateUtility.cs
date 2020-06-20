using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x0200056E RID: 1390
	public static class MurderousRageMentalStateUtility
	{
		// Token: 0x06002744 RID: 10052 RVA: 0x000E5498 File Offset: 0x000E3698
		public static Pawn FindPawnToKill(Pawn pawn)
		{
			if (!pawn.Spawned)
			{
				return null;
			}
			MurderousRageMentalStateUtility.tmpTargets.Clear();
			List<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				Pawn pawn2 = allPawnsSpawned[i];
				if ((pawn2.Faction == pawn.Faction || (pawn2.IsPrisoner && pawn2.HostFaction == pawn.Faction)) && pawn2.RaceProps.Humanlike && pawn2 != pawn && pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn) && (pawn2.CurJob == null || !pawn2.CurJob.exitMapOnArrival))
				{
					MurderousRageMentalStateUtility.tmpTargets.Add(pawn2);
				}
			}
			if (!MurderousRageMentalStateUtility.tmpTargets.Any<Pawn>())
			{
				return null;
			}
			Pawn result = MurderousRageMentalStateUtility.tmpTargets.RandomElement<Pawn>();
			MurderousRageMentalStateUtility.tmpTargets.Clear();
			return result;
		}

		// Token: 0x04001761 RID: 5985
		private static List<Pawn> tmpTargets = new List<Pawn>();
	}
}
