using System;

namespace Verse.AI
{
	// Token: 0x02000538 RID: 1336
	public static class JobFailReason
	{
		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x0600263D RID: 9789 RVA: 0x000E1B26 File Offset: 0x000DFD26
		public static string Reason
		{
			get
			{
				return JobFailReason.lastReason;
			}
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x0600263E RID: 9790 RVA: 0x000E1B2D File Offset: 0x000DFD2D
		public static bool HaveReason
		{
			get
			{
				return JobFailReason.lastReason != null;
			}
		}

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x0600263F RID: 9791 RVA: 0x000E1B37 File Offset: 0x000DFD37
		public static string CustomJobString
		{
			get
			{
				return JobFailReason.lastCustomJobString;
			}
		}

		// Token: 0x06002640 RID: 9792 RVA: 0x000E1B3E File Offset: 0x000DFD3E
		public static void Is(string reason, string customJobString = null)
		{
			JobFailReason.lastReason = reason;
			JobFailReason.lastCustomJobString = customJobString;
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x000E1B4C File Offset: 0x000DFD4C
		public static void Clear()
		{
			JobFailReason.lastReason = null;
			JobFailReason.lastCustomJobString = null;
		}

		// Token: 0x04001716 RID: 5910
		private static string lastReason;

		// Token: 0x04001717 RID: 5911
		private static string lastCustomJobString;
	}
}
