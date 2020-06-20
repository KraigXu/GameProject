using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000959 RID: 2393
	public class QuestPart_MergeOutcomes : QuestPart
	{
		// Token: 0x0600389E RID: 14494 RVA: 0x0012EA20 File Offset: 0x0012CC20
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

		// Token: 0x0600389F RID: 14495 RVA: 0x0012EA94 File Offset: 0x0012CC94
		private QuestEndOutcome GetOutcome(SignalArgs args)
		{
			QuestEndOutcome result;
			if (args.TryGetArg<QuestEndOutcome>("OUTCOME", out result))
			{
				return result;
			}
			return QuestEndOutcome.Unknown;
		}

		// Token: 0x060038A0 RID: 14496 RVA: 0x0012EAB4 File Offset: 0x0012CCB4
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

		// Token: 0x060038A1 RID: 14497 RVA: 0x0012EBC0 File Offset: 0x0012CDC0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Collections.Look<QuestEndOutcome?>(ref this.signalsReceived, "signalsReceived", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x060038A2 RID: 14498 RVA: 0x0012EC14 File Offset: 0x0012CE14
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

		// Token: 0x04002172 RID: 8562
		public List<string> inSignals = new List<string>();

		// Token: 0x04002173 RID: 8563
		public string outSignal;

		// Token: 0x04002174 RID: 8564
		private List<QuestEndOutcome?> signalsReceived = new List<QuestEndOutcome?>();
	}
}
