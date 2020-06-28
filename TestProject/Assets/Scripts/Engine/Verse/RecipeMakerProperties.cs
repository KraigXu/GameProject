using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200009C RID: 156
	public class RecipeMakerProperties
	{
		// Token: 0x040002EE RID: 750
		public int productCount = 1;

		// Token: 0x040002EF RID: 751
		public int targetCountAdjustment = 1;

		// Token: 0x040002F0 RID: 752
		public int bulkRecipeCount = -1;

		// Token: 0x040002F1 RID: 753
		public int workAmount = -1;

		// Token: 0x040002F2 RID: 754
		public StatDef workSpeedStat;

		// Token: 0x040002F3 RID: 755
		public StatDef efficiencyStat;

		// Token: 0x040002F4 RID: 756
		public ThingDef unfinishedThingDef;

		// Token: 0x040002F5 RID: 757
		public ThingFilter defaultIngredientFilter;

		// Token: 0x040002F6 RID: 758
		public List<SkillRequirement> skillRequirements;

		// Token: 0x040002F7 RID: 759
		public SkillDef workSkill;

		// Token: 0x040002F8 RID: 760
		public float workSkillLearnPerTick = 1f;

		// Token: 0x040002F9 RID: 761
		public WorkTypeDef requiredGiverWorkType;

		// Token: 0x040002FA RID: 762
		public EffecterDef effectWorking;

		// Token: 0x040002FB RID: 763
		public SoundDef soundWorking;

		// Token: 0x040002FC RID: 764
		public List<ThingDef> recipeUsers;

		// Token: 0x040002FD RID: 765
		public ResearchProjectDef researchPrerequisite;

		// Token: 0x040002FE RID: 766
		public List<ResearchProjectDef> researchPrerequisites;

		// Token: 0x040002FF RID: 767
		[NoTranslate]
		public List<string> factionPrerequisiteTags;
	}
}
