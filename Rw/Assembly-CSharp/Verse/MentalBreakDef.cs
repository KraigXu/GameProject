using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x020000C7 RID: 199
	public class MentalBreakDef : Def
	{
		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060005A8 RID: 1448 RVA: 0x0001BD36 File Offset: 0x00019F36
		public MentalBreakWorker Worker
		{
			get
			{
				if (this.workerInt == null && this.workerClass != null)
				{
					this.workerInt = (MentalBreakWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0001BD76 File Offset: 0x00019F76
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.intensity == MentalBreakIntensity.None)
			{
				yield return "intensity not set";
			}
			yield break;
			yield break;
		}

		// Token: 0x04000449 RID: 1097
		public Type workerClass = typeof(MentalBreakWorker);

		// Token: 0x0400044A RID: 1098
		public MentalStateDef mentalState;

		// Token: 0x0400044B RID: 1099
		public float baseCommonality;

		// Token: 0x0400044C RID: 1100
		public SimpleCurve commonalityFactorPerPopulationCurve;

		// Token: 0x0400044D RID: 1101
		public MentalBreakIntensity intensity;

		// Token: 0x0400044E RID: 1102
		public TraitDef requiredTrait;

		// Token: 0x0400044F RID: 1103
		private MentalBreakWorker workerInt;
	}
}
