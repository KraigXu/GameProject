using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A3F RID: 2623
	public class ListerHaulables
	{
		// Token: 0x06003DF9 RID: 15865 RVA: 0x00146B58 File Offset: 0x00144D58
		public ListerHaulables(Map map)
		{
			this.map = map;
		}

		// Token: 0x06003DFA RID: 15866 RVA: 0x00146B88 File Offset: 0x00144D88
		public List<Thing> ThingsPotentiallyNeedingHauling()
		{
			return this.haulables;
		}

		// Token: 0x06003DFB RID: 15867 RVA: 0x00146B90 File Offset: 0x00144D90
		public void Notify_Spawned(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06003DFC RID: 15868 RVA: 0x00146B99 File Offset: 0x00144D99
		public void Notify_DeSpawned(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06003DFD RID: 15869 RVA: 0x00146B90 File Offset: 0x00144D90
		public void HaulDesignationAdded(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06003DFE RID: 15870 RVA: 0x00146B99 File Offset: 0x00144D99
		public void HaulDesignationRemoved(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06003DFF RID: 15871 RVA: 0x00146B90 File Offset: 0x00144D90
		public void Notify_Unforbidden(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06003E00 RID: 15872 RVA: 0x00146B99 File Offset: 0x00144D99
		public void Notify_Forbidden(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06003E01 RID: 15873 RVA: 0x00146BA4 File Offset: 0x00144DA4
		public void Notify_SlotGroupChanged(SlotGroup sg)
		{
			List<IntVec3> cellsList = sg.CellsList;
			if (cellsList != null)
			{
				for (int i = 0; i < cellsList.Count; i++)
				{
					this.RecalcAllInCell(cellsList[i]);
				}
			}
		}

		// Token: 0x06003E02 RID: 15874 RVA: 0x00146BDC File Offset: 0x00144DDC
		public void ListerHaulablesTick()
		{
			ListerHaulables.groupCycleIndex++;
			if (ListerHaulables.groupCycleIndex >= 2147473647)
			{
				ListerHaulables.groupCycleIndex = 0;
			}
			List<SlotGroup> allGroupsListForReading = this.map.haulDestinationManager.AllGroupsListForReading;
			if (allGroupsListForReading.Count == 0)
			{
				return;
			}
			int num = ListerHaulables.groupCycleIndex % allGroupsListForReading.Count;
			SlotGroup slotGroup = allGroupsListForReading[ListerHaulables.groupCycleIndex % allGroupsListForReading.Count];
			if (slotGroup.CellsList.Count != 0)
			{
				while (this.cellCycleIndices.Count <= num)
				{
					this.cellCycleIndices.Add(0);
				}
				if (this.cellCycleIndices[num] >= 2147473647)
				{
					this.cellCycleIndices[num] = 0;
				}
				for (int i = 0; i < 4; i++)
				{
					List<int> list = this.cellCycleIndices;
					int index = num;
					int num2 = list[index];
					list[index] = num2 + 1;
					List<Thing> thingList = slotGroup.CellsList[this.cellCycleIndices[num] % slotGroup.CellsList.Count].GetThingList(this.map);
					for (int j = 0; j < thingList.Count; j++)
					{
						if (thingList[j].def.EverHaulable)
						{
							this.Check(thingList[j]);
							break;
						}
					}
				}
			}
		}

		// Token: 0x06003E03 RID: 15875 RVA: 0x00146D2C File Offset: 0x00144F2C
		public void RecalcAllInCell(IntVec3 c)
		{
			List<Thing> thingList = c.GetThingList(this.map);
			for (int i = 0; i < thingList.Count; i++)
			{
				this.Check(thingList[i]);
			}
		}

		// Token: 0x06003E04 RID: 15876 RVA: 0x00146D64 File Offset: 0x00144F64
		public void RecalcAllInCells(IEnumerable<IntVec3> cells)
		{
			foreach (IntVec3 c in cells)
			{
				this.RecalcAllInCell(c);
			}
		}

		// Token: 0x06003E05 RID: 15877 RVA: 0x00146DAC File Offset: 0x00144FAC
		private void Check(Thing t)
		{
			if (this.ShouldBeHaulable(t))
			{
				if (!this.haulables.Contains(t))
				{
					this.haulables.Add(t);
					return;
				}
			}
			else if (this.haulables.Contains(t))
			{
				this.haulables.Remove(t);
			}
		}

		// Token: 0x06003E06 RID: 15878 RVA: 0x00146DF8 File Offset: 0x00144FF8
		private bool ShouldBeHaulable(Thing t)
		{
			if (t.IsForbidden(Faction.OfPlayer))
			{
				return false;
			}
			if (!t.def.alwaysHaulable)
			{
				if (!t.def.EverHaulable)
				{
					return false;
				}
				if (this.map.designationManager.DesignationOn(t, DesignationDefOf.Haul) == null && !t.IsInAnyStorage())
				{
					return false;
				}
			}
			return !t.IsInValidBestStorage();
		}

		// Token: 0x06003E07 RID: 15879 RVA: 0x00146E5D File Offset: 0x0014505D
		private void CheckAdd(Thing t)
		{
			if (this.ShouldBeHaulable(t) && !this.haulables.Contains(t))
			{
				this.haulables.Add(t);
			}
		}

		// Token: 0x06003E08 RID: 15880 RVA: 0x00146E82 File Offset: 0x00145082
		private void TryRemove(Thing t)
		{
			if (t.def.category == ThingCategory.Item && this.haulables.Contains(t))
			{
				this.haulables.Remove(t);
			}
		}

		// Token: 0x06003E09 RID: 15881 RVA: 0x00146EB0 File Offset: 0x001450B0
		internal string DebugString()
		{
			if (Time.frameCount % 10 == 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("======= All haulables (Count " + this.haulables.Count + ")");
				int num = 0;
				foreach (Thing thing in this.haulables)
				{
					stringBuilder.AppendLine(thing.ThingID);
					num++;
					if (num > 200)
					{
						break;
					}
				}
				this.debugOutput = stringBuilder.ToString();
			}
			return this.debugOutput;
		}

		// Token: 0x04002424 RID: 9252
		private Map map;

		// Token: 0x04002425 RID: 9253
		private List<Thing> haulables = new List<Thing>();

		// Token: 0x04002426 RID: 9254
		private const int CellsPerTick = 4;

		// Token: 0x04002427 RID: 9255
		private static int groupCycleIndex;

		// Token: 0x04002428 RID: 9256
		private List<int> cellCycleIndices = new List<int>();

		// Token: 0x04002429 RID: 9257
		private string debugOutput = "uninitialized";
	}
}
