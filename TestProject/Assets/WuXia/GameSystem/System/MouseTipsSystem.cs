using System;
using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using GameSystem.Ui;
using Newtonsoft.Json;
using Unity.Rendering;
namespace GameSystem
{
    /// <summary>
    /// 针对ElementType显示TipsUi
    /// </summary>
    public class MouseTipsSystem : ComponentSystem
    {

        struct Data
        {
            public readonly int Length;
            public ComponentDataArray<Position> Position;
            public ComponentDataArray<InteractionElement> Interaction;
            public ComponentDataArray<Element> Element;
            public EntityArray Entity;
        }

        [Inject]
        private Data _data;
        private Vector3 _point;
        private EntityManager _entityManager;

        public MouseTipsSystem()
        {
            _entityManager = World.Active.GetOrCreateManager<EntityManager>();
        }

        protected override void OnUpdate()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red);
                _point = hit.point;
            }
            else
            {
                _point = Vector3.zero;
            }
            ShowWindowData showWindowData = new ShowWindowData();
            TipsInfoWindowData tipsInfo = new TipsInfoWindowData();
            tipsInfo.Point = _point;
            tipsInfo.IsShow = false;
            showWindowData.contextData = tipsInfo;
            for (int i = 0; i < _data.Length; i++)
            {
                var interaction = _data.Interaction[i];
                var position = _data.Position[i];
                var element = _data.Element[i];
                if (Vector3.Distance(position.Value, _point) < interaction.Distance)
                {
                    switch (element.Type)
                    {
                        case ElementType.None:
                            break;
                        case ElementType.Biological:
                            //Biological biological=_entityManager.GetComponentData<Biological>(_data.Entity[i]);
                            
                            break;
                        case ElementType.District:
                            break;
                        case ElementType.LivingArea:
                            {
                                tipsInfo.IsShow = true;
                                
                                LivingArea livingArea = _entityManager.GetComponentData<LivingArea>(_data.Entity[i]);
                                List<TipsInfoItemData> infoItemDatas = new List<TipsInfoItemData>();
                                infoItemDatas.Add(new TipsInfoItemData("名称:", GameStaticData.LivingAreaName[livingArea.Id]));
                                infoItemDatas.Add(new TipsInfoItemData("介绍:", GameStaticData.LivingAreaDescription[livingArea.Id]));
                                tipsInfo.InfoItemDatas = infoItemDatas;
                                tipsInfo.Id = livingArea.Id;
                            }
                            break;
                        case ElementType.Terrain:
                            break;
                        case ElementType.Team:
                            break;
                    }
                }

            }

            UICenterMasterManager.Instance.ShowWindow(WindowID.TipsWindow, showWindowData);
        }
    }
}
