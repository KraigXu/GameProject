using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CDE RID: 3294
	public static class PsychicDroneLevelUtility
	{
		// Token: 0x06004FEB RID: 20459 RVA: 0x001AF51C File Offset: 0x001AD71C
		public static string GetLabel(this PsychicDroneLevel level)
		{
			switch (level)
			{
			case PsychicDroneLevel.None:
				return "PsychicDroneLevel_None".Translate();
			case PsychicDroneLevel.GoodMedium:
				return "PsychicDroneLevel_GoodMedium".Translate();
			case PsychicDroneLevel.BadLow:
				return "PsychicDroneLevel_BadLow".Translate();
			case PsychicDroneLevel.BadMedium:
				return "PsychicDroneLevel_BadMedium".Translate();
			case PsychicDroneLevel.BadHigh:
				return "PsychicDroneLevel_BadHigh".Translate();
			case PsychicDroneLevel.BadExtreme:
				return "PsychicDroneLevel_BadExtreme".Translate();
			default:
				return "error";
			}
		}

		// Token: 0x06004FEC RID: 20460 RVA: 0x001AF5AE File Offset: 0x001AD7AE
		public static string GetLabelCap(this PsychicDroneLevel level)
		{
			return level.GetLabel().CapitalizeFirst();
		}
	}
}
