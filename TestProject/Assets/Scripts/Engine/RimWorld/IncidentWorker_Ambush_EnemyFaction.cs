using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020009CE RID: 2510
	public class IncidentWorker_Ambush_EnemyFaction : IncidentWorker_Ambush
	{
		// Token: 0x06003BF2 RID: 15346 RVA: 0x0013C0A0 File Offset: 0x0013A2A0
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Faction faction;
			return base.CanFireNowSub(parms) && PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroup(parms.points, out faction, null, false, false, false, true);
		}

		// Token: 0x06003BF3 RID: 15347 RVA: 0x0013C0CC File Offset: 0x0013A2CC
		protected override List<Pawn> GeneratePawns(IncidentParms parms)
		{
			if (!PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroup(parms.points, out parms.faction, null, false, false, false, true))
			{
				Log.Error("Could not find any valid faction for " + this.def + " incident.", false);
				return new List<Pawn>();
			}
			PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(PawnGroupKindDefOf.Combat, parms, false);
			defaultPawnGroupMakerParms.generateFightersOnly = true;
			defaultPawnGroupMakerParms.dontUseSingleUseRocketLaunchers = true;
			return PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms, true).ToList<Pawn>();
		}

		// Token: 0x06003BF4 RID: 15348 RVA: 0x0013C137 File Offset: 0x0013A337
		protected override LordJob CreateLordJob(List<Pawn> generatedPawns, IncidentParms parms)
		{
			return new LordJob_AssaultColony(parms.faction, true, false, false, false, true);
		}

		// Token: 0x06003BF5 RID: 15349 RVA: 0x0013C14C File Offset: 0x0013A34C
		protected override string GetLetterText(Pawn anyPawn, IncidentParms parms)
		{
			Caravan caravan = parms.target as Caravan;
			return string.Format(this.def.letterText, (caravan != null) ? caravan.Name : "yourCaravan".TranslateSimple(), parms.faction.def.pawnsPlural, parms.faction.Name).CapitalizeFirst();
		}
	}
}
