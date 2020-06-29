﻿using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_PassAll : QuestPart
	{
		
		// (get) Token: 0x06003843 RID: 14403 RVA: 0x0012D7BD File Offset: 0x0012B9BD
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
						Find.SignalManager.SendSignal(new Signal(this.outSignal));
					}
				}
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
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
			this.outSignal = "DebugSignal" + Rand.Int;
		}

		
		public List<string> inSignals = new List<string>();

		
		public string outSignal;

		
		private List<bool> signalsReceived = new List<bool>();
	}
}
