using UnityEngine;
using System;
using System.Collections.Generic;

namespace Verse.UIFrameWork
{
    /// <summary>
    /// Base window
    /// </summary>
    public abstract class UIWindowBase : MonoBehaviour, IWindowAnimation
    {
        // protected UIPanel originPanel;

        // Lock state
        // When your window is in loading state
        // You can check IsLock to enable some button click method and so on
        // or you Add a very high depth collider instead
        private bool isLock = false;
        // in showing
        protected bool isShown = false;
        // Current windowID
        private WindowID windowID = WindowID.Invaild;

        // if there is no BackSequece Data just check the preWindowID
        // Try open preWindowID
        protected WindowID preWindowID = WindowID.Invaild;
        // Core window data must be init before open the Window
        public WindowCoreData windowData = new WindowCoreData();

        // Return Logic when leaving current window
        private event BoolDelegate returnPreLogic = null;

        protected RectTransform _RectTransform;
       // protected Tweener _CurentPlayAnimation = null;


        public bool IsLock
        {
            get { return isLock; }
            set { isLock = value; }
        }

        private int minDepth = 1;
        public int MinDepth
        {
            get { return minDepth; }
            set { minDepth = value; }
        }

        public WindowID ID
        {
            get
            {
                if (this.windowID == WindowID.Invaild)
                    Debug.LogError("window id is " + WindowID.Invaild);
                return windowID;
            }
            protected set { windowID = value; }
        }

        public WindowID PreWindowID
        {
            get { return preWindowID; }
            private set { preWindowID = value; }
        }

        // Need to added to back seq data
        public bool CanAddedToBackSeq
        {
            get { return this.windowData.navigationMode == UIWindowNavigationMode.NormalNavigation; }
        }

        // Need Refresh the back seq data
        public bool RefreshBackSeqData
        {
            get { return this.windowData.navigationMode == UIWindowNavigationMode.NormalNavigation; }
        }

        // Set the window Id use 
        protected abstract void InitWindowData();

        /// <summary>
        /// Called on Awake() used for window data Init
        /// </summary>
        public abstract void InitWindowOnAwake();

        /// <summary>
        /// Get the current window's manager
        /// </summary>
        public UIManagerBase GetWindowManager
        {
            get
            {
                UIManagerBase baseManager = this.gameObject.GetComponent<UIManagerBase>();
                return baseManager;
            }
        }

        protected virtual void Awake()
        {
            this.gameObject.SetActive(true);
            _RectTransform = transform as RectTransform;
            InitWindowData();
            InitWindowOnAwake();
            
        }

        /// <summary>
        /// Reset the window
        /// </summary>
        public virtual void ResetWindow()
        {
        }

        protected virtual void BeforeShowWindow(BaseWindowContextData contextData = null)
        {
            this.gameObject.SetActive(true);
        }

        /// <summary>
        /// 有动画的窗口显示窗口分为3步
        /// 1.显示窗口之前的初始化
        /// 2.准备好显示窗口
        /// 3.显示完成
        /// 无动画的窗口 没有区别。
        /// </summary>
        /// <param name="contextData"></param>
        public void ReadyToShowWindow(BaseWindowContextData contextData = null)
        {
            ///算法有问题 
            //List<Tween> playinglist = DOTween.TweensByTarget(_RectTransform);
            //if (playinglist != null && playinglist[playinglist.Count - 1].IsPlaying())
            //{
            //    Debug.Log(ID.ToString() + playinglist.Count + "正在执行");
            //    playinglist[playinglist.Count - 1].OnStepComplete(() =>
            //      {
            //          Debug.Log(ID.ToString() + "上次动画执行完成正在执行新的动画");
            //          //优先执行打开动画。
            //          //但是每次回调完成之后会把这个回调制空。否则会每次都调用
            //          RealShow(contextData);
            //          playinglist[playinglist.Count - 1].OnStepComplete(() => { });
            //      });
            //}
            //else
            //{
            //    RealShow(contextData);
            //}

            RealShow(contextData);
        }




        private void RealShow(BaseWindowContextData contextData)
        {
            BeforeShowWindow(contextData);
            if (windowData.animationType != UIWindowAnimationType.None)
            {
                //if (_CurentPlayAnimation == null)
                //{
                //    EnterAnimation(() =>
                //    {
                      
                //        ShowWindow(contextData);
                //    });
                //}
                //else
                //{
                //    _CurentPlayAnimation.PlayForward();
                //    _CurentPlayAnimation.OnComplete(() =>
                //    {
                //        //Debug.Log("第2次调用完成");
                //        ShowWindow(contextData);
                //    });
                //}
            }
            else
            {
                ShowWindow(contextData);
            }
        }
        public virtual void ShowWindow(BaseWindowContextData contextData)
        {
            isShown = true;
            isLock = false;
        }

