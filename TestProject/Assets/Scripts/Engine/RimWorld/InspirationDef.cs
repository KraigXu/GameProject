using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008D7 RID: 2263
	public class InspirationDef : Def
	{
		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x0600364C RID: 13900 RVA: 0x00126562 File Offset: 0x00124762
		public InspirationWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (InspirationWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x04001E93 RID: 7827
		public Type inspirationClass = typeof(Inspiration);

		// Token: 0x04001E94 RID: 7828
		public Type workerClass = typeof(InspirationWorker);

		// Token: 0x04001E95 RID: 7829
		public float baseCommonality = 1f;

		// Token: 0x04001E96 RID: 7830
		public float baseDurationDays = 1f;

		// Token: 0x04001E97 RID: 7831
		public bool allowedOnAnimals;

		// Token: 0x04001E98 RID: 7832
		public bool allowedOnNonColonists;

		// Token: 0x04001E99 RID: 7833
		public bool allowedOnDownedPawns = true;

		// Token: 0x04001E9A RID: 7834
		public List<StatDef> requiredNonDisabledStats;

		// Token: 0x04001E9B RID: 7835
		public List<SkillRequirement> requiredSkills;

		// Token: 0x04001E9C RID: 7836
		public List<SkillRequirement> requiredAnySkill;

		// Token: 0x04001E9D RID: 7837
		public List<WorkTypeDef> requiredNonDisabledWorkTypes;

		// Token: 0x04001E9E RID: 7838
		public List<WorkTypeDef> requiredAnyNonDisabledWorkType;

		// Token: 0x04001E9F RID: 7839
		public List<PawnCapacityDef> requiredCapacities;

		// Token: 0x04001EA0 RID: 7840
		public List<SkillDef> associatedSkills;

		// Token: 0x04001EA1 RID: 7841
		public List<StatModifier> statOffsets;

		// Token: 0x04001EA2 RID: 7842
		public List<StatModifier> statFactors;

		// Token: 0x04001EA3 RID: 7843
		[MustTranslate]
		public string beginLetter;

		// Token: 0x04001EA4 RID: 7844
		[MustTranslate]
		public string beginLetterLabel;

		// Token: 0x04001EA5 RID: 7845
		public LetterDef beginLetterDef;

		// Token: 0x04001EA6 RID: 7846
		[MustTranslate]
		public string endMessage;

		// Token: 0x04001EA7 RID: 7847
		[MustTranslate]
		public string baseInspectLine;

		// Token: 0x04001EA8 RID: 7848
		[Unsaved(false)]
		private InspirationWorker workerInt;
	}
}
