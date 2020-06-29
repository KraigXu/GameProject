using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Zone_Stockpile : Zone, ISlotGroupParent, IStoreSettingsParent, IHaulDestination
	{
		
		// (get) Token: 0x06004131 RID: 16689 RVA: 0x0001028D File Offset: 0x0000E48D
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		
		// (get) Token: 0x06004132 RID: 16690 RVA: 0x00010306 File Offset: 0x0000E506
		public bool IgnoreStoredThingsBeauty
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x06004133 RID: 16691 RVA: 0x0015D402 File Offset: 0x0015B602
		protected override Color NextZoneColor
		{
			get
			{
				return ZoneColorUtility.NextStorageZoneColor();
			}
		}

		
		public Zone_Stockpile()
		{
			this.slotGroup = new SlotGroup(this);
		}

		
		public Zone_Stockpile(StorageSettingsPreset preset, ZoneManager zoneManager) : base(preset.PresetName(), zoneManager)
		{
			this.settings = new StorageSettings(this);
			this.settings.SetFromPreset(preset);
			this.slotGroup = new SlotGroup(this);
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", new object[]
			{
				this
			});
		}

		
		public override void AddCell(IntVec3 sq)
		{
			base.AddCell(sq);
			if (this.slotGroup != null)
			{
				this.slotGroup.Notify_AddedCell(sq);
			}
		}

		
		public override void RemoveCell(IntVec3 sq)
		{
			base.RemoveCell(sq);
			this.slotGroup.Notify_LostCell(sq);
		}

		
		public override void PostDeregister()
		{
			base.PostDeregister();
			BillUtility.Notify_ZoneStockpileRemoved(this);
		}

		
		public override IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return Zone_Stockpile.ITabs;
		}

		
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in this.n__0())
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

		
		public override IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			yield return DesignatorUtility.FindAllowedDesignator<Designator_ZoneAddStockpile_Expand>();
			yield break;
		}

		
		public SlotGroup GetSlotGroup()
		{
			return this.slotGroup;
		}

		
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

		
		public List<IntVec3> AllSlotCellsList()
		{
			return this.cells;
		}

		
		public StorageSettings GetParentStoreSettings()
		{
			return null;
		}

		
		public StorageSettings GetStoreSettings()
		{
			return this.settings;
		}

		
		public bool Accepts(Thing t)
		{
			return this.settings.AllowedToAccept(t);
		}

		
		public string SlotYielderLabel()
		{
			return this.label;
		}

		
		public void Notify_ReceivedThing(Thing newItem)
		{
			if (newItem.def.storedConceptLearnOpportunity != null)
			{
				LessonAutoActivator.TeachOpportunity(newItem.def.storedConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
		}

		
		public void Notify_LostThing(Thing newItem)
		{
		}

		
		public StorageSettings settings;

		
		public SlotGroup slotGroup;

		
		private static readonly ITab[] ITabs = new ITab[]
		{
			new ITab_Storage()
		};
	}
}
