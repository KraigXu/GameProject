using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CC9 RID: 3273
	public class ThingSetMaker_Conditional_ResearchFinished : ThingSetMaker_Conditional
	{
		// Token: 0x06004F55 RID: 20309 RVA: 0x001AB72F File Offset: 0x001A992F
		protected override bool Condition(ThingSetMakerParams parms)
		{
			return this.researchProject.IsFinished;
		}

		// Token: 0x04002C87 RID: 11399
		public ResearchProjectDef researchProject;
	}
}
