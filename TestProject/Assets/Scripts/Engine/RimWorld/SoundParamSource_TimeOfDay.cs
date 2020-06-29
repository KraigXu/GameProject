using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	
	public class SoundParamSource_TimeOfDay : SoundParamSource
	{
		
		// (get) Token: 0x06003584 RID: 13700 RVA: 0x00123BDC File Offset: 0x00121DDC
		public override string Label
		{
			get
			{
				return "Time of day (hour)";
			}
		}

		
		public override float ValueFor(Sample samp)
		{
			if (Find.CurrentMap == null)
			{
				return 0f;
			}
			return GenLocalDate.HourFloat(Find.CurrentMap);
		}
	}
}
