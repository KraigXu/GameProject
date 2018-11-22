// ##############################################################################
//
// ICECreaturePlayer.cs
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
using System.Collections.Generic;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;


namespace ICE.Creatures{

	public class ICECreaturePlayer : ICECreatureOrganism {

		/// <summary>
		/// Gets the entity classification type of the object
		/// </summary>
		/// <value>The classification type of the entity.</value>
		/// <description>The EntityClassType will be used to quickly 
		/// identify the correct class type of a derived entity object 
		/// without casts
		/// </description>
		public override EntityClassType EntityType{
			get{ return EntityClassType.Player; }
		}




		/// <summary>
		/// OnRegisterPublicMethods is called whithin the GetPublicMethods() method to update the m_PublicMethods list.
		/// Override this event to register your own methods by 
		/// using the RegisterPublicMethod(); while doing so you can use base.OnRegisterPublicMethods(); to call the event in
		/// the base classes too.
		/// </summary>
		protected override void OnRegisterBehaviourEvents()
		{
			base.OnRegisterBehaviourEvents();
			RegisterBehaviourEvent( "Suicide");
			RegisterBehaviourEvent( "PickupItem" );
			RegisterBehaviourEvent( "DropItem", BehaviourEventParameterType.Integer );
		}

		[SerializeField]
		private PlayerObject m_Player = null;
		public PlayerObject Player{
			get{ return m_Player = ( m_Player == null ? new PlayerObject(this):m_Player ); }
			set{ Player.Copy( value ); }
		}

		[SerializeField]
		private PlayerInventoryObject m_Inventory = null;
		public PlayerInventoryObject Inventory{
			get{ return m_Inventory = ( m_Inventory == null ? new PlayerInventoryObject(this):m_Inventory ); }
			set{ Inventory.Copy( value ); }
		}

		[SerializeField]
		private InputEventsObject m_Events = null;
		public InputEventsObject Events{
			get{ return m_Events = ( m_Events == null ? new InputEventsObject(this):m_Events ); }
			set{ Events.Copy( value ); }
		}

		public override void Start()
		{
			base.Start();

			Player.Init( this );
			Inventory.Init( this );
		}




		public override void OnEnable()
		{
			base.OnEnable();

			if( Player.UseDeathCamera && Player.DeathCamera != null && Player.DeathCamera.activeInHierarchy )
				Player.DeathCamera.SetActive( false );
		}

		public override void OnDisable()
		{
			base.OnDisable();

			if( Player.UseDeathCamera && ! ApplicationQuits )
			{				
				if( gameObject.GetComponentInChildren<Camera>() != null )
				{
					Camera _camera = gameObject.GetComponentInChildren<Camera>();
		
					if( Player.DeathCamera == null )
					{
						if( Player.DeathCameraReference == null )
							Player.DeathCameraReference = _camera.gameObject;

#if UNITY_5_4_OR_NEWER
							Player.DeathCamera = (GameObject)GameObject.Instantiate( Player.DeathCameraReference, null ); 
#else
							Player.DeathCamera = (GameObject)GameObject.Instantiate( Player.DeathCameraReference ); 
							Player.DeathCamera.transform.SetParent( null, true );
#endif
						Player.DeathCamera.name = gameObject.name + "KillCam";
					}

					Player.DeathCamera.transform.position = _camera.transform.position;
					Player.DeathCamera.transform.rotation = _camera.transform.rotation;		
					Player.DeathCamera.SetActive( true );
				}
			}
				
		}

		public override void Update()
		{
			base.Update();

			Events.Update( gameObject );
		}

		public override void LateUpdate()
		{
			base.LateUpdate();

			if( Status.DurabilityInPercent <= 0 )
				Status.Remove();
		}

		public void Suicide()
		{
			Status.SetDurability( 0 );
		}

		public void PickupItem()
		{
			if( Inventory.Enabled == false )
				return;
			
			Ray _ray = Camera.main.ScreenPointToRay( Input.mousePosition );
			RaycastHit _hit;

			if( Physics.Raycast(_ray, out _hit) )
			{
				ICECreatureItem _item = _hit.transform.GetComponent<ICECreatureItem>();
				if( _item != null )
					Inventory.Insert( _item.gameObject );
			}
		}

		public void DropItem( int _index )
		{
			Inventory.Drop( _index );
		}
	}
}
