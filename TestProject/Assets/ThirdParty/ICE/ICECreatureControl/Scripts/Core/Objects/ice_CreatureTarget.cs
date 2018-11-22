// ##############################################################################
//
// ice_CreatureTarget.cs
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

using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;
using ICE.Creatures.Attributes;

namespace ICE.Creatures.Objects
{

	[System.Serializable]
	public class TargetBehaviourObject : ICEObject
	{
		public TargetBehaviourObject(){}
		public TargetBehaviourObject( TargetBehaviourObject _data ){ Copy( _data ); }

		public void Copy( TargetBehaviourObject _data )
		{
			if( _data == null )
				return;
			Foldout = _data.Foldout;

			UseAdvancedBehaviour = _data.UseAdvancedBehaviour;
			UseSelectiveBehaviour = _data.UseSelectiveBehaviour;

			CurrentBehaviourModeKey = _data.CurrentBehaviourModeKey;
			BehaviourModeKey = _data.BehaviourModeKey;
			BehaviourModeKeyReached = _data.BehaviourModeKeyReached;
		}

		public bool Foldout = false;

		public bool UseAdvancedBehaviour = false;
		public bool UseSelectiveBehaviour = false;

		public string CurrentBehaviourModeKey = "";
		public string BehaviourModeKey = "";
		public string BehaviourModeKeyReached = "";

		public bool UseDefault{
			get{ return ( UseAdvancedBehaviour == false && UseSelectiveBehaviour == false && string.IsNullOrEmpty( BehaviourModeKey.Trim() ) == true ? true : false );}
		}

		public bool IsStandard{
			get{ return ( UseAdvancedBehaviour == false && UseSelectiveBehaviour == false && string.IsNullOrEmpty( BehaviourModeKey.Trim() ) == false ? true : false );}
		}

		public bool IsAdvanced{
			get{ return ( UseAdvancedBehaviour == true && ! IsSelective && ! string.IsNullOrEmpty( BehaviourModeKey.Trim() ) && ! string.IsNullOrEmpty( BehaviourModeKeyReached.Trim() ) ? true : false ); }
		}

		public bool IsSelective{
			get{ return ( UseSelectiveBehaviour == true ? true : false ); }
		}
			
		public void SetDefault()
		{
			UseAdvancedBehaviour = false;
			UseSelectiveBehaviour = false;
		}

		public string BehaviourTitleSuffix(){
			return ( BehaviourTitle().Trim() != "" ? " (" + BehaviourTitle().Trim() + ")" : "" );
		}

		public string BehaviourTitle()
		{
			if( IsStandard )
				return BehaviourModeKey;
			else if( IsAdvanced )
				return BehaviourModeKey + "/" + BehaviourModeKeyReached;
			else
				return "";
		}
	}

	[System.Serializable]
	public class TargetMoveObject : ICEDataObject
	{
		public TargetMoveObject(){}
		public TargetMoveObject( TargetMoveObject _data ) : base( _data ) { Copy( _data ); }
		
		public void Copy( TargetMoveObject _data )
		{
			if( _data == null )
				return;
			
			base.Copy( _data );

			m_OverlapPreventionLayerMask = _data.OverlapPreventionLayerMask;
			MovePositionType = _data.MovePositionType;

			Offset = _data.Offset;
			m_OffsetAngle = _data.OffsetAngle;
			m_OffsetDistance = _data.OffsetDistance;
			m_RandomizedOffset = _data.RandomizedOffset;

			LastKnownPosition = _data.LastKnownPosition;

			StoppingDistance = _data.StoppingDistance;
			StoppingDistanceZoneRestricted = _data.StoppingDistanceZoneRestricted;
			IgnoreLevelDifference = _data.IgnoreLevelDifference;

			FixedRange = _data.FixedRange;
			FixedRangeMaximum = _data.FixedRangeMaximum;

			RandomRange = _data.RandomRange;
			RandomRangeMaximum = _data.RandomRangeMaximum;
			UseRandomRect = _data.UseRandomRect;
			RandomRangeRect = _data.RandomRangeRect;

			SmoothingSpeed = _data.SmoothingSpeed;

			UseUpdateOffsetOnTargetChanged = _data.UseUpdateOffsetOnTargetChanged;
			UseUpdateOffsetOnActivateTarget = _data.UseUpdateOffsetOnActivateTarget;
			UseUpdateOffsetOnMovePositionReached = _data.UseUpdateOffsetOnMovePositionReached;
			UseUpdateOffsetOnRandomizedTimer = _data.UseUpdateOffsetOnRandomizedTimer;

			OffsetUpdateTimeMin = _data.OffsetUpdateTimeMin;
			OffsetUpdateTimeMax = _data.OffsetUpdateTimeMax;

			UseDynamicOffsetAngle = _data.UseDynamicOffsetAngle;
			UseRandomOffsetAngle= _data.UseRandomOffsetAngle;
			MinOffsetAngle = _data.MinOffsetAngle;
			MaxOffsetAngle = _data.MaxOffsetAngle;
			DynamicOffsetAngleUpdateSpeed = _data.DynamicOffsetAngleUpdateSpeed;

			UseDynamicOffsetDistance = _data.UseDynamicOffsetDistance;
			UseRandomOffsetDistance = _data.UseRandomOffsetDistance;
			MinOffsetDistance = _data.MinOffsetDistance;
			MaxOffsetDistance = _data.MaxOffsetDistance;
			OffsetDistanceMaximum = _data.OffsetDistanceMaximum;
			DynamicOffsetDistanceUpdateSpeed = _data.DynamicOffsetDistanceUpdateSpeed;

			MoveTargetName = _data.MoveTargetName;
			UseVerifiedDesiredTargetMovePosition = _data.UseVerifiedDesiredTargetMovePosition;

			CoverRange = _data.CoverRange;
			CoverRangeMaximum = _data.CoverRangeMaximum;
			CoverStepAngle = _data.CoverStepAngle;
			CoverMaxAngle = _data.CoverMaxAngle;
			CoverHorizontalOffset = _data.CoverHorizontalOffset;
			CoverHorizontalOffsetMaximum = _data.CoverHorizontalOffsetMaximum;
			CoverLayer = _data.CoverLayer;

		}

		private LayerMask m_OverlapPreventionLayerMask = 0;
		public LayerMask OverlapPreventionLayerMask{
			get{ return m_OverlapPreventionLayerMask = ( m_OverlapPreventionLayerMask == 0 ? WorldManager.OverlapPreventionLayerMask : m_OverlapPreventionLayerMask ); }
		}
		public TargetMovePositionType MovePositionType = TargetMovePositionType.Offset;
		public Vector3 Offset = Vector3.zero;

		public Vector3 LastKnownPosition = Vector3.zero;
		public string MoveTargetName = "";

		public bool UseVerifiedDesiredTargetMovePosition = false;

		public float RandomRange = 0;
		public float RandomRangeMaximum = 250;

		public bool UseRandomRect = false;
		public Vector3 RandomRangeRect = Vector3.zero;

		public float StoppingDistance = 2;
		public float StoppingDistanceMaximum = 10;
		public bool StoppingDistanceZoneRestricted = false;
		public bool IgnoreLevelDifference = true;

		public bool UseCreatureSpeed = false;
		public float SmoothingSpeed = 0;
		public float SmoothingSpeedMaximum = 10;

		public bool UseUpdateOffsetOnTargetChanged = false;
		public bool UseUpdateOffsetOnActivateTarget = false;
		public bool UseUpdateOffsetOnMovePositionReached = false;
		public bool UseUpdateOffsetOnRandomizedTimer = false;

		public float OffsetUpdateTimeMin = 5;
		public float OffsetUpdateTimeMax = 15;
		
		public bool UseDynamicOffsetAngle = false;
		public bool UseRandomOffsetAngle= false;
		public float MinOffsetAngle = 0;
		public float MaxOffsetAngle = 360;
		public float DynamicOffsetAngleUpdateSpeed = 0.05f;
		
		public bool UseDynamicOffsetDistance = false;
		public bool UseRandomOffsetDistance = false;
		public float MinOffsetDistance = 5;
		public float MaxOffsetDistance = 15;
		public float OffsetDistanceMaximum = 100;
		public float DynamicOffsetDistanceUpdateSpeed = 0.05f;

		public float FixedRange = 0;
		public float FixedRangeMaximum = 100;

		public Vector4 ComplexOffset{
			get{ return new Vector4( Offset.x, Offset.y, Offset.z, m_OffsetAngle ); }
		}

		public bool HasRandomRange{
			get{ return ( RandomRange > 0 || ( UseRandomRect && RandomRangeRect != Vector3.zero ) ? true : false ); }
		}
			
		[SerializeField]
		private float m_OffsetAngle = 0;
		public float OffsetAngle{
			get{ return m_OffsetAngle; }
			set{ UpdateOffset( value, m_OffsetDistance ); }
		}
			
		private float m_OffsetDistance = 0;
		public float OffsetDistance{
			get{ return m_OffsetDistance;}
		}
			
		private Vector3 m_RandomizedOffset = Vector3.zero;
		public Vector3 RandomizedOffset{
			get{ return Offset + ( m_OffsetAngle > 0  ? PositionTools.RotatePointAroundPivot( Vector3.zero, m_RandomizedOffset, MathTools.NormalizeAngle( m_OffsetAngle - 180 ) ) : m_RandomizedOffset ); }
		}

		/// <summary>
		/// Overwrites the randomized offset.
		/// </summary>
		/// <param name="_offset">Offset.</param>
		public void SetRandomizedOffset( Vector3 _offset ){
			m_RandomizedOffset = _offset;
		}

		public bool UpdateRandomRange( float _random_range )
		{
			bool _changed = ( RandomRange != _random_range );
			RandomRange = _random_range;
			return _changed;
		}

		public bool UpdateRandomRange( Vector3 _random_rect )
		{
			bool _changed = ( RandomRangeRect != _random_rect );
			RandomRangeRect = _random_rect;
			return _changed;
		}

		public bool UpdateOffset( Vector4 _complex )
		{
			Vector3 _offset = new Vector3( _complex.x, _complex.y, _complex.z );

			if( ! MathTools.CompareVectors( Offset, _offset, 0.5f ) ) //|| ( _offset != Vector3.zero && ( m_OffsetAngle == 0 || m_OffsetDistance == 0 ) ) )
			{
				Offset = _offset;

				if( Offset != Vector3.zero )
					m_OffsetAngle = PositionTools.GetNormalizedVectorAngle( Offset );
				else
					m_OffsetAngle = _complex.w;
				
				m_OffsetDistance = PositionTools.GetOverGroundDistance( Vector3.zero, Offset );

				return true;
			}
			else if( _complex.w != m_OffsetAngle )
				return UpdateOffset( _complex.w , m_OffsetDistance );
			else
				return false;
		}

		public bool UpdateOffset( Vector3 _offset )
		{
			if( ! MathTools.CompareVectors( Offset, _offset, 0.5f ) ) //|| ( _offset != Vector3.zero && ( m_OffsetAngle == 0 || m_OffsetDistance == 0 ) ) )
			{
				Offset = _offset;

				if( ! MathTools.CompareVectors( Offset, Vector3.zero, 0.01f ) )
					m_OffsetAngle = PositionTools.GetNormalizedVectorAngle( Offset );
				
				m_OffsetDistance = PositionTools.GetOverGroundDistance( Vector3.zero, Offset );

				return true;
			}
			else
				return false;
		}

		public bool UpdateOffset( float _angle , float _distance )
		{
			if( m_OffsetAngle != _angle || m_OffsetDistance != _distance )
			{
				m_OffsetAngle = _angle;
				m_OffsetDistance = _distance;

				Offset = PositionTools.GetPositionByAngleAndRadius( Vector3.zero, m_OffsetAngle, m_OffsetDistance );
			
				return true;
			}
			else
				return false;
		}
			
		private float m_OffsetTime = 0;
		private float m_OffsetTimer = 0;

		public bool UpdateOffsetOnRandomizedTimer()
		{
			if( ! Enabled || ! UseUpdateOffsetOnRandomizedTimer )
				return false;

			m_OffsetTimer += Time.deltaTime;
			if( m_OffsetTimer >= m_OffsetTime )
			{
				UpdateRandomOffset();
				m_OffsetTimer = 0;
				m_OffsetTime = Random.Range( OffsetUpdateTimeMin, OffsetUpdateTimeMax );

				return true;
			}
			else
				return false;

		}

		private float m_DynamicOffsetAngleUpdateSpeed = 0;
		private float m_DynamicOffsetDistanceUpdateSpeed = 0;

		private float m_DynamicOffsetDistance = 0;
		private float m_DynamicOffsetAngle = 0;
		/*
		public Vector3 UpdatePosition( Vector3 _current_position, Vector3 _target_position, float _speed, bool _reached, bool _randomize )
		{
			UpdateDynamicOffset();

			if( UpdateOffsetOnRandomizedTimer() || ( UseUpdateOffsetOnMovePositionReached && _reached ) || _randomize )
				UpdateRandomOffset();

			return SmoothPosition( _current_position, _target_position, _speed, _reached );
		}*/

		public float CoverDistance = 0.5f;
		public float CoverRange = 10;
		public float CoverRangeMaximum = 100;
		public float CoverStepAngle = 1;
		public float CoverMaxAngle = 180;
		public float CoverHorizontalOffset = 1;
		public float CoverHorizontalOffsetMaximum = 5;

		private Vector3 m_CoverMovePosition = Vector3.zero;
		public Vector3 CoverMovePosition{
			get{ return m_CoverMovePosition; }
		}

		[SerializeField]
		private LayerObject m_CoverLayer = null;
		public LayerObject CoverLayer{
			get{ return m_CoverLayer = ( m_CoverLayer == null ? new LayerObject( (LayerMask)Physics.DefaultRaycastLayers ) : m_CoverLayer ); }
			set{ CoverLayer.Copy( value ); }
		}

		public LayerMask CoverLayerMask{
			get{ return CoverLayer.Mask; }
		}

		public Vector3 UpdateCoverPosition( Transform _owner, Transform _target, bool _debug )
		{
			if( _owner == null || _target == null )
				return Vector3.zero;

			float _best_cover_distance = Mathf.Infinity;

			float _range = CoverRange;

			float _offset = CoverHorizontalOffset;

			Vector3 _direction = ( _owner.position - _target.position ).normalized;
			Vector3 _direction_left = _direction;
			Vector3 _direction_right = _direction;

			//ICEDebug.DrawRay( _owner.position, _direction * _range, Color.blue );

			Vector3 _owner_pos = _owner.position;
			Vector3 _target_pos = _target.position;

			RaycastHit _hit;
			Ray _ray;
		
			float _raycast_range = _range;
			float _raycast_angle = 0;
			float _raycast_max_angle = CoverMaxAngle * 0.5f;
			float _raycast_step_angle = Mathf.Clamp( CoverStepAngle, 1, 36 );
			while( _raycast_angle < _raycast_max_angle ) 
			{
				_ray = new Ray( _target_pos, _direction_right * _raycast_range );
				if( Physics.Raycast( _ray, out _hit, _range, CoverLayerMask ) ) 
				{
					_ray = new Ray( _hit.point - _hit.normal * _range, _hit.normal );
					if( _hit.collider.Raycast( _ray, out _hit, Mathf.Infinity ) ) 
					{
						Vector3 _pos = _hit.point + ( _hit.normal * _offset );
						float _dist = ( ( Vector3.Distance( _pos, _owner_pos ) + Vector3.Distance( _pos, _target_pos ) ) * 0.5f );
						if( _dist < _best_cover_distance )
						{
							_best_cover_distance = _dist;
							m_CoverMovePosition = _pos;
						}

						if( _debug )
							ICEDebug.DrawLine( _target_pos, m_CoverMovePosition, Color.green );

					}
					else if( _debug )
						ICEDebug.DrawLine( _target_pos, _hit.point, Color.yellow );
				}
				else if( _debug )
					ICEDebug.DrawRay( _target_pos, _direction_right * _raycast_range, Color.red );

				_ray = new Ray( _target_pos, _direction_left * _raycast_range );
				if( Physics.Raycast( _ray, out _hit, _range, CoverLayerMask ) ) 
				{
					_ray = new Ray( _hit.point - _hit.normal * _range, _hit.normal );
					if( _hit.collider.Raycast( _ray, out _hit, Mathf.Infinity ) ) 
					{
						Vector3 _pos = _hit.point + ( _hit.normal * _offset );
						float _dist = ( ( Vector3.Distance( _pos, _owner_pos ) + Vector3.Distance( _pos, _target_pos ) ) * 0.5f );
						if( _dist < _best_cover_distance )
						{
							_best_cover_distance = _dist;
							m_CoverMovePosition = _pos;
						}

						if( _debug )
							ICEDebug.DrawLine( _target_pos, m_CoverMovePosition, Color.green );
					}
					else if( _debug )
						ICEDebug.DrawLine( _target_pos, _hit.point, Color.yellow );
				}
				else if( _debug )
					ICEDebug.DrawRay( _target_pos, _direction_left * _raycast_range, Color.red );
				
				_raycast_angle += _raycast_step_angle;
				_direction_right = Quaternion.Euler( 0, _raycast_angle, 0 ) * _direction;
				_direction_left = Quaternion.Euler( 0, - _raycast_angle, 0 ) * _direction;

				if( _raycast_angle <= 90 )
					_raycast_range = _range + Mathf.Pow( _range * MathTools.Normalize( _raycast_angle, 0, 180 ), 1 );
				else
					_raycast_range = _range + Mathf.Pow( _range * ( 1 - MathTools.Normalize( _raycast_angle, 0, 180 ) ), 1 );
			}

			return m_CoverMovePosition;
		}

		public Vector3 UpdateRandomOffset()
		{
			if( UseRandomRect )
				m_RandomizedOffset = PositionTools.GetRandomRectPosition( Vector3.zero, RandomRangeRect, true );
			else if( RandomRange > 0 )
				m_RandomizedOffset = PositionTools.GetRandomCirclePosition( Vector3.zero, RandomRange );
			else
				m_RandomizedOffset = Vector3.zero;
			
			return RandomizedOffset;
		}

		public void UpdateDynamicOffset()
		{
			if( ! Enabled || ( ! UseDynamicOffsetAngle && ! UseDynamicOffsetDistance ) )
				return;

			float _offset_angle = m_OffsetAngle;
			float _offset_distance = m_OffsetDistance;

			if( UseDynamicOffsetDistance && ( MinOffsetDistance > 0 || MaxOffsetDistance > 0 ) )
			{
				if( MinOffsetDistance == MaxOffsetDistance )
					MinOffsetDistance = 0;


				if( MinOffsetDistance < MaxOffsetDistance )
				{
					if( ( m_DynamicOffsetDistanceUpdateSpeed < 0 && _offset_distance <= m_DynamicOffsetDistance ) || 
						( m_DynamicOffsetDistanceUpdateSpeed > 0 && _offset_distance >= m_DynamicOffsetDistance ) )
					{
						if( UseRandomOffsetDistance )
							m_DynamicOffsetDistance = Random.Range( MinOffsetDistance, MaxOffsetDistance );
						else if( m_DynamicOffsetDistance >= MaxOffsetDistance )
							m_DynamicOffsetDistance = MinOffsetDistance;
						else
							m_DynamicOffsetDistance = MaxOffsetDistance;
					}

				}

				m_DynamicOffsetDistanceUpdateSpeed = DynamicOffsetDistanceUpdateSpeed;
				if( _offset_distance >= m_DynamicOffsetDistance )
					m_DynamicOffsetDistanceUpdateSpeed *= -1;

				_offset_distance += m_DynamicOffsetDistanceUpdateSpeed;
			}

			if( UseDynamicOffsetAngle && _offset_distance > 0 )
			{
				if( MinOffsetAngle == MaxOffsetAngle )
					m_DynamicOffsetAngle = MinOffsetAngle;
				else if( MinOffsetAngle < MaxOffsetAngle )
				{
					if( ( m_DynamicOffsetAngleUpdateSpeed < 0 && _offset_angle - DynamicOffsetAngleUpdateSpeed <= m_DynamicOffsetAngle ) || 
						( m_DynamicOffsetAngleUpdateSpeed > 0 && _offset_angle + DynamicOffsetAngleUpdateSpeed >= m_DynamicOffsetAngle ) )
					{
						if( UseRandomOffsetAngle )
							m_DynamicOffsetAngle = Random.Range( MinOffsetAngle, MaxOffsetAngle );
						else if( m_DynamicOffsetAngle >= MaxOffsetAngle )
							m_DynamicOffsetAngle = MinOffsetAngle;
						else
							m_DynamicOffsetAngle = MaxOffsetAngle;
					}

				}
				else
					m_DynamicOffsetAngle = 360;

				m_DynamicOffsetAngleUpdateSpeed = DynamicOffsetAngleUpdateSpeed;
				if( _offset_angle >= m_DynamicOffsetAngle )
					m_DynamicOffsetAngleUpdateSpeed *= -1;

				_offset_angle += m_DynamicOffsetAngleUpdateSpeed;

				if( _offset_angle >= 360 )
					_offset_angle = _offset_angle - 360;
			}

			UpdateOffset( _offset_angle, _offset_distance );
		}
			
		public Vector3 SmoothPosition( Vector3 _current_position, Vector3 _target_position, float _speed, bool _reached )
		{
			if( ! Enabled || SmoothingSpeed == 0 || _target_position == _current_position )
				return _target_position;
		
			float _update_speed = ( UseCreatureSpeed ? _speed + SmoothingSpeed : SmoothingSpeed );
			float _distance = PositionTools.Distance( _target_position, _current_position );

			if( _update_speed < _speed )
				_update_speed = _speed;
			
			_update_speed *= Time.deltaTime;

			if( _distance > _update_speed && _distance > StoppingDistance )
			{
				Vector3 _new_move_position = _target_position;

				if( _reached )
					_new_move_position = PositionTools.GetPositionByDirectionAndRadius( _current_position, _target_position - _current_position, StoppingDistance );
				else
					_new_move_position = PositionTools.GetPositionByDirectionAndRadius( _current_position, _target_position - _current_position, _update_speed );

				_target_position = _new_move_position;
			}
		
			return _target_position;
		}

	}

	//--------------------------------------------------
	// TargetDataObject is the data container for TargetObject
	//--------------------------------------------------
	[System.Serializable]
	public class TargetDataObject : EntityDataObject //, ISerializationCallbackReceiver
	{
		public TargetDataObject(){}
		public TargetDataObject( TargetType _type ){ m_Type = _type; }
		public TargetDataObject( TargetDataObject _data ) : base( _data ) { Copy( _data ); }
		public TargetDataObject( TargetDataObject _data, ICEWorldBehaviour _component ) : base( _data ) { SetOwnerComponent( _component ); Copy( _data ); }


		//TODO: UPDATE OBSOLETE VALUES
		//public void OnBeforeSerialize(){}
		//public void OnAfterDeserialize(){}

		public void Copy( TargetDataObject _data )
		{
			if( _data == null )
				return;
			
			base.Copy( _data );

			InfoText = _data.InfoText;

			SetIsPrefab( _data.IsPrefab );

			AccessType = _data.AccessType;

			OverrideTargetGameObject( _data.TargetGameObject );
			TargetName = _data.TargetName;
			TargetTag = _data.TargetTag;
			TargetEntityType = _data.TargetEntityType;
				
			UseTargetAttributes = _data.UseTargetAttributes;

			GroupMessage.Copy( _data.GroupMessage );
			Influences.Copy( _data.Influences );
			Selectors.Copy( _data.Selectors );
			Move.Copy( _data.Move );
			Events.Copy( _data.Events );
			Behaviour.Copy( _data.Behaviour );
		}

		private GameObject m_Owner = null;
		private ICEWorldBehaviour m_OwnerComponent = null;

		[XmlIgnore]
		public GameObject Owner{
			get{ return m_Owner; }
		}

		[XmlIgnore]
		public Transform OwnerTransform{
			get{ return ( m_Owner != null ? m_Owner.transform : null ); }
		}

		[XmlIgnore]
		public ICEWorldBehaviour OwnerComponent{
			get{ return m_OwnerComponent; }
		}

		public void SetOwnerComponent( ICEWorldBehaviour _component ){
			m_OwnerComponent = _component;
			m_Owner = ( m_OwnerComponent != null ? m_OwnerComponent.gameObject:null );
		}


		public bool OwnerIsReady( ICEWorldBehaviour _component ){

			if( _component != null && _component != m_OwnerComponent )
			{
				m_OwnerComponent = _component;
				m_Owner = ( m_OwnerComponent != null ? m_OwnerComponent.gameObject:null );
			}

			return ( m_OwnerComponent != null || m_Owner != null ? true : false ); 
		}

		//public string BehaviourModeKeyApproach;
		//public string BehaviourModeKeyAction;

		[SerializeField]
		private BehaviourEventsObject m_Events = null;
		public BehaviourEventsObject Events{
			get{ return m_Events = ( m_Events == null ? new BehaviourEventsObject() : m_Events ); }
			set{ Events.Copy( value ); }
		}

		[SerializeField]
		private BroadcastMessageObject m_GroupMessage = null;
		public BroadcastMessageObject GroupMessage{
			get{ return m_GroupMessage = ( m_GroupMessage == null ? new BroadcastMessageObject() : m_GroupMessage ); }
			set{ GroupMessage.Copy( value ); }
		}

		[SerializeField]
		private SelectionCriteriaObject m_Selectors = null;
		public SelectionCriteriaObject Selectors{
			get{ return m_Selectors = ( m_Selectors == null ? new SelectionCriteriaObject() : m_Selectors ); }
			set{ Selectors.Copy( value ); }
		}

		[SerializeField]
		private TargetMoveObject m_Move = null;
		public TargetMoveObject Move{
			get{ return m_Move = ( m_Move == null ? new TargetMoveObject() : m_Move ); }
			set{ Move.Copy( value ); }
		}

		[SerializeField]
		private TargetBehaviourObject m_Behaviour = null;
		public TargetBehaviourObject Behaviour{
			get{ return m_Behaviour = ( m_Behaviour == null ? new TargetBehaviourObject() : m_Behaviour ); }
			set{ Behaviour.Copy( value ); }
		}

		[SerializeField]
		private InfluenceObject m_Influences = null;
		public InfluenceObject Influences{
			get{ return m_Influences = ( m_Influences == null ? new InfluenceObject():m_Influences ); }
			set{ Influences.Copy( value ); }
		}

		public TargetAccessType AccessType = TargetAccessType.NAME;

		[SerializeField]
		private string m_TargetTag = "";
		public string TargetTag{
			set{ if( AccessType == TargetAccessType.TAG ) SetTargetByTag( value ); }
			get{ return m_TargetTag = ( string.IsNullOrEmpty( m_TargetTag ) && m_EntityGameObject != null ? m_EntityGameObject.tag : m_TargetTag ); }
		}

		[SerializeField]
		private string m_TargetName = "";
		public string TargetName{
			set{ if( AccessType == TargetAccessType.NAME ) SetTargetByName( value ); }
			get{ return m_TargetName = ( string.IsNullOrEmpty( m_TargetName ) && m_EntityGameObject != null ? m_EntityGameObject.name : m_TargetName ); }
		}

		[SerializeField]
		private EntityClassType m_TargetEntityType = EntityClassType.Undefined;
		public EntityClassType TargetEntityType{
			set{ if( AccessType == TargetAccessType.TYPE ) SetTargetByType( value ); }
			get{ return m_TargetEntityType = ( m_TargetEntityType == EntityClassType.Undefined || ( EntityComponent != null && ! EntityComponent.CompareType( m_TargetEntityType ) ) ? EntityType : m_TargetEntityType ); }
		}
			
			
		/// <summary>
		/// Gets the name of the target parent.
		/// </summary>
		/// <value>The name of the target parent.</value>
		public string TargetParentName{
			get{ return ( m_EntityGameObject != null && m_EntityGameObject.transform.parent != null ? m_EntityGameObject.transform.parent.name : "" ); }
		}
			
		/// <summary>
		/// Gets a value indicating whether this <see cref="ICE.Creatures.Objects.TargetDataObject"/> target has parent.
		/// </summary>
		/// <value><c>true</c> if target has parent; otherwise, <c>false</c>.</value>
		public bool TargetHasParent{
			get{ return ( m_EntityGameObject != null && m_EntityGameObject.transform.parent != null ? true : false ); }
		}

		protected bool m_TargetMoveComplete = false;
		public bool TargetMoveComplete{
			get{ return m_TargetMoveComplete; }
		}

		[SerializeField]
		protected bool m_UseTargetAttributes = false;
		public bool UseTargetAttributes{
			get{ return m_UseTargetAttributes; }
			set{ m_UseTargetAttributes = value; }
		}

		private int m_LastTargetID = 0;
		public int LastTargetID{
			get{ return m_LastTargetID; }
		}
		private int m_TargetID = 0;
		public int TargetID{
			get{return m_TargetID = ( m_TargetID == 0 && m_EntityGameObject != null ? m_EntityGameObject.GetInstanceID() : m_TargetID );}
		}

		/// <summary>
		/// Gets the target GameObject.
		/// </summary>
		/// <value>The target GameObject.</value>
		public GameObject TargetGameObject{
			get{ return m_EntityGameObject; }
		}

		protected float m_ActiveTimeTotal = 0;
		public float ActiveTimeTotal{
			get{ return m_ActiveTimeTotal; }
		}

		protected float m_ActiveTime = 0;
		public float ActiveTime{
			get{ return m_ActiveTime; }
		}

		protected bool m_Active = false;
		public bool Active{
			get{ return m_Active; }
		}

		/// <summary>
		/// Sets the target by tag.
		/// </summary>
		/// <param name="_tag">Tag.</param>
		/// <param name="_owner">Owner.</param>
		public void SetTargetByTag( string _tag, GameObject _owner = null )
		{
			if( ! string.IsNullOrEmpty( _tag ) && _tag != "Undefined" )
			{
				AccessType = TargetAccessType.TAG;

				if( Application.isPlaying == false )
					OverrideTargetGameObject( CreatureRegister.GetReferenceGameObjectByTag( _tag ) );
				else if( _owner != null )
					OverrideTargetGameObject( CreatureRegister.FindBestObjectByTag( _owner, _tag, Selectors.SelectionRange, Selectors.Preselection.ActiveCounterpartsLimit, Selectors.Preselection.PreferActiveCounterparts, Selectors.Preselection.UseChildObjects ) );
			}
			else
			{
				SetEntityGameObject( null );
				m_TargetName = "";
				m_TargetTag = "Untagged";	
				m_TargetID = 0;
				m_TargetEntityType = EntityClassType.Undefined;
			}
		}

		/// <summary>
		/// Sets the name of the target by.
		/// </summary>
		/// <param name="_name">Name.</param>
		/// <param name="_owner">Owner.</param>
		public void SetTargetByName( string _name, GameObject _owner = null )
		{
			if( ! string.IsNullOrEmpty( _name ) )
			{
				AccessType = TargetAccessType.NAME;

				if( Application.isPlaying == false )
					OverrideTargetGameObject( CreatureRegister.GetReferenceGameObjectByName( _name ) );
				else if( _owner != null )
					OverrideTargetGameObject( CreatureRegister.FindBestObjectByName( _owner, _name, Selectors.SelectionRange, Selectors.Preselection.ActiveCounterpartsLimit, Selectors.Preselection.PreferActiveCounterparts, Selectors.Preselection.UseChildObjects ) );
			}
			else
			{
				SetEntityGameObject( null );
				m_TargetName = "";
				m_TargetTag = "Untagged";	
				m_TargetID = 0;
				m_TargetEntityType = EntityClassType.Undefined;
			}
		}

		public void SetTargetByType( EntityClassType _type, GameObject _owner = null )
		{
			AccessType = TargetAccessType.TYPE;

			if( Application.isPlaying == false )
				OverrideTargetGameObject( CreatureRegister.GetReferenceGameObjectByType( _type ) );
			else if( _owner != null )
				OverrideTargetGameObject( CreatureRegister.FindBestObjectByType( _owner, _type, Selectors.SelectionRange, Selectors.Preselection.ActiveCounterpartsLimit, Selectors.Preselection.PreferActiveCounterparts, Selectors.Preselection.UseChildObjects ) );

			if( EntityComponent != null && EntityComponent.CompareType( _type ) )
				m_TargetEntityType = _type;
			else
				m_TargetEntityType = EntityClassType.Undefined;
		}

		/// <summary>
		/// Sets the target by game object.
		/// </summary>
		/// <param name="_object">Object.</param>
		public void SetTargetByGameObject( GameObject _object )
		{
			if( TargetGameObject != _object )
				Selectors.ResetStatus();
			
			SetEntityGameObject( _object );
			if( EntityGameObject != null )
			{
				AccessType = TargetAccessType.OBJECT;
				m_TargetName = m_EntityGameObject.name;
				m_TargetTag = m_EntityGameObject.tag;	
				m_TargetEntityType = EntityType;

				if( ICECreatureRegister.Instance != null )
					ICECreatureRegister.Instance.AddReference( m_EntityGameObject );
			}
			else
			{
				m_TargetName = "";
				m_TargetTag = "Untagged";
				m_TargetID = 0;
				m_TargetEntityType = EntityClassType.Undefined;
			}	
		}

		public GameObject TestOverrideTargetGameObject( GameObject _object )
		{
			if( _object == null )
				return TargetGameObject;

			SetEntityGameObject( _object );

			if( m_EntityGameObject != null )
			{
				m_TargetName = m_EntityGameObject.name;
				m_TargetTag = m_EntityGameObject.tag;	
				m_TargetID = m_EntityGameObject.GetInstanceID();
				m_TargetEntityType = EntityType;

				if( m_TargetID != m_LastTargetID )
				{
					m_LastTargetID = m_TargetID;
				}
			}

			return TargetGameObject;
		}

		/// <summary>
		/// Overrides the TargetGameObject.
		/// </summary>
		/// <returns>The TargetGameObject.</returns>
		/// <param name="_object">GameObject.</param>
		public GameObject OverrideTargetGameObject( GameObject _object )
		{
			//if( _object == null )
			//	return TargetGameObject;

			//CompareGameObject( _object );

			if( Application.isPlaying && _object != null && ! _object.activeInHierarchy )
				_object = null;

			if( TargetGameObject != _object )
			{
				if( Active && EntityComponent != null )
					EntityComponent.RemoveActiveCounterpart( OwnerComponent as ICECreatureEntity );
			}
				
			SetEntityGameObject( _object );
	
			if( TargetGameObject != null )
			{
				if( Active && EntityComponent != null )
					EntityComponent.AddActiveCounterpart( OwnerComponent as ICECreatureEntity );

				m_TargetName = m_EntityGameObject.name;
				m_TargetTag = m_EntityGameObject.tag;	
				m_TargetID = m_EntityGameObject.GetInstanceID();
				m_TargetEntityType = EntityType;

				if( m_TargetID != m_LastTargetID )
				{
					m_LastTargetID = m_TargetID;
					Selectors.ResetStatus();
				}
			}

			return TargetGameObject;
		}




		[SerializeField]
		private TargetType m_Type = TargetType.UNDEFINED;
		public TargetType Type{ get{ return m_Type; } }
		public void SetType( TargetType _type ){ m_Type = _type;}

		// Editor helper
		[SerializeField]
		private bool m_IsPrefab = false;
		public bool IsPrefab{ get{ return m_IsPrefab; } }
		public void SetIsPrefab( bool _value ){ m_IsPrefab = _value; }

		public string TargetTitle{
			get{

				if( AccessType == TargetAccessType.TAG )
					return TargetTag;
				else if( AccessType == TargetAccessType.NAME )
					return TargetName;
				else if( TargetGameObject != null )
					return TargetGameObject.name;
				else
					return "INVALID";
			}
		}
		//public BroadcastMessageObject LastReceivedGroupMessage = null;

	}

	//--------------------------------------------------
	// TargetObject is the data container for all potential targets (home, escort, defender, attacker)
	//--------------------------------------------------
	[System.Serializable]
	public class TargetObject : TargetDataObject
	{
		public TargetObject() : base() {}
		public TargetObject( TargetType _type ) : base( _type ){}
		public TargetObject( TargetObject _target ) : base( _target as TargetDataObject ){}
		public TargetObject( TargetObject _target, ICEWorldBehaviour _component ) : base( _target as TargetDataObject, _component ){}
		public TargetObject( TargetDataObject _data ) : base( _data ){}




		private Vector3 m_TargetMovePositionOffset = Vector3.zero;
		public Vector3 TargetMovePositionOffset{
			get{ return m_TargetMovePositionOffset; }
		}

		//private float m_TargetMovePositionLevel = 0;
		private Vector3 m_DesiredTargetMovePosition = Vector3.zero;
		private Vector3 m_TargetMovePosition = Vector3.zero;

		private Vector3 m_LastTargetPosition = Vector3.zero;

		private Vector3 m_TargetDirection = Vector3.zero;
		public Vector3 TargetDirection{
			get{ return m_TargetDirection; }
		}

		private Vector3 m_TargetVelocity = Vector3.zero;
		public Vector3 TargetVelocity{
			get{ return m_TargetVelocity; }
		}

		private float m_TargetSpeed = 0.0f;
		public float TargetSpeed{
			get{ return m_TargetSpeed; }
		}
			
		/// <summary>
		/// Reset this instance.
		/// </summary>
		public override void Reset()
		{
			base.Reset();

			if( ! Application.isPlaying )
				return;

			if( Active )
				SetActive( false );

			// Resets the target move positions
			m_TargetMovePosition = Vector3.zero;
			UpdateTargetMovePositionOffset( true );
		}

		/// <summary>
		/// Resets the target game object.
		/// </summary>
		public void ResetTargetGameObject()
		{
			if( AccessType == TargetAccessType.OBJECT )
				return;
			
			// TODO: if AccessType is OBJECT it's important to change it to NAME or TAG otherwise the Target will invalid 
			//if( AccessType == TargetAccessType.OBJECT )
			//	AccessType = TargetAccessType.NAME;

			SetEntityGameObject( null );
			Selectors.ResetStatus();
			SetActive( false );

		}

		protected override void DoEntityGameObjectChanged(){
			if( Move.UseUpdateOffsetOnTargetChanged )
				UpdateTargetMovePositionOffset( true );
		}

		public string GetBestBehaviourModeKey( GameObject _owner )
		{
			if( Behaviour.IsSelective )
			{
				return Behaviour.BehaviourModeKey;
			}
			else if( Behaviour.IsAdvanced )
			{
				if( TargetMovePositionReached( _owner.transform.position ) )
					return Behaviour.BehaviourModeKeyReached;
				else
					return Behaviour.BehaviourModeKey;
			}
			else
			{
				return Behaviour.BehaviourModeKey;
			}				
		}

		public bool TargetPreselectionRecheck()
		{
			if( ! Selectors.Preselection.UseTargetRecheck || Owner == null || ! IsValidAndReady )
				return true;

			Transform _owner_transform = Owner.transform;

			if( TargetDistanceTo( _owner_transform.position ) <= Selectors.FixedSelectionRange )
			{
				if( Selectors.Preselection.UseChildObjects || TargetTransform.IsChildOf( _owner_transform ) == false )
				{
					ICECreatureEntity _target_entity = EntityComponent;
					ICECreatureEntity _owner_entity = OwnerComponent as ICECreatureEntity;

					int _cp_limit = Selectors.Preselection.ActiveCounterpartsLimit;
					int _cp_count = _target_entity.ActiveCounterparts.Count;
		
					if( 
						( Selectors.Preselection.PreferActiveCounterparts == false || 
							Active == false || 
							_owner_entity == null || 
							_owner_entity.ActiveCounterpartExists( _target_entity ) 
						) &&
						( Selectors.Preselection.ActiveCounterpartsLimit == -1 || 
							_target_entity == null || 
							( Active == true && _cp_count <= _cp_limit + 1 && _target_entity.ActiveCounterpartExists( _owner_entity ) ) || 
							( Active == false && _cp_count <= _cp_limit ) ) )
					{
						return true; 
					}
				}
			}
				
			return false;
		}

		/// <summary>
		/// Gets the best TargetGameObject.
		/// </summary>
		/// <returns>The best TargetGameObject.</returns>
		/// <param name="_owner">Owner.</param>
		/// <param name="_distance">Distance.</param>
		public GameObject PrepareTargetGameObject( ICEWorldBehaviour _component )
		{
			if( ! OwnerIsReady( _component ) )
				return null;

			if( TargetGameObject == null && string.IsNullOrEmpty( TargetName ) && string.IsNullOrEmpty( TargetTag ) )
				return null;
		
			if( ! TargetPreselectionRecheck() )
			{
				if( EntityComponent != null )
				{
					if( Active )
						EntityComponent.RemoveActiveCounterpart( OwnerComponent as ICECreatureEntity );
				}

				SetEntityGameObject( null );
			}	
			
			if( Selectors.Preselection.RefreshRequired() || ! IsValidAndReady )
			{
				if( Selectors.Preselection.PreferActiveCounterparts )
				{
					ICECreatureEntity _owner_entity = OwnerComponent as ICECreatureEntity;
					if( _owner_entity != null )
					{
						ICECreatureEntity _active_entity = null;

						if( AccessType == TargetAccessType.NAME )
							_active_entity = _owner_entity.GetBestCounterpartByName( TargetName, Selectors.Preselection.ActiveCounterpartsLimit, Selectors.Preselection.UseChildObjects );
						else if( AccessType == TargetAccessType.TAG )
							_active_entity = _owner_entity.GetBestCounterpartByTag( TargetTag, Selectors.Preselection.ActiveCounterpartsLimit, Selectors.Preselection.UseChildObjects );
						else if( AccessType == TargetAccessType.TYPE )
							_active_entity = _owner_entity.GetBestCounterpartByType( TargetEntityType, Selectors.Preselection.ActiveCounterpartsLimit, Selectors.Preselection.UseChildObjects );

						if( _active_entity != null )
							return OverrideTargetGameObject( _active_entity.gameObject );
					}
				}

				GameObject _object = null;

				if( Selectors.Preselection.UseAllAvailableObjects )
				{
					GameObject[] _objects = null;

					if( AccessType == TargetAccessType.NAME || ( AccessType == TargetAccessType.OBJECT && TargetGameObject == null ) )
						_objects = CreatureRegister.FindObjectsByName( TargetName );
					else if( AccessType == TargetAccessType.TAG )
						_objects = CreatureRegister.FindObjectsByTag( TargetTag );
					else if( AccessType == TargetAccessType.TYPE )
						_objects = CreatureRegister.FindObjectsByType( EntityType );

					if( _objects != null )
					{
						Selectors.SetAlternateObjectsCount( _objects.Length );

						SelectorObject _selector = new SelectorObject( OwnerComponent );
						_object = _selector.SelectBestGameObject( this, _objects );
						_selector = null;
					}
				}
				else
				{
					if( AccessType == TargetAccessType.NAME || ( AccessType == TargetAccessType.OBJECT && TargetGameObject == null ) )
						_object = CreatureRegister.FindBestObjectByName( Owner, TargetName, Selectors.FixedSelectionRange, Selectors.Preselection.ActiveCounterpartsLimit, Selectors.Preselection.PreferActiveCounterparts, Selectors.Preselection.UseChildObjects );
					else if( AccessType == TargetAccessType.TAG )
						_object = CreatureRegister.FindBestObjectByTag( Owner, TargetTag, Selectors.FixedSelectionRange, Selectors.Preselection.ActiveCounterpartsLimit, Selectors.Preselection.PreferActiveCounterparts, Selectors.Preselection.UseChildObjects );
					else if( AccessType == TargetAccessType.TYPE )
						_object = CreatureRegister.FindBestObjectByType( Owner, TargetEntityType, Selectors.FixedSelectionRange, Selectors.Preselection.ActiveCounterpartsLimit, Selectors.Preselection.PreferActiveCounterparts, Selectors.Preselection.UseChildObjects );
					else
						_object = TargetGameObject;
				}

				// will override and return the new TargetGameObject if the specified object isn't null, otherwise it will return the given TargetGameObject
				return OverrideTargetGameObject( _object );
			}
			else
				return TargetGameObject;
		}

		/// <summary>
		/// Gets all target game objects.
		/// </summary>
		/// <description>GetAllTargetGameObjects will be used e.g. by the debug class to get all interactors</description>
		/// <returns>All target game objects according to the given AccessType</returns>
		/// <param name="_owner">Owner.</param>
		public List<GameObject> GetAllTargetGameObjects( GameObject _owner )
		{
			if( ICECreatureRegister.Instance == null )
				return null;
			
			List<GameObject> _objects = null;
			
			if( AccessType == TargetAccessType.NAME || ( AccessType == TargetAccessType.OBJECT && TargetGameObject == null ) )
				_objects = ICECreatureRegister.Instance.GetActiveObjectsByName( TargetName );
			else if( AccessType == TargetAccessType.TAG )
				_objects = ICECreatureRegister.Instance.GetActiveObjectsByTag( TargetTag );
			else if( AccessType == TargetAccessType.TYPE )
				_objects = ICECreatureRegister.Instance.GetActiveObjectsByType( EntityType );
			
			return _objects;
		}

		/// <summary>
		/// Sets the target default values.
		/// </summary>
		/// <param name="_targets">_targets.</param>
		public void SetTargetDefaultValues( List<TargetObject> _targets )
		{
			if( _targets == null || _targets.Count == 0 )
				return;

			// makes a random copy of the target settings   
			Copy( _targets[ Random.Range( 0, _targets.Count ) ] );
		}

		/// <summary>
		/// Reads the target default values.
		/// </summary>
		/// <returns>A list of target objects with default values based on the ICECreatureTarget scripts of the given target</returns>
		public List<TargetObject> ReadTargetAttributeData()
		{
			if( TargetGameObject == null || TargetGameObject.GetComponent<ICECreatureTargetAttribute>() == null )
				return null;

			ICECreatureTargetAttribute[] _values = TargetGameObject.GetComponents<ICECreatureTargetAttribute>();

			List<TargetObject> _targets = new List<TargetObject>();
			foreach( ICECreatureTargetAttribute _data in _values )
			{
				if( _data != null && _data.Target != null && _data.isActiveAndEnabled == true )
					_targets.Add( _data.Target );
			}

			if( _targets.Count > 0 )
				return _targets;
			else
				return null;
		}

		[XmlIgnore]
		public Transform TargetTransform{
			get{ return ( TargetGameObject != null ? TargetGameObject.transform : null ); }
		}

		private Renderer m_Renderer = null;
		public Renderer TargetRenderer()
		{
			if( TargetGameObject == null )
				return null;

			if( m_Renderer == null )
				m_Renderer = TargetGameObject.GetComponentInChildren<Renderer>();

			return m_Renderer;
		}
			
		/// <summary>
		/// Gets the age of the target entity object or 0.
		/// </summary>
		/// <value>The age of the target entity object or 0.</value>
		public float Age{
			get{ return ( EntityComponent != null ? EntityComponent.Age : 0 ); }
		}

		public InventoryObject Inventory()
		{
			if( TargetGameObject == null )
				return null;

			InventoryObject _inventory = new InventoryObject();

			if( EntityCreature != null )
				_inventory.Copy( EntityCreature.Creature.Status.Inventory );
			else if( EntityItem != null )
				_inventory.Copy( EntityItem.Inventory );
			else if( EntityPlayer != null )
				_inventory.Copy( EntityPlayer.Inventory );
	
			//if( ! _inventory.IgnoreInventoryOwner )
			_inventory.Insert( TargetGameObject.name, 1 );

			return _inventory;
		}

		public OdourObject Odour()
		{
			if( TargetGameObject == null )
				return null;

			OdourObject _odour = new OdourObject();

			if( EntityComponent != null )
				_odour.Copy( EntityComponent.Status.Odour );

			return _odour;
		}

		public bool TargetIsDead{
			get{
				if( EntityCreature != null )
					return EntityCreature.Creature.Status.IsDead;
				else
					return false;
			}
		}

		public MoveType TargetMoveType{
			get{
				MoveType _move = MoveType.DEFAULT;

				switch( Type )
				{
					case TargetType.INTERACTOR:
						_move = MoveType.DEFAULT;
						break;
					case TargetType.OUTPOST:
						_move = MoveType.RANDOM;
						break;
					case TargetType.ESCORT:
						_move = MoveType.DEFAULT;
						break;
					case TargetType.PATROL:
						_move = MoveType.DEFAULT;
						break;
					case TargetType.WAYPOINT:
						_move = MoveType.DEFAULT;
						break;
					default:
						_move = MoveType.DEFAULT;
						break;
					
				}
				return _move;
			}

		}



		public int SelectionPriority{
			get{ return Selectors.GetPriority( Type ); }
		}
			
		public bool TargetTimeCheckComplied( CreatureObject _creature, bool _final_result )
		{
			if( _creature.ActiveTarget == null || _creature.ActiveTarget.IsValidAndReady == false )
				return _final_result;

			bool _is_active = CompareTarget( _creature.ActiveTarget );

			if( _final_result )
			{
				Selectors.RetainingTimer.Reset();
				return Selectors.DelayTimer.Update( _is_active );
			}
			else
			{
				Selectors.DelayTimer.Reset();
				return ! Selectors.RetainingTimer.Update( ! _is_active );
			}

		}

		public bool TargetBasicCheckComplied( CreatureObject _creature )
		{
			if( _creature == null || ! IsValidAndReady )
				return false;

			if( TargetInSelectionArea( _creature.Owner.transform ) &&	
				( _creature.Status.Sensoria.Enabled == false || 
					( ! Selectors.UseFieldOfView && ! Selectors.UseVisibilityCheck && ! Selectors.UseAudibleCheck && ! Selectors.UseOdourCheck ) ||
					( 
						( 
							( 
								Selectors.UseFieldOfView && 
								Selectors.UseVisibilityCheck && 
								TargetInFieldOfView( _creature.Status ) && 
								TargetIsVisible( _creature.Status ) 
							) ||
							( 
								Selectors.UseFieldOfView && 
								! Selectors.UseVisibilityCheck && 
								TargetInFieldOfView( _creature.Status ) 
							) ||
							( 
								! Selectors.UseFieldOfView && 
								Selectors.UseVisibilityCheck && 
								TargetIsVisible( _creature.Status ) 
							)
						) ||
						( Selectors.UseOdourCheck && TargetIsSmellable( _creature.Status ) ) || 
						( Selectors.UseAudibleCheck && TargetIsAudible( _creature.Status ) ) 
					) 
				) 
			)
				return true;
			else 
				return false;
		}

		public bool BasicCheckSkipped()
		{
			if( ! Selectors.UseSelectionRange &&
				! Selectors.UseFieldOfView && 
				! Selectors.UseVisibilityCheck && 
				! Selectors.UseOdourCheck &&
				! Selectors.UseAudibleCheck &&
				Selectors.UseAdvanced )
				return true;
			else
				return false;
		}

		public bool CompareTarget( TargetObject _target, int _target_id )
		{
			if( _target == null || _target != this || _target.TargetID != TargetID || _target.TargetID != _target_id )
				return false;
			else
				return true;
		}

		public bool CompareTarget( TargetObject _target )
		{
			if( _target == null || _target != this || _target.TargetID != TargetID )
				return false;
			else
				return true;
		}

		private bool m_IsInFieldOfView = false;
		public bool IsInFieldOfView{
			get{ return (Selectors.UseFieldOfView?m_IsInFieldOfView:true); }
		}

		private bool m_IsVisible = false;
		public bool IsVisible{
			get{ return (Selectors.UseVisibilityCheck?m_IsVisible:true); }
		}

		private bool m_IsAudible = false;
		public bool IsAudible{
			get{ return (Selectors.UseAudibleCheck?m_IsAudible:true); }
		}

		private bool m_IsSmellable = false;
		public bool IsSmellable{
			get{ return (Selectors.UseOdourCheck?m_IsSmellable:true); }
		}

		public float Visibility( StatusObject _status ){
			return ( _status != null ? _status.ObjectVisibility( TargetGameObject ) : 0 );
		}

		public float Smellability( StatusObject _status ){
			return ( _status != null ? _status.ObjectSmellability( TargetGameObject, Odour() ) : 0 );
		}

		public float Audibility( StatusObject _status ){
			return ( _status != null ? _status.ObjectAudibility( TargetGameObject ) : 0 );
		}
			
		public bool TargetInFieldOfView( StatusObject _status ){
			return ( _status != null ? _status.ObjectInFieldOfView( TargetGameObject, ref m_IsInFieldOfView, ref Move.LastKnownPosition ) : false );
		}

		/// <summary>
		/// Target is smellable.
		/// </summary>
		/// <returns><c>true</c>, if is smellable was targeted, <c>false</c> otherwise.</returns>
		/// <param name="_status">Status.</param>
		public bool TargetIsSmellable( StatusObject _status ){
			return ( _status != null ? _status.ObjectIsSmellable( TargetGameObject, ref m_IsSmellable, ref Move.LastKnownPosition, Odour() ) : false );
		}

		/// <summary>
		/// Target is audible.
		/// </summary>
		/// <returns><c>true</c>, if is audible was targeted, <c>false</c> otherwise.</returns>
		/// <param name="_status">Status.</param>
		public bool TargetIsAudible( StatusObject _status ){
			return ( _status != null ? _status.ObjectIsAudible( TargetGameObject, ref m_IsAudible, ref Move.LastKnownPosition ) : false );
		}

		/// <summary>
		/// Target is visible.
		/// </summary>
		/// <returns><c>true</c>, if is visible was targeted, <c>false</c> otherwise.</returns>
		/// <param name="_status">Status.</param>
		public bool TargetIsVisible( StatusObject _status ){
			return ( _status != null ? _status.ObjectIsVisible( TargetGameObject, ref m_IsVisible, ref Move.LastKnownPosition ) : false );
		}
			
		/// <summary>
		/// Checks if the target is within the specified selection area.
		/// </summary>
		/// <returns><c>true</c>, if the target is in selection area, <c>false</c> otherwise.</returns>
		/// <param name="_transform">Transform.</param>
		public bool TargetInSelectionArea( Transform _transform )
		{
			if( IsValidAndReady == false || _transform == null )
				return false;

			// if there are no custom criteria for home, the home target is always available ...
			if( Type == TargetType.HOME && Selectors.Enabled == false )
				return true;

			// unlimited - the target is always available
			if( ( ! Selectors.UseSelectionRange || Selectors.SelectionRange == 0 ) && 
				( ! Selectors.UseSelectionAngle || Selectors.SelectionAngle <= 0 || Selectors.SelectionAngle >= 180 ) )
				return true;
			else if( ( ! Selectors.UseSelectionRange || TargetDistanceTo( _transform.position ) <= Selectors.FixedSelectionRange ) &&
				( ! Selectors.UseSelectionAngle || Selectors.FixedSelectionAngle == 0 || Mathf.Abs( PositionTools.GetSignedDirectionAngle( TargetGameObject.transform, _transform.position ) ) <= Selectors.FixedSelectionAngle ) )
				return true;
			else
				return false;
				

			/*
			// additional to the range we have also to check the correct field of view angle
			if( Selectors.UseSelectionAngle && Selectors.SelectionAngle > 0 && Selectors.SelectionAngle < 180 )
			{
				float _angle = PositionTools.GetSignedDirectionAngle( TargetGameObject.transform, _transform.position );

				if( _distance <= Selectors.SelectionRange && Mathf.Abs( _angle ) <= Selectors.SelectionAngle )
					return true;
				else
					return false;
			}

			// here we will check the distance only
			else if( Selectors.UseSelectionRange && _distance <= Selectors.SelectionRange )
				return true;

			else
				return false;*/
		}

		//public bool UseOffsetAngle = false;



		public void SetActive( ICEWorldBehaviour _component )
		{
			if( ! OwnerIsReady( _component ) )
				return;				

			SetActive( true );
		}

		/// <summary>
		/// Activates or deactives the target
		/// </summary>
		/// <param name="_value">If set to <c>true</c> value.</param>
		public void SetActive( bool _value )
		{
			if( m_Active == false && _value == true )
			{
				UpdateTargetMovePositionOffset( Move.UseUpdateOffsetOnActivateTarget );

				Events.Start( Owner, TargetGameObject );
				Influences.Start();

			}
			else if( _value == false )
			{
				Events.Stop();
				Influences.Stop( Owner );
			}

			if( m_Active != _value )
				m_ActiveTime = 0;

			m_Active = _value;
		}

		public bool IsValid{
			get{ return ( TargetGameObject == null ? false : true ); }
		}

		public bool IsValidAndReady{
			get{ return ( TargetGameObject == null || TargetGameObject.activeInHierarchy == false ? false : true ); }
		}

		public bool TargetIsChildOf( Transform _transform ){
			return ( _transform != null && TargetTransform != null ? TargetTransform.IsChildOf( _transform ) : false );
		}




		/// <summary>
		/// Gets the target transform position.
		/// </summary>
		/// <value>The target transform position.</value>
		public Vector3 TargetTransformPosition{
			get{ return ( TargetGameObject != null ? TargetGameObject.transform.position : Vector3.zero ); }
		}

		/// <summary>
		/// Gets the target transform forward.
		/// </summary>
		/// <value>The target transform forward.</value>
		public Vector3 TargetTransformForward{
			get{ return( TargetGameObject != null ? TargetGameObject.transform.forward : Vector3.forward ); }
		}

		/// <summary>
		/// Gets the target offset position.
		/// </summary>
		/// <value>The target offset position.</value>
		public Vector3 TargetOffsetPosition{ 
			get{ return ( TargetGameObject != null ? PositionTools.FixTransformPoint( TargetTransform, Move.Offset ) : Vector3.zero ); }
		}

		public float TargetOffsetAngle{
			get{ return ( TargetGameObject != null ? MathTools.NormalizeAngle( Move.OffsetAngle + TargetTransform.eulerAngles.y ) : 0 ); }
		}
			
		public Vector3 DesiredTargetMovePosition{
			get{ 
				if( ! IsValid )
					return Vector3.zero;
	
				if( ! Move.Enabled )
				{
					m_DesiredTargetMovePosition = TargetTransformPosition;
				}
				else if( Move.MovePositionType == TargetMovePositionType.LastKnownPosition )
				{
					m_DesiredTargetMovePosition = TargetLastKnownPosition;
				}
				else if( Move.MovePositionType == TargetMovePositionType.Range && Owner != null )
				{
					Vector3 _position = TargetTransformPosition;
					Vector3 _heading = Owner.transform.position - TargetTransformPosition;
					float _angle = PositionTools.GetNormalizedVectorAngle( _heading );


					//_angle += Random.Range( - CurrentMove.Escape.RandomEscapeAngle, CurrentMove.Escape.RandomEscapeAngle );
					m_DesiredTargetMovePosition  = PositionTools.GetPositionByAngleAndRadius( _position, _angle, Move.FixedRange );
				}
				else if( Move.MovePositionType == TargetMovePositionType.Cover && Owner != null )
				{			
					m_DesiredTargetMovePosition = TargetTransformPosition;
				}
				else if( Move.MovePositionType == TargetMovePositionType.OtherTarget && ! string.IsNullOrEmpty( Move.MoveTargetName ) )
				{
					GameObject _object = CreatureRegister.FindBestObjectByName( Owner, Move.MoveTargetName , Mathf.Infinity, -1, false, false );

					if( _object != null )
						m_DesiredTargetMovePosition = _object.transform.position;
				}
				else
				{
					m_DesiredTargetMovePosition = PositionTools.FixTransformPoint( TargetGameObject.transform, TargetMovePositionOffset );
					m_DesiredTargetMovePosition.y = TargetGameObject.transform.position.y;
				}
					
				return VerifiedDesiredTargetMovePosition( m_DesiredTargetMovePosition );
			}
		}

		/// <summary>
		/// Checks the position.
		/// </summary>
		/// <returns>The position.</returns>
		/// <param name="_position">Position.</param>
		public Vector3 VerifiedDesiredTargetMovePosition( Vector3 _position )
		{
			// if the desired position is within the stopping distance of the target we can return the position as it is
			if( Move.Enabled == false || Move.UseVerifiedDesiredTargetMovePosition == false )//|| Vector3.Distance( _position, TargetTransformPosition ) <= Move.StoppingDistance )
				return _position;

			Collider[] _colliders = Physics.OverlapSphere( _position, Move.StoppingDistance , Move.OverlapPreventionLayerMask );

			int i = 0;
			while( i < _colliders.Length ) 
			{
				Collider _collider = _colliders[i];

				if( _collider != null && ( Owner != null && ! _collider.gameObject.transform.IsChildOf( Owner.transform ) ) && ( TargetTransform != null && ! _collider.gameObject.transform.IsChildOf( TargetTransform ) ) )
				{
					// try to get the closest point but this could fail for some reasons ...
					Vector3 _point = _collider.ClosestPointOnBounds( _position ); 

					// ... so compare the points and try to get the closest point on another way ...
					if( ( _point - _position ).magnitude < 0.05f )
						_point = SystemTools.ClosestPointOnSurface( _collider, _position );

					if( ( _point - _position ).magnitude < 0.01f )
						_point = _collider.transform.position;

					_point.y = _position.y;

					Vector3 _dir = _point - _position;

					_position += _dir - ( _dir.normalized * Move.StoppingDistance );
				}
				i++;
			}

			return _position;
		}
			
		public Vector3 TargetLastKnownPosition{
			get{ return ( Move.LastKnownPosition != Vector3.zero && ! m_IsVisible && ! m_IsAudible && ! m_IsSmellable ? Move.LastKnownPosition : TargetTransformPosition ); }
		}

		/// <summary>
		/// Overwrites the final TargetMovePosition and adapts the RandomizedOffset. Call this method to overwrite the
		/// TargetMovePosition if the given value isn't suitable to your needs. Info: this method will be called if the 
		/// given position isn't on the nav mesh, here the position will be overwritten by the NavMesh.SamplePosition
		/// </summary>
		/// <returns>The target move position.</returns>
		/// <param name="_position">Position.</param>
		public Vector3 SetTargetMovePosition( Vector3 _position )
		{
			m_TargetMovePosition = _position;
			Move.SetRandomizedOffset( PositionTools.FixInverseTransformPoint( TargetGameObject.transform, m_TargetMovePosition ) );
			return m_TargetMovePosition;
		}

		/// <summary>
		/// Gets the target move position.
		/// </summary>
		/// <value>The target move position.</value>
		public Vector3 TargetMovePosition{
			get{ return m_TargetMovePosition = ( m_TargetMovePosition == Vector3.zero ? DesiredTargetMovePosition : m_TargetMovePosition ); }
		}
			

		/// <summary>
		/// Target in inside the max range (Random Range + Stopping Distance).
		/// </summary>
		/// <returns><c>true</c>, if the target is in the max range (Random Range + Stopping Distance), <c>false</c> otherwise.</returns>
		/// <param name="position">Position.</param>
		public bool TargetInMaxRange( Vector3 _position ){

			if( Move.UseRandomRect ) 
			{
				Vector3 _origin = TargetOffsetPosition;

				float _x = ( Move.RandomRangeRect.x + Move.StoppingDistance ) * 0.5f;
				float _z = ( Move.RandomRangeRect.z + Move.StoppingDistance ) * 0.5f;

				Vector3[] _points = new Vector3[] { 
					new Vector3( _origin.x - _x , _origin.y , _origin.z - _z ),
					new Vector3( _origin.x - _x , _origin.y , _origin.z + _z ),
					new Vector3( _origin.x + _x , _origin.y , _origin.z + _z ),
					new Vector3( _origin.x + _x , _origin.y , _origin.z - _z ) };

				_points = PositionTools.RotatePointAroundPivot( _origin, _points, TargetOffsetAngle );

				return MathTools.ContainsPoint( _points, _position );
				
			}
			else
				return ( TargetOffsetPositionDistanceTo( _position) <= Move.RandomRange + Move.StoppingDistance ? true : false );
		}

		/// <summary>
		/// Target move position was reached.
		/// </summary>
		/// <returns><c>true</c>, if move position reached was targeted, <c>false</c> otherwise.</returns>
		/// <param name="position">Position.</param>
		public bool TargetMovePositionReached( Vector3 position ){
			return ( TargetMovePositionDistanceTo( position ) <= Move.StoppingDistance ? true : false );
		}

		public bool TargetMovePositionReached(){
			return ( Owner != null && TargetMovePositionDistanceTo( Owner.transform.position ) <= Move.StoppingDistance ? true : false );
		}

		public bool TargetMovePositionReached( GameObject _owner ){
			return ( _owner != null && TargetMovePositionDistanceTo( _owner.transform.position ) <= Move.StoppingDistance ? true : false );
		}

		/// <summary>
		/// Targets the move position distance to.
		/// </summary>
		/// <returns>The move position distance to.</returns>
		/// <param name="_position">Position.</param>
		public float TargetMovePositionDistanceTo( Vector3 _position ){
			return PositionTools.GetDistance( TargetMovePosition, _position, Move.IgnoreLevelDifference );
		}

		/// <summary>
		/// Targets the last known position distance to.
		/// </summary>
		/// <returns>The last known position distance to.</returns>
		/// <param name="_position">Position.</param>
		public float TargetLastKnownPositionDistanceTo( Vector3 _position ){
			return PositionTools.GetDistance( TargetLastKnownPosition, _position, Move.IgnoreLevelDifference );
		}

		/// <summary>
		/// Targets the offset position distance to.
		/// </summary>
		/// <returns>The offset position distance to.</returns>
		/// <param name="position">Position.</param>
		public float TargetOffsetPositionDistanceTo( Vector3 _position ){
			return PositionTools.GetDistance( TargetOffsetPosition, _position, Move.IgnoreLevelDifference );
		}

		/// <summary>
		/// Targets distance between its transform position and the specified position.
		/// </summary>
		/// <returns>The distance to.</returns>
		/// <param name="position">Position.</param>
		public float TargetDistanceTo( Vector3 _position ){
			return PositionTools.GetDistance( TargetTransformPosition, _position, Move.IgnoreLevelDifference );
		}

		/// <summary>
		/// Targets vertical distance between its transform position and the specified position.
		/// </summary>
		/// <returns>The vertical distance to.</returns>
		/// <param name="_position">Position.</param>
		public float TargetVerticalDistanceTo( Vector3 _position ){
			return PositionTools.GetVerticalDistance( TargetTransformPosition, _position );
		}

		/// <summary>
		/// Update
		/// </summary>
		/// <param name="_owner">Owner.</param>
		public void EarlyUpdate( ICEWorldBehaviour _component )
		{
			if( ! OwnerIsReady( _component ) )
				return;
			
			m_TargetMoveComplete = TargetMovePositionReached( Owner.transform.position );

			if( m_Active == true )
			{
			   	m_ActiveTime += Time.deltaTime;
				m_ActiveTimeTotal += Time.deltaTime;
			}
			else
				m_ActiveTime = 0;
								
			Events.Update();
		}

		/// <summary>
		/// Late update.
		/// </summary>
		/// <param name="_owner">Owner.</param>
		public void LateUpdate( GameObject _owner, float _speed ){
			UpdateTargetMovePositionData( _speed );
			UpdateTargetVelocity();

			Selectors.LateUpdate();

		}
			
		/// <summary>
		/// Fixed update.
		/// </summary>
		/// <returns>The update.</returns>
		public void FixedUpdate(){
			
		}

		/// <summary>
		/// Updates target direction, velocity and speed.
		/// </summary>
		private void UpdateTargetVelocity()
		{
			if( ! IsValidAndReady )
				return;

			m_TargetDirection = TargetGameObject.transform.position - m_LastTargetPosition;
			Vector3 _velocity = m_TargetDirection / Time.deltaTime;
			Vector3 _local_velocity = TargetGameObject.transform.InverseTransformDirection( _velocity );

			m_TargetVelocity = Vector3.Lerp( m_TargetVelocity, _local_velocity, 0.5f );
			m_TargetSpeed = Mathf.Lerp( m_TargetSpeed, _local_velocity.z, 0.5f );

			m_LastTargetPosition = TargetGameObject.transform.position;
		}


		public void UpdateTargetMovePositionOffset( bool _randomize ){
			UpdateTargetMovePositionData(0, _randomize );
		}

		private void UpdateTargetMovePositionData( float _speed, bool _randomize = false )
		{
			if( Move.Enabled )
			{
				if( Move.MovePositionType == TargetMovePositionType.Offset )
				{
					Move.UpdateDynamicOffset();
		
					if( Move.UpdateOffsetOnRandomizedTimer() || ( Move.UseUpdateOffsetOnMovePositionReached && TargetMoveComplete ) || _randomize )
						m_TargetMovePositionOffset = Move.UpdateRandomOffset();
					else
						m_TargetMovePositionOffset = Move.RandomizedOffset;

					m_TargetMovePosition = Move.SmoothPosition( m_TargetMovePosition, DesiredTargetMovePosition, _speed, TargetMoveComplete );
				}
				else if( Move.MovePositionType == TargetMovePositionType.Cover )
				{
					
					m_TargetMovePosition = Move.UpdateCoverPosition( OwnerTransform, TargetTransform, ( OwnerComponent != null ? OwnerComponent.DebugRayIsEnabled : false ) );

					if( m_TargetMovePosition == Vector3.zero )
						m_TargetMovePosition = TargetTransformPosition;
				}
				else
					m_TargetMovePosition = TargetTransformPosition;
			}
			else
			{
				m_TargetMovePosition = TargetTransformPosition;
			}
		}
	}

}
