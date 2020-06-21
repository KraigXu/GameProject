using System;
using System.Collections.Generic;

namespace Spirit
{
    // Token: 0x02000481 RID: 1153
    public interface IVerbOwner
    {

        VerbTracker VerbTracker { get; }

        // Token: 0x170006A8 RID: 1704
        // (get) Token: 0x06002212 RID: 8722
        List<VerbProperties> VerbProperties { get; }

        // Token: 0x170006A9 RID: 1705
        // (get) Token: 0x06002213 RID: 8723
        List<Tool> Tools { get; }

        // Token: 0x170006AA RID: 1706
        // (get) Token: 0x06002214 RID: 8724
        ImplementOwnerTypeDef ImplementOwnerTypeDef { get; }

        // Token: 0x06002215 RID: 8725
        string UniqueVerbOwnerID();

        // Token: 0x06002216 RID: 8726
        bool VerbsStillUsableBy(Pawn p);

        // Token: 0x170006AB RID: 1707
        // (get) Token: 0x06002217 RID: 8727
        Thing ConstantCaster { get; }
    }
}
