using System;

namespace Verse.AI
{
	
	public static class JobFailReason
	{
		
		// (get) Token: 0x0600263D RID: 9789 RVA: 0x000E1B26 File Offset: 0x000DFD26
		public static string Reason
		{
			get
			{
				return JobFailReason.lastReason;
			}
		}

		
		// (get) Token: 0x0600263E RID: 9790 RVA: 0x000E1B2D File Offset: 0x000DFD2D
		public static bool HaveReason
		{
			get
			{
				return JobFailReason.lastReason != null;
			}
		}

		
		// (get) Token: 0x0600263F RID: 9791 RVA: 0x000E1B37 File Offset: 0x000DFD37
		public static string CustomJobString
		{
			get
			{
				return JobFailReason.lastCustomJobString;
			}
		}

		
		public static void Is(string reason, string customJobString = null)
		{
			JobFailReason.lastReason = reason;
			JobFailReason.lastCustomJobString = customJobString;
		}

		
		public static void Clear()
		{
			JobFailReason.lastReason = null;
			JobFailReason.lastCustomJobString = null;
		}

		
		private static string lastReason;

		
		private static string lastCustomJobString;
	}
}
