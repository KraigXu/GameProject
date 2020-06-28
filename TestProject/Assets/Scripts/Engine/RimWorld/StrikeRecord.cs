using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A73 RID: 2675
	internal struct StrikeRecord : IExposable
	{
		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x06003F05 RID: 16133 RVA: 0x0014F658 File Offset: 0x0014D858
		public bool Expired
		{
			get
			{
				return Find.TickManager.TicksGame > this.ticksGame + 900000;
			}
		}

		// Token: 0x06003F06 RID: 16134 RVA: 0x0014F674 File Offset: 0x0014D874
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.ticksGame, "ticksGame", 0, false);
			Scribe_Defs.Look<ThingDef>(ref this.def, "def");
		}

		// Token: 0x06003F07 RID: 16135 RVA: 0x0014F6C0 File Offset: 0x0014D8C0
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.cell,
				", ",
				this.def,
				", ",
				this.ticksGame,
				")"
			});
		}

		// Token: 0x040024B2 RID: 9394
		public IntVec3 cell;

		// Token: 0x040024B3 RID: 9395
		public int ticksGame;

		// Token: 0x040024B4 RID: 9396
		public ThingDef def;

		// Token: 0x040024B5 RID: 9397
		private const int StrikeRecordExpiryDays = 15;
	}
}
