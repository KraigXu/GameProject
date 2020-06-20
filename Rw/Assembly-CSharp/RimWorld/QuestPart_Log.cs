using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000962 RID: 2402
	public class QuestPart_Log : QuestPart
	{
		// Token: 0x060038DF RID: 14559 RVA: 0x0012F6F5 File Offset: 0x0012D8F5
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				Log.Message(signal.args.GetFormattedText(this.message), false);
			}
		}

		// Token: 0x060038E0 RID: 14560 RVA: 0x0012F733 File Offset: 0x0012D933
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.message, "message", null, false);
		}

		// Token: 0x060038E1 RID: 14561 RVA: 0x0012F75F File Offset: 0x0012D95F
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.message = "Dev: Test";
		}

		// Token: 0x04002189 RID: 8585
		public string inSignal;

		// Token: 0x0400218A RID: 8586
		public string message;
	}
}
