using System;
using Verse;

namespace RimWorld
{
	
	public class Alert_UnhappyNudity : Alert_Thought
	{
		
		// (get) Token: 0x060056D6 RID: 22230 RVA: 0x001CCB32 File Offset: 0x001CAD32
		protected override ThoughtDef Thought
		{
			get
			{
				return ThoughtDefOf.Naked;
			}
		}

		
		public Alert_UnhappyNudity()
		{
			this.defaultLabel = "AlertUnhappyNudity".Translate();
			this.explanationKey = "AlertUnhappyNudityDesc";
		}
	}
}
