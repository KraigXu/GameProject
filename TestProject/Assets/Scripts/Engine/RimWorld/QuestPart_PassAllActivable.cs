using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000946 RID: 2374
	public class QuestPart_PassAllActivable : QuestPartActivable
	{
		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x06003849 RID: 14409 RVA: 0x0012D958 File Offset: 0x0012BB58
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

		// Token: 0x0600384A RID: 14410 RVA: 0x0012D9A6 File Offset: 0x0012BBA6
		protected override void Enable(SignalArgs receivedArgs)
		{
			this.signalsReceived.Clear();
			base.Enable(receivedArgs);
		}

		// Token: 0x0600384B RID: 14411 RVA: 0x0012D9BC File Offset: 0x0012BBBC
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

		// Token: 0x0600384C RID: 14412 RVA: 0x0012DA1D File Offset: 0x0012BC1D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<bool>(ref this.signalsReceived, "signalsReceived", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x0600384D RID: 14413 RVA: 0x0012DA54 File Offset: 0x0012BC54
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignals.Clear();
			for (int i = 0; i < 3; i++)
			{
				this.inSignals.Add("DebugSignal" + Rand.Int);
			}
		}

		// Token: 0x04002149 RID: 8521
		public List<string> inSignals = new List<string>();

		// Token: 0x0400214A RID: 8522
		private List<bool> signalsReceived = new List<bool>();
	}
}
