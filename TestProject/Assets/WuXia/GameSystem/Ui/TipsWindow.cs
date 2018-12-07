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
        private RectTransform _rect;

        [SerializeField]
        private  Text  _text;
        private int _curId;
        [SerializeField]
        private float _curdelaytime;

        protected override void SetWindowId()
        {
            this.ID = WindowID.TipsWindow;
        }
        protected override void InitWindowCoreData()
        {
            windowData.windowType = UIWindowType.NormalLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.colliderMode = UIWindowColliderMode.None;
            windowData.closeModel = UIWindowCloseModel.Destory;
        }

        public override void InitWindowOnAwake()
        {
        }

        void Update()
        {
            if (_curdelaytime > 0)
            {
                _curdelaytime -= Time.deltaTime;
                if (_curdelaytime < 0)
                {
                    _rect.gameObject.SetActive(false);
                }
            }
        }


        /// <summary>
        /// 检查当前ID是否与之前一样
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool CheckId(int id)
        {
            if (id == _curId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetBiologicalTip(Vector3 point,int id)
        {
            _curdelaytime = 0.3f;
            _rect.gameObject.SetActive(true);
            if (CheckId(id * 2)==false)
            {
                BaseCorrespondenceByModelControl control= _rect.gameObject.GetComponent<BaseCorrespondenceByModelControl>();
                control.Init(Camera.main,UICenterMasterManager.Instance._Camera,point);
                _text.text = GameStaticData.BiologicalSurnameDic[id] + GameStaticData.BiologicalNameDic[id];
                _curId = id * 2;
            }
        }

        public void Hide()
        {
            _rect.gameObject.SetActive(false);
        }
    }
}