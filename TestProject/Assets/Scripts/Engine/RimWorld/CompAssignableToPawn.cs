using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class CompAssignableToPawn : ThingComp
	{
		
		// (get) Token: 0x06005075 RID: 20597 RVA: 0x001B132C File Offset: 0x001AF52C
		public CompProperties_AssignableToPawn Props
		{
			get
			{
				return (CompProperties_AssignableToPawn)this.props;
			}
		}

		
		// (get) Token: 0x06005076 RID: 20598 RVA: 0x001B1339 File Offset: 0x001AF539
		public int MaxAssignedPawnsCount
		{
			get
			{
				return this.Props.maxAssignedPawnsCount;
			}
		}

		
		// (get) Token: 0x06005077 RID: 20599 RVA: 0x001B1348 File Offset: 0x001AF548
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

		
		// (get) Token: 0x06005078 RID: 20600 RVA: 0x001B13B2 File Offset: 0x001AF5B2
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

		
		// (get) Token: 0x06005079 RID: 20601 RVA: 0x001B13DC File Offset: 0x001AF5DC
		public List<Pawn> AssignedPawnsForReading
		{
			get
			{
				return this.assignedPawns;
			}
		}

		
		// (get) Token: 0x0600507A RID: 20602 RVA: 0x001B13DC File Offset: 0x001AF5DC
		public IEnumerable<Pawn> AssignedPawns
		{
			get
			{
				return this.assignedPawns;
			}
		}

		
		// (get) Token: 0x0600507B RID: 20603 RVA: 0x001B13E4 File Offset: 0x001AF5E4
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
