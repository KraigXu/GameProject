using System;

namespace Verse.UIFrameWork
{
    /// <summary>
    /// 窗口动画
    /// </summary>
    interface IWindowAnimation
    {
        /// <summary>
        /// 显示动画
        /// </summary>
        void EnterAnimation(Action onComplete);

        /// <summary>
        /// 隐藏动画
        /// </summary>
        void QuitAnimation(Action onComplete);

        ///// <summary>
        ///// 重置动画
        ///// </summary>
        void ResetAnimation();
    }
}

