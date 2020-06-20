using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A3E RID: 2622
	public class ListerFilthInHomeArea
	{
		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x06003DF2 RID: 15858 RVA: 0x00146942 File Offset: 0x00144B42
		public List<Thing> FilthInHomeArea
		{
			get
			{
				return this.filthInHomeArea;
			}
		}

		// Token: 0x06003DF3 RID: 15859 RVA: 0x0014694A File Offset: 0x00144B4A
		public ListerFilthInHomeArea(Map map)
		{
			this.map = map;
		}

		// Token: 0x06003DF4 RID: 15860 RVA: 0x00146964 File Offset: 0x00144B64
		public void RebuildAll()
		{
			this.filthInHomeArea.Clear();
			foreach (IntVec3 c in this.map.AllCells)
			{
				this.Notify_HomeAreaChanged(c);
			}
		}

		// Token: 0x06003DF5 RID: 15861 RVA: 0x001469C4 File Offset: 0x00144BC4
		public void Notify_FilthSpawned(Filth f)
		{
			if (this.map.areaManager.Home[f.Position])
			{
				this.filthInHomeArea.Add(f);
			}
		}

		// Token: 0x06003DF6 RID: 15862 RVA: 0x001469F0 File Offset: 0x00144BF0
		public void Notify_FilthDespawned(Filth f)
		{
			for (int i = 0; i < this.filthInHomeArea.Count; i++)
			{
				if (this.filthInHomeArea[i] == f)
				{
					this.filthInHomeArea.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x06003DF7 RID: 15863 RVA: 0x00146A30 File Offset: 0x00144C30
		public void Notify_HomeAreaChanged(IntVec3 c)
		{
			if (this.map.areaManager.Home[c])
			{
				List<Thing> thingList = c.GetThingList(this.map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Filth filth = thingList[i] as Filth;
					if (filth != null)
					{
						this.filthInHomeArea.Add(filth);
					}
				}
				return;
			}
			for (int j = this.filthInHomeArea.Count - 1; j >= 0; j--)
			{
				if (this.filthInHomeArea[j].Position == c)
				{
					this.filthInHomeArea.RemoveAt(j);
				}
			}
		}

		// Token: 0x06003DF8 RID: 15864 RVA: 0x00146AD0 File Offset: 0x00144CD0
		internal string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("======= Filth in home area");
			foreach (Thing thing in this.filthInHomeArea)
			{
				stringBuilder.AppendLine(thing.ThingID + " " + thing.Position);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04002422 RID: 9250
		private Map map;

		// Token: 0x04002423 RID: 9251
		private List<Thing> filthInHomeArea = new List<Thing>();
	}
}
