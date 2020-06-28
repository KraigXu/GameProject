using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000294 RID: 660
	[StaticConstructorOnStartup]
	public sealed class PawnDestinationReservationManager : IExposable
	{
		// Token: 0x0600123F RID: 4671 RVA: 0x00068028 File Offset: 0x00066228
		public PawnDestinationReservationManager.PawnDestinationSet GetPawnDestinationSetFor(Faction faction)
		{
			if (!this.reservedDestinations.ContainsKey(faction))
			{
				this.reservedDestinations.Add(faction, new PawnDestinationReservationManager.PawnDestinationSet());
			}
			return this.reservedDestinations[faction];
		}

		// Token: 0x06001240 RID: 4672 RVA: 0x00068058 File Offset: 0x00066258
		public void Reserve(Pawn p, Job job, IntVec3 loc)
		{
			if (p.Faction == null)
			{
				return;
			}
			Pawn pawn;
			if (p.Drafted && p.Faction == Faction.OfPlayer && this.IsReserved(loc, out pawn) && pawn != p && !pawn.HostileTo(p) && pawn.Faction != p.Faction && (pawn.mindState == null || pawn.mindState.mentalStateHandler == null || !pawn.mindState.mentalStateHandler.InMentalState || (pawn.mindState.mentalStateHandler.CurStateDef.category != MentalStateCategory.Aggro && pawn.mindState.mentalStateHandler.CurStateDef.category != MentalStateCategory.Malicious)))
			{
				pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
			this.ObsoleteAllClaimedBy(p);
			this.GetPawnDestinationSetFor(p.Faction).list.Add(new PawnDestinationReservationManager.PawnDestinationReservation
			{
				target = loc,
				claimant = p,
				job = job
			});
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x00068150 File Offset: 0x00066350
		public PawnDestinationReservationManager.PawnDestinationReservation MostRecentReservationFor(Pawn p)
		{
			if (p.Faction == null)
			{
				return null;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(p.Faction).list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].claimant == p && !list[i].obsolete)
				{
					return list[i];
				}
			}
			return null;
		}

		// Token: 0x06001242 RID: 4674 RVA: 0x000681B0 File Offset: 0x000663B0
		public IntVec3 FirstObsoleteReservationFor(Pawn p)
		{
			if (p.Faction == null)
			{
				return IntVec3.Invalid;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(p.Faction).list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].claimant == p && list[i].obsolete)
				{
					return list[i].target;
				}
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06001243 RID: 4675 RVA: 0x00068220 File Offset: 0x00066420
		public Job FirstObsoleteReservationJobFor(Pawn p)
		{
			if (p.Faction == null)
			{
				return null;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(p.Faction).list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].claimant == p && list[i].obsolete)
				{
					return list[i].job;
				}
			}
			return null;
		}

		// Token: 0x06001244 RID: 4676 RVA: 0x00068288 File Offset: 0x00066488
		public bool IsReserved(IntVec3 loc)
		{
			Pawn pawn;
			return this.IsReserved(loc, out pawn);
		}

		// Token: 0x06001245 RID: 4677 RVA: 0x000682A0 File Offset: 0x000664A0
		public bool IsReserved(IntVec3 loc, out Pawn claimant)
		{
			foreach (KeyValuePair<Faction, PawnDestinationReservationManager.PawnDestinationSet> keyValuePair in this.reservedDestinations)
			{
				List<PawnDestinationReservationManager.PawnDestinationReservation> list = keyValuePair.Value.list;
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].target == loc)
					{
						claimant = list[i].claimant;
						return true;
					}
				}
			}
			claimant = null;
			return false;
		}

		// Token: 0x06001246 RID: 4678 RVA: 0x00068338 File Offset: 0x00066538
		public bool CanReserve(IntVec3 c, Pawn searcher, bool draftedOnly = false)
		{
			if (searcher.Faction == null)
			{
				return true;
			}
			if (searcher.Faction == Faction.OfPlayer)
			{
				return this.CanReserveInt(c, searcher.Faction, searcher, draftedOnly);
			}
			foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
			{
				if (!faction.HostileTo(searcher.Faction) && !this.CanReserveInt(c, faction, searcher, draftedOnly))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001247 RID: 4679 RVA: 0x000683D0 File Offset: 0x000665D0
		private bool CanReserveInt(IntVec3 c, Faction faction, Pawn ignoreClaimant = null, bool draftedOnly = false)
		{
			if (faction == null)
			{
				return true;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(faction).list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].target == c && (ignoreClaimant == null || list[i].claimant != ignoreClaimant) && (!draftedOnly || list[i].claimant.Drafted))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001248 RID: 4680 RVA: 0x00068440 File Offset: 0x00066640
		public Pawn FirstReserverOf(IntVec3 c, Faction faction)
		{
			if (faction == null)
			{
				return null;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(faction).list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].target == c)
				{
					return list[i].claimant;
				}
			}
			return null;
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x00068494 File Offset: 0x00066694
		public void ReleaseAllObsoleteClaimedBy(Pawn p)
		{
			if (p.Faction == null)
			{
				return;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(p.Faction).list;
			int i = 0;
			while (i < list.Count)
			{
				if (list[i].claimant == p && list[i].obsolete)
				{
					list[i] = list[list.Count - 1];
					list.RemoveLast<PawnDestinationReservationManager.PawnDestinationReservation>();
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x0600124A RID: 4682 RVA: 0x00068508 File Offset: 0x00066708
		public void ReleaseAllClaimedBy(Pawn p)
		{
			if (p.Faction == null)
			{
				return;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(p.Faction).list;
			int i = 0;
			while (i < list.Count)
			{
				if (list[i].claimant == p)
				{
					list[i] = list[list.Count - 1];
					list.RemoveLast<PawnDestinationReservationManager.PawnDestinationReservation>();
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x00068570 File Offset: 0x00066770
		public void ReleaseClaimedBy(Pawn p, Job job)
		{
			if (p.Faction == null)
			{
				return;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(p.Faction).list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].claimant == p && list[i].job == job)
				{
					list[i].job = null;
					if (list[i].obsolete)
					{
						list[i] = list[list.Count - 1];
						list.RemoveLast<PawnDestinationReservationManager.PawnDestinationReservation>();
						i--;
					}
				}
			}
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x00068604 File Offset: 0x00066804
		public void ObsoleteAllClaimedBy(Pawn p)
		{
			if (p.Faction == null)
			{
				return;
			}
			List<PawnDestinationReservationManager.PawnDestinationReservation> list = this.GetPawnDestinationSetFor(p.Faction).list;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].claimant == p)
				{
					list[i].obsolete = true;
					if (list[i].job == null)
					{
						list[i] = list[list.Count - 1];
						list.RemoveLast<PawnDestinationReservationManager.PawnDestinationReservation>();
						i--;
					}
				}
			}
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x00068688 File Offset: 0x00066888
		public void DebugDrawDestinations()
		{
			foreach (PawnDestinationReservationManager.PawnDestinationReservation pawnDestinationReservation in this.GetPawnDestinationSetFor(Faction.OfPlayer).list)
			{
				if (!(pawnDestinationReservation.claimant.Position == pawnDestinationReservation.target))
				{
					IntVec3 target = pawnDestinationReservation.target;
					Vector3 s = new Vector3(1f, 1f, 1f);
					Matrix4x4 matrix = default(Matrix4x4);
					matrix.SetTRS(target.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays), Quaternion.identity, s);
					Graphics.DrawMesh(MeshPool.plane10, matrix, PawnDestinationReservationManager.DestinationMat, 0);
					if (Find.Selector.IsSelected(pawnDestinationReservation.claimant))
					{
						Graphics.DrawMesh(MeshPool.plane10, matrix, PawnDestinationReservationManager.DestinationSelectionMat, 0);
					}
				}
			}
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x00068770 File Offset: 0x00066970
		public void DebugDrawReservations()
		{
			foreach (KeyValuePair<Faction, PawnDestinationReservationManager.PawnDestinationSet> keyValuePair in this.reservedDestinations)
			{
				foreach (PawnDestinationReservationManager.PawnDestinationReservation pawnDestinationReservation in keyValuePair.Value.list)
				{
					IntVec3 target = pawnDestinationReservation.target;
					MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
					materialPropertyBlock.SetColor("_Color", keyValuePair.Key.Color);
					Vector3 s = new Vector3(1f, 1f, 1f);
					Matrix4x4 matrix = default(Matrix4x4);
					matrix.SetTRS(target.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays), Quaternion.identity, s);
					Graphics.DrawMesh(MeshPool.plane10, matrix, PawnDestinationReservationManager.DestinationMat, 0, Camera.main, 0, materialPropertyBlock);
					if (Find.Selector.IsSelected(pawnDestinationReservation.claimant))
					{
						Graphics.DrawMesh(MeshPool.plane10, matrix, PawnDestinationReservationManager.DestinationSelectionMat, 0);
					}
				}
			}
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x000688A4 File Offset: 0x00066AA4
		public void ExposeData()
		{
			Scribe_Collections.Look<Faction, PawnDestinationReservationManager.PawnDestinationSet>(ref this.reservedDestinations, "reservedDestinations", LookMode.Reference, LookMode.Deep, ref this.reservedDestinationsKeysWorkingList, ref this.reservedDestinationsValuesWorkingList);
		}

		// Token: 0x04000CB7 RID: 3255
		private Dictionary<Faction, PawnDestinationReservationManager.PawnDestinationSet> reservedDestinations = new Dictionary<Faction, PawnDestinationReservationManager.PawnDestinationSet>();

		// Token: 0x04000CB8 RID: 3256
		private static readonly Material DestinationMat = MaterialPool.MatFrom("UI/Overlays/ReservedDestination");

		// Token: 0x04000CB9 RID: 3257
		private static readonly Material DestinationSelectionMat = MaterialPool.MatFrom("UI/Overlays/ReservedDestinationSelection");

		// Token: 0x04000CBA RID: 3258
		private List<Faction> reservedDestinationsKeysWorkingList;

		// Token: 0x04000CBB RID: 3259
		private List<PawnDestinationReservationManager.PawnDestinationSet> reservedDestinationsValuesWorkingList;

		// Token: 0x02001464 RID: 5220
		public class PawnDestinationReservation : IExposable
		{
			// Token: 0x06007A6F RID: 31343 RVA: 0x00298A9C File Offset: 0x00296C9C
			public void ExposeData()
			{
				Scribe_Values.Look<IntVec3>(ref this.target, "target", default(IntVec3), false);
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
				Scribe_References.Look<Job>(ref this.job, "job", false);
				Scribe_Values.Look<bool>(ref this.obsolete, "obsolete", false, false);
			}

			// Token: 0x04004D64 RID: 19812
			public IntVec3 target;

			// Token: 0x04004D65 RID: 19813
			public Pawn claimant;

			// Token: 0x04004D66 RID: 19814
			public Job job;

			// Token: 0x04004D67 RID: 19815
			public bool obsolete;
		}

		// Token: 0x02001465 RID: 5221
		public class PawnDestinationSet : IExposable
		{
			// Token: 0x06007A71 RID: 31345 RVA: 0x00298AF8 File Offset: 0x00296CF8
			public void ExposeData()
			{
				Scribe_Collections.Look<PawnDestinationReservationManager.PawnDestinationReservation>(ref this.list, "list", LookMode.Deep, Array.Empty<object>());
				if (Scribe.mode == LoadSaveMode.PostLoadInit)
				{
					if (this.list.RemoveAll((PawnDestinationReservationManager.PawnDestinationReservation x) => x.claimant.DestroyedOrNull()) != 0)
					{
						Log.Warning("Some destination reservations had null or destroyed claimant.", false);
					}
				}
			}

			// Token: 0x04004D68 RID: 19816
			public List<PawnDestinationReservationManager.PawnDestinationReservation> list = new List<PawnDestinationReservationManager.PawnDestinationReservation>();
		}
	}
}
