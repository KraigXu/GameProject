using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_PassAllOutMany : QuestPart
	{
		
		// (get) Token: 0x0600384F RID: 14415 RVA: 0x0012DABB File Offset: 0x0012BCBB
		private bool AllSignalsReceived
		{
			get
			{
				return PassAllQuestPartUtility.AllReceived(this.inSignals, this.signalsReceived);
			}
		}

		
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (!this.AllSignalsReceived)
			{
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
						for (int i = 0; i < this.outSignals.Count; i++)
						{
							Find.SignalManager.SendSignal(new Signal(this.outSignals[i]));
						}
					}
				}
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.outSignals, "outSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<bool>(ref this.signalsReceived, "signalsReceived", LookMode.Value, Array.Empty<object>());
		}

		
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignals.Clear();
			for (int i = 0; i < 3; i++)
			{
				this.inSignals.Add("DebugSignal" + Rand.Int);
				this.outSignals.Add("DebugSignal" + Rand.Int);
			}
		}

		
		public List<string> inSignals = new List<string>();

		
		public List<string> outSignals = new List<string>();

		
		private List<bool> signalsReceived = new List<bool>();
	}
}
