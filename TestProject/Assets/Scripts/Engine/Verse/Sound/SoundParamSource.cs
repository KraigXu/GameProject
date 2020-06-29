using System;

namespace Verse.Sound
{
	
	[EditorShowClassName]
	[EditorReplaceable]
	public abstract class SoundParamSource
	{
		
		// (get) Token: 0x0600244E RID: 9294
		public abstract string Label { get; }

		
		public abstract float ValueFor(Sample samp);
	}
}
