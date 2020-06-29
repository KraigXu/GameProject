using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class RaidStrategyWorker_SiegeMechanoid : RaidStrategyWorker_Siege
	{
		
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return parms.points >= this.MinimumPoints(parms.faction, groupKind) && parms.faction == Faction.OfMechanoids && ModsConfig.RoyaltyActive;
		}

		
		public override void TryGenerateThreats(IncidentParms parms)
		{
			parms.mechClusterSketch = MechClusterGenerator.GenerateClusterSketch(parms.points, parms.target as Map, true);
		}

		
		public override List<Pawn> SpawnThreats(IncidentParms parms)
		{
			return MechClusterUtility.SpawnCluster(parms.spawnCenter, (Map)parms.target, parms.mechClusterSketch, true, true, parms.questTag).OfType<Pawn>().ToList<Pawn>();
		}

		
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			return null;
		}

		
		public override void MakeLords(IncidentParms parms, List<Pawn> pawns)
		{
		}
	}
}
