using UnityEngine;
using System.Collections;
using CreativeSpore;

namespace CreativeSpore.RpgMapEditor
{

    [AddComponentMenu("RpgMapEditor/Behaviours/TileMovementBehaviour", 10)]
    public class TileMovementBehaviour : MonoBehaviour
    {
        public enum eState
        {
            Idle,
            Moving,
        }

        public enum eTrigger
        {
            MoveLeft,
            MoveRight,
            MoveUp,
            MoveDown,
        }

        public eState PlayerState = eState.Idle;
        public AutoTile TargetTile = null;
        public int FogSightLength = 5;

        private AutoTile m_CurTile = null;
        private float m_fAxisX = 0;
        private float m_fAxisY = 0;

        protected DirectionalAnimation m_animCtrl;
        protected PhysicCharBehaviour m_phyChar;
        void Start()
        {
            m_animCtrl = GetComponent<DirectionalAnimation>();
            m_phyChar = GetComponent<PhysicCharBehaviour>();
            TargetTile = RpgMapHelper.GetAutoTileByPosition(transform.position, 0);
        }

        void Update()
        {               
            m_CurTile = RpgMapHelper.GetAutoTileByPosition(transform.position, 0);
            if (TargetTile == null) TargetTile = m_CurTile;

            _DoStateLogic();
        }

        public void SetState(eState newState)
        {
            if (PlayerState != newState)
            {
                PlayerState = newState;
            }
        }

        public void ActivateTrigger(eTrigger trigger)
        {
            if (m_CurTile == null) return;

            Vector2 targetMapPos = new Vector2(m_CurTile.TileX, m_CurTile.TileY);

            switch (trigger)
            {
                case eTrigger.MoveDown:
                    targetMapPos.y += 1;
                    break;
                case eTrigger.MoveLeft:
                    targetMapPos.x -= 1;
                    break;
                case eTrigger.MoveRight:
                    targetMapPos.x += 1;
                    break;
                case eTrigger.MoveUp:
                    targetMapPos.y -= 1;
                    break;
            }

            if (PlayerState == eState.Idle && (targetMapPos.x != m_CurTile.TileX || targetMapPos.y != m_CurTile.TileY))
            {
                if (AutoTileMap.Instance.GetCellAutotileCollision((int)targetMapPos.x, (int)targetMapPos.y) == eTileCollisionType.PASSABLE)
                {
                    Vector3 vTargetPos = RpgMapHelper.GetTileCenterPosition((int)targetMapPos.x, (int)targetMapPos.y);
                    TargetTile = RpgMapHelper.GetAutoTileByPosition(vTargetPos, 0);
                    m_fAxisX = TargetTile.TileX - m_CurTile.TileX; m_fAxisX = m_fAxisX != 0 ? Mathf.Sign(m_fAxisX) : 0;
                    m_fAxisY = TargetTile.TileY - m_CurTile.TileY; m_fAxisY = m_fAxisY != 0 ? Mathf.Sign(m_fAxisY) : 0;

                    SetState(eState.Moving);
                }
            }
        }

        private void _DoStateLogic()
        {
            switch (PlayerState)
            {
                case eState.Idle:
                    UpdateMovement(0, 0);
                    transform.position = RpgMapHelper.GetTileCenterPosition(m_CurTile.TileX, m_CurTile.TileY) + new Vector3(0, 0, transform.position.z);
                    break;

                case eState.Moving:
                    UpdateMovement(m_fAxisX, -m_fAxisY);
                    Vector3 vCheckPos = transform.position - new Vector3((AutoTileMap.Instance.CellSize.x / 2) * m_fAxisX, -(AutoTileMap.Instance.CellSize.y / 2) * m_fAxisY);
                    AutoTile checkTile = RpgMapHelper.GetAutoTileByPosition(vCheckPos, 0);
                    if (checkTile == TargetTile)
                    {
                        transform.position = RpgMapHelper.GetTileCenterPosition(TargetTile.TileX, TargetTile.TileY) + new Vector3(0, 0, transform.position.z);
                        SetState(eState.Idle);
                    }
                    break;
            }
        }

        void UpdateMovement(float fAxisX, float fAxisY)
        {
            m_phyChar.Dir = new Vector3(fAxisX, fAxisY, 0);
            m_animCtrl.IsPlaying = m_phyChar.IsMoving;
            m_animCtrl.SetAnimDirection(m_phyChar.Dir);
        }
    }
}