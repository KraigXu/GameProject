using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C66 RID: 3174
	public interface ISlotGroupParent : IStoreSettingsParent, IHaulDestination
	{
		// Token: 0x17000D69 RID: 3433
		// (get) Token: 0x06004C0C RID: 19468
		bool IgnoreStoredThingsBeauty { get; }

		// Token: 0x06004C0D RID: 19469
		IEnumerable<IntVec3> AllSlotCells();

		// Token: 0x06004C0E RID: 19470
		List<IntVec3> AllSlotCellsList();

		// Token: 0x06004C0F RID: 19471
		void Notify_ReceivedThing(Thing newItem);

		// Token: 0x06004C10 RID: 19472
		void Notify_LostThing(Thing newItem);

		// Token: 0x06004C11 RID: 19473
		string SlotYielderLabel();

		// Token: 0x06004C12 RID: 19474
		SlotGroup GetSlotGroup();
	}
}
