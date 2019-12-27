using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Unity.Entities;


namespace Signals
{

    public class NoneParameter : Signal { }


    public class DataLoad : Signal<DataLoadEcs> { }
    public class MouseSignal : Signal<MouseInput> { }

    public class BiologicalEntityData : Signal<EntityArray> { }
}
