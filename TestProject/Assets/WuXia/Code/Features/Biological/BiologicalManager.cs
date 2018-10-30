using System;
using DataAccessObject;
using System.Collections.Generic;
using UnityEngine;

public class BiologicalManager : MonoBehaviour
{
    public List<Biological> BiologicalArray = new List<Biological>();  //所有
    public List<Biological> BiologicalDebutArray = new List<Biological>();

    public List<BiologicalGroup> BiologicalGroups= new List<BiologicalGroup>();
    public Dictionary<int, BiologicalGroup> GroupsDic = new Dictionary<int, BiologicalGroup>();
    [SerializeField]
    private GameObject _biologicalGoPrefab;

    void Awake()
    {
    }

    void Start()
    {

    }

    void Update()
    {

    }
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="time"></param>
    public void InitBiological(DateTime time)
    {
        List<BiologicalData> biologicalModels = SqlData.GetWhereDatas<BiologicalData>(" IsDebut=? ", new object[] { 1 });

        for (int i = 0; i < biologicalModels.Count; i++)
        {
            GameObject go = GameObject.Instantiate(_biologicalGoPrefab, transform);
            Biological biological = null;
            switch ((RaceType)biologicalModels[i].RaceType)
            {
                case RaceType.Human:
                    biological = go.AddComponent<BiologicalPerson>();
                    biological.BiologicalId = biologicalModels[i].Id;
                    biological.Name = biologicalModels[i].Name;
                    biological.Description = biologicalModels[i].Description;
                    biological.RaceType = RaceType.Human;
                    biological.Sex = (SexType)biologicalModels[i].Sex;
                    biological.Age = biologicalModels[i].Age;
                    biological.AgeMax = biologicalModels[i].AgeMax;
                    biological.TimeAppearance = biologicalModels[i].TimeAppearance;
                    biological.TimeEnd = biologicalModels[i].TimeEnd;
                    biological.Prestige = biologicalModels[i].Prestige;
                    //----Human
                    break;
                case RaceType.Ghost:
                    break;
                default:
                    biological = go.AddComponent<Biological>();
                    break;
            }
            biological.Model = biologicalModels[i];


            //if (Define.Value.PlayerId == biologicalModels[i].Id)
            //{
            //    PlayerCon = go.AddComponent<PlayerControl>();
            //}
            //else
            //{
            //    go.AddComponent<StateController>();
            //}

            BiologicalDebutArray.Add(biological);
        }
    }

    public Biological GetPlayer(int playerId)
    {
        for (int i = 0; i < BiologicalDebutArray.Count; i++)
        {
            if (BiologicalDebutArray[i].Model.Id == playerId)
            {
                return BiologicalDebutArray[i];
            }
        }
        return null;
    }


    //-----------------------Behaviour

    public void CalculationProperty(Biological biological)
    {
        if (biological.Model != null)
        {
            switch ((RaceType)biological.Model.RaceType)
            {
                case RaceType.Human:
                    //biological.Blood=biological.Model.
                    
                    break;
                case RaceType.Elf:
                    break;
                case RaceType.Ghost:
                    break;  
            }
        }
        else
        {
            Debuger.LogError("Model Is NULL");
        }

    }


    


}
