using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public abstract class IncidentWorker_Raid : IncidentWorker_PawnsArrive
	{
		
		protected abstract bool TryResolveRaidFaction(IncidentParms parms);

		
		public abstract void ResolveRaidStrategy(IncidentParms parms, PawnGroupKindDef groupKind);

		
		protected abstract string GetLetterLabel(IncidentParms parms);

		
		protected abstract string GetLetterText(IncidentParms parms, List<Pawn> pawns);

		
		protected abstract LetterDef GetLetterDef();

		
		protected abstract string GetRelatedPawnsInfoLetterText(IncidentParms parms);

		
		protected abstract void ResolveRaidPoints(IncidentParms parms);

		
		public virtual void ResolveRaidArriveMode(IncidentParms parms)
		{
			if (parms.raidArrivalMode != null)
			{
				return;
			}
			if (parms.raidArrivalModeForQuickMilitaryAid)
			{
				if (!(from d in DefDatabase<PawnsArrivalModeDef>.AllDefs
				where d.forQuickMilitaryAid
				select d).Any((PawnsArrivalModeDef d) => d.Worker.GetSelectionWeight(parms) > 0f))
				{
					parms.raidArrivalMode = ((Rand.Value < 0.6f) ? PawnsArrivalModeDefOf.EdgeDrop : PawnsArrivalModeDefOf.CenterDrop);
					return;
				}
			}
			if (parms.raidStrategy == null)
			{
				Log.Error("parms raidStrategy was null but shouldn't be. Defaulting to ImmediateAttack.", false);
				parms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
			}
			if (!(from x in parms.raidStrategy.arriveModes
			where x.Worker.CanUseWith(parms)
			select x).TryRandomElementByWeight((PawnsArrivalModeDef x) => x.Worker.GetSelectionWeight(parms), out parms.raidArrivalMode))
			{
				Log.Error("Could not resolve arrival mode for raid. Defaulting to EdgeWalkIn. parms=" + parms, false);
				parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
			}
		}

		
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			this.ResolveRaidPoints(parms);
			if (!this.TryResolveRaidFaction(parms))
			{
				return false;
			}
			PawnGroupKindDef combat = PawnGroupKindDefOf.Combat;
			this.ResolveRaidStrategy(parms, combat);
			this.ResolveRaidArriveMode(parms);
			parms.raidStrategy.Worker.TryGenerateThreats(parms);
			if (!parms.raidArrivalMode.Worker.TryResolveRaidSpawnCenter(parms))
			{
				return false;
			}
			parms.points = IncidentWorker_Raid.AdjustedRaidPoints(parms.points, parms.raidArrivalMode, parms.raidStrategy, parms.faction, combat);
			List<Pawn> list = parms.raidStrategy.Worker.SpawnThreats(parms);
			if (list == null)
			{
				list = PawnGroupMakerUtility.GeneratePawns(IncidentParmsUtility.GetDefaultPawnGroupMakerParms(combat, parms, false), true).ToList<Pawn>();
				if (list.Count == 0)
				{
					Log.Error("Got no pawns spawning raid from parms " + parms, false);
					return false;
				}
				parms.raidArrivalMode.Worker.Arrive(list, parms);
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Points = " + parms.points.ToString("F0"));
			foreach (Pawn pawn in list)
			{
				string str = (pawn.equipment != null && pawn.equipment.Primary != null) ? pawn.equipment.Primary.LabelCap : "unarmed";
				stringBuilder.AppendLine(pawn.KindLabel + " - " + str);
			}
			TaggedString baseLetterLabel = this.GetLetterLabel(parms);
			TaggedString baseLetterText = this.GetLetterText(parms, list);
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(list, ref baseLetterLabel, ref baseLetterText, this.GetRelatedPawnsInfoLetterText(parms), true, true);
			List<TargetInfo> list2 = new List<TargetInfo>();
			if (parms.pawnGroups != null)
			{
				List<List<Pawn>> list3 = IncidentParmsUtility.SplitIntoGroups(list, parms.pawnGroups);
				List<Pawn> list4 = list3.MaxBy((List<Pawn> x) => x.Count);
				if (list4.Any<Pawn>())
				{
					list2.Add(list4[0]);
				}
				for (int i = 0; i < list3.Count; i++)
				{
					if (list3[i] != list4 && list3[i].Any<Pawn>())
					{
						list2.Add(list3[i][0]);
					}
				}
			}
			else if (list.Any<Pawn>())
			{
				foreach (Pawn t in list)
				{
					list2.Add(t);
				}
			}
			base.SendStandardLetter(baseLetterLabel, baseLetterText, this.GetLetterDef(), parms, list2, Array.Empty<NamedArgument>());
			parms.raidStrategy.Worker.MakeLords(parms, list);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.EquippingWeapons, OpportunityType.Critical);
			if (!PlayerKnowledgeDatabase.IsComplete(ConceptDefOf.ShieldBelts))
			{
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].apparel.WornApparel.Any((Apparel ap) => ap is ShieldBelt))
					{
						LessonAutoActivator.TeachOpportunity(ConceptDefOf.ShieldBelts, OpportunityType.Critical);
						break;
					}
				}
			}
			return true;
		}

		
		public static float AdjustedRaidPoints(float points, PawnsArrivalModeDef raidArrivalMode, RaidStrategyDef raidStrategy, Faction faction, PawnGroupKindDef groupKind)
		{
			if (raidArrivalMode.pointsFactorCurve != null)
			{
				points *= raidArrivalMode.pointsFactorCurve.Evaluate(points);
			}
			if (raidStrategy.pointsFactorCurve != null)
			{
				points *= raidStrategy.pointsFactorCurve.Evaluate(points);
			}
			points = Mathf.Max(points, raidStrategy.Worker.MinimumPoints(faction, groupKind) * 1.05f);
			return points;
		}

		
		public void DoTable_RaidFactionSampled()
		{
			int ticksGame = Find.TickManager.TicksGame;
			Find.TickManager.DebugSetTicksGame(36000000);
			List<TableDataGetter<Faction>> list = new List<TableDataGetter<Faction>>();
			list.Add(new TableDataGetter<Faction>("name", (Faction f) => f.Name));
			foreach (float points in DebugActionsUtility.PointsOptions(false))
			{
				Dictionary<Faction, int> factionCount = new Dictionary<Faction, int>();
				foreach (Faction key in Find.FactionManager.AllFactions)
				{
					factionCount.Add(key, 0);
				}
				for (int i = 0; i < 500; i++)
				{
					IncidentParms incidentParms = new IncidentParms();
					incidentParms.target = Find.CurrentMap;
					incidentParms.points = points;
					if (this.TryResolveRaidFaction(incidentParms))
					{
						Dictionary<Faction, int> factionCount2 = factionCount;
						Faction faction = incidentParms.faction;
						int num = factionCount2[faction];
						factionCount2[faction] = num + 1;
					}
				}
				list.Add(new TableDataGetter<Faction>(points.ToString("F0"), (Faction str) => ((float)factionCount[str] / 500f).ToStringPercent()));
			}
			Find.TickManager.DebugSetTicksGame(ticksGame);
			DebugTables.MakeTablesDialog<Faction>(Find.FactionManager.AllFactions, list.ToArray());
		}

		
		public void DoTable_RaidStrategySampled(Faction fac)
		{
			int ticksGame = Find.TickManager.TicksGame;
			Find.TickManager.DebugSetTicksGame(36000000);
			List<TableDataGetter<RaidStrategyDef>> list = new List<TableDataGetter<RaidStrategyDef>>();
			list.Add(new TableDataGetter<RaidStrategyDef>("defName", (RaidStrategyDef d) => d.defName));
			foreach (float points in DebugActionsUtility.PointsOptions(false))
			{
				Dictionary<RaidStrategyDef, int> strats = new Dictionary<RaidStrategyDef, int>();
				foreach (RaidStrategyDef key in DefDatabase<RaidStrategyDef>.AllDefs)
				{
					strats.Add(key, 0);
				}
				for (int i = 0; i < 500; i++)
				{
					IncidentParms incidentParms = new IncidentParms();
					incidentParms.target = Find.CurrentMap;
					incidentParms.points = points;
					incidentParms.faction = fac;
					if (this.TryResolveRaidFaction(incidentParms))
					{
						this.ResolveRaidStrategy(incidentParms, PawnGroupKindDefOf.Combat);
						if (incidentParms.raidStrategy != null)
						{
							Dictionary<RaidStrategyDef, int> strats2 = strats;
							RaidStrategyDef raidStrategy = incidentParms.raidStrategy;
							int num = strats2[raidStrategy];
							strats2[raidStrategy] = num + 1;
						}
					}
				}
				list.Add(new TableDataGetter<RaidStrategyDef>(points.ToString("F0"), (RaidStrategyDef str) => ((float)strats[str] / 500f).ToStringPercent()));
			}
			Find.TickManager.DebugSetTicksGame(ticksGame);
			DebugTables.MakeTablesDialog<RaidStrategyDef>(DefDatabase<RaidStrategyDef>.AllDefs, list.ToArray());
		}

		
		public void DoTable_RaidArrivalModeSampled(Faction fac)
		{
			int ticksGame = Find.TickManager.TicksGame;
			Find.TickManager.DebugSetTicksGame(36000000);
			List<TableDataGetter<PawnsArrivalModeDef>> list = new List<TableDataGetter<PawnsArrivalModeDef>>();
			list.Add(new TableDataGetter<PawnsArrivalModeDef>("mode", (PawnsArrivalModeDef f) => f.defName));
			foreach (float points in DebugActionsUtility.PointsOptions(false))
			{
				Dictionary<PawnsArrivalModeDef, int> modeCount = new Dictionary<PawnsArrivalModeDef, int>();
				foreach (PawnsArrivalModeDef key in DefDatabase<PawnsArrivalModeDef>.AllDefs)
				{
					modeCount.Add(key, 0);
				}
				for (int i = 0; i < 500; i++)
				{
					IncidentParms incidentParms = new IncidentParms();
					incidentParms.target = Find.CurrentMap;
					incidentParms.points = points;
					incidentParms.faction = fac;
					if (this.TryResolveRaidFaction(incidentParms))
					{
						this.ResolveRaidStrategy(incidentParms, PawnGroupKindDefOf.Combat);
						if (incidentParms.raidStrategy != null)
						{
							this.ResolveRaidArriveMode(incidentParms);
							Dictionary<PawnsArrivalModeDef, int> modeCount2 = modeCount;
							PawnsArrivalModeDef raidArrivalMode = incidentParms.raidArrivalMode;
							int num = modeCount2[raidArrivalMode];
							modeCount2[raidArrivalMode] = num + 1;
						}
					}
				}
				list.Add(new TableDataGetter<PawnsArrivalModeDef>(points.ToString("F0"), (PawnsArrivalModeDef str) => ((float)modeCount[str] / 500f).ToStringPercent()));
			}
			Find.TickManager.DebugSetTicksGame(ticksGame);
			DebugTables.MakeTablesDialog<PawnsArrivalModeDef>(DefDatabase<PawnsArrivalModeDef>.AllDefs, list.ToArray());
		}
	}
}
