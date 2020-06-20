using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x02000945 RID: 2373
	public static class PassAllQuestPartUtility
	{
		// Token: 0x06003848 RID: 14408 RVA: 0x0012D91C File Offset: 0x0012BB1C
		public static bool AllReceived(List<string> inSignals, List<bool> signalsReceived)
		{
			if (inSignals.Count != signalsReceived.Count)
			{
				return false;
			}
			for (int i = 0; i < signalsReceived.Count; i++)
			{
				if (!signalsReceived[i])
				{
					return false;
				}
			}
			return true;
		}
	}
}
