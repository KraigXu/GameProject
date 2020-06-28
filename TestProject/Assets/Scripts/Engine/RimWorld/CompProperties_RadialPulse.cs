using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D3F RID: 3391
	public class CompProperties_RadialPulse : CompProperties
	{
		// Token: 0x0600526A RID: 21098 RVA: 0x001B8D51 File Offset: 0x001B6F51
		public CompProperties_RadialPulse()
		{
			this.compClass = typeof(CompRadialPulse);
		}

		// Token: 0x04002D7A RID: 11642
		public int ticksBetweenPulses = 300;

		// Token: 0x04002D7B RID: 11643
		public int ticksPerPulse = 60;

		// Token: 0x04002D7C RID: 11644
		public Color color;

		// Token: 0x04002D7D RID: 11645
		public float radius;
	}
}
