using UnityEngine;
using System.Collections;

namespace CreativeSpore.RpgMapEditor
{
    /// <summary>
    /// This component will sort the gameobject by Z position in a range between the first ground layer found in the tilemap to the last Overlay layer found in the tilemap.
    /// </summary>
    [AddComponentMenu("RpgMapEditor/Controllers/IsoSpriteController", 10)]
	public class IsoSpriteController : MonoBehaviour {

		public SpriteRenderer m_spriteRender;

        private float m_OverlayLayerZ, m_GroundOverlayLayerZ;

        void Reset()
        {
            m_spriteRender = GetComponent<SpriteRenderer>();
        }

		void Start () 
        {
			if (m_spriteRender == null)
			{
				m_spriteRender = GetComponent<SpriteRenderer>();
			}

            OnMapLoaded(AutoTileMap.Instance); // call this now because map is loaded on Awake when scene is loaded, and this event is missed
            AutoTileMap.Instance.OnMapLoaded += OnMapLoaded;            
		}

        void OnMapLoaded(AutoTileMap autoTileMap)
        {
            m_OverlayLayerZ = autoTileMap.FindFirstLayer(eLayerType.Overlay).Depth;
            m_GroundOverlayLayerZ = autoTileMap.FindLastLayer(eLayerType.Ground).Depth;
        }
		
		void Update () 
		{
			Vector3 vPos = m_spriteRender.transform.position;
            float f0 = Mathf.Abs(m_OverlayLayerZ - m_GroundOverlayLayerZ);
            float f1 = Mathf.Abs(AutoTileMap.Instance.transform.position.y - transform.position.y) / (AutoTileMap.Instance.MapTileHeight * AutoTileMap.Instance.CellSize.y);
            vPos.z = m_GroundOverlayLayerZ - f0 * f1;
			m_spriteRender.transform.position = vPos;
		}
	}
}