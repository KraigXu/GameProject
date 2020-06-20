using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CB2 RID: 3250
	public class LiquidFuel : Filth
	{
		// Token: 0x06004ED5 RID: 20181 RVA: 0x001A8BA8 File Offset: 0x001A6DA8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.spawnTick, "spawnTick", 0, false);
		}

		// Token: 0x06004ED6 RID: 20182 RVA: 0x001A8BC2 File Offset: 0x001A6DC2
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.spawnTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06004ED7 RID: 20183 RVA: 0x001A8BDC File Offset: 0x001A6DDC
		public void Refill()
		{
			this.spawnTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06004ED8 RID: 20184 RVA: 0x001A8BEE File Offset: 0x001A6DEE
		public override void Tick()
		{
			if (this.spawnTick + 1500 < Find.TickManager.TicksGame)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06004ED9 RID: 20185 RVA: 0x001A8C0F File Offset: 0x001A6E0F
		public override void ThickenFilth()
		{
			base.ThickenFilth();
			this.Refill();
		}

		// Token: 0x04002C3F RID: 11327
		private int spawnTick;

		// Token: 0x04002C40 RID: 11328
		private const int DryOutTime = 1500;
	}
}
