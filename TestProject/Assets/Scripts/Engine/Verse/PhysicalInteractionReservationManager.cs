using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000057 RID: 87
	public class PhysicalInteractionReservationManager : IExposable
	{
		// Token: 0x060003D0 RID: 976 RVA: 0x000138A0 File Offset: 0x00011AA0
		public void Reserve(Pawn claimant, Job job, LocalTargetInfo target)
		{
			if (this.IsReservedBy(claimant, target))
			{
				return;
			}
			PhysicalInteractionReservationManager.PhysicalInteractionReservation physicalInteractionReservation = new PhysicalInteractionReservationManager.PhysicalInteractionReservation();
			physicalInteractionReservation.target = target;
			physicalInteractionReservation.claimant = claimant;
			physicalInteractionReservation.job = job;
			this.reservations.Add(physicalInteractionReservation);
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x000138E0 File Offset: 0x00011AE0
		public void Release(Pawn claimant, Job job, LocalTargetInfo target)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				PhysicalInteractionReservationManager.PhysicalInteractionReservation physicalInteractionReservation = this.reservations[i];
				if (physicalInteractionReservation.target == target && physicalInteractionReservation.claimant == claimant && physicalInteractionReservation.job == job)
				{
					this.reservations.RemoveAt(i);
					return;
				}
			}
			Log.Warning(string.Concat(new object[]
			{
				claimant,
				" tried to release reservation on target ",
				target,
				", but it's not reserved by him."
			}), false);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0001396C File Offset: 0x00011B6C
		public bool IsReservedBy(Pawn claimant, LocalTargetInfo target)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				PhysicalInteractionReservationManager.PhysicalInteractionReservation physicalInteractionReservation = this.reservations[i];
				if (physicalInteractionReservation.target == target && physicalInteractionReservation.claimant == claimant)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x000139B6 File Offset: 0x00011BB6
		public bool IsReserved(LocalTargetInfo target)
		{
			return this.FirstReserverOf(target) != null;
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x000139C4 File Offset: 0x00011BC4
		public Pawn FirstReserverOf(LocalTargetInfo target)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				PhysicalInteractionReservationManager.PhysicalInteractionReservation physicalInteractionReservation = this.reservations[i];
				if (physicalInteractionReservation.target == target)
				{
					return physicalInteractionReservation.claimant;
				}
			}
			return null;
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00013A0C File Offset: 0x00011C0C
		public LocalTargetInfo FirstReservationFor(Pawn claimant)
		{
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].claimant == claimant)
				{
					return this.reservations[i].target;
				}
			}
			return LocalTargetInfo.Invalid;
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x00013A5C File Offset: 0x00011C5C
		public void ReleaseAllForTarget(LocalTargetInfo target)
		{
			this.reservations.RemoveAll((PhysicalInteractionReservationManager.PhysicalInteractionReservation x) => x.target == target);
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00013A90 File Offset: 0x00011C90
		public void ReleaseClaimedBy(Pawn claimant, Job job)
		{
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].claimant == claimant && this.reservations[i].job == job)
				{
					this.reservations.RemoveAt(i);
				}
			}
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00013AEC File Offset: 0x00011CEC
		public void ReleaseAllClaimedBy(Pawn claimant)
		{
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].claimant == claimant)
				{
					this.reservations.RemoveAt(i);
				}
			}
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00013B34 File Offset: 0x00011D34
		public void ExposeData()
		{
			Scribe_Collections.Look<PhysicalInteractionReservationManager.PhysicalInteractionReservation>(ref this.reservations, "reservations", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.reservations.RemoveAll((PhysicalInteractionReservationManager.PhysicalInteractionReservation x) => x.claimant.DestroyedOrNull()) != 0)
				{
					Log.Warning("Some physical interaction reservations had null or destroyed claimant.", false);
				}
			}
		}

		// Token: 0x04000119 RID: 281
		private List<PhysicalInteractionReservationManager.PhysicalInteractionReservation> reservations = new List<PhysicalInteractionReservationManager.PhysicalInteractionReservation>();

		// Token: 0x0200131D RID: 4893
		public class PhysicalInteractionReservation : IExposable
		{
			// Token: 0x060073C4 RID: 29636 RVA: 0x00282D27 File Offset: 0x00280F27
			public void ExposeData()
			{
				Scribe_TargetInfo.Look(ref this.target, "target");
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
				Scribe_References.Look<Job>(ref this.job, "job", false);
			}

			// Token: 0x04004847 RID: 18503
			public LocalTargetInfo target;

			// Token: 0x04004848 RID: 18504
			public Pawn claimant;

			// Token: 0x04004849 RID: 18505
			public Job job;
		}
	}
}
