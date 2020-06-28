using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C62 RID: 3170
	public class Building_Storage : Building, ISlotGroupParent, IStoreSettingsParent, IHaulDestination
	{
		// Token: 0x06004BE4 RID: 19428 RVA: 0x00198BB2 File Offset: 0x00196DB2
		public Building_Storage()
		{
			this.slotGroup = new SlotGroup(this);
		}

		// Token: 0x17000D5D RID: 3421
		// (get) Token: 0x06004BE5 RID: 19429 RVA: 0x0001028D File Offset: 0x0000E48D
		public bool StorageTabVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D5E RID: 3422
		// (get) Token: 0x06004BE6 RID: 19430 RVA: 0x00198BC6 File Offset: 0x00196DC6
		public bool IgnoreStoredThingsBeauty
		{
			get
			{
				return this.def.building.ignoreStoredThingsBeauty;
			}
		}

		// Token: 0x06004BE7 RID: 19431 RVA: 0x00198BD8 File Offset: 0x00196DD8
		public SlotGroup GetSlotGroup()
		{
			return this.slotGroup;
		}

		// Token: 0x06004BE8 RID: 19432 RVA: 0x00198BE0 File Offset: 0x00196DE0
		public virtual void Notify_ReceivedThing(Thing newItem)
		{
			if (base.Faction == Faction.OfPlayer && newItem.def.storedConceptLearnOpportunity != null)
			{
				LessonAutoActivator.TeachOpportunity(newItem.def.storedConceptLearnOpportunity, OpportunityType.GoodToKnow);
			}
		}

		// Token: 0x06004BE9 RID: 19433 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_LostThing(Thing newItem)
		{
		}

		// Token: 0x06004BEA RID: 19434 RVA: 0x00198C0D File Offset: 0x00196E0D
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

		// Token: 0x06004BEB RID: 19435 RVA: 0x00198C1D File Offset: 0x00196E1D
		public List<IntVec3> AllSlotCellsList()
		{
			if (this.cachedOccupiedCells == null)
			{
				this.cachedOccupiedCells = this.AllSlotCells().ToList<IntVec3>();
			}
			return this.cachedOccupiedCells;
		}

		// Token: 0x06004BEC RID: 19436 RVA: 0x00198C3E File Offset: 0x00196E3E
		public StorageSettings GetStoreSettings()
		{
			return this.settings;
		}

		// Token: 0x06004BED RID: 19437 RVA: 0x00198C46 File Offset: 0x00196E46
		public StorageSettings GetParentStoreSettings()
		{
			return this.def.building.fixedStorageSettings;
		}

		// Token: 0x06004BEE RID: 19438 RVA: 0x000255AA File Offset: 0x000237AA
		public string SlotYielderLabel()
		{
			return this.LabelCap;
		}

		// Token: 0x06004BEF RID: 19439 RVA: 0x00198C58 File Offset: 0x00196E58
		public bool Accepts(Thing t)
		{
			return this.settings.AllowedToAccept(t);
		}

		// Token: 0x06004BF0 RID: 19440 RVA: 0x00198C68 File Offset: 0x00196E68
		public override void PostMake()
		{
			base.PostMake();
			this.settings = new StorageSettings(this);
			if (this.def.building.defaultStorageSettings != null)
			{
				this.settings.CopyFrom(this.def.building.defaultStorageSettings);
			}
		}

		// Token: 0x06004BF1 RID: 19441 RVA: 0x00198CB4 File Offset: 0x00196EB4
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			this.cachedOccupiedCells = null;
			base.SpawnSetup(map, respawningAfterLoad);
		}

		// Token: 0x06004BF2 RID: 19442 RVA: 0x00198CC5 File Offset: 0x00196EC5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", new object[]
			{
				this
			});
		}

		// Token: 0x06004BF3 RID: 19443 RVA: 0x00198CE7 File Offset: 0x00196EE7
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

		// Token: 0x04002AD5 RID: 10965
		public StorageSettings settings;

		// Token: 0x04002AD6 RID: 10966
		public SlotGroup slotGroup;

		// Token: 0x04002AD7 RID: 10967
		private List<IntVec3> cachedOccupiedCells;
	}
}
