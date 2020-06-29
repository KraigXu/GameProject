﻿using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class Building_Storage : Building, ISlotGroupParent, IStoreSettingsParent, IHaulDestination
	{
		
		public Building_Storage()
		{
			this.slotGroup = new SlotGroup(this);
		}

		
		// (get) Token: 0x06004BE5 RID: 19429 RVA: 0x0001028D File Offset: 0x0000E48D
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		
		// (get) Token: 0x06004BE6 RID: 19430 RVA: 0x00198BC6 File Offset: 0x00196DC6
		public bool IgnoreStoredThingsBeauty
		{
			get
			{
				return this.def.building.ignoreStoredThingsBeauty;
			}
		}

		
		public SlotGroup GetSlotGroup()
		{
			return this.slotGroup;
		}

		
		public virtual void Notify_ReceivedThing(Thing newItem)
		{
			if (base.Faction == Faction.OfPlayer && newItem.def.storedConceptLearnOpportunity != null)
			{
				LessonAutoActivator.TeachOpportunity(newItem.def.storedConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
		}

		
		public virtual void Notify_LostThing(Thing newItem)
		{
		}

		
		public virtual IEnumerable<IntVec3> AllSlotCells()
		{
			foreach (IntVec3 intVec in GenAdj.CellsOccupiedBy(this))
			{
				yield return intVec;
			}
			IEnumerator<IntVec3> enumerator = null;
			yield break;
			yield break;
		}

		
		public List<IntVec3> AllSlotCellsList()
		{
			if (this.cachedOccupiedCells == null)
			{
				this.cachedOccupiedCells = this.AllSlotCells().ToList<IntVec3>();
			}
			return this.cachedOccupiedCells;
		}

		
		public StorageSettings GetStoreSettings()
		{
			return this.settings;
		}

		
		public StorageSettings GetParentStoreSettings()
		{
			return this.def.building.fixedStorageSettings;
		}

		
		public string SlotYielderLabel()
		{
			return this.LabelCap;
		}

		
		public bool Accepts(Thing t)
		{
			return this.settings.AllowedToAccept(t);
		}

		
		public override void PostMake()
		{
			base.PostMake();
			this.settings = new StorageSettings(this);
			if (this.def.building.defaultStorageSettings != null)
			{
				this.settings.CopyFrom(this.def.building.defaultStorageSettings);
			}
		}

		
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			this.cachedOccupiedCells = null;
			base.SpawnSetup(map, respawningAfterLoad);
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", new object[]
			{
				this
			});
		}

		
		public override IEnumerable<Gizmo> GetGizmos()
		{

			IEnumerator<Gizmo> enumerator = null;
			foreach (Gizmo gizmo2 in StorageSettingsClipboard.CopyPasteGizmosFor(this.settings))
			{
				yield return gizmo2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		
		public StorageSettings settings;

		
		public SlotGroup slotGroup;

		
		private List<IntVec3> cachedOccupiedCells;
	}
}
