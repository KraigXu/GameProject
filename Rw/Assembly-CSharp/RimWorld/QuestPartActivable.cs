using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000998 RID: 2456
	public abstract class QuestPartActivable : QuestPart
	{
		// Token: 0x17000A74 RID: 2676
		// (get) Token: 0x06003A2C RID: 14892 RVA: 0x00134400 File Offset: 0x00132600
		public QuestPartState State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x06003A2D RID: 14893 RVA: 0x00134408 File Offset: 0x00132608
		public int EnableTick
		{
			get
			{
				if (this.State != QuestPartState.Enabled)
				{
					return -1;
				}
				return this.enableTick;
			}
		}

		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x06003A2E RID: 14894 RVA: 0x0013441C File Offset: 0x0013261C
		public string OutSignalEnabled
		{
			get
			{
				return string.Concat(new object[]
				{
					"Quest",
					this.quest.id,
					".Part",
					base.Index,
					".Enabled"
				});
			}
		}

		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x06003A2F RID: 14895 RVA: 0x00134470 File Offset: 0x00132670
		public string OutSignalCompleted
		{
			get
			{
				return string.Concat(new object[]
				{
					"Quest",
					this.quest.id,
					".Part",
					base.Index,
					".Completed"
				});
			}
		}

		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x06003A30 RID: 14896 RVA: 0x001344C1 File Offset: 0x001326C1
		public virtual string ExpiryInfoPart { get; }

		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x06003A31 RID: 14897 RVA: 0x001344C9 File Offset: 0x001326C9
		public virtual string ExpiryInfoPartTip { get; }

		// Token: 0x17000A7A RID: 2682
		// (get) Token: 0x06003A32 RID: 14898 RVA: 0x001344D1 File Offset: 0x001326D1
		public virtual AlertReport AlertReport
		{
			get
			{
				return AlertReport.Inactive;
			}
		}

		// Token: 0x17000A7B RID: 2683
		// (get) Token: 0x06003A33 RID: 14899 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual string AlertLabel
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000A7C RID: 2684
		// (get) Token: 0x06003A34 RID: 14900 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual string AlertExplanation
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000A7D RID: 2685
		// (get) Token: 0x06003A35 RID: 14901 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool AlertCritical
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x06003A36 RID: 14902 RVA: 0x001344D8 File Offset: 0x001326D8
		public bool AlertDirty
		{
			get
			{
				return this.cachedAlert != null && ((this.AlertCritical && !(this.cachedAlert is Alert_CustomCritical)) || (!this.AlertCritical && !(this.cachedAlert is Alert_Custom)));
			}
		}

		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x06003A37 RID: 14903 RVA: 0x00134518 File Offset: 0x00132718
		public Alert CachedAlert
		{
			get
			{
				AlertReport alertReport = this.AlertReport;
				if (this.cachedAlert == null)
				{
					if (!alertReport.active)
					{
						return null;
					}
					if (this.AlertCritical)
					{
						this.cachedAlert = new Alert_CustomCritical();
					}
					else
					{
						this.cachedAlert = new Alert_Custom();
					}
				}
				Alert_Custom alert_Custom = this.cachedAlert as Alert_Custom;
				if (alert_Custom != null)
				{
					if (alertReport.active)
					{
						alert_Custom.label = this.AlertLabel;
						alert_Custom.explanation = this.AlertExplanation;
					}
					alert_Custom.report = alertReport;
				}
				Alert_CustomCritical alert_CustomCritical = this.cachedAlert as Alert_CustomCritical;
				if (alert_CustomCritical != null)
				{
					if (alertReport.active)
					{
						alert_CustomCritical.label = this.AlertLabel;
						alert_CustomCritical.explanation = this.AlertExplanation;
					}
					alert_CustomCritical.report = alertReport;
				}
				return this.cachedAlert;
			}
		}

		// Token: 0x06003A38 RID: 14904 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void QuestPartTick()
		{
		}

		// Token: 0x06003A39 RID: 14905 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual string ExtraInspectString(ISelectable target)
		{
			return null;
		}

		// Token: 0x06003A3A RID: 14906 RVA: 0x001345D0 File Offset: 0x001327D0
		public void ClearCachedAlert()
		{
			this.cachedAlert = null;
		}

		// Token: 0x06003A3B RID: 14907 RVA: 0x001345DC File Offset: 0x001327DC
		protected virtual void Enable(SignalArgs receivedArgs)
		{
			if (this.state == QuestPartState.Enabled)
			{
				Log.Error("Tried to enable QuestPart while already enabled. part=" + this, false);
				return;
			}
			this.state = QuestPartState.Enabled;
			this.enableTick = Find.TickManager.TicksGame;
			Find.SignalManager.SendSignal(new Signal(this.OutSignalEnabled));
		}

		// Token: 0x06003A3C RID: 14908 RVA: 0x00134630 File Offset: 0x00132830
		protected void Complete()
		{
			this.Complete(default(SignalArgs));
		}

		// Token: 0x06003A3D RID: 14909 RVA: 0x0013464C File Offset: 0x0013284C
		protected void Complete(NamedArgument signalArg1)
		{
			this.Complete(new SignalArgs(signalArg1));
		}

		// Token: 0x06003A3E RID: 14910 RVA: 0x0013465A File Offset: 0x0013285A
		protected void Complete(NamedArgument signalArg1, NamedArgument signalArg2)
		{
			this.Complete(new SignalArgs(signalArg1, signalArg2));
		}

		// Token: 0x06003A3F RID: 14911 RVA: 0x00134669 File Offset: 0x00132869
		protected void Complete(NamedArgument signalArg1, NamedArgument signalArg2, NamedArgument signalArg3)
		{
			this.Complete(new SignalArgs(signalArg1, signalArg2, signalArg3));
		}

		// Token: 0x06003A40 RID: 14912 RVA: 0x00134679 File Offset: 0x00132879
		protected void Complete(NamedArgument signalArg1, NamedArgument signalArg2, NamedArgument signalArg3, NamedArgument signalArg4)
		{
			this.Complete(new SignalArgs(signalArg1, signalArg2, signalArg3, signalArg4));
		}

		// Token: 0x06003A41 RID: 14913 RVA: 0x0013468B File Offset: 0x0013288B
		protected void Complete(params NamedArgument[] signalArgs)
		{
			this.Complete(new SignalArgs(signalArgs));
		}

		// Token: 0x06003A42 RID: 14914 RVA: 0x0013469C File Offset: 0x0013289C
		protected virtual void Complete(SignalArgs signalArgs)
		{
			if (this.state != QuestPartState.Enabled)
			{
				Log.Error("Tried to end QuestPart but its state is not Active. part=" + this, false);
				return;
			}
			this.state = QuestPartState.Disabled;
			if (this.outcomeCompletedSignalArg != QuestEndOutcome.Unknown)
			{
				signalArgs.Add(this.outcomeCompletedSignalArg.Named("OUTCOME"));
			}
			Find.SignalManager.SendSignal(new Signal(this.OutSignalCompleted, signalArgs));
			if (!this.outSignalsCompleted.NullOrEmpty<string>())
			{
				for (int i = 0; i < this.outSignalsCompleted.Count; i++)
				{
					if (!this.outSignalsCompleted[i].NullOrEmpty())
					{
						Find.SignalManager.SendSignal(new Signal(this.outSignalsCompleted[i], signalArgs));
					}
				}
			}
		}

		// Token: 0x06003A43 RID: 14915 RVA: 0x00134757 File Offset: 0x00132957
		protected virtual void Disable()
		{
			if (this.state != QuestPartState.Enabled)
			{
				Log.Error("Tried to disable QuestPart but its state is not enabled. part=" + this, false);
				return;
			}
			this.state = QuestPartState.Disabled;
		}

		// Token: 0x06003A44 RID: 14916 RVA: 0x0013477C File Offset: 0x0013297C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignalEnable && (this.state == QuestPartState.NeverEnabled || (this.state == QuestPartState.Disabled && this.reactivatable)))
			{
				this.Enable(signal.args);
				return;
			}
			if (this.state == QuestPartState.Enabled)
			{
				if (signal.tag == this.inSignalDisable)
				{
					this.Disable();
					return;
				}
				this.ProcessQuestSignal(signal);
			}
		}

		// Token: 0x06003A45 RID: 14917 RVA: 0x00002681 File Offset: 0x00000881
		protected virtual void ProcessQuestSignal(Signal signal)
		{
		}

		// Token: 0x06003A46 RID: 14918 RVA: 0x001347EC File Offset: 0x001329EC
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignalEnable = this.quest.InitiateSignal;
		}

		// Token: 0x06003A47 RID: 14919 RVA: 0x00134808 File Offset: 0x00132A08
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignalEnable, "inSignalEnable", null, false);
			Scribe_Values.Look<string>(ref this.inSignalDisable, "inSignalDisable", null, false);
			Scribe_Values.Look<bool>(ref this.reactivatable, "reactivatable", false, false);
			if (Scribe.mode != LoadSaveMode.Saving || !this.outSignalsCompleted.NullOrEmpty<string>())
			{
				Scribe_Collections.Look<string>(ref this.outSignalsCompleted, "outSignalsCompleted", LookMode.Value, Array.Empty<object>());
			}
			Scribe_Values.Look<QuestEndOutcome>(ref this.outcomeCompletedSignalArg, "outcomeCompletedSignalArg", QuestEndOutcome.Unknown, false);
			Scribe_Values.Look<QuestPartState>(ref this.state, "state", QuestPartState.NeverEnabled, false);
			Scribe_Values.Look<int>(ref this.enableTick, "enableTick", -1, false);
		}

		// Token: 0x0400224A RID: 8778
		public string inSignalEnable;

		// Token: 0x0400224B RID: 8779
		public string inSignalDisable;

		// Token: 0x0400224C RID: 8780
		public bool reactivatable;

		// Token: 0x0400224D RID: 8781
		public List<string> outSignalsCompleted = new List<string>();

		// Token: 0x0400224E RID: 8782
		public QuestEndOutcome outcomeCompletedSignalArg;

		// Token: 0x0400224F RID: 8783
		private QuestPartState state;

		// Token: 0x04002250 RID: 8784
		protected int enableTick = -1;

		// Token: 0x04002251 RID: 8785
		private Alert cachedAlert;
	}
}
