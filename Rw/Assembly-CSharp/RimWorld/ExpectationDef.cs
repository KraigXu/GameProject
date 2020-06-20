using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008BD RID: 2237
	public class ExpectationDef : Def
	{
		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x060035E5 RID: 13797 RVA: 0x00124DBC File Offset: 0x00122FBC
		public bool WealthTriggered
		{
			get
			{
				return this.maxMapWealth >= 0f;
			}
		}

		// Token: 0x060035E6 RID: 13798 RVA: 0x00124DCE File Offset: 0x00122FCE
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.order < 0)
			{
				yield return "order not defined";
			}
			yield break;
		}

		// Token: 0x04001DD0 RID: 7632
		public int order = -1;

		// Token: 0x04001DD1 RID: 7633
		public int thoughtStage = -1;

		// Token: 0x04001DD2 RID: 7634
		public float maxMapWealth = -1f;

		// Token: 0x04001DD3 RID: 7635
		public float joyToleranceDropPerDay;

		// Token: 0x04001DD4 RID: 7636
		public int joyKindsNeeded;
	}
}
