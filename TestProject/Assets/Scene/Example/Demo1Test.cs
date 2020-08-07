using RimWorld;
using RimWorld.Planet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

public class Demo1Test : MonoBehaviour
{



   
    void Start()
    {

        Map map = new Map();
        map.uniqueID = 888888888;
        map.generationTick = GenTicks.TicksGame;
        //mapBeingGenerated = map;
        map.info.Size = new IntVec3(10,10,10);
        map.info.parent = null;
        map.ConstructComponents();



        //  Map currentMap = MapGenerator.GenerateMap(intVec, settlement, settlement.MapGeneratorDef, settlement.ExtraGenStepDefs);
        // worldInt.info.initialMapSize = intVec;

        //Current.Game = new Game();
        //Current.Game.InitData = new GameInitData();
        //Current.Game.Scenario = ScenarioDefOf.Crashlanded.scenario;
        //Find.Scenario.PreConfigure();
        //Current.Game.InitNewGame();

        //Current.Game.storyteller = new Storyteller(StorytellerDefOf.Cassandra, DifficultyDefOf.Rough);
        //Current.Game.World = WorldGenerator.GenerateWorld(0.05f, GenText.RandomSeedString(), OverallRainfall.Normal, OverallTemperature.Normal, OverallPopulation.Normal);
        //Find.GameInitData.ChooseRandomStartingTile();
        //Find.GameInitData.mapSize = 150;
        //Find.GameInitData.PrepForMapGen();
        //Find.Scenario.PreMapGenerate();

        //LongEventHandler.QueueLongEvent(delegate
        //{


        //}, "GeneratingMap", doAsynchronously: true, GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap);

    }

    void Update()
    {
        
    }
    private static void SetupForQuickTestPlay()
    {
        Current.ProgramState = ProgramState.Entry;
        Current.Game = new Game();
        Current.Game.InitData = new GameInitData();
        Current.Game.Scenario = ScenarioDefOf.Crashlanded.scenario;
        Find.Scenario.PreConfigure();
        Current.Game.storyteller = new Storyteller(StorytellerDefOf.Cassandra, DifficultyDefOf.Rough);
        Current.Game.World = WorldGenerator.GenerateWorld(0.05f, GenText.RandomSeedString(), OverallRainfall.Normal, OverallTemperature.Normal, OverallPopulation.Normal);
        Find.GameInitData.ChooseRandomStartingTile();
        Find.GameInitData.mapSize = 150;
        Find.GameInitData.PrepForMapGen();
        Find.Scenario.PreMapGenerate();
    }
}
