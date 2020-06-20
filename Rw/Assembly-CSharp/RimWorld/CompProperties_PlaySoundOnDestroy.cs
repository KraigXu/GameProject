using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D38 RID: 3384
	public class CompProperties_PlaySoundOnDestroy : CompProperties
	{
		// Token: 0x06005233 RID: 21043 RVA: 0x001B770D File Offset: 0x001B590D
		public CompProperties_PlaySoundOnDestroy()
		{
			this.compClass = typeof(CompPlaySoundOnDestroy);
		}

		// Token: 0x04002D59 RID: 11609
		public SoundDef sound;
	}
}
