using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000A6F RID: 2671
	public class GenStep_SleepingMechanoids : GenStep
	{
		// Token: 0x17000B2A RID: 2858
		// (get) Token: 0x06003EF9 RID: 16121 RVA: 0x0014E680 File Offset: 0x0014C880
		public override int SeedPart
		{
			get
			{
				return 341176078;
			}
		}

		// Token: 0x06003EFA RID: 16122 RVA: 0x0014EFFC File Offset: 0x0014D1FC
		public static void SendMechanoidsToSleepImmediately(List<Pawn> spawnedMechanoids)
		{
			for (int i = 0; i < spawnedMechanoids.Count; i++)
			{
				spawnedMechanoids[i].jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				JobDriver curDriver = spawnedMechanoids[i].jobs.curDriver;
				if (curDriver != null)
				{
					curDriver.asleep = true;
				}
				CompCanBeDormant comp = spawnedMechanoids[i].GetComp<CompCanBeDormant>();
				if (comp != null)
				{
					comp.ToSleep();
				}
				else
				{
					Log.ErrorOnce("Tried spawning sleeping mechanoid " + spawnedMechanoids[i] + " without CompCanBeDormant!", 317364857 ^ spawnedMechanoids[i].def.defName.GetHashCode(), false);
				}
			}
		}

		// Token: 0x06003EFB RID: 16123 RVA: 0x0014F0A0 File Offset: 0x0014D2A0
		public override void Generate(Map map, GenStepParams parms)
		{
			CellRect around;
			IntVec3 near;
			if (!SiteGenStepUtility.TryFindRootToSpawnAroundRectOfInterest(out around, out near, map))
			{
				return;
			}
			List<Pawn> list = new List<Pawn>();
			foreach (Pawn pawn in this.GeneratePawns(parms, map))
			{
				IntVec3 loc;
				if (!SiteGenStepUtility.TryFindSpawnCellAroundOrNear(around, near, map, out loc))
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					break;
				}
				GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
				list.Add(pawn);
			}
			if (!list.Any<Pawn>())
			{
				return;
			}
			bool @bool = Rand.Bool;
			foreach (Pawn pawn2 in list)
			{
				CompWakeUpDormant comp = pawn2.GetComp<CompWakeUpDormant>();
				if (comp != null)
				{
					comp.wakeUpIfColonistClose = @bool;
				}
			}
			LordMaker.MakeNewLord(Faction.OfMechanoids, new LordJob_SleepThenAssaultColony(Faction.OfMechanoids), map, list);
			GenStep_SleepingMechanoids.SendMechanoidsToSleepImmediately(list);
		}

		// Token: 0x06003EFC RID: 16124 RVA: 0x0014F1A8 File Offset: 0x0014D3A8
		private IEnumerable<Pawn> GeneratePawns(GenStepParams parms, Map map)
		{
			float points = (parms.sitePart != null) ? parms.sitePart.parms.threatPoints : this.defaultPointsRange.RandomInRange;
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
			pawnGroupMakerParms.groupKind = PawnGroupKindDefOf.Combat;
			pawnGroupMakerParms.tile = map.Tile;
			pawnGroupMakerParms.faction = Faction.OfMechanoids;
			pawnGroupMakerParms.points = points;
			if (parms.sitePart != null)
			{
				pawnGroupMakerParms.seed = new int?(SleepingMechanoidsSitePartUtility.GetPawnGroupMakerSeed(parms.sitePart.parms));
			}
			return PawnGroupMakerUtility.GeneratePawns(pawnGroupMakerParms, true);
		}

		// Token: 0x040024A5 RID: 9381
		public FloatRange defaultPointsRange = new FloatRange(340f, 1000f);
	}
}
