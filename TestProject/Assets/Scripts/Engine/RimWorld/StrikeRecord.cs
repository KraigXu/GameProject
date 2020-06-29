using System;
using Verse;

namespace RimWorld
{
	
	internal struct StrikeRecord : IExposable
	{
		
		// (get) Token: 0x06003F05 RID: 16133 RVA: 0x0014F658 File Offset: 0x0014D858
		public bool Expired
		{
			get
			{
				return Find.TickManager.TicksGame > this.ticksGame + 900000;
			}
		}

		
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.cell, "cell", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.ticksGame, "ticksGame", 0, false);
			Scribe_Defs.Look<ThingDef>(ref this.def, "def");
		}

		
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

		
		public IntVec3 cell;

		
		public int ticksGame;

		
		public ThingDef def;

		
		private const int StrikeRecordExpiryDays = 15;
	}
}
