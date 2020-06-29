﻿using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	
	public class Tale_DoublePawnAndTrader : Tale_DoublePawn
	{
		
		public Tale_DoublePawnAndTrader()
		{
		}

		
		public Tale_DoublePawnAndTrader(Pawn firstPawn, Pawn secondPawn, ITrader trader) : base(firstPawn, secondPawn)
		{
			this.traderData = TaleData_Trader.GenerateFrom(trader);
		}

		
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.traderData.pawnID == th.thingIDNumber;
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Trader>(ref this.traderData, "traderData", Array.Empty<object>());
		}

		
		protected override IEnumerable<Rule> SpecialTextGenerationRules()
		{
			foreach (Rule rule in this.n__0())
			{
				yield return rule;
			}
			IEnumerator<Rule> enumerator = null;
			foreach (Rule rule2 in this.traderData.GetRules("TRADER"))
			{
				yield return rule2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.traderData = TaleData_Trader.GenerateRandom();
		}

		
		public TaleData_Trader traderData;
	}
}
