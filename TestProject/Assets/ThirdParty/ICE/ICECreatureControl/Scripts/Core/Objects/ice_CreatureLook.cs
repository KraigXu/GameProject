// ##############################################################################
//
// ice_CreatureLook.cs
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
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;

using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.EnumTypes;

namespace ICE.Creatures.Objects
{
	[System.Serializable]
	public class LookDataObject : ICEDataObject
	{
		public LookDataObject(){}
		public LookDataObject( LookDataObject _look )
		{
			Enabled = _look.Enabled;

			UseInvisibility = _look.UseInvisibility;
			InvisibilityType = _look.InvisibilityType;
		}


			
		public bool UseInvisibility = false;
		public LookInvisibleType InvisibilityType = LookInvisibleType.None;
	}

	[System.Serializable]
	public class LookObject : ICEOwnerObject
	{
		//private LookDataObject m_LastLook = null;
		public LookObject(){}
		public LookObject( GameObject _owner )
		{
			Init( _owner );
		}

		public void Init( GameObject _owner )
		{
			base.SetOwner( _owner );
		}

		public void Adapt( LookDataObject _look )
		{
			if( Owner == null || _look == null || _look.Enabled == false )
				return;

			//if( m_LastLook.GetHashCode != _look )

			SystemTools.EnableRenderer( Owner.transform, ! _look.UseInvisibility );
		}

	
	}
}


