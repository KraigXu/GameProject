using System;

namespace Verse.Sound
{
	// Token: 0x020004EB RID: 1259
	public class SoundParamTarget_Pitch : SoundParamTarget
	{
		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x06002474 RID: 9332 RVA: 0x000D908F File Offset: 0x000D728F
		public override string Label
		{
			get
			{
				return "Pitch";
			}
		}

		// Token: 0x06002475 RID: 9333 RVA: 0x000D9098 File Offset: 0x000D7298
		public override void SetOn(Sample sample, float value)
		{
			float num;
			if (this.pitchType == PitchParamType.Multiply)
			{
				num = value;
			}
			else
			{
				num = (float)Math.Pow(1.05946, (double)value);
			}
			sample.source.pitch = AudioSourceUtility.GetSanitizedPitch(sample.SanitizedPitch * num, "SoundParamTarget_Pitch");
		}

		// Token: 0x04001609 RID: 5641
		[Description("The scale used for this pitch input.\n\nMultiply means a multiplier for the natural frequency of the sound. 1.0 gives normal sound, 0.5 gives twice as long and one octave down, and 2.0 gives half as long and an octave up.\n\nSemitones sets a number of semitones to offset the sound.")]
		private PitchParamType pitchType;
	}
}
