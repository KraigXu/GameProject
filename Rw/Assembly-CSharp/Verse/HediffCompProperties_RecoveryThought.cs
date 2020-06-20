using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000269 RID: 617
	public class HediffCompProperties_RecoveryThought : HediffCompProperties
	{
		// Token: 0x060010AD RID: 4269 RVA: 0x0005EEF1 File Offset: 0x0005D0F1
		public HediffCompProperties_RecoveryThought()
		{
			this.compClass = typeof(HediffComp_RecoveryThought);
		}

		// Token: 0x04000C27 RID: 3111
		public ThoughtDef thought;
	}
}
