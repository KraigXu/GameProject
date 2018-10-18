using System;
using DataAccessObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSystem.AI;
public class BiologicalManager : MonoBehaviour
{
    public List<Biological> BiologicalArray = new List<Biological>();  //所有
    public List<Biological> BiologicalDebutArray = new List<Biological>();
    public PlayerElement Player;
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
                    break;
                case RaceType.Ghost:
                    break;
                default:
                    biological = go.AddComponent<Biological>();
                    break;
            }
            biological.Model = biologicalModels[i];

            if (Define.Value.PlayerId == biologicalModels[i].Id)
            {
                Player = go.AddComponent<PlayerElement>();
            }
            else
            {
                go.AddComponent<StateController>();
            }

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

    
}
