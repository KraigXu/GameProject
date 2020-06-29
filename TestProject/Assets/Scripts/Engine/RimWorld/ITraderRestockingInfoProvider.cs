using System;

namespace RimWorld
{
	
	public interface ITraderRestockingInfoProvider
	{
		
		// (get) Token: 0x060054EC RID: 21740
		bool EverVisited { get; }

		
		// (get) Token: 0x060054ED RID: 21741
		bool RestockedSinceLastVisit { get; }

		
		// (get) Token: 0x060054EE RID: 21742
		int NextRestockTick { get; }
	}
}
