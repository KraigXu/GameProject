using System;

namespace Verse.AI
{
	
	public static class JobFailReason
	{
		
		
		public static string Reason
		{
			get
			{
				return JobFailReason.lastReason;
			}
		}

		
		
		public static bool HaveReason
		{
			get
			{
				return JobFailReason.lastReason != null;
			}
		}

		
		
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
