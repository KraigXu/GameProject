using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200094C RID: 2380
	public class QuestPart_QuestEnd : QuestPart
	{
		// Token: 0x06003864 RID: 14436 RVA: 0x0012DFE8 File Offset: 0x0012C1E8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				QuestEndOutcome questEndOutcome;
				if (this.outcome != null)
				{
					questEndOutcome = this.outcome.Value;
				}
				else if (!signal.args.TryGetArg<QuestEndOutcome>("OUTCOME", out questEndOutcome))
				{
					questEndOutcome = QuestEndOutcome.Unknown;
				}
				this.quest.End(questEndOutcome, this.sendLetter);
			}
		}

		// Token: 0x06003865 RID: 14437 RVA: 0x0012E054 File Offset: 0x0012C254
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<QuestEndOutcome?>(ref this.outcome, "outcome", null, false);
			Scribe_Values.Look<bool>(ref this.sendLetter, "sendLetter", false, false);
		}

		// Token: 0x06003866 RID: 14438 RVA: 0x0012E0A5 File Offset: 0x0012C2A5
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
		}

		// Token: 0x04002155 RID: 8533
		public string inSignal;

		// Token: 0x04002156 RID: 8534
		public QuestEndOutcome? outcome;

		// Token: 0x04002157 RID: 8535
		public bool sendLetter;
	}
}
