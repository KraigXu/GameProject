using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000584 RID: 1412
	[StaticConstructorOnStartup]
	public sealed class ReservationManager : IExposable
	{
		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x0600282D RID: 10285 RVA: 0x000ED660 File Offset: 0x000EB860
		public List<ReservationManager.Reservation> ReservationsReadOnly
		{
			get
			{
				return this.reservations;
			}
		}

		// Token: 0x0600282E RID: 10286 RVA: 0x000ED668 File Offset: 0x000EB868
		public ReservationManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600282F RID: 10287 RVA: 0x000ED684 File Offset: 0x000EB884
		public void ExposeData()
		{
			Scribe_Collections.Look<ReservationManager.Reservation>(ref this.reservations, "reservations", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int i = this.reservations.Count - 1; i >= 0; i--)
				{
					ReservationManager.Reservation reservation = this.reservations[i];
					if (reservation.Target.Thing != null && reservation.Target.Thing.Destroyed)
					{
						Log.Error("Loaded reservation with destroyed target: " + reservation + ". Deleting it...", false);
						this.reservations.Remove(reservation);
					}
					if (reservation.Claimant != null && reservation.Claimant.Destroyed)
					{
						Log.Error("Loaded reservation with destroyed claimant: " + reservation + ". Deleting it...", false);
						this.reservations.Remove(reservation);
					}
					if (reservation.Claimant == null)
					{
						Log.Error("Loaded reservation with null claimant: " + reservation + ". Deleting it...", false);
						this.reservations.Remove(reservation);
					}
					if (reservation.Job == null)
					{
						Log.Error("Loaded reservation with null job: " + reservation + ". Deleting it...", false);
						this.reservations.Remove(reservation);
					}
				}
			}
		}

		// Token: 0x06002830 RID: 10288 RVA: 0x000ED7B4 File Offset: 0x000EB9B4
		public bool CanReserve(Pawn claimant, LocalTargetInfo target, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			if (claimant == null)
			{
				Log.Error("CanReserve with null claimant", false);
				return false;
			}
			if (!claimant.Spawned || claimant.Map != this.map)
			{
				return false;
			}
			if (!target.IsValid || target.ThingDestroyed)
			{
				return false;
			}
			if (target.HasThing && target.Thing.SpawnedOrAnyParentSpawned && target.Thing.MapHeld != this.map)
			{
				return false;
			}
			int num = target.HasThing ? target.Thing.stackCount : 1;
			int num2 = (stackCount == -1) ? num : stackCount;
			if (num2 > num)
			{
				return false;
			}
			if (!ignoreOtherReservations)
			{
				if (this.map.physicalInteractionReservationManager.IsReserved(target) && !this.map.physicalInteractionReservationManager.IsReservedBy(claimant, target))
				{
					return false;
				}
				for (int i = 0; i < this.reservations.Count; i++)
				{
					ReservationManager.Reservation reservation = this.reservations[i];
					if (reservation.Target == target && reservation.Layer == layer && reservation.Claimant == claimant && (reservation.StackCount == -1 || reservation.StackCount >= num2))
					{
						return true;
					}
				}
				int num3 = 0;
				int num4 = 0;
				for (int j = 0; j < this.reservations.Count; j++)
				{
					ReservationManager.Reservation reservation2 = this.reservations[j];
					if (!(reservation2.Target != target) && reservation2.Layer == layer && reservation2.Claimant != claimant && ReservationManager.RespectsReservationsOf(claimant, reservation2.Claimant))
					{
						if (reservation2.MaxPawns != maxPawns)
						{
							return false;
						}
						num3++;
						if (reservation2.StackCount == -1)
						{
							num4 += num;
						}
						else
						{
							num4 += reservation2.StackCount;
						}
						if (num3 >= maxPawns || num2 + num4 > num)
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06002831 RID: 10289 RVA: 0x000ED984 File Offset: 0x000EBB84
		public int CanReserveStack(Pawn claimant, LocalTargetInfo target, int maxPawns = 1, ReservationLayerDef layer = null, bool ignoreOtherReservations = false)
		{
			if (claimant == null)
			{
				Log.Error("CanReserve with null claimant", false);
				return 0;
			}
			if (!claimant.Spawned || claimant.Map != this.map)
			{
				return 0;
			}
			if (!target.IsValid || target.ThingDestroyed)
			{
				return 0;
			}
			if (target.HasThing && target.Thing.SpawnedOrAnyParentSpawned && target.Thing.MapHeld != this.map)
			{
				return 0;
			}
			int num = target.HasThing ? target.Thing.stackCount : 1;
			int num2 = 0;
			if (!ignoreOtherReservations)
			{
				if (this.map.physicalInteractionReservationManager.IsReserved(target) && !this.map.physicalInteractionReservationManager.IsReservedBy(claimant, target))
				{
					return 0;
				}
				int num3 = 0;
				for (int i = 0; i < this.reservations.Count; i++)
				{
					ReservationManager.Reservation reservation = this.reservations[i];
					if (!(reservation.Target != target) && reservation.Layer == layer && reservation.Claimant != claimant && ReservationManager.RespectsReservationsOf(claimant, reservation.Claimant))
					{
						if (reservation.MaxPawns != maxPawns)
						{
							return 0;
						}
						num3++;
						if (reservation.StackCount == -1)
						{
							num2 += num;
						}
						else
						{
							num2 += reservation.StackCount;
						}
						if (num3 >= maxPawns || num2 >= num)
						{
							return 0;
						}
					}
				}
			}
			return Mathf.Max(num - num2, 0);
		}

		// Token: 0x06002832 RID: 10290 RVA: 0x000EDAE0 File Offset: 0x000EBCE0
		public bool Reserve(Pawn claimant, Job job, LocalTargetInfo target, int maxPawns = 1, int stackCount = -1, ReservationLayerDef layer = null, bool errorOnFailed = true)
		{
			if (maxPawns > 1 && stackCount == -1)
			{
				Log.ErrorOnce("Reserving with maxPawns > 1 and stackCount = All; this will not have a useful effect (suppressing future warnings)", 83269, false);
			}
			if (job == null)
			{
				Log.Warning(claimant.ToStringSafe<Pawn>() + " tried to reserve thing " + target.ToStringSafe<LocalTargetInfo>() + " without a valid job", false);
				return false;
			}
			int num = target.HasThing ? target.Thing.stackCount : 1;
			int num2 = (stackCount == -1) ? num : stackCount;
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (reservation.Target == target && reservation.Claimant == claimant && reservation.Job == job && reservation.Layer == layer && (reservation.StackCount == -1 || reservation.StackCount >= num2))
				{
					return true;
				}
			}
			if (!target.IsValid || target.ThingDestroyed)
			{
				return false;
			}
			if (this.CanReserve(claimant, target, maxPawns, stackCount, layer, false))
			{
				this.reservations.Add(new ReservationManager.Reservation(claimant, job, maxPawns, stackCount, target, layer));
				return true;
			}
			if (job != null && job.playerForced && this.CanReserve(claimant, target, maxPawns, stackCount, layer, true))
			{
				this.reservations.Add(new ReservationManager.Reservation(claimant, job, maxPawns, stackCount, target, layer));
				foreach (ReservationManager.Reservation reservation2 in this.reservations.ToList<ReservationManager.Reservation>())
				{
					if (reservation2.Target == target && reservation2.Claimant != claimant && reservation2.Layer == layer && ReservationManager.RespectsReservationsOf(claimant, reservation2.Claimant))
					{
						reservation2.Claimant.jobs.EndCurrentOrQueuedJob(reservation2.Job, JobCondition.InterruptForced, true);
					}
				}
				return true;
			}
			if (errorOnFailed)
			{
				this.LogCouldNotReserveError(claimant, job, target, maxPawns, stackCount, layer);
			}
			return false;
		}

		// Token: 0x06002833 RID: 10291 RVA: 0x000EDCDC File Offset: 0x000EBEDC
		public void Release(LocalTargetInfo target, Pawn claimant, Job job)
		{
			if (target.ThingDestroyed)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Releasing destroyed thing ",
					target,
					" for ",
					claimant
				}), false);
			}
			ReservationManager.Reservation reservation = null;
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation2 = this.reservations[i];
				if (reservation2.Target == target && reservation2.Claimant == claimant && reservation2.Job == job)
				{
					reservation = reservation2;
					break;
				}
			}
			if (reservation == null && !target.ThingDestroyed)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to release ",
					target,
					" that wasn't reserved by ",
					claimant,
					"."
				}), false);
				return;
			}
			this.reservations.Remove(reservation);
		}

		// Token: 0x06002834 RID: 10292 RVA: 0x000EDDBC File Offset: 0x000EBFBC
		public void ReleaseAllForTarget(Thing t)
		{
			if (t == null)
			{
				return;
			}
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].Target.Thing == t)
				{
					this.reservations.RemoveAt(i);
				}
			}
		}

		// Token: 0x06002835 RID: 10293 RVA: 0x000EDE10 File Offset: 0x000EC010
		public void ReleaseClaimedBy(Pawn claimant, Job job)
		{
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].Claimant == claimant && this.reservations[i].Job == job)
				{
					this.reservations.RemoveAt(i);
				}
			}
		}

		// Token: 0x06002836 RID: 10294 RVA: 0x000EDE6C File Offset: 0x000EC06C
		public void ReleaseAllClaimedBy(Pawn claimant)
		{
			if (claimant == null)
			{
				return;
			}
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].Claimant == claimant)
				{
					this.reservations.RemoveAt(i);
				}
			}
		}

		// Token: 0x06002837 RID: 10295 RVA: 0x000EDEB8 File Offset: 0x000EC0B8
		public LocalTargetInfo FirstReservationFor(Pawn claimant)
		{
			if (claimant == null)
			{
				return LocalTargetInfo.Invalid;
			}
			for (int i = 0; i < this.reservations.Count; i++)
			{
				if (this.reservations[i].Claimant == claimant)
				{
					return this.reservations[i].Target;
				}
			}
			return LocalTargetInfo.Invalid;
		}

		// Token: 0x06002838 RID: 10296 RVA: 0x000EDF10 File Offset: 0x000EC110
		public bool IsReservedByAnyoneOf(LocalTargetInfo target, Faction faction)
		{
			if (!target.IsValid)
			{
				return false;
			}
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (reservation.Target == target && reservation.Claimant.Faction == faction)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002839 RID: 10297 RVA: 0x000EDF6A File Offset: 0x000EC16A
		public bool IsReservedAndRespected(LocalTargetInfo target, Pawn claimant)
		{
			return this.FirstRespectedReserver(target, claimant) != null;
		}

		// Token: 0x0600283A RID: 10298 RVA: 0x000EDF78 File Offset: 0x000EC178
		public Pawn FirstRespectedReserver(LocalTargetInfo target, Pawn claimant)
		{
			if (!target.IsValid)
			{
				return null;
			}
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (reservation.Target == target && ReservationManager.RespectsReservationsOf(claimant, reservation.Claimant))
				{
					return reservation.Claimant;
				}
			}
			return null;
		}

		// Token: 0x0600283B RID: 10299 RVA: 0x000EDFD8 File Offset: 0x000EC1D8
		public bool ReservedBy(LocalTargetInfo target, Pawn claimant, Job job = null)
		{
			if (!target.IsValid)
			{
				return false;
			}
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (reservation.Target == target && reservation.Claimant == claimant && (job == null || reservation.Job == job))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600283C RID: 10300 RVA: 0x000EE03C File Offset: 0x000EC23C
		public bool ReservedBy<TDriver>(LocalTargetInfo target, Pawn claimant, LocalTargetInfo? targetAIsNot = null, LocalTargetInfo? targetBIsNot = null, LocalTargetInfo? targetCIsNot = null)
		{
			if (!target.IsValid)
			{
				return false;
			}
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (reservation.Target == target && reservation.Claimant == claimant && reservation.Job != null && reservation.Job.GetCachedDriver(claimant) is TDriver && (targetAIsNot == null || reservation.Job.targetA != targetAIsNot) && (targetBIsNot == null || reservation.Job.targetB != targetBIsNot) && (targetCIsNot == null || reservation.Job.targetC != targetCIsNot))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600283D RID: 10301 RVA: 0x000EE156 File Offset: 0x000EC356
		public IEnumerable<Thing> AllReservedThings()
		{
			return from res in this.reservations
			select res.Target.Thing;
		}

		// Token: 0x0600283E RID: 10302 RVA: 0x000EE184 File Offset: 0x000EC384
		private static bool RespectsReservationsOf(Pawn newClaimant, Pawn oldClaimant)
		{
			if (newClaimant == oldClaimant)
			{
				return true;
			}
			if (newClaimant.Faction == null || oldClaimant.Faction == null)
			{
				return false;
			}
			if (newClaimant.Faction == oldClaimant.Faction)
			{
				return true;
			}
			if (!newClaimant.Faction.HostileTo(oldClaimant.Faction))
			{
				return true;
			}
			if (oldClaimant.HostFaction != null && oldClaimant.HostFaction == newClaimant.HostFaction)
			{
				return true;
			}
			if (newClaimant.HostFaction != null)
			{
				if (oldClaimant.HostFaction != null)
				{
					return true;
				}
				if (newClaimant.HostFaction == oldClaimant.Faction)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600283F RID: 10303 RVA: 0x000EE20C File Offset: 0x000EC40C
		internal string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("All reservation in ReservationManager:");
			for (int i = 0; i < this.reservations.Count; i++)
			{
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"[",
					i,
					"] ",
					this.reservations[i].ToString()
				}));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002840 RID: 10304 RVA: 0x000EE288 File Offset: 0x000EC488
		internal void DebugDrawReservations()
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				ReservationManager.Reservation reservation = this.reservations[i];
				if (reservation.Target.Thing != null)
				{
					if (reservation.Target.Thing.Spawned)
					{
						Thing thing = reservation.Target.Thing;
						Vector3 s = new Vector3((float)thing.RotatedSize.x, 1f, (float)thing.RotatedSize.z);
						Matrix4x4 matrix = default(Matrix4x4);
						matrix.SetTRS(thing.DrawPos + Vector3.up * 0.1f, Quaternion.identity, s);
						Graphics.DrawMesh(MeshPool.plane10, matrix, ReservationManager.DebugReservedThingIcon, 0);
						GenDraw.DrawLineBetween(reservation.Claimant.DrawPos, reservation.Target.Thing.DrawPos);
					}
					else
					{
						Graphics.DrawMesh(MeshPool.plane03, reservation.Claimant.DrawPos + Vector3.up + new Vector3(0.5f, 0f, 0.5f), Quaternion.identity, ReservationManager.DebugReservedThingIcon, 0);
					}
				}
				else
				{
					Graphics.DrawMesh(MeshPool.plane10, reservation.Target.Cell.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays), Quaternion.identity, ReservationManager.DebugReservedThingIcon, 0);
					GenDraw.DrawLineBetween(reservation.Claimant.DrawPos, reservation.Target.Cell.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays));
				}
			}
		}

		// Token: 0x06002841 RID: 10305 RVA: 0x000EE424 File Offset: 0x000EC624
		private void LogCouldNotReserveError(Pawn claimant, Job job, LocalTargetInfo target, int maxPawns, int stackCount, ReservationLayerDef layer)
		{
			Job curJob = claimant.CurJob;
			string text = "null";
			int num = -1;
			if (curJob != null)
			{
				text = curJob.ToString();
				if (claimant.jobs.curDriver != null)
				{
					num = claimant.jobs.curDriver.CurToilIndex;
				}
			}
			string text2;
			if (target.HasThing && target.Thing.def.stackLimit != 1)
			{
				text2 = "(current stack count: " + target.Thing.stackCount + ")";
			}
			else
			{
				text2 = "";
			}
			string text3 = string.Concat(new object[]
			{
				"Could not reserve ",
				target.ToStringSafe<LocalTargetInfo>(),
				text2,
				" (layer: ",
				layer.ToStringSafe<ReservationLayerDef>(),
				") for ",
				claimant.ToStringSafe<Pawn>(),
				" for job ",
				job.ToStringSafe<Job>(),
				" (now doing job ",
				text,
				"(curToil=",
				num,
				")) for maxPawns ",
				maxPawns,
				" and stackCount ",
				stackCount,
				"."
			});
			Pawn pawn = this.FirstRespectedReserver(target, claimant);
			if (pawn != null)
			{
				string text4 = "null";
				int num2 = -1;
				Job curJob2 = pawn.CurJob;
				if (curJob2 != null)
				{
					text4 = curJob2.ToStringSafe<Job>();
					if (pawn.jobs.curDriver != null)
					{
						num2 = pawn.jobs.curDriver.CurToilIndex;
					}
				}
				text3 = string.Concat(new object[]
				{
					text3,
					" Existing reserver: ",
					pawn.ToStringSafe<Pawn>(),
					" doing job ",
					text4,
					" (toilIndex=",
					num2,
					")"
				});
			}
			else
			{
				text3 += " No existing reserver.";
			}
			Pawn pawn2 = this.map.physicalInteractionReservationManager.FirstReserverOf(target);
			if (pawn2 != null)
			{
				text3 = text3 + " Physical interaction reserver: " + pawn2.ToStringSafe<Pawn>();
			}
			Log.Error(text3, false);
		}

		// Token: 0x04001837 RID: 6199
		private Map map;

		// Token: 0x04001838 RID: 6200
		private List<ReservationManager.Reservation> reservations = new List<ReservationManager.Reservation>();

		// Token: 0x04001839 RID: 6201
		private static readonly Material DebugReservedThingIcon = MaterialPool.MatFrom("UI/Overlays/ReservedForWork", ShaderDatabase.Cutout);

		// Token: 0x0400183A RID: 6202
		public const int StackCount_All = -1;

		// Token: 0x0200176F RID: 5999
		public class Reservation : IExposable
		{
			// Token: 0x17001560 RID: 5472
			// (get) Token: 0x060087FF RID: 34815 RVA: 0x002BBC3C File Offset: 0x002B9E3C
			public Pawn Claimant
			{
				get
				{
					return this.claimant;
				}
			}

			// Token: 0x17001561 RID: 5473
			// (get) Token: 0x06008800 RID: 34816 RVA: 0x002BBC44 File Offset: 0x002B9E44
			public Job Job
			{
				get
				{
					return this.job;
				}
			}

			// Token: 0x17001562 RID: 5474
			// (get) Token: 0x06008801 RID: 34817 RVA: 0x002BBC4C File Offset: 0x002B9E4C
			public LocalTargetInfo Target
			{
				get
				{
					return this.target;
				}
			}

			// Token: 0x17001563 RID: 5475
			// (get) Token: 0x06008802 RID: 34818 RVA: 0x002BBC54 File Offset: 0x002B9E54
			public ReservationLayerDef Layer
			{
				get
				{
					return this.layer;
				}
			}

			// Token: 0x17001564 RID: 5476
			// (get) Token: 0x06008803 RID: 34819 RVA: 0x002BBC5C File Offset: 0x002B9E5C
			public int MaxPawns
			{
				get
				{
					return this.maxPawns;
				}
			}

			// Token: 0x17001565 RID: 5477
			// (get) Token: 0x06008804 RID: 34820 RVA: 0x002BBC64 File Offset: 0x002B9E64
			public int StackCount
			{
				get
				{
					return this.stackCount;
				}
			}

			// Token: 0x17001566 RID: 5478
			// (get) Token: 0x06008805 RID: 34821 RVA: 0x002BBC6C File Offset: 0x002B9E6C
			public Faction Faction
			{
				get
				{
					return this.claimant.Faction;
				}
			}

			// Token: 0x06008806 RID: 34822 RVA: 0x002BBC79 File Offset: 0x002B9E79
			public Reservation()
			{
			}

			// Token: 0x06008807 RID: 34823 RVA: 0x002BBC88 File Offset: 0x002B9E88
			public Reservation(Pawn claimant, Job job, int maxPawns, int stackCount, LocalTargetInfo target, ReservationLayerDef layer)
			{
				this.claimant = claimant;
				this.job = job;
				this.maxPawns = maxPawns;
				this.stackCount = stackCount;
				this.target = target;
				this.layer = layer;
			}

			// Token: 0x06008808 RID: 34824 RVA: 0x002BBCC4 File Offset: 0x002B9EC4
			public void ExposeData()
			{
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
				Scribe_References.Look<Job>(ref this.job, "job", false);
				Scribe_TargetInfo.Look(ref this.target, "target");
				Scribe_Values.Look<int>(ref this.maxPawns, "maxPawns", 0, false);
				Scribe_Values.Look<int>(ref this.stackCount, "stackCount", 0, false);
				Scribe_Defs.Look<ReservationLayerDef>(ref this.layer, "layer");
			}

			// Token: 0x06008809 RID: 34825 RVA: 0x002BBD38 File Offset: 0x002B9F38
			public override string ToString()
			{
				return string.Concat(new object[]
				{
					(this.claimant != null) ? this.claimant.LabelShort : "null",
					":",
					this.job.ToStringSafe<Job>(),
					", ",
					this.target.ToStringSafe<LocalTargetInfo>(),
					", ",
					this.layer.ToStringSafe<ReservationLayerDef>(),
					", ",
					this.maxPawns,
					", ",
					this.stackCount
				});
			}

			// Token: 0x0400596D RID: 22893
			private Pawn claimant;

			// Token: 0x0400596E RID: 22894
			private Job job;

			// Token: 0x0400596F RID: 22895
			private LocalTargetInfo target;

			// Token: 0x04005970 RID: 22896
			private ReservationLayerDef layer;

			// Token: 0x04005971 RID: 22897
			private int maxPawns;

			// Token: 0x04005972 RID: 22898
			private int stackCount = -1;
		}
	}
}
