using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TinyFrameWork
{
    public class TipsWindow : UIWindowBase
    {
        [SerializeField]
        private RectTransform _rect;
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

        public void SetTip(Vector3 point)
        {

        }
    }
}