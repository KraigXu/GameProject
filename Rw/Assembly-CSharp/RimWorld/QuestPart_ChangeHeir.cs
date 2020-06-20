using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099A RID: 2458
	public class QuestPart_ChangeHeir : QuestPart
	{
		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x06003A51 RID: 14929 RVA: 0x00134B2C File Offset: 0x00132D2C
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.<>n__0())
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				yield return this.holder;
				yield return this.heir;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x06003A52 RID: 14930 RVA: 0x00134B3C File Offset: 0x00132D3C
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in this.<>n__1())
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

		// Token: 0x06003A53 RID: 14931 RVA: 0x00134B4C File Offset: 0x00132D4C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.faction != null)
			{
				this.holder.royalty.SetHeir(this.heir, this.faction);
				this.done = true;
			}
		}

		// Token: 0x06003A54 RID: 14932 RVA: 0x00134B9E File Offset: 0x00132D9E
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.holder == replace)
			{
				this.holder = with;
			}
			if (this.heir == replace)
			{
				this.heir = with;
			}
		}

		// Token: 0x06003A55 RID: 14933 RVA: 0x00134BC0 File Offset: 0x00132DC0
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<Pawn>(ref this.holder, "holder", false);
			Scribe_References.Look<Pawn>(ref this.heir, "heir", false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.done, "done", false, false);
		}

		// Token: 0x0400225A RID: 8794
		public Faction faction;

		// Token: 0x0400225B RID: 8795
		public Pawn holder;

		// Token: 0x0400225C RID: 8796
		public Pawn heir;

		// Token: 0x0400225D RID: 8797
		public string inSignal;

		// Token: 0x0400225E RID: 8798
		public bool done;
	}
}
