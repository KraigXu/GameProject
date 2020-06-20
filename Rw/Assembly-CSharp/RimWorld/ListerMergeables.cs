using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A40 RID: 2624
	public class ListerMergeables
	{
		// Token: 0x06003E0B RID: 15883 RVA: 0x00146F64 File Offset: 0x00145164
		public ListerMergeables(Map map)
		{
			this.map = map;
		}

		// Token: 0x06003E0C RID: 15884 RVA: 0x00146F89 File Offset: 0x00145189
		public List<Thing> ThingsPotentiallyNeedingMerging()
		{
			return this.mergeables;
		}

		// Token: 0x06003E0D RID: 15885 RVA: 0x00146F91 File Offset: 0x00145191
		public void Notify_Spawned(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06003E0E RID: 15886 RVA: 0x00146F9A File Offset: 0x0014519A
		public void Notify_DeSpawned(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06003E0F RID: 15887 RVA: 0x00146F91 File Offset: 0x00145191
		public void Notify_Unforbidden(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x06003E10 RID: 15888 RVA: 0x00146F9A File Offset: 0x0014519A
		public void Notify_Forbidden(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x06003E11 RID: 15889 RVA: 0x00146FA4 File Offset: 0x001451A4
		public void Notify_SlotGroupChanged(SlotGroup sg)
		{
			if (sg.CellsList != null)
			{
				for (int i = 0; i < sg.CellsList.Count; i++)
				{
					this.RecalcAllInCell(sg.CellsList[i]);
				}
			}
		}

		// Token: 0x06003E12 RID: 15890 RVA: 0x00146FE1 File Offset: 0x001451E1
		public void Notify_ThingStackChanged(Thing t)
		{
			this.Check(t);
		}

		// Token: 0x06003E13 RID: 15891 RVA: 0x00146FEC File Offset: 0x001451EC
		public void RecalcAllInCell(IntVec3 c)
		{
			List<Thing> thingList = c.GetThingList(this.map);
			for (int i = 0; i < thingList.Count; i++)
			{
				this.Check(thingList[i]);
			}
		}

		// Token: 0x06003E14 RID: 15892 RVA: 0x00147024 File Offset: 0x00145224
		private void Check(Thing t)
		{
			if (this.ShouldBeMergeable(t))
			{
				if (!this.mergeables.Contains(t))
				{
					this.mergeables.Add(t);
					return;
				}
			}
			else
			{
				this.mergeables.Remove(t);
			}
		}

		// Token: 0x06003E15 RID: 15893 RVA: 0x00147057 File Offset: 0x00145257
		private bool ShouldBeMergeable(Thing t)
		{
			return !t.IsForbidden(Faction.OfPlayer) && t.GetSlotGroup() != null && t.stackCount != t.def.stackLimit;
		}

		// Token: 0x06003E16 RID: 15894 RVA: 0x00147088 File Offset: 0x00145288
		private void CheckAdd(Thing t)
		{
			if (this.ShouldBeMergeable(t) && !this.mergeables.Contains(t))
			{
				this.mergeables.Add(t);
			}
		}

		// Token: 0x06003E17 RID: 15895 RVA: 0x001470AD File Offset: 0x001452AD
		private void TryRemove(Thing t)
		{
			if (t.def.category == ThingCategory.Item)
			{
				this.mergeables.Remove(t);
			}
		}

		// Token: 0x06003E18 RID: 15896 RVA: 0x001470CC File Offset: 0x001452CC
		internal string DebugString()
		{
			if (Time.frameCount % 10 == 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("======= All mergeables (Count " + this.mergeables.Count + ")");
				int num = 0;
				foreach (Thing thing in this.mergeables)
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

		// Token: 0x0400242A RID: 9258
		private Map map;

		// Token: 0x0400242B RID: 9259
		private List<Thing> mergeables = new List<Thing>();

		// Token: 0x0400242C RID: 9260
		private string debugOutput = "uninitialized";
	}
}
