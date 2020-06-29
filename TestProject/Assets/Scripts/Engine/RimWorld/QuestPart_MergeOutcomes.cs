using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_MergeOutcomes : QuestPart
	{
		
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			int num = this.inSignals.IndexOf(signal.tag);
			if (num >= 0)
			{
				while (this.signalsReceived.Count <= num)
				{
					this.signalsReceived.Add(null);
				}
				this.signalsReceived[num] = new QuestEndOutcome?(this.GetOutcome(signal.args));
				this.CheckEnd();
			}
		}

		
		private QuestEndOutcome GetOutcome(SignalArgs args)
		{
			QuestEndOutcome result;
			if (args.TryGetArg<QuestEndOutcome>("OUTCOME", out result))
			{
				return result;
			}
			return QuestEndOutcome.Unknown;
		}

		
		private void CheckEnd()
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = this.inSignals.Count == this.signalsReceived.Count;
			for (int i = 0; i < this.signalsReceived.Count; i++)
			{
				if (this.signalsReceived[i] == null)
				{
					flag3 = false;
				}
				else if (this.signalsReceived[i].Value == QuestEndOutcome.Success)
				{
					flag = true;
				}
				else if (this.signalsReceived[i].Value == QuestEndOutcome.Fail)
				{
					flag2 = true;
				}
			}
			if (flag2)
			{
				Find.SignalManager.SendSignal(new Signal(this.outSignal, QuestEndOutcome.Fail.Named("OUTCOME")));
				return;
			}
			if (flag3)
			{
				if (flag)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignal, QuestEndOutcome.Success.Named("OUTCOME")));
					return;
				}
				Find.SignalManager.SendSignal(new Signal(this.outSignal, QuestEndOutcome.Unknown.Named("OUTCOME")));
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Collections.Look<QuestEndOutcome?>(ref this.signalsReceived, "signalsReceived", LookMode.Value, Array.Empty<object>());
		}

		
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

		
		public List<string> inSignals = new List<string>();

		
		public string outSignal;

		
		private List<QuestEndOutcome?> signalsReceived = new List<QuestEndOutcome?>();
	}
}
