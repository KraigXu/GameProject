using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200099D RID: 2461
	public class QuestPart_GiveRoyalFavor : QuestPart
	{
		// Token: 0x17000A87 RID: 2695
		// (get) Token: 0x06003A6F RID: 14959 RVA: 0x001357A7 File Offset: 0x001339A7
		public override bool RequiresAccepter
		{
			get
			{
				return this.giveToAccepter;
			}
		}

		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x06003A70 RID: 14960 RVA: 0x001357AF File Offset: 0x001339AF
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

		// Token: 0x06003A71 RID: 14961 RVA: 0x001357C0 File Offset: 0x001339C0
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				Pawn pawn = this.giveToAccepter ? this.quest.AccepterPawn : this.giveTo;
				if (pawn == null)
				{
					signal.args.TryGetArg<Pawn>("CHOSEN", out pawn);
				}
				if (pawn != null && pawn.royalty != null)
				{
					pawn.royalty.GainFavor(this.faction, this.amount);
				}
			}
		}

		// Token: 0x06003A72 RID: 14962 RVA: 0x0013583C File Offset: 0x00133A3C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.giveTo = PawnsFinder.AllMaps_FreeColonists.RandomElement<Pawn>();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			this.amount = 10;
		}

		// Token: 0x06003A73 RID: 14963 RVA: 0x00135898 File Offset: 0x00133A98
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.giveTo, "giveTo", false);
			Scribe_Values.Look<bool>(ref this.giveToAccepter, "giveToAccepter", false, false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<int>(ref this.amount, "amount", 0, false);
		}

		// Token: 0x06003A74 RID: 14964 RVA: 0x00135903 File Offset: 0x00133B03
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.giveTo == replace)
			{
				this.giveTo = with;
			}
		}

		// Token: 0x04002276 RID: 8822
		public Pawn giveTo;

		// Token: 0x04002277 RID: 8823
		public bool giveToAccepter;

		// Token: 0x04002278 RID: 8824
		public string inSignal;

		// Token: 0x04002279 RID: 8825
		public int amount;

		// Token: 0x0400227A RID: 8826
		public Faction faction;
	}
}
