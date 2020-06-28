using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000C41 RID: 3137
	public class Tale_SinglePawnAndThing : Tale_SinglePawn
	{
		// Token: 0x06004ADA RID: 19162 RVA: 0x0019468F File Offset: 0x0019288F
		public Tale_SinglePawnAndThing()
		{
		}

		// Token: 0x06004ADB RID: 19163 RVA: 0x00194718 File Offset: 0x00192918
		public Tale_SinglePawnAndThing(Pawn pawn, Thing item) : base(pawn)
		{
			this.thingData = TaleData_Thing.GenerateFrom(item);
		}

		// Token: 0x06004ADC RID: 19164 RVA: 0x0019472D File Offset: 0x0019292D
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || th.thingIDNumber == this.thingData.thingID;
		}

		// Token: 0x06004ADD RID: 19165 RVA: 0x0019474D File Offset: 0x0019294D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Thing>(ref this.thingData, "thingData", Array.Empty<object>());
		}

		// Token: 0x06004ADE RID: 19166 RVA: 0x0019476A File Offset: 0x0019296A
		protected override IEnumerable<Rule> SpecialTextGenerationRules()
		{
			foreach (Rule rule in this.<>n__0())
			{
				yield return rule;
			}
			IEnumerator<Rule> enumerator = null;
			foreach (Rule rule2 in this.thingData.GetRules("THING"))
			{
				yield return rule2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06004ADF RID: 19167 RVA: 0x0019477A File Offset: 0x0019297A
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.thingData = TaleData_Thing.GenerateRandom();
		}

		// Token: 0x04002A6F RID: 10863
		public TaleData_Thing thingData;
	}
}
