using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public sealed class HaulDestinationManager
	{
		
		// (get) Token: 0x06004BF5 RID: 19445 RVA: 0x00198CF7 File Offset: 0x00196EF7
		public IEnumerable<IHaulDestination> AllHaulDestinations
		{
			get
			{
				return this.allHaulDestinationsInOrder;
			}
		}

		
		// (get) Token: 0x06004BF6 RID: 19446 RVA: 0x00198CF7 File Offset: 0x00196EF7
		public List<IHaulDestination> AllHaulDestinationsListForReading
		{
			get
			{
				return this.allHaulDestinationsInOrder;
			}
		}

		
		// (get) Token: 0x06004BF7 RID: 19447 RVA: 0x00198CF7 File Offset: 0x00196EF7
		public List<IHaulDestination> AllHaulDestinationsListInPriorityOrder
		{
			get
			{
				return this.allHaulDestinationsInOrder;
			}
		}

		
		// (get) Token: 0x06004BF8 RID: 19448 RVA: 0x00198CFF File Offset: 0x00196EFF
		public IEnumerable<SlotGroup> AllGroups
		{
			get
			{
				return this.allGroupsInOrder;
			}
		}

		
		// (get) Token: 0x06004BF9 RID: 19449 RVA: 0x00198CFF File Offset: 0x00196EFF
		public List<SlotGroup> AllGroupsListForReading
		{
			get
			{
				return this.allGroupsInOrder;
			}
		}

		
		// (get) Token: 0x06004BFA RID: 19450 RVA: 0x00198CFF File Offset: 0x00196EFF
		public List<SlotGroup> AllGroupsListInPriorityOrder
		{
			get
			{
				return this.allGroupsInOrder;
			}
		}

		
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

		
		public HaulDestinationManager(Map map)
		{
			this.map = map;
			this.groupGrid = new SlotGroup[map.Size.x, map.Size.y, map.Size.z];
		}

		
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

		
		public void Notify_HaulDestinationChangedPriority()
		{
			this.allHaulDestinationsInOrder.InsertionSort(new Comparison<IHaulDestination>(HaulDestinationManager.CompareHaulDestinationPrioritiesDescending));
			this.allGroupsInOrder.InsertionSort(new Comparison<SlotGroup>(HaulDestinationManager.CompareSlotGroupPrioritiesDescending));
		}

		
		private static int CompareHaulDestinationPrioritiesDescending(IHaulDestination a, IHaulDestination b)
		{
			return ((int)b.GetStoreSettings().Priority).CompareTo((int)a.GetStoreSettings().Priority);
		}

		
		private static int CompareSlotGroupPrioritiesDescending(SlotGroup a, SlotGroup b)
		{
			return ((int)b.Settings.Priority).CompareTo((int)a.Settings.Priority);
		}

		
		public SlotGroup SlotGroupAt(IntVec3 loc)
		{
			return this.groupGrid[loc.x, loc.y, loc.z];
		}

		
		public ISlotGroupParent SlotGroupParentAt(IntVec3 loc)
		{
			SlotGroup slotGroup = this.SlotGroupAt(loc);
			if (slotGroup == null)
			{
				return null;
			}
			return slotGroup.parent;
		}

		
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

		
		private Map map;

		
		private List<IHaulDestination> allHaulDestinationsInOrder = new List<IHaulDestination>();

		
		private List<SlotGroup> allGroupsInOrder = new List<SlotGroup>();

		
		private SlotGroup[,,] groupGrid;
	}
}
