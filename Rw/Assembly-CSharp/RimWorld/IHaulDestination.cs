using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C64 RID: 3172
	public interface IHaulDestination : IStoreSettingsParent
	{
		// Token: 0x17000D66 RID: 3430
		// (get) Token: 0x06004C06 RID: 19462
		IntVec3 Position { get; }

		// Token: 0x17000D67 RID: 3431
		// (get) Token: 0x06004C07 RID: 19463
		Map Map { get; }

		// Token: 0x06004C08 RID: 19464
		bool Accepts(Thing t);
	}
}
