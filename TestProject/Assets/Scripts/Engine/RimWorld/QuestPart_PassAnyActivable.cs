using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000949 RID: 2377
	public class QuestPart_PassAnyActivable : QuestPartActivable
	{
		// Token: 0x06003858 RID: 14424 RVA: 0x0012DD26 File Offset: 0x0012BF26
		protected override void ProcessQuestSignal(Signal signal)
		{
			base.ProcessQuestSignal(signal);
			if (this.inSignals.Contains(signal.tag))
			{
				base.Complete();
			}
		}

		// Token: 0x06003859 RID: 14425 RVA: 0x0012DD48 File Offset: 0x0012BF48
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x0600385A RID: 14426 RVA: 0x0012DD68 File Offset: 0x0012BF68
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignals.Clear();
			for (int i = 0; i < 3; i++)
			{
				this.inSignals.Add("DebugSignal" + Rand.Int);
			}
		}

		// Token: 0x04002150 RID: 8528
		public List<string> inSignals = new List<string>();
	}
}
