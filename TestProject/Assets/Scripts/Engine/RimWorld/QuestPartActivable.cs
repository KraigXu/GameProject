using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public abstract class QuestPartActivable : QuestPart
	{
		
		
		public QuestPartState State
		{
			get
			{
				return this.state;
			}
		}

		
		
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

		
		
		public virtual string ExpiryInfoPart { get; }

		
		
		public virtual string ExpiryInfoPartTip { get; }

		
		
		public virtual AlertReport AlertReport
		{
			get
			{
				return AlertReport.Inactive;
			}
		}

		
		
		public virtual string AlertLabel
		{
			get
			{
				return null;
			}
		}

		
		
		public virtual string AlertExplanation
		{
			get
			{
				return null;
			}
		}

		
		
		public virtual bool AlertCritical
		{
			get
			{
				return false;
			}
		}

		
		
		public bool AlertDirty
		{
			get
			{
				return this.cachedAlert != null && ((this.AlertCritical && !(this.cachedAlert is Alert_CustomCritical)) || (!this.AlertCritical && !(this.cachedAlert is Alert_Custom)));
			}
		}

		
		
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

		
		public virtual void QuestPartTick()
		{
		}

		
		public virtual string ExtraInspectString(ISelectable target)
		{
			return null;
		}

		
		public void ClearCachedAlert()
		{
			this.cachedAlert = null;
		}

		
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

		
		protected void Complete()
		{
			this.Complete(default(SignalArgs));
		}

		
		protected void Complete(NamedArgument signalArg1)
		{
			this.Complete(new SignalArgs(signalArg1));
		}

		
		protected void Complete(NamedArgument signalArg1, NamedArgument signalArg2)
		{
			this.Complete(new SignalArgs(signalArg1, signalArg2));
		}

		
		protected void Complete(NamedArgument signalArg1, NamedArgument signalArg2, NamedArgument signalArg3)
		{
			this.Complete(new SignalArgs(signalArg1, signalArg2, signalArg3));
		}

		
		protected void Complete(NamedArgument signalArg1, NamedArgument signalArg2, NamedArgument signalArg3, NamedArgument signalArg4)
		{
			this.Complete(new SignalArgs(signalArg1, signalArg2, signalArg3, signalArg4));
		}

		
		protected void Complete(params NamedArgument[] signalArgs)
		{
			this.Complete(new SignalArgs(signalArgs));
		}

		
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

		
		protected virtual void Disable()
		{
			if (this.state != QuestPartState.Enabled)
			{
				Log.Error("Tried to disable QuestPart but its state is not enabled. part=" + this, false);
				return;
			}
			this.state = QuestPartState.Disabled;
		}

		
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

		
		protected virtual void ProcessQuestSignal(Signal signal)
		{
		}

		
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignalEnable = this.quest.InitiateSignal;
		}

		
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

		
		public string inSignalEnable;

		
		public string inSignalDisable;

		
		public bool reactivatable;

		
		public List<string> outSignalsCompleted = new List<string>();

		
		public QuestEndOutcome outcomeCompletedSignalArg;

		
		private QuestPartState state;

		
		protected int enableTick = -1;

		
		private Alert cachedAlert;
	}
}
