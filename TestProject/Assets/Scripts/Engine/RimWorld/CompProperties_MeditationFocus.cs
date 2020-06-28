using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D20 RID: 3360
	public class CompProperties_MeditationFocus : CompProperties_StatOffsetBase
	{
		// Token: 0x060051BA RID: 20922 RVA: 0x001B5C27 File Offset: 0x001B3E27
		public CompProperties_MeditationFocus()
		{
			this.compClass = typeof(CompMeditationFocus);
		}

		// Token: 0x060051BB RID: 20923 RVA: 0x001B5C4A File Offset: 0x001B3E4A
		public override IEnumerable<string> GetExplanationAbstract(ThingDef def)
		{
			int num;
			for (int i = 0; i < this.offsets.Count; i = num + 1)
			{
				string explanationAbstract = this.offsets[i].GetExplanationAbstract(def);
				if (!explanationAbstract.NullOrEmpty())
				{
					yield return explanationAbstract;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x04002D27 RID: 11559
		public List<MeditationFocusDef> focusTypes = new List<MeditationFocusDef>();
	}
}
