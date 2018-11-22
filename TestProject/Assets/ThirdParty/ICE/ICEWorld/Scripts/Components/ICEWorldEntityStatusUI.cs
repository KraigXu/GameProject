// ##############################################################################
//
// ICEFirstPersonController.cs
// Version 1.4.0
// 
// Copyrights © Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.
// http://www.icecreaturecontrol.com
// mailto:support@icecreaturecontrol.com
//
// Unity Asset Store End User License Agreement (EULA)
// http://unity3d.com/legal/as_terms
//
// This controller based on the Unity Standard Assets First Person Controller 
// and will used in ICE demo scenes instead of the original Unity Standard Assets
// scripts to avoid conflicts with given namespaces. 
//
// ##############################################################################


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;

namespace ICE.World
{
	public class ICEWorldEntityStatusUI : ICEWorldBehaviour {

		private ICEWorldEntity m_Entity = null;
		private ICEWorldEntity Entity{
			get{ return m_Entity = ( m_Entity == null ? GetComponent<ICEWorldEntity>() : m_Entity ); }
		}

		[SerializeField]
		private EntityStatusDisplayObject m_StatusUI = null;
		public virtual EntityStatusDisplayObject StatusUI{
			get{ return m_StatusUI = ( m_StatusUI == null ? new EntityStatusDisplayObject(this) : m_StatusUI ); }
			set{ StatusUI.Copy( value ); }
		}

		public Image HealthBar = null;

		public override void OnEnable () {
			base.OnEnable();
			StatusUI.Init( this );
		}

		public override void OnDisable () {
			base.OnDisable();
			StatusUI.Reset();
		}

		// Update is called once per frame
		public override void Update () 
		{
			base.Update();

			if( Entity == null )
				return;
	
			if( HealthBar != null )
			{
				HealthBar.fillAmount = Entity.Status.DurabilityInPercent * 0.01f;
				HealthBar.color = HSBColor.ToColor( HSBColor.Lerp( Color.red, Color.green, HealthBar.fillAmount ) );
			}
		}

		private bool m_DisplayInfo = false;
		private float m_InfoDelayTime = 0;
		private float m_InfoDelayTimer = 0;
		public override void FixedUpdate() {
			base.FixedUpdate();

			{
				m_InfoDelayTimer += Time.fixedDeltaTime;
				if( m_InfoDelayTimer < m_InfoDelayTime )
					return;

				m_DisplayInfo = false;
				m_InfoDelayTimer = 0;
			}
		}

		public void DisplayInfos( float _delay ){
			m_InfoDelayTime = _delay + 0.1f;
			m_InfoDelayTimer = 0;
			m_DisplayInfo = true;
		}

		void OnGUI (){

			if( m_DisplayInfo )
				StatusUI.Display();
		}
	}
}


namespace ICE.World.Objects
{
	[System.Serializable]
	public class EntityStatusDisplayObject : ICEOwnerObject
	{
		public EntityStatusDisplayObject(){}
		public EntityStatusDisplayObject( ICEWorldBehaviour _component ) : base( _component ){}
		public EntityStatusDisplayObject( EntityStatusDisplayObject _object ) : base( _object ){ Copy( _object ); }

		public void Copy( EntityStatusDisplayObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );
		}

		public bool ShowInfo = true;

		[SerializeField]
		private string m_Name = "";
		public string Name{
			get{ return m_Name = ( string.IsNullOrEmpty( m_Name ) && Owner != null ? Owner.name : m_Name ); }
			set{ m_Name = value; }
		}
		public bool ShowName = true;
		public bool ShowCommand = true;
		public string Command = "Press F to Pickup\n";
		public Vector3 Offset = Vector3.zero;

		public void Display()
		{
			if( Camera.main != null && Owner != null ) 
			{
				if( string.IsNullOrEmpty( Name ) )
					Name = Owner.name;

				Vector3 _pos = Camera.main.WorldToScreenPoint( PositionTools.FixTransformPoint( Owner.transform, Offset ) );
				GUI.skin.label.alignment = TextAnchor.MiddleCenter;
				GUI.Label( new Rect( _pos.x, Screen.height - _pos.y, 200, 60 ), ( ShowCommand ? Command : "" ) + ( ShowName ? Name : "" ) );
			}
		}
	}
}
