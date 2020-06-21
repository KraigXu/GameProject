using System;
using System.Collections.Generic;

namespace Spirit
{

    public interface IThingHolder
    {
        IThingHolder ParentHolder { get; }

        void GetChildHolders(List<IThingHolder> outChildren);
     
        ThingOwner GetDirectlyHeldThings();
    }
}
