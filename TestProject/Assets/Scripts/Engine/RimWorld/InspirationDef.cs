using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class InspirationDef : Def
	{
		
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

		
		public Type inspirationClass = typeof(Inspiration);

		
		public Type workerClass = typeof(InspirationWorker);

		
		public float baseCommonality = 1f;

		
		public float baseDurationDays = 1f;

		
		public bool allowedOnAnimals;

		
		public bool allowedOnNonColonists;

		
		public bool allowedOnDownedPawns = true;

		
		public List<StatDef> requiredNonDisabledStats;

		
		public List<SkillRequirement> requiredSkills;

		
		public List<SkillRequirement> requiredAnySkill;

		
		public List<WorkTypeDef> requiredNonDisabledWorkTypes;

		
		public List<WorkTypeDef> requiredAnyNonDisabledWorkType;

		
		public List<PawnCapacityDef> requiredCapacities;

		
		public List<SkillDef> associatedSkills;

		
		public List<StatModifier> statOffsets;

		
		public List<StatModifier> statFactors;

		
		[MustTranslate]
		public string beginLetter;

		
		[MustTranslate]
		public string beginLetterLabel;

		
		public LetterDef beginLetterDef;

		
		[MustTranslate]
		public string endMessage;

		
		[MustTranslate]
		public string baseInspectLine;

		
		[Unsaved(false)]
		private InspirationWorker workerInt;
	}
}
