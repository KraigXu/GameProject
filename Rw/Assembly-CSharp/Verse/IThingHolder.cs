using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000313 RID: 787
	public interface IThingHolder
	{
		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06001690 RID: 5776
		IThingHolder ParentHolder { get; }

		// Token: 0x06001691 RID: 5777
		void GetChildHolders(List<IThingHolder> outChildren);

		// Token: 0x06001692 RID: 5778
		ThingOwner GetDirectlyHeldThings();
	}
}
