using System;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_Pass : QuestPart
	{
		
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

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Values.Look<QuestEndOutcome?>(ref this.outSignalOutcomeArg, "outSignalOutcomeArg", null, false);
		}

		
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.outSignal = "DebugSignal" + Rand.Int;
		}

		
		public string inSignal;

		
		public string outSignal;

		
		public QuestEndOutcome? outSignalOutcomeArg;
	}
}
