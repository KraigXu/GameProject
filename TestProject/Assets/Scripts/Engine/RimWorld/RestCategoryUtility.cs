using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B98 RID: 2968
	public static class RestCategoryUtility
	{
		// Token: 0x06004591 RID: 17809 RVA: 0x00177E80 File Offset: 0x00176080
		public static string GetLabel(this RestCategory fatigue)
		{
			switch (fatigue)
			{
			case RestCategory.Rested:
				return "HungerLevel_Rested".Translate();
			case RestCategory.Tired:
				return "HungerLevel_Tired".Translate();
			case RestCategory.VeryTired:
				return "HungerLevel_VeryTired".Translate();
			case RestCategory.Exhausted:
				return "HungerLevel_Exhausted".Translate();
			default:
				throw new InvalidOperationException();
			}
		}
	}
}
