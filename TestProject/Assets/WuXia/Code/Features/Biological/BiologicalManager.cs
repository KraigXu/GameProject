using System;
using DataAccessObject;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class BiologicalManager : MonoBehaviour
{
    public List<Biological> BiologicalDebut = new List<Biological>();
    public Dictionary<int, BiologicalGroup> GroupsDic = new Dictionary<int, BiologicalGroup>();

    void Start()
    {

    }

    void Update()
    {

    }


    /// <summary>
    /// 初始化
    /// </summary>
    public void InitBiological()
    {
        List<BiologicalData> biologicalModels = SqlData.GetWhereDatas<BiologicalData>(" IsDebut=? ", new object[] { 1 });
        for (int i = 0; i < biologicalModels.Count; i++)
        {
            Biological biological = null;
            switch ((RaceType) biologicalModels[i].RaceType)
            {
                case RaceType.Human:
                {
                    if (biologicalModels[i].LocationType == 1)
                    {
                        GameObject biologicalgo = new GameObject(biologicalModels[i].Name);
                        biological = biologicalgo.AddComponent<BiologicalPerson>();
                    }
                    else if (biologicalModels[i].LocationType == 2)
                    {
                        ModelMapData model = SqlData.GetDataWhereOnly<ModelMapData>(" Code=? ",new object[] {biologicalModels[i].ModeCode});

                        GameObject biologicalgo = Instantiate(Resources.Load<GameObject>(model.Path));
                        biological = biologicalgo.AddComponent<BiologicalPerson>();
                    }

                    if (biological != null)
                    {
                        biological.Id = biologicalModels[i].Id;
                        biological.Surname = biologicalModels[i].Surname;
                        biological.Name = biologicalModels[i].Name;
                        biological.AvatarCode = biologicalModels[i].AvatarCode;
                        biological.ModeCode = biologicalModels[i].ModeCode;
                        biological.Title = biologicalModels[i].Title;
                        biological.Description = biologicalModels[i].Description;
                        biological.Description = biologicalModels[i].Description;
                        biological.RaceType = RaceType.Human;
                        biological.Sex = (SexType) biologicalModels[i].Sex;
                        biological.Age = biologicalModels[i].Age;
                        biological.AgeMax = biologicalModels[i].AgeMax;
                        biological.TimeAppearance = biologicalModels[i].TimeAppearance;
                        biological.TimeEnd = biologicalModels[i].TimeEnd;
                        biological.Prestige = biologicalModels[i].Prestige;
                        biological.Influence = biologicalModels[i].Influence;
                        biological.Disposition = biologicalModels[i].Disposition;
                        biological.Property1 = biologicalModels[i].Property1;
                        biological.Property2 = biologicalModels[i].Property2;
                        biological.Property3 = biologicalModels[i].Property3;
                        biological.Property4 = biologicalModels[i].Property4;
                        biological.Property5 = biologicalModels[i].Property5;
                        biological.Property6 = biologicalModels[i].Property6;
                        biological.FeatureIds = biologicalModels[i].FeatureIds;
                        biological.Location = biologicalModels[i].Location;
                        biological.LocationType = (LocationType)biologicalModels[i].LocationType;
                        biological.ArticleJson = biologicalModels[i].ArticleJson;
                        biological.EquipmentJson = biologicalModels[i].EquipmentJson;
                        biological.LanguageJson = biologicalModels[i].LanguageJson;
                        biological.GongfaJson = biologicalModels[i].GongfaJson;
                        biological.JifaJson = biologicalModels[i].JifaJson;

                        BiologicalDebut.Add(biological);
                    }
                    else
                    {
                        Debuger.LogError("Biological code+" + biological.Id + ">> Error");
                    }
                }
                    break;
                case RaceType.Elf:
                {

                }
                    break;
            }
        }
    }


    public Biological GetBiological(int id)
    {
        for (int i = 0; i < BiologicalDebut.Count; i++)
        {
            if (BiologicalDebut[i].Id == id)
            {
                return BiologicalDebut[i];
            }
        }

        return null;
    }

}
