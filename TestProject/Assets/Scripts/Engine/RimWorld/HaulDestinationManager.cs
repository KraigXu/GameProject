using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C63 RID: 3171
	public sealed class HaulDestinationManager
	{
		// Token: 0x17000D5F RID: 3423
		// (get) Token: 0x06004BF5 RID: 19445 RVA: 0x00198CF7 File Offset: 0x00196EF7
		public IEnumerable<IHaulDestination> AllHaulDestinations
		{
			get
			{
				return this.allHaulDestinationsInOrder;
			}
		}

		// Token: 0x17000D60 RID: 3424
		// (get) Token: 0x06004BF6 RID: 19446 RVA: 0x00198CF7 File Offset: 0x00196EF7
		public List<IHaulDestination> AllHaulDestinationsListForReading
		{
			get
			{
				return this.allHaulDestinationsInOrder;
			}
		}

		// Token: 0x17000D61 RID: 3425
		// (get) Token: 0x06004BF7 RID: 19447 RVA: 0x00198CF7 File Offset: 0x00196EF7
		public List<IHaulDestination> AllHaulDestinationsListInPriorityOrder
		{
			get
			{
				return this.allHaulDestinationsInOrder;
			}
		}

		// Token: 0x17000D62 RID: 3426
		// (get) Token: 0x06004BF8 RID: 19448 RVA: 0x00198CFF File Offset: 0x00196EFF
		public IEnumerable<SlotGroup> AllGroups
		{
			get
			{
				return this.allGroupsInOrder;
			}
		}

		// Token: 0x17000D63 RID: 3427
		// (get) Token: 0x06004BF9 RID: 19449 RVA: 0x00198CFF File Offset: 0x00196EFF
		public List<SlotGroup> AllGroupsListForReading
		{
			get
			{
				return this.allGroupsInOrder;
			}
		}

		// Token: 0x17000D64 RID: 3428
		// (get) Token: 0x06004BFA RID: 19450 RVA: 0x00198CFF File Offset: 0x00196EFF
		public List<SlotGroup> AllGroupsListInPriorityOrder
		{
			get
			{
				return this.allGroupsInOrder;
			}
		}

		// Token: 0x17000D65 RID: 3429
		// (get) Token: 0x06004BFB RID: 19451 RVA: 0x00198D07 File Offset: 0x00196F07
		public IEnumerable<IntVec3> AllSlots
		{
			get
			{
				int num;
				for (int i = 0; i < this.allGroupsInOrder.Count; i = num + 1)
				{
					List<IntVec3> cellsList = this.allGroupsInOrder[i].CellsList;
					int j = 0;
					while (j < this.allGroupsInOrder.Count)
					{
						yield return cellsList[j];
						num = i;
						i = num + 1;
					}
					cellsList = null;
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x06004BFC RID: 19452 RVA: 0x00198D18 File Offset: 0x00196F18
		public HaulDestinationManager(Map map)
		{
			this.map = map;
			this.groupGrid = new SlotGroup[map.Size.x, map.Size.y, map.Size.z];
		}

		// Token: 0x06004BFD RID: 19453 RVA: 0x00198D74 File Offset: 0x00196F74
		public void AddHaulDestination(IHaulDestination haulDestination)
		{
			if (this.allHaulDestinationsInOrder.Contains(haulDestination))
			{
				Log.Error("Double-added haul destination " + haulDestination.ToStringSafe<IHaulDestination>(), false);
				return;
			}
			this.allHaulDestinationsInOrder.Add(haulDestination);
			this.allHaulDestinationsInOrder.InsertionSort(new Comparison<IHaulDestination>(HaulDestinationManager.CompareHaulDestinationPrioritiesDescending));
			ISlotGroupParent slotGroupParent = haulDestination as ISlotGroupParent;
			if (slotGroupParent != null)
			{
				SlotGroup slotGroup = slotGroupParent.GetSlotGroup();
				if (slotGroup == null)
				{
					Log.Error("ISlotGroupParent gave null slot group: " + slotGroupParent.ToStringSafe<ISlotGroupParent>(), false);
					return;
				}
				this.allGroupsInOrder.Add(slotGroup);
				this.allGroupsInOrder.InsertionSort(new Comparison<SlotGroup>(HaulDestinationManager.CompareSlotGroupPrioritiesDescending));
				List<IntVec3> cellsList = slotGroup.CellsList;
				for (int i = 0; i < cellsList.Count; i++)
				{
					this.SetCellFor(cellsList[i], slotGroup);
				}
				this.map.listerHaulables.Notify_SlotGroupChanged(slotGroup);
				this.map.listerMergeables.Notify_SlotGroupChanged(slotGroup);
			}
		}

		// Token: 0x06004BFE RID: 19454 RVA: 0x00198E64 File Offset: 0x00197064
		public void RemoveHaulDestination(IHaulDestination haulDestination)
		{
			if (!this.allHaulDestinationsInOrder.Contains(haulDestination))
			{
				Log.Error("Removing haul destination that isn't registered " + haulDestination.ToStringSafe<IHaulDestination>(), false);
				return;
			}
			this.allHaulDestinationsInOrder.Remove(haulDestination);
			ISlotGroupParent slotGroupParent = haulDestination as ISlotGroupParent;
			if (slotGroupParent != null)
			{
				SlotGroup slotGroup = slotGroupParent.GetSlotGroup();
				if (slotGroup == null)
				{
					Log.Error("ISlotGroupParent gave null slot group: " + slotGroupParent.ToStringSafe<ISlotGroupParent>(), false);
					return;
				}
				this.allGroupsInOrder.Remove(slotGroup);
				List<IntVec3> cellsList = slotGroup.CellsList;
				for (int i = 0; i < cellsList.Count; i++)
				{
					IntVec3 intVec = cellsList[i];
					this.groupGrid[intVec.x, intVec.y, intVec.z] = null;
				}
				this.map.listerHaulables.Notify_SlotGroupChanged(slotGroup);
				this.map.listerMergeables.Notify_SlotGroupChanged(slotGroup);
			}
		}

		// Token: 0x06004BFF RID: 19455 RVA: 0x00198F42 File Offset: 0x00197142
		public void Notify_HaulDestinationChangedPriority()
		{
			this.allHaulDestinationsInOrder.InsertionSort(new Comparison<IHaulDestination>(HaulDestinationManager.CompareHaulDestinationPrioritiesDescending));
			this.allGroupsInOrder.InsertionSort(new Comparison<SlotGroup>(HaulDestinationManager.CompareSlotGroupPrioritiesDescending));
		}

		// Token: 0x06004C00 RID: 19456 RVA: 0x00198F74 File Offset: 0x00197174
		private static int CompareHaulDestinationPrioritiesDescending(IHaulDestination a, IHaulDestination b)
		{
			return ((int)b.GetStoreSettings().Priority).CompareTo((int)a.GetStoreSettings().Priority);
		}

		// Token: 0x06004C01 RID: 19457 RVA: 0x00198FA0 File Offset: 0x001971A0
		private static int CompareSlotGroupPrioritiesDescending(SlotGroup a, SlotGroup b)
		{
			return ((int)b.Settings.Priority).CompareTo((int)a.Settings.Priority);
		}

		// Token: 0x06004C02 RID: 19458 RVA: 0x00198FCB File Offset: 0x001971CB
		public SlotGroup SlotGroupAt(IntVec3 loc)
		{
			return this.groupGrid[loc.x, loc.y, loc.z];
		}

		// Token: 0x06004C03 RID: 19459 RVA: 0x00198FEC File Offset: 0x001971EC
		public ISlotGroupParent SlotGroupParentAt(IntVec3 loc)
		{
			SlotGroup slotGroup = this.SlotGroupAt(loc);
			if (slotGroup == null)
			{
				return null;
			}
			return slotGroup.parent;
		}

		// Token: 0x06004C04 RID: 19460 RVA: 0x0019900C File Offset: 0x0019720C
		public void SetCellFor(IntVec3 c, SlotGroup group)
		{
			if (this.SlotGroupAt(c) != null)
			{
				Log.Error(string.Concat(new object[]
				{
					group,
					" overwriting slot group square ",
					c,
					" of ",
					this.SlotGroupAt(c)
				}), false);
			}
			this.groupGrid[c.x, c.y, c.z] = group;
		}

		// Token: 0x06004C05 RID: 19461 RVA: 0x00199078 File Offset: 0x00197278
		public void ClearCellFor(IntVec3 c, SlotGroup group)
		{
			if (this.SlotGroupAt(c) != group)
			{
				Log.Error(string.Concat(new object[]
				{
					group,
					" clearing group grid square ",
					c,
					" containing ",
					this.SlotGroupAt(c)
				}), false);
			}
			this.groupGrid[c.x, c.y, c.z] = null;
		}

		// Token: 0x04002AD8 RID: 10968
		private Map map;

		// Token: 0x04002AD9 RID: 10969
		private List<IHaulDestination> allHaulDestinationsInOrder = new List<IHaulDestination>();

		// Token: 0x04002ADA RID: 10970
		private List<SlotGroup> allGroupsInOrder = new List<SlotGroup>();

		// Token: 0x04002ADB RID: 10971
		private SlotGroup[,,] groupGrid;
	}
}
