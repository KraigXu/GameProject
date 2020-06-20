using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000988 RID: 2440
	public class QuestPart_ReplaceLostLeaderReferences : QuestPart
	{
		// Token: 0x060039C7 RID: 14791 RVA: 0x00133040 File Offset: 0x00131240
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				Pawn arg = signal.args.GetArg<Pawn>("SUBJECT");
				Pawn arg2 = signal.args.GetArg<Pawn>("NEWFACTIONLEADER");
				if (arg != null && arg2 != null)
				{
					List<QuestPart> partsListForReading = this.quest.PartsListForReading;
					for (int i = 0; i < partsListForReading.Count; i++)
					{
						partsListForReading[i].ReplacePawnReferences(arg, arg2);
					}
					if (arg.questTags != null)
					{
						if (arg2.questTags == null)
						{
							arg2.questTags = new List<string>();
						}
						arg2.questTags.AddRange(arg.questTags);
					}
				}
			}
		}

		// Token: 0x060039C8 RID: 14792 RVA: 0x001330EB File Offset: 0x001312EB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
		}

		// Token: 0x060039C9 RID: 14793 RVA: 0x00133105 File Offset: 0x00131305
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
		}

		// Token: 0x04002213 RID: 8723
		public string inSignal;
	}
}
