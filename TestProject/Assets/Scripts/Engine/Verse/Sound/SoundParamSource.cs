using System;

namespace Verse.Sound
{
	// Token: 0x020004DC RID: 1244
	[EditorShowClassName]
	[EditorReplaceable]
	public abstract class SoundParamSource
	{
		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x0600244E RID: 9294
		public abstract string Label { get; }

		// Token: 0x0600244F RID: 9295
		public abstract float ValueFor(Sample samp);
	}
}
