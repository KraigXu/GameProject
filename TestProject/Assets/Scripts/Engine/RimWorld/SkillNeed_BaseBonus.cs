using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public class SkillNeed_BaseBonus : SkillNeed
	{
		
		public override float ValueFor(Pawn pawn)
		{
			if (pawn.skills == null)
			{
				return 1f;
			}
			int level = pawn.skills.GetSkill(this.skill).Level;
			return this.ValueAtLevel(level);
		}

		
		private float ValueAtLevel(int level)
		{
			return this.baseValue + this.bonusPerLevel * (float)level;
		}

		
		public override IEnumerable<string> ConfigErrors()
		{

			{
				
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

		
		private float baseValue = 0.5f;

		
		private float bonusPerLevel = 0.05f;
	}
}
