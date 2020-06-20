using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02001095 RID: 4245
	public class GlobalSettings
	{
		// Token: 0x060064B2 RID: 25778 RVA: 0x002309DC File Offset: 0x0022EBDC
		public void Clear()
		{
			this.map = null;
			this.minBuildings = 0;
			this.minBarracks = 0;
			this.minEmptyNodes = 0;
			this.mainRect = CellRect.Empty;
			this.basePart_buildingsResolved = 0;
			this.basePart_emptyNodesResolved = 0;
			this.basePart_barracksResolved = 0;
			this.basePart_batteriesCoverage = 0f;
			this.basePart_farmsCoverage = 0f;
			this.basePart_powerPlantsCoverage = 0f;
			this.basePart_breweriesCoverage = 0f;
		}

		// Token: 0x04003D45 RID: 15685
		public Map map;

		// Token: 0x04003D46 RID: 15686
		public int minBuildings;

		// Token: 0x04003D47 RID: 15687
		public int minEmptyNodes;

		// Token: 0x04003D48 RID: 15688
		public int minBarracks;

		// Token: 0x04003D49 RID: 15689
		public CellRect mainRect;

		// Token: 0x04003D4A RID: 15690
		public int basePart_buildingsResolved;

		// Token: 0x04003D4B RID: 15691
		public int basePart_emptyNodesResolved;

		// Token: 0x04003D4C RID: 15692
		public int basePart_barracksResolved;

		// Token: 0x04003D4D RID: 15693
		public float basePart_batteriesCoverage;

		// Token: 0x04003D4E RID: 15694
		public float basePart_farmsCoverage;

		// Token: 0x04003D4F RID: 15695
		public float basePart_powerPlantsCoverage;

		// Token: 0x04003D50 RID: 15696
		public float basePart_breweriesCoverage;
	}
}
