using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200089D RID: 2205
	public class SkillNeed_Direct : SkillNeed
	{
		// Token: 0x06003582 RID: 13698 RVA: 0x00123B50 File Offset: 0x00121D50
		public override float ValueFor(Pawn pawn)
		{
			if (pawn.skills == null)
			{
				return 1f;
			}
			int level = pawn.skills.GetSkill(this.skill).Level;
			if (this.valuesPerLevel.Count > level)
			{
				return this.valuesPerLevel[level];
			}
			if (this.valuesPerLevel.Count > 0)
			{
				return this.valuesPerLevel[this.valuesPerLevel.Count - 1];
			}
			return 1f;
		}

		// Token: 0x04001D4E RID: 7502
		public List<float> valuesPerLevel = new List<float>();
	}
}
