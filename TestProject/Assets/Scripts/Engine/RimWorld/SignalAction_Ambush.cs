using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	
	public class SignalAction_Ambush : SignalAction
	{
		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.points, "points", 0f, false);
			Scribe_Values.Look<SignalActionAmbushType>(ref this.ambushType, "ambushType", SignalActionAmbushType.Normal, false);
			Scribe_Values.Look<IntVec3>(ref this.spawnNear, "spawnNear", default(IntVec3), false);
			Scribe_Values.Look<CellRect>(ref this.spawnAround, "spawnAround", default(CellRect), false);
			Scribe_Values.Look<bool>(ref this.spawnPawnsOnEdge, "spawnPawnsOnEdge", false, false);
		}

		
		protected override void DoAction(SignalArgs args)
		{
			if (this.points <= 0f)
			{
				return;
			}
			List<Pawn> list = new List<Pawn>();
			foreach (Pawn pawn in this.GenerateAmbushPawns())
			{
				IntVec3 loc;
				if (this.spawnPawnsOnEdge)
				{
					if (!CellFinder.TryFindRandomEdgeCellWith((IntVec3 x) => x.Standable(base.Map) && !x.Fogged(base.Map) && base.Map.reachability.CanReachColony(x), base.Map, CellFinder.EdgeRoadChance_Ignore, out loc))
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
						break;
					}
				}
				else if (!SiteGenStepUtility.TryFindSpawnCellAroundOrNear(this.spawnAround, this.spawnNear, base.Map, out loc))
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					break;
				}
				GenSpawn.Spawn(pawn, loc, base.Map, WipeMode.Vanish);
				if (!this.spawnPawnsOnEdge)
				{
					for (int i = 0; i < 10; i++)
					{
						MoteMaker.ThrowAirPuffUp(pawn.DrawPos, base.Map);
					}
				}
				list.Add(pawn);
			}
			if (!list.Any<Pawn>())
			{
				return;
			}
			if (this.ambushType == SignalActionAmbushType.Manhunters)
			{
				for (int j = 0; j < list.Count; j++)
				{
					list[j].health.AddHediff(HediffDefOf.Scaria, null, null, null);
					list[j].mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, null, false, false, null, false);
				}
			}
			else
			{
				Faction faction = list[0].Faction;
				LordMaker.MakeNewLord(faction, new LordJob_AssaultColony(faction, true, true, false, false, true), base.Map, list);
			}
			if (!this.spawnPawnsOnEdge)
			{
				for (int k = 0; k < list.Count; k++)
				{
					list[k].jobs.StartJob(JobMaker.MakeJob(JobDefOf.Wait, 120, false), JobCondition.None, null, false, true, null, null, false, false);
					list[k].Rotation = Rot4.Random;
				}
			}
			Find.LetterStack.ReceiveLetter("LetterLabelAmbushInExistingMap".Translate(), "LetterAmbushInExistingMap".Translate(Faction.OfPlayer.def.pawnsPlural).CapitalizeFirst(), LetterDefOf.ThreatBig, list[0], null, null, null, null);
		}

		
		private IEnumerable<Pawn> GenerateAmbushPawns()
		{
			if (this.ambushType == SignalActionAmbushType.Manhunters)
			{
				PawnKindDef animalKind;
				if (!ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(this.points, base.Map.Tile, out animalKind) && !ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(this.points, -1, out animalKind))
				{
					return Enumerable.Empty<Pawn>();
				}
				return ManhunterPackIncidentUtility.GenerateAnimals_NewTmp(animalKind, base.Map.Tile, this.points, 0);
			}
			else
			{
				Faction faction;
				if (this.ambushType == SignalActionAmbushType.Mechanoids)
				{
					faction = Faction.OfMechanoids;
				}
				else
				{
					faction = (base.Map.ParentFaction ?? Find.FactionManager.RandomEnemyFaction(false, false, false, TechLevel.Undefined));
				}
				if (faction == null)
				{
					return Enumerable.Empty<Pawn>();
				}
				return PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
				{
					groupKind = PawnGroupKindDefOf.Combat,
					tile = base.Map.Tile,
					faction = faction,
					points = Mathf.Max(this.points, faction.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat))
				}, true);
			}
		}

		
		public float points;

		
		public SignalActionAmbushType ambushType;

		
		public IntVec3 spawnNear = IntVec3.Invalid;

		
		public CellRect spawnAround;

		
		public bool spawnPawnsOnEdge;

		
		private const int PawnsDelayAfterSpawnTicks = 120;
	}
}
