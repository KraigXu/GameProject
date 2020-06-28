using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000642 RID: 1602
	public class Toils_Tend
	{
		// Token: 0x06002BD7 RID: 11223 RVA: 0x000FBE28 File Offset: 0x000FA028
		public static Toil ReserveMedicine(TargetIndex ind, Pawn injured)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(ind).Thing;
				int num = actor.Map.reservationManager.CanReserveStack(actor, thing, 10, null, false);
				if (num <= 0 || !actor.Reserve(thing, curJob, 10, Mathf.Min(num, Medicine.GetMedicineCountToFullyHeal(injured)), null, true))
				{
					toil.actor.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x06002BD8 RID: 11224 RVA: 0x000FBE8C File Offset: 0x000FA08C
		public static Toil PickupMedicine(TargetIndex ind, Pawn injured)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Thing thing = curJob.GetTarget(ind).Thing;
				int num = Medicine.GetMedicineCountToFullyHeal(injured);
				if (actor.carryTracker.CarriedThing != null)
				{
					num -= actor.carryTracker.CarriedThing.stackCount;
				}
				int num2 = Mathf.Min(actor.Map.reservationManager.CanReserveStack(actor, thing, 10, null, false), num);
				if (num2 > 0)
				{
					actor.carryTracker.TryStartCarry(thing, num2, true);
				}
				curJob.count = num - num2;
				if (thing.Spawned)
				{
					toil.actor.Map.reservationManager.Release(thing, actor, curJob);
				}
				curJob.SetTarget(ind, actor.carryTracker.CarriedThing);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x06002BD9 RID: 11225 RVA: 0x000FBEE4 File Offset: 0x000FA0E4
		public static Toil FinalizeTend(Pawn patient)
		{
			Toil toil = new Toil();
			toil.initAction = delegate
			{
				Pawn actor = toil.actor;
				Medicine medicine = (Medicine)actor.CurJob.targetB.Thing;
				float num = patient.RaceProps.Animal ? 175f : 500f;
				float num2 = (medicine == null) ? 0.5f : medicine.def.MedicineTendXpGainFactor;
				actor.skills.Learn(SkillDefOf.Medicine, num * num2, false);
				TendUtility.DoTend(actor, patient, medicine);
				if (medicine != null && medicine.Destroyed)
				{
					actor.CurJob.SetTarget(TargetIndex.B, LocalTargetInfo.Invalid);
				}
				if (toil.actor.CurJob.endAfterTendedOnce)
				{
					actor.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x040019B6 RID: 6582
		public const int MaxMedicineReservations = 10;
	}
}
