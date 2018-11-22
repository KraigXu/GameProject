// ##############################################################################
//
// ice_objects_sensoria.cs | SensoriaObject
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
	public class SensoriaObject : ICEOwnerObject
	{
		public SensoriaObject(){}
		public SensoriaObject( ICEWorldBehaviour _component ) : base( _component ){ Init( _component ); }
		public SensoriaObject( SensoriaObject _object ){ Copy( _object );  }

		public void Copy( SensoriaObject _object )
		{
			base.Copy( _object );

			FieldOfView = _object.FieldOfView;
			VisualRange = _object.VisualRange;
			VisualRangeMaximum  = _object.VisualRangeMaximum;

			UseDynamicVisualSensorPosition = _object.UseDynamicVisualSensorPosition;
			VisualSensorName  = _object.VisualSensorName;
			VisualSensorOffset = _object.VisualSensorOffset;
			VisualSensorHorizontalOffset = _object.VisualSensorHorizontalOffset;

			UseSphereCast  = _object.UseSphereCast;
			SphereCastRadius = _object.SphereCastRadius;

			DefaultSenseVisual = _object.DefaultSenseVisual; 
			SenseVisualAgeMultiplier = _object.SenseVisualAgeMultiplier;
			SenseVisualFitnessMultiplier = _object.SenseVisualFitnessMultiplier;

			DefaultSenseAuditory = _object.DefaultSenseAuditory;
			SenseAuditoryAgeMultiplier = _object.SenseAuditoryAgeMultiplier;
			SenseAuditoryFitnessMultiplier = _object.SenseAuditoryFitnessMultiplier;

			DefaultSenseOlfactory = _object.DefaultSenseOlfactory; 
			SenseOlfactoryAgeMultiplier = _object.SenseOlfactoryAgeMultiplier;
			SenseOlfactoryFitnessMultiplier = _object.SenseOlfactoryFitnessMultiplier;

			DefaultSenseGustatory = _object.DefaultSenseGustatory; 
			SenseGustatoryAgeMultiplier = _object.SenseGustatoryAgeMultiplier;
			SenseGustatoryFitnessMultiplier = _object.SenseGustatoryFitnessMultiplier;

			DefaultSenseTactile = _object.DefaultSenseTactile; 
			SenseTouchAgeMultiplier = _object.SenseTouchAgeMultiplier;
			SenseTouchFitnessMultiplier = _object.SenseTouchFitnessMultiplier;
		}

		public float FieldOfView = 80;
		public float VisualRange = 0;
		public float VisualRangeMaximum = 100;

		public bool UseDynamicVisualSensorPosition = false;
		public string VisualSensorName = "";
		public float VisualSensorHorizontalOffset = 0.15f;

		public bool UseSphereCast = false;
		public float SphereCastRadius = 0.3f;
		public float SphereCastRadiusMaximum = 1;


		public int DefaultSenseVisual = 100; 
		public float SenseVisualAgeMultiplier = 0.0f;
		public float SenseVisualFitnessMultiplier = 0.0f;

		public int DefaultSenseAuditory = 100;
		public float SenseAuditoryAgeMultiplier = 0.0f;
		public float SenseAuditoryFitnessMultiplier = 0.0f;

		public int DefaultSenseOlfactory = 100; 
		public float SenseOlfactoryAgeMultiplier = 0.0f;
		public float SenseOlfactoryFitnessMultiplier = 0.0f;

		public int DefaultSenseGustatory = 100; 
		public float SenseGustatoryAgeMultiplier = 0.0f;
		public float SenseGustatoryFitnessMultiplier = 0.0f;

		public int DefaultSenseTactile = 100; 
		public float SenseTouchAgeMultiplier = 0.0f;
		public float SenseTouchFitnessMultiplier = 0.0f;

		private Transform m_VisualSensorTransform = null;

		public Vector3 VisualSensorOffset = new Vector3( 0, 1.6f, 0.15f );

		public Vector3 VisualSensorPosition{
			get{ return GetVisualSensorPosition(); }
		}

		/// <summary>
		/// Gets the visual sensor position.
		/// </summary>
		/// <returns>The visual sensor position.</returns>
		public Vector3 GetVisualSensorPosition()
		{
			if( Owner == null )
				return Vector3.zero;
				
			Vector3 _position = Vector3.zero;

			if( UseDynamicVisualSensorPosition )
			{
				if( m_VisualSensorTransform == null )
					m_VisualSensorTransform = SystemTools.FindChildByName( VisualSensorName, Owner.transform );
			}
			else
				m_VisualSensorTransform = null;

			if( m_VisualSensorTransform != null )
				_position = m_VisualSensorTransform.position;
			else 
				_position = PositionTools.FixTransformPoint( Owner.transform, VisualSensorOffset );

			return _position;
		}
	}

}
