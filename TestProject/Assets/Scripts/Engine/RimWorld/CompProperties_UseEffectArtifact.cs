using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D9C RID: 3484
	public class CompProperties_UseEffectArtifact : CompProperties_UseEffect
	{
		// Token: 0x060054B2 RID: 21682 RVA: 0x001C3A0D File Offset: 0x001C1C0D
		public CompProperties_UseEffectArtifact()
		{
			this.compClass = typeof(CompUseEffect_Artifact);
		}

		// Token: 0x04002E82 RID: 11906
		public SoundDef sound;
	}
}
