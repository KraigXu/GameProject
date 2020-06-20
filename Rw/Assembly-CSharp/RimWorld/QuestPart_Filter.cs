using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200094D RID: 2381
	public abstract class QuestPart_Filter : QuestPart
	{
		// Token: 0x06003868 RID: 14440
		protected abstract bool Pass(SignalArgs args);

		// Token: 0x06003869 RID: 14441 RVA: 0x0012E0C8 File Offset: 0x0012C2C8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.Pass(signal.args))
			{
				Find.SignalManager.SendSignal(new Signal(this.outSignal, signal.args));
			}
		}

		// Token: 0x0600386A RID: 14442 RVA: 0x0012E118 File Offset: 0x0012C318
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
		}

		// Token: 0x0600386B RID: 14443 RVA: 0x0012E144 File Offset: 0x0012C344
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.outSignal = "DebugSignal" + Rand.Int;
		}

		// Token: 0x04002158 RID: 8536
		public string inSignal;

		// Token: 0x04002159 RID: 8537
		public string outSignal;
	}
}
