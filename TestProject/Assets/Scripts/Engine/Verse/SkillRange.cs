using System;
using RimWorld;

namespace Verse
{
	
	public class SkillRange
	{
		
		// (get) Token: 0x060005D4 RID: 1492 RVA: 0x0001C3C1 File Offset: 0x0001A5C1
		public SkillDef Skill
		{
			get
			{
				return this.skill;
			}
		}

		
		// (get) Token: 0x060005D5 RID: 1493 RVA: 0x0001C3C9 File Offset: 0x0001A5C9
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
