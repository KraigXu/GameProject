using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_Alert : QuestPartActivable
	{
		
		
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{

			
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

		
		
		public override string AlertLabel
		{
			get
			{
				return this.resolvedLabel;
			}
		}

		
		
		public override string AlertExplanation
		{
			get
			{
				return this.resolvedExplanation;
			}
		}

		
		
		public override bool AlertCritical
		{
			get
			{
				return this.critical;
			}
		}

		
		
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

		
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.label = "Dev: Test";
			this.explanation = "Test text";
		}

		
		public string label;

		
		public string explanation;

		
		public LookTargets lookTargets;

		
		public bool critical;

		
		public bool getLookTargetsFromSignal;

		
		private string resolvedLabel;

		
		private string resolvedExplanation;

		
		private LookTargets resolvedLookTargets;
	}
}
