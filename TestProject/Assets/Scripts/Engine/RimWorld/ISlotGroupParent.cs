using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public interface ISlotGroupParent : IStoreSettingsParent, IHaulDestination
	{
		
		// (get) Token: 0x06004C0C RID: 19468
		bool IgnoreStoredThingsBeauty { get; }

		
		IEnumerable<IntVec3> AllSlotCells();

		
		List<IntVec3> AllSlotCellsList();

		
		void Notify_ReceivedThing(Thing newItem);

		
		void Notify_LostThing(Thing newItem);

		
		string SlotYielderLabel();

		
		SlotGroup GetSlotGroup();
	}
}
