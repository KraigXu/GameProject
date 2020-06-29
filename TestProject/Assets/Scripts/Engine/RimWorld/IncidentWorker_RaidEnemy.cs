using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class IncidentWorker_RaidEnemy : IncidentWorker_Raid
	{
		
		protected override bool FactionCanBeGroupSource(Faction f, Map map, bool desperate = false)
		{
			return base.FactionCanBeGroupSource(f, map, desperate) && f.HostileTo(Faction.OfPlayer) && (desperate || (float)GenDate.DaysPassed >= f.def.earliestRaidDays);
		}

		
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			if (!base.TryExecuteWorker(parms))
			{
				return false;
			}
			Find.TickManager.slower.SignalForceNormalSpeedShort();
			Find.StoryWatcher.statsRecord.numRaidsEnemy++;
			return true;
		}

		
		protected override bool TryResolveRaidFaction(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (parms.faction != null)
			{
				return true;
			}
			float num = parms.points;
			if (num <= 0f)
			{
				num = 999999f;
			}
			return PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroup(num, out parms.faction, (Faction f) => this.FactionCanBeGroupSource(f, map, false), true, true, true, true) || PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroup(num, out parms.faction, (Faction f) => this.FactionCanBeGroupSource(f, map, true), true, true, true, true);
		}

		
		protected override void ResolveRaidPoints(IncidentParms parms)
		{
			if (parms.points <= 0f)
			{
				Log.Error("RaidEnemy is resolving raid points. They should always be set before initiating the incident.", false);
				parms.points = StorytellerUtility.DefaultThreatPointsNow(parms.target);
			}
		}

		
		public override void ResolveRaidStrategy(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			if (parms.raidStrategy != null)
			{
				return;
			}
			Map map = (Map)parms.target;
			Predicate<PawnsArrivalModeDef> 9__2;
			RaidStrategyDef raidStrategy;
			DefDatabase<RaidStrategyDef>.AllDefs.Where(delegate(RaidStrategyDef d)
			{
				if (!d.Worker.CanUseWith(parms, groupKind))
				{
					return false;
				}
				if (parms.raidArrivalMode != null)
				{
					return true;
				}
				if (d.arriveModes != null)
				{
					List<PawnsArrivalModeDef> arriveModes = d.arriveModes;
					Predicate<PawnsArrivalModeDef> predicate;
					if ((predicate ) == null)
					{
						predicate = (9__2 = ((PawnsArrivalModeDef x) => x.Worker.CanUseWith(parms)));
					}
					return arriveModes.Any(predicate);
				}
				return false;
			}).TryRandomElementByWeight((RaidStrategyDef d) => d.Worker.SelectionWeight(map, parms.points), out raidStrategy);
			parms.raidStrategy = raidStrategy;
			if (parms.raidStrategy == null)
			{
				Log.Error(string.Concat(new object[]
				{
					"No raid stategy found, defaulting to ImmediateAttack. Faction=",
					parms.faction.def.defName,
					", points=",
					parms.points,
					", groupKind=",
					groupKind,
					", parms=",
					parms
				}), false);
				parms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
			}
		}

		
		protected override string GetLetterLabel(IncidentParms parms)
		{
			return parms.raidStrategy.letterLabelEnemy + ": " + parms.faction.Name;
		}

		
		protected override string GetLetterText(IncidentParms parms, List<Pawn> pawns)
		{
			string text = string.Format(parms.raidArrivalMode.textEnemy, parms.faction.def.pawnsPlural, parms.faction.Name.ApplyTag(parms.faction)).CapitalizeFirst();
			text += "\n\n";
			text += parms.raidStrategy.arrivalTextEnemy;
			Pawn pawn = pawns.Find((Pawn x) => x.Faction.leader == x);
			if (pawn != null)
			{
				text += "\n\n";
				text += "EnemyRaidLeaderPresent".Translate(pawn.Faction.def.pawnsPlural, pawn.LabelShort, pawn.Named("LEADER"));
			}
			return text;
		}

		
		protected override LetterDef GetLetterDef()
		{
			return LetterDefOf.ThreatBig;
		}

		
		protected override string GetRelatedPawnsInfoLetterText(IncidentParms parms)
		{
			return "LetterRelatedPawnsRaidEnemy".Translate(Faction.OfPlayer.def.pawnsPlural, parms.faction.def.pawnsPlural);
		}
	}
}
