// ##############################################################################
//
// ice_objects_obstacles.cs | GroundAvoidanceObject, ObstacleAvoidanceObject, OverlapPreventionObject
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
	public class GroundAvoidanceObject : ICEOwnerObject
	{
		public GroundAvoidanceObject(){}
		public GroundAvoidanceObject( GroundAvoidanceObject _move ){ Copy( _move );  }

		public void Copy( GroundAvoidanceObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			AvoidWater = _object.AvoidWater;
			UseSlopeLimits = _object.UseSlopeLimits;
			UseSlipping = _object.UseSlipping;
			ScanningRange = _object.ScanningRange;
			ScanningRangeMaximum = _object.ScanningRangeMaximum;
			ScanningAngle = _object.ScanningAngle;
			MaxPathSlopeAngle = _object.MaxPathSlopeAngle;
			MaxSurfaceSlopeAngle = _object.MaxSurfaceSlopeAngle;

		}

		public bool AvoidWater = false;
		public bool UseSlopeLimits = false;
		public bool UseSlipping = false;
		public float ScanningRange = 5;
		public float ScanningRangeMaximum = 10;
		public int ScanningAngle = 10;
		public float MaxPathSlopeAngle = 35;
		public float MaxSurfaceSlopeAngle = 45;


		private float m_VerticalRaycastOffset = 0;

		protected Vector3 m_SlopePathPosition = Vector3.zero;
		public Vector3 SlopePathPosition{
			get{ return m_SlopePathPosition; }
		}

		public Vector3 Scan( Transform _transform, Vector3 _position, LayerMask _ground_mask, LayerMask _water_mask, float _offset )
		{
			if( ScanningRange == 0 || ScanningAngle == 0 || ( ! UseSlopeLimits && ! AvoidWater ) )
				return _position;

			if( Owner == null )
				SetOwner( _transform.gameObject );

			RaycastHit _hit;

			Vector3 _pos = PositionTools.GetDirectionPosition( _transform, 0, ScanningRange );
			_offset += m_VerticalRaycastOffset + ScanningRange;
			_pos.y += _offset;	

			if( UseSlipping && MaxSurfaceSlopeAngle > 0 )
			{
				if( Physics.Raycast( _transform.position + Vector3.up , Vector3.down, out _hit, Mathf.Infinity, _ground_mask, WorldManager.TriggerInteraction ))
				{
					if( Mathf.Abs( Vector3.Angle( _hit.normal, Vector3.up ) ) > MaxSurfaceSlopeAngle )
					{
						m_SlopePathPosition = Vector3.zero;
						_transform.position += new Vector3( _hit.normal.x * 9.8f * Time.deltaTime, 0, _hit.normal.z * 9.8f * Time.deltaTime  );
						return _position;
					}		
				}
			}

			if( m_SlopePathPosition == Vector3.zero || PositionTools.Distance( _transform.position, m_SlopePathPosition ) < ScanningRange * 0.25f )
			{		
				LayerMask _combined_mask = _ground_mask;

				if( _water_mask.value != 0 )
					_combined_mask |= _water_mask;

				if( Physics.Raycast( _pos, Vector3.down, out _hit, Mathf.Infinity, _combined_mask, WorldManager.TriggerInteraction ))
				{
					Vector3 _dir = ( _hit.point - _transform.position ).normalized;
					float _path_angle = Vector3.Angle( _dir, Vector3.down ) - 90;
					float _surface_angle = Vector3.Angle( _hit.normal, Vector3.up );

					if( ( MaxSurfaceSlopeAngle > 0 && Mathf.Abs( _surface_angle ) > MaxSurfaceSlopeAngle ) ||
						( MaxPathSlopeAngle > 0 && Mathf.Abs( _path_angle ) > MaxPathSlopeAngle ) || 
						( AvoidWater && SystemTools.IsInLayerMask( _hit.transform.gameObject, _water_mask ) ) )
					{
						DebugLine( _hit.point, _hit.point + ( Vector3.up * 2 ) , Color.yellow );

						for( int i = ScanningAngle ; i <= 180 ; i += ScanningAngle )
						{
							Vector3 _pos_right = Vector3.zero;
							Vector3 _pos_left = Vector3.zero;

							int _right_angle = i;
							_pos = PositionTools.GetDirectionPosition( _transform, _right_angle, ScanningRange );
							_pos.y += _offset;
							if( Physics.Raycast( _pos, Vector3.down, out _hit, Mathf.Infinity, _combined_mask, WorldManager.TriggerInteraction ))
							{
								_dir = ( _hit.point - _transform.position ).normalized;
								_path_angle = Vector3.Angle( _dir, Vector3.down ) - 90;
								_surface_angle = Vector3.Angle( _hit.normal, Vector3.up );

								bool _walkable_right = true;
								bool _water = false;
						
								if( MaxSurfaceSlopeAngle > 0 && Mathf.Abs( _surface_angle ) > MaxSurfaceSlopeAngle )
									_walkable_right = false;

								if( MaxPathSlopeAngle > 0 && _walkable_right && Mathf.Abs( _path_angle ) > MaxPathSlopeAngle )
									_walkable_right = false;

								if( AvoidWater && SystemTools.IsInLayerMask( _hit.transform.gameObject, _water_mask ) )
								{
									_walkable_right = false;
									_water = true;
								}
				
								if( DebugRayIsEnabled )
								{
									float _h = ( MaxPathSlopeAngle > 0 ? MathTools.Normalize( MaxPathSlopeAngle - Mathf.Abs( _path_angle ), 0, MaxPathSlopeAngle ) :  MathTools.Normalize( MaxSurfaceSlopeAngle - _surface_angle, 0, MaxSurfaceSlopeAngle ) );									
									//DebugLine( _pos, _hit.point, ( _water ? Color.blue : ( _walkable_right ? Color.green : new HSBColor( _h * 0.3333333f, 1f, 1f ).ToColor() ) ) );
									DebugLine( _hit.point, _hit.point + ( Vector3.up * 2 ) , ( _water ? Color.blue : ( _walkable_right ? Color.green : new HSBColor( _h * 0.3333333f, 1f, 1f ).ToColor() ) ) );
								}

								if( _walkable_right )
									_pos_right = _hit.point;						
							}
							else
								m_VerticalRaycastOffset += 0.25f;

							int _left_angle = 360 - i;
							_pos = PositionTools.GetDirectionPosition( _transform, _left_angle, ScanningRange );
							_pos.y += _offset;
							if( Physics.Raycast( _pos, Vector3.down, out _hit, Mathf.Infinity, _combined_mask, WorldManager.TriggerInteraction ))
							{
								_dir = ( _hit.point - _transform.position ).normalized;
								_path_angle = Vector3.Angle( _dir, Vector3.down ) - 90;
								_surface_angle = Vector3.Angle( _hit.normal, Vector3.up );

								bool _walkable_left = true;
								bool _water = false;

								if( MaxSurfaceSlopeAngle > 0 && Mathf.Abs( _surface_angle ) > MaxSurfaceSlopeAngle )
									_walkable_left = false;

								if( MaxPathSlopeAngle > 0 && _walkable_left && Mathf.Abs( _path_angle ) > MaxPathSlopeAngle )
									_walkable_left = false;

								if( AvoidWater && SystemTools.IsInLayerMask( _hit.transform.gameObject, _water_mask ) )
								{
									_walkable_left = false;
									_water = true;
								}

								if( DebugRayIsEnabled )
								{
									float _h = ( MaxPathSlopeAngle > 0 ? MathTools.Normalize( MaxPathSlopeAngle - Mathf.Abs( _path_angle ), 0, MaxPathSlopeAngle ) :  MathTools.Normalize( MaxSurfaceSlopeAngle - _surface_angle, 0, MaxSurfaceSlopeAngle ) );									
									//DebugLine( _pos, _hit.point, ( _water ? Color.blue : ( _walkable_left ? Color.green : new HSBColor( _h * 0.3333333f, 1f, 0.25f ).ToColor() ) ) );
									DebugLine( _hit.point, _hit.point + ( Vector3.up * 2 ) , ( _water ? Color.blue : ( _walkable_left ? Color.green : new HSBColor( _h * 0.3333333f, 1f, 1f ).ToColor() ) ) );
								}

								if( _walkable_left )
									_pos_left = _hit.point;
							}
							else
								m_VerticalRaycastOffset += 0.25f;

							if( _pos_right != Vector3.zero && _pos_left != Vector3.zero )
							{
								//if( Vector3.Distance( _position, _pos_right ) <= Vector3.Distance( _position, _pos_left ) )
								if( UnityEngine.Random.Range( 0, 2 ) == 0 )
									m_SlopePathPosition = _pos_right;
								else
									m_SlopePathPosition = _pos_left;
								
								break;
							}
							else if( _pos_right != Vector3.zero )
							{
								m_SlopePathPosition = _pos_right;
								break;
							}
							else if( _pos_left != Vector3.zero )
							{
								m_SlopePathPosition = _pos_left;
								break;
							}
						}
					}
					else
					{
						m_SlopePathPosition = Vector3.zero;

						if( DebugRayIsEnabled )
						{
							float _h = ( MaxPathSlopeAngle > 0 ? MathTools.Normalize( MaxPathSlopeAngle - Mathf.Abs( _path_angle ), 0, MaxPathSlopeAngle ) :  MathTools.Normalize( MaxSurfaceSlopeAngle - _surface_angle, 0, MaxSurfaceSlopeAngle ) );									
							DebugLine( _pos, _hit.point + ( Vector3.up * 2 ), new HSBColor( _h * 0.3333333f, 1f, 0.25f ).ToColor() );
							DebugLine( _hit.point, _hit.point + ( Vector3.up * 2 ) , new HSBColor( _h * 0.3333333f , 1f, 1f ).ToColor() );
						}
					}
				}
				else
					m_VerticalRaycastOffset += 0.25f;
			}

			if( m_SlopePathPosition != Vector3.zero )
			{
				DebugLine( m_SlopePathPosition, m_SlopePathPosition + ( Vector3.up * 2 ) , Color.green );
				_position = m_SlopePathPosition;
			}

			return _position;

		}

	}

	[System.Serializable]
	public class ObstacleAvoidanceObject : ICEOwnerObject
	{
		public ObstacleAvoidanceObject(){}
		public ObstacleAvoidanceObject( ObstacleAvoidanceObject _move ){ Copy( _move );  }

		public void Copy( ObstacleAvoidanceObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			UseCrossBelowSpeed = _object.UseCrossBelowSpeed;
			UseCrossOverSpeed = _object.UseCrossOverSpeed;

			//CheckAngle = _object.CheckAngle;
			ScanningAngle = _object.ScanningAngle;
			ScanningRange = _object.ScanningRange;
			UseDynamicScanningRange = _object.UseDynamicScanningRange;
			DynamicScanningRangeSpeedMultiplier = _object.DynamicScanningRangeSpeedMultiplier;
			CheckDistanceMax = _object.CheckDistanceMax;
			UseFixDirection = _object.UseFixDirection;
		}


		public bool UseOvercomeObstacles = false;

		public bool UseCrossBelowSpeed = false;
		public float CrossBelowStartDistance = 1.2f;
		public float CrossBelowStartDistanceSpeedMultiplier = 1.2f;

		public bool UseCrossOverSpeed = false;
		public float CrossOverStartDistance = 1.2f;
		public float CrossOverStartDistanceSpeedMultiplier = 1.2f;

		public float VerticalRaycastOffset = 1.2f;
		public float VerticalRaycastOffsetMaximum = 3;
		public float VerticalRaycastOffsetDifference = 0.5f;
		//public int CheckAngle = 90;
		public int ScanningAngle = 10;
		public float ScanningRange = 15;
		public float DynamicScanningRangeSpeedMultiplier = 1.5f;
		public bool UseDynamicScanningRange = false;
		public float CheckDistanceMax = 50;
		public bool UseFixDirection = true;

		protected PreferedDirectionType m_FixDirection = PreferedDirectionType.UNDEFINED;
		protected float m_StoppingDistance = 2;

		protected Vector3 m_ObstacleAvoidancePosition = Vector3.zero;
		public Vector3 ObstacleAvoidancePosition{
			get{ return m_ObstacleAvoidancePosition; }
		}

		protected Vector3 m_DesiredCrossOverPosition = Vector3.zero;
		public Vector3 DesiredCrossOverPosition{
			get{ return m_DesiredCrossOverPosition; }
		}

		protected Vector3 m_DesiredCrossBelowPosition = Vector3.zero;
		public Vector3 DesiredCrossBelowPosition{
			get{ return m_DesiredCrossBelowPosition; }
		}

		public bool OvercomeObstaclesPossible{
			get{ return ( UseOvercomeObstacles && ( CrossBelowPossible || CrossOverPossible ) ? true : false ); }
		}

		protected bool m_CrossBelowPossible = false;
		public bool CrossBelowPossible{
			get{ return m_CrossBelowPossible; }
		}

		protected bool m_CrossOverPossible = false;
		public bool CrossOverPossible{
			get{ return m_CrossOverPossible; }
		}

		protected ObstacleAvoidanceActionType m_ActionType = ObstacleAvoidanceActionType.None;
		public ObstacleAvoidanceActionType ActionType{
			get{ return m_ActionType; }
		}

		public bool AvoidanceMovePositionReached{
			get{ return ( m_ObstacleAvoidancePosition == Vector3.zero || m_StoppingDistance == 0 || AvoidanceMovePositionDistance <= m_StoppingDistance ? true : false ); }
		}

		public float AvoidanceMovePositionDistance{
			get{ return ( Owner == null ? 0 : PositionTools.GetDistance( m_ObstacleAvoidancePosition, Owner.transform.position, true ) ); }
		}

		private float m_StartTime = 0;
		private float m_ExpectedActionTime = 0;

		public Vector3 Scan( Transform _transform, Vector3 _position, LayerMask _mask, float _stopping_distance, float _speed )
		{
			if( ( ! UseDynamicScanningRange && ScanningRange == 0 ) || ( UseDynamicScanningRange && _speed == 0 ) )
				return _position;

			if( Time.time - m_StartTime <= m_ExpectedActionTime )
				return _position;


			m_ActionType = ObstacleAvoidanceActionType.None;

			m_StoppingDistance = _stopping_distance;

			if( Owner != _transform.gameObject )
				SetOwner( _transform.gameObject );

			float _vertical_offset = 0;

			Vector3 _avoid_position = _position;
			Vector3 _transform_pos = _transform.position;
			Vector3 _move_pos = _position;
			float _distance = ( UseDynamicScanningRange ? _speed * DynamicScanningRangeSpeedMultiplier : ScanningRange );

			_transform_pos.y = _transform_pos.y + _vertical_offset;
			_move_pos.y = _transform_pos.y;


			RaycastHit _hit;
			RaycastHit _hit_up = new RaycastHit();
			RaycastHit _hit_down = new RaycastHit();

			Vector3 _desired_dir = _position - _transform.position;
			Vector3 _origin = _transform.position + (_transform.up * VerticalRaycastOffset );
			Vector3 _origin_up = _transform.position + (_transform.up * ( VerticalRaycastOffset + VerticalRaycastOffsetDifference ) ); // TODO : slide height
			Vector3 _origin_down = _transform.position + (_transform.up * ( VerticalRaycastOffset - VerticalRaycastOffsetDifference ) ); // TODO : slide height
			Vector3 _forward = _transform.forward;

			float _cross_down_distance = ( UseCrossBelowSpeed ? _speed * CrossBelowStartDistanceSpeedMultiplier : CrossBelowStartDistance );
			float _cross_up_distance = ( UseCrossOverSpeed ? _speed * CrossOverStartDistanceSpeedMultiplier : CrossOverStartDistance );

			float _hit_down_distance = Mathf.Infinity;
			float _hit_up_distance = Mathf.Infinity;

			m_CrossBelowPossible = false;
			m_CrossOverPossible = false;

			if( UseOvercomeObstacles && Physics.Raycast( _origin_down, _forward, out _hit_down, _distance, _mask, WorldManager.TriggerInteraction ) && ! _hit_down.transform.IsChildOf( _transform ) )
			{
				DebugLine( _origin_down, _hit_down.point , Color.red );
				DebugRay( _origin_down, _forward * _cross_down_distance , Color.blue );
				_hit_down_distance = _hit_down.distance;
				m_CrossBelowPossible = true;
			}
			else if( UseOvercomeObstacles )
				DebugRay( _origin_down, _forward * _distance , Color.green );

			if( UseOvercomeObstacles && Physics.Raycast( _origin_up, _forward, out _hit_up, _distance, _mask, WorldManager.TriggerInteraction ) && ! _hit_up.transform.IsChildOf( _transform ) )
			{
				DebugLine( _origin_up, _hit_up.point , Color.red );
				DebugRay( _origin_up, _forward * _cross_up_distance , Color.blue );
				_hit_up_distance = _hit_up.distance;
				m_CrossOverPossible = true;
			}
			else if( UseOvercomeObstacles )
				DebugRay( _origin_up, _forward * _distance , Color.green );

			// CHECK POSIBLE CROSS OVER
			if( UseOvercomeObstacles && _hit_down_distance < _hit_up_distance )
			{
				if( _hit_down.distance < _cross_up_distance )
				{
					m_DesiredCrossOverPosition = _hit_down.point;
					m_DesiredCrossOverPosition.y = _hit_down.collider.bounds.center.y + 0.5f * Owner.GetComponent<Collider>().bounds.extents.y + 0.075f;
					m_StartTime = Time.time;
					m_ExpectedActionTime = _cross_up_distance * 2 / ( _speed > 0 ? _speed : 1 );
					m_ActionType = ObstacleAvoidanceActionType.CrossOver;
				}
			}
			// CHECK POSIBLE CROSS BELOW
			else if( UseOvercomeObstacles && _hit_up_distance < _hit_down_distance )
			{
				if( _hit_up.distance < _cross_down_distance )
				{
					m_DesiredCrossBelowPosition = _position + ( 1.25f * _hit_up.distance * _forward );
					m_StartTime = Time.time;
					m_ExpectedActionTime = _cross_down_distance * 2 / ( _speed > 0 ? _speed : 1 );
					m_ActionType = ObstacleAvoidanceActionType.CrossBelow;
				}
			}
			// CHECK AVOIDANCE
			else if( Physics.Raycast( _origin, _forward, out _hit, _distance, _mask, WorldManager.TriggerInteraction ) )
			{
				if( ! _hit.transform.IsChildOf( _transform ) )
				{
					DebugLine( _origin, _hit.point , Color.red );

					if( _desired_dir.magnitude < PositionTools.Distance( _hit.point, _transform.position ) )
					{
						DebugLine( _origin , _desired_dir , Color.green );	
						m_ObstacleAvoidancePosition = Vector3.zero;
						m_FixDirection = PreferedDirectionType.UNDEFINED;
					}	
					else
					{
						if( _distance > _hit.collider.bounds.size.magnitude * 0.5f )
							_distance = _hit.collider.bounds.size.magnitude * 0.5f;
						else if( _distance > new Vector2( _hit.collider.bounds.size.z, _hit.collider.bounds.size.x ).magnitude )
							_distance = new Vector2( _hit.collider.bounds.size.z, _hit.collider.bounds.size.x ).magnitude;

						DebugLine( _origin , _hit.point, Color.red );

						int _cost_right = 0;
						Vector3 _avoid_right = Vector3.zero;
						for( int i = ScanningAngle ; i <= 360 ; i += ScanningAngle )
						{
							_cost_right++;
							Vector3 _pos = PositionTools.GetDirectionPosition( _transform, i, _distance ); 

							if( ! Physics.Linecast( _origin, _pos, _mask ) )
							{
								_avoid_right = _pos;
								DebugLine( _origin , _pos , SystemTools.ColorA( Color.blue, 0.5f ) );
								break;
							}
							else
							{
								//TODO: if there is no free position we could determinate the best posibility
								DebugLine( _origin , _pos, SystemTools.ColorA( Color.red, 0.25f ) );
							}
						}

						int _cost_left = 0;
						Vector3 _avoid_left = Vector3.zero;
						for( int i = 360 - ScanningAngle; i > 0 ; i -= ScanningAngle )
						{
							_cost_left++;
							Vector3 _pos = PositionTools.GetDirectionPosition( _transform, i, _distance ); 

							if( ! Physics.Linecast( _origin, _pos, _mask ) )
							{
								_avoid_left = _pos;
								DebugLine( _origin , _pos , SystemTools.ColorA( Color.blue, 0.5f ) );
								break;
							}
							else
							{
								//TODO: if there is no free position we could determinate the best posibility
								DebugLine( _origin , _pos, SystemTools.ColorA( Color.red, 0.25f ) );
							}
						}


						// selects the best solution according to the given costs
						if( _avoid_right != Vector3.zero && _avoid_left != Vector3.zero )
						{
							if( _cost_left < _cost_right )
								m_ObstacleAvoidancePosition = _avoid_left;
							else if( _cost_right < _cost_left )
								m_ObstacleAvoidancePosition = _avoid_right;
							else
								m_ObstacleAvoidancePosition = ( Random.Range(0,1) == 1 ? _avoid_left : _avoid_right );
						}
						else if( _avoid_right != Vector3.zero )
							m_ObstacleAvoidancePosition = _avoid_right;
						else if( _avoid_left != Vector3.zero )
							m_ObstacleAvoidancePosition = _avoid_left;

						// makes sure that the creature will not change the direction if not needed
						if( UseFixDirection )
						{
							if( m_FixDirection == PreferedDirectionType.UNDEFINED )
							{
								if( m_ObstacleAvoidancePosition == _avoid_right )
									m_FixDirection = PreferedDirectionType.RIGHT;
								else if( m_ObstacleAvoidancePosition == _avoid_left )
									m_FixDirection = PreferedDirectionType.LEFT;
							}
							else if( m_FixDirection == PreferedDirectionType.RIGHT && _avoid_right != Vector3.zero )
								m_ObstacleAvoidancePosition = _avoid_right;							
							else if(  m_FixDirection == PreferedDirectionType.LEFT && _avoid_left != Vector3.zero )
								m_ObstacleAvoidancePosition = _avoid_left;	
						}
					}
				}
			}
			else 
			{
				DebugRay( _origin, _forward * _distance , Color.green );

				m_FixDirection = PreferedDirectionType.UNDEFINED;

				if( Physics.Raycast( _origin, _desired_dir, out _hit, _distance, _mask, WorldManager.TriggerInteraction ) )
				{
					DebugLine( _origin , _hit.point , SystemTools.ColorA( Color.red, MathTools.Normalize( _distance - PositionTools.Distance(_hit.point, _origin ), 0, _distance ) ) );	

					if( AvoidanceMovePositionReached )
						m_ObstacleAvoidancePosition = _transform.position +  ( _transform.forward * _distance );
				}						
				else
				{
					DebugRay( _origin , _desired_dir.normalized * _distance , Color.gray );	
					m_ObstacleAvoidancePosition = Vector3.zero;
				}
			}

			if( m_ObstacleAvoidancePosition != Vector3.zero )
				_avoid_position = m_ObstacleAvoidancePosition;

			return _avoid_position;
		}
	}


	[System.Serializable]
	public class OverlapPreventionObject : ICEOwnerObject
	{
		public OverlapPreventionObject(){}
		public OverlapPreventionObject( ICEWorldBehaviour _component ) : base( _component ){}
		public OverlapPreventionObject( OverlapPreventionObject _object ){ Copy( _object );  }

		public void Copy( OverlapPreventionObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			OverlapPreventionType = _object.OverlapPreventionType;
			UseAvoiding = _object.UseAvoiding;
			Size = _object.Size;
			Center = _object.Center;
			End = _object.End;
			Radius = _object.Radius;
			OverlapRadiusMaximum = _object.OverlapRadiusMaximum;
			AvoidSpeedMultiplier = _object.AvoidSpeedMultiplier;
			AvoidSpeedMultiplierMaximum = _object.AvoidSpeedMultiplierMaximum;
			EscapeSpeedMultiplier = _object.EscapeSpeedMultiplier;
			EscapeSpeedMultiplierMaximum = _object.EscapeSpeedMultiplierMaximum;
			AngularSpeed = _object.AngularSpeed;
			AngularSpeedMaximum = _object.AngularSpeedMaximum;
		}

		public OverlapType OverlapPreventionType = OverlapType.NONE;
		public bool UseAvoiding = false;
		public Vector3 Size = Vector3.one;
		public Vector3 Center = Vector3.zero;
		public Vector3 End = Vector3.zero;
		public float Radius = 2;
		public float OverlapRadiusMaximum = 25;
		public float AvoidSpeedMultiplier = 0.5f;
		public float AvoidSpeedMultiplierMaximum = 2;
		public float EscapeSpeedMultiplier = 1.5f;
		public float EscapeSpeedMultiplierMaximum = 2;
		public float AngularSpeed = 10f;
		public float AngularSpeedMaximum = 50;

		private Vector3 m_LastTransformPosition = Vector3.zero;
		private Quaternion m_LastTransformRotation = Quaternion.identity;
		private Vector3 m_LastTransformForward = Vector3.zero;

		/*
		private Rigidbody m_Rigidbody = null;
		private Rigidbody TransformRigidbody{
			get{ return m_Rigidbody = ( m_Rigidbody == null && m_Transform != null ? m_Transform.GetComponent<Rigidbody>() : m_Rigidbody ); }
		}*/

		private Transform m_Transform = null;

		private void StoreTransformData( Transform _transform )
		{
			if( m_Transform == _transform )
			{
				m_Transform = _transform;
				//m_Rigidbody = _transform.GetComponent<Rigidbody>();
			}
		}

		private bool m_MoveRequired = false;
		public bool MoveRequired{
			get{ return m_MoveRequired; }
		}

		private bool m_IsBlocked = false;
		public bool IsBlocked{
			get{ return m_IsBlocked; }
		}

		public Vector3 CheckPosition( Vector3 _position, float _range, LayerMask _mask )
		{
			Collider[] _colliders = Physics.OverlapSphere( _position, _range, _mask );

			int i = 0;
			while( i < _colliders.Length ) 
			{
				Collider _collider = _colliders[i];

				if( _collider != null )
				{
					// try to get the closest point but this could fail for some reasons ...
					Vector3 _point = _collider.ClosestPointOnBounds( _position ); 

					// ... so compare the points and try to get the closest point on another way ...
					if( _point == _position )
						_point = SystemTools.ClosestPointOnSurface( _collider, _position );

					if( _point == _position )
						_point = _collider.transform.position;

					_position += ( _position - _point ).normalized * ( PositionTools.Distance( _position, _point ) + _range );
				}
				i++;
			}

			return _position;
		}

		public Vector3 Update( Transform _transform, LayerMask _mask, float _speed, bool _allow = true )
		{
			Vector3 _position = _transform.position;
			Quaternion _rotation = _transform.rotation;
			m_MoveRequired = false;
			m_IsBlocked = false;

			if( OverlapPreventionType != OverlapType.NONE && _allow == true )
			{
				StoreTransformData( _transform );

				Vector3 _center = _transform.TransformPoint( Center );

				Collider[] _colliders = null;

#if UNITY_5_4_OR_NEWER

				Vector3 _end = _transform.TransformPoint( End );

				if( OverlapPreventionType == OverlapType.SPHERE )
					_colliders = Physics.OverlapSphere( _center, Radius, _mask );
				else if( OverlapPreventionType == OverlapType.BOX )
					_colliders = Physics.OverlapBox( _center, Size / 2, _transform.rotation, _mask ); 
				else if( OverlapPreventionType == OverlapType.CAPSULE )
					_colliders = Physics.OverlapCapsule( _center, _end, Radius, _mask ); 
#elif UNITY_5_3 || UNITY_5_3_OR_NEWER
				if( OverlapPreventionType == OverlapType.BOX )
					_colliders = Physics.OverlapBox( _center, Size / 2, _transform.rotation, _mask ); 
				else
					_colliders = Physics.OverlapSphere( _center, Radius, _mask );				 
#else
				_colliders = Physics.OverlapSphere( _center, Radius, _mask );
#endif
				// this list will be used to handle multiple collisions
				//List<Vector3> _points = new List<Vector3>();

				float _last_angle = 0;

				int i = 0;
				while( i < _colliders.Length ) 
				{
					Collider _collider = _colliders[i];

					if( _collider != null && ! _collider.gameObject.transform.IsChildOf( _transform ) )
					{
						// try to get the closest point but this could fail for some reasons ...
						Vector3 _point = _collider.ClosestPointOnBounds( _transform.position ); 

						// ... so compare the points and try to get the closest point on another way ...
						if( ( _point - _position ).magnitude < 0.05f )
							_point = SystemTools.ClosestPointOnSurface( _collider, _transform.position  );////SystemTools.NearestVertexTo( _collider.gameObject, _transform.position );
							
						if( ( _point - _position ).magnitude < 0.01f )
							_point = _collider.transform.position;

						// this block will be used to handle multiple collisions
						/*{
						// if we have found the closest point we will store it ...
						//if( _point != _transform.position )
						//	_points.Add( _point );

						// if we have more than two points we have to evaluate the center
						//if( _points.Count > 1 )
						//{
						//	_point = SystemTools.FindCenterPoint( _points.ToArray() );							
							//Debug.DrawRay( _point, Vector3.up * 20, Color.red );
				//		}
						}*/

					
							//Debug.DrawLine( _transform.position, _point, Color.green );
		
						Vector3 _heading = PositionTools.OverGroundHeading( _transform.position, _point );
						Vector3 _direction = PositionTools.Direction( _heading );

						if( _direction == Vector3.zero )
							_direction = _transform.forward;
						
						float _angle = PositionTools.DirectionAngle( _transform.forward, _direction ) * 180;

						//PrintDebugLog( this, "OverlapPrevention Hit#" + i + " Position" + _transform.position + " HitPoint" + _point + " Heading" + _heading + " Direction" + _direction + " Angle = " + _angle );
							
						// INFRONT - if the collision is infront of the object it will stop or try to avoid the collision object
						if( Mathf.Abs( _angle ) <= 90 )
						{
							// if our object have to avoid the collision it will turn right or left and try to move around
							if( UseAvoiding )
							{
								_last_angle += ( _last_angle == 0 ? ( _angle < 0 ? 90 : -90 ) : _last_angle );

								Vector3 _avoid_direction = MathTools.RotateVectorFromTo( _transform.rotation, MathTools.AddRotation( _transform.rotation, new Vector3( 0, _last_angle, 0 ) ), _direction );
								_rotation = Quaternion.Slerp( m_LastTransformRotation, Quaternion.LookRotation( _avoid_direction.normalized ), AngularSpeed * Time.deltaTime );
								_position = m_LastTransformPosition + ( _avoid_direction.normalized * _speed * AvoidSpeedMultiplier * Time.deltaTime );
							}

							// if the object have to stop, it will  be replaced outside of the collision range or use its last position
							else
							{
								if( _speed == 0 )
								{
									float _radius = Size.magnitude;
#if UNITY_5_4_OR_NEWER
									if( OverlapPreventionType == OverlapType.SPHERE || OverlapPreventionType == OverlapType.CAPSULE )
										_radius = Radius;
#else
									if( OverlapPreventionType == OverlapType.SPHERE )
										_radius = Radius;
#endif
									_point.y = _position.y;

									Vector3 _dir = _point - _position;

									_position = m_LastTransformPosition + _dir - ( _dir.normalized * _radius );
								}
								else
									_position = m_LastTransformPosition;
								
								_rotation = m_LastTransformRotation;
								m_IsBlocked = true;
							}
						}

						// BEHIND - ... otherwise, if the collision is behind our object it can try to escape forwards while itsn't doing an avoid move
						else if( _last_angle == 0 )
						{
							if( m_LastTransformForward == Vector3.zero )
								m_LastTransformForward = _transform.forward;
							
							_rotation = Quaternion.Slerp( m_LastTransformRotation, Quaternion.LookRotation( m_LastTransformForward ), AngularSpeed * Time.deltaTime );
							_position = m_LastTransformPosition + ( m_LastTransformForward * _speed * EscapeSpeedMultiplier * Time.deltaTime );
						}

						// UNCLEAR - ... in all other cases our object have to stop to avoid stuppid motions
						else
						{
							float _radius = Size.magnitude;

#if UNITY_5_4_OR_NEWER
							if( OverlapPreventionType == OverlapType.SPHERE || OverlapPreventionType == OverlapType.CAPSULE )
							_radius = Radius;
#else
							if( OverlapPreventionType == OverlapType.SPHERE )
								_radius = Radius;
#endif

							_point.y = _position.y;

							Vector3 _dir = _point - _position;

							_position = m_LastTransformPosition + _dir - ( _dir.normalized * _radius );
							_rotation = m_LastTransformRotation;
							m_IsBlocked = true;
						}
					}
					i++;
				}
			}



			if( _transform.position != _position )
				_transform.position = _position;

			if( _transform.rotation != _rotation )
				_transform.rotation = _rotation;

			m_LastTransformPosition = _transform.position;
			m_LastTransformRotation = _transform.rotation;
			m_LastTransformForward = _transform.forward;


			return _position;
		}
	}

}
