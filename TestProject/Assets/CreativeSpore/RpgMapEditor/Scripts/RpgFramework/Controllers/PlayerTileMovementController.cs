using UnityEngine;
using System.Collections;

namespace CreativeSpore.RpgMapEditor
{
    [AddComponentMenu("RpgMapEditor/Controllers/PlayerTileMovementController", 10)]
    [RequireComponent(typeof(TileMovementBehaviour))]
    public class PlayerTileMovementController : PlayerController
    {
        private TileMovementBehaviour m_tileMovementBehaviour;
        protected override void Start()
        {
            base.Start();            
            m_tileMovementBehaviour = GetComponent<TileMovementBehaviour>();
        }

        protected override void Update()
        {
            Vector3 savedDir = m_phyChar.Dir;
            base.Update();
            m_phyChar.Dir = savedDir;

            m_tileMovementBehaviour.enabled = (Vehicle == null);
            if (m_tileMovementBehaviour.enabled)
            {
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    m_tileMovementBehaviour.ActivateTrigger(TileMovementBehaviour.eTrigger.MoveRight);
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    m_tileMovementBehaviour.ActivateTrigger(TileMovementBehaviour.eTrigger.MoveLeft);
                }
                else if (Input.GetKey(KeyCode.UpArrow))
                {
                    m_tileMovementBehaviour.ActivateTrigger(TileMovementBehaviour.eTrigger.MoveUp);
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    m_tileMovementBehaviour.ActivateTrigger(TileMovementBehaviour.eTrigger.MoveDown);
                }
            }
        }             
    }
}
