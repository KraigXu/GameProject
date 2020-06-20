using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x020010E7 RID: 4327
	public class QuestTextRequest
	{
		// Token: 0x04003DDE RID: 15838
		public string keyword;

		// Token: 0x04003DDF RID: 15839
		public Action<string> setter;

		// Token: 0x04003DE0 RID: 15840
		public List<Rule> extraRules;
	}
}
