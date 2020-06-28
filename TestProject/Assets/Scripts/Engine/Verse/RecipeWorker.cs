using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x0200009D RID: 157
	public class RecipeWorker
	{
		// Token: 0x0600050E RID: 1294 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool AvailableOnNow(Thing thing)
		{
			return true;
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x000199F3 File Offset: 0x00017BF3
		public virtual IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			yield break;
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x000199FC File Offset: 0x00017BFC
		public virtual bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			return (pawn.Faction != billDoerFaction || pawn.IsQuestLodger()) && this.recipe.isViolation;
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00019A1C File Offset: 0x00017C1C
		public virtual string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			return this.recipe.label;
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00019A29 File Offset: 0x00017C29
		public virtual void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
			ingredient.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_IterationCompleted(Pawn billDoer, List<Thing> ingredients)
		{
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void CheckForWarnings(Pawn billDoer)
		{
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x00019A34 File Offset: 0x00017C34
		protected void ReportViolation(Pawn pawn, Pawn billDoer, Faction factionToInform, int goodwillImpact, string reason)
		{
			if (factionToInform != null && billDoer != null && billDoer.Faction != null)
			{
				factionToInform.TryAffectGoodwillWith(billDoer.Faction, goodwillImpact, true, true, reason, new GlobalTargetInfo?(pawn));
				QuestUtility.SendQuestTargetSignals(pawn.questTags, "SurgeryViolation", pawn.Named("SUBJECT"));
			}
		}

		// Token: 0x04000300 RID: 768
		public RecipeDef recipe;
	}
}
