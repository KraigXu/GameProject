using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public static class ThreatsGenerator
	{
		
		public static IEnumerable<FiringIncident> MakeIntervalIncidents(ThreatsGeneratorParams parms, IIncidentTarget target, int startTick)
		{
			float threatsGeneratorThreatCountFactor = Find.Storyteller.difficulty.threatsGeneratorThreatCountFactor;
			int incCount = IncidentCycleUtility.IncidentCountThisInterval(target, parms.randSeed, (float)startTick / 60000f, parms.onDays, parms.offDays, parms.minSpacingDays, parms.numIncidentsRange.min * threatsGeneratorThreatCountFactor, parms.numIncidentsRange.max * threatsGeneratorThreatCountFactor, 1f);
			int num;
			for (int i = 0; i < incCount; i = num + 1)
			{
				FiringIncident firingIncident = ThreatsGenerator.MakeThreat(parms, target);
				if (firingIncident != null)
				{
					yield return firingIncident;
				}
				num = i;
			}
			yield break;
		}

		
		private static FiringIncident MakeThreat(ThreatsGeneratorParams parms, IIncidentTarget target)
		{
			IncidentParms incParms = ThreatsGenerator.GetIncidentParms(parms, target);
			IncidentDef def;
			if (!(from x in ThreatsGenerator.GetPossibleIncidents(parms.allowedThreats)
			where x.Worker.CanFireNow(incParms, false)
			select x).TryRandomElementByWeight((IncidentDef x) => x.Worker.BaseChanceThisGame, out def))
			{
				return null;
			}
			return new FiringIncident
			{
				def = def,
				parms = incParms
			};
		}

		
		public static bool AnyIncidentPossible(ThreatsGeneratorParams parms, IIncidentTarget target)
		{
			IncidentParms incParms = ThreatsGenerator.GetIncidentParms(parms, target);
			return ThreatsGenerator.GetPossibleIncidents(parms.allowedThreats).Any((IncidentDef x) => x.Worker.BaseChanceThisGame > 0f && x.Worker.CanFireNow(incParms, false));
		}

		
		private static IncidentParms GetIncidentParms(ThreatsGeneratorParams parms, IIncidentTarget target)
		{
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.target = target;
			incidentParms.points = (parms.threatPoints ?? (StorytellerUtility.DefaultThreatPointsNow(target) * parms.currentThreatPointsFactor));
			if (parms.minThreatPoints != null)
			{
				incidentParms.points = Mathf.Max(incidentParms.points, parms.minThreatPoints.Value);
			}
			incidentParms.faction = parms.faction;
			incidentParms.forced = true;
			return incidentParms;
		}

		
		private static IEnumerable<IncidentDef> GetPossibleIncidents(AllowedThreatsGeneratorThreats allowedThreats)
		{
			if ((allowedThreats & AllowedThreatsGeneratorThreats.Raids) != AllowedThreatsGeneratorThreats.None)
			{
				yield return IncidentDefOf.RaidEnemy;
			}
			if ((allowedThreats & AllowedThreatsGeneratorThreats.MechClusters) != AllowedThreatsGeneratorThreats.None)
			{
				yield return IncidentDefOf.MechCluster;
			}
			yield break;
		}
	}
}
