using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace GameSystem.Ui
{
    /// <summary>
    /// 固定标题窗口
    /// </summary>
    public class FixedTitleWindow : UIWindowBase
    {

        [SerializeField]
        private RectTransform _titlePrefab;

        private FixedTitleWindowData _data;
        private List<FixedTitleTf> _items= new List<FixedTitleTf>();
        private Camera _camera3D;
        private Camera _camera2D;

        [SerializeField]
        private RectTransform _Prefab;
        private Dictionary<int, UiLivingAreaTitleItem> _titleMap = new Dictionary<int, UiLivingAreaTitleItem>();

        public class FixedTitleTf
        {
            public string Content;
            public Vector3 Point;
            public RectTransform node;
        }

        protected override void InitWindowData()
        {
            this.ID = WindowID.FixedTitleWindow;

            windowData.windowType = UIWindowType.BackgroundLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.colliderMode = UIWindowColliderMode.None;
            windowData.closeModel = UIWindowCloseModel.Destory;

        }

        public override void InitWindowOnAwake()
        {
            _camera3D = Camera.main;
            _camera2D = UICenterMasterManager.Instance._Camera;
        }

        protected override void BeforeShowWindow(BaseWindowContextData contextData)
        {
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

        //void LateUpdate()
        //{

        //    for (int i = 0; i < _items.Count; i++)
        //    {
        //        if (Define.IsAPointInACamera(_camera3D, _items[i].Point))
        //        {
                    
        //            Vector2 tempPos = _camera3D.WorldToScreenPoint(_items[i].Point);
        //            Vector3 temppos = _camera2D.ScreenToWorldPoint(tempPos);
        //            temppos.z = 0f;
        //            _items[i].node.localScale = Vector3.one;
        //            _items[i].node.position = temppos;
        //        }
        //        else
        //        {
        //            _items[i].node.localScale = Vector3.zero;
        //        }
        //    }
            
        //}

        void LateUpdate()
        {
            IEnumerator<UiLivingAreaTitleItem> items = _titleMap.Values.GetEnumerator();
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

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="livingArea"></param>
        /// <param name="target"></param>
        public void Change(LivingArea livingArea, Transform target)
        {
            if (livingArea.TitleType == 0)
            {
                if (_titleMap.ContainsKey(livingArea.Id) == true)
                {
                    _titleMap[livingArea.Id].Target = target;
                }
                else
                {
                    RectTransform newRect = WXPoolManager.Pools[Define.GeneratedPool].Spawn(_Prefab, transform);
                    UiLivingAreaTitleItem uiLivingAreaTitleItem = newRect.gameObject.GetComponent<UiLivingAreaTitleItem>();
                    uiLivingAreaTitleItem.Target = target;

                    LivingAreaData data = SQLService.Instance.QueryUnique<LivingAreaData>(" Id=? ", livingArea.Id);
                    uiLivingAreaTitleItem.Text.text = data.Name;

                    _titleMap.Add(livingArea.Id, uiLivingAreaTitleItem);
                }
            }
            else if (livingArea.TitleType == 1)
            {
                if (_titleMap.ContainsKey(livingArea.Id) == true)
                {
                    _titleMap[livingArea.Id].Rect.localScale = Vector3.zero;
                }
                else
                {
                    RectTransform newRect = WXPoolManager.Pools[Define.GeneratedPool].Spawn(_Prefab, transform);
                    _titleMap.Add(livingArea.Id, newRect.gameObject.GetComponent<UiLivingAreaTitleItem>());
                    _titleMap[livingArea.Id].Rect.localScale = Vector3.zero;
                }

            }
            else if (livingArea.TitleType == 2)
            {
            }
            else
            {


            }
        }
    }

}