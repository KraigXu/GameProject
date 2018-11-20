using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CreativeSpore.RpgMapEditor
{
    public enum eDirectionalAnimType
    {
        Single = 0,
        Two,
        Four,
        Eight,
    }

    public enum eAnimDir
    {
        Down = 0,
        Down_Right = 1,
        Right = 2,
        Up_Right = 3,
        Up = 4,
        Up_Left = 5,
        Left = 6,
        Down_Left = 7
    }    

    public class DirectionalAnimationController : ScriptableObject
    {        
        public int FramesPerAnim { get { return m_fpa; } }
        public int DirectionsPerAnim { get { return 1 << (int)m_dirType; } }
        public eDirectionalAnimType DirectionType { get { return m_dirType; } }
        public eAnimDir[] DirectionMapping { get { return m_dirMapping; } }

        public SpriteAlignment SpriteAlignment { get { return m_spriteAlignment; } }
        public Vector2 CustomPivot { get { return m_pivot; } }

        [SerializeField]
        SpriteAlignment m_spriteAlignment = SpriteAlignment.BottomCenter;
        [SerializeField]
        Vector2 m_pivot = Vector2.zero;
        [SerializeField]
        // 0, 
        eAnimDir[] m_dirMapping = new eAnimDir[] { eAnimDir.Down, eAnimDir.Left, eAnimDir.Right, eAnimDir.Up, eAnimDir.Down_Left, eAnimDir.Down_Right, eAnimDir.Up_Left, eAnimDir.Up_Right };
        [SerializeField]
        private float m_fps = 6; // frames per second
        [SerializeField]
        private int m_fpa = 3; // frames per animation
        [SerializeField]
        eDirectionalAnimType m_dirType = eDirectionalAnimType.Four;
        [SerializeField]
        List<DirectionalAnimData> m_anims = new List<DirectionalAnimData>();

        public DirectionalAnimData GetAnim(int animIdx)
        {
            if(animIdx >= 0 && animIdx < m_anims.Count)
            {
                return m_anims[animIdx];
            }
            return null;
        }

        public List<DirectionalAnimData> GetAnimList()
        {
            return m_anims;
        }

        public eAnimDir GetAnimDirByVector(Vector2 vDir)
        {
            float angle360 = Vector3.Cross(-Vector2.up, vDir).z >= 0f ? Vector2.Angle(-Vector2.up, vDir) : 360f - Vector2.Angle(-Vector2.up, vDir);
            int dirIdx = Mathf.RoundToInt((angle360 * DirectionsPerAnim) / 360f) % DirectionsPerAnim;
            return (eAnimDir)(dirIdx << (int)(eDirectionalAnimType.Eight - m_dirType));
        }

        public DirectionalAnimData AddAnim( DirectionalAnimData animToClone = null )
        {
            DirectionalAnimData newAnim = animToClone != null ? new DirectionalAnimData(animToClone) : new DirectionalAnimData(DirectionsPerAnim, m_fpa);
            m_anims.Add(newAnim);
            return newAnim;
        }

        public Sprite GetPreviewAnimSprite(Vector2 vDir, int animIdx)
        {
            int frameIdx = (int)(Time.realtimeSinceStartup * m_fps) % m_fpa;
            animIdx = Mathf.Clamp(animIdx, -1, m_anims.Count);
            if (animIdx >= 0)
            {
                eAnimDir dir = GetAnimDirByVector(vDir);
                return m_anims[animIdx].GetAnimFrame(dir, frameIdx);
            }
            else
            {
                return null;
            }
        }

        public Sprite GetAnimFrame(int animIdx, eAnimDir dir, int frame)
        {
            if(animIdx >= 0 && animIdx < m_anims.Count)
            {
                return m_anims[animIdx].GetAnimFrame(dir, frame);
            }
            return null;
        }

        public void CleanInvalidAnims()
        {
            List<DirectionalAnimData> validAnims = new List<DirectionalAnimData>();
            foreach(DirectionalAnimData anim in m_anims)
            {
                if(anim.CheckIntegrity())
                {
                    validAnims.Add(anim);
                }
            }
            m_anims = validAnims;
        }
    }

    [System.Serializable]
    public class DirectionalAnimData
    {
        public string name = "new animation";
        public Sprite[] animSprites;
        public int DirCount { get { return m_dirNb; } }
        public int FramesPerDir { get { return m_frameNb; } }

        [SerializeField]
        private int m_dirNb;
        [SerializeField]
        private int m_frameNb;

        public DirectionalAnimData(int dirNb, int frameNb)
        {
            m_dirNb = dirNb;
            m_frameNb = frameNb;
            animSprites = new Sprite[m_dirNb * m_frameNb];
        }

        public DirectionalAnimData(DirectionalAnimData anim)
        {
            m_dirNb = anim.m_dirNb;
            m_frameNb = anim.m_frameNb;
            animSprites = new Sprite[m_dirNb * m_frameNb];
            System.Array.Copy(anim.animSprites, animSprites, animSprites.Length);
        }

        public Sprite GetAnimFrame(int dirIdx, int frame)
        {
            int idx = dirIdx * m_frameNb + frame;
            return idx >= 0 && idx < animSprites.Length ? animSprites[idx] : null;
        }

        public Sprite GetAnimFrame(eAnimDir dir, int frame)
        {
            int dirIdx = (int)dir * m_dirNb / 8; // 8 maximum number of directions
            if(m_dirNb == 4)
            {
                switch(dir)
                {
                    case eAnimDir.Right:
                        dirIdx = 1; break;
                    case eAnimDir.Left:
                        dirIdx = 3; break;
                    case eAnimDir.Down:
                    case eAnimDir.Down_Left:
                    case eAnimDir.Down_Right:
                        dirIdx = 0; break;
                    case eAnimDir.Up:
                    case eAnimDir.Up_Left:
                    case eAnimDir.Up_Right:
                        dirIdx = 2; break;
                }
            }

            int idx = dirIdx * m_frameNb + frame;
            return idx >= 0 && idx < animSprites.Length ? animSprites[idx] : null;
        }

        public void SetAnimFrame(int dir, int frame, Sprite sprite)
        {
            animSprites[dir * m_frameNb + frame] = sprite;
        }

        public bool CheckIntegrity()
        {
            return animSprites != null && animSprites.Length == m_dirNb * m_frameNb;
        }
    }
}