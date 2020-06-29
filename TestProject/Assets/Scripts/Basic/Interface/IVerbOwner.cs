using System;
using System.Collections.Generic;
using Verse;

namespace Spirit
{
    
    public interface IVerbOwner
    {

        VerbTracker VerbTracker { get; }

        
        
        List<VerbProperties> VerbProperties { get; }

        
        
        List<Tool> Tools { get; }

        
        
        ImplementOwnerTypeDef ImplementOwnerTypeDef { get; }

        
        string UniqueVerbOwnerID();

        
        bool VerbsStillUsableBy(Pawn p);

        
        
        Thing ConstantCaster { get; }
    }
}
