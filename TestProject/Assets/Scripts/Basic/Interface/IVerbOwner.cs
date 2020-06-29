using System;
using System.Collections.Generic;

namespace Spirit
{
    
    public interface IVerbOwner
    {

        VerbTracker VerbTracker { get; }

        
        // (get) Token: 0x06002212 RID: 8722
        List<VerbProperties> VerbProperties { get; }

        
        // (get) Token: 0x06002213 RID: 8723
        List<Tool> Tools { get; }

        
        // (get) Token: 0x06002214 RID: 8724
        ImplementOwnerTypeDef ImplementOwnerTypeDef { get; }

        
        string UniqueVerbOwnerID();

        
        bool VerbsStillUsableBy(Pawn p);

        
        // (get) Token: 0x06002217 RID: 8727
        Thing ConstantCaster { get; }
    }
}
