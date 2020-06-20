using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C67 RID: 3175
	public class SlotGroup
	{
		// Token: 0x17000D6A RID: 3434
		// (get) Token: 0x06004C13 RID: 19475 RVA: 0x001990E5 File Offset: 0x001972E5
		private Map Map
		{
			get
			{
				return this.parent.Map;
			}
		}

		// Token: 0x17000D6B RID: 3435
		// (get) Token: 0x06004C14 RID: 19476 RVA: 0x001990F2 File Offset: 0x001972F2
		public StorageSettings Settings
		{
			get
			{
				return this.parent.GetStoreSettings();
			}
		}

		// Token: 0x17000D6C RID: 3436
		// (get) Token: 0x06004C15 RID: 19477 RVA: 0x001990FF File Offset: 0x001972FF
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

		// Token: 0x17000D6D RID: 3437
		// (get) Token: 0x06004C16 RID: 19478 RVA: 0x0019910F File Offset: 0x0019730F
		public List<IntVec3> CellsList
		{
			get
			{
				return this.parent.AllSlotCellsList();
			}
		}

		// Token: 0x06004C17 RID: 19479 RVA: 0x0019911C File Offset: 0x0019731C
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

		// Token: 0x06004C18 RID: 19480 RVA: 0x0019912B File Offset: 0x0019732B
		public SlotGroup(ISlotGroupParent parent)
		{
			this.parent = parent;
		}

		// Token: 0x06004C19 RID: 19481 RVA: 0x0019913A File Offset: 0x0019733A
		public void Notify_AddedCell(IntVec3 c)
		{
			this.Map.haulDestinationManager.SetCellFor(c, this);
			this.Map.listerHaulables.RecalcAllInCell(c);
			this.Map.listerMergeables.RecalcAllInCell(c);
		}

		// Token: 0x06004C1A RID: 19482 RVA: 0x00199170 File Offset: 0x00197370
		public void Notify_LostCell(IntVec3 c)
		{
			this.Map.haulDestinationManager.ClearCellFor(c, this);
			this.Map.listerHaulables.RecalcAllInCell(c);
			this.Map.listerMergeables.RecalcAllInCell(c);
		}

		// Token: 0x06004C1B RID: 19483 RVA: 0x001991A6 File Offset: 0x001973A6
		public override string ToString()
		{
			if (this.parent != null)
			{
				return this.parent.ToString();
			}
			return "NullParent";
		}

		// Token: 0x04002ADC RID: 10972
		public ISlotGroupParent parent;
	}
}
