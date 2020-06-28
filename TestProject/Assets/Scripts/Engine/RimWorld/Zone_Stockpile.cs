using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ABF RID: 2751
	public class Zone_Stockpile : Zone, ISlotGroupParent, IStoreSettingsParent, IHaulDestination
	{
		// Token: 0x17000B8F RID: 2959
		// (get) Token: 0x06004131 RID: 16689 RVA: 0x0001028D File Offset: 0x0000E48D
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000B90 RID: 2960
		// (get) Token: 0x06004132 RID: 16690 RVA: 0x00010306 File Offset: 0x0000E506
		public bool IgnoreStoredThingsBeauty
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000B91 RID: 2961
		// (get) Token: 0x06004133 RID: 16691 RVA: 0x0015D402 File Offset: 0x0015B602
		protected override Color NextZoneColor
		{
			get
			{
				return ZoneColorUtility.NextStorageZoneColor();
			}
		}

		// Token: 0x06004134 RID: 16692 RVA: 0x0015D409 File Offset: 0x0015B609
		public Zone_Stockpile()
		{
			this.slotGroup = new SlotGroup(this);
		}

		// Token: 0x06004135 RID: 16693 RVA: 0x0015D41D File Offset: 0x0015B61D
		public Zone_Stockpile(StorageSettingsPreset preset, ZoneManager zoneManager) : base(preset.PresetName(), zoneManager)
		{
			this.settings = new StorageSettings(this);
			this.settings.SetFromPreset(preset);
			this.slotGroup = new SlotGroup(this);
		}

		// Token: 0x06004136 RID: 16694 RVA: 0x0015D450 File Offset: 0x0015B650
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", new object[]
			{
				this
			});
		}

		// Token: 0x06004137 RID: 16695 RVA: 0x0015D472 File Offset: 0x0015B672
		public override void AddCell(IntVec3 sq)
		{
			base.AddCell(sq);
			if (this.slotGroup != null)
			{
				this.slotGroup.Notify_AddedCell(sq);
			}
		}

		// Token: 0x06004138 RID: 16696 RVA: 0x0015D48F File Offset: 0x0015B68F
		public override void RemoveCell(IntVec3 sq)
		{
			base.RemoveCell(sq);
			this.slotGroup.Notify_LostCell(sq);
		}

		// Token: 0x06004139 RID: 16697 RVA: 0x0015D4A4 File Offset: 0x0015B6A4
		public override void PostDeregister()
		{
			base.PostDeregister();
			BillUtility.Notify_ZoneStockpileRemoved(this);
		}

		// Token: 0x0600413A RID: 16698 RVA: 0x0015D4B2 File Offset: 0x0015B6B2
		public override IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return Zone_Stockpile.ITabs;
		}

		// Token: 0x0600413B RID: 16699 RVA: 0x0015D4B9 File Offset: 0x0015B6B9
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in this.<>n__0())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			foreach (Gizmo gizmo2 in StorageSettingsClipboard.CopyPasteGizmosFor(this.settings))
			{
				yield return gizmo2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600413C RID: 16700 RVA: 0x0015D4C9 File Offset: 0x0015B6C9
		public override IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			yield return DesignatorUtility.FindAllowedDesignator<Designator_ZoneAddStockpile_Expand>();
			yield break;
		}

		// Token: 0x0600413D RID: 16701 RVA: 0x0015D4D2 File Offset: 0x0015B6D2
		public SlotGroup GetSlotGroup()
		{
			return this.slotGroup;
		}

		// Token: 0x0600413E RID: 16702 RVA: 0x0015D4DA File Offset: 0x0015B6DA
		public IEnumerable<IntVec3> AllSlotCells()
		{
			int num;
			for (int i = 0; i < this.cells.Count; i = num + 1)
			{
				yield return this.cells[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x0600413F RID: 16703 RVA: 0x0015D4EA File Offset: 0x0015B6EA
		public List<IntVec3> AllSlotCellsList()
		{
			return this.cells;
		}

		// Token: 0x06004140 RID: 16704 RVA: 0x00019EA1 File Offset: 0x000180A1
		public StorageSettings GetParentStoreSettings()
		{
			return null;
		}

		// Token: 0x06004141 RID: 16705 RVA: 0x0015D4F2 File Offset: 0x0015B6F2
		public StorageSettings GetStoreSettings()
		{
			return this.settings;
		}

		// Token: 0x06004142 RID: 16706 RVA: 0x0015D4FA File Offset: 0x0015B6FA
		public bool Accepts(Thing t)
		{
			return this.settings.AllowedToAccept(t);
		}

		// Token: 0x06004143 RID: 16707 RVA: 0x0004F058 File Offset: 0x0004D258
		public string SlotYielderLabel()
		{
			return this.label;
		}

		// Token: 0x06004144 RID: 16708 RVA: 0x0015D508 File Offset: 0x0015B708
		public void Notify_ReceivedThing(Thing newItem)
		{
			if (newItem.def.storedConceptLearnOpportunity != null)
			{
				LessonAutoActivator.TeachOpportunity(newItem.def.storedConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
		}

		// Token: 0x06004145 RID: 16709 RVA: 0x00002681 File Offset: 0x00000881
		public void Notify_LostThing(Thing newItem)
		{
		}

		// Token: 0x040025DC RID: 9692
		public StorageSettings settings;

		// Token: 0x040025DD RID: 9693
		public SlotGroup slotGroup;

		// Token: 0x040025DE RID: 9694
		private static readonly ITab[] ITabs = new ITab[]
		{
			new ITab_Storage()
		};
	}
}
