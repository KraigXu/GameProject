using System;

namespace Verse
{
	// Token: 0x02000260 RID: 608
	public class HediffCompProperties_Infecter : HediffCompProperties
	{
		// Token: 0x0600108B RID: 4235 RVA: 0x0005E706 File Offset: 0x0005C906
		public HediffCompProperties_Infecter()
		{
			this.compClass = typeof(HediffComp_Infecter);
		}

		// Token: 0x04000C13 RID: 3091
		public float infectionChance = 0.5f;
	}
}
