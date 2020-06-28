using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000904 RID: 2308
	public class SkillDef : Def
	{
		// Token: 0x060036F9 RID: 14073 RVA: 0x00128933 File Offset: 0x00126B33
		public override void PostLoad()
		{
			if (this.label == null)
			{
				this.label = this.skillLabel;
			}
		}

		// Token: 0x060036FA RID: 14074 RVA: 0x0012894C File Offset: 0x00126B4C
		public bool IsDisabled(WorkTags combinedDisabledWorkTags, IEnumerable<WorkTypeDef> disabledWorkTypes)
		{
			if ((combinedDisabledWorkTags & this.disablingWorkTags) != WorkTags.None)
			{
				return true;
			}
			if (this.neverDisabledBasedOnWorkTypes)
			{
				return false;
			}
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			bool result = false;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[i];
				for (int j = 0; j < workTypeDef.relevantSkills.Count; j++)
				{
					if (workTypeDef.relevantSkills[j] == this)
					{
						if (!disabledWorkTypes.Contains(workTypeDef))
						{
							return false;
						}
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x04001FC3 RID: 8131
		[MustTranslate]
		public string skillLabel;

		// Token: 0x04001FC4 RID: 8132
		public bool usuallyDefinedInBackstories = true;

		// Token: 0x04001FC5 RID: 8133
		public bool pawnCreatorSummaryVisible;

		// Token: 0x04001FC6 RID: 8134
		public WorkTags disablingWorkTags;

		// Token: 0x04001FC7 RID: 8135
		public float listOrder;

		// Token: 0x04001FC8 RID: 8136
		public bool neverDisabledBasedOnWorkTypes;
	}
}
