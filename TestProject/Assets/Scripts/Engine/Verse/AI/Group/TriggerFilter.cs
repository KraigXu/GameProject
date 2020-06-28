using System;

namespace Verse.AI.Group
{
	// Token: 0x020005EC RID: 1516
	public abstract class TriggerFilter
	{
		// Token: 0x060029F5 RID: 10741
		public abstract bool AllowActivation(Lord lord, TriggerSignal signal);
	}
}
