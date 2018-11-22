// ##############################################################################
//
// ice_CreatureTemperatur.cs
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

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.World.Utilities;

namespace ICE.Creatures.Objects
{
	[System.Serializable]
	public class TemperaturDataObject : ICEOwnerObject
	{
		public TemperaturDataObject(){}
		public TemperaturDataObject( ICEWorldBehaviour _component ) : base( _component ) {}
	}

	[System.Serializable]
	public class TemperaturObject : TemperaturDataObject {

		public TemperaturObject(){}
		public TemperaturObject( ICEWorldBehaviour _component ) : base( _component ) { Init( _component ); }

		public override void Init( ICEWorldBehaviour _component )
		{
			base.Init( _component );
		}
	}
}
