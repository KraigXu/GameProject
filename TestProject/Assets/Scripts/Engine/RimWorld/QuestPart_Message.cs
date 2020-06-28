using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000983 RID: 2435
	public class QuestPart_Message : QuestPart
	{
		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x060039A6 RID: 14758 RVA: 0x0013286E File Offset: 0x00130A6E
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

		// Token: 0x060039A7 RID: 14759 RVA: 0x00132880 File Offset: 0x00130A80
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				LookTargets lookTargets = this.lookTargets;
				if (this.getLookTargetsFromSignal && !lookTargets.IsValid())
				{
					SignalArgsUtility.TryGetLookTargets(signal.args, "SUBJECT", out lookTargets);
				}
				TaggedString formattedText = signal.args.GetFormattedText(this.message);
				if (!formattedText.NullOrEmpty())
				{
					Messages.Message(formattedText, lookTargets, this.messageType ?? MessageTypeDefOf.NeutralEvent, this.quest, this.historical);
				}
			}
		}

		// Token: 0x060039A8 RID: 14760 RVA: 0x0013291C File Offset: 0x00130B1C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.message, "message", null, false);
			Scribe_Defs.Look<MessageTypeDef>(ref this.messageType, "messageType");
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.historical, "historical", true, false);
			Scribe_Values.Look<bool>(ref this.getLookTargetsFromSignal, "getLookTargetsFromSignal", true, false);
		}

		// Token: 0x060039A9 RID: 14761 RVA: 0x0013299C File Offset: 0x00130B9C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.message = "Dev: Test";
			this.messageType = MessageTypeDefOf.PositiveEvent;
		}

		// Token: 0x040021FC RID: 8700
		public string inSignal;

		// Token: 0x040021FD RID: 8701
		public string message;

		// Token: 0x040021FE RID: 8702
		public MessageTypeDef messageType;

		// Token: 0x040021FF RID: 8703
		public LookTargets lookTargets;

		// Token: 0x04002200 RID: 8704
		public bool historical = true;

		// Token: 0x04002201 RID: 8705
		public bool getLookTargetsFromSignal = true;
	}
}
