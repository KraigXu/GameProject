using System;

namespace Verse.Sound
{
	
	public class SoundParamTarget_Volume : SoundParamTarget
	{
		
		// (get) Token: 0x06002471 RID: 9329 RVA: 0x000D9076 File Offset: 0x000D7276
		public override string Label
		{
			get
			{
				return "Volume";
			}
		}

		
		public override void SetOn(Sample sample, float value)
		{
			sample.SignalMappedVolume(value, this);
		}
	}
}
