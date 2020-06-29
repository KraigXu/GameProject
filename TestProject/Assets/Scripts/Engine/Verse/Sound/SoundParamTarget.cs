using System;

namespace Verse.Sound
{
	
	[EditorShowClassName]
	public abstract class SoundParamTarget
	{
		
		// (get) Token: 0x0600246D RID: 9325
		public abstract string Label { get; }

		
		// (get) Token: 0x0600246E RID: 9326 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual Type NeededFilterType
		{
			get
			{
				return null;
			}
		}

		
		public abstract void SetOn(Sample sample, float value);
	}
}
