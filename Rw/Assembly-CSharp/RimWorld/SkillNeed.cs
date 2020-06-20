using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200089B RID: 2203
	public class SkillNeed
	{
		// Token: 0x0600357A RID: 13690 RVA: 0x000255BF File Offset: 0x000237BF
		public virtual float ValueFor(Pawn pawn)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600357B RID: 13691 RVA: 0x00123AC3 File Offset: 0x00121CC3
		public virtual IEnumerable<string> ConfigErrors()
		{
			yield break;
		}

		// Token: 0x04001D4B RID: 7499
		public SkillDef skill;
	}
}
