using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000247 RID: 583
	public class HediffCompProperties_DamageBrain : HediffCompProperties
	{
		// Token: 0x06001038 RID: 4152 RVA: 0x0005D34A File Offset: 0x0005B54A
		public HediffCompProperties_DamageBrain()
		{
			this.compClass = typeof(HediffComp_DamageBrain);
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x0005D36D File Offset: 0x0005B56D
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			foreach (string text in this.<>n__0(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.damageAmount == IntRange.zero)
			{
				yield return "damageAmount is not defined";
			}
			if (this.mtbDaysPerStage == null)
			{
				yield return "mtbDaysPerStage is not defined";
			}
			else if (this.mtbDaysPerStage.Count != parentDef.stages.Count)
			{
				yield return "mtbDaysPerStage count doesn't match Hediffs number of stages";
			}
			yield break;
			yield break;
		}

		// Token: 0x04000BE5 RID: 3045
		public IntRange damageAmount = IntRange.zero;

		// Token: 0x04000BE6 RID: 3046
		public List<float> mtbDaysPerStage;
	}
}
