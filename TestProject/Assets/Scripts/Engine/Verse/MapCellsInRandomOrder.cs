using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200045C RID: 1116
	public class MapCellsInRandomOrder
	{
		// Token: 0x0600213A RID: 8506 RVA: 0x000CBE05 File Offset: 0x000CA005
		public MapCellsInRandomOrder(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600213B RID: 8507 RVA: 0x000CBE14 File Offset: 0x000CA014
		public List<IntVec3> GetAll()
		{
			this.CreateListIfShould();
			return this.randomizedCells;
		}

		// Token: 0x0600213C RID: 8508 RVA: 0x000CBE22 File Offset: 0x000CA022
		public IntVec3 Get(int index)
		{
			this.CreateListIfShould();
			return this.randomizedCells[index];
		}

		// Token: 0x0600213D RID: 8509 RVA: 0x000CBE38 File Offset: 0x000CA038
		private void CreateListIfShould()
		{
			if (this.randomizedCells != null)
			{
				return;
			}
			this.randomizedCells = new List<IntVec3>(this.map.Area);
			foreach (IntVec3 item in this.map.AllCells)
			{
				this.randomizedCells.Add(item);
			}
			Rand.PushState();
			Rand.Seed = (Find.World.info.Seed ^ this.map.Tile);
			this.randomizedCells.Shuffle<IntVec3>();
			Rand.PopState();
		}

		// Token: 0x04001437 RID: 5175
		private Map map;

		// Token: 0x04001438 RID: 5176
		private List<IntVec3> randomizedCells;
	}
}
