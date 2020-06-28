using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200068B RID: 1675
	public class JobDriver_FoodFeedPatient : JobDriver
	{
		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x06002D80 RID: 11648 RVA: 0x000DF68D File Offset: 0x000DD88D
		protected Thing Food
		{
			get
			{
				return this.job.targetA.Thing;
			}
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x06002D81 RID: 11649 RVA: 0x001004A2 File Offset: 0x000FE6A2
		protected Pawn Deliveree
		{
			get
			{
				return (Pawn)this.job.targetB.Thing;
			}
		}

		// Token: 0x06002D82 RID: 11650 RVA: 0x001005E8 File Offset: 0x000FE7E8
		public override string GetReport()
		{
			if (this.job.GetTarget(TargetIndex.A).Thing is Building_NutrientPasteDispenser && this.Deliveree != null)
			{
				return JobUtility.GetResolvedJobReportRaw(this.job.def.reportString, ThingDefOf.MealNutrientPaste.label, ThingDefOf.MealNutrientPaste, this.Deliveree.LabelShort, this.Deliveree, "", "");
			}
			return base.GetReport();
		}

		// Token: 0x06002D83 RID: 11651 RVA: 0x00100660 File Offset: 0x000FE860
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (!this.pawn.Reserve(this.Deliveree, this.job, 1, -1, null, errorOnFailed))
			{
				return false;
			}
			if (!(base.TargetThingA is Building_NutrientPasteDispenser) && (this.pawn.inventory == null || !this.pawn.inventory.Contains(base.TargetThingA)))
			{
				int maxAmountToPickup = FoodUtility.GetMaxAmountToPickup(this.Food, this.pawn, this.job.count);
				if (!this.pawn.Reserve(this.Food, this.job, 10, maxAmountToPickup, null, errorOnFailed))
				{
					return false;
				}
				this.job.count = maxAmountToPickup;
			}
			return true;
		}

		// Token: 0x06002D84 RID: 11652 RVA: 0x00100712 File Offset: 0x000FE912
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.B);
			this.FailOn(() => !FoodUtility.ShouldBeFedBySomeone(this.Deliveree));
			if (this.pawn.inventory != null && this.pawn.inventory.Contains(base.TargetThingA))
			{
				yield return Toils_Misc.TakeItemFromInventoryToCarrier(this.pawn, TargetIndex.A);
			}
			else if (base.TargetThingA is Building_NutrientPasteDispenser)
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnForbidden(TargetIndex.A);
				yield return Toils_Ingest.TakeMealFromDispenser(TargetIndex.A, this.pawn);
			}
			else
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnForbidden(TargetIndex.A);
				yield return Toils_Ingest.PickupIngestible(TargetIndex.A, this.Deliveree);
			}
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch);
			yield return Toils_Ingest.ChewIngestible(this.Deliveree, 1.5f, TargetIndex.A, TargetIndex.None).FailOnCannotTouch(TargetIndex.B, PathEndMode.Touch);
			yield return Toils_Ingest.FinalizeIngest(this.Deliveree, TargetIndex.A);
			yield break;
		}

		// Token: 0x04001A26 RID: 6694
		private const TargetIndex FoodSourceInd = TargetIndex.A;

		// Token: 0x04001A27 RID: 6695
		private const TargetIndex DelivereeInd = TargetIndex.B;

		// Token: 0x04001A28 RID: 6696
		private const float FeedDurationMultiplier = 1.5f;
	}
}
