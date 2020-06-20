using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B8F RID: 2959
	public static class HungerLevelUtility
	{
		// Token: 0x06004562 RID: 17762 RVA: 0x00177000 File Offset: 0x00175200
		public static string GetLabel(this HungerCategory hunger)
		{
			switch (hunger)
			{
			case HungerCategory.Fed:
				return "HungerLevel_Fed".Translate();
			case HungerCategory.Hungry:
				return "HungerLevel_Hungry".Translate();
			case HungerCategory.UrgentlyHungry:
				return "HungerLevel_UrgentlyHungry".Translate();
			case HungerCategory.Starving:
				return "HungerLevel_Starving".Translate();
			default:
				throw new InvalidOperationException();
			}
		}
	}
}
