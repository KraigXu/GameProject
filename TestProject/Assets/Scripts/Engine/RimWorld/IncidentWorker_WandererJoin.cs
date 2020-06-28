using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009F5 RID: 2549
	public class IncidentWorker_WandererJoin : IncidentWorker
	{
		// Token: 0x06003C9A RID: 15514 RVA: 0x00140350 File Offset: 0x0013E550
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return this.TryFindEntryCell(map, out intVec);
		}

		// Token: 0x06003C9B RID: 15515 RVA: 0x00140380 File Offset: 0x0013E580
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 loc;
			if (!this.TryFindEntryCell(map, out loc))
			{
				return false;
			}
			Gender? fixedGender = null;
			if (this.def.pawnFixedGender != Gender.None)
			{
				fixedGender = new Gender?(this.def.pawnFixedGender);
			}
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(this.def.pawnKind, Faction.OfPlayer, PawnGenerationContext.NonPlayer, -1, true, false, false, false, true, this.def.pawnMustBeCapableOfViolence, 20f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, null, null, fixedGender, null, null, null, null));
			GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
			TaggedString baseLetterText = this.def.letterText.Formatted(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			TaggedString baseLetterLabel = this.def.letterLabel.Formatted(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true);
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref baseLetterText, ref baseLetterLabel, pawn);
			base.SendStandardLetter(baseLetterLabel, baseLetterText, LetterDefOf.PositiveEvent, parms, pawn, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x06003C9C RID: 15516 RVA: 0x001404D0 File Offset: 0x0013E6D0
		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c) && !c.Fogged(map), map, CellFinder.EdgeRoadChance_Neutral, out cell);
		}

		// Token: 0x04002393 RID: 9107
		private const float RelationWithColonistWeight = 20f;
	}
}
