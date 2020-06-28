using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000971 RID: 2417
	public class QuestPart_Dialog : QuestPart
	{
		// Token: 0x17000A44 RID: 2628
		// (get) Token: 0x06003942 RID: 14658 RVA: 0x00130BF0 File Offset: 0x0012EDF0
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

		// Token: 0x17000A45 RID: 2629
		// (get) Token: 0x06003943 RID: 14659 RVA: 0x00130C00 File Offset: 0x0012EE00
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in this.<>n__1())
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.relatedFaction != null)
				{
					yield return this.relatedFaction;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06003944 RID: 14660 RVA: 0x00130C10 File Offset: 0x0012EE10
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

		// Token: 0x06003945 RID: 14661 RVA: 0x00130E18 File Offset: 0x0012F018
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

		// Token: 0x06003946 RID: 14662 RVA: 0x00130ED4 File Offset: 0x0012F0D4
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

		// Token: 0x040021BC RID: 8636
		public string inSignal;

		// Token: 0x040021BD RID: 8637
		public string text;

		// Token: 0x040021BE RID: 8638
		public string title;

		// Token: 0x040021BF RID: 8639
		public List<QuestPart_Dialog.Option> options = new List<QuestPart_Dialog.Option>();

		// Token: 0x040021C0 RID: 8640
		public Faction relatedFaction;

		// Token: 0x040021C1 RID: 8641
		public bool addToArchive = true;

		// Token: 0x040021C2 RID: 8642
		public bool radioMode;

		// Token: 0x040021C3 RID: 8643
		public bool getLookTargetsFromSignal;

		// Token: 0x040021C4 RID: 8644
		public LookTargets lookTargets;

		// Token: 0x02001971 RID: 6513
		public class Option : IExposable
		{
			// Token: 0x060092BD RID: 37565 RVA: 0x002DF617 File Offset: 0x002DD817
			public void ExposeData()
			{
				Scribe_Values.Look<string>(ref this.text, "text", null, false);
				Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			}

			// Token: 0x040060EB RID: 24811
			public string text;

			// Token: 0x040060EC RID: 24812
			public string outSignal;
		}
	}
}
