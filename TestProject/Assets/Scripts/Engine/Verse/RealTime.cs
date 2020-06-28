using System;
using System.Threading;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000452 RID: 1106
	public static class RealTime
	{
		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x06002110 RID: 8464 RVA: 0x000CAFCF File Offset: 0x000C91CF
		public static float LastRealTime
		{
			get
			{
				return RealTime.lastRealTime;
			}
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x000CAFD8 File Offset: 0x000C91D8
		public static void Update()
		{
			RealTime.frameCount = Time.frameCount;
			RealTime.deltaTime = Time.deltaTime;
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			RealTime.realDeltaTime = realtimeSinceStartup - RealTime.lastRealTime;
			RealTime.lastRealTime = realtimeSinceStartup;
			if (Current.ProgramState == ProgramState.Playing)
			{
				RealTime.moteList.MoteListUpdate();
			}
			else
			{
				RealTime.moteList.Clear();
			}
			if (DebugSettings.lowFPS && Time.deltaTime < 100f)
			{
				Thread.Sleep((int)(100f - Time.deltaTime));
			}
		}

		// Token: 0x04001423 RID: 5155
		public static float deltaTime;

		// Token: 0x04001424 RID: 5156
		public static float realDeltaTime;

		// Token: 0x04001425 RID: 5157
		public static RealtimeMoteList moteList = new RealtimeMoteList();

		// Token: 0x04001426 RID: 5158
		public static int frameCount;

		// Token: 0x04001427 RID: 5159
		private static float lastRealTime = 0f;
	}
}
