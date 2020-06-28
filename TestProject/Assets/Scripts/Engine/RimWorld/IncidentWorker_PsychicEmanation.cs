using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009EB RID: 2539
	public abstract class IncidentWorker_PsychicEmanation : IncidentWorker
	{
		// Token: 0x06003C72 RID: 15474 RVA: 0x0013F714 File Offset: 0x0013D914
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			return !map.gameConditionManager.ConditionIsActive(GameConditionDefOf.PsychicDrone) && !map.gameConditionManager.ConditionIsActive(GameConditionDefOf.PsychicSoothe) && map.listerThings.ThingsOfDef(ThingDefOf.PsychicDronerShipPart).Count <= 0 && map.mapPawns.FreeColonistsCount != 0;
		}

		// Token: 0x06003C73 RID: 15475 RVA: 0x0013F780 File Offset: 0x0013D980
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			this.DoConditionAndLetter(parms, map, Mathf.RoundToInt(this.def.durationDays.RandomInRange * 60000f), map.mapPawns.FreeColonists.RandomElement<Pawn>().gender, parms.points);
			return true;
		}

		// Token: 0x06003C74 RID: 15476
		protected abstract void DoConditionAndLetter(IncidentParms parms, Map map, int duration, Gender gender, float points);
	}
}
