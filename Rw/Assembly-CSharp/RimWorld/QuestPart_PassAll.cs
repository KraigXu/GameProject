using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000944 RID: 2372
	public class QuestPart_PassAll : QuestPart
	{
		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x06003843 RID: 14403 RVA: 0x0012D7BD File Offset: 0x0012B9BD
		private bool AllSignalsReceived
		{
			get
			{
				return PassAllQuestPartUtility.AllReceived(this.inSignals, this.signalsReceived);
			}
		}

		// Token: 0x06003844 RID: 14404 RVA: 0x0012D7D0 File Offset: 0x0012B9D0
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
						Find.SignalManager.SendSignal(new Signal(this.outSignal));
					}
				}
			}
		}

		// Token: 0x06003845 RID: 14405 RVA: 0x0012D844 File Offset: 0x0012BA44
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Collections.Look<bool>(ref this.signalsReceived, "signalsReceived", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x06003846 RID: 14406 RVA: 0x0012D898 File Offset: 0x0012BA98
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

		// Token: 0x04002146 RID: 8518
		public List<string> inSignals = new List<string>();

		// Token: 0x04002147 RID: 8519
		public string outSignal;

		// Token: 0x04002148 RID: 8520
		private List<bool> signalsReceived = new List<bool>();
	}
}
