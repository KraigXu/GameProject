using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WX
{

    public enum PlayerType
    {
        WatchingWar,
        ParticipatingWar,
        Dying
    }

    public class Player : MonoBehaviour
    {
        public int PlayerId;
        public PlayerType PlayerType;

    }

}

