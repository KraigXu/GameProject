using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000C35 RID: 3125
	public class TaleData_Trader : TaleData
	{
		// Token: 0x17000D1C RID: 3356
		// (get) Token: 0x06004A7E RID: 19070 RVA: 0x00192FA9 File Offset: 0x001911A9
		private bool IsPawn
		{
			get
			{
				return this.pawnID >= 0;
			}
		}

		// Token: 0x06004A7F RID: 19071 RVA: 0x00192FB7 File Offset: 0x001911B7
		public override void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<int>(ref this.pawnID, "pawnID", -1, false);
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.Male, false);
		}

		// Token: 0x06004A80 RID: 19072 RVA: 0x00192FEF File Offset: 0x001911EF
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

		// Token: 0x06004A81 RID: 19073 RVA: 0x00193008 File Offset: 0x00191208
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

		// Token: 0x06004A82 RID: 19074 RVA: 0x0019304C File Offset: 0x0019124C
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

		// Token: 0x04002A5C RID: 10844
		public string name;

		// Token: 0x04002A5D RID: 10845
		public int pawnID = -1;

		// Token: 0x04002A5E RID: 10846
		public Gender gender = Gender.Male;
	}
}
