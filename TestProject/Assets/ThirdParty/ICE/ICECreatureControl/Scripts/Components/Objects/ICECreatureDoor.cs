// ##############################################################################
//
// ICECreatureDoor.cs
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
using System;
using System.Collections;
using System.Collections.Generic;

using ICE;
using ICE.World;
using ICE.World.Utilities;
using ICE.World.Objects;
using ICE.World.EnumTypes;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;

namespace ICE.Creatures{

	[RequireComponent(typeof(Animator))]
	public class ICECreatureDoor : ICECreatureObject {

		/// <summary>
		/// Gets the entity classification type of the object
		/// </summary>
		/// <value>The classification type of the entity.</value>
		/// <description>The EntityClassType will be used to quickly 
		/// identify the correct class type of a derived entity object 
		/// without casts
		/// </description>
		public override EntityClassType EntityType{
			get{ return EntityClassType.Door; }
		}

		protected override void OnRegisterBehaviourEvents()
		{
			base.OnRegisterBehaviourEvents();
			RegisterBehaviourEvent("ToggleLock");
		}

		private Animator m_Animator = null;

		public bool FunctionalityFoldout = true;

		public bool IsOpen = false;
		public bool IsLocked = false;

		private float m_ClosingDelayTimer = 0;
		public float ClosingDelayTime = 5;
		public float ClosingDelayMaximum = 30;

		public bool UseFunctionKey = false;
		public KeyCode DoorFunctionKey = KeyCode.F;
		public bool UseLockToggleKey = false;
		public KeyCode DoorLockToggleKey = KeyCode.L;

		public string OpenAnimatorParameter = "Open";

		public override void Awake()
		{
			base.Awake();

			m_Animator = GetComponent<Animator>();


		}

		public override void Update()
		{
			if( IsOpen && ClosingDelayTime > 0 )
			{
				m_ClosingDelayTimer += Time.deltaTime;
				if( m_ClosingDelayTimer >= ClosingDelayTime )
					HandleDoor( false, null );
			}

			base.Update();
		}

		public override void OnTriggerEnter ( Collider _collider ) {

			if( transform.IsChildOf( _collider.transform ) )
				return;

			HandleDoor( true, _collider.transform.gameObject );


		}

		public override void OnTriggerStay ( Collider _collider ) {

			if( transform.IsChildOf( _collider.transform ) )
				return;

			HandleDoor( true, _collider.transform.gameObject );
		}

		public override void OnTriggerExit ( Collider _collider ) {

			if( transform.IsChildOf( _collider.transform ) )
				return;

			if( ClosingDelayTime == 0 )
				HandleDoor( false, _collider.transform.gameObject );
		}

		private void HandleDoor( bool _open, GameObject _object )
		{
			if( m_Animator == null )
				return;

			ICECreaturePlayer _player = ( _object != null ? _object.GetComponent<ICECreaturePlayer>() : null );

			if( _player != null && Input.GetKeyDown( DoorLockToggleKey ) )
				IsLocked = ! IsLocked;

			if( IsLocked )
				_open = false;

			if( _object == null || _player == null || ! UseFunctionKey || Input.GetKeyDown( DoorFunctionKey ) )
			{
				IsOpen = _open;

				if( m_Animator.GetBool( OpenAnimatorParameter ) != IsOpen )
					m_Animator.SetBool( OpenAnimatorParameter, _open );
				
				m_ClosingDelayTimer = 0;
			}
		}

		public void ToggleLock()
		{
		}

	}
}
