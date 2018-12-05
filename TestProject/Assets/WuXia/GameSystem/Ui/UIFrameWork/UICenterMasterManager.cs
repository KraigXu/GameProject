using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GameSystem.Ui
{
    /// <summary>
    /// UI Main Manager "the Master" most important Class
    ///         Control all the "Big Parent" window:UIRank,UIShop,UIGame,UIMainMenu and so on.
    ///         UIRankManager: control the rank window logic (UIRankDetail sub window)
    ///         May be UIShopManager:control the UIShopDetailWindow or UIShopSubTwoWindow
    /// 
    /// 枢纽中心，控制整个大界面的显示逻辑UIRank，UIMainMenu等
    ///         UIRank排行榜界面可能也会有自己的Manager用来管理Rank系统中自己的子界面，这些子界面不交给"老大"UICenterMasterManager管理
    ///         分而治之，不是中央集权
    /// </summary>
    public class UICenterMasterManager : UIManagerBase
    {
        // save the UIRoot
        [HideInInspector]
        public Transform UIRoot;
        [HideInInspector]
        public Canvas _Canvas;
        // NormalWindow node
        [System.NonSerialized]
        public RectTransform UINormalLayerRoot;
        // PopUpWindow node
        [System.NonSerialized]
        public RectTransform UIForegroundLayerRoot;
        // FixedWindow node
        [System.NonSerialized]
        public RectTransform UIBackgroundLayerRoot;
        [HideInInspector]
        public Camera _Camera;

        // Each Type window start Depth
        private const int fixedWindowDepth = 100;
        private const int popUpWindowDepth = 150;
        private const int normalWindowDepth = 2;

        // Atlas reference
        // Mask Atlas for sprite mask(Common Collider window background)
        //         public UIAtlas maskAtlas;
        // 
        private static UICenterMasterManager instance;
        public static UICenterMasterManager Instance
        {
            get { return instance; }
        }

        protected override void Awake()
        {
            base.Awake();
            instance = this;
            UIRoot = transform;
            _Canvas = UIRoot.GetComponent<Canvas>();
            _Camera = _Canvas.worldCamera;
            InitWindowManager();
            Debuger.Log("## UICenterMasterManager is call awake.");
        }

        public override UIWindowBase ShowWindow(WindowID id, ShowWindowData showData = null)
        {
            UIWindowBase baseWindow = ReadyToShowBaseWindow(id, showData);
            if (baseWindow != null)
            {
                RealShowWindow(baseWindow, id, showData);
            }
            else
            {
                baseWindow = GetGameWindow(id);
                if (baseWindow != null)
                {
                    BaseWindowContextData contextData = showData == null ? null : showData.contextData;
                    baseWindow.ReadyToShowWindow(contextData);
                }
            }
            return baseWindow;
        }

        protected override UIWindowBase ReadyToShowBaseWindow(WindowID id, ShowWindowData showData = null)
        {
            // Check the window control state
            if (!this.IsWindowInControl(id))
            {
                Debuger.Log("## UIManager has no control power of " + id.ToString());
                return null;
            }

            // If the window in shown list just return
            if (dicShownWindows.ContainsKey(id))
                return null;

            UIWindowBase baseWindow = GetGameWindow(id);

            // If window not in scene start Instantiate new window to scene
            bool newAdded = false;
            if (!baseWindow)
            {
                newAdded = true;
                if (UIResourceDefine.windowPrefabPath.ContainsKey(id))
                {
                    GameObject prefab = Resources.Load<GameObject>(UIResourceDefine.windowPrefabPath[id]);
                    if (prefab != null)
                    {
                        GameObject uiObject = (GameObject)GameObject.Instantiate(prefab);
                        //// NGUITools.SetActive(uiObject, true);
                        //// NOTE: You can add component to the window in the inspector
                        //// Or just AddComponent<UIxxxxWindow>() to the target
                        baseWindow = uiObject.GetComponent<UIWindowBase>();
                        if (baseWindow.ID != id)
                        {
                            Debuger.LogError(string.Format("<color=#E6E6FA>[BaseWindowId :{0} != shownWindowId :{1}]</color>", baseWindow.ID, id));
                            return null;
                        }
                        // Get the window target root parent
                        Transform targetRoot = GetTargetRoot(baseWindow.windowData.windowType);
                        Transform t = uiObject.transform;
                        t.SetParent(targetRoot.transform);
                        t.localPosition = Vector3.zero;
                        t.localRotation = Quaternion.identity;
                        t.localScale = Vector3.one;
                        uiObject.layer = targetRoot.gameObject.layer;
                        RectTransform _targetRectTransform = uiObject.GetComponent<RectTransform>();
                        if (_targetRectTransform.anchorMax == Vector2.one && _targetRectTransform.anchorMin == Vector2.zero)
                        {
                            //如果是拉伸就吧边角设置成0 
                            UGUIGraphControl.SetRectTransformOffset(uiObject, Vector2.zero, Vector2.zero);
                        }
                        UGUIGraphControl.SetRectTransformAnchoredPosition(uiObject, Vector2.zero);
                        //  baseWindow = UGUITools.AddChild(targetRoot.gameObject, prefab).GetComponent<UIWindowBase>();
                        if (baseWindow.ID != id)
                        {
                            Debuger.LogError(string.Format("<color=#E6E6FA>[BaseWindowId :{0} != shownWindowId :{1}]</color>", baseWindow.ID, id));
                            return null;
                        }
                        dicAllWindows[id] = baseWindow;
                        prefab = null;
                    }
                }
            }

            if (baseWindow == null)
                Debuger.LogError("[window instance is null.]" + id.ToString());

            // Call reset window when first load new window
            // Or get forceResetWindow param
            if (newAdded || (showData != null && showData.forceResetWindow))
                baseWindow.ResetWindow();

            if (showData == null || (showData != null && showData.executeNavLogic))
            {
                // refresh the navigation data
                ExecuteNavigationLogic(baseWindow, showData);
            }

            // Adjust the window depth
            AdjustBaseWindowDepth(baseWindow);

            // Add common background collider to window
            AddColliderBgForWindow(baseWindow);
            return baseWindow;
        }

        public override void HideWindow(WindowID id, Action onComplete = null)
        {
            CheckDirectlyHide(id, onComplete);
        }

        public override void InitWindowManager()
        {
            base.InitWindowManager();
            InitWindowControl();
            isNeedWaitHideOver = true;


            if (UIBackgroundLayerRoot == null)
            {
                GameObject BackgroundLayerRoot = new GameObject("UIBackgroundLayerRoot");
                UIBackgroundLayerRoot = BackgroundLayerRoot.gameObject.AddComponent<RectTransform>();
                UIBackgroundLayerRoot.anchorMax = new UnityEngine.Vector2(1f, 1f);
                UIBackgroundLayerRoot.anchorMin = new UnityEngine.Vector2(0f, 0f);
                UIBackgroundLayerRoot.offsetMax = new UnityEngine.Vector2(0, 0);
                UIBackgroundLayerRoot.offsetMin = new UnityEngine.Vector2(0, 0);
                UGUITools.AddChildToTarget(UIRoot, UIBackgroundLayerRoot);
                UGUITools.ChangeChildLayer(UIBackgroundLayerRoot, UIRoot.gameObject.layer);
            }
            if (UINormalLayerRoot == null)
            {
                GameObject NormalLayerRoot = new GameObject("UINormalWindowRoot");
                UINormalLayerRoot = NormalLayerRoot.gameObject.AddComponent<RectTransform>();
                UINormalLayerRoot.anchorMax = new UnityEngine.Vector2(1f, 1f);
                UINormalLayerRoot.anchorMin = new UnityEngine.Vector2(0f, 0f);
                UINormalLayerRoot.offsetMax = new UnityEngine.Vector2(0, 0);
                UINormalLayerRoot.offsetMin = new UnityEngine.Vector2(0, 0);
                UGUITools.AddChildToTarget(UIRoot, UINormalLayerRoot);
                UGUITools.ChangeChildLayer(UINormalLayerRoot, UIRoot.gameObject.layer);
            }
            if (UIForegroundLayerRoot == null)
            {
                GameObject ForegroundLayerRoot = new GameObject("UIForegroundLayerRoot");
                UIForegroundLayerRoot = ForegroundLayerRoot.gameObject.AddComponent<RectTransform>();
                UIForegroundLayerRoot.anchorMax = new UnityEngine.Vector2(1f, 1f);
                UIForegroundLayerRoot.anchorMin = new UnityEngine.Vector2(0f, 0f);
                UIForegroundLayerRoot.offsetMax = new UnityEngine.Vector2(0, 0);
                UIForegroundLayerRoot.offsetMin = new UnityEngine.Vector2(0, 0);
                UGUITools.AddChildToTarget(UIRoot, UIForegroundLayerRoot);
                UGUITools.ChangeChildLayer(UIForegroundLayerRoot, UIRoot.gameObject.layer);
            }
        }

        protected override void InitWindowControl()
        {
            managedWindowIds.Clear();
            foreach (KeyValuePair<WindowID, string> kp in UIResourceDefine.windowPrefabPath)
            {
                AddWindowInControl(kp.Key);
            }
        }


        /// <summary>
        /// Return logic 
        /// When return back navigation check current window's Return Logic
        /// If true just execute the return logic
        /// If false immediately enter the RealReturnWindow() logic
        /// </summary>
        public override bool PopNavigationWindow()
        {
            if (curNavigationWindow != null)
            {
                bool needReturn = curNavigationWindow.ExecuteReturnLogic();
                if (needReturn)
                    return false;
            }
            return RealPopNavigationWindow();
        }


        /// <summary>
        /// 清除指定root下所有窗口
        /// </summary>
        /// <param name="type"></param>
        public virtual void ClearAllWindowByWindowType(UIWindowType type)
        {
            switch (type)
            {
                case UIWindowType.BackgroundLayer:
                    {
                        for (int i = 0; i < UIBackgroundLayerRoot.transform.childCount; i++)
                        {
                            UIWindowBase chidWindow = UIBackgroundLayerRoot.transform.GetChild(i).GetComponent<UIWindowBase>();
                            DestroyWindow(chidWindow.ID, null);
                        }
                    }
                    break;
                case UIWindowType.ForegroundLayer:
                    {
                        for (int i = 0; i < UIForegroundLayerRoot.transform.childCount; i++)
                        {
                            UIWindowBase chidWindow = UIForegroundLayerRoot.transform.GetChild(i).GetComponent<UIWindowBase>();
                            DestroyWindow(chidWindow.ID, null);
                        }
                    }
                    break;
                case UIWindowType.NormalLayer:
                    {
                        for (int i = 0; i < UINormalLayerRoot.transform.childCount; i++)
                        {
                            UIWindowBase chidWindow = UINormalLayerRoot.transform.GetChild(i).GetComponent<UIWindowBase>();
                            DestroyWindow(chidWindow.ID, null);
                        }
                    }
                    break;
            }

        }

        /// <summary>
        /// 根据窗口类型清除对应panl下的窗口
        /// 根据UIWindowCloseModel 来确定是删除还是隐藏
        /// </summary>
        /// <param name="type"></param>
        public virtual void ClearAllWindowByWindowTypeAndCloseModel(UIWindowType type)
        {
            switch (type)
            {
                case UIWindowType.BackgroundLayer:
                    {
                        for (int i = 0; i < UIBackgroundLayerRoot.transform.childCount; i++)
                        {
                            UIWindowBase chidWindow = UIBackgroundLayerRoot.transform.GetChild(i).GetComponent<UIWindowBase>();
                            switch (chidWindow.windowData.closeModel)
                            {
                                case UIWindowCloseModel.Destory:
                                    DestroyWindow(chidWindow.ID, null);
                                    break;
                                case UIWindowCloseModel.Hide:
                                    HideWindow(chidWindow.ID);
                                    break;
                            }
                        }
                    }
                    break;
                case UIWindowType.ForegroundLayer:
                    {
                        for (int i = 0; i < UIForegroundLayerRoot.transform.childCount; i++)
                        {
                            UIWindowBase chidWindow = UIForegroundLayerRoot.transform.GetChild(i).GetComponent<UIWindowBase>();
                            switch (chidWindow.windowData.closeModel)
                            {
                                case UIWindowCloseModel.Destory:
                                    DestroyWindow(chidWindow.ID, null);
                                    break;
                                case UIWindowCloseModel.Hide:
                                    HideWindow(chidWindow.ID);
                                    break;
                            }
                        }
                    }
                    break;
                case UIWindowType.NormalLayer:
                    {
                        for (int i = 0; i < UINormalLayerRoot.transform.childCount; i++)
                        {
                            UIWindowBase chidWindow = UINormalLayerRoot.transform.GetChild(i).GetComponent<UIWindowBase>();
                            switch (chidWindow.windowData.closeModel)
                            {
                                case UIWindowCloseModel.Destory:
                                    DestroyWindow(chidWindow.ID, null);
                                    break;
                                case UIWindowCloseModel.Hide:
                                    HideWindow(chidWindow.ID);
                                    break;
                            }
                        }
                    }
                    break;
            }

        }
        /// <summary>
        /// 清除指定root下所有窗口
        /// </summary>
        /// <param name="type"></param>
        public virtual void HideAllWindowByWindowType(UIWindowType type)
        {
            switch (type)
            {
                case UIWindowType.BackgroundLayer:
                    {
                        for (int i = 0; i < UIBackgroundLayerRoot.transform.childCount; i++)
                        {
                            UIWindowBase chidWindow = UIBackgroundLayerRoot.transform.GetChild(i).GetComponent<UIWindowBase>();
                            HideWindow(chidWindow.ID);
                        }
                    }
                    break;
                case UIWindowType.ForegroundLayer:
                    {
                        for (int i = 0; i < UIForegroundLayerRoot.transform.childCount; i++)
                        {
                            UIWindowBase chidWindow = UIForegroundLayerRoot.transform.GetChild(i).GetComponent<UIWindowBase>();
                            HideWindow(chidWindow.ID);
                        }
                    }
                    break;
                case UIWindowType.NormalLayer:
                    {
                        for (int i = 0; i < UINormalLayerRoot.transform.childCount; i++)
                        {
                            UIWindowBase chidWindow = UINormalLayerRoot.transform.GetChild(i).GetComponent<UIWindowBase>();
                            HideWindow(chidWindow.ID);
                        }
                    }
                    break;
            }

        }


        /// <summary>
        /// 关闭窗口。如果有导航就返回上一个界面 
        /// 如果没有就直接根据窗口的设定关闭或者隐藏窗口
        /// </summary>
        /// <param name="wndId"></param>
        public void CloseWindow(WindowID wndId)
        {
            if (!IsWindowInControl(wndId))
            {
                Debuger.LogError("## Current UI Manager has no control power of " + wndId.ToString());
                return;
            }

            if (!dicShownWindows.ContainsKey(wndId))
                return;

            UIWindowBase window = dicShownWindows[wndId];
            if (this.backSequence.Count > 0)
            {
                NavigationData seqData = this.backSequence.Peek();
                if (seqData != null && seqData.CloseTargetWindow == window)
                {
                    PopNavigationWindow();
                    Debuger.Log("<color=magenta>## close window use PopNavigationWindow() ##</color>");
                    return;
                }

            }


            switch (dicShownWindows[wndId].windowData.closeModel)
            {
                case UIWindowCloseModel.Destory:
                    DestroyWindow(wndId, null);
                    break;
                case UIWindowCloseModel.Hide:
                    HideWindow(wndId);
                    break;
            }


            //  HideWindow(wndId);
            Debuger.Log("<color=magenta>## close window without PopNavigationWindow() ##</color>");
        }

        /// <summary>
        /// Calculate right depth with windowType
        /// </summary>
        /// <param name="baseWindow"></param>
        private void AdjustBaseWindowDepth(UIWindowBase baseWindow)
        {
            UIWindowType windowType = baseWindow.windowData.windowType;
            int needDepth = 1;
            if (windowType == UIWindowType.NormalLayer)
            {
                needDepth = Mathf.Clamp(UGUITools.GetMaxTargetDepth(UINormalLayerRoot.gameObject, false) + 1, normalWindowDepth, int.MaxValue);
                Debuger.Log(string.Format("<color=#E6E6FA>[UIWindowType.Normal] maxDepth is {0} , {1}.</color>", needDepth.ToString(), baseWindow.ID.ToString()));
            }
            else if (windowType == UIWindowType.ForegroundLayer)
            {
                needDepth = Mathf.Clamp(UGUITools.GetMaxTargetDepth(UIForegroundLayerRoot.gameObject) + 1, popUpWindowDepth, int.MaxValue);
                Debuger.Log(string.Format("<color=#E6E6FA>[UIWindowType.PopUp] maxDepth is {0} , {1}.</color>", needDepth.ToString(), baseWindow.ID.ToString()));
            }
            else if (windowType == UIWindowType.BackgroundLayer)
            {
                needDepth = Mathf.Clamp(UGUITools.GetMaxTargetDepth(UIBackgroundLayerRoot.gameObject) + 1, fixedWindowDepth, int.MaxValue);
                Debuger.Log(string.Format("<color=#E6E6FA>[UIWindowType.Fixed] maxDepth is {0} , {1}.</color>", needDepth.ToString(), baseWindow.ID.ToString()));
            }
            if (baseWindow.MinDepth != needDepth)
                UGUITools.SetTargetMinPanelDepth(baseWindow.gameObject, needDepth);

            // send window added message to game client
            if (baseWindow.windowData.windowType == UIWindowType.ForegroundLayer)
            {
                // trigger the window PopRoot added window event
                // EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent(EventId.PopRootWindowAdded);
                //暂时不需要事件系统
            }
            baseWindow.MinDepth = needDepth;
        }

        /// <summary>
        /// Add Collider and BgTexture for target window
        /// </summary>
        private void AddColliderBgForWindow(UIWindowBase baseWindow)
        {
            UIWindowColliderMode colliderMode = baseWindow.windowData.colliderMode;
            if (colliderMode == UIWindowColliderMode.None)
                return;
            GameObject bgObj = null;
            if (colliderMode == UIWindowColliderMode.Normal)
                bgObj = UGUITools.AddColliderBgToTarget(baseWindow.gameObject, "Mask02", true);
            if (colliderMode == UIWindowColliderMode.WithBg)
                bgObj = UGUITools.AddColliderBgToTarget(baseWindow.gameObject, "Mask02", false);
            baseWindow.OnAddColliderBg(bgObj);
        }

        private void ExecuteNavigationLogic(UIWindowBase baseWindow, ShowWindowData showData)
        {
            WindowCoreData windowData = baseWindow.windowData;
            if (baseWindow.RefreshBackSeqData)
            {
                RefreshBackSequenceData(baseWindow, showData);
            }
            else if (windowData.showMode == UIWindowShowMode.HideOtherWindow)
            {
                HideAllShownWindow();
            }

            // If target window is mark as force clear all the navigation sequence data
            // Show data need force clear the back seq data
            if (baseWindow.windowData.forceClearNavigation || (showData != null && showData.forceClearBackSeqData))
            {
                Debuger.Log("<color=#E6E6FA>## [Enter the start window, reset the backSequenceData for the navigation system.]##</color>");
                ClearBackSequence();
            }
            else
            {
                if ((showData != null && showData.checkNavigation))
                    CheckBackSequenceData(baseWindow);
            }

        }

        private void RefreshBackSequenceData(UIWindowBase targetWindow, ShowWindowData showData)
        {
            WindowCoreData coreData = targetWindow.windowData;

            if (dicShownWindows.Count == 0)
                return;

            List<WindowID> removedKey = null;
            List<UIWindowBase> sortedHiddenWindows = new List<UIWindowBase>();

            NavigationData backData = new NavigationData();
            foreach (KeyValuePair<WindowID, UIWindowBase> window in dicShownWindows)
            {
                if (coreData.showMode != UIWindowShowMode.DoNothing)
                {
                    if (window.Value.windowData.windowType == UIWindowType.BackgroundLayer)
                        continue;
                    if (removedKey == null)
                        removedKey = new List<WindowID>();
                    removedKey.Add(window.Key);
                    window.Value.HideWindowDirectly();
                }

                if (window.Value.windowData.windowType != UIWindowType.BackgroundLayer)
                    sortedHiddenWindows.Add(window.Value);
            }

            if (removedKey != null)
            {
                for (int i = 0; i < removedKey.Count; i++)
                    dicShownWindows.Remove(removedKey[i]);
            }

            // Push new navigation data
            if (coreData.navigationMode == UIWindowNavigationMode.NormalNavigation && (showData == null || (!showData.ignoreAddNavData)))
            {
                // Add to return show target list
                sortedHiddenWindows.Sort(this.compareWindowFun);
                List<WindowID> navHiddenWindows = new List<WindowID>();
                for (int i = 0; i < sortedHiddenWindows.Count; i++)
                {
                    WindowID pushWindowId = sortedHiddenWindows[i].ID;
                    navHiddenWindows.Add(pushWindowId);
                }
                backData.CloseTargetWindow = targetWindow;
                backData.backShowTargets = navHiddenWindows;
                backSequence.Push(backData);
                Debuger.Log("<color=#E6E6FA>### !!!Push new Navigation data!!! ###</color>");
            }
        }

        // 如果当前存在BackSequence数据
        // 1.栈顶界面不是当前要显示的界面需要清空BackSequence(导航被重置)
        // 2.栈顶界面是当前显示界面,如果类型为(NeedBack)则需要显示所有backShowTargets界面

        // 栈顶不是即将显示界面(导航序列被打断)
        // 如果当前导航队列顶部元素和当前显示的界面一致，表示和当前的导航数衔接上，后续导航直接使用导航数据
        // 不一致则表示，导航已经失效，下次点击返回按钮，我们直接根据window的preWindowId确定跳转到哪一个界面

        // 如果测试：进入到demo的 关卡详情，点击失败按钮，然后你可以选择从游戏中跳转到哪一个界面，查看导航输出信息
        // 可以知道是否破坏了导航数据

        // if the navigation stack top window not equals to current show window just clear the navigation stack
        // check whether the navigation is broken

        // Example:(we from mainmenu to uilevelwindow to uileveldetailwindow)
        // UILevelDetailWindow <- UILevelWindow <- UIMainMenu   (current navigation stack top element is UILevelDetailWindow)

        // click the GotoGame in UILevelDetailWindow to enter the real Game

        // 1. Exit game we want to enter UILevelDetailWindow(OK, the same as navigation stack top UILevelDetailWindow) so we not break the navigation
        // when we enter the UILevelDetailWindow our system will follow the navigation system

        // 2. Exit game we want to enter UISkillWindow(OK, not the same as navigation stack top UILevelDetailWindow)so we break the navigation
        // reset the navigation data 
        // when we click return Button in the UISkillWindow we will find UISkillWindow's preWindowId to navigation because our navigation data is empty
        // we should use preWindowId for navigating to next window

        // HOW to Test
        // when you in the MatchResultWindow , you need click the lose button choose to different window and check the ConsoleLog find something useful
        private void CheckBackSequenceData(UIWindowBase baseWindow)
        {
            if (baseWindow.RefreshBackSeqData)
            {
                if (backSequence.Count > 0)
                {
                    NavigationData backData = backSequence.Peek();
                    if (backData.CloseTargetWindow != null)
                    {
                        if (backData.CloseTargetWindow.ID != baseWindow.ID)
                        {
                            Debuger.Log("<color=#E6E6FA>## UICenterMasterManager : clear sequence data ##</color>");
                            Debuger.Log("## UICenterMasterManager : Hide target window and show window id is " + backData.CloseTargetWindow.ID + " != " + baseWindow.ID);
                            ClearBackSequence();
                        }
                    }
                    else
                        Debuger.LogError("Back data hide target window is null!");
                }
            }
        }

        public Transform GetTargetRoot(UIWindowType type)
        {
            if (type == UIWindowType.BackgroundLayer)
                return UIBackgroundLayerRoot;
            if (type == UIWindowType.NormalLayer)
                return UINormalLayerRoot;
            if (type == UIWindowType.ForegroundLayer)
                return UIForegroundLayerRoot;
            return UIRoot;
        }

        // Push target GameObject to top depth
        // Case: when you open multi PopWindow
        // You want one of these PopWindow stay at the Top 
        // You can register the EventSystemDefine.EventUIFrameWorkPopRootWindowAdded 
        // Call this method to push window to top
        public static void AdjustTargetWindowDepthToTop(UIWindowBase targetWindow)
        {
            if (targetWindow == null)
                return;

            Transform windowRoot = UICenterMasterManager.Instance.GetTargetRoot(targetWindow.windowData.windowType);
            int needDepth = Mathf.Clamp(UGUITools.GetMaxTargetDepth(windowRoot.gameObject, true) + 1, popUpWindowDepth, int.MaxValue);
            UGUITools.SetTargetMinPanelDepth(targetWindow.gameObject, needDepth);
            targetWindow.MinDepth = needDepth;
        }


        void Update()
        {
            if (UIBackgroundLayerRoot != null && UINormalLayerRoot != null && UIForegroundLayerRoot != null)
            {
                UIBackgroundLayerRoot.SetSiblingIndex(0);
                UINormalLayerRoot.SetSiblingIndex(1);
                UIForegroundLayerRoot.SetSiblingIndex(2);

                if (UIBackgroundLayerRoot.parent != UIRoot)
                {

                    UIBackgroundLayerRoot.SetParent(UIRoot);
                }
                if (UINormalLayerRoot.parent != UIRoot)
                {
                    UINormalLayerRoot.SetParent(UIRoot);
                }
                if (UIForegroundLayerRoot.parent != UIRoot)
                {
                    UIForegroundLayerRoot.SetParent(UIRoot);
                }


                if (UIBackgroundLayerRoot.anchorMax != Vector2.one || UIBackgroundLayerRoot.anchorMin != Vector2.zero || UIBackgroundLayerRoot.offsetMax != Vector2.zero || UIBackgroundLayerRoot.offsetMin != Vector2.zero ||
                    UINormalLayerRoot.anchorMax != Vector2.one || UINormalLayerRoot.anchorMin != Vector2.zero || UINormalLayerRoot.offsetMax != Vector2.zero || UINormalLayerRoot.offsetMin != Vector2.zero ||
                  UIForegroundLayerRoot.anchorMax != Vector2.one || UIForegroundLayerRoot.anchorMin != Vector2.zero || UIForegroundLayerRoot.offsetMax != Vector2.zero || UIForegroundLayerRoot.offsetMin != Vector2.zero)
                {
                    UIBackgroundLayerRoot.anchorMax = new UnityEngine.Vector2(1f, 1f);
                    UIBackgroundLayerRoot.anchorMin = new UnityEngine.Vector2(0f, 0f);
                    UIBackgroundLayerRoot.offsetMax = new UnityEngine.Vector2(0, 0);
                    UIBackgroundLayerRoot.offsetMin = new UnityEngine.Vector2(0, 0);

                    UINormalLayerRoot.anchorMax = new UnityEngine.Vector2(1f, 1f);
                    UINormalLayerRoot.anchorMin = new UnityEngine.Vector2(0f, 0f);
                    UINormalLayerRoot.offsetMax = new UnityEngine.Vector2(0, 0);
                    UINormalLayerRoot.offsetMin = new UnityEngine.Vector2(0, 0);

                    UIForegroundLayerRoot.anchorMax = new UnityEngine.Vector2(1f, 1f);
                    UIForegroundLayerRoot.anchorMin = new UnityEngine.Vector2(0f, 0f);
                    UIForegroundLayerRoot.offsetMax = new UnityEngine.Vector2(0, 0);
                    UIForegroundLayerRoot.offsetMin = new UnityEngine.Vector2(0, 0);
                }
            }
            Application.targetFrameRate = -1;

        }
    }
}