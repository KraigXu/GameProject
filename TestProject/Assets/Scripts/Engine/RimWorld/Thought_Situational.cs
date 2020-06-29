using System;
using Verse;

namespace RimWorld
{
	
	public class Thought_Situational : Thought
	{
		
		
		public bool Active
		{
			get
			{
				return this.curStageIndex >= 0;
			}
		}

		
		
		public override int CurStageIndex
		{
			get
			{
				return this.curStageIndex;
			}
		}

		
		
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
