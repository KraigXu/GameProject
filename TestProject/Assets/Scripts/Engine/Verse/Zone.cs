﻿using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020001E9 RID: 489
	public abstract class Zone : IExposable, ISelectable, ILoadReferenceable
	{
		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000DBD RID: 3517 RVA: 0x0004E9F3 File Offset: 0x0004CBF3
		public Map Map
		{
			get
			{
				return this.zoneManager.map;
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000DBE RID: 3518 RVA: 0x0004EA00 File Offset: 0x0004CC00
		public IntVec3 Position
		{
			get
			{
				if (this.cells.Count == 0)
				{
					return IntVec3.Invalid;
				}
				return this.cells[0];
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000DBF RID: 3519 RVA: 0x0004EA21 File Offset: 0x0004CC21
		public Material Material
		{
			get
			{
				if (this.materialInt == null)
				{
					this.materialInt = SolidColorMaterials.SimpleSolidColorMaterial(this.color, false);
					this.materialInt.renderQueue = 3600;
				}
				return this.materialInt;
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000DC0 RID: 3520 RVA: 0x0004EA59 File Offset: 0x0004CC59
		public string BaseLabel
		{
			get
			{
				return this.baseLabel;
			}
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x0004EA61 File Offset: 0x0004CC61
		public IEnumerator<IntVec3> GetEnumerator()
		{
			int num;
			for (int i = 0; i < this.cells.Count; i = num + 1)
			{
				yield return this.cells[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000DC2 RID: 3522 RVA: 0x0004EA70 File Offset: 0x0004CC70
		public List<IntVec3> Cells
		{
			get
			{
				if (!this.cellsShuffled)
				{
					this.cells.Shuffle<IntVec3>();
					this.cellsShuffled = true;
				}
				return this.cells;
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000DC3 RID: 3523 RVA: 0x0004EA92 File Offset: 0x0004CC92
		public IEnumerable<Thing> AllContainedThings
		{
			get
			{
				ThingGrid grids = this.Map.thingGrid;
				int num;
				for (int i = 0; i < this.cells.Count; i = num + 1)
				{
					List<Thing> thingList = grids.ThingsListAt(this.cells[i]);
					for (int j = 0; j < thingList.Count; j = num + 1)
					{
						yield return thingList[j];
						num = j;
					}
					thingList = null;
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000DC4 RID: 3524 RVA: 0x0004EAA4 File Offset: 0x0004CCA4
		public bool ContainsStaticFire
		{
			get
			{
				if (Find.TickManager.TicksGame > this.lastStaticFireCheckTick + 1000)
				{
					this.lastStaticFireCheckResult = false;
					for (int i = 0; i < this.cells.Count; i++)
					{
						if (this.cells[i].ContainsStaticFire(this.Map))
						{
							this.lastStaticFireCheckResult = true;
							break;
						}
					}
				}
				return this.lastStaticFireCheckResult;
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000DC5 RID: 3525 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool IsMultiselectable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000DC6 RID: 3526
		protected abstract Color NextZoneColor { get; }

		// Token: 0x06000DC7 RID: 3527 RVA: 0x0004EB0E File Offset: 0x0004CD0E
		public Zone()
		{
		}

		// Token: 0x06000DC8 RID: 3528 RVA: 0x0004EB40 File Offset: 0x0004CD40
		public Zone(string baseName, ZoneManager zoneManager)
		{
			this.baseLabel = baseName;
			this.label = zoneManager.NewZoneName(baseName);
			this.zoneManager = zoneManager;
			this.ID = Find.UniqueIDsManager.GetNextZoneID();
			this.color = this.NextZoneColor;
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x0004EBB4 File Offset: 0x0004CDB4
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Values.Look<string>(ref this.baseLabel, "baseLabel", null, false);
			Scribe_Values.Look<Color>(ref this.color, "color", default(Color), false);
			Scribe_Values.Look<bool>(ref this.hidden, "hidden", false, false);
			Scribe_Collections.Look<IntVec3>(ref this.cells, "cells", LookMode.Undefined, Array.Empty<object>());
			BackCompatibility.PostExposeData(this);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.CheckAddHaulDestination();
			}
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x0004EC50 File Offset: 0x0004CE50
		public virtual void AddCell(IntVec3 c)
		{
			if (this.cells.Contains(c))
			{
				Log.Error(string.Concat(new object[]
				{
					"Adding cell to zone which already has it. c=",
					c,
					", zone=",
					this
				}), false);
				return;
			}
			List<Thing> list = this.Map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (!thing.def.CanOverlapZones)
				{
					Log.Error("Added zone over zone-incompatible thing " + thing, false);
					return;
				}
			}
			this.cells.Add(c);
			this.zoneManager.AddZoneGridCell(this, c);
			this.Map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Zone);
			AutoHomeAreaMaker.Notify_ZoneCellAdded(c, this);
			this.cellsShuffled = false;
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x0004ED24 File Offset: 0x0004CF24
		public virtual void RemoveCell(IntVec3 c)
		{
			if (!this.cells.Contains(c))
			{
				Log.Error(string.Concat(new object[]
				{
					"Cannot remove cell from zone which doesn't have it. c=",
					c,
					", zone=",
					this
				}), false);
				return;
			}
			this.cells.Remove(c);
			this.zoneManager.ClearZoneGridCell(c);
			this.Map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Zone);
			this.cellsShuffled = false;
			if (this.cells.Count == 0)
			{
				this.Deregister();
			}
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x0004EDB8 File Offset: 0x0004CFB8
		public virtual void Delete()
		{
			SoundDefOf.Designate_ZoneDelete.PlayOneShotOnCamera(this.Map);
			if (this.cells.Count == 0)
			{
				this.Deregister();
			}
			else
			{
				while (this.cells.Count > 0)
				{
					this.RemoveCell(this.cells[this.cells.Count - 1]);
				}
			}
			Find.Selector.Deselect(this);
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x0004EE21 File Offset: 0x0004D021
		public void Deregister()
		{
			this.zoneManager.DeregisterZone(this);
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x0004EE2F File Offset: 0x0004D02F
		public virtual void PostRegister()
		{
			this.CheckAddHaulDestination();
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x0004EE38 File Offset: 0x0004D038
		public virtual void PostDeregister()
		{
			IHaulDestination haulDestination = this as IHaulDestination;
			if (haulDestination != null)
			{
				this.Map.haulDestinationManager.RemoveHaulDestination(haulDestination);
			}
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x0004EE60 File Offset: 0x0004D060
		public bool ContainsCell(IntVec3 c)
		{
			for (int i = 0; i < this.cells.Count; i++)
			{
				if (this.cells[i] == c)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x0004EE9A File Offset: 0x0004D09A
		public virtual string GetInspectString()
		{
			return "";
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return null;
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x0004EEA1 File Offset: 0x0004D0A1
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			yield return new Command_Action
			{
				icon = ContentFinder<Texture2D>.Get("UI/Commands/RenameZone", true),
				defaultLabel = "CommandRenameZoneLabel".Translate(),
				defaultDesc = "CommandRenameZoneDesc".Translate(),
				action = delegate
				{
					Dialog_RenameZone dialog_RenameZone = new Dialog_RenameZone(this);
					if (KeyBindingDefOf.Misc1.IsDown)
					{
						dialog_RenameZone.WasOpenedByHotkey();
					}
					Find.WindowStack.Add(dialog_RenameZone);
				},
				hotKey = KeyBindingDefOf.Misc1
			};
			yield return new Command_Toggle
			{
				icon = ContentFinder<Texture2D>.Get("UI/Commands/HideZone", true),
				defaultLabel = (this.hidden ? "CommandUnhideZoneLabel".Translate() : "CommandHideZoneLabel".Translate()),
				defaultDesc = "CommandHideZoneDesc".Translate(),
				isActive = (() => this.hidden),
				toggleAction = delegate
				{
					this.hidden = !this.hidden;
					foreach (IntVec3 loc in this.Cells)
					{
						this.Map.mapDrawer.MapMeshDirty(loc, MapMeshFlag.Zone);
					}
				},
				hotKey = KeyBindingDefOf.Misc2
			};
			foreach (Gizmo gizmo in this.GetZoneAddGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			Designator designator = DesignatorUtility.FindAllowedDesignator<Designator_ZoneDelete_Shrink>();
			if (designator != null)
			{
				yield return designator;
			}
			yield return new Command_Action
			{
				icon = TexButton.DeleteX,
				defaultLabel = "CommandDeleteZoneLabel".Translate(),
				defaultDesc = "CommandDeleteZoneDesc".Translate(),
				action = new Action(this.Delete),
				hotKey = KeyBindingDefOf.Designator_Deconstruct
			};
			yield break;
			yield break;
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x0004EEB1 File Offset: 0x0004D0B1
		public virtual IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			yield break;
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x0004EEBC File Offset: 0x0004D0BC
		public void CheckContiguous()
		{
			if (this.cells.Count == 0)
			{
				return;
			}
			if (Zone.extantGrid == null)
			{
				Zone.extantGrid = new BoolGrid(this.Map);
			}
			else
			{
				Zone.extantGrid.ClearAndResizeTo(this.Map);
			}
			if (Zone.foundGrid == null)
			{
				Zone.foundGrid = new BoolGrid(this.Map);
			}
			else
			{
				Zone.foundGrid.ClearAndResizeTo(this.Map);
			}
			for (int i = 0; i < this.cells.Count; i++)
			{
				Zone.extantGrid.Set(this.cells[i], true);
			}
			Predicate<IntVec3> passCheck = (IntVec3 c) => Zone.extantGrid[c] && !Zone.foundGrid[c];
			int numFound = 0;
			Action<IntVec3> processor = delegate(IntVec3 c)
			{
				Zone.foundGrid.Set(c, true);
				int numFound = numFound;
				numFound++;
			};
			this.Map.floodFiller.FloodFill(this.cells[0], passCheck, processor, int.MaxValue, false, null);
			if (numFound < this.cells.Count)
			{
				foreach (IntVec3 c2 in this.Map.AllCells)
				{
					if (Zone.extantGrid[c2] && !Zone.foundGrid[c2])
					{
						this.RemoveCell(c2);
					}
				}
			}
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x0004F030 File Offset: 0x0004D230
		private void CheckAddHaulDestination()
		{
			IHaulDestination haulDestination = this as IHaulDestination;
			if (haulDestination != null)
			{
				this.Map.haulDestinationManager.AddHaulDestination(haulDestination);
			}
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x0004F058 File Offset: 0x0004D258
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x0004F060 File Offset: 0x0004D260
		public string GetUniqueLoadID()
		{
			return "Zone_" + this.ID;
		}

		// Token: 0x04000A84 RID: 2692
		public ZoneManager zoneManager;

		// Token: 0x04000A85 RID: 2693
		public int ID = -1;

		// Token: 0x04000A86 RID: 2694
		public string label;

		// Token: 0x04000A87 RID: 2695
		private string baseLabel;

		// Token: 0x04000A88 RID: 2696
		public List<IntVec3> cells = new List<IntVec3>();

		// Token: 0x04000A89 RID: 2697
		private bool cellsShuffled;

		// Token: 0x04000A8A RID: 2698
		public Color color = Color.white;

		// Token: 0x04000A8B RID: 2699
		private Material materialInt;

		// Token: 0x04000A8C RID: 2700
		public bool hidden;

		// Token: 0x04000A8D RID: 2701
		private int lastStaticFireCheckTick = -9999;

		// Token: 0x04000A8E RID: 2702
		private bool lastStaticFireCheckResult;

		// Token: 0x04000A8F RID: 2703
		private const int StaticFireCheckInterval = 1000;

		// Token: 0x04000A90 RID: 2704
		private static BoolGrid extantGrid;

		// Token: 0x04000A91 RID: 2705
		private static BoolGrid foundGrid;
	}
}
