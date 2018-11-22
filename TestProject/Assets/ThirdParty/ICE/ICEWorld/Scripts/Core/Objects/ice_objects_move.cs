// ice_objects_move.cs | MoveInfoObject
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

namespace ICE.World.Objects
{
	[System.Serializable]
	public class MoveInfoObject : ICEOwnerObject
	{
		public MoveInfoObject(){}
		public MoveInfoObject( ICEWorldBehaviour _component ) : base( _component ){ Init( _component ); }
		public MoveInfoObject( MoveInfoObject _object ){ Copy( _object );  }

		public void Copy( MoveInfoObject _object )
		{
			base.Copy( _object );
		}
	}
}
