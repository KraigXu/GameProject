using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000947 RID: 2375
	public class QuestPart_PassAllOutMany : QuestPart
	{
		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x0600384F RID: 14415 RVA: 0x0012DABB File Offset: 0x0012BCBB
		private bool AllSignalsReceived
		{
			get
			{
				return PassAllQuestPartUtility.AllReceived(this.inSignals, this.signalsReceived);
			}
		}

		// Token: 0x06003850 RID: 14416 RVA: 0x0012DAD0 File Offset: 0x0012BCD0
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

		// Token: 0x06003851 RID: 14417 RVA: 0x0012DB60 File Offset: 0x0012BD60
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.outSignals, "outSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<bool>(ref this.signalsReceived, "signalsReceived", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x06003852 RID: 14418 RVA: 0x0012DBB8 File Offset: 0x0012BDB8
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

		// Token: 0x0400214B RID: 8523
		public List<string> inSignals = new List<string>();

		// Token: 0x0400214C RID: 8524
		public List<string> outSignals = new List<string>();

		// Token: 0x0400214D RID: 8525
		private List<bool> signalsReceived = new List<bool>();
	}
}
