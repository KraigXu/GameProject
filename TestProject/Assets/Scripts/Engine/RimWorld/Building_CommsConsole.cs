using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C76 RID: 3190
	public class Building_CommsConsole : Building
	{
		// Token: 0x17000D81 RID: 3457
		// (get) Token: 0x06004C84 RID: 19588 RVA: 0x0019ADDE File Offset: 0x00198FDE
		public bool CanUseCommsNow
		{
			get
			{
				return (!base.Spawned || !base.Map.gameConditionManager.ElectricityDisabled) && (this.powerComp == null || this.powerComp.PowerOn);
			}
		}

		// Token: 0x06004C85 RID: 19589 RVA: 0x0019AE11 File Offset: 0x00199011
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.powerComp = base.GetComp<CompPowerTrader>();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.BuildOrbitalTradeBeacon, OpportunityType.GoodToKnow);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.OpeningComms, OpportunityType.GoodToKnow);
		}

		// Token: 0x06004C86 RID: 19590 RVA: 0x0019AE40 File Offset: 0x00199040
		private void UseAct(Pawn myPawn, ICommunicable commTarget)
		{
			Job job = JobMaker.MakeJob(JobDefOf.UseCommsConsole, this);
			job.commTarget = commTarget;
			myPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.OpeningComms, KnowledgeAmount.Total);
		}

		// Token: 0x06004C87 RID: 19591 RVA: 0x0019AE80 File Offset: 0x00199080
		private FloatMenuOption GetFailureReason(Pawn myPawn)
		{
			if (!myPawn.CanReach(this, PathEndMode.InteractionCell, Danger.Some, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotUseNoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			if (base.Spawned && base.Map.gameConditionManager.ElectricityDisabled)
			{
				return new FloatMenuOption("CannotUseSolarFlare".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			if (this.powerComp != null && !this.powerComp.PowerOn)
			{
				return new FloatMenuOption("CannotUseNoPower".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			if (!myPawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking))
			{
				return new FloatMenuOption("CannotUseReason".Translate("IncapableOfCapacity".Translate(PawnCapacityDefOf.Talking.label, myPawn.Named("PAWN"))), null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			if (!this.CanUseCommsNow)
			{
				Log.Error(myPawn + " could not use comm console for unknown reason.", false);
				return new FloatMenuOption("Cannot use now", null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			return null;
		}

		// Token: 0x06004C88 RID: 19592 RVA: 0x0019AFBE File Offset: 0x001991BE
		public IEnumerable<ICommunicable> GetCommTargets(Pawn myPawn)
		{
			return myPawn.Map.passingShipManager.passingShips.Cast<ICommunicable>().Concat(Find.FactionManager.AllFactionsVisibleInViewOrder.Cast<ICommunicable>());
		}

		// Token: 0x06004C89 RID: 19593 RVA: 0x0019AFE9 File Offset: 0x001991E9
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
		{
			FloatMenuOption failureReason = this.GetFailureReason(myPawn);
			if (failureReason != null)
			{
				yield return failureReason;
				yield break;
			}
			foreach (ICommunicable communicable in this.GetCommTargets(myPawn))
			{
				FloatMenuOption floatMenuOption = communicable.CommFloatMenuOption(this, myPawn);
				if (floatMenuOption != null)
				{
					yield return floatMenuOption;
				}
			}
			IEnumerator<ICommunicable> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06004C8A RID: 19594 RVA: 0x0019B000 File Offset: 0x00199200
		public void GiveUseCommsJob(Pawn negotiator, ICommunicable target)
		{
			Job job = JobMaker.MakeJob(JobDefOf.UseCommsConsole, this);
			job.commTarget = target;
			negotiator.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.OpeningComms, KnowledgeAmount.Total);
		}

		// Token: 0x04002B03 RID: 11011
		private CompPowerTrader powerComp;
	}
}
