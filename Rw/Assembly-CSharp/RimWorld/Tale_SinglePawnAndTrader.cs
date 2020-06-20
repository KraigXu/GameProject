using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000C42 RID: 3138
	public class Tale_SinglePawnAndTrader : Tale_SinglePawn
	{
		// Token: 0x06004AE1 RID: 19169 RVA: 0x0019468F File Offset: 0x0019288F
		public Tale_SinglePawnAndTrader()
		{
		}

		// Token: 0x06004AE2 RID: 19170 RVA: 0x0019478D File Offset: 0x0019298D
		public Tale_SinglePawnAndTrader(Pawn pawn, ITrader trader) : base(pawn)
		{
			this.traderData = TaleData_Trader.GenerateFrom(trader);
		}

		// Token: 0x06004AE3 RID: 19171 RVA: 0x001947A2 File Offset: 0x001929A2
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.traderData.pawnID == th.thingIDNumber;
		}

		// Token: 0x06004AE4 RID: 19172 RVA: 0x001947C2 File Offset: 0x001929C2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Trader>(ref this.traderData, "traderData", Array.Empty<object>());
		}

		// Token: 0x06004AE5 RID: 19173 RVA: 0x001947DF File Offset: 0x001929DF
		protected override IEnumerable<Rule> SpecialTextGenerationRules()
		{
			foreach (Rule rule in this.<>n__0())
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

		// Token: 0x06004AE6 RID: 19174 RVA: 0x001947EF File Offset: 0x001929EF
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.traderData = TaleData_Trader.GenerateRandom();
		}

		// Token: 0x04002A70 RID: 10864
		public TaleData_Trader traderData;
	}
}
