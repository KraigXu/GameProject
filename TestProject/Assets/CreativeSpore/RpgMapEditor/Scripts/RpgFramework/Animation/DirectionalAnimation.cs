using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CreativeSpore.RpgMapEditor
{
    //TODO: add AddComponentMenu to the other scripts
    [AddComponentMenu("RpgMapEditor/Animation/DirectionalAnimation", 10)]
    [DisallowMultipleComponent]
    public class DirectionalAnimation : MonoBehaviour 
    {
        [System.Flags]
        public enum ePlayMode
        {
            Normal,
            Reverse,
            PingPong,
            PingPong_Reverse
        }

        public delegate void OnAnimationLoopOverDelegate(DirectionalAnimation source);
        public OnAnimationLoopOverDelegate OnAnimationLoopOver;

        public SpriteRenderer TargetSpriteRenderer;
        public DirectionalAnimationController DirectionalAnimController { get { return m_dirAnimCtrl; } set { m_dirAnimCtrl = value; } }
        public eAnimDir AnimDirection { get { return m_dir; } set { m_dir = value; } }
        public ePlayMode PlayMode { get { return m_playMode; } set { m_playMode = value; } }
        public bool IsPingPongAnim { get { return (m_playMode & ePlayMode.PingPong) != 0; } }
        public bool IsReverse { get { return (m_playMode & ePlayMode.Reverse) != 0; } }
        public bool IsPlaying { get { return m_isPlaying; } set { m_isPlaying = value; } }
        public float AnimSpeed { get { return m_fps; } set { m_fps = value; } }
        public int AnimIndex { get { return m_animIdx; } set { m_animIdx = value; } }

        [SerializeField]
        private ePlayMode m_playMode = ePlayMode.Normal;
        [SerializeField]
        private bool m_isPlaying = true;
        [SerializeField]
        private float m_fps = 9f;
        [SerializeField]
        private int m_animIdx = 0;
        [SerializeField]
        private DirectionalAnimationController m_dirAnimCtrl;

        public int CurrentFrame
        {
            get
            {
                int animFrames = m_dirAnimCtrl.GetAnim(m_animIdx).FramesPerDir;
                int curFrame = IsReverse ? animFrames - 1 - m_internalFrame : m_internalFrame;
                if (IsPingPongAnim && m_isPingPongReverse)
                {
                    curFrame = animFrames - 1 - curFrame;
                }
                return curFrame;
            }
        }


        [SerializeField]
        private eAnimDir m_dir = eAnimDir.Down;

        private int m_internalFrame; // frame counter used internally. CurFrame would be the real animation frame
        private float m_curFrameTime;
        private bool m_isPingPongReverse = false;

        void Start()
        {
            if (TargetSpriteRenderer == null)
            {
                TargetSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }
        }

        void Reset()
        {
            TargetSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        void Update()
        {
            UpdateAnim(Time.deltaTime);
        }

        public void UpdateAnim(float dt)
        {
            if (TargetSpriteRenderer == null || m_dirAnimCtrl == null)
                return;
            
            m_animIdx = (m_animIdx + m_dirAnimCtrl.GetAnimList().Count) % m_dirAnimCtrl.GetAnimList().Count;
            if (IsPlaying)
            {
                DirectionalAnimData anim = m_dirAnimCtrl.GetAnim(m_animIdx);
                if (IsPingPongAnim && (m_internalFrame == 0 || m_internalFrame == (anim.FramesPerDir - 1)))
                    m_curFrameTime += dt * AnimSpeed * 2f; // avoid stay twice of the time in the first and last frame of the animation
                else
                    m_curFrameTime += dt * AnimSpeed;
                while (m_curFrameTime >= 1f)
                {
                    m_curFrameTime -= 1f;
                    ++m_internalFrame; m_internalFrame %= anim.FramesPerDir;
                    if (m_internalFrame == 0)
                    {
                        if (OnAnimationLoopOver != null) OnAnimationLoopOver(this);
                        m_isPingPongReverse = !m_isPingPongReverse;
                    }
                }
            }
            else
            {
                m_isPingPongReverse = false;
                m_internalFrame = 0;
            }

            TargetSpriteRenderer.sprite = GetCurrentSprite(AnimDirection);
        }

        public Sprite GetCurrentSprite(eAnimDir dir)
        {
            return m_dirAnimCtrl.GetAnim(m_animIdx).GetAnimFrame(dir, CurrentFrame);
        }

        public void SetAnim(string name)
        {
            int animIdx = m_dirAnimCtrl.GetAnimList().FindIndex(x => x.name == name);
            if (animIdx >= 0)
                m_animIdx = animIdx;
            else
                Debug.LogError("Animation " + name + " not found!");
        }

        public void SetAnimDirection(Vector2 vDir)
        {
            if (vDir.magnitude > Vector2.kEpsilon)
            {
                m_dir = m_dirAnimCtrl.GetAnimDirByVector(vDir);
            }
        }

        public Vector2 GetAnimDirection()
        {
            switch(m_dir)
            {
                case eAnimDir.Down: return new Vector2(0f, -1f);
                case eAnimDir.Up: return new Vector2(0f, 1f);
                case eAnimDir.Right: return new Vector2(1f, 0f);
                case eAnimDir.Left: return new Vector2(-1f, 0f);
                case eAnimDir.Down_Left: return new Vector2(-1, -1).normalized;
                case eAnimDir.Down_Right: return new Vector2(1, -1).normalized;
                case eAnimDir.Up_Left: return new Vector2(-1, 1).normalized;
                case eAnimDir.Up_Right: return new Vector2(1, 1).normalized;
            }
            return Vector2.zero;
        }
    }
}