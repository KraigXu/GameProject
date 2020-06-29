using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class CompAssignableToPawn : ThingComp
	{
		
		
		public CompProperties_AssignableToPawn Props
		{
			get
			{
				return (CompProperties_AssignableToPawn)this.props;
			}
		}

		
		
		public int MaxAssignedPawnsCount
		{
			get
			{
				return this.Props.maxAssignedPawnsCount;
			}
		}

		
		
		public bool PlayerCanSeeAssignments
		{
			get
			{
				if (this.parent.Faction == Faction.OfPlayer)
				{
					return true;
				}
				for (int i = 0; i < this.assignedPawns.Count; i++)
				{
					if (this.assignedPawns[i].Faction == Faction.OfPlayer || this.assignedPawns[i].HostFaction == Faction.OfPlayer)
					{
						return true;
					}
				}
				return false;
			}
		}

		
		
		public virtual IEnumerable<Pawn> AssigningCandidates
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return Enumerable.Empty<Pawn>();
				}
				return this.parent.Map.mapPawns.FreeColonists;
			}
		}

		
		
		public List<Pawn> AssignedPawnsForReading
		{
			get
			{
				return this.assignedPawns;
			}
		}

		
		
		public IEnumerable<Pawn> AssignedPawns
		{
			get
			{
				return this.assignedPawns;
			}
		}

		
		
		public bool HasFreeSlot
		{
			get
			{
				return this.assignedPawns.Count < this.Props.maxAssignedPawnsCount;
			}
		}

		
		protected virtual bool CanDrawOverlayForPawn(Pawn pawn)
		{
			return true;
		}

		
		public override void DrawGUIOverlay()
		{
			if (!this.Props.drawAssignmentOverlay || (!this.Props.drawUnownedAssignmentOverlay && !this.assignedPawns.Any<Pawn>()))
			{
				return;
			}
			if (Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest && this.PlayerCanSeeAssignments)
			{
				Color defaultThingLabelColor = GenMapUI.DefaultThingLabelColor;
				if (!this.assignedPawns.Any<Pawn>())
				{
					GenMapUI.DrawThingLabel(this.parent, "Unowned".Translate(), defaultThingLabelColor);
				}
				if (this.assignedPawns.Count == 1)
				{
					if (!this.CanDrawOverlayForPawn(this.assignedPawns[0]))
					{
						return;
					}
					GenMapUI.DrawThingLabel(this.parent, this.assignedPawns[0].LabelShort, defaultThingLabelColor);
				}
			}
		}

		
		protected virtual void SortAssignedPawns()
		{
			this.assignedPawns.SortBy((Pawn x) => x.thingIDNumber);
		}

		
		public virtual void ForceAddPawn(Pawn pawn)
		{
			if (!this.assignedPawns.Contains(pawn))
			{
				this.assignedPawns.Add(pawn);
			}
			this.SortAssignedPawns();
		}

		
		public virtual void ForceRemovePawn(Pawn pawn)
		{
			if (this.assignedPawns.Contains(pawn))
			{
				this.assignedPawns.Remove(pawn);
			}
			this.SortAssignedPawns();
		}

		
		public virtual AcceptanceReport CanAssignTo(Pawn pawn)
		{
			return AcceptanceReport.WasAccepted;
		}

		
		public virtual void TryAssignPawn(Pawn pawn)
		{
			if (this.assignedPawns.Contains(pawn))
			{
				return;
			}
			this.assignedPawns.Add(pawn);
			this.SortAssignedPawns();
		}

		
		public virtual void TryUnassignPawn(Pawn pawn, bool sort = true)
		{
			if (!this.assignedPawns.Contains(pawn))
			{
				return;
			}
			this.assignedPawns.Remove(pawn);
			if (sort)
			{
				this.SortAssignedPawns();
			}
		}

		
		public virtual bool AssignedAnything(Pawn pawn)
		{
			return this.assignedPawns.Contains(pawn);
		}

		
		protected virtual bool ShouldShowAssignmentGizmo()
		{
			return this.parent.Faction == Faction.OfPlayer;
		}

		
		protected virtual string GetAssignmentGizmoLabel()
		{
			return "CommandThingSetOwnerLabel".Translate();
		}

		
		protected virtual string GetAssignmentGizmoDesc()
		{
			return "CommandThroneSetOwnerDesc".Translate();
		}

		
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.ShouldShowAssignmentGizmo())
			{
				yield return new Command_Action
				{
					defaultLabel = this.GetAssignmentGizmoLabel(),
					icon = ContentFinder<Texture2D>.Get("UI/Commands/AssignOwner", true),
					defaultDesc = this.GetAssignmentGizmoDesc(),
					action = delegate
					{
						Find.WindowStack.Add(new Dialog_AssignBuildingOwner(this));
					},
					hotKey = KeyBindingDefOf.Misc3
				};
			}
			yield break;
		}

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Collections.Look<Pawn>(ref this.assignedPawns, "assignedPawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.assignedPawns.RemoveAll((Pawn x) => x == null);
			}
		}

		
		public override void PostDeSpawn(Map map)
		{
			for (int i = this.assignedPawns.Count - 1; i >= 0; i--)
			{
				this.TryUnassignPawn(this.assignedPawns[i], false);
			}
		}

		
		protected List<Pawn> assignedPawns = new List<Pawn>();
	}
}
