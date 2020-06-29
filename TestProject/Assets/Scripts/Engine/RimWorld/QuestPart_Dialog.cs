using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_Dialog : QuestPart
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

		
		
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{

		
				IEnumerator<Faction> enumerator = null;
				if (this.relatedFaction != null)
				{
					yield return this.relatedFaction;
				}
				yield break;
				yield break;
			}
		}

		
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				DiaNode diaNode = new DiaNode(signal.args.GetFormattedText(this.text));
				LookTargets resolvedLookTargets = this.lookTargets;
				if (this.getLookTargetsFromSignal && !resolvedLookTargets.IsValid())
				{
					SignalArgsUtility.TryGetLookTargets(signal.args, "SUBJECT", out resolvedLookTargets);
				}
				if (resolvedLookTargets.IsValid())
				{
					DiaOption diaOption = new DiaOption("JumpToLocation".Translate());
					diaOption.action = delegate
					{
						CameraJumper.TryJumpAndSelect(resolvedLookTargets.TryGetPrimaryTarget());
					};
					diaOption.resolveTree = true;
					diaNode.options.Add(diaOption);
				}
				if (this.options.Any<QuestPart_Dialog.Option>())
				{
					for (int i = 0; i < this.options.Count; i++)
					{
						int localIndex = i;
						DiaOption diaOption2 = new DiaOption(signal.args.GetFormattedText(this.options[i].text));
						diaOption2.action = delegate
						{
							Find.SignalManager.SendSignal(new Signal(this.options[localIndex].outSignal));
						};
						diaOption2.resolveTree = true;
						diaNode.options.Add(diaOption2);
					}
				}
				else
				{
					DiaOption diaOption3 = new DiaOption("OK".Translate());
					diaOption3.resolveTree = true;
					diaNode.options.Add(diaOption3);
				}
				TaggedString formattedText = signal.args.GetFormattedText(this.title);
				Find.WindowStack.Add(new Dialog_NodeTreeWithFactionInfo(diaNode, this.relatedFaction, true, this.radioMode, formattedText));
				if (this.addToArchive)
				{
					Find.Archive.Add(new ArchivedDialog(diaNode.text, formattedText, this.relatedFaction));
				}
			}
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_Collections.Look<QuestPart_Dialog.Option>(ref this.options, "options", LookMode.Deep, Array.Empty<object>());
			Scribe_References.Look<Faction>(ref this.relatedFaction, "relatedFaction", false);
			Scribe_Values.Look<bool>(ref this.addToArchive, "addToArchive", true, false);
			Scribe_Values.Look<bool>(ref this.radioMode, "radioMode", false, false);
			Scribe_Values.Look<bool>(ref this.getLookTargetsFromSignal, "getLookTargetsFromSignal", false, false);
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
		}

		
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.title = "Title";
			this.text = "Dev: Test";
			this.relatedFaction = Faction.OfMechanoids;
			this.addToArchive = false;
			QuestPart_Dialog.Option option = new QuestPart_Dialog.Option();
			option.text = "Option 1";
			option.outSignal = "DebugSignal" + Rand.Int;
			this.options.Add(option);
			QuestPart_Dialog.Option option2 = new QuestPart_Dialog.Option();
			option2.text = "Option 2";
			option2.outSignal = "DebugSignal" + Rand.Int;
			this.options.Add(option2);
		}

		
		public string inSignal;

		
		public string text;

		
		public string title;

		
		public List<QuestPart_Dialog.Option> options = new List<QuestPart_Dialog.Option>();

		
		public Faction relatedFaction;

		
		public bool addToArchive = true;

		
		public bool radioMode;

		
		public bool getLookTargetsFromSignal;

		
		public LookTargets lookTargets;

		
		public class Option : IExposable
		{
			
			public void ExposeData()
			{
				Scribe_Values.Look<string>(ref this.text, "text", null, false);
				Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			}

			
			public string text;

			
			public string outSignal;
		}
	}
}
