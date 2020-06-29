using System;
using Verse;

namespace RimWorld
{
	
	public class Thought_Dumb : Thought
	{
		
		// (get) Token: 0x060047B1 RID: 18353 RVA: 0x0018522B File Offset: 0x0018342B
		public override int CurStageIndex
		{
			get
			{
				return this.forcedStage;
			}
		}

		
		public void SetForcedStage(int stageIndex)
		{
			this.forcedStage = stageIndex;
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.forcedStage, "stageIndex", 0, false);
		}

		
		private int forcedStage;
	}
}
