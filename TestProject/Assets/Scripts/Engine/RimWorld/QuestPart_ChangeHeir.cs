using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_ChangeHeir : QuestPart
	{
		
		// (get) Token: 0x06003A51 RID: 14929 RVA: 0x00134B2C File Offset: 0x00132D2C
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.n__0())
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

		
		// (get) Token: 0x06003A52 RID: 14930 RVA: 0x00134B3C File Offset: 0x00132D3C
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in this.n__1())
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

		
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.faction != null)
			{
				this.holder.royalty.SetHeir(this.heir, this.faction);
				this.done = true;
			}
		}

		
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

		
		public override void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<Pawn>(ref this.holder, "holder", false);
			Scribe_References.Look<Pawn>(ref this.heir, "heir", false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.done, "done", false, false);
		}

		
		public Faction faction;

		
		public Pawn holder;

		
		public Pawn heir;

		
		public string inSignal;

		
		public bool done;
	}
}
