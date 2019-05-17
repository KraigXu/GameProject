using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem.Ui
{

    /// <summary>
    /// 生活区标题
    /// </summary>
    public class LivingAreaTitleWindow : UIWindowBase
    {
        //Sytle Info Data
        private List<UiLivingAreaTitleItem> _titles=new List<UiLivingAreaTitleItem>();
        private EntityManager _entityManager;
        private LivingAreaSystem _livingAreaSystem;

        protected override void InitWindowData()
        {
            this.ID = WindowID.LivingAreaTitleWindow;

            windowData.windowType = UIWindowType.BackgroundLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.colliderMode = UIWindowColliderMode.None;
            windowData.closeModel = UIWindowCloseModel.Destory;

        }

        public override void InitWindowOnAwake()
        {
            _entityManager = World.Active.GetOrCreateManager<EntityManager>();
            _livingAreaSystem = SystemManager.Get<LivingAreaSystem>();
        }

        public override void ShowWindow(BaseWindowContextData contextData)
        {
            base.ShowWindow(contextData);

            
        }

        void Update()
        {
            if (_livingAreaSystem.CurEntityArray.Length != _titles.Count)
            {
                TitleSpawn();
            }
        }

        private void TitleSpawn()
        {
            EntityArray entityArray = _livingAreaSystem.CurEntityArray;
            for (int i = 0; i < entityArray.Length; i++)
            {
                var livingArea = _entityManager.GetComponentData<LivingArea>(entityArray[i]);

                var data = SQLService.Instance.QueryUnique<LivingAreaData>(" Id=? ", livingArea.Id);

                RectTransform titleRect = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyStyle.Instance.UiLivingAreaTitle, transform);
                titleRect.localScale = Vector3.zero;

                UiLivingAreaTitleItem titleItem = titleRect.gameObject.GetComponent<UiLivingAreaTitleItem>();
                titleItem.ContetntEntity = entityArray[i];
                titleItem.Data = data;
                titleItem.Name = data.Name;

                //更改样式
                titleItem._effectImag.overrideSprite = null;
                titleItem._typeImag.overrideSprite = null;
                titleItem._usedImag.overrideSprite = null;

                _titles.Add(titleItem);
            }
        }


        void LateUpdate()
        {
            IEnumerator<UiLivingAreaTitleItem> items = _titles.GetEnumerator();
            while (items.MoveNext())
            {
                Vector3 point = items.Current.Target.position;
                if (Define.IsAPointInACamera(Camera.main, point))
                {
                    Vector2 tempPos = Camera.main.WorldToScreenPoint(point);
                    items.Current.Rect.position = tempPos;
                    items.Current.Rect.localScale = Vector3.one;

                }
                else
                {
                    items.Current.Rect.localScale = Vector3.zero;
                }
            }
        }

        public void ClearItem()
        {
            for (int i = 0; i < _titles.Count; i++)
            {
                WXPoolManager.Pools[Define.GeneratedPool].Spawn(_titles[i].Rect);
            }
            _titles.Clear();

        }
    }

}