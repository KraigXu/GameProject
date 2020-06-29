using System;

namespace Verse.Sound
{
	
	public class SoundParamSource_Random : SoundParamSource
	{
		
		// (get) Token: 0x06002467 RID: 9319 RVA: 0x000D9022 File Offset: 0x000D7222
		public override string Label
		{
			get
			{
				return "Random";
			}
		}

		
		public override float ValueFor(Sample samp)
		{
			return Rand.Value;
		}
	}
}
