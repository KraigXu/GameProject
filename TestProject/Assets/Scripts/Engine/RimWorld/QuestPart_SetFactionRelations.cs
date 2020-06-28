using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000990 RID: 2448
	public class QuestPart_SetFactionRelations : QuestPart
	{
		// Token: 0x17000A61 RID: 2657
		// (get) Token: 0x060039EA RID: 14826 RVA: 0x00133889 File Offset: 0x00131A89
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in this.<>n__0())
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.faction != null)
				{
					yield return this.faction;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x060039EB RID: 14827 RVA: 0x0013389C File Offset: 0x00131A9C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.faction != null && this.faction != Faction.OfPlayer)
			{
				this.faction.TrySetRelationKind(Faction.OfPlayer, this.relationKind, this.canSendLetter, null, null);
			}
		}

		// Token: 0x060039EC RID: 14828 RVA: 0x00133900 File Offset: 0x00131B00
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<FactionRelationKind>(ref this.relationKind, "relationKind", FactionRelationKind.Hostile, false);
			Scribe_Values.Look<bool>(ref this.canSendLetter, "canSendLetter", false, false);
		}

		// Token: 0x04002221 RID: 8737
		public string inSignal;

		// Token: 0x04002222 RID: 8738
		public Faction faction;

		// Token: 0x04002223 RID: 8739
		public FactionRelationKind relationKind;

		// Token: 0x04002224 RID: 8740
		public bool canSendLetter;
	}
}
