using System;
using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using TinyFrameWork;
using Unity.Entities;
using UnityEngine;

namespace WX
{
    public class BiologicalGroup
    {
        public int GroupId;
        public Biological LeaderId;
        public List<Biological> Partners = new List<Biological>();
    }
    public enum BiologicalModelType
    {
        HumanMen,
        HumanWoMen,
    }

    public enum LocationType
    {
        Field = 0,
        City = 1,
        Event = 3,
        InLivingArea =4,
        LivingAreaExit=5,
        LivingAreaEnter=6,
        LivingAreaIn=7
    }



    public class BiologicalSystem : ComponentSystem
    {
        struct BiologicalData
        {
            public readonly int Length;
            public ComponentDataArray<Biological> Biological;
        }
        [Inject] private BiologicalData _data;

        struct BiologicalGroup
        {
            public readonly int Length;
            public ComponentDataArray<Biological> Biological;
            public ComponentArray<CapsuleCollider> Renderer;
        }
        [Inject]
        private BiologicalGroup _biologicalData;

        private TipsWindow _tipsWindow;
        protected override void OnUpdate()
        {
            //Change Property
            for (int i = 0; i < _data.Length; i++)
            {
                var b = _data.Biological[i];

                b.Jing = Convert.ToInt16(b.Tizhi + (b.Wuxing * 0.3f) + (b.Lidao * 0.5f));
                b.Qi = Convert.ToInt16(b.Jingshen + (b.Tizhi * 0.5f) + (b.Wuxing * 0.5f));
                b.Shen = Convert.ToInt16(b.Wuxing + b.Lidao * 0.3);
                _data.Biological[i] = b;
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    //定义一条射线，这条射线从摄像机屏幕射向鼠标所在位置
            RaycastHit hit;    //声明一个碰撞的点
            bool flag = false;
            if (Physics.Raycast(ray, out hit))
            {
                for (int j = 0; j < _biologicalData.Length; j++)
                {
                    if (_biologicalData.Renderer[j].bounds.Contains(hit.point))
                    {
                        _tipsWindow.SetBiologicalTip(hit.point, _biologicalData.Biological[j].BiologicalId);
                        flag = true;
                        return;
                    }
                }

                if (flag == false)
                {
                    _tipsWindow.Hide();
                }
            }

            if (_tipsWindow == null)
            {
                _tipsWindow = (TipsWindow)UICenterMasterManager.Instance.ShowWindow(WindowID.TipsWindow);
            }
        }


    }

}

