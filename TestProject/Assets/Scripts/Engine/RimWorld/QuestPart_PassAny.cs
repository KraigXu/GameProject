using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000948 RID: 2376
	public class QuestPart_PassAny : QuestPart
	{
		// Token: 0x06003854 RID: 14420 RVA: 0x0012DC49 File Offset: 0x0012BE49
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (this.inSignals.Contains(signal.tag))
			{
				Find.SignalManager.SendSignal(new Signal(this.outSignal, signal.args));
			}
		}

		// Token: 0x06003855 RID: 14421 RVA: 0x0012DC80 File Offset: 0x0012BE80
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
		}

		// Token: 0x06003856 RID: 14422 RVA: 0x0012DCB0 File Offset: 0x0012BEB0
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignals.Clear();
			for (int i = 0; i < 3; i++)
			{
				this.inSignals.Add("DebugSignal" + Rand.Int);
			}
			this.outSignal = "DebugSignal" + Rand.Int;
		}

		// Token: 0x0400214E RID: 8526
		public List<string> inSignals = new List<string>();

		// Token: 0x0400214F RID: 8527
		public string outSignal;
	}
}
