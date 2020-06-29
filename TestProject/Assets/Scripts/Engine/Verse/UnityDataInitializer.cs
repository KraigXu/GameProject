using System;

namespace Verse
{
	
	public static class UnityDataInitializer
	{
		
		public static void CopyUnityData()
		{
			UnityDataInitializer.initializing = true;
			try
			{
				UnityData.CopyUnityData();
			}
			finally
			{
				UnityDataInitializer.initializing = false;
			}
		}

		
		public static bool initializing;
	}
}
