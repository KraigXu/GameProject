using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DA5 RID: 3493
	public class CompProperties_UseEffectPlaySound : CompProperties_Usable
	{
		// Token: 0x060054DB RID: 21723 RVA: 0x001C4819 File Offset: 0x001C2A19
		public CompProperties_UseEffectPlaySound()
		{
			this.compClass = typeof(CompUseEffect_PlaySound);
		}

		// Token: 0x04002E88 RID: 11912
		public SoundDef soundOnUsed;
	}
}
