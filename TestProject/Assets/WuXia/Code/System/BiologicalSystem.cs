using System;
using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using UnityEngine;
using WX.Ui;

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

    public enum LocationType
    {
        None=0,
        Field = 1,    
        InLivingArea = 4,
        LivingAreaExit = 5,
        LivingAreaEnter = 6,
        LivingAreaIn = 7,
        SocialDialog=8
    }


    public class BiologicalSystem : ComponentSystem
    {

        struct BiologicalGroup
        {
            public readonly int Length;
            public ComponentDataArray<Biological> Biological;
            public ComponentArray<CapsuleCollider> Renderer;
            public ComponentDataArray<BiologicalStatus> Status;
        }

        [Inject]
        private BiologicalGroup _biologicalGroup;


        private TipsWindow _tipsWindow;
        protected override void OnUpdate()
        {
            for (int i = 0; i < _biologicalGroup.Length; i++)
            {
                var property = _biologicalGroup.Biological[i];

                var status = _biologicalGroup.Status[i];

                property.Jing = Convert.ToInt16(property.Tizhi + (property.Wuxing * 0.3f) + (property.Lidao * 0.5f));
                property.Qi = Convert.ToInt16(property.Jingshen + (property.Tizhi * 0.5f) + (property.Wuxing * 0.5f));
                property.Shen = Convert.ToInt16(property.Wuxing + property.Lidao * 0.3);

                _biologicalGroup.Biological[i] = property;

                

                
            }
        }


    }

}

