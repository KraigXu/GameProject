using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	
	[StaticConstructorOnStartup]
	public sealed class PawnDestinationReservationManager : IExposable
	{
		
		public PawnDestinationReservationManager.PawnDestinationSet GetPawnDestinationSetFor(Faction faction)
		{
			if (!this.reservedDestinations.ContainsKey(faction))
			{
				this.reservedDestinations.Add(faction, new PawnDestinationReservationManager.PawnDestinationSet());
			}
			return this.reservedDestinations[faction];
		}

		
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

		
		public bool IsReserved(IntVec3 loc)
		{
			Pawn pawn;
			return this.IsReserved(loc, out pawn);
		}

		
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

		
		public void ExposeData()
		{
			Scribe_Collections.Look<Faction, PawnDestinationReservationManager.PawnDestinationSet>(ref this.reservedDestinations, "reservedDestinations", LookMode.Reference, LookMode.Deep, ref this.reservedDestinationsKeysWorkingList, ref this.reservedDestinationsValuesWorkingList);
		}

		
		private Dictionary<Faction, PawnDestinationReservationManager.PawnDestinationSet> reservedDestinations = new Dictionary<Faction, PawnDestinationReservationManager.PawnDestinationSet>();

		
		private static readonly Material DestinationMat = MaterialPool.MatFrom("UI/Overlays/ReservedDestination");

		
		private static readonly Material DestinationSelectionMat = MaterialPool.MatFrom("UI/Overlays/ReservedDestinationSelection");

		
		private List<Faction> reservedDestinationsKeysWorkingList;

		
		private List<PawnDestinationReservationManager.PawnDestinationSet> reservedDestinationsValuesWorkingList;

		
		public class PawnDestinationReservation : IExposable
		{
			
			public void ExposeData()
			{
				Scribe_Values.Look<IntVec3>(ref this.target, "target", default(IntVec3), false);
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
				Scribe_References.Look<Job>(ref this.job, "job", false);
				Scribe_Values.Look<bool>(ref this.obsolete, "obsolete", false, false);
			}

			
			public IntVec3 target;

			
			public Pawn claimant;

			
			public Job job;

			
			public bool obsolete;
		}

		
		public class PawnDestinationSet : IExposable
		{
			
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

			
			public List<PawnDestinationReservationManager.PawnDestinationReservation> list = new List<PawnDestinationReservationManager.PawnDestinationReservation>();
		}
	}
}
