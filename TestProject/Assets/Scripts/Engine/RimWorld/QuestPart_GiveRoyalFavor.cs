using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_GiveRoyalFavor : QuestPart
	{
		
		
		public override bool RequiresAccepter
		{
			get
			{
				return this.giveToAccepter;
			}
		}

		
		
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{

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

		
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.giveTo = PawnsFinder.AllMaps_FreeColonists.RandomElement<Pawn>();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			this.amount = 10;
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.giveTo, "giveTo", false);
			Scribe_Values.Look<bool>(ref this.giveToAccepter, "giveToAccepter", false, false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<int>(ref this.amount, "amount", 0, false);
		}

		
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.giveTo == replace)
			{
				this.giveTo = with;
			}
		}

		
		public Pawn giveTo;

		
		public bool giveToAccepter;

		
		public string inSignal;

		
		public int amount;

		
		public Faction faction;
	}
}
