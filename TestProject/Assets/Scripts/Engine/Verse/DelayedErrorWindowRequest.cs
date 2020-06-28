using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200032C RID: 812
	public static class DelayedErrorWindowRequest
	{
		// Token: 0x060017BF RID: 6079 RVA: 0x00087F14 File Offset: 0x00086114
		public static void DelayedErrorWindowRequestOnGUI()
		{
			try
			{
				for (int i = 0; i < DelayedErrorWindowRequest.requests.Count; i++)
				{
					Find.WindowStack.Add(new Dialog_MessageBox(DelayedErrorWindowRequest.requests[i].text, "OK".Translate(), null, null, null, DelayedErrorWindowRequest.requests[i].title, false, null, null));
				}
			}
			finally
			{
				DelayedErrorWindowRequest.requests.Clear();
			}
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x00087F9C File Offset: 0x0008619C
		public static void Add(string text, string title = null)
		{
			DelayedErrorWindowRequest.Request item = default(DelayedErrorWindowRequest.Request);
			item.text = text;
			item.title = title;
			DelayedErrorWindowRequest.requests.Add(item);
		}

		// Token: 0x04000EEA RID: 3818
		private static List<DelayedErrorWindowRequest.Request> requests = new List<DelayedErrorWindowRequest.Request>();

		// Token: 0x020014C0 RID: 5312
		private struct Request
		{
			// Token: 0x04004E9E RID: 20126
			public string text;

			// Token: 0x04004E9F RID: 20127
			public string title;
		}
	}
}
