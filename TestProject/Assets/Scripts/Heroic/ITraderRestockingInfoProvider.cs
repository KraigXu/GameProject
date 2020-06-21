using System;

namespace Heroic
{
    // Token: 0x02000DA9 RID: 3497
    public interface ITraderRestockingInfoProvider
    {
        // Token: 0x17000F18 RID: 3864
        // (get) Token: 0x060054EC RID: 21740
        bool EverVisited { get; }

        // Token: 0x17000F19 RID: 3865
        // (get) Token: 0x060054ED RID: 21741
        bool RestockedSinceLastVisit { get; }

        // Token: 0x17000F1A RID: 3866
        // (get) Token: 0x060054EE RID: 21742
        int NextRestockTick { get; }
    }
}
