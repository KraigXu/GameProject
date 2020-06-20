using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CED RID: 3309
	public class CompAssignableToPawn : ThingComp
	{
		// Token: 0x17000E1D RID: 3613
		// (get) Token: 0x06005075 RID: 20597 RVA: 0x001B132C File Offset: 0x001AF52C
		public CompProperties_AssignableToPawn Props
		{
			get
			{
				return (CompProperties_AssignableToPawn)this.props;
			}
		}

		// Token: 0x17000E1E RID: 3614
		// (get) Token: 0x06005076 RID: 20598 RVA: 0x001B1339 File Offset: 0x001AF539
		public int MaxAssignedPawnsCount
		{
			get
			{
				return this.Props.maxAssignedPawnsCount;
			}
		}

		// Token: 0x17000E1F RID: 3615
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

		// Token: 0x17000E20 RID: 3616
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

		// Token: 0x17000E21 RID: 3617
		// (get) Token: 0x06005079 RID: 20601 RVA: 0x001B13DC File Offset: 0x001AF5DC
		public List<Pawn> AssignedPawnsForReading
		{
			get
			{
				return this.assignedPawns;
			}
		}

		// Token: 0x17000E22 RID: 3618
		// (get) Token: 0x0600507A RID: 20602 RVA: 0x001B13DC File Offset: 0x001AF5DC
		public IEnumerable<Pawn> AssignedPawns
		{
			get
			{
				return this.assignedPawns;
			}
		}

		// Token: 0x17000E23 RID: 3619
		// (get) Token: 0x0600507B RID: 20603 RVA: 0x001B13E4 File Offset: 0x001AF5E4
		public bool HasFreeSlot
		{
			get
			{
				return this.assignedPawns.Count < this.Props.maxAssignedPawnsCount;
			}
		}

		// Token: 0x0600507C RID: 20604 RVA: 0x0001028D File Offset: 0x0000E48D
		protected virtual bool CanDrawOverlayForPawn(Pawn pawn)
		{
			return true;
		}

		// Token: 0x0600507D RID: 20605 RVA: 0x001B1400 File Offset: 0x001AF600
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

		// Token: 0x0600507E RID: 20606 RVA: 0x001B14B7 File Offset: 0x001AF6B7
		protected virtual void SortAssignedPawns()
		{
			this.assignedPawns.SortBy((Pawn x) => x.thingIDNumber);
		}

		// Token: 0x0600507F RID: 20607 RVA: 0x001B14E3 File Offset: 0x001AF6E3
		public virtual void ForceAddPawn(Pawn pawn)
		{
			if (!this.assignedPawns.Contains(pawn))
			{
				this.assignedPawns.Add(pawn);
			}
			this.SortAssignedPawns();
		}

		// Token: 0x06005080 RID: 20608 RVA: 0x001B1505 File Offset: 0x001AF705
		public virtual void ForceRemovePawn(Pawn pawn)
		{
			if (this.assignedPawns.Contains(pawn))
			{
				this.assignedPawns.Remove(pawn);
			}
			this.SortAssignedPawns();
		}

		// Token: 0x06005081 RID: 20609 RVA: 0x00044240 File Offset: 0x00042440
		public virtual AcceptanceReport CanAssignTo(Pawn pawn)
		{
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06005082 RID: 20610 RVA: 0x001B1528 File Offset: 0x001AF728
		public virtual void TryAssignPawn(Pawn pawn)
		{
			if (this.assignedPawns.Contains(pawn))
			{
				return;
			}
			this.assignedPawns.Add(pawn);
			this.SortAssignedPawns();
		}

		// Token: 0x06005083 RID: 20611 RVA: 0x001B154B File Offset: 0x001AF74B
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

		// Token: 0x06005084 RID: 20612 RVA: 0x001B1572 File Offset: 0x001AF772
		public virtual bool AssignedAnything(Pawn pawn)
		{
			return this.assignedPawns.Contains(pawn);
		}

		// Token: 0x06005085 RID: 20613 RVA: 0x001B1580 File Offset: 0x001AF780
		protected virtual bool ShouldShowAssignmentGizmo()
		{
			return this.parent.Faction == Faction.OfPlayer;
		}

		// Token: 0x06005086 RID: 20614 RVA: 0x001B1594 File Offset: 0x001AF794
		protected virtual string GetAssignmentGizmoLabel()
		{
			return "CommandThingSetOwnerLabel".Translate();
		}

		// Token: 0x06005087 RID: 20615 RVA: 0x001B15A5 File Offset: 0x001AF7A5
		protected virtual string GetAssignmentGizmoDesc()
		{
			return "CommandThroneSetOwnerDesc".Translate();
		}

		// Token: 0x06005088 RID: 20616 RVA: 0x001B15B6 File Offset: 0x001AF7B6
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

		// Token: 0x06005089 RID: 20617 RVA: 0x001B15C8 File Offset: 0x001AF7C8
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Collections.Look<Pawn>(ref this.assignedPawns, "assignedPawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.assignedPawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0600508A RID: 20618 RVA: 0x001B1624 File Offset: 0x001AF824
		public override void PostDeSpawn(Map map)
		{
			for (int i = this.assignedPawns.Count - 1; i >= 0; i--)
			{
				this.TryUnassignPawn(this.assignedPawns[i], false);
			}
		}

		// Token: 0x04002CC6 RID: 11462
		protected List<Pawn> assignedPawns = new List<Pawn>();
	}
}
