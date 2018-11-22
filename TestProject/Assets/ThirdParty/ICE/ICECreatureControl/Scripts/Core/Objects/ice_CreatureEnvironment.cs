// ##############################################################################
//
// ice_CreatureEnvironment.cs
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

using ICE.World;
using ICE.World.Objects;

using ICE.Creatures;
using ICE.Creatures.Utilities;

namespace ICE.Creatures.Objects
{
	//--------------------------------------------------
	// ICECreatureObject.FootstepDataObject
	//--------------------------------------------------
	[System.Serializable]
	public class EnvironmentObject : ICEOwnerObject
	{
		public EnvironmentObject(){}
		public EnvironmentObject( ICEWorldBehaviour _component ) : base( _component ){ Init( _component ); }
		public EnvironmentObject( EnvironmentObject _object ) : base( _object ){ Copy( _object ); }

		public override void Init( ICEWorldBehaviour _component )
		{
			base.Init( _component );

			SurfaceHandler.Init( _component );
			CollisionHandler.Init( _component );

		}

		public void Copy( EnvironmentObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			CollisionHandler.Copy( _object.CollisionHandler );
			SurfaceHandler.Copy( _object.SurfaceHandler );
		}

		[SerializeField]
		private CollisionObject m_Collision = null;
		public CollisionObject CollisionHandler{
			get{ return m_Collision = ( m_Collision == null ? new CollisionObject(OwnerComponent):m_Collision); }
			set{ CollisionHandler.Copy( value ); }
		}

		[SerializeField]
		private SurfaceObject m_Surface = null;
		public SurfaceObject SurfaceHandler{
			get{ return m_Surface = ( m_Surface == null ? new SurfaceObject(OwnerComponent):m_Surface); }
			set{ SurfaceHandler.Copy( value ); }
		}

		/// <summary>
		/// Reset this instance.
		/// </summary>
		public override void Reset()
		{
			CollisionHandler.Reset();
			SurfaceHandler.Reset();
		}
	}
}