        /// <summary>
        /// 关闭窗口之前要调用此接口
        /// 
        /// </summary>
        /// <param name="action"></param>
        public void ReadyToCloseWindow(Action action = null)
        {
            ////算法有问题 临时去掉
            //             List<Tween> Playinglist = DOTween.TweensByTarget(_RectTransform, true);
            //             Debug.Log(Playinglist);
            //             if (Playinglist != null)
            //             {
            //                 Debug.Log("关闭之前有个开始");
            //                 //List<Tween> temptween = DOTween.TweensByTarget(_RectTransform);
            //                 Playinglist[Playinglist.Count - 1].OnStepComplete(() =>
            //                 {
            //                     Debug.Log("关闭开始");
            //                     RealCloseWindow(action);
            //                     Playinglist[Playinglist.Count - 1].OnStepComplete(() => { });
            //                     // Playinglist[Playinglist.Count - 1].OnStepComplete(null);
            //                 });
            // 
            //             }
            //             else
            //             {
            //                 RealCloseWindow(action);
            //             }

            RealCloseWindow(action);

        }
        private void RealCloseWindow(Action action)
        {
            if (windowData.animationType != UIWindowAnimationType.None)
            {
                QuitAnimation(action);
            }
            else
            {
                HideWindow(action);
            }
        }

        /// <summary>
        /// 窗口被关闭时产生调用。
        /// 
        /// </summary>
        protected virtual void CloseWindow()
        {

        }
        protected void HideWindow(Action action = null)
        {
            IsLock = false;
            isShown = false;
            this.gameObject.SetActive(false);
            if (action != null)
                action();
            CloseWindow();
        }
        public void HideWindowDirectly()
        {
            ReadyToCloseWindow(null);
        }

        public virtual void DestroyWindow()
        {
            BeforeDestroyWindow();
            ReadyToCloseWindow(() =>
            {
                GameObject.Destroy(this.gameObject);

            });


        }

        protected virtual void BeforeDestroyWindow()
        {
        }

        // On Add Collider bg to window
        // Add collider bg click event
        public virtual void OnAddColliderBg(GameObject obj)
        {

        }

        /// <summary>
        /// Register call back method before the window returned(closed)
        /// Case: when you exit a window to pop up a confirm MessageBox
        /// </summary>
        protected void RegisterReturnLogic(BoolDelegate newLogic)
        {
            returnPreLogic = newLogic;
        }

        public bool ExecuteReturnLogic()
        {
            if (returnPreLogic == null)
                return false;
            else
                return returnPreLogic();
        }



