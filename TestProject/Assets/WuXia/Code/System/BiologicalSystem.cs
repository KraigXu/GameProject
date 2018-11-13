using System;
using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using UnityEngine;

namespace WX
{
    public class BiologicalGroup
    {
        public int GroupId;
        public Biological LeaderId;
        public List<Biological> Partners = new List<Biological>();
    }
    public enum BiologicalModelType
    {

        HumanMen,
        HumanWoMen,

    }

    public enum RaceType
    {
        Human = 1,
        Elf = 2,
    }

    public enum SexType
    {
        Male = 1,
        Female = 2,
        Neutral = 3
    }


    public enum LocationType
    {
        Field=0,
        City = 1,
        Event=3,
    }

    public enum WhereStatus
    {
        City,
        Wilderness
    }

    public enum BiologicalStatus
    {

    }

    public class BiologicalSystem : ComponentSystem
    {
        struct BiologicalData
        {
            public readonly int Length;
            public ComponentDataArray<Biological> Biological;
        }
        [Inject] private BiologicalData _data;

        struct BiologicalPanel
        {
            public readonly int Length;
            public ComponentArray<BiologicalUi> Ui;
        }

        [Inject] private BiologicalPanel _panel;


        public static List<Biological> BiologicalDebut = new List<Biological>();
        public Dictionary<int, BiologicalGroup> GroupsDic = new Dictionary<int, BiologicalGroup>();


        protected override void OnUpdate()
        {
            //Change Property
            for (int i = 0; i < _data.Length; i++)
            {
                var b = _data.Biological[i];

                b.Jing = Convert.ToInt16(b.Tizhi + (b.Wuxing * 0.3f) + (b.Lidao * 0.5f));
                b.Qi= Convert.ToInt16(b.Jingshen + (b.Tizhi * 0.5f) + (b.Wuxing * 0.5f));
                b.Shen = Convert.ToInt16(b.Wuxing + b.Lidao * 0.3);
                _data.Biological[i] = b;
            }
        }
    }

}

