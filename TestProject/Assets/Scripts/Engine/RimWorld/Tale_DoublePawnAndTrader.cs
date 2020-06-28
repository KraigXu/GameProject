using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000C3D RID: 3133
	public class Tale_DoublePawnAndTrader : Tale_DoublePawn
	{
		// Token: 0x06004AC3 RID: 19139 RVA: 0x00194490 File Offset: 0x00192690
		public Tale_DoublePawnAndTrader()
		{
		}

		// Token: 0x06004AC4 RID: 19140 RVA: 0x0019451A File Offset: 0x0019271A
		public Tale_DoublePawnAndTrader(Pawn firstPawn, Pawn secondPawn, ITrader trader) : base(firstPawn, secondPawn)
		{
			this.traderData = TaleData_Trader.GenerateFrom(trader);
		}

		// Token: 0x06004AC5 RID: 19141 RVA: 0x00194530 File Offset: 0x00192730
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.traderData.pawnID == th.thingIDNumber;
		}

		// Token: 0x06004AC6 RID: 19142 RVA: 0x00194550 File Offset: 0x00192750
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Trader>(ref this.traderData, "traderData", Array.Empty<object>());
		}

		// Token: 0x06004AC7 RID: 19143 RVA: 0x0019456D File Offset: 0x0019276D
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

		// Token: 0x06004AC8 RID: 19144 RVA: 0x0019457D File Offset: 0x0019277D
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.traderData = TaleData_Trader.GenerateRandom();
		}

		// Token: 0x04002A6C RID: 10860
		public TaleData_Trader traderData;
	}
}
