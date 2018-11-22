// ##############################################################################
//
// ice_objects_culling.cs | CullingOptionsObject
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
using ICE.World.EnumTypes;

namespace ICE.World.Objects
{
	[System.Serializable]
	public class CullingOptionsObject : ICEDataObject
	{
		public CullingOptionsObject(){}
		public CullingOptionsObject( CullingOptionsObject _object ){ Copy( _object );  }

		public virtual void Copy( CullingOptionsObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			UseCullingConditions = _object.UseCullingConditions;
			MainCameraDistanceMin = _object.MainCameraDistanceMin;
			MainCameraDistanceMax = _object.MainCameraDistanceMax;
			MainCameraDistanceMaximum = _object.MainCameraDistanceMaximum;
		}

		public bool UseCullingConditions = false;
		public float MainCameraDistanceMin = -100;
		public float MainCameraDistanceMax = 200;
		public float MainCameraDistanceMaximum = 1000;

		public bool CheckCullingConditions( GameObject _object ){
			return CheckCullingConditions( ( _object != null ? _object.transform.position : Vector3.zero ) );
		}

		public bool CheckCullingConditions( ICEWorldBehaviour _component ){
			return CheckCullingConditions( _component.transform.position );
		}

		public bool CheckCullingConditions( Vector3 _position ) 
		{
			if( ! Enabled || Camera.main == null )
				return false;

			Vector3 _heading = _position - Camera.main.transform.position;
			float _distance = Vector3.Dot( _heading, Camera.main.transform.forward );
			float _side_distance = Mathf.Abs( Vector3.Dot( _heading, Camera.main.transform.right ) );

			return ( _distance >= MainCameraDistanceMin && _distance <= MainCameraDistanceMax && _side_distance <= Mathf.Abs( MainCameraDistanceMax ) ? false : true );
		}
	}
}