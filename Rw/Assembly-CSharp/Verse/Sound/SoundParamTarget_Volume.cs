using System;

namespace Verse.Sound
{
	// Token: 0x020004E9 RID: 1257
	public class SoundParamTarget_Volume : SoundParamTarget
	{
		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x06002471 RID: 9329 RVA: 0x000D9076 File Offset: 0x000D7276
		public override string Label
		{
			get
			{
				return "Volume";
			}
		}

		// Token: 0x06002472 RID: 9330 RVA: 0x000D907D File Offset: 0x000D727D
		public override void SetOn(Sample sample, float value)
		{
			sample.SignalMappedVolume(value, this);
		}
	}
}
