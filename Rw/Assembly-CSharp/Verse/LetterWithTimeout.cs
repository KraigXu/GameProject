using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200039D RID: 925
	public abstract class LetterWithTimeout : Letter
	{
		// Token: 0x1700053F RID: 1343
		// (get) Token: 0x06001B38 RID: 6968 RVA: 0x000A6E75 File Offset: 0x000A5075
		public bool TimeoutActive
		{
			get
			{
				return this.disappearAtTick >= 0;
			}
		}

		// Token: 0x17000540 RID: 1344
		// (get) Token: 0x06001B39 RID: 6969 RVA: 0x000A6E83 File Offset: 0x000A5083
		public bool TimeoutPassed
		{
			get
			{
				return this.TimeoutActive && Find.TickManager.TicksGame >= this.disappearAtTick;
			}
		}

		// Token: 0x17000541 RID: 1345
		// (get) Token: 0x06001B3A RID: 6970 RVA: 0x000A6EA4 File Offset: 0x000A50A4
		public override bool CanShowInLetterStack
		{
			get
			{
				return base.CanShowInLetterStack && !this.TimeoutPassed;
			}
		}

		// Token: 0x06001B3B RID: 6971 RVA: 0x000A6EBB File Offset: 0x000A50BB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.disappearAtTick, "disappearAtTick", -1, false);
		}

		// Token: 0x06001B3C RID: 6972 RVA: 0x000A6ED5 File Offset: 0x000A50D5
		public void StartTimeout(int duration)
		{
			this.disappearAtTick = Find.TickManager.TicksGame + duration;
		}

		// Token: 0x06001B3D RID: 6973 RVA: 0x000A6EEC File Offset: 0x000A50EC
		protected override string PostProcessedLabel()
		{
			string text = base.PostProcessedLabel();
			if (this.TimeoutActive)
			{
				int num = Mathf.RoundToInt((float)(this.disappearAtTick - Find.TickManager.TicksGame) / 2500f);
				text += " (" + num + "LetterHour".Translate() + ")";
			}
			return text;
		}

		// Token: 0x0400102A RID: 4138
		public int disappearAtTick = -1;
	}
}
