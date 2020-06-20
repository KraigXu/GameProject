using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000943 RID: 2371
	public class QuestPart_PassActivable : QuestPartActivable
	{
		// Token: 0x0600383F RID: 14399 RVA: 0x0012D759 File Offset: 0x0012B959
		protected override void ProcessQuestSignal(Signal signal)
		{
			base.ProcessQuestSignal(signal);
			if (signal.tag == this.inSignal)
			{
				this.Complete(signal.args);
			}
		}

		// Token: 0x06003840 RID: 14400 RVA: 0x0012D781 File Offset: 0x0012B981
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
		}

		// Token: 0x06003841 RID: 14401 RVA: 0x0012D79B File Offset: 0x0012B99B
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
		}

		// Token: 0x04002145 RID: 8517
		public string inSignal;
	}
}
