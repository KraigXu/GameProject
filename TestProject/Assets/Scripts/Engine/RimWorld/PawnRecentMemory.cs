using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BA1 RID: 2977
	public class PawnRecentMemory : IExposable
	{
		// Token: 0x17000C57 RID: 3159
		// (get) Token: 0x060045B6 RID: 17846 RVA: 0x00178D0B File Offset: 0x00176F0B
		public int TicksSinceLastLight
		{
			get
			{
				return Find.TickManager.TicksGame - this.lastLightTick;
			}
		}

		// Token: 0x17000C58 RID: 3160
		// (get) Token: 0x060045B7 RID: 17847 RVA: 0x00178D1E File Offset: 0x00176F1E
		public int TicksSinceOutdoors
		{
			get
			{
				return Find.TickManager.TicksGame - this.lastOutdoorTick;
			}
		}

		// Token: 0x060045B8 RID: 17848 RVA: 0x00178D31 File Offset: 0x00176F31
		public PawnRecentMemory(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060045B9 RID: 17849 RVA: 0x00178D56 File Offset: 0x00176F56
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastLightTick, "lastLightTick", 999999, false);
			Scribe_Values.Look<int>(ref this.lastOutdoorTick, "lastOutdoorTick", 999999, false);
		}

		// Token: 0x060045BA RID: 17850 RVA: 0x00178D84 File Offset: 0x00176F84
		public void RecentMemoryInterval()
		{
			if (!this.pawn.Spawned)
			{
				return;
			}
			if (this.pawn.Map.glowGrid.PsychGlowAt(this.pawn.Position) != PsychGlow.Dark)
			{
				this.lastLightTick = Find.TickManager.TicksGame;
			}
			if (this.Outdoors())
			{
				this.lastOutdoorTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x060045BB RID: 17851 RVA: 0x00178DEC File Offset: 0x00176FEC
		private bool Outdoors()
		{
			Room room = this.pawn.GetRoom(RegionType.Set_Passable);
			return room != null && room.PsychologicallyOutdoors;
		}

		// Token: 0x060045BC RID: 17852 RVA: 0x00178E11 File Offset: 0x00177011
		public void Notify_Spawned(bool respawningAfterLoad)
		{
			this.lastLightTick = Find.TickManager.TicksGame;
			if (!respawningAfterLoad && this.Outdoors())
			{
				this.lastOutdoorTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x0400281F RID: 10271
		private Pawn pawn;

		// Token: 0x04002820 RID: 10272
		private int lastLightTick = 999999;

		// Token: 0x04002821 RID: 10273
		private int lastOutdoorTick = 999999;
	}
}
