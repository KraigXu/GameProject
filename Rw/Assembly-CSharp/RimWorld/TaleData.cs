using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000C30 RID: 3120
	public abstract class TaleData : IExposable
	{
		// Token: 0x06004A66 RID: 19046
		public abstract void ExposeData();

		// Token: 0x06004A67 RID: 19047 RVA: 0x00192856 File Offset: 0x00190A56
		public virtual IEnumerable<Rule> GetRules(string prefix)
		{
			Log.Error(base.GetType() + " cannot do GetRules with a prefix.", false);
			yield break;
		}

		// Token: 0x06004A68 RID: 19048 RVA: 0x00192866 File Offset: 0x00190A66
		public virtual IEnumerable<Rule> GetRules()
		{
			Log.Error(base.GetType() + " cannot do GetRules without a prefix.", false);
			yield break;
		}
	}
}
