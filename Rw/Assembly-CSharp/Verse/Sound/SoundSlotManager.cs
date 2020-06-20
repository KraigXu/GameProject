using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000502 RID: 1282
	public static class SoundSlotManager
	{
		// Token: 0x060024DF RID: 9439 RVA: 0x000DAAE8 File Offset: 0x000D8CE8
		public static bool CanPlayNow(string slotName)
		{
			if (slotName == "")
			{
				return true;
			}
			float num = 0f;
			return !SoundSlotManager.allowedPlayTimes.TryGetValue(slotName, out num) || Time.realtimeSinceStartup >= SoundSlotManager.allowedPlayTimes[slotName];
		}

		// Token: 0x060024E0 RID: 9440 RVA: 0x000DAB30 File Offset: 0x000D8D30
		public static void Notify_Played(string slot, float duration)
		{
			if (slot == "")
			{
				return;
			}
			float a;
			if (SoundSlotManager.allowedPlayTimes.TryGetValue(slot, out a))
			{
				SoundSlotManager.allowedPlayTimes[slot] = Mathf.Max(a, Time.realtimeSinceStartup + duration);
				return;
			}
			SoundSlotManager.allowedPlayTimes[slot] = Time.realtimeSinceStartup + duration;
		}

		// Token: 0x0400165E RID: 5726
		private static Dictionary<string, float> allowedPlayTimes = new Dictionary<string, float>();
	}
}
