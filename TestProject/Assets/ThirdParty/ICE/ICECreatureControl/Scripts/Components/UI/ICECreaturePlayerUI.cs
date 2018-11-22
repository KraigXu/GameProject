// ##############################################################################
//
// ICECreaturePlayerUI.cs
// Version 1.4.0
//
// Copyrights © Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.
// http://www.icecreaturecontrol.com
// mailto:support@icecreaturecontrol.com
// 
// Unity Asset Store End User License Agreement (EULA)
// http://unity3d.com/legal/as_terms
//
// ##############################################################################

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;

using ICE.Creatures;
using ICE.Creatures.Objects;
using ICE.Creatures.Utilities;

namespace ICE.Creatures.UI
{
	public class ICECreaturePlayerUI : ICEWorldEntityStatusUI {

		private ICECreaturePlayer m_Player = null;
		private ICECreaturePlayer Player{
			get{ return m_Player = ( m_Player == null ? GetComponent<ICECreaturePlayer>() : m_Player ); }
		}

		public Image DamageIndicator = null;
		public Image InventorySlotBar = null;

		public bool UseDisplayEntityInfos = true;
		public float DisplayUpdateInterval = 0.5f;
		private float m_DisplayUpdateTimer = 0;

		private Camera m_DisplayCamera = null;

		// Update is called once per frame
		public override void Update () {
		
			if( Player != null )
			{
				if( HealthBar != null )
				{
					HealthBar.fillAmount = Player.Status.DurabilityInPercent * 0.01f;
					HealthBar.color = HSBColor.ToColor( HSBColor.Lerp( Color.red, Color.green, HealthBar.fillAmount ) );
				}

				if( DamageIndicator != null )
				{
					DamageIndicator.color = new Color( DamageIndicator.color.r, DamageIndicator.color.g, DamageIndicator.color.b, Mathf.Clamp01( 1 - ( Player.Status.DurabilityInPercent * 0.01f ) ) );
				}

				if( InventorySlotBar != null )
				{
					InventorySlotObject _slot = Player.Inventory.GetSlotByIndex(0);

					if( _slot != null )
					{
						InventorySlotBar.fillAmount = _slot.AmountInPercent * 0.01f;
						InventorySlotBar.color = HSBColor.ToColor( HSBColor.Lerp( Color.red, Color.green, InventorySlotBar.fillAmount ) );
					}

				}
			}

			DisplayEntityInfos();
		}

		public void DisplayEntityInfos()
		{
			if( ! UseDisplayEntityInfos )
				return;

			m_DisplayUpdateTimer += Time.deltaTime;
			if( m_DisplayUpdateTimer < DisplayUpdateInterval )
				return;

			m_DisplayUpdateTimer = 0;

			if( m_DisplayCamera == null )
			{
				m_DisplayCamera = gameObject.GetComponentInChildren<Camera>();
				if( m_DisplayCamera == null )
					m_DisplayCamera = Camera.main;
			}

			if( m_DisplayCamera != null )
				DisplayEntityInfos( m_DisplayCamera.transform.position, m_DisplayCamera.transform.forward );
				
		}

		public void DisplayEntityInfos( Vector3 _origin, Vector3 _direction )
		{

			float _size = 3;
			// Ray cast to any object to getting an object info
			RaycastHit[] _hits = Physics.RaycastAll( _origin, _direction, _size );
			foreach( RaycastHit _hit in _hits )
			{
				if( _hit.collider != null )
				{
					_hit.collider.SendMessageUpwards( "DisplayInfos", DisplayUpdateInterval, SendMessageOptions.DontRequireReceiver );
				}
			}
		}
	}
}
