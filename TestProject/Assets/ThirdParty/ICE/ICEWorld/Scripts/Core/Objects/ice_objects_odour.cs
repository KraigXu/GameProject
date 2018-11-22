// ##############################################################################
//
// ice_CreatureOdour.cs
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
using ICE.World.EnumTypes;

namespace ICE.World.Objects
{
	[System.Serializable]
	public struct OdourContainer
	{
		public bool Enabled;
		public OdourType Type;
		public float Intensity;
		public float IntensityMax;
		public float Range;
		public float RangeMax;
		public bool UseMarker;
		public bool UseEffect;
	}

	[System.Serializable]
	public class OdourDataObject : ICEDataObject
	{
		public OdourDataObject(){}
		public OdourDataObject( OdourDataObject _data ) : base( _data ) { Copy( _data ); }

		public OdourType Type = OdourType.NONE;
		public float Intensity = 50f;
		public float IntensityMax = 100f;
		public float Range = 25f;
		public float RangeMax = 100f;
		public bool UseMarker = false;
		public float MarkerMinInterval = 2;
		public float MarkerMaxInterval = 5;
		public float MarkerIntervalMax = 60;
		[XmlIgnore]
		public GameObject MarkerPrefab = null;

		public bool UseEffect = false;
		[XmlIgnore]
		public GameObject EffectPrefab = null;
		[XmlIgnore]
		protected GameObject m_Effect = null;

		public void Copy( OdourDataObject _odour )
		{
			base.Copy( _odour );

			Type = _odour.Type;
			Intensity = _odour.Intensity;
			IntensityMax = _odour.IntensityMax;
			Range = _odour.Range;
			RangeMax = _odour.RangeMax;

			UseMarker = _odour.UseMarker;
			MarkerMinInterval = _odour.MarkerMinInterval;
			MarkerMaxInterval = _odour.MarkerMaxInterval;
			MarkerIntervalMax = _odour.MarkerIntervalMax;
			MarkerPrefab = _odour.MarkerPrefab;

			UseEffect = _odour.UseEffect;
			EffectPrefab = _odour.EffectPrefab;
		}

		public void SetOdour( OdourContainer _odour )
		{
			Type = _odour.Type;
			Intensity = _odour.Intensity;
			IntensityMax = _odour.IntensityMax;
			Range = _odour.Range;
			RangeMax = _odour.RangeMax;
			UseMarker = _odour.UseMarker;
			UseEffect = _odour.UseEffect;
		}

		public OdourContainer GetOdour()
		{
			OdourContainer _odour;

			_odour.Enabled = false;
			_odour.Type = Type;
			_odour.Intensity = Intensity;
			_odour.IntensityMax = IntensityMax;
			_odour.Range = Range;	
			_odour.RangeMax = RangeMax;
			_odour.UseMarker = UseMarker;
			_odour.UseEffect = UseEffect;
			_odour.Type = Type;

			return _odour;
		}
	}

	[System.Serializable]
	public class OdourObject : OdourDataObject
	{
		public OdourObject(){}
		public OdourObject( ICEWorldBehaviour _component ){ Init( _component ); }
		public OdourObject( OdourObject _data ) : base( _data ) {}

		public void Init( ICEWorldBehaviour _component )
		{
			//Todo : ???
		}

		private float m_OdourTimer = 0;
		private float m_OdourInterval = 0;
		public void HandleOdourMarker(  Transform _transform  )
		{
			if( Type == OdourType.NONE || UseMarker == false || MarkerPrefab == null )
				return;

			m_OdourTimer += Time.deltaTime;
			if( m_OdourTimer > m_OdourInterval )
			{
				m_OdourTimer = 0;
				m_OdourInterval = Random.Range( MarkerMinInterval, MarkerMaxInterval );

				GameObject _marker = WorldManager.Spawn( MarkerPrefab, _transform.position, _transform.rotation );
				if( _marker != null )
					_marker.name = MarkerPrefab.name;
			}
		}

		public void StartEffect( Transform _transform )
		{
			if( UseEffect && EffectPrefab != null && m_Effect == null )
			{
				m_Effect = WorldManager.Spawn( EffectPrefab, _transform.position, _transform.rotation );

				if( m_Effect != null )
					m_Effect.transform.parent = _transform;
			}
		}

		public void StopEffect()
		{
			if( m_Effect != null )
				WorldManager.Destroy( m_Effect );
		}

		public float GetIntensityByDistance(float _distance )
		{
			float _m = 1 - MathTools.Normalize( _distance, 0, Range );
			_m = (_m<0?0:_m);
			_m = (_m>1?1:_m);

			return (Intensity * _m);
		}
	}
}
