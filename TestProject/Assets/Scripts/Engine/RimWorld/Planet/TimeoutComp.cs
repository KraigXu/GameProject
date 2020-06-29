using System;
using Verse;

namespace RimWorld.Planet
{
	
	public class TimeoutComp : WorldObjectComp
	{
		
		
		public bool Active
		{
			get
			{
				return this.timeoutEndTick != -1;
			}
		}

		
		
		public bool Passed
		{
			get
			{
				return this.Active && Find.TickManager.TicksGame >= this.timeoutEndTick;
			}
		}

		
		
		private bool ShouldRemoveWorldObjectNow
		{
			get
			{
				return this.Passed && !base.ParentHasMap;
			}
		}

		
		
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
