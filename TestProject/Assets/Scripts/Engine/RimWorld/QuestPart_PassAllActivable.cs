using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_PassAllActivable : QuestPartActivable
	{
		
		
		private bool AllSignalsReceived
		{
			get
			{
				if (this.inSignals.Count != this.signalsReceived.Count)
				{
					return false;
				}
				for (int i = 0; i < this.signalsReceived.Count; i++)
				{
					if (!this.signalsReceived[i])
					{
						return false;
					}
				}
				return true;
			}
		}

		
		protected override void Enable(SignalArgs receivedArgs)
		{
			this.signalsReceived.Clear();
			base.Enable(receivedArgs);
		}

		
		protected override void ProcessQuestSignal(Signal signal)
		{
			base.ProcessQuestSignal(signal);
			int num = this.inSignals.IndexOf(signal.tag);
			if (num >= 0)
			{
				while (this.signalsReceived.Count <= num)
				{
					this.signalsReceived.Add(false);
				}
				this.signalsReceived[num] = true;
				if (this.AllSignalsReceived)
				{
					base.Complete();
				}
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<bool>(ref this.signalsReceived, "signalsReceived", LookMode.Value, Array.Empty<object>());
		}

		
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignals.Clear();
			for (int i = 0; i < 3; i++)
			{
				this.inSignals.Add("DebugSignal" + Rand.Int);
			}
		}

		
		public List<string> inSignals = new List<string>();

		
		private List<bool> signalsReceived = new List<bool>();
	}
}
