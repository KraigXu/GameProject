using System;

namespace Verse.Sound
{
	// Token: 0x020004E8 RID: 1256
	[EditorShowClassName]
	public abstract class SoundParamTarget
	{
		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x0600246D RID: 9325
		public abstract string Label { get; }

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x0600246E RID: 9326 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual Type NeededFilterType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600246F RID: 9327
		public abstract void SetOn(Sample sample, float value);
	}
}
