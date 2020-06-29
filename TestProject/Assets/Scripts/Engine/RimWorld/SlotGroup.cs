using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class SlotGroup
	{
		
		
		private Map Map
		{
			get
			{
				return this.parent.Map;
			}
		}

		
		
		public StorageSettings Settings
		{
			get
			{
				return this.parent.GetStoreSettings();
			}
		}

		
		
		public IEnumerable<Thing> HeldThings
		{
			get
			{
				List<IntVec3> cellsList = this.CellsList;
				Map map = this.Map;
				int num;
				for (int i = 0; i < cellsList.Count; i = num + 1)
				{
					List<Thing> thingList = map.thingGrid.ThingsListAt(cellsList[i]);
					for (int j = 0; j < thingList.Count; j = num + 1)
					{
						if (thingList[j].def.EverStorable(false))
						{
							yield return thingList[j];
						}
						num = j;
					}
					thingList = null;
					num = i;
				}
				yield break;
			}
		}

		
		
		public List<IntVec3> CellsList
		{
			get
			{
				return this.parent.AllSlotCellsList();
			}
		}

		
		public IEnumerator<IntVec3> GetEnumerator()
		{
			List<IntVec3> cellsList = this.CellsList;
			int num;
			for (int i = 0; i < cellsList.Count; i = num + 1)
			{
				yield return cellsList[i];
				num = i;
			}
			yield break;
		}

		
		public SlotGroup(ISlotGroupParent parent)
		{
			this.parent = parent;
		}

		
		public void Notify_AddedCell(IntVec3 c)
		{
			this.Map.haulDestinationManager.SetCellFor(c, this);
			this.Map.listerHaulables.RecalcAllInCell(c);
			this.Map.listerMergeables.RecalcAllInCell(c);
		}

		
		public void Notify_LostCell(IntVec3 c)
		{
			this.Map.haulDestinationManager.ClearCellFor(c, this);
			this.Map.listerHaulables.RecalcAllInCell(c);
			this.Map.listerMergeables.RecalcAllInCell(c);
		}

		
		public override string ToString()
		{
			if (this.parent != null)
			{
				return this.parent.ToString();
			}
			return "NullParent";
		}

		
		public ISlotGroupParent parent;
	}
}
