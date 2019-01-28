using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem.Ui
{
    /// <summary>
    /// 显示鼠标提示信息
    /// </summary>
    public class TipsWindow : UIWindowBase
    {
        [SerializeField]
        private RectTransform _textPrefab;
        [SerializeField]
        private RectTransform _pointTf;
        [SerializeField]
        private RectTransform _contentParent;

        private Vector3 _wordpos = Vector3.zero;
        private Camera _camera3D;
        private Camera _camera2D;
        private bool _isNeedModelBlockOut = false;
        private TipsInfoWindowData _infodata;
        private List<RectTransform> _initItem = new List<RectTransform>();
        private int _curId;

        protected override void InitWindowData()
        {
            this.ID = WindowID.TipsWindow;

            windowData.windowType = UIWindowType.NormalLayer;
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

        protected override void BeforeShowWindow(BaseWindowContextData contextData = null)
        {
            if (contextData == null)
                return;
            _infodata = (TipsInfoWindowData)contextData;
            List<TipsInfoItemData> infoItemDatas = _infodata.InfoItemDatas;

            if (_initItem.Count >= infoItemDatas.Count)
            {
                for (int i = 0; i < _initItem.Count; i++)
                {
                    if (i < infoItemDatas.Count)
                    {
                        TipsInfoItemData data = infoItemDatas[i];
                        RectTransform item = _initItem[i];
                        item.GetComponent<Text>().text = data.Title + data.Content;
                    }
                    else
                    {
                        WXPoolManager.Pools[Define.PoolName].Despawn(_initItem[i]);
                    }
                }
                _initItem.RemoveRange(infoItemDatas.Count,_initItem.Count-infoItemDatas.Count);
            }
            else
            {
                for (int i = 0; i < _initItem.Count; i++)
                {
                    WXPoolManager.Pools[Define.PoolName].Despawn(_initItem[i]);
                }
                _initItem.Clear();
                for (int i = 0; i < infoItemDatas.Count; i++)
                {
                    TipsInfoItemData data = infoItemDatas[i];
                    RectTransform item = WXPoolManager.Pools[Define.PoolName].Spawn(_textPrefab, _contentParent);
                    item.GetComponent<Text>().text = data.Title + data.Content;
                    _initItem.Add(item);
                }
                _curId = _infodata.Id;
            }
        }

        void LateUpdate()
        {
            if (_infodata.IsShow)
            {
                _pointTf.gameObject.SetActive(true);
            }
            else
            {
                _pointTf.gameObject.SetActive(false);
                return;
            }

            _wordpos = _infodata.Point;
            if (Define.IsAPointInACamera(_camera3D, _wordpos))
            {
                _pointTf.localScale = Vector3.one;
                Vector2 tempPos = _camera3D.WorldToScreenPoint(_wordpos);
                Vector3 temppos = _camera2D.ScreenToWorldPoint(tempPos);
                temppos.z = 0f;
                _pointTf.position = temppos;
            }
            else
            {
                _pointTf.localScale = Vector3.zero;
            }
        }
       

        //public void SetBiologicalTip(Vector3 point,int id)
        //{
        //    _curdelaytime = 0.3f;
        //    _pointTf.gameObject.SetActive(true);
        //    if (CheckId(id * 2)==false)
        //    {
        //        UiTitleitem control= _pointTf.gameObject.GetComponent<UiTitleitem>();
        //        control.Init(Camera.main,UICenterMasterManager.Instance._Camera,point);
        //        //_text.text = GameStaticData.BiologicalSurnameDic[id] + GameStaticData.BiologicalNameDic[id];
        //        _curId = id * 2;
        //    }
        //}

        //public void Hide()
        //{
        //    _pointTf.gameObject.SetActive(false);
        //}
    }
}