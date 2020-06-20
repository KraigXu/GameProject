using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020011AA RID: 4522
	public class QuestNode_GetExampleRaid : QuestNode
	{
		// Token: 0x06006892 RID: 26770 RVA: 0x00243DE5 File Offset: 0x00241FE5
		protected override bool TestRunInt(Slate slate)
		{
			return slate.Exists("map", false);
		}

		// Token: 0x06006893 RID: 26771 RVA: 0x00248268 File Offset: 0x00246468
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

		// Token: 0x040040E7 RID: 16615
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040040E8 RID: 16616
		public SlateRef<Faction> faction;

		// Token: 0x040040E9 RID: 16617
		public SlateRef<float> points;
	}
}
