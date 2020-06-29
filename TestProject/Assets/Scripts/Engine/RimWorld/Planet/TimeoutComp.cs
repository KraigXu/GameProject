using System;
using Verse;

namespace RimWorld.Planet
{
	
	public class TimeoutComp : WorldObjectComp
	{
		
		// (get) Token: 0x06006F09 RID: 28425 RVA: 0x0026AD60 File Offset: 0x00268F60
		public bool Active
		{
			get
			{
				return this.timeoutEndTick != -1;
			}
		}

		
		// (get) Token: 0x06006F0A RID: 28426 RVA: 0x0026AD6E File Offset: 0x00268F6E
		public bool Passed
		{
			get
			{
				return this.Active && Find.TickManager.TicksGame >= this.timeoutEndTick;
			}
		}

		
		// (get) Token: 0x06006F0B RID: 28427 RVA: 0x0026AD8F File Offset: 0x00268F8F
		private bool ShouldRemoveWorldObjectNow
		{
			get
			{
				return this.Passed && !base.ParentHasMap;
			}
		}

		
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

		
		public void StartTimeout(int ticks)
		{
			this.timeoutEndTick = Find.TickManager.TicksGame + ticks;
		}

		
		public void StopTimeout()
		{
			this.timeoutEndTick = -1;
		}

		
		public override void CompTick()
		{
			base.CompTick();
			if (this.ShouldRemoveWorldObjectNow)
			{
				this.parent.Destroy();
			}
		}

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.timeoutEndTick, "timeoutEndTick", 0, false);
		}

		
		public override string CompInspectStringExtra()
		{
			if (this.Active && !base.ParentHasMap)
			{
				return "WorldObjectTimeout".Translate(this.TicksLeft.ToStringTicksToPeriod(true, false, true, true));
			}
			return null;
		}

		
		private int timeoutEndTick = -1;
	}
}
