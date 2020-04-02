using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MapMagic
{
    [System.Serializable]
    public class CityPool : ISerializationCallbackReceiver
    {
        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            throw new System.NotImplementedException();
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            throw new System.NotImplementedException();
        }

        [System.Serializable]
        public struct Transition
        {

        }

    }
}
    












