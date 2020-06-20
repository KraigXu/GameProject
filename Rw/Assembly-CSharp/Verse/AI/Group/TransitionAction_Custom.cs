using System;

namespace Verse.AI.Group
{
	// Token: 0x020005E0 RID: 1504
	public class TransitionAction_Custom : TransitionAction
	{
		// Token: 0x060029D7 RID: 10711 RVA: 0x000F588D File Offset: 0x000F3A8D
		public TransitionAction_Custom(Action action)
		{
			this.action = action;
		}

		// Token: 0x060029D8 RID: 10712 RVA: 0x000F589C File Offset: 0x000F3A9C
		public TransitionAction_Custom(Action<Transition> actionWithArg)
		{
			this.actionWithArg = actionWithArg;
		}

		// Token: 0x060029D9 RID: 10713 RVA: 0x000F58AB File Offset: 0x000F3AAB
		public override void DoAction(Transition trans)
		{
			if (this.actionWithArg != null)
			{
				this.actionWithArg(trans);
			}
			if (this.action != null)
			{
				this.action();
			}
		}

		// Token: 0x0400190E RID: 6414
		public Action action;

		// Token: 0x0400190F RID: 6415
		public Action<Transition> actionWithArg;
	}
}
