using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000217 RID: 535
	internal struct PawnStatusEffecters
	{
		// Token: 0x06000F13 RID: 3859 RVA: 0x00055C8A File Offset: 0x00053E8A
		public PawnStatusEffecters(Pawn pawn)
		{
			this.pawn = pawn;
			this.pairs = new List<PawnStatusEffecters.LiveEffecter>();
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x00055CA0 File Offset: 0x00053EA0
		public void EffectersTick()
		{
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				HediffComp_Effecter hediffComp_Effecter = hediffs[i].TryGetComp<HediffComp_Effecter>();
				if (hediffComp_Effecter != null)
				{
					EffecterDef effecterDef = hediffComp_Effecter.CurrentStateEffecter();
					if (effecterDef != null)
					{
						this.AddOrMaintain(effecterDef);
					}
				}
			}
			if (this.pawn.mindState.mentalStateHandler.CurState != null)
			{
				EffecterDef effecterDef2 = this.pawn.mindState.mentalStateHandler.CurState.CurrentStateEffecter();
				if (effecterDef2 != null)
				{
					this.AddOrMaintain(effecterDef2);
				}
			}
			for (int j = this.pairs.Count - 1; j >= 0; j--)
			{
				if (this.pairs[j].Expired)
				{
					this.pairs[j].Cleanup();
					this.pairs.RemoveAt(j);
				}
				else
				{
					this.pairs[j].Tick(this.pawn);
				}
			}
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x00055DA0 File Offset: 0x00053FA0
		private void AddOrMaintain(EffecterDef def)
		{
			for (int i = 0; i < this.pairs.Count; i++)
			{
				if (this.pairs[i].def == def)
				{
					this.pairs[i].Maintain();
					return;
				}
			}
			PawnStatusEffecters.LiveEffecter liveEffecter = FullPool<PawnStatusEffecters.LiveEffecter>.Get();
			liveEffecter.def = def;
			liveEffecter.Maintain();
			this.pairs.Add(liveEffecter);
		}

		// Token: 0x04000B2F RID: 2863
		public Pawn pawn;

		// Token: 0x04000B30 RID: 2864
		private List<PawnStatusEffecters.LiveEffecter> pairs;

		// Token: 0x02001425 RID: 5157
		private class LiveEffecter : IFullPoolable
		{
			// Token: 0x1700147D RID: 5245
			// (get) Token: 0x06007937 RID: 31031 RVA: 0x002958CA File Offset: 0x00293ACA
			public bool Expired
			{
				get
				{
					return Find.TickManager.TicksGame > this.lastMaintainTick;
				}
			}

			// Token: 0x06007939 RID: 31033 RVA: 0x002958DE File Offset: 0x00293ADE
			public void Cleanup()
			{
				if (this.effecter != null)
				{
					this.effecter.Cleanup();
				}
				FullPool<PawnStatusEffecters.LiveEffecter>.Return(this);
			}

			// Token: 0x0600793A RID: 31034 RVA: 0x002958F9 File Offset: 0x00293AF9
			public void Reset()
			{
				this.def = null;
				this.effecter = null;
				this.lastMaintainTick = -1;
			}

			// Token: 0x0600793B RID: 31035 RVA: 0x00295910 File Offset: 0x00293B10
			public void Maintain()
			{
				this.lastMaintainTick = Find.TickManager.TicksGame;
			}

			// Token: 0x0600793C RID: 31036 RVA: 0x00295922 File Offset: 0x00293B22
			public void Tick(Pawn pawn)
			{
				if (this.effecter == null)
				{
					this.effecter = this.def.Spawn();
				}
				this.effecter.EffectTick(pawn, null);
			}

			// Token: 0x04004C84 RID: 19588
			public EffecterDef def;

			// Token: 0x04004C85 RID: 19589
			public Effecter effecter;

			// Token: 0x04004C86 RID: 19590
			public int lastMaintainTick;
		}
	}
}
