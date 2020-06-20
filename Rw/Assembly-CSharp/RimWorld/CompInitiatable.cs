using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D9A RID: 3482
	public class CompInitiatable : ThingComp
	{
		// Token: 0x17000F04 RID: 3844
		// (get) Token: 0x060054A4 RID: 21668 RVA: 0x001C3868 File Offset: 0x001C1A68
		public bool Initiated
		{
			get
			{
				return this.Delay <= 0 || (this.spawnedTick >= 0 && Find.TickManager.TicksGame >= this.spawnedTick + this.Delay);
			}
		}

		// Token: 0x17000F05 RID: 3845
		// (get) Token: 0x060054A5 RID: 21669 RVA: 0x001C389C File Offset: 0x001C1A9C
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

		// Token: 0x17000F06 RID: 3846
		// (get) Token: 0x060054A6 RID: 21670 RVA: 0x001C38B9 File Offset: 0x001C1AB9
		private CompProperties_Initiatable Props
		{
			get
			{
				return (CompProperties_Initiatable)this.props;
			}
		}

		// Token: 0x060054A7 RID: 21671 RVA: 0x001C38C6 File Offset: 0x001C1AC6
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.spawnedTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x060054A8 RID: 21672 RVA: 0x001C38E4 File Offset: 0x001C1AE4
		public override string CompInspectStringExtra()
		{
			if (!this.Initiated)
			{
				return "InitiatesIn".Translate() + ": " + (this.spawnedTick + this.Delay - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true);
			}
			return base.CompInspectStringExtra();
		}

		// Token: 0x060054A9 RID: 21673 RVA: 0x001C393F File Offset: 0x001C1B3F
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.spawnedTick, "spawnedTick", -1, false);
			Scribe_Values.Look<int>(ref this.initiationDelayTicksOverride, "initiationDelayTicksOverride", 0, false);
		}

		// Token: 0x04002E7F RID: 11903
		private int spawnedTick = -1;

		// Token: 0x04002E80 RID: 11904
		public int initiationDelayTicksOverride;
	}
}
