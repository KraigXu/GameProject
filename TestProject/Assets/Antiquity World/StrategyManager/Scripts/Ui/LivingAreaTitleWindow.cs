using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem.Ui
{
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

        protected override void BeforeShowWindow(BaseWindowContextData contextData=null)
        {

            EntityArray entityArray = _livingAreaSystem.CurEntityArray;

            for (int i = 0; i < entityArray.Length; i++)
            {
                var livingArea = _entityManager.GetComponentData<LivingArea>(entityArray[i]);

                var data = SQLService.Instance.QueryUnique<LivingAreaData>(" Id=? ", livingArea.Id);

                RectTransform titleRect = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyStyle.Instance.UiLivingAreaTitle, transform);
                titleRect.localScale = Vector3.zero;


                UiLivingAreaTitleItem titleItem= titleRect.gameObject.GetComponent<UiLivingAreaTitleItem>();
                titleItem.ContetntEntity = entityArray[i];
                titleItem.Data = data;
                titleItem.Name = data.Name;

                //更改样式
                titleItem._effectImag.overrideSprite = null;
                titleItem._typeImag.overrideSprite = null;
                titleItem._usedImag.overrideSprite = null;

                _titles.Add(titleItem);
            }


            //SystemManager.Get<LivingAreaSystem>().

            // for (int i = 0; i < UPPER; i++)
            //  {

            // }

            //if(contextData==null)return;

            //base.ShowWindow(contextData);
            //_data = (FixedTitleWindowData) contextData;

            //List<KeyValuePair<string, Vector3>> kv = _data.Items;

            //if (_items.Count >= kv.Count)
            //{
            //    for (int i = 0; i < _items.Count; i++)
            //    {
            //        if (i < kv.Count)
            //        {
            //            if (_items[i].Content == kv[i].Key && _items[i].Point == kv[i].Value)
            //            {
            //                continue;
            //            }

            //            _items[i].Point = kv[i].Value;
            //            _items[i].Content = kv[i].Key;

            //        }
            //        else
            //        {
            //            WXPoolManager.Pools[Define.GeneratedPool].Despawn(_items[i].node);
            //        }
            //    }

            //    _items.RemoveRange(kv.Count, _items.Count - kv.Count);
            //}
            //else
            //{
            //    for (int i = 0; i < _items.Count; i++)
            //    {
            //        WXPoolManager.Pools[Define.GeneratedPool].Despawn(_items[i].node);
            //    }
            //    _items.Clear();
            //    for (int i = 0; i < kv.Count; i++)
            //    {
            //        RectTransform rectGo = WXPoolManager.Pools[Define.GeneratedPool].Spawn(_titlePrefab, transform);
            //        rectGo.GetComponentInChildren<Text>().text = kv[i].Key;
            //        FixedTitleTf tf=new FixedTitleTf();
            //        tf.Content = kv[i].Key;
            //        tf.Point = kv[i].Value;
            //        tf.node = rectGo;
            //        _items.Add(tf);
            //    }

            //}
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