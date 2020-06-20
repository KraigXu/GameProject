using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E0B RID: 3595
	public class Alert_UnhappyNudity : Alert_Thought
	{
		// Token: 0x17000F81 RID: 3969
		// (get) Token: 0x060056D6 RID: 22230 RVA: 0x001CCB32 File Offset: 0x001CAD32
		protected override ThoughtDef Thought
		{
			get
			{
				return ThoughtDefOf.Naked;
			}
		}

		// Token: 0x060056D7 RID: 22231 RVA: 0x001CCB39 File Offset: 0x001CAD39
		public Alert_UnhappyNudity()
		{
			this.defaultLabel = "AlertUnhappyNudity".Translate();
			this.explanationKey = "AlertUnhappyNudityDesc";
		}
	}
}
