// ##############################################################################
//
// ice_objects_fire.cs | ICE.World.Objects.FireObject
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
	public class FireObject : ICEOwnerObject 
	{
		public FireObject(){}
		public FireObject( ICEWorldBehaviour _component ) : base( _component ) {}
		public FireObject( FireObject _object ){ Copy( _object ); }

		public void Copy( FireObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			IntensityMin = _object.IntensityMin;
			IntensityMax = _object.IntensityMax;

			FireLight = _object.FireLight;
		}

		public float IntensityMin = 1.5f;
		public float IntensityMax = 3f;

		public Light FireLight = null;

		public override void Init( ICEWorldBehaviour _component)
		{
			base.Init( _component );

			if(  FireLight == null && _component != null )
				FireLight = _component.GetComponentInChildren<Light>();
		}

		public bool IsActive{
			set{ if( FireLight != null && Enabled == true )FireLight.enabled = value; }
			get{ return ( FireLight != null && Enabled == true ?FireLight.enabled:false ); }
		}

		public void Update()
		{
			if( FireLight == null || Enabled == false )
				return;
			
			float _noise = Mathf.PerlinNoise( Random.Range(0.0f, 150.0f), Time.time );
			FireLight.intensity = Mathf.Lerp( IntensityMin, IntensityMax, _noise );
		}
	}
}
