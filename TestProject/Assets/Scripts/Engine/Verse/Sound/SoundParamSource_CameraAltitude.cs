using System;

namespace Verse.Sound
{
	
	public class SoundParamSource_CameraAltitude : SoundParamSource
	{
		
		// (get) Token: 0x06002457 RID: 9303 RVA: 0x000D8E6C File Offset: 0x000D706C
		public override string Label
		{
			get
			{
				return "Camera altitude";
			}
		}

		
		public override float ValueFor(Sample samp)
		{
			return Find.Camera.transform.position.y;
		}
	}
}
