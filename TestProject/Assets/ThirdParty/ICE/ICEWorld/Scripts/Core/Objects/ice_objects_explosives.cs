// ##############################################################################
//
// ice_objects_explosives.cs | ICE.World.Objects.ExplosiveDataObject | ICE.World.Objects.ExplosiveObject
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
	public class ExplosiveDataObject : ICEOwnerObject{

		public ExplosiveDataObject(){}
		public ExplosiveDataObject( ExplosiveDataObject _object ) : base( _object ){ Copy( _object ); }
		public ExplosiveDataObject( ICEWorldBehaviour _component ) : base( _component ){ Init( _component ); }

		public void Copy( ExplosiveDataObject _object  )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			DetonateOnContact = _object.DetonateOnContact;
			DetonateOnDestroyed = _object.DetonateOnDestroyed;
		}

		public bool DetonateOnContact = true;
		public bool DetonateOnCountdown = true;
		public bool DetonateOnDestroyed = true;
	}

	[System.Serializable]
	public class ExplosiveObject : ExplosiveDataObject{

		public ExplosiveObject(){}
		public ExplosiveObject( ExplosiveObject _object ) : base( _object ){ Copy( _object ); }
		public ExplosiveObject( ICEWorldBehaviour _component ) : base( _component ){ Init( _component ); }


	}
}
