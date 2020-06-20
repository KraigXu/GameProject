using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200089E RID: 2206
	public class SoundParamSource_TimeOfDay : SoundParamSource
	{
		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x06003584 RID: 13700 RVA: 0x00123BDC File Offset: 0x00121DDC
		public override string Label
		{
			get
			{
				return "Time of day (hour)";
			}
		}

		// Token: 0x06003585 RID: 13701 RVA: 0x00123BE3 File Offset: 0x00121DE3
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
