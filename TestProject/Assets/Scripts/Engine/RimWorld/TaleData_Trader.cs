﻿using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	
	public class TaleData_Trader : TaleData
	{
		
		// (get) Token: 0x06004A7E RID: 19070 RVA: 0x00192FA9 File Offset: 0x001911A9
		private bool IsPawn
		{
			get
			{
				return this.pawnID >= 0;
			}
		}

		
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<int>(ref this.pawnID, "pawnID", -1, false);
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.Male, false);
		}

		
		public override IEnumerable<Rule> GetRules(string prefix)
		{
			string output;
			if (this.IsPawn)
			{
				output = this.name;
			}
			else
			{
				output = Find.ActiveLanguageWorker.WithIndefiniteArticle(this.name, false, false);
			}
			yield return new Rule_String(prefix + "_nameFull", output);
			string nameShortIndefinite;
			if (this.IsPawn)
			{
				nameShortIndefinite = this.name;
			}
			else
			{
				nameShortIndefinite = Find.ActiveLanguageWorker.WithIndefiniteArticle(this.name, false, false);
			}
			yield return new Rule_String(prefix + "_indefinite", nameShortIndefinite);
			yield return new Rule_String(prefix + "_nameIndef", nameShortIndefinite);
			nameShortIndefinite = null;
			if (this.IsPawn)
			{
				nameShortIndefinite = this.name;
			}
			else
			{
				nameShortIndefinite = Find.ActiveLanguageWorker.WithDefiniteArticle(this.name, false, false);
			}
			yield return new Rule_String(prefix + "_definite", nameShortIndefinite);
			yield return new Rule_String(prefix + "_nameDef", nameShortIndefinite);
			nameShortIndefinite = null;
			yield return new Rule_String(prefix + "_pronoun", this.gender.GetPronoun());
			yield return new Rule_String(prefix + "_possessive", this.gender.GetPossessive());
			yield break;
		}

		
		public static TaleData_Trader GenerateFrom(ITrader trader)
		{
			TaleData_Trader taleData_Trader = new TaleData_Trader();
			taleData_Trader.name = trader.TraderName;
			Pawn pawn = trader as Pawn;
			if (pawn != null)
			{
				taleData_Trader.pawnID = pawn.thingIDNumber;
				taleData_Trader.gender = pawn.gender;
			}
			return taleData_Trader;
		}

		
		public static TaleData_Trader GenerateRandom()
		{
			PawnKindDef pawnKindDef = (from d in DefDatabase<PawnKindDef>.AllDefs
			where d.trader
			select d).RandomElement<PawnKindDef>();
			Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, FactionUtility.DefaultFactionFrom(pawnKindDef.defaultFactionType));
			pawn.mindState.wantsToTradeWithColony = true;
			PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn, true);
			return TaleData_Trader.GenerateFrom(pawn);
		}

		
		public string name;

		
		public int pawnID = -1;

		
		public Gender gender = Gender.Male;
	}
}
