using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D01 RID: 3329
	public class CompDestroyAfterDelay : ThingComp
	{
		// Token: 0x17000E36 RID: 3638
		// (get) Token: 0x060050F0 RID: 20720 RVA: 0x001B2979 File Offset: 0x001B0B79
		public CompProperties_DestroyAfterDelay Props
		{
			get
			{
				return (CompProperties_DestroyAfterDelay)this.props;
			}
		}

		// Token: 0x060050F1 RID: 20721 RVA: 0x001B2986 File Offset: 0x001B0B86
		public override void CompTick()
		{
			base.CompTick();
			if (Find.TickManager.TicksGame > this.spawnTick + this.Props.delayTicks && !this.parent.Destroyed)
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x060050F2 RID: 20722 RVA: 0x001B29C5 File Offset: 0x001B0BC5
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.spawnTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x060050F3 RID: 20723 RVA: 0x001B29E1 File Offset: 0x001B0BE1
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.spawnTick, "spawnTick", 0, false);
		}

		// Token: 0x04002CED RID: 11501
		public int spawnTick;
	}
}
