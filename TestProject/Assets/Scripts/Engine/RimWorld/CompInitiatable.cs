using System;
using Verse;

namespace RimWorld
{
	
	public class CompInitiatable : ThingComp
	{
		
		
		public bool Initiated
		{
			get
			{
				return this.Delay <= 0 || (this.spawnedTick >= 0 && Find.TickManager.TicksGame >= this.spawnedTick + this.Delay);
			}
		}

		
		
		private int Delay
		{
			get
			{
				if (this.initiationDelayTicksOverride <= 0)
				{
					return this.Props.initiationDelayTicks;
				}
				return this.initiationDelayTicksOverride;
			}
		}

		
		
		private CompProperties_Initiatable Props
		{
			get
			{
				return (CompProperties_Initiatable)this.props;
			}
		}

		
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.spawnedTick = Find.TickManager.TicksGame;
			}
		}

		
		public override string CompInspectStringExtra()
		{
			if (!this.Initiated)
			{
				return "InitiatesIn".Translate() + ": " + (this.spawnedTick + this.Delay - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true);
			}
			return base.CompInspectStringExtra();
		}

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.spawnedTick, "spawnedTick", -1, false);
			Scribe_Values.Look<int>(ref this.initiationDelayTicksOverride, "initiationDelayTicksOverride", 0, false);
		}

		
		private int spawnedTick = -1;

		
		public int initiationDelayTicksOverride;
	}
}
