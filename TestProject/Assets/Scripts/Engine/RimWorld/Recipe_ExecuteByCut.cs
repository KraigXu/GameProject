using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AF8 RID: 2808
	public class Recipe_ExecuteByCut : RecipeWorker
	{
		// Token: 0x0600424E RID: 16974 RVA: 0x00162184 File Offset: 0x00160384
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (this.IsViolationOnPawn(pawn, part, Faction.OfPlayer))
			{
				base.ReportViolation(pawn, billDoer, pawn.FactionOrExtraHomeFaction, -100, "GoodwillChangedReason_EuthanizedPawn".Translate(pawn.Named("PAWN")));
			}
			ExecutionUtility.DoExecutionByCut(billDoer, pawn);
			ThoughtUtility.GiveThoughtsForPawnExecuted(pawn, PawnExecutionKind.GenericHumane);
		}
	}
}
