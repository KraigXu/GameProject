using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000942 RID: 2370
	public class QuestPart_Pass : QuestPart
	{
		// Token: 0x0600383B RID: 14395 RVA: 0x0012D64C File Offset: 0x0012B84C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				SignalArgs args = new SignalArgs(signal.args);
				if (this.outSignalOutcomeArg != null)
				{
					args.Add(this.outSignalOutcomeArg.Value.Named("OUTCOME"));
				}
				Find.SignalManager.SendSignal(new Signal(this.outSignal, args));
			}
		}

		// Token: 0x0600383C RID: 14396 RVA: 0x0012D6C4 File Offset: 0x0012B8C4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Values.Look<QuestEndOutcome?>(ref this.outSignalOutcomeArg, "outSignalOutcomeArg", null, false);
		}

		// Token: 0x0600383D RID: 14397 RVA: 0x0012D715 File Offset: 0x0012B915
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.outSignal = "DebugSignal" + Rand.Int;
		}

		// Token: 0x04002142 RID: 8514
		public string inSignal;

		// Token: 0x04002143 RID: 8515
		public string outSignal;

		// Token: 0x04002144 RID: 8516
		public QuestEndOutcome? outSignalOutcomeArg;
	}
}
