// ##############################################################################
//
// ice_objects_input.cs | ICE.World.Objects.InputObject
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

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;

namespace ICE.World.Objects
{
	[System.Serializable]
	public class InputObject : ICEOwnerObject 
	{
		public InputObject(){}
		public InputObject( ICEWorldBehaviour _component ) : base(_component ){}
		public InputObject( InputObject _object ){ Copy( _object ); }

		public void Copy( LayerObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

		}
	}
}
