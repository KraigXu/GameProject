using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000AE RID: 174
	public class EffecterDef : Def
	{
		// Token: 0x06000569 RID: 1385 RVA: 0x0001B251 File Offset: 0x00019451
		public Effecter Spawn()
		{
			return new Effecter(this);
		}

		// Token: 0x0400037E RID: 894
		public List<SubEffecterDef> children;

		// Token: 0x0400037F RID: 895
		public float positionRadius;

		// Token: 0x04000380 RID: 896
		public FloatRange offsetTowardsTarget;
	}
}
