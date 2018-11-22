using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using UnityEngine;
using UnityEngine.UI;

namespace WX.Ui
{
    public class TipsWindow : UIWindowBase
    {
        [SerializeField]
        private RectTransform _rect;

        [SerializeField]
        private  Text   _text;

        private int _curId;

        protected override void SetWindowId()
        {
            this.ID = WindowID.TipsWindow;
        }

        public override void InitWindowOnAwake()
        {
        }

        protected override void InitWindowCoreData()
        {
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
            if (CheckId(id * 2)==false)
            {
                _rect.gameObject.SetActive(true);
                BaseCorrespondenceByModelControl control= _rect.gameObject.GetComponent<BaseCorrespondenceByModelControl>();
                control.Init(Camera.main,UICenterMasterManager.Instance._Camera,point);

                BiologicalData data= SqlData.GetDataId<BiologicalData>(id);
                _text.text = data.Surname+data.Name;
                _curId = id * 2;
            }
        }

        public void Hide()
        {
            _rect.gameObject.SetActive(false);
        }
    }
}