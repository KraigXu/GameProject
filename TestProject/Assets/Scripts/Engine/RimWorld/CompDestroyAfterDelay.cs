using System;
using Verse;

namespace RimWorld
{
	
	public class CompDestroyAfterDelay : ThingComp
	{
		
		// (get) Token: 0x060050F0 RID: 20720 RVA: 0x001B2979 File Offset: 0x001B0B79
		public CompProperties_DestroyAfterDelay Props
		{
			get
			{
				return (CompProperties_DestroyAfterDelay)this.props;
			}
		}

		
		public override void CompTick()
		{
			base.CompTick();
			if (Find.TickManager.TicksGame > this.spawnTick + this.Props.delayTicks && !this.parent.Destroyed)
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.spawnTick = Find.TickManager.TicksGame;
			}
		}

		
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.spawnTick, "spawnTick", 0, false);
		}

		
		public int spawnTick;
	}
}