        /// <summary>
        /// 窗口动画进入
        /// </summary>
        /// <param name="onComplete"></param>
        public virtual void EnterAnimation(Action onComplete)
        {
            if (windowData.animationType == UIWindowAnimationType.None)
                return;
            switch (windowData.animationType)
            {
                case UIWindowAnimationType.FadesOut:
                    {
                        CanvasGroup cg = _RectTransform.gameObject.GetComponent<CanvasGroup>();
                        if (cg == null)
                            cg = _RectTransform.gameObject.AddComponent<CanvasGroup>();
                        cg.alpha = 0;
                        //_CurentPlayAnimation = cg.DOFade(1, windowData.animationDuration).SetAutoKill(false);
                        //_CurentPlayAnimation.OnComplete(() =>
                        //{
                        //    if (onComplete != null)
                        //        onComplete();
                        //});
                    }
                    break;
                case UIWindowAnimationType.Scale:
                    {
                        switch (windowData.playAnimationModel)
                        {
                            case UIWindowPlayAnimationModel.ToLeft:
                                {
                                    _RectTransform.pivot = new Vector2(0f, _RectTransform.pivot.y);
                                    _RectTransform.localScale = new Vector3(0, 1, 1);
                                    //_CurentPlayAnimation = _RectTransform.DOScaleX(1, windowData.animationDuration).SetAutoKill(false);
                                    //_CurentPlayAnimation.OnComplete(() =>
                                    //{
                                    //    if (onComplete != null)
                                    //        onComplete();
                                    //});
                                }
                                break;
                            case UIWindowPlayAnimationModel.ToRight:
                                {
                                    _RectTransform.pivot = new Vector2(1f, _RectTransform.pivot.y);
                                    _RectTransform.localScale = new Vector3(0, 1, 1);
                                    //_CurentPlayAnimation = _RectTransform.DOScaleX(1, windowData.animationDuration).SetAutoKill(false);
                                    //_CurentPlayAnimation.OnComplete(() =>
                                    //{
                                    //    if (onComplete != null)
                                    //        onComplete();
                                    //});
                                }
                                break;
                            case UIWindowPlayAnimationModel.ToUp:
                                {
                                    _RectTransform.pivot = new Vector2(_RectTransform.pivot.x, 0f);
                                    _RectTransform.localScale = new Vector3(1, 0, 1);
                                    //_CurentPlayAnimation = _RectTransform.DOScaleY(1, windowData.animationDuration).SetAutoKill(false);
                                    //_CurentPlayAnimation.OnComplete(() =>
                                    //{
                                    //    if (onComplete != null)
                                    //        onComplete();
                                    //});
                                }
                                break;
                            case UIWindowPlayAnimationModel.ToDown:
                                {
                                    _RectTransform.pivot = new Vector2(_RectTransform.pivot.x, 1f);
                                    _RectTransform.localScale = new Vector3(1, 0, 1);
                                    //_CurentPlayAnimation = _RectTransform.DOScaleY(1, windowData.animationDuration).SetAutoKill(false);
                                    //_CurentPlayAnimation.OnComplete(() =>
                                    //{
                                    //    if (onComplete != null)
                                    //        onComplete();
                                    //});
                                }
                                break;
                            case UIWindowPlayAnimationModel.Stretching:
                                {
                                    _RectTransform.pivot = new Vector2(0.5f, 0.5f);
                                    _RectTransform.localScale = new Vector3(0, 0, 1);
                                    //_CurentPlayAnimation = _RectTransform.DOScale(1, windowData.animationDuration).SetAutoKill(false);
                                    //_CurentPlayAnimation.OnComplete(() =>
                                    //{
                                    //    if (onComplete != null)
                                    //        onComplete();
                                    //});
                                }
                                break;
                        }
                    }
                    break;
                case UIWindowAnimationType.Translation:
                    {
                        switch (windowData.playAnimationModel)
                        {
                            case UIWindowPlayAnimationModel.ToLeft:
                                {
                                    _RectTransform.anchoredPosition = new Vector2(_RectTransform.anchoredPosition.x + _RectTransform.rect.width, 0);
                                    //         _CurentPlayAnimation = _RectTransform.DOAnchorPosX(0, windowData.animationDuration).SetAutoKill(false);
                                    //_CurentPlayAnimation.OnComplete(() =>
                                    //{
                                    //    if (onComplete != null)
                                    //        onComplete();
                                    //});
                                }
                                break;
                            case UIWindowPlayAnimationModel.ToRight:
                                {
                                    _RectTransform.anchoredPosition = new Vector2(_RectTransform.anchoredPosition.x - _RectTransform.rect.width, 0);
                                    //     _CurentPlayAnimation = _RectTransform.DOAnchorPosX(0, windowData.animationDuration).SetAutoKill(false);
                                    //_CurentPlayAnimation.OnComplete(() =>
                                    //{
                                    //    if (onComplete != null)
                                    //        onComplete();
                                    //});
                                }
                                break;
                            case UIWindowPlayAnimationModel.ToUp:
                                {
                                    _RectTransform.anchoredPosition = new Vector2(0, _RectTransform.anchoredPosition.y - _RectTransform.rect.height);
                                    //    _CurentPlayAnimation = _RectTransform.DOAnchorPosY(0, windowData.animationDuration).SetAutoKill(false);
                                    //_CurentPlayAnimation.OnComplete(() =>
                                    //{
                                    //    if (onComplete != null)
                                    //        onComplete();
                                    //});
                                }
                                break;
                            case UIWindowPlayAnimationModel.ToDown:
                                {
                                    _RectTransform.anchoredPosition = new Vector2(0, _RectTransform.anchoredPosition.y + _RectTransform.rect.height);
                                    //     _CurentPlayAnimation = _RectTransform.DOAnchorPosY(0, windowData.animationDuration).SetAutoKill(false);
                                    //_CurentPlayAnimation.OnComplete(() =>
                                    //{
                                    //    if (onComplete != null)
                                    //        onComplete();
                                    //});
                                }
                                break;
                        }

                    }
                    break;
            }
        }



        /// <summary>
        /// 窗口离开时的动画
        /// </summary>
        /// <param name="onComplete"></param>
        public virtual void QuitAnimation(Action onComplete)
        {
            //if (_CurentPlayAnimation != null)
            //{
            //    _CurentPlayAnimation.PlayBackwards();
            //    _CurentPlayAnimation.OnRewind(() => { HideWindow(onComplete); });
            //}
            //else
            //{
            //    HideWindow(onComplete);
            //}
        }

        public void ResetAnimation()
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// Define the Window core data
    /// 1. windowType
    /// 2. showMode
    /// 3. colliderMode
    /// !!You must init the window's core data in the InitWindowData !!
    /// </summary>
    public class WindowCoreData
    {
        // If target window is mark as [forceClearNavigation] force clear all the navigation sequence data
        // Your start Game MainMenu always the force clear navigation
        public bool forceClearNavigation = false;
        public UIWindowType windowType = UIWindowType.NormalLayer;
        public UIWindowShowMode showMode = UIWindowShowMode.DoNothing;
        public UIWindowColliderMode colliderMode = UIWindowColliderMode.None;
        public UIWindowNavigationMode navigationMode = UIWindowNavigationMode.IgnoreNavigation;
        public UIWindowCloseModel closeModel = UIWindowCloseModel.Hide;
        public UIWindowAnimationType animationType = UIWindowAnimationType.None;
        public UIWindowPlayAnimationModel playAnimationModel = UIWindowPlayAnimationModel.ToUp;
        public float animationDuration = 0.2f;
    }

    public class NavigationData
    {
        public UIWindowBase CloseTargetWindow;
        public List<WindowID> backShowTargets;
    }
    public delegate bool BoolDelegate();
}
