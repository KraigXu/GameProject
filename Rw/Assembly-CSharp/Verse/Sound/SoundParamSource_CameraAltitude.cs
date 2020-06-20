using System;

namespace Verse.Sound
{
	// Token: 0x020004DF RID: 1247
	public class SoundParamSource_CameraAltitude : SoundParamSource
	{
		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x06002457 RID: 9303 RVA: 0x000D8E6C File Offset: 0x000D706C
		public override string Label
		{
			get
			{
				return "Camera altitude";
			}
		}

		// Token: 0x06002458 RID: 9304 RVA: 0x000D8E73 File Offset: 0x000D7073
		public override float ValueFor(Sample samp)
		{
			return Find.Camera.transform.position.y;
		}
	}
}
