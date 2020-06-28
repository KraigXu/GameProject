using System;

namespace Verse.Sound
{
	// Token: 0x020004E0 RID: 1248
	public class SoundParamSource_OutdoorTemperature : SoundParamSource
	{
		// Token: 0x1700072B RID: 1835
		// (get) Token: 0x0600245A RID: 9306 RVA: 0x000D8E89 File Offset: 0x000D7089
		public override string Label
		{
			get
			{
				return "Outdoor temperature";
			}
		}

		// Token: 0x0600245B RID: 9307 RVA: 0x000D8E90 File Offset: 0x000D7090
		public override float ValueFor(Sample samp)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return 0f;
			}
			if (Find.CurrentMap == null)
			{
				return 0f;
			}
			return Find.CurrentMap.mapTemperature.OutdoorTemp;
		}
	}
}
