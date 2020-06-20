using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000C40 RID: 3136
	public class Tale_SinglePawnAndDef : Tale_SinglePawn
	{
		// Token: 0x06004AD4 RID: 19156 RVA: 0x0019468F File Offset: 0x0019288F
		public Tale_SinglePawnAndDef()
		{
		}

		// Token: 0x06004AD5 RID: 19157 RVA: 0x00194697 File Offset: 0x00192897
		public Tale_SinglePawnAndDef(Pawn pawn, Def def) : base(pawn)
		{
			this.defData = TaleData_Def.GenerateFrom(def);
		}

		// Token: 0x06004AD6 RID: 19158 RVA: 0x001946AC File Offset: 0x001928AC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Def>(ref this.defData, "defData", Array.Empty<object>());
		}

		// Token: 0x06004AD7 RID: 19159 RVA: 0x001946C9 File Offset: 0x001928C9
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

		// Token: 0x06004AD8 RID: 19160 RVA: 0x001946D9 File Offset: 0x001928D9
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.defData = TaleData_Def.GenerateFrom((Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), this.def.defType, "GetRandom"));
		}

		// Token: 0x04002A6E RID: 10862
		public TaleData_Def defData;
	}
}
