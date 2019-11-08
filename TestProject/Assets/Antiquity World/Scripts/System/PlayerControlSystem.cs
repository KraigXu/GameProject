using System.Collections;
using System.Collections.Generic;
using GameSystem.Ui;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{


    public class PlayerControlSystem : ComponentSystem
    {
        struct Data
        {
            public readonly int Length;
            public EntityArray Entitys;
            public ComponentDataArray<PlayerMember> Members;
            public ComponentDataArray<Biological> Biologicals;
        }
        [Inject]
        private Data _data;

        private PlayerInfoWindow _infoWin;

        protected override void OnUpdate()
        {
            for (int i = 0; i < _data.Length; i++)
            {
                Entity entity = _data.Entitys[i];
                Biological biological = _data.Biologicals[i];

                _infoWin.ChangePlayer(entity);
                //GameStaticData.BiologicalRunDic[entity]
            }
        }



        public void OpenUi()
        {
            _infoWin=(PlayerInfoWindow)UICenterMasterManager.Instance.ShowWindow(WindowID.PlayerInfoWindow);


            
        }

        public void CloseUi()
        {

        }



        public static void SetupComponentData(EntityManager entityManager, Entity entity)
        {
            entityManager.AddComponentData(entity, new PlayerMember());
        }

        




    }

}