using System;
using System.Collections.Generic;

namespace Verse
{
	
	public interface IThingHolder
	{
		
		// (get) Token: 0x06001690 RID: 5776
		IThingHolder ParentHolder { get; }

		
		void GetChildHolders(List<IThingHolder> outChildren);

		
		ThingOwner GetDirectlyHeldThings();
	}
}
