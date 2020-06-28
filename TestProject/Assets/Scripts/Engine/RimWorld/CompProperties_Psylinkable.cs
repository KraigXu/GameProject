using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D3D RID: 3389
	public class CompProperties_Psylinkable : CompProperties
	{
		// Token: 0x0600525A RID: 21082 RVA: 0x001B856A File Offset: 0x001B676A
		public CompProperties_Psylinkable()
		{
			this.compClass = typeof(CompPsylinkable);
		}

		// Token: 0x04002D72 RID: 11634
		public List<int> requiredSubplantCountPerPsylinkLevel;

		// Token: 0x04002D73 RID: 11635
		public MeditationFocusDef requiredFocus;

		// Token: 0x04002D74 RID: 11636
		public SoundDef linkSound;

		// Token: 0x04002D75 RID: 11637
		public string enoughPlantsLetterLabel;

		// Token: 0x04002D76 RID: 11638
		public string enoughPlantsLetterText;

		// Token: 0x04002D77 RID: 11639
		public string enoughPlantsLetterLevelText;
	}
}
