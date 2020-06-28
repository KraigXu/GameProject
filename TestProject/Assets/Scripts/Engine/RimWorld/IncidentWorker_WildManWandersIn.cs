using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009F6 RID: 2550
	public class IncidentWorker_WildManWandersIn : IncidentWorker
	{
		// Token: 0x06003C9E RID: 15518 RVA: 0x00140508 File Offset: 0x0013E708
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Faction faction;
			if (!this.TryFindFormerFaction(out faction))
			{
				return false;
			}
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return !map.GameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout) && map.mapTemperature.SeasonAcceptableFor(ThingDefOf.Human) && this.TryFindEntryCell(map, out intVec);
		}

		// Token: 0x06003C9F RID: 15519 RVA: 0x0014056C File Offset: 0x0013E76C
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 loc;
			if (!this.TryFindEntryCell(map, out loc))
			{
				return false;
			}
			Faction faction;
			if (!this.TryFindFormerFaction(out faction))
			{
				return false;
			}
			Pawn pawn = PawnGenerator.GeneratePawn(PawnKindDefOf.WildMan, faction);
			pawn.SetFaction(null, null);
			GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
			TaggedString baseLetterLabel = this.def.letterLabel.Formatted(pawn.LabelShort, pawn.Named("PAWN")).CapitalizeFirst();
			TaggedString baseLetterText = this.def.letterText.Formatted(pawn.NameShortColored, pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true).CapitalizeFirst();
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref baseLetterText, ref baseLetterLabel, pawn);
			base.SendStandardLetter(baseLetterLabel, baseLetterText, this.def.letterDef, parms, pawn, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x06003CA0 RID: 15520 RVA: 0x0014065C File Offset: 0x0013E85C
		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c), map, CellFinder.EdgeRoadChance_Ignore, out cell);
		}

		// Token: 0x06003CA1 RID: 15521 RVA: 0x00140693 File Offset: 0x0013E893
		private bool TryFindFormerFaction(out Faction formerFaction)
		{
			return Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out formerFaction, false, true, TechLevel.Undefined);
		}
	}
}
