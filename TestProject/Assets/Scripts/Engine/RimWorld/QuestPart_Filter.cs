using System;
using Verse;

namespace RimWorld
{
	
	public abstract class QuestPart_Filter : QuestPart
	{
		
		protected abstract bool Pass(SignalArgs args);

		
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.Pass(signal.args))
			{
				Find.SignalManager.SendSignal(new Signal(this.outSignal, signal.args));
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
		}

		
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.outSignal = "DebugSignal" + Rand.Int;
		}

		
		public string inSignal;

		
		public string outSignal;
	}
}
