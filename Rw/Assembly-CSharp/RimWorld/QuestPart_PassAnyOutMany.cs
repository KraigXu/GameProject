using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200094A RID: 2378
	public class QuestPart_PassAnyOutMany : QuestPart
	{
		// Token: 0x0600385C RID: 14428 RVA: 0x0012DDC4 File Offset: 0x0012BFC4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (this.inSignals.Contains(signal.tag))
			{
				for (int i = 0; i < this.outSignals.Count; i++)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignals[i], signal.args));
				}
			}
		}

		// Token: 0x0600385D RID: 14429 RVA: 0x0012DE22 File Offset: 0x0012C022
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.outSignals, "outSignals", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x0600385E RID: 14430 RVA: 0x0012DE58 File Offset: 0x0012C058
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignals.Clear();
			this.outSignals.Clear();
			for (int i = 0; i < 3; i++)
			{
				this.inSignals.Add("DebugSignal" + Rand.Int);
				this.outSignals.Add("DebugSignal" + Rand.Int);
			}
		}

		// Token: 0x04002151 RID: 8529
		public List<string> inSignals = new List<string>();

		// Token: 0x04002152 RID: 8530
		public List<string> outSignals = new List<string>();
	}
}
