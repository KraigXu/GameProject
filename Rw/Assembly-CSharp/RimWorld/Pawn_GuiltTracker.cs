using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BC1 RID: 3009
	public class Pawn_GuiltTracker : IExposable
	{
		// Token: 0x17000CA9 RID: 3241
		// (get) Token: 0x06004725 RID: 18213 RVA: 0x001811E9 File Offset: 0x0017F3E9
		public bool IsGuilty
		{
			get
			{
				return this.TicksUntilInnocent > 0;
			}
		}

		// Token: 0x17000CAA RID: 3242
		// (get) Token: 0x06004726 RID: 18214 RVA: 0x001811F4 File Offset: 0x0017F3F4
		public int TicksUntilInnocent
		{
			get
			{
				return Mathf.Max(0, this.lastGuiltyTick + 60000 - Find.TickManager.TicksGame);
			}
		}

		// Token: 0x06004728 RID: 18216 RVA: 0x00181226 File Offset: 0x0017F426
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastGuiltyTick, "lastGuiltyTick", -99999, false);
		}

		// Token: 0x06004729 RID: 18217 RVA: 0x0018123E File Offset: 0x0017F43E
		public void Notify_Guilty()
		{
			this.lastGuiltyTick = Find.TickManager.TicksGame;
		}

		// Token: 0x040028F5 RID: 10485
		public int lastGuiltyTick = -99999;

		// Token: 0x040028F6 RID: 10486
		private const int GuiltyDuration = 60000;
	}
}
