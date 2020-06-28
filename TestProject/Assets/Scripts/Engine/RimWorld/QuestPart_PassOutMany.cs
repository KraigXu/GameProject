using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200094B RID: 2379
	public class QuestPart_PassOutMany : QuestPart
	{
		// Token: 0x06003860 RID: 14432 RVA: 0x0012DEEC File Offset: 0x0012C0EC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.outSignals.Count; i++)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignals[i], signal.args));
				}
			}
		}

		// Token: 0x06003861 RID: 14433 RVA: 0x0012DF4A File Offset: 0x0012C14A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<string>(ref this.outSignals, "outSignals", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x06003862 RID: 14434 RVA: 0x0012DF7C File Offset: 0x0012C17C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			for (int i = 0; i < 3; i++)
			{
				this.outSignals.Add("DebugSignal" + Rand.Int);
			}
		}

		// Token: 0x04002153 RID: 8531
		public string inSignal;

		// Token: 0x04002154 RID: 8532
		public List<string> outSignals = new List<string>();
	}
}
