using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FF2 RID: 4082
	public abstract class StatPart
	{
		// Token: 0x060061E1 RID: 25057
		public abstract void TransformValue(StatRequest req, ref float val);

		// Token: 0x060061E2 RID: 25058
		public abstract string ExplanationPart(StatRequest req);

		// Token: 0x060061E3 RID: 25059 RVA: 0x0021FFD3 File Offset: 0x0021E1D3
		public virtual IEnumerable<string> ConfigErrors()
		{
			yield break;
		}

		// Token: 0x060061E4 RID: 25060 RVA: 0x0021FFDC File Offset: 0x0021E1DC
		public virtual IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest req)
		{
			yield break;
		}

		// Token: 0x04003BD2 RID: 15314
		public float priority;

		// Token: 0x04003BD3 RID: 15315
		[Unsaved(false)]
		public StatDef parentStat;
	}
}
