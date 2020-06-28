using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000583 RID: 1411
	public class AttackTargetReservationManager : IExposable
	{
		// Token: 0x06002821 RID: 10273 RVA: 0x000ED23D File Offset: 0x000EB43D
		public AttackTargetReservationManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06002822 RID: 10274 RVA: 0x000ED258 File Offset: 0x000EB458
		public void Reserve(Pawn claimant, Job job, IAttackTarget target)
		{
			if (target == null)
			{
				Log.Warning(claimant + " tried to reserve null attack target.", false);
				return;
			}
			if (this.IsReservedBy(claimant, target))
			{
				return;
			}
			AttackTargetReservationManager.AttackTargetReservation attackTargetReservation = new AttackTargetReservationManager.AttackTargetReservation();
			attackTargetReservation.target = target;
			attackTargetReservation.claimant = claimant;
			attackTargetReservation.job = job;
			this.reservations.Add(attackTargetReservation);
		}

		// Token: 0x06002823 RID: 10275 RVA: 0x000ED2AC File Offset: 0x000EB4AC
		public void Release(Pawn claimant, Job job, IAttackTarget target)
		{
			if (target == null)
			{
				Log.Warning(claimant + " tried to release reservation on null attack target.", false);
				return;
			}
			for (int i = 0; i < this.reservations.Count; i++)
			{
				AttackTargetReservationManager.AttackTargetReservation attackTargetReservation = this.reservations[i];
				if (attackTargetReservation.target == target && attackTargetReservation.claimant == claimant && attackTargetReservation.job == job)
				{
					this.reservations.RemoveAt(i);
					return;
				}
			}
			Log.Warning(string.Concat(new object[]
			{
				claimant,
				" with job ",
				job,
				" tried to release reservation on target ",
				target,
				", but it's not reserved by him."
			}), false);
		}

		// Token: 0x06002824 RID: 10276 RVA: 0x000ED350 File Offset: 0x000EB550
		public bool CanReserve(Pawn claimant, IAttackTarget target)
		{
			if (this.IsReservedBy(claimant, target))
			{
				return true;
			}
			int reservationsCount = this.GetReservationsCount(target, claimant.Faction);
			int maxPreferredReservationsCount = this.GetMaxPreferredReservationsCount(target);
			return reservationsCount < maxPreferredReservationsCount;
		}

		// Token: 0x06002825 RID: 10277 RVA: 0x000ED384 File Offset: 0x000EB584
		public bool IsReservedBy(Pawn claimant, IAttackTarget target)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				AttackTargetReservationManager.AttackTargetReservation attackTargetReservation = this.reservations[i];
				if (attackTargetReservation.target == target && attackTargetReservation.claimant == claimant)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002826 RID: 10278 RVA: 0x000ED3CC File Offset: 0x000EB5CC
		public void ReleaseAllForTarget(IAttackTarget target)
		{
			this.reservations.RemoveAll((AttackTargetReservationManager.AttackTargetReservation x) => x.target == target);
		}

		// Token: 0x06002827 RID: 10279 RVA: 0x000ED400 File Offset: 0x000EB600
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

		// Token: 0x06002828 RID: 10280 RVA: 0x000ED45C File Offset: 0x000EB65C
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

		// Token: 0x06002829 RID: 10281 RVA: 0x000ED4A4 File Offset: 0x000EB6A4
		public IAttackTarget FirstReservationFor(Pawn claimant)
		{
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].claimant == claimant)
				{
					return this.reservations[i].target;
				}
			}
			return null;
		}

		// Token: 0x0600282A RID: 10282 RVA: 0x000ED4F0 File Offset: 0x000EB6F0
		public void ExposeData()
		{
			Scribe_Collections.Look<AttackTargetReservationManager.AttackTargetReservation>(ref this.reservations, "reservations", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.reservations.RemoveAll((AttackTargetReservationManager.AttackTargetReservation x) => x.target == null);
				if (this.reservations.RemoveAll((AttackTargetReservationManager.AttackTargetReservation x) => x.claimant.DestroyedOrNull()) != 0)
				{
					Log.Warning("Some attack target reservations had null or destroyed claimant.", false);
				}
			}
		}

		// Token: 0x0600282B RID: 10283 RVA: 0x000ED580 File Offset: 0x000EB780
		private int GetReservationsCount(IAttackTarget target, Faction faction)
		{
			int num = 0;
			for (int i = 0; i < this.reservations.Count; i++)
			{
				AttackTargetReservationManager.AttackTargetReservation attackTargetReservation = this.reservations[i];
				if (attackTargetReservation.target == target && attackTargetReservation.claimant.Faction == faction)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600282C RID: 10284 RVA: 0x000ED5D0 File Offset: 0x000EB7D0
		private int GetMaxPreferredReservationsCount(IAttackTarget target)
		{
			int num = 0;
			CellRect cellRect = target.Thing.OccupiedRect();
			foreach (IntVec3 c in cellRect.ExpandedBy(1))
			{
				if (!cellRect.Contains(c) && c.InBounds(this.map) && c.Standable(this.map))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x04001835 RID: 6197
		private Map map;

		// Token: 0x04001836 RID: 6198
		private List<AttackTargetReservationManager.AttackTargetReservation> reservations = new List<AttackTargetReservationManager.AttackTargetReservation>();

		// Token: 0x0200176C RID: 5996
		public class AttackTargetReservation : IExposable
		{
			// Token: 0x060087F7 RID: 34807 RVA: 0x002BBBD3 File Offset: 0x002B9DD3
			public void ExposeData()
			{
				Scribe_References.Look<IAttackTarget>(ref this.target, "target", false);
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
				Scribe_References.Look<Job>(ref this.job, "job", false);
			}

			// Token: 0x04005966 RID: 22886
			public IAttackTarget target;

			// Token: 0x04005967 RID: 22887
			public Pawn claimant;

			// Token: 0x04005968 RID: 22888
			public Job job;
		}
	}
}
