using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Ui
{

    /// <summary>
    /// 城市标题窗口
    /// </summary>
    public class LivingAreaTitleWindow : UIWindowBase
    {
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
        }

        protected override void BeforeShowWindow(BaseWindowContextData contextData)
        {
            var value= SystemManager.Get<LivingAreaSystem>().LivingAreaBuildMap;

            foreach (var item in value)
            {
                
            }

        }
    }

}