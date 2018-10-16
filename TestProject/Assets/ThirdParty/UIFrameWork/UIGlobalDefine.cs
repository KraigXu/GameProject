using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TinyFrameWork
{
    public enum WindowID
    {
        Invaild = 0,
        StrategyWindow,
        StrategyMessageWindow,
        StrategyTimeWindow,
        LivingAreaBasicWindow,
        LivingAreaMainWindow,
        LivingAreaTitleWindow,
        ExtendedMenuWindow,
        WXCharacterPanelWindow,
        WXCharacterRelationshipWindow,
        WXCharacterPrestigeWindow,
    }

    public enum UIWindowType
    {
        NormalLayer,    // 可推出界面(UIMainMenu,UIRank等)
        BackgroundLayer,     // 固定窗口(UITopBar等)
        ForegroundLayer,     // 模式窗口(UIMessageBox, yourPopWindow , yourTipsWindow ......)
    }

    /// <summary>
    /// 1. HideOther (close or hide other window when open HideOther window add to navigation sequence data)
    /// 2. NeedBack (open window don't close other window add to navigation sequence data)
    /// 3. NoNeedBack (open window close other window no need add to navigation sequence data)
    /// 4. DoNoting (no close other window and not communication with navigation)

    /// 8.22 Divide UIWindowShowMode to UIWindowShowMode and new UIWindowNavigationMode
    /// ShowMode control the window show mode
    /// NavigationMode control the navigation system
    /// </summary>
    public enum UIWindowShowMode
    {
        //DoNothing,     // Really do nothing
        //HideOther,     // 打开界面关闭其他界面
        //NeedBack,      // 打开界面不关闭其他界面
        //NoNeedBack,    // 打开界面关闭其他界面，不加入导航队列

        DoNothing = 0,
        HideOtherWindow,
        DestoryOtherWindow,
    }


    public enum UIWindowPlayAnimationModel
    {
        /// <summary>
        /// 拉伸
        /// </summary>
        Stretching,
        /// <summary>
        /// 左方向
        /// </summary>
        ToLeft,
        /// <summary>
        /// 右方向
        /// </summary>
        ToRight,
        /// <summary>
        /// 上方向
        /// </summary>
        ToUp,
        /// <summary>
        /// 下方向
        /// </summary>
        ToDown
    }

    /// <summary>
    /// 支持缩放平移淡入淡出效果
    /// 特殊效果选则用户自定制
    /// </summary>
    public enum UIWindowAnimationType
    {
        None,
        /// <summary>
        /// 自定制
        /// </summary>
        SelfCustomization,
        /// <summary>
        /// 缩放
        /// </summary>
        Scale,
        /// <summary>
        /// 平移
        /// </summary>
        Translation,
        /// <summary>
        /// 淡入淡出效果
        /// </summary>
        FadesOut
    }
    public enum UIWindowCloseModel
    {
        Destory,
        Hide
    }
    public enum UIWindowNavigationMode
    {
        IgnoreNavigation = 0,
        NormalNavigation,
    }

    public enum UIWindowColliderMode
    {
        None,      // No BgTexture and No Collider
        Normal,    // Collider with alpha 0.001 BgTexture
        WithBg,    // Collider with alpha 1 BgTexture
    }

    public class UIResourceDefine
    {
        // Define the UIWindow prefab paths
        // all window prefab placed in Resources folder
        // maybe your window assetbundle path
        public static Dictionary<WindowID, string> windowPrefabPath = new Dictionary<WindowID, string>()
        {
            {WindowID.Invaild,"UiPrefab/InvaildWindow"},
            {WindowID.StrategyWindow,"UiPrefab/StrategyWindow"},
            {WindowID.StrategyTimeWindow,"UiPrefab/StrategyTimeWindow"},
            {WindowID.LivingAreaBasicWindow,"UiPrefab/LivingArea/LivingAreaBasicWindow"},
            {WindowID.LivingAreaTitleWindow,"UiPrefab/LivingArea/LivingAreaTitleWindow"},
            {WindowID.LivingAreaMainWindow,"UiPrefab/LivingArea/LivingAreaMainWindow"},
            {WindowID.ExtendedMenuWindow,"UiPrefab/ExtendedMenuWindow"},
            {WindowID.WXCharacterPanelWindow,"UiPrefab/WXCharacterPanelWidow"},
            {WindowID.WXCharacterRelationshipWindow,"UiPrefab/WXCharacterRelationshipWindow"},
            {WindowID.WXCharacterPrestigeWindow,"UiPrefab/WXCharacterPrestigeWindow"},
        };
    }
}