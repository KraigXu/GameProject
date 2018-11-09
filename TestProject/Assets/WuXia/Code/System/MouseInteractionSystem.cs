using System.Collections;
using System.Collections.Generic;
using TinyFrameWork;
using Unity.Entities;
using UnityEngine;

namespace  WX
{
    public class MouseInteractionSystem : ComponentSystem
    {

        struct BiologicalData
        {
            public readonly int Length;
            public ComponentDataArray<Biological> Biological;
            public ComponentArray<CapsuleCollider> Renderer;
        }
        [Inject]
        private BiologicalData _biologicalData;

        private TipsWindow _tipsWindow;
        protected override void OnUpdate()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    //定义一条射线，这条射线从摄像机屏幕射向鼠标所在位置
            RaycastHit hit;    //声明一个碰撞的点
            Vector3 point = Vector3.zero;
            if (Physics.Raycast(ray, out hit))
            {
                for (int i = 0; i < _biologicalData.Length; i++)
                {
                    Debug.DrawLine(ray.origin, hit.point, Color.blue);
                    if (_biologicalData.Renderer[i].bounds.Contains(hit.point))
                    {
                        if (_tipsWindow == null)
                        {
                            _tipsWindow = (TipsWindow)UICenterMasterManager.Instance.ShowWindow(WindowID.TipsWindow);
                        }
                        _tipsWindow.SetTip(hit.point);
                        Debuger.Log("ZAI ");
                    }
                }

            }
        }
    }


}
