using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007B4 RID: 1972
	public class RaidStrategyWorker_SiegeMechanoid : RaidStrategyWorker_Siege
	{
		// Token: 0x06003322 RID: 13090 RVA: 0x0011BB76 File Offset: 0x00119D76
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return parms.points >= this.MinimumPoints(parms.faction, groupKind) && parms.faction == Faction.OfMechanoids && ModsConfig.RoyaltyActive;
		}

		// Token: 0x06003323 RID: 13091 RVA: 0x0011BBA3 File Offset: 0x00119DA3
		public override void TryGenerateThreats(IncidentParms parms)
		{
			parms.mechClusterSketch = MechClusterGenerator.GenerateClusterSketch(parms.points, parms.target as Map, true);
		}

		// Token: 0x06003324 RID: 13092 RVA: 0x0011BBC2 File Offset: 0x00119DC2
		public override List<Pawn> SpawnThreats(IncidentParms parms)
		{
			return MechClusterUtility.SpawnCluster(parms.spawnCenter, (Map)parms.target, parms.mechClusterSketch, true, true, parms.questTag).OfType<Pawn>().ToList<Pawn>();
		}

		// Token: 0x06003325 RID: 13093 RVA: 0x00019EA1 File Offset: 0x000180A1
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			return null;
		}

		// Token: 0x06003326 RID: 13094 RVA: 0x00002681 File Offset: 0x00000881
		public override void MakeLords(IncidentParms parms, List<Pawn> pawns)
		{
		}
	}
}
