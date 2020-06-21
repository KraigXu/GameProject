using System;
using System.Collections.Generic;
using Spirit;

namespace Heroic
{
    public interface IBillGiver
    {

        Map Map { get; }

        BillStack BillStack { get; }


        IEnumerable<IntVec3> IngredientStackCells { get; }

        string LabelShort { get; }

        bool CurrentlyUsableForBills();

        bool UsableForBillsAfterFueling();
    }
}
