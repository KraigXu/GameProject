using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020003B2 RID: 946
	public static class MessagesRepeatAvoider
	{
		// Token: 0x06001BEA RID: 7146 RVA: 0x000AA3EE File Offset: 0x000A85EE
		public static void Reset()
		{
			MessagesRepeatAvoider.lastShowTimes.Clear();
		}

		// Token: 0x06001BEB RID: 7147 RVA: 0x000AA3FC File Offset: 0x000A85FC
		public static bool MessageShowAllowed(string tag, float minSecondsSinceLastShow)
		{
			float num;
			if (!MessagesRepeatAvoider.lastShowTimes.TryGetValue(tag, out num))
			{
				num = -99999f;
			}
			bool flag = RealTime.LastRealTime > num + minSecondsSinceLastShow;
			if (flag)
			{
				MessagesRepeatAvoider.lastShowTimes[tag] = RealTime.LastRealTime;
			}
			return flag;
		}

		// Token: 0x04001077 RID: 4215
		private static Dictionary<string, float> lastShowTimes = new Dictionary<string, float>();
	}
}
