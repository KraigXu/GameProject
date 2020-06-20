using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000C3C RID: 3132
	public class Tale_DoublePawnAndDef : Tale_DoublePawn
	{
		// Token: 0x06004ABD RID: 19133 RVA: 0x00194490 File Offset: 0x00192690
		public Tale_DoublePawnAndDef()
		{
		}

		// Token: 0x06004ABE RID: 19134 RVA: 0x00194498 File Offset: 0x00192698
		public Tale_DoublePawnAndDef(Pawn firstPawn, Pawn secondPawn, Def def) : base(firstPawn, secondPawn)
		{
			this.defData = TaleData_Def.GenerateFrom(def);
		}

		// Token: 0x06004ABF RID: 19135 RVA: 0x001944AE File Offset: 0x001926AE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Def>(ref this.defData, "defData", Array.Empty<object>());
		}

		// Token: 0x06004AC0 RID: 19136 RVA: 0x001944CB File Offset: 0x001926CB
		protected override IEnumerable<Rule> SpecialTextGenerationRules()
		{
			if (this.def.defSymbol.NullOrEmpty())
			{
				Log.Error(this.def + " uses tale type with def but defSymbol is not set.", false);
			}
			foreach (Rule rule in this.<>n__0())
			{
				yield return rule;
			}
			IEnumerator<Rule> enumerator = null;
			foreach (Rule rule2 in this.defData.GetRules(this.def.defSymbol))
			{
				yield return rule2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06004AC1 RID: 19137 RVA: 0x001944DB File Offset: 0x001926DB
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.defData = TaleData_Def.GenerateFrom((Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), this.def.defType, "GetRandom"));
		}

		// Token: 0x04002A6B RID: 10859
		public TaleData_Def defData;
	}
}
