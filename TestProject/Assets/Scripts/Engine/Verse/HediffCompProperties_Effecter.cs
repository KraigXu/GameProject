using System;

namespace Verse
{
	// Token: 0x02000253 RID: 595
	public class HediffCompProperties_Effecter : HediffCompProperties
	{
		// Token: 0x06001063 RID: 4195 RVA: 0x0005DD5C File Offset: 0x0005BF5C
		public HediffCompProperties_Effecter()
		{
			this.compClass = typeof(HediffComp_Effecter);
		}

		// Token: 0x04000BF6 RID: 3062
		public EffecterDef stateEffecter;

		// Token: 0x04000BF7 RID: 3063
		public IntRange severityIndices = new IntRange(-1, -1);
	}
}
