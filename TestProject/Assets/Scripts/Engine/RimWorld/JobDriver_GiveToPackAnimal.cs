using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000628 RID: 1576
	public class JobDriver_GiveToPackAnimal : JobDriver
	{
		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x06002B1A RID: 11034 RVA: 0x000FA5CC File Offset: 0x000F87CC
		private Thing Item
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x06002B1B RID: 11035 RVA: 0x000FA5F0 File Offset: 0x000F87F0
		private Pawn Animal
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06002B1C RID: 11036 RVA: 0x000FA616 File Offset: 0x000F8816
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Item, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06002B1D RID: 11037 RVA: 0x000FA638 File Offset: 0x000F8838
		protected override IEnumerable<Toil> MakeNewToils()
		{
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false);
			Toil findNearestCarrier = this.FindCarrierToil();
			yield return findNearestCarrier;
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.B).JumpIf(() => !this.CanCarryAtLeastOne(this.Animal), findNearestCarrier);
			yield return this.GiveToCarrierAsMuchAsPossibleToil();
			yield return Toils_Jump.JumpIf(findNearestCarrier, () => this.pawn.carryTracker.CarriedThing != null);
			yield break;
		}

		// Token: 0x06002B1E RID: 11038 RVA: 0x000FA648 File Offset: 0x000F8848
		private Toil FindCarrierToil()
		{
			return new Toil
			{
				initAction = delegate
				{
					Pawn pawn = this.FindCarrier();
					if (pawn == null)
					{
						this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
						return;
					}
					this.job.SetTarget(TargetIndex.B, pawn);
				}
			};
		}

		// Token: 0x06002B1F RID: 11039 RVA: 0x000FA664 File Offset: 0x000F8864
		private Pawn FindCarrier()
		{
			IEnumerable<Pawn> enumerable = GiveToPackAnimalUtility.CarrierCandidatesFor(this.pawn);
			Pawn animal = this.Animal;
			if (animal != null && enumerable.Contains(animal) && animal.RaceProps.packAnimal && this.CanCarryAtLeastOne(animal))
			{
				return animal;
			}
			Pawn pawn = null;
			float num = -1f;
			foreach (Pawn pawn2 in enumerable)
			{
				if (pawn2.RaceProps.packAnimal && this.CanCarryAtLeastOne(pawn2))
				{
					float num2 = (float)pawn2.Position.DistanceToSquared(this.pawn.Position);
					if (pawn == null || num2 < num)
					{
						pawn = pawn2;
						num = num2;
					}
				}
			}
			return pawn;
		}

		// Token: 0x06002B20 RID: 11040 RVA: 0x000FA72C File Offset: 0x000F892C
		private bool CanCarryAtLeastOne(Pawn carrier)
		{
			return !MassUtility.WillBeOverEncumberedAfterPickingUp(carrier, this.Item, 1);
		}

		// Token: 0x06002B21 RID: 11041 RVA: 0x000FA73E File Offset: 0x000F893E
		private Toil GiveToCarrierAsMuchAsPossibleToil()
		{
			return new Toil
			{
				initAction = delegate
				{
					if (this.Item == null)
					{
						this.pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
						return;
					}
					int count = Mathf.Min(MassUtility.CountToPickUpUntilOverEncumbered(this.Animal, this.Item), this.Item.stackCount);
					this.pawn.carryTracker.innerContainer.TryTransferToContainer(this.Item, this.Animal.inventory.innerContainer, count, true);
				}
			};
		}

		// Token: 0x04001990 RID: 6544
		private const TargetIndex ItemInd = TargetIndex.A;

		// Token: 0x04001991 RID: 6545
		private const TargetIndex AnimalInd = TargetIndex.B;
	}
}
