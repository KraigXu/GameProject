using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001274 RID: 4724
	public class DebugArena : WorldObjectComp
	{
		// Token: 0x06006EC1 RID: 28353 RVA: 0x0026A07A File Offset: 0x0026827A
		public DebugArena()
		{
			this.tickCreated = Find.TickManager.TicksGame;
		}

		// Token: 0x06006EC2 RID: 28354 RVA: 0x0026A094 File Offset: 0x00268294
		public override void CompTick()
		{
			if (this.lhs == null || this.rhs == null)
			{
				Log.ErrorOnce("DebugArena improperly set up", 73785616, false);
				return;
			}
			if ((this.tickFightStarted == 0 && Find.TickManager.TicksGame - this.tickCreated > 10000) || (this.tickFightStarted != 0 && Find.TickManager.TicksGame - this.tickFightStarted > 60000))
			{
				Log.Message("Fight timed out", false);
				ArenaUtility.ArenaResult obj = default(ArenaUtility.ArenaResult);
				obj.tickDuration = Find.TickManager.TicksGame - this.tickCreated;
				obj.winner = ArenaUtility.ArenaResult.Winner.Other;
				this.callback(obj);
				this.parent.Destroy();
				return;
			}
			if (this.tickFightStarted == 0)
			{
				foreach (Pawn pawn3 in this.lhs.Concat(this.rhs))
				{
					if (pawn3.records.GetValue(RecordDefOf.ShotsFired) > 0f || (pawn3.CurJob != null && pawn3.CurJob.def == JobDefOf.AttackMelee && pawn3.Position.DistanceTo(pawn3.CurJob.targetA.Thing.Position) <= 2f))
					{
						this.tickFightStarted = Find.TickManager.TicksGame;
						break;
					}
				}
			}
			if (this.tickFightStarted != 0)
			{
				bool flag = !this.lhs.Any((Pawn pawn) => !pawn.Dead && !pawn.Downed && pawn.Spawned);
				bool flag2 = !this.rhs.Any((Pawn pawn) => !pawn.Dead && !pawn.Downed && pawn.Spawned);
				if (flag || flag2)
				{
					ArenaUtility.ArenaResult obj2 = default(ArenaUtility.ArenaResult);
					obj2.tickDuration = Find.TickManager.TicksGame - this.tickFightStarted;
					if (flag && !flag2)
					{
						obj2.winner = ArenaUtility.ArenaResult.Winner.Rhs;
					}
					else if (!flag && flag2)
					{
						obj2.winner = ArenaUtility.ArenaResult.Winner.Lhs;
					}
					else
					{
						obj2.winner = ArenaUtility.ArenaResult.Winner.Other;
					}
					this.callback(obj2);
					foreach (Pawn pawn2 in this.lhs.Concat(this.rhs))
					{
						if (!pawn2.Destroyed)
						{
							pawn2.Destroy(DestroyMode.Vanish);
						}
					}
					this.parent.Destroy();
				}
			}
		}

		// Token: 0x0400443D RID: 17469
		public List<Pawn> lhs;

		// Token: 0x0400443E RID: 17470
		public List<Pawn> rhs;

		// Token: 0x0400443F RID: 17471
		public Action<ArenaUtility.ArenaResult> callback;

		// Token: 0x04004440 RID: 17472
		private int tickCreated;

		// Token: 0x04004441 RID: 17473
		private int tickFightStarted;
	}
}
