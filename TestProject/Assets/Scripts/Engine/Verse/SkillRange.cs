using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020000D3 RID: 211
	public class SkillRange
	{
		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060005D4 RID: 1492 RVA: 0x0001C3C1 File Offset: 0x0001A5C1
		public SkillDef Skill
		{
			get
			{
				return this.skill;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060005D5 RID: 1493 RVA: 0x0001C3C9 File Offset: 0x0001A5C9
		public IntRange Range
		{
			get
			{
				return this.range;
			}
		}

		// Token: 0x040004E5 RID: 1253
		private SkillDef skill;

		// Token: 0x040004E6 RID: 1254
		private IntRange range = IntRange.one;
	}
}
