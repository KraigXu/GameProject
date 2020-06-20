using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BD4 RID: 3028
	public class Thought_Situational : Thought
	{
		// Token: 0x17000CCB RID: 3275
		// (get) Token: 0x060047DB RID: 18395 RVA: 0x00185996 File Offset: 0x00183B96
		public bool Active
		{
			get
			{
				return this.curStageIndex >= 0;
			}
		}

		// Token: 0x17000CCC RID: 3276
		// (get) Token: 0x060047DC RID: 18396 RVA: 0x001859A4 File Offset: 0x00183BA4
		public override int CurStageIndex
		{
			get
			{
				return this.curStageIndex;
			}
		}

		// Token: 0x17000CCD RID: 3277
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

		// Token: 0x060047DE RID: 18398 RVA: 0x00185A1C File Offset: 0x00183C1C
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

		// Token: 0x060047DF RID: 18399 RVA: 0x00185A67 File Offset: 0x00183C67
		protected virtual ThoughtState CurrentStateInternal()
		{
			return this.def.Worker.CurrentState(this.pawn);
		}

		// Token: 0x0400293B RID: 10555
		private int curStageIndex = -1;

		// Token: 0x0400293C RID: 10556
		protected string reason;
	}
}
