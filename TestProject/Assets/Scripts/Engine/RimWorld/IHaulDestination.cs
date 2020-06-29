using System;
using Verse;

namespace RimWorld
{
	
	public interface IHaulDestination : IStoreSettingsParent
	{
		
		// (get) Token: 0x06004C06 RID: 19462
		IntVec3 Position { get; }

		
		// (get) Token: 0x06004C07 RID: 19463
		Map Map { get; }

		
		bool Accepts(Thing t);
	}
}
