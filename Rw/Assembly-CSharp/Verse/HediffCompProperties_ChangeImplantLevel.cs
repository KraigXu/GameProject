using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000243 RID: 579
	public class HediffCompProperties_ChangeImplantLevel : HediffCompProperties
	{
		// Token: 0x0600102C RID: 4140 RVA: 0x0005D104 File Offset: 0x0005B304
		public HediffCompProperties_ChangeImplantLevel()
		{
			this.compClass = typeof(HediffComp_ChangeImplantLevel);
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x0005D11C File Offset: 0x0005B31C
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			foreach (string text in this.<>n__0(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.implant == null)
			{
				yield return "implant is null";
			}
			else if (!typeof(Hediff_ImplantWithLevel).IsAssignableFrom(this.implant.hediffClass))
			{
				yield return "implant is not Hediff_ImplantWithLevel";
			}
			if (this.levelOffset == 0)
			{
				yield return "levelOffset is 0";
			}
			if (this.probabilityPerStage == null)
			{
				yield return "probabilityPerStage is not defined";
			}
			else if (this.probabilityPerStage.Count != parentDef.stages.Count)
			{
				yield return "probabilityPerStage count doesn't match Hediffs number of stages";
			}
			yield break;
			yield break;
		}

		// Token: 0x04000BDE RID: 3038
		public HediffDef implant;

		// Token: 0x04000BDF RID: 3039
		public int levelOffset;

		// Token: 0x04000BE0 RID: 3040
		public List<ChangeImplantLevel_Probability> probabilityPerStage;
	}
}
