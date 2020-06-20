using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000966 RID: 2406
	public class QuestPart_Alert : QuestPartActivable
	{
		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x06003900 RID: 14592 RVA: 0x0012FE16 File Offset: 0x0012E016
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.<>n__0())
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				GlobalTargetInfo globalTargetInfo2 = this.lookTargets.TryGetPrimaryTarget();
				if (globalTargetInfo2.IsValid)
				{
					yield return globalTargetInfo2;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000A38 RID: 2616
		// (get) Token: 0x06003901 RID: 14593 RVA: 0x0012FE26 File Offset: 0x0012E026
		public override string AlertLabel
		{
			get
			{
				return this.resolvedLabel;
			}
		}

		// Token: 0x17000A39 RID: 2617
		// (get) Token: 0x06003902 RID: 14594 RVA: 0x0012FE2E File Offset: 0x0012E02E
		public override string AlertExplanation
		{
			get
			{
				return this.resolvedExplanation;
			}
		}

		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x06003903 RID: 14595 RVA: 0x0012FE36 File Offset: 0x0012E036
		public override bool AlertCritical
		{
			get
			{
				return this.critical;
			}
		}

		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x06003904 RID: 14596 RVA: 0x0012FE3E File Offset: 0x0012E03E
		public override AlertReport AlertReport
		{
			get
			{
				if (this.resolvedLookTargets.IsValid())
				{
					return AlertReport.CulpritsAre(this.resolvedLookTargets.targets);
				}
				return AlertReport.Active;
			}
		}

		// Token: 0x06003905 RID: 14597 RVA: 0x0012FE64 File Offset: 0x0012E064
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.resolvedLabel = receivedArgs.GetFormattedText(this.label);
			this.resolvedExplanation = receivedArgs.GetFormattedText(this.explanation);
			this.resolvedLookTargets = this.lookTargets;
			if (this.getLookTargetsFromSignal && !this.resolvedLookTargets.IsValid())
			{
				SignalArgsUtility.TryGetLookTargets(receivedArgs, "SUBJECT", out this.resolvedLookTargets);
			}
		}

		// Token: 0x06003906 RID: 14598 RVA: 0x0012FEE8 File Offset: 0x0012E0E8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Values.Look<string>(ref this.explanation, "explanation", null, false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.critical, "critical", false, false);
			Scribe_Values.Look<bool>(ref this.getLookTargetsFromSignal, "getLookTargetsFromSignal", false, false);
			Scribe_Values.Look<string>(ref this.resolvedLabel, "resolvedLabel", null, false);
			Scribe_Values.Look<string>(ref this.resolvedExplanation, "resolvedExplanation", null, false);
			Scribe_Deep.Look<LookTargets>(ref this.resolvedLookTargets, "resolvedLookTargets", Array.Empty<object>());
		}

		// Token: 0x06003907 RID: 14599 RVA: 0x0012FF91 File Offset: 0x0012E191
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.label = "Dev: Test";
			this.explanation = "Test text";
		}

		// Token: 0x0400219A RID: 8602
		public string label;

		// Token: 0x0400219B RID: 8603
		public string explanation;

		// Token: 0x0400219C RID: 8604
		public LookTargets lookTargets;

		// Token: 0x0400219D RID: 8605
		public bool critical;

		// Token: 0x0400219E RID: 8606
		public bool getLookTargetsFromSignal;

		// Token: 0x0400219F RID: 8607
		private string resolvedLabel;

		// Token: 0x040021A0 RID: 8608
		private string resolvedExplanation;

		// Token: 0x040021A1 RID: 8609
		private LookTargets resolvedLookTargets;
	}
}
