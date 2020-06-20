using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BCE RID: 3022
	public class Thought_Dumb : Thought
	{
		// Token: 0x17000CBD RID: 3261
		// (get) Token: 0x060047B1 RID: 18353 RVA: 0x0018522B File Offset: 0x0018342B
		public override int CurStageIndex
		{
			get
			{
				return this.forcedStage;
			}
		}

		// Token: 0x060047B2 RID: 18354 RVA: 0x00185233 File Offset: 0x00183433
		public void SetForcedStage(int stageIndex)
		{
			this.forcedStage = stageIndex;
		}

		// Token: 0x060047B3 RID: 18355 RVA: 0x0018523C File Offset: 0x0018343C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.forcedStage, "stageIndex", 0, false);
		}

		// Token: 0x0400292F RID: 10543
		private int forcedStage;
	}
}
