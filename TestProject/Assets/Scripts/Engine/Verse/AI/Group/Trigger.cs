using System;
using System.Collections.Generic;

namespace Verse.AI.Group
{
	// Token: 0x020005EA RID: 1514
	public abstract class Trigger
	{
		// Token: 0x060029EF RID: 10735
		public abstract bool ActivateOn(Lord lord, TriggerSignal signal);

		// Token: 0x060029F0 RID: 10736 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void SourceToilBecameActive(Transition transition, LordToil previousToil)
		{
		}

		// Token: 0x060029F1 RID: 10737 RVA: 0x000F5D15 File Offset: 0x000F3F15
		public override string ToString()
		{
			return base.GetType().ToString();
		}

		// Token: 0x04001916 RID: 6422
		public TriggerData data;

		// Token: 0x04001917 RID: 6423
		public List<TriggerFilter> filters;
	}
}
