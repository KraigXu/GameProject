using System;

namespace Verse.Sound
{
	
	public class SoundParamSource_OutdoorTemperature : SoundParamSource
	{
		
		// (get) Token: 0x0600245A RID: 9306 RVA: 0x000D8E89 File Offset: 0x000D7089
		public override string Label
		{
			get
			{
				return "Outdoor temperature";
			}
		}

		
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
