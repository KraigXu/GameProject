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
        [SerializeField]
        private List<UiLivingAreaTitleItem> _titles=new List<UiLivingAreaTitleItem>();

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
        }


        public void Change(LivingArea livingArea,HexCell cell)
        {
            bool flag = false;
            int index;
            for (index = 0; index < _titles.Count; index++)
            {
                if (_titles[index].Id == livingArea.Id)
                {
                    flag = true;
                    break;
                }
            }


            if (flag)  //说明已有
            {
                UiLivingAreaTitleItem item = _titles[index];

            }
            else   //没有
            {
                var data = SQLService.Instance.QueryUnique<LivingAreaData>(" Id=? ", livingArea.Id);
                RectTransform titleRect = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyAssetManager.UiLivingAreaTitle, transform);
                titleRect.localScale = Vector3.zero;

                UiLivingAreaTitleItem titleItem = titleRect.gameObject.GetComponent<UiLivingAreaTitleItem>();
                titleItem.Target = cell.transform;
                titleItem.Id = livingArea.Id;
                titleItem.Data = data;
                titleItem.Name = data.Name;

                _titles.Add(titleItem);

            }
        }

        void Update()
        {
            IEnumerator<UiLivingAreaTitleItem> items = _titles.GetEnumerator();
            while (items.MoveNext())
            {
                Vector3 point = items.Current.Target.position+new Vector3(0,3,0);
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
    }

}