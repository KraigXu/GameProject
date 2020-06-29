using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class Pawn_Ownership : IExposable
	{
		
		
		
		public Building_Bed OwnedBed
		{
			get
			{
				return this.intOwnedBed;
			}
			private set
			{
				if (this.intOwnedBed != value)
				{
					this.intOwnedBed = value;
					ThoughtUtility.RemovePositiveBedroomThoughts(this.pawn);
				}
			}
		}

		
		
		
		public Building_Grave AssignedGrave { get; private set; }

		
		
		
		public Building_Throne AssignedThrone { get; private set; }

		
		
		
		public Building AssignedMeditationSpot { get; private set; }

		
		
		public Room OwnedRoom
		{
			get
			{
				if (this.OwnedBed == null)
				{
					return null;
				}
				Room room = this.OwnedBed.GetRoom(RegionType.Set_Passable);
				if (room == null)
				{
					return null;
				}
				if (room.Owners.Contains(this.pawn))
				{
					return room;
				}
				return null;
			}
		}

		
		public Pawn_Ownership(Pawn pawn)
		{
			this.pawn = pawn;
		}

		
		public void ExposeData()
		{
			Building_Grave assignedGrave = this.AssignedGrave;
			Building_Throne assignedThrone = this.AssignedThrone;
			Building assignedMeditationSpot = this.AssignedMeditationSpot;
			Scribe_References.Look<Building_Bed>(ref this.intOwnedBed, "ownedBed", false);
			Scribe_References.Look<Building>(ref assignedMeditationSpot, "assignedMeditationSpot", false);
			Scribe_References.Look<Building_Grave>(ref assignedGrave, "assignedGrave", false);
			Scribe_References.Look<Building_Throne>(ref assignedThrone, "assignedThrone", false);
			this.AssignedGrave = assignedGrave;
			this.AssignedThrone = assignedThrone;
			this.AssignedMeditationSpot = assignedMeditationSpot;
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.intOwnedBed != null)
				{
					CompAssignableToPawn compAssignableToPawn = this.intOwnedBed.TryGetComp<CompAssignableToPawn>();
					if (compAssignableToPawn != null && !compAssignableToPawn.AssignedPawns.Contains(this.pawn))
					{
						Building_Bed newBed = this.intOwnedBed;
						this.UnclaimBed();
						this.ClaimBedIfNonMedical(newBed);
					}
				}
				if (this.AssignedGrave != null)
				{
					CompAssignableToPawn compAssignableToPawn2 = this.AssignedGrave.TryGetComp<CompAssignableToPawn>();
					if (compAssignableToPawn2 != null && !compAssignableToPawn2.AssignedPawns.Contains(this.pawn))
					{
						Building_Grave assignedGrave2 = this.AssignedGrave;
						this.UnclaimGrave();
						this.ClaimGrave(assignedGrave2);
					}
				}
				if (this.AssignedThrone != null)
				{
					CompAssignableToPawn compAssignableToPawn3 = this.AssignedThrone.TryGetComp<CompAssignableToPawn>();
					if (compAssignableToPawn3 != null && !compAssignableToPawn3.AssignedPawns.Contains(this.pawn))
					{
						Building_Throne assignedThrone2 = this.AssignedThrone;
						this.UnclaimThrone();
						this.ClaimThrone(assignedThrone2);
					}
				}
			}
		}

		
		public bool ClaimBedIfNonMedical(Building_Bed newBed)
		{
			if (newBed.OwnersForReading.Contains(this.pawn) || newBed.Medical)
			{
				return false;
			}
			this.UnclaimBed();
			if (newBed.OwnersForReading.Count == newBed.SleepingSlotsCount)
			{
				newBed.OwnersForReading[newBed.OwnersForReading.Count - 1].ownership.UnclaimBed();
			}
			newBed.CompAssignableToPawn.ForceAddPawn(this.pawn);
			this.OwnedBed = newBed;
			if (newBed.Medical)
			{
				Log.Warning(this.pawn.LabelCap + " claimed medical bed.", false);
				this.UnclaimBed();
			}
			return true;
		}

		
		public bool UnclaimBed()
		{
			if (this.OwnedBed == null)
			{
				return false;
			}
			this.OwnedBed.CompAssignableToPawn.ForceRemovePawn(this.pawn);
			this.OwnedBed = null;
			return true;
		}

		
		public bool ClaimGrave(Building_Grave newGrave)
		{
			if (newGrave.AssignedPawn == this.pawn)
			{
				return false;
			}
			this.UnclaimGrave();
			if (newGrave.AssignedPawn != null)
			{
				newGrave.AssignedPawn.ownership.UnclaimGrave();
			}
			newGrave.CompAssignableToPawn.ForceAddPawn(this.pawn);
			newGrave.GetStoreSettings().Priority = StoragePriority.Critical;
			this.AssignedGrave = newGrave;
			return true;
		}

		
		public bool UnclaimGrave()
		{
			if (this.AssignedGrave == null)
			{
				return false;
			}
			this.AssignedGrave.CompAssignableToPawn.ForceRemovePawn(this.pawn);
			this.AssignedGrave.GetStoreSettings().Priority = StoragePriority.Important;
			this.AssignedGrave = null;
			return true;
		}

		
		public bool ClaimThrone(Building_Throne newThrone)
		{
			if (newThrone.AssignedPawn == this.pawn)
			{
				return false;
			}
			this.UnclaimThrone();
			if (newThrone.AssignedPawn != null)
			{
				newThrone.AssignedPawn.ownership.UnclaimThrone();
			}
			newThrone.CompAssignableToPawn.ForceAddPawn(this.pawn);
			this.AssignedThrone = newThrone;
			return true;
		}

		
		public bool UnclaimThrone()
		{
			if (this.AssignedThrone == null)
			{
				return false;
			}
			this.AssignedThrone.CompAssignableToPawn.ForceRemovePawn(this.pawn);
			this.AssignedThrone = null;
			return true;
		}

		
		public bool ClaimMeditationSpot(Building newSpot)
		{
			if (newSpot.GetAssignedPawn() == this.pawn)
			{
				return false;
			}
			this.UnclaimMeditationSpot();
			if (newSpot.GetAssignedPawn() != null)
			{
				newSpot.GetAssignedPawn().ownership.UnclaimMeditationSpot();
			}
			newSpot.TryGetComp<CompAssignableToPawn>().ForceAddPawn(this.pawn);
			this.AssignedMeditationSpot = newSpot;
			return true;
		}

		
		public bool UnclaimMeditationSpot()
		{
			if (this.AssignedMeditationSpot == null)
			{
				return false;
			}
			this.AssignedMeditationSpot.TryGetComp<CompAssignableToPawn>().ForceRemovePawn(this.pawn);
			this.AssignedMeditationSpot = null;
			return true;
		}

		
		public void UnclaimAll()
		{
			this.UnclaimBed();
			this.UnclaimGrave();
			this.UnclaimThrone();
		}

		
		public void Notify_ChangedGuestStatus()
		{
			if (this.OwnedBed != null && ((this.OwnedBed.ForPrisoners && !this.pawn.IsPrisoner) || (!this.OwnedBed.ForPrisoners && this.pawn.IsPrisoner)))
			{
				this.UnclaimBed();
			}
		}

		
		private Pawn pawn;

		
		private Building_Bed intOwnedBed;
	}
}
