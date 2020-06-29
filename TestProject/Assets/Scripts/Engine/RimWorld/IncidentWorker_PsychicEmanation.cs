using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public abstract class IncidentWorker_PsychicEmanation : IncidentWorker
	{
		
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return !map.gameConditionManager.ConditionIsActive(GameConditionDefOf.PsychicDrone) && !map.gameConditionManager.ConditionIsActive(GameConditionDefOf.PsychicSoothe) && map.listerThings.ThingsOfDef(ThingDefOf.PsychicDronerShipPart).Count <= 0 && map.mapPawns.FreeColonistsCount != 0;
		}

		
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			this.DoConditionAndLetter(parms, map, Mathf.RoundToInt(this.def.durationDays.RandomInRange * 60000f), map.mapPawns.FreeColonists.RandomElement<Pawn>().gender, parms.points);
			return true;
		}

		
		protected abstract void DoConditionAndLetter(IncidentParms parms, Map map, int duration, Gender gender, float points);
	}
}
