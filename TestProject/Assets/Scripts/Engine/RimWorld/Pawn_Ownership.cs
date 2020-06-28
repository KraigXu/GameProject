using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BAD RID: 2989
	public class Pawn_Ownership : IExposable
	{
		// Token: 0x17000C77 RID: 3191
		// (get) Token: 0x0600463D RID: 17981 RVA: 0x0017B210 File Offset: 0x00179410
		// (set) Token: 0x0600463E RID: 17982 RVA: 0x0017B218 File Offset: 0x00179418
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

		// Token: 0x17000C78 RID: 3192
		// (get) Token: 0x0600463F RID: 17983 RVA: 0x0017B235 File Offset: 0x00179435
		// (set) Token: 0x06004640 RID: 17984 RVA: 0x0017B23D File Offset: 0x0017943D
		public Building_Grave AssignedGrave { get; private set; }

		// Token: 0x17000C79 RID: 3193
		// (get) Token: 0x06004641 RID: 17985 RVA: 0x0017B246 File Offset: 0x00179446
		// (set) Token: 0x06004642 RID: 17986 RVA: 0x0017B24E File Offset: 0x0017944E
		public Building_Throne AssignedThrone { get; private set; }

		// Token: 0x17000C7A RID: 3194
		// (get) Token: 0x06004643 RID: 17987 RVA: 0x0017B257 File Offset: 0x00179457
		// (set) Token: 0x06004644 RID: 17988 RVA: 0x0017B25F File Offset: 0x0017945F
		public Building AssignedMeditationSpot { get; private set; }

		// Token: 0x17000C7B RID: 3195
		// (get) Token: 0x06004645 RID: 17989 RVA: 0x0017B268 File Offset: 0x00179468
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

		// Token: 0x06004646 RID: 17990 RVA: 0x0017B2A7 File Offset: 0x001794A7
		public Pawn_Ownership(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06004647 RID: 17991 RVA: 0x0017B2B8 File Offset: 0x001794B8
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

		// Token: 0x06004648 RID: 17992 RVA: 0x0017B400 File Offset: 0x00179600
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

		// Token: 0x06004649 RID: 17993 RVA: 0x0017B4AA File Offset: 0x001796AA
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

		// Token: 0x0600464A RID: 17994 RVA: 0x0017B4D4 File Offset: 0x001796D4
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

		// Token: 0x0600464B RID: 17995 RVA: 0x0017B536 File Offset: 0x00179736
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

		// Token: 0x0600464C RID: 17996 RVA: 0x0017B574 File Offset: 0x00179774
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

		// Token: 0x0600464D RID: 17997 RVA: 0x0017B5CA File Offset: 0x001797CA
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

		// Token: 0x0600464E RID: 17998 RVA: 0x0017B5F4 File Offset: 0x001797F4
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

		// Token: 0x0600464F RID: 17999 RVA: 0x0017B64A File Offset: 0x0017984A
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

		// Token: 0x06004650 RID: 18000 RVA: 0x0017B674 File Offset: 0x00179874
		public void UnclaimAll()
		{
			this.UnclaimBed();
			this.UnclaimGrave();
			this.UnclaimThrone();
		}

		// Token: 0x06004651 RID: 18001 RVA: 0x0017B68C File Offset: 0x0017988C
		public void Notify_ChangedGuestStatus()
		{
			if (this.OwnedBed != null && ((this.OwnedBed.ForPrisoners && !this.pawn.IsPrisoner) || (!this.OwnedBed.ForPrisoners && this.pawn.IsPrisoner)))
			{
				this.UnclaimBed();
			}
		}

		// Token: 0x04002851 RID: 10321
		private Pawn pawn;

		// Token: 0x04002852 RID: 10322
		private Building_Bed intOwnedBed;
	}
}
