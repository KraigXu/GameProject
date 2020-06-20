using System;

namespace RimWorld
{
	// Token: 0x02000D2F RID: 3375
	public class CompProperties_MoteEmitterProximityScan : CompProperties_MoteEmitter
	{
		// Token: 0x06005204 RID: 20996 RVA: 0x001B67AB File Offset: 0x001B49AB
		public CompProperties_MoteEmitterProximityScan()
		{
			this.compClass = typeof(CompMoteEmitterProximityScan);
		}

		// Token: 0x04002D33 RID: 11571
		public float warmupPulseFadeInTime;

		// Token: 0x04002D34 RID: 11572
		public float warmupPulseSolidTime;

		// Token: 0x04002D35 RID: 11573
		public float warmupPulseFadeOutTime;
	}
}
