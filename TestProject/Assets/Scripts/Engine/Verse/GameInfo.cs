using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000112 RID: 274
	public sealed class GameInfo : IExposable
	{
		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060007A8 RID: 1960 RVA: 0x00023B14 File Offset: 0x00021D14
		public float RealPlayTimeInteracting
		{
			get
			{
				return this.realPlayTimeInteracting;
			}
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x00023B1C File Offset: 0x00021D1C
		public void GameInfoOnGUI()
		{
			if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseMove || Event.current.type == EventType.KeyDown)
			{
				this.lastInputRealTime = Time.realtimeSinceStartup;
			}
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x00023B50 File Offset: 0x00021D50
		public void GameInfoUpdate()
		{
			if (Time.realtimeSinceStartup < this.lastInputRealTime + 90f && Find.MainTabsRoot.OpenTab != MainButtonDefOf.Menu && Current.ProgramState == ProgramState.Playing && !Find.WindowStack.IsOpen<Dialog_Options>())
			{
				this.realPlayTimeInteracting += RealTime.realDeltaTime;
			}
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x00023BA7 File Offset: 0x00021DA7
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.realPlayTimeInteracting, "realPlayTimeInteracting", 0f, false);
			Scribe_Values.Look<bool>(ref this.permadeathMode, "permadeathMode", false, false);
			Scribe_Values.Look<string>(ref this.permadeathModeUniqueName, "permadeathModeUniqueName", null, false);
		}

		// Token: 0x040006E9 RID: 1769
		public bool permadeathMode;

		// Token: 0x040006EA RID: 1770
		public string permadeathModeUniqueName;

		// Token: 0x040006EB RID: 1771
		private float realPlayTimeInteracting;

		// Token: 0x040006EC RID: 1772
		private float lastInputRealTime;
	}
}
