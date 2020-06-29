﻿using System;
using UnityEngine;

namespace Verse
{
	
	public abstract class LetterWithTimeout : Letter
	{
		
		// (get) Token: 0x06001B38 RID: 6968 RVA: 0x000A6E75 File Offset: 0x000A5075
		public bool TimeoutActive
		{
			get
			{
				return this.disappearAtTick >= 0;
			}
		}

		
		// (get) Token: 0x06001B39 RID: 6969 RVA: 0x000A6E83 File Offset: 0x000A5083
		public bool TimeoutPassed
		{
			get
			{
				return this.TimeoutActive && Find.TickManager.TicksGame >= this.disappearAtTick;
			}
		}

		
		// (get) Token: 0x06001B3A RID: 6970 RVA: 0x000A6EA4 File Offset: 0x000A50A4
		public override bool CanShowInLetterStack
		{
			get
			{
				return base.CanShowInLetterStack && !this.TimeoutPassed;
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.disappearAtTick, "disappearAtTick", -1, false);
		}

		
		public void StartTimeout(int duration)
		{
			this.disappearAtTick = Find.TickManager.TicksGame + duration;
		}

		
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

		
		public int disappearAtTick = -1;
	}
}
