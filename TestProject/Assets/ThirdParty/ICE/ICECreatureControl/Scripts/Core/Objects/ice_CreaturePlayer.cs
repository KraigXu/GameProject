// ##############################################################################
//
// ice_CreaturePlayer.cs
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
using System.Xml;
using System.Xml.Serialization;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;


namespace ICE.Creatures.Objects
{
	[System.Serializable]
	public class PlayerObject : ICEOwnerObject
	{
		public PlayerObject(){}
		public PlayerObject( ICEWorldBehaviour _component ) : base( _component )
		{
			Init( _component );
		}

		public override void Init( ICEWorldBehaviour _component )
		{
			base.Init( _component );


		}

		public override void Reset()
		{
			if( Application.isPlaying )
				return;


		}

		[SerializeField]
		private bool m_UseDeathCamera = true;
		public bool UseDeathCamera{
			get{ return ( Enabled ? m_UseDeathCamera : false ); }
			set{ m_UseDeathCamera = ( Enabled ? value  : m_UseDeathCamera ); }
		}

		public GameObject DeathCameraReference = null;
		public GameObject DeathCamera = null;

		[SerializeField]
		private bool m_UseMousePositionToAim = false;
		public bool UseMousePositionToAim{
			get{ return ( Enabled ? m_UseMousePositionToAim : false ); }
			set{ m_UseMousePositionToAim = ( Enabled ? value  : m_UseMousePositionToAim ); }
		}
	}
}
