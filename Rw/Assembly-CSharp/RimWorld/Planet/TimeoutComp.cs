using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200127F RID: 4735
	public class TimeoutComp : WorldObjectComp
	{
		// Token: 0x170012A8 RID: 4776
		// (get) Token: 0x06006F09 RID: 28425 RVA: 0x0026AD60 File Offset: 0x00268F60
		public bool Active
		{
			get
			{
				return this.timeoutEndTick != -1;
			}
		}

		// Token: 0x170012A9 RID: 4777
		// (get) Token: 0x06006F0A RID: 28426 RVA: 0x0026AD6E File Offset: 0x00268F6E
		public bool Passed
		{
			get
			{
				return this.Active && Find.TickManager.TicksGame >= this.timeoutEndTick;
			}
		}

		// Token: 0x170012AA RID: 4778
		// (get) Token: 0x06006F0B RID: 28427 RVA: 0x0026AD8F File Offset: 0x00268F8F
		private bool ShouldRemoveWorldObjectNow
		{
			get
			{
				return this.Passed && !base.ParentHasMap;
			}
		}

		// Token: 0x170012AB RID: 4779
		// (get) Token: 0x06006F0C RID: 28428 RVA: 0x0026ADA4 File Offset: 0x00268FA4
		public int TicksLeft
		{
			get
			{
				if (!this.Active)
				{
					return 0;
				}
				return this.timeoutEndTick - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06006F0D RID: 28429 RVA: 0x0026ADC1 File Offset: 0x00268FC1
		public void StartTimeout(int ticks)
		{
			this.timeoutEndTick = Find.TickManager.TicksGame + ticks;
		}

		// Token: 0x06006F0E RID: 28430 RVA: 0x0026ADD5 File Offset: 0x00268FD5
		public void StopTimeout()
		{
			this.timeoutEndTick = -1;
		}

		// Token: 0x06006F0F RID: 28431 RVA: 0x0026ADDE File Offset: 0x00268FDE
		public override void CompTick()
		{
			base.CompTick();
			if (this.ShouldRemoveWorldObjectNow)
			{
				this.parent.Destroy();
			}
		}

		// Token: 0x06006F10 RID: 28432 RVA: 0x0026ADF9 File Offset: 0x00268FF9
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.timeoutEndTick, "timeoutEndTick", 0, false);
		}

		// Token: 0x06006F11 RID: 28433 RVA: 0x0026AE13 File Offset: 0x00269013
		public override string CompInspectStringExtra()
		{
			if (this.Active && !base.ParentHasMap)
			{
				return "WorldObjectTimeout".Translate(this.TicksLeft.ToStringTicksToPeriod(true, false, true, true));
			}
			return null;
		}

		// Token: 0x0400444E RID: 17486
		private int timeoutEndTick = -1;
	}
}
