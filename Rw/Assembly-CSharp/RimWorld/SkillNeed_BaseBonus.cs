using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200089C RID: 2204
	public class SkillNeed_BaseBonus : SkillNeed
	{
		// Token: 0x0600357D RID: 13693 RVA: 0x00123ACC File Offset: 0x00121CCC
		public override float ValueFor(Pawn pawn)
		{
			if (pawn.skills == null)
			{
				return 1f;
			}
			int level = pawn.skills.GetSkill(this.skill).Level;
			return this.ValueAtLevel(level);
		}

		// Token: 0x0600357E RID: 13694 RVA: 0x00123B05 File Offset: 0x00121D05
		private float ValueAtLevel(int level)
		{
			return this.baseValue + this.bonusPerLevel * (float)level;
		}

		// Token: 0x0600357F RID: 13695 RVA: 0x00123B17 File Offset: 0x00121D17
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in this.<>n__0())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			int num;
			for (int i = 1; i <= 20; i = num + 1)
			{
				if (this.ValueAtLevel(i) <= 0f)
				{
					yield return "SkillNeed yields factor < 0 at skill level " + i;
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x04001D4C RID: 7500
		private float baseValue = 0.5f;

		// Token: 0x04001D4D RID: 7501
		private float bonusPerLevel = 0.05f;
	}
}
