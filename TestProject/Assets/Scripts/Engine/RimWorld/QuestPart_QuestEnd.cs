using System;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_QuestEnd : QuestPart
	{
		
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

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<QuestEndOutcome?>(ref this.outcome, "outcome", null, false);
			Scribe_Values.Look<bool>(ref this.sendLetter, "sendLetter", false, false);
		}

		
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
		}

		
		public string inSignal;

		
		public QuestEndOutcome? outcome;

		
		public bool sendLetter;
	}
}
