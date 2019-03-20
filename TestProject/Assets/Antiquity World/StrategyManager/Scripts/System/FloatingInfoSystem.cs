using System.Collections;
using System.Collections.Generic;
using GameSystem.Ui;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace GameSystem
{
    /// <summary>
    /// 悬浮窗系统
    /// </summary>
    public class FloatingInfoSystem : ComponentSystem
    {
        struct Data
        {
            public readonly int Length;
            public ComponentDataArray<Position> Position;
            public ComponentDataArray<Element> Element;
            public ComponentDataArray<FloatingInfo> FloatingInfo;
            public EntityArray Entity;
        }
        [Inject]
        private Data _data;

        private EntityManager _entityManager;

        public FloatingInfoSystem()
        {
            _entityManager = World.Active.GetOrCreateManager<EntityManager>();
        }


        protected override void OnUpdate()
        {
            ShowWindowData showWindowData = new ShowWindowData();
            FixedTitleWindowData fixedTitle=new FixedTitleWindowData();
            List<KeyValuePair<string, Vector3>> items = new List<KeyValuePair<string, Vector3>>();

            for (int i = 0; i < _data.Length; i++)
            {
                var position = _data.Position[i];
                var element = _data.Element[i];

                switch (element.Type)
                {
                    case ElementType.None:
                        break;
                    case ElementType.Biological:
                        break;
                    case ElementType.District:
                        break;
                    case ElementType.LivingArea:
                    {
                        LivingArea livingArea = _entityManager.GetComponentData<LivingArea>(_data.Entity[i]);
                        items.Add(new KeyValuePair<string, Vector3>(GameStaticData.LivingAreaName[livingArea.Id], position.Value));
                    }
                        break;
                    case ElementType.Terrain:
                        break;
                    case ElementType.Team:
                        break;
                }
            }

            fixedTitle.Items = items;
            showWindowData.contextData = fixedTitle;
            UICenterMasterManager.Instance.ShowWindow(WindowID.FixedTitleWindow, showWindowData);
        }

    }

}