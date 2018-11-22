// ##############################################################################
//
// ice_objects_status.cs | EntityStatusObject
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
	public class DurabilityInfluenceObject : ICEDataObject
	{
		public DurabilityInfluenceObject(){}
		public DurabilityInfluenceObject( string _key ) : base(){
			Key = _key;
		}
		public DurabilityInfluenceObject( DurabilityInfluenceObject _data ) : base( _data ){
			Copy( _data );
		}

		public void Copy( DurabilityInfluenceObject _data )
		{
			if( _data == null )
				return;

			base.Copy( _data );

			Key = _data.Key;
			ValueInPercent = _data.ValueInPercent;
		}

		public string Key = "";
		public float ValueInPercent = 100;
		public float DefaultMultiplier = 1;

		public void ApplyValue( float _value )
		{
			ValueInPercent += _value;
			ValueInPercent = MathTools.FixedPercent( ValueInPercent );
		}

	}

	[System.Serializable]
	public class DurabilityInfluenceMultiplierObject : ICEDataObject
	{
		public DurabilityInfluenceMultiplierObject(){}
		public DurabilityInfluenceMultiplierObject( DurabilityInfluenceMultiplierObject _data ) :base(_data)
		{
			Copy( _data );
		}
		public void Copy( DurabilityInfluenceMultiplierObject _data )
		{
			if( _data == null )
				return;

			base.Copy( _data );

			Influence = _data.Influence;
			Multiplier = _data.Multiplier;
		}


		public DurabilityInfluenceObject Influence;
		public float Multiplier;

		public float Value()
		{
			if( Influence != null )
				return Influence.ValueInPercent * Multiplier;
			else
				return 0;
		}
	}

	[System.Serializable]
	public class DurabilityAttributeObject : ICEDataObject
	{
		public DurabilityAttributeObject(){}
		public DurabilityAttributeObject( string _key ) : base(){
			Key = _key;
		}
		public DurabilityAttributeObject( DurabilityAttributeObject _data ) : base( _data ){
			Copy( _data );
		}

		public void Copy( DurabilityAttributeObject _data )
		{
			if( _data == null )
				return;

			base.Copy( _data );

			Key = _data.Key;

			Multiplier.Clear();
			foreach( DurabilityInfluenceMultiplierObject _multiplier in _data.Multiplier )
				Multiplier.Add( new DurabilityInfluenceMultiplierObject( _multiplier ) );
		}

		public string Key = "";

		[SerializeField]
		private List<DurabilityInfluenceMultiplierObject> m_Multiplier = null;
		public List<DurabilityInfluenceMultiplierObject> Multiplier{
			get{ return m_Multiplier = ( m_Multiplier == null ? new List<DurabilityInfluenceMultiplierObject>() : m_Multiplier ); }
			set{ m_Multiplier = value; }
		}

		public float GetInfluencesInPercent()
		{
			float _value_in_percent = 100;

			foreach( DurabilityInfluenceMultiplierObject _multiplier in Multiplier )
				_value_in_percent += _multiplier.Value();

			return MathTools.FixedPercent( _value_in_percent );
		}
	}

	[System.Serializable]
	public class DurabilityCompositionObject : ICEDataObject
	{
		public DurabilityCompositionObject(){}
		public DurabilityCompositionObject( DurabilityCompositionObject _composition ) : base( _composition ){
			Copy( _composition );
		}

		public void Copy( DurabilityCompositionObject _composition )
		{
			Attributes.Clear();
			foreach( DurabilityAttributeObject _attribute in _composition.Attributes )
				Attributes.Add( new DurabilityAttributeObject( _attribute ) );

			Influences.Clear();
			foreach( DurabilityInfluenceObject _influence in _composition.Influences )
				Influences.Add( new DurabilityInfluenceObject( _influence ) );
		}

		public override void Reset()
		{
			Attributes.Clear();
			Influences.Clear();
		}

		[SerializeField]
		private List<DurabilityAttributeObject> m_Attributes = null;
		public List<DurabilityAttributeObject> Attributes{
			get{ return m_Attributes = ( m_Attributes == null ? new List<DurabilityAttributeObject>() : m_Attributes ); }
			set{ m_Attributes = value; }
		}

		public void AddAttributeByKey( string _key )
		{
			DurabilityAttributeObject _attribute = GetAttributeByKey( _key );

			if( _attribute == null )
				Attributes.Add( new DurabilityAttributeObject( _key ) );
		}

		public DurabilityAttributeObject GetAttributeByKey( string _key )
		{
			foreach( DurabilityAttributeObject _attribute in Attributes )
				if( _attribute.Key == _key )
					return _attribute;

			return null;
		}

		[SerializeField]
		private List<DurabilityInfluenceObject> m_Influences = null;
		public List<DurabilityInfluenceObject> Influences{
			get{ return m_Influences = ( m_Influences == null ? new List<DurabilityInfluenceObject>() : m_Influences ); }
			set{ m_Influences = value; }
		}

		public void AddInfluenceByKey( string _key )
		{
			DurabilityInfluenceObject _influence = GetInfluenceByKey( _key );

			if( _influence == null )
				Influences.Add( new DurabilityInfluenceObject( _key ) );
		}

		public DurabilityInfluenceObject GetInfluenceByKey( string _key )
		{
			foreach( DurabilityInfluenceObject _influence in Influences )
				if( _influence.Key == _key )
					return _influence;

			return null;
		}

		public DurabilityInfluenceObject ForceInfluenceByKey( string _key )
		{
			DurabilityInfluenceObject _influence = GetInfluenceByKey( _key );

			if( _influence == null )
			{
				_influence = new DurabilityInfluenceObject( _key );
				Influences.Add( _influence );
			}

			return _influence;
		}

		public float InfluencesInPercent
		{
			get{
				float _influences = 0;

				if( Attributes.Count > 0 )
				{
					foreach( DurabilityAttributeObject _influence in Attributes )
						_influences += _influence.GetInfluencesInPercent();

					_influences = _influences / Attributes.Count;
				}
				else
					_influences = 100;

				return MathTools.FixedPercent( _influences );
			}
		}

		public virtual float UpdateDurability( float _default ){
			return _default / 100 * InfluencesInPercent;			
		}

		public void ApplyInfluenceValue( string _key, float _value )
		{
			DurabilityInfluenceObject _influence = GetInfluenceByKey( _key );

			if( _influence != null )
				_influence.ApplyValue( _value );
		}
	}

}
