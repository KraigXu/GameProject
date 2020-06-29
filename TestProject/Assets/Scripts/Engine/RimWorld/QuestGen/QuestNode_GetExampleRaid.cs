using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGenNew
{
	
	public class QuestNode_GetExampleRaid : QuestNode
	{
		
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Exists("map", false);
		}

		
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			slate.Get<Map>("map", null, false);
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
			pawnGroupMakerParms.groupKind = PawnGroupKindDefOf.Combat;
			pawnGroupMakerParms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
			PawnGroupMakerParms pawnGroupMakerParms2 = pawnGroupMakerParms;
			Faction faction;
			if ((faction = this.faction.GetValue(slate)) == null)
			{
				faction = (from x in Find.FactionManager.GetFactions(false, false, true, TechLevel.Industrial)
				where x.HostileTo(Faction.OfPlayer)
				select x).RandomElement<Faction>();
			}
			pawnGroupMakerParms2.faction = faction;
			pawnGroupMakerParms.points = IncidentWorker_Raid.AdjustedRaidPoints(this.points.GetValue(slate), PawnsArrivalModeDefOf.EdgeWalkIn, RaidStrategyDefOf.ImmediateAttack, pawnGroupMakerParms.faction, PawnGroupKindDefOf.Combat);
			IEnumerable<PawnKindDef> pawnKinds = PawnGroupMakerUtility.GeneratePawnKindsExample(pawnGroupMakerParms);
			slate.Set<string>(this.storeAs.GetValue(slate), PawnUtility.PawnKindsToLineList(pawnKinds, "  - "), false);
		}

		
		[NoTranslate]
		public SlateRef<string> storeAs;

		
		public SlateRef<Faction> faction;

		
		public SlateRef<float> points;
	}
}
