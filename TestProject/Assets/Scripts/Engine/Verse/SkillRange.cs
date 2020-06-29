using System;
using RimWorld;

namespace Verse
{
	
	public class SkillRange
	{
		
		
		public SkillDef Skill
		{
			get
			{
				return this.skill;
			}
		}

		
		
		public IntRange Range
		{
			get
			{
				return this.range;
			}
		}

		
		private SkillDef skill;

		
		private IntRange range = IntRange.one;
	}
}
