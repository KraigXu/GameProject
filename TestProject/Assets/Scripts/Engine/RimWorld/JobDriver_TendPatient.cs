using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class JobDriver_TendPatient : JobDriver
	{
		
		
		protected Thing MedicineUsed
		{
			get
			{
				return this.job.targetB.Thing;
			}
		}

		
		
		protected Pawn Deliveree
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.usesMedicine, "usesMedicine", false, false);
		}

		
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.usesMedicine = (this.MedicineUsed != null);
		}

		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (this.Deliveree != this.pawn && !this.pawn.Reserve(this.Deliveree, this.job, 1, -1, null, errorOnFailed))
			{
				return false;
			}
			if (this.usesMedicine)
			{
				int num = this.pawn.Map.reservationManager.CanReserveStack(this.pawn, this.MedicineUsed, 10, null, false);
				if (num <= 0 || !this.pawn.Reserve(this.MedicineUsed, this.job, 10, Mathf.Min(num, Medicine.GetMedicineCountToFullyHeal(this.Deliveree)), null, errorOnFailed))
				{
					return false;
				}
			}
			return true;
		}

		
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOn(delegate
			{
				if (!WorkGiver_Tend.GoodLayingStatusForTend(this.Deliveree, this.pawn))
				{
					return true;
				}
				if (this.MedicineUsed != null && this.pawn.Faction == Faction.OfPlayer)
				{
					if (this.Deliveree.playerSettings == null)
					{
						return true;
					}
					if (!this.Deliveree.playerSettings.medCare.AllowsMedicine(this.MedicineUsed.def))
					{
						return true;
					}
				}
				return this.pawn == this.Deliveree && this.pawn.Faction == Faction.OfPlayer && !this.pawn.playerSettings.selfTend;
			});
			base.AddEndCondition(delegate
			{
				if (this.pawn.Faction == Faction.OfPlayer && HealthAIUtility.ShouldBeTendedNowByPlayer(this.Deliveree))
				{
					return JobCondition.Ongoing;
				}
				if (this.pawn.Faction != Faction.OfPlayer && this.Deliveree.health.HasHediffsNeedingTend(false))
				{
					return JobCondition.Ongoing;
				}
				return JobCondition.Succeeded;
			});
			this.FailOnAggroMentalState(TargetIndex.A);
			Toil reserveMedicine = null;
			if (this.usesMedicine)
			{
				reserveMedicine = Toils_Tend.ReserveMedicine(TargetIndex.B, this.Deliveree).FailOnDespawnedNullOrForbidden(TargetIndex.B);
				yield return reserveMedicine;
				yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B);
				yield return Toils_Tend.PickupMedicine(TargetIndex.B, this.Deliveree).FailOnDestroyedOrNull(TargetIndex.B);
				yield return Toils_Haul.CheckForGetOpportunityDuplicate(reserveMedicine, TargetIndex.B, TargetIndex.None, true, null);
			}
			PathEndMode interactionCell = (this.Deliveree == this.pawn) ? PathEndMode.OnCell : PathEndMode.InteractionCell;
			Toil gotoToil = Toils_Goto.GotoThing(TargetIndex.A, interactionCell);
			yield return gotoToil;
			Toil toil = Toils_General.Wait((int)(1f / this.pawn.GetStatValue(StatDefOf.MedicalTendSpeed, true) * 600f), TargetIndex.None).FailOnCannotTouch(TargetIndex.A, interactionCell).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f).PlaySustainerOrSound(SoundDefOf.Interact_Tend);
			toil.activeSkill = (() => SkillDefOf.Medicine);
			if (this.pawn == this.Deliveree && this.pawn.Faction != Faction.OfPlayer)
			{
				toil.tickAction = delegate
				{
					if (this.pawn.IsHashIntervalTick(100) && !this.pawn.Position.Fogged(this.pawn.Map))
					{
						MoteMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, ThingDefOf.Mote_HealingCross);
					}
				};
			}
			yield return toil;
			yield return Toils_Tend.FinalizeTend(this.Deliveree);
			if (this.usesMedicine)
			{
				yield return new Toil
				{
					initAction = delegate
					{
						if (this.MedicineUsed.DestroyedOrNull())
						{
							Thing thing = HealthAIUtility.FindBestMedicine(this.pawn, this.Deliveree);
							if (thing != null)
							{
								this.job.targetB = thing;
								this.JumpToToil(reserveMedicine);
							}
						}
					}
				};
			}
			yield return Toils_Jump.Jump(gotoToil);
			yield break;
		}

		
		public override void Notify_DamageTaken(DamageInfo dinfo)
		{
			base.Notify_DamageTaken(dinfo);
			if (dinfo.Def.ExternalViolenceFor(this.pawn) && this.pawn.Faction != Faction.OfPlayer && this.pawn == this.Deliveree)
			{
				this.pawn.jobs.CheckForJobOverride();
			}
		}

		
		private bool usesMedicine;

		
		private const int BaseTendDuration = 600;

		
		private const int TicksBetweenSelfTendMotes = 100;
	}
}
