using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

public class TestP : MonoBehaviour
{

    private void Awake()
    {
        Current.Game = new Game();
        Current.Game.InitData = new GameInitData();
    //    Current.Game.Scenario = ScenarioDefOf.Tutorial.scenario;


    }

    private void Update()
    {
       
    }
}
