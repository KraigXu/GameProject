using System;
using Verse;

namespace RimWorld
{
	
	public class Thought_Situational : Thought
	{
		
		// (get) Token: 0x060047DB RID: 18395 RVA: 0x00185996 File Offset: 0x00183B96
		public bool Active
		{
			get
			{
				return this.curStageIndex >= 0;
			}
		}

		
		// (get) Token: 0x060047DC RID: 18396 RVA: 0x001859A4 File Offset: 0x00183BA4
		public override int CurStageIndex
		{
			get
			{
				return this.curStageIndex;
			}
		}

		
		// (get) Token: 0x060047DD RID: 18397 RVA: 0x001859AC File Offset: 0x00183BAC
		public override string LabelCap
		{
			get
			{
				if (!this.reason.NullOrEmpty())
				{
					string text = base.CurStage.label.Formatted(this.reason).CapitalizeFirst();
					if (this.def.Worker != null)
					{
						text = this.def.Worker.PostProcessLabel(this.pawn, text);
					}
					return text;
				}
				return base.LabelCap;
			}
		}

		
		public void RecalculateState()
		{
			ThoughtState thoughtState = this.CurrentStateInternal();
			if (thoughtState.ActiveFor(this.def))
			{
				this.curStageIndex = thoughtState.StageIndexFor(this.def);
				this.reason = thoughtState.Reason;
				return;
			}
			this.curStageIndex = -1;
		}

		
		protected virtual ThoughtState CurrentStateInternal()
		{
			return this.def.Worker.CurrentState(this.pawn);
		}

		
		private int curStageIndex = -1;

		
		protected string reason;
	}
}
