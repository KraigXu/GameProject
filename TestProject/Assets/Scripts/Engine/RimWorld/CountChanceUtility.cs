using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DB5 RID: 3509
	public static class CountChanceUtility
	{
		// Token: 0x0600552D RID: 21805 RVA: 0x001C53B4 File Offset: 0x001C35B4
		public static int RandomCount(List<CountChance> chances)
		{
			float value = Rand.Value;
			float num = 0f;
			for (int i = 0; i < chances.Count; i++)
			{
				num += chances[i].chance;
				if (value < num)
				{
					if (num > 1f)
					{
						Log.Error("CountChances error: Total chance is " + num + " but it should not be above 1.", false);
					}
					return chances[i].count;
				}
			}
			return 0;
		}
	}
}
