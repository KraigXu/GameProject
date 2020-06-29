using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_ShuttleLeaveDelay : QuestPart_Delay
	{
		
		// (get) Token: 0x060038BF RID: 14527 RVA: 0x0012F274 File Offset: 0x0012D474
		public override AlertReport AlertReport
		{
			get
			{
				if (this.shuttle == null)
				{
					return false;
				}
				return AlertReport.CulpritIs(this.shuttle);
			}
		}

		
		// (get) Token: 0x060038C0 RID: 14528 RVA: 0x0012F295 File Offset: 0x0012D495
		public override bool AlertCritical
		{
			get
			{
				return base.TicksLeft < 60000;
			}
		}

		
		// (get) Token: 0x060038C1 RID: 14529 RVA: 0x0012F2A4 File Offset: 0x0012D4A4
		public override string AlertLabel
		{
			get
			{
				return "QuestPartShuttleLeaveDelay".Translate(base.TicksLeft.ToStringTicksToPeriod(true, false, true, true));
			}
		}

		
		// (get) Token: 0x060038C2 RID: 14530 RVA: 0x0012F2CC File Offset: 0x0012D4CC
		public override string AlertExplanation
		{
			get
			{
				return "QuestPartShuttleLeaveDelayDesc".Translate(this.quest.name, base.TicksLeft.ToStringTicksToPeriod(true, false, true, true).Colorize(ColoredText.DateTimeColor), this.shuttle.TryGetComp<CompShuttle>().RequiredThingsLabel);
			}
		}

		
		// (get) Token: 0x060038C3 RID: 14531 RVA: 0x0012F32B File Offset: 0x0012D52B
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.n__0())
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.shuttle != null)
				{
					yield return this.shuttle;
				}
				yield break;
				yield break;
			}
		}

		
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (this.inSignalsDisable.Contains(signal.tag))
			{
				this.Disable();
			}
		}

		
		public override string ExtraInspectString(ISelectable target)
		{
			if (target == this.shuttle)
			{
				return "ShuttleLeaveDelayInspectString".Translate(base.TicksLeft.ToStringTicksToPeriod(true, false, true, true));
			}
			return null;
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_Collections.Look<string>(ref this.inSignalsDisable, "inSignalsDisable", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.inSignalsDisable == null)
			{
				this.inSignalsDisable = new List<string>();
			}
		}

		
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.shuttle != null)
			{
				this.shuttle.TryGetComp<CompShuttle>().requiredPawns.Replace(replace, with);
			}
		}

		
		public Thing shuttle;

		
		public List<string> inSignalsDisable = new List<string>();
	}
}
