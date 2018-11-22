// ##############################################################################
//
// ice_CreatureDebug.cs
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

#if UNITY_5_5 || UNITY_5_5_OR_NEWER
using UnityEngine.AI;
#endif

using System.Collections;
using System.Collections.Generic;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;

namespace ICE.Creatures.Objects
{

	/// <summary>
	/// Gizmo object.
	/// </summary>
	[System.Serializable]
	public class GizmoObject : ICEOwnerObject
	{
		public GizmoObject(){}
		public GizmoObject( ICEWorldBehaviour _component ) : base( _component ) {}

		private ICECreatureControl m_CreatureControl = null;
		public ICECreatureControl CreatureControl{
			get{ return m_CreatureControl = ( m_CreatureControl == null && Owner != null ? Owner.GetComponent<ICECreatureControl>():m_CreatureControl ); }
		}
		
		public override void Init( ICEWorldBehaviour _component )
		{
			base.Init( _component );
		}

		public bool ShowPath = true;
		private List<Vector3> m_PathPositions = new List<Vector3>();
		public int PathPositionsLimit = 1000;
		public float PathPrecision = 0.5f;

		public bool ShowText = true;
		
		public float Level = 0;
		public bool UseObjectLevel = true;

		public Color TargetColor = new Vector4(0, 0.5f, 1, 0.5f);
		public Color ActiveTargetColor = new Vector4(0, 0, 1, 1);

		public Color TargetStoppingDistanceColor = new Vector4(1, 0, 0, 0.05f);
		public Color TargetSelectionRangeColor = new Vector4(0, 0, 1, 0.05f);
		public Color TargetRandomRangeColor = new Vector4(1, 1, 0, 0.05f);

		public Color MoveColor = new Vector4(0, 0.5f, 0.5f, 0.5f);
		public Color MovePreviousPathColor = new Vector4(0, 0.5f, 0.5f, 0.5f);
		public Color MoveCurrentPathColor = new Vector4(0, 0.9f, 0.9f, 1f);
		public Color MoveProjectedPathColor = new Vector4(0, 0.9f, 0.9f, 1f);
		public Color MoveEscapeColor = new Vector4(0,0.75f, 0.75f, 0.5f);
		public Color MoveAvoidColor = new Vector4(0,0.75f, 0.75f, 0.5f);
		public Color MoveDetourColor = new Vector4(0,0.75f, 0.75f, 0.5f);
		public Color MoveOrbitColor = new Vector4(0,0.75f, 0.75f, 0.5f);
		public Color InteractionColor = new Vector4(0.75f, 0.5f, 0.65f, 1);
		//public bool ShowSolidInteractionRange = true;
		//public float SolidInteractionAlpha = 0.025f;

		public bool ShowHome = true;
		public bool ShowOutpost = true;
		public bool ShowEscort = true;
		public bool ShowPatrol = true;
		public bool ShowInteractor = true;
		
		public LabelType Label = LabelType.Blue;

		/// <summary>
		/// Draws the home.
		/// </summary>
		public void DrawHome()
		{
			if( ! CreatureControl.Creature.Essentials.TargetReady() || ShowHome == false )
				return;
		
			TargetObject _target = CreatureControl.Creature.Essentials.Target;
			DrawTargetGizmos( _target );	

			if( ! Application.isPlaying )
			{
				BehaviourModeObject _mode = null;
				_mode = CreatureControl.Creature.Behaviour.GetBehaviourModeByKey( CreatureControl.Creature.Essentials.BehaviourModeRun );			
				DrawBehaviourModeGizmos( _target, _mode );
				
				_mode = CreatureControl.Creature.Behaviour.GetBehaviourModeByKey( CreatureControl.Creature.Essentials.BehaviourModeWalk );			
				DrawBehaviourModeGizmos( _target, _mode );
				
				_mode = CreatureControl.Creature.Behaviour.GetBehaviourModeByKey( CreatureControl.Creature.Essentials.BehaviourModeIdle );			
				DrawBehaviourModeGizmos( _target, _mode );
			}
		}

		/// <summary>
		/// Draws the outpost.
		/// </summary>
		public void DrawOutpost()
		{
			if( ! CreatureControl.Creature.Missions.Outpost.TargetReady() || ShowOutpost == false  )
				return;

			TargetObject _target = CreatureControl.Creature.Missions.Outpost.Target;
			DrawTargetGizmos( _target );	

			if( ! Application.isPlaying )
			{
				BehaviourModeObject _mode = null;
				_mode = CreatureControl.Creature.Behaviour.GetBehaviourModeByKey( CreatureControl.Creature.Missions.Outpost.BehaviourModeTravel );			
				DrawBehaviourModeGizmos( _target, _mode );
				
				_mode = CreatureControl.Creature.Behaviour.GetBehaviourModeByKey( CreatureControl.Creature.Missions.Outpost.BehaviourModeLeisure );			
				DrawBehaviourModeGizmos( _target, _mode );

				_mode = CreatureControl.Creature.Behaviour.GetBehaviourModeByKey( CreatureControl.Creature.Missions.Outpost.BehaviourModeRendezvous );			
				DrawBehaviourModeGizmos( _target, _mode );
			}
		}


		/// <summary>
		/// Draws the escort.
		/// </summary>
		public void DrawEscort()
		{
			if( ! CreatureControl.Creature.Missions.Escort.TargetReady() || ShowEscort == false  )
				return;

			TargetObject _target = CreatureControl.Creature.Missions.Escort.Target;
			DrawTargetGizmos( _target );	

			if( ! Application.isPlaying )
			{

				BehaviourModeObject _mode = null;
				_mode = CreatureControl.Creature.Behaviour.GetBehaviourModeByKey( CreatureControl.Creature.Missions.Escort.BehaviourModeFollow );			
				DrawBehaviourModeGizmos( _target, _mode );

				_mode = CreatureControl.Creature.Behaviour.GetBehaviourModeByKey( CreatureControl.Creature.Missions.Escort.BehaviourModeEscort );			
				DrawBehaviourModeGizmos( _target, _mode );

				_mode = CreatureControl.Creature.Behaviour.GetBehaviourModeByKey( CreatureControl.Creature.Missions.Escort.BehaviourModeIdle );			
				DrawBehaviourModeGizmos( _target, _mode );

				_mode = CreatureControl.Creature.Behaviour.GetBehaviourModeByKey( CreatureControl.Creature.Missions.Escort.BehaviourModeStandby );			
				DrawBehaviourModeGizmos( _target, _mode );
			}

		}

		/// <summary>
		/// Draws the patrol.
		/// </summary>
		public void DrawPatrol()
		{
			if( ! CreatureControl.Creature.Missions.Patrol.TargetReady() || ShowPatrol == false  )
				return;
			
			TargetObject _target = null;
			Vector3 _target_move_position = Vector3.zero;
			float _target_stop_distance = 0;
			
			WaypointObject _last_waypoint_target =	CreatureControl.Creature.Missions.Patrol.Waypoints.GetLastValidWaypoint();
			
			Vector3 _last_target_move_position = Vector3.zero;
			float _last_target_stop_distance = 0;
			
			if( _last_waypoint_target != null )
			{
				_last_target_move_position = _last_waypoint_target.TargetMovePosition;
				_last_target_move_position.y = GetLevel( _last_waypoint_target.TargetGameObject );
				_last_target_stop_distance = _last_waypoint_target.Move.StoppingDistance;
			}

			for(  int i = 0 ; i < CreatureControl.Creature.Missions.Patrol.Waypoints.Waypoints.Count ; i++ )
			{
				_target = (TargetObject)CreatureControl.Creature.Missions.Patrol.Waypoints.Waypoints[i];

				if( _target.IsValidAndReady == false )
					continue;

				_target_move_position = _target.TargetMovePosition;
				_target_move_position.y = GetLevel( _target.TargetGameObject );
				_target_stop_distance = _target.Move.StoppingDistance;
				
				if( CreatureControl.Creature.Missions.Patrol.Waypoints.Waypoints[i].Enabled )
				{
					float _last_level = DrawTargetGizmos( _target );

					Color _default_color = Gizmos.color;
					Gizmos.color = new Color( TargetStoppingDistanceColor.r,TargetStoppingDistanceColor.g,TargetStoppingDistanceColor.b, 1 );
					CustomGizmos.OffsetPath( _last_target_move_position, _last_target_stop_distance , _target_move_position, _target_stop_distance );
					Gizmos.color = _default_color;

					_last_target_move_position = _target_move_position;
					_last_target_move_position.y = _last_level;
					_last_target_stop_distance = _target_stop_distance;
				}
				else
				{
					Color _color = TargetColor;
					_color.a = 0.25f;
					DrawTargetGizmos( _target, _color );
				}

				if( ! Application.isPlaying )
				{
					BehaviourModeObject _mode = null;
					_mode = CreatureControl.Creature.Behaviour.GetBehaviourModeByKey( CreatureControl.Creature.Missions.Patrol.GetBehaviourModeTravelByIndex(i) );			
					DrawBehaviourModeGizmos( _target, _mode );

					_mode = CreatureControl.Creature.Behaviour.GetBehaviourModeByKey( CreatureControl.Creature.Missions.Patrol.GetBehaviourModePatrolByIndex(i));			
					DrawBehaviourModeGizmos( _target, _mode );

					_mode = CreatureControl.Creature.Behaviour.GetBehaviourModeByKey( CreatureControl.Creature.Missions.Patrol.GetBehaviourModeLeisureByIndex(i) );			
					DrawBehaviourModeGizmos( _target, _mode );

					_mode = CreatureControl.Creature.Behaviour.GetBehaviourModeByKey( CreatureControl.Creature.Missions.Patrol.GetBehaviourModeRendezvousByIndex(i) );			
					DrawBehaviourModeGizmos( _target, _mode );
				}
			}
		}

		/// <summary>
		/// Draws the interaction.
		/// </summary>
		public void DrawInteraction()
		{
			if( CreatureControl.Creature.Interaction.Interactors.Count == 0 || ShowInteractor == false  )
				return;

			for(  int i = 0 ; i < CreatureControl.Creature.Interaction.Interactors.Count ; i++ )
			{
				InteractorObject _interactor = CreatureControl.Creature.Interaction.Interactors[i];
								
				if( ! _interactor.Enabled )
					continue;

				List<GameObject> _target_game_objects = _interactor.GetAllTargetGameObjects( CreatureControl.gameObject );


				if( _target_game_objects != null && _target_game_objects.Count > 0 )
				{
					foreach( GameObject _target_game_object in _target_game_objects )
					{
						if( _target_game_object == null ) 
							continue;

						Vector3 _interactor_pos = _target_game_object.transform.position;	

						TargetObject _target = new TargetObject( _interactor );
						_target.OverrideTargetGameObject( _target_game_object );

						DrawTargetGizmos( _target );
			
						// DIRECTION
						CustomGizmos.Arrow( _target.TargetTransformPosition, _target.TargetTransformForward * _target.Move.StoppingDistance , 0.5f, 20 );
					
						Vector3 _last_move_position = _target.TargetMovePosition;
						float _last_stop_distance = _target.Move.StoppingDistance;

						_last_move_position.y = GetLevel( _last_move_position );

						Color _default_color = Gizmos.color;
						Color _target_color = ( _target.Active?ActiveTargetColor:TargetColor); 

						foreach( InteractorRuleObject _rule in _interactor.Rules )
						{
							if( ! _rule.Enabled )
								continue;

							_target = new TargetObject( TargetType.INTERACTOR );
							
							_target.OverrideTargetGameObject( _target_game_object );
							_target.Behaviour.CurrentBehaviourModeKey = _rule.Behaviour.CurrentBehaviourModeKey;	
							_target.Behaviour.Copy( _rule.Behaviour );
							_target.Selectors.Copy( _rule.Selectors ); 
			
							if( _rule.OverrideTargetMovePosition )
								_target.Move.Copy( _rule.Move );
							else
								_target.Move.Copy( _interactor.Move );
			
							DrawTargetGizmos( _target );

							Vector3 _move_position = _target.TargetMovePosition;
							_move_position.y = GetLevel( _move_position );

							Gizmos.color = _target_color;
							if( _last_move_position != Vector3.zero )
								CustomGizmos.OffsetPath( _last_move_position, _last_stop_distance, _move_position, _target.Move.StoppingDistance, true );
			
							_last_stop_distance = _target.Move.StoppingDistance;
							_last_move_position = _move_position;

						}
						
						Gizmos.color = _target_color;
						if( _last_move_position != Vector3.zero )
							CustomGizmos.OffsetPath( _last_move_position, _last_stop_distance, _interactor_pos, 0, false );
				
						Gizmos.color = _default_color;
						//CustomGizmos.OffsetPath( _default_data.OffsetPosition, _default_data.StoppingDistance, _interactor_pos, 0 );
					}
				}		
			}
		}


		/// <summary>
		/// Gets the level.
		/// </summary>
		/// <returns>The level.</returns>
		private float GetLevel()
		{
			return CreatureControl.transform.position.y + Level;
		}

		private float GetLevel( Vector3 _position )
		{
			return CreatureRegister.GetGroundLevel( _position ) + Level;
		}

		/// <summary>
		/// Gets the level.
		/// </summary>
		/// <returns>The level.</returns>
		private float GetLevel( GameObject _object )
		{
			if( _object != null )
				return _object.transform.position.y + Level;
			else
				return CreatureControl.transform.position.y + Level;
		}
		

		/// <summary>
		/// Gets creature list by name
		/// </summary>
		/// <returns>The creatures by name.</returns>
		/// <param name="_name">_name.</param>
		/// <summary></summary>
	/*	private List<GameObject> GetTargets( TargetObject _target )
		{
			if( ICECreatureRegister.Register )
				return ICECreatureRegister.Register.GetCreaturesByKey( _name );

			return null;
		}*/

		private void DrawDirectionAngle( Transform _center, float _radius, float _angle, float _max = 0, float _min = 0 )
		{
			Vector3 _center_pos = _center.position;
			float _level = GetLevel( _center_pos );
			
			Vector3 _left_pos = PositionTools.GetDirectionPosition( _center, - _angle, _radius );
			Vector3 _right_pos = PositionTools.GetDirectionPosition( _center, _angle, _radius );		
			
			_center_pos.y = _level;
			_left_pos.y = _level;
			_right_pos.y = _level;
			
			Gizmos.DrawLine( _center_pos, _left_pos );
			Gizmos.DrawLine( _center_pos, _right_pos );

			CustomGizmos.Arc( _center, _radius, 1, - _angle, _angle, _level, false ); 

			if( _min > 0 && _min < _radius )
				CustomGizmos.Arc( _center, _min, 1, - _angle, _angle, _level, true );
			if( _max > _radius )
				CustomGizmos.Arc( _center, _max, 1, - _angle, _angle, _level, true ); 
		}

		private void DrawDirectionAngle( Transform _center, float _inner_radius, float _outer_radius, float _outer_radius_max, float _outer_radius_min, float _inner_angle, float _outer_angle )
		{
			Vector3 _center_pos = _center.position;
			float _level = GetLevel( _center_pos );

			Vector3 _left_inner_pos = PositionTools.GetDirectionPosition( _center, - _inner_angle, _inner_radius );
			Vector3 _right_inner_pos = PositionTools.GetDirectionPosition( _center, _inner_angle, _inner_radius );				
			Vector3 _left_outer_pos = PositionTools.GetDirectionPosition( _center, - _outer_angle, _outer_radius );
			Vector3 _right_outer_pos = PositionTools.GetDirectionPosition( _center, _outer_angle, _outer_radius );
			
			_center_pos.y = _level;
			_left_inner_pos.y = _level;
			_right_inner_pos.y = _level;
			_left_outer_pos.y = _level;
			_right_outer_pos.y = _level;
			
			Gizmos.DrawLine( _center_pos, _left_inner_pos );
			Gizmos.DrawLine( _center_pos, _right_inner_pos );
			
			Gizmos.DrawLine( _left_inner_pos, _left_outer_pos );
			Gizmos.DrawLine( _right_inner_pos, _right_outer_pos );
			
			CustomGizmos.Arc( _center, _inner_radius, 1, - _inner_angle, _inner_angle, _level, false ); 
			CustomGizmos.Arc( _center, _outer_radius, 1, - _outer_angle, _outer_angle, _level, false ); 
			
			CustomGizmos.Arc( _center, _outer_radius_min, 1, - _outer_angle, _outer_angle, _level, true ); 
			CustomGizmos.Arc( _center, _outer_radius_max, 1, - _outer_angle, _outer_angle, _level, true ); 
		}

		private void DrawBehaviourModeRuleGizmos( TargetObject _target, BehaviourModeRuleObject _rule )
		{
			#if UNITY_EDITOR
			if( _target == null || _target.IsValidAndReady == false || _rule == null )
				return;

			Vector3 _owner_position = Owner.transform.position;
			Vector3 _target_pos = _target.TargetTransformPosition;
			Vector3 _target_move_position = _target.TargetMovePosition;
			float _target_stop_distance = _target.Move.StoppingDistance;
			float _target_selection_range = _target.Selectors.SelectionRange;
			//float _target_selection_angle = _target.Selectors.SelectionAngle;

			_owner_position.y = GetLevel( _owner_position );
			_target_pos.y = GetLevel( _target_pos );
			_target_move_position.y = GetLevel( _target_move_position );

			if( _rule.Move.Type == MoveType.DEFAULT )
			{
			}
			else if( _rule.Move.Type == MoveType.AVOID )
			{
				MoveDataObject _move = new MoveDataObject( _rule.Move );
				//float _radius = _target_selection_range  + 0.25f;// _move.Avoid.MaxAvoidDistance;//( _target_selection_range / 2 ) + 0.25f;
				//float _angle = _move.Avoid.MaxDirectionAngle;

				CustomGizmos.Color( MoveAvoidColor );

				//DrawDirectionAngle( Owner.transform, _radius, _angle );
				//DrawDirectionAngle( _target.TargetGameObject.transform, _radius, _angle );

				Vector3 _right = Vector3.Cross( _target.TargetGameObject.transform.up, _target.TargetGameObject.transform.forward );
				Vector3 _avoid_pos_right = _target_pos + ( _right * _move.Avoid.AvoidDistance );
				Vector3 _avoid_pos_left = _target_pos + ( _right * -_move.Avoid.AvoidDistance );

				CustomGizmos.OffsetPath( _target_pos, 0, _avoid_pos_right, _move.StoppingDistance );
				CustomGizmos.DottedCircle( _avoid_pos_right,_move.StoppingDistance, 5, 2 );
				CustomGizmos.OffsetPath( _target_pos, 0, _avoid_pos_left, _move.StoppingDistance );
				CustomGizmos.DottedCircle( _avoid_pos_left,_move.StoppingDistance, 5, 2 );

				if( Application.isPlaying )
				{
					/*
					Gizmos.color = Color.blue;
					Gizmos.DrawLine( _target_pos, CreatureControl.Creature.Move.AvoidMovePosition );
					Gizmos.color = Color.red;
					Gizmos.DrawLine( _target_pos, CreatureControl.Creature.Move.MovePosition );
					*/

				}
			}
			else if( _rule.Move.Type == MoveType.ESCAPE )
			{
				MoveDataObject _move = new MoveDataObject( _rule.Move );
				float _inner_radius = ( _target_selection_range / 2 ) + 0.25f;
				float _outer_radius = _inner_radius + _rule.Move.SegmentLength;
				float _outer_radius_max = _inner_radius + _rule.Move.MoveSegmentLengthMax;
				float _outer_radius_min = _inner_radius + _rule.Move.MoveSegmentLengthMin;

				float _inner_angle = 0;
				float _outer_angle = _inner_angle + _move.Escape.RandomEscapeAngle;

				Gizmos.color = MoveEscapeColor;

				Vector3 _left_inner_pos = PositionTools.GetDirectionPosition( _target.TargetGameObject.transform, - _inner_angle, _inner_radius );
				Vector3 _right_inner_pos = PositionTools.GetDirectionPosition( _target.TargetGameObject.transform, _inner_angle, _inner_radius );				
				Vector3 _left_outer_pos = PositionTools.GetDirectionPosition( _target.TargetGameObject.transform, - _outer_angle, _outer_radius );
				Vector3 _right_outer_pos = PositionTools.GetDirectionPosition( _target.TargetGameObject.transform, _outer_angle, _outer_radius );

				_left_inner_pos.y = _target_pos.y;
				_right_inner_pos.y = _target_pos.y;
				_left_outer_pos.y = _target_pos.y;
				_right_outer_pos.y = _target_pos.y;

				Gizmos.DrawLine( _target_pos, _left_inner_pos );
				Gizmos.DrawLine( _target_pos, _right_inner_pos );

				Gizmos.DrawLine( _left_inner_pos, _left_outer_pos );
				Gizmos.DrawLine( _right_inner_pos, _right_outer_pos );

				CustomGizmos.Arc( _target.TargetGameObject.transform, _inner_radius, 1, - _inner_angle, _inner_angle, _target_pos.y, false ); 
				CustomGizmos.Arc( _target.TargetGameObject.transform, _outer_radius, 1, - _outer_angle, _outer_angle, _target_pos.y, false ); 

				CustomGizmos.Arc( _target.TargetGameObject.transform, _outer_radius_min, 1, - _outer_angle, _outer_angle, _target_pos.y, true ); 
				CustomGizmos.Arc( _target.TargetGameObject.transform, _outer_radius_max, 1, - _outer_angle, _outer_angle, _target_pos.y, true ); 

				// DENGER ZONE BEGIN
				_inner_radius+= 0.25f;
				float _degree = CustomGizmos.GetBestDegrees( _inner_radius, _inner_angle );
				CustomGizmos.ArrowArc( _target.TargetGameObject.transform, _inner_radius,_degree ,- _inner_angle,_inner_angle, _target_pos.y ); 

				Transform _target_transform = _target.TargetGameObject.transform;
				Vector3 _center = _target_transform.position;
				Vector3 _center_pos = PositionTools.GetDirectionPosition( _target_transform, 0, _inner_radius + ( _inner_radius / 10 ) );
				Vector3 _left_pos = PositionTools.GetDirectionPosition( _target_transform, - _inner_angle, _inner_radius );
				Vector3 _right_pos = PositionTools.GetDirectionPosition( _target_transform, _inner_angle , _inner_radius );
				
				_center.y = _target_pos.y;
				_center_pos.y = _target_pos.y;
				_left_pos.y = _target_pos.y;
				_right_pos.y = _target_pos.y;
				
				Gizmos.DrawLine( _center, _center_pos );
				Gizmos.DrawLine( _center, _left_pos );
				Gizmos.DrawLine( _center, _right_pos );

			}
			else if( _rule.Move.Type == MoveType.ORBIT )
			{
				MoveDataObject _move = new MoveDataObject( _rule.Move );
				float _degrees = CreatureControl.Creature.Move.CurrentMove.OrbitDegrees;
				float _radius = _move.Orbit.Radius;

				if( Application.isPlaying )
				{
					_move = CreatureControl.Creature.Move.CurrentMove;
					_radius = CreatureControl.Creature.Move.CurrentMove.OrbitRadius;
				}

				Gizmos.color = MoveOrbitColor;
				CustomGizmos.Orbit( Owner.transform.position, _target_move_position, _radius, _degrees, _move.Orbit.RadiusShift, _move.Orbit.MinDistance, _move.Orbit.MaxDistance, _owner_position.y );
			}
			else if( _rule.Move.Type == MoveType.DETOUR )
			{
				Vector3 _detour_pos = _rule.Move.Detour.Position;

				_detour_pos.y = GetLevel( _detour_pos );
	
				Gizmos.color = MoveDetourColor;
				CustomGizmos.OffsetPath( _detour_pos, _rule.Move.StoppingDistance , _target_move_position, _target_stop_distance ); 
				CustomGizmos.Circle( _detour_pos, _rule.Move.StoppingDistance, 5, false ); // STOP DISTANCE RANGE
				//DrawMoveGizmos( Vector3 _initial_position, Vector3 _move_position, float _stop_distance, float _random_range, Vector3 _target_position, float _target_stop_distance )
			}

			#endif
		}
		
		private Vector3 DrawBehaviourModeGizmos( TargetObject _target, BehaviourModeObject _mode )
		{
			if( _target == null || _target.IsValidAndReady == false || _mode == null )
				return Vector3.zero;
			
			foreach( BehaviourModeRuleObject _rule in _mode.Rules )
				DrawBehaviourModeRuleGizmos( _target, _rule );
			
			return Vector3.zero;
		}

		private Vector3 m_PreviousOwnerPosition = Vector3.zero;
		public void DrawPathGizmos()
		{
			// PATH BEGIN
			if( ShowPath == false )
				return;

			Color _default = Gizmos.color;
			Gizmos.color = MovePreviousPathColor;

			Vector3 _owner_pos = Owner.transform.position;
			_owner_pos.y = GetLevel( Owner );

			if( PathPositionsLimit > 0 && m_PathPositions.Count >= PathPositionsLimit )
				m_PathPositions.RemoveAt(0);
			
			if( PathPrecision == 0 || PositionTools.Distance( m_PreviousOwnerPosition, _owner_pos ) >= PathPrecision )
			{
				m_PreviousOwnerPosition = _owner_pos;
				m_PathPositions.Add( _owner_pos );
			}
			
			Vector3 _prior_pos = Vector3.zero;
			foreach( Vector3 _pos in m_PathPositions)
			{
				if( _prior_pos != Vector3.zero  )
					Gizmos.DrawLine( _prior_pos, _pos );
	
				_prior_pos = _pos;
			}

			if( CreatureControl.Creature.Move.UseNavMeshAgent )
			{
				Gizmos.color = MoveCurrentPathColor;
				NavMeshPath _path = CreatureControl.Creature.Move.NavMeshAgentComponent.path;
				_prior_pos = Vector3.zero;
				foreach( Vector3 _pos in _path.corners )
				{
					if( _prior_pos != Vector3.zero  )
						Gizmos.DrawLine( _prior_pos, _pos );
					
					_prior_pos = _pos;
				}
			}

			Gizmos.color = _default;
		}

		public void DrawOwnerGizmos( Color _gizmo_color )
		{
			#if UNITY_EDITOR
			Vector3 _owner_pos = Owner.transform.position;
			_owner_pos.y = GetLevel( Owner );

			if( Application.isPlaying )
			{
				DrawPathGizmos();
				DrawMoveGizmos();
			}

			//Color _default = Gizmos.color;
			Gizmos.color = _gizmo_color;

			CustomGizmos.Triangle( _owner_pos, Owner.transform.forward, 2, 35 );

			if( CreatureControl.Creature.Status.Sensoria.Enabled && CreatureControl.Creature.Status.Sensoria.FieldOfView > 0 )
			{
				float _angle = CreatureControl.Creature.Status.Sensoria.FieldOfView;
				float _distance = CreatureControl.Creature.Status.Sensoria.VisualRange;
				
				if( _distance == 0 )
					_distance = 1.5f;

				float _degree = CustomGizmos.GetBestDegrees( _distance, _angle );
				CustomGizmos.ArrowArc( Owner.transform, _distance, _degree, - _angle, _angle, GetLevel() );

				if( _angle < 180 )
				{
					Vector3 _left_pos = PositionTools.GetDirectionPosition( Owner.transform, - _angle, _distance );
					Vector3 _right_pos = PositionTools.GetDirectionPosition( Owner.transform, _angle, _distance );

					CustomGizmos.OffsetPath( Owner.transform.position, 2, _left_pos, 1) ;
					CustomGizmos.OffsetPath( Owner.transform.position, 2, _right_pos, 1) ;
				}
			}

			if( CreatureControl.Creature.Move.DefaultBody.Type == BodyOrientationType.DEFAULT || CreatureControl.Creature.Move.DefaultBody.Type == BodyOrientationType.QUADRUPED )
			{
				if( CreatureControl.Creature.Move.DefaultBody.UseAdvanced )
				{
					MoveObject _move = CreatureControl.Creature.Move;
						
					if( ! Application.isPlaying )
					{
						_move.CurrentBody.Copy( _move.DefaultBody );
						_move.CurrentBody.Type = BodyOrientationType.QUADRUPED;
						_move.Init( CreatureControl );
						_move.HandleGroundOrientation( Owner.transform.rotation );
					}

					CustomGizmos.Color( Color.blue );

					CustomGizmos.Cube( 0, _move.FrontLeftPositionGround, Owner.transform.rotation, 0.1f );
					CustomGizmos.Cube( 0, _move.BackLeftPositionGround, Owner.transform.rotation, 0.1f );
					CustomGizmos.Cube( 0, _move.FrontRightPositionGround, Owner.transform.rotation, 0.1f );
					CustomGizmos.Cube( 0, _move.BackRightPositionGround, Owner.transform.rotation, 0.1f );

					CustomGizmos.DrawLine( _move.FrontLeftPositionGround, _move.FrontLeftPosition );
					CustomGizmos.DrawLine( _move.BackLeftPositionGround, _move.BackLeftPosition );
					CustomGizmos.DrawLine( _move.FrontRightPositionGround, _move.FrontRightPosition );
					CustomGizmos.DrawLine( _move.BackRightPositionGround, _move.BackRightPosition );

					CustomGizmos.Color( Color.red );

					CustomGizmos.Cube( 0, _move.FrontLeftPosition, Owner.transform.rotation, 0.1f );
					CustomGizmos.Cube( 0, _move.BackLeftPosition, Owner.transform.rotation, 0.1f );
					CustomGizmos.Cube( 0, _move.FrontRightPosition, Owner.transform.rotation, 0.1f );
					CustomGizmos.Cube( 0, _move.BackRightPosition, Owner.transform.rotation, 0.1f );


				}
			}

			if( CreatureControl.Creature.Move.OverlapPrevention.OverlapPreventionType == OverlapType.SPHERE )
			{
				Vector3 _center = Owner.transform.TransformPoint( CreatureControl.Creature.Move.OverlapPrevention.Center );
				Gizmos.color = Color.red;
				Gizmos.DrawWireSphere( _center, CreatureControl.Creature.Move.OverlapPrevention.Radius );
			}

#if UNITY_5_3 || UNITY_5_3_OR_NEWER
			else if( CreatureControl.Creature.Move.OverlapPrevention.OverlapPreventionType == OverlapType.BOX )
			{
				Gizmos.color = Color.red;
				ICE.World.Utilities.CustomGizmos.Box( Owner.transform, CreatureControl.Creature.Move.OverlapPrevention.Size, CreatureControl.Creature.Move.OverlapPrevention.Center );
			}
#endif

#if UNITY_5_4_OR_NEWER
			else if( CreatureControl.Creature.Move.OverlapPrevention.OverlapPreventionType == OverlapType.CAPSULE )
			{
				Vector3 _start = CreatureControl.Creature.Move.OverlapPrevention.Center ;//Owner.transform.TransformPoint( CreatureControl.Creature.Move.OverlapPrevention.OverlapCenter );
				Vector3 _end = CreatureControl.Creature.Move.OverlapPrevention.End;// Owner.transform.TransformPoint( CreatureControl.Creature.Move.OverlapPrevention.OverlapEnd );
				Gizmos.color = Color.red;
				ICE.World.Utilities.CustomGizmos.Capsule( Owner.transform, _start, _end, CreatureControl.Creature.Move.OverlapPrevention.Radius);
			}
#endif
			if( CreatureControl.Creature.Move.ObstacleCheck != ObstacleCheckType.NONE )
			{

			}

			string _text = Owner.name + 
				"\nBehaviour : " + CreatureControl.Creature.Behaviour.ActiveBehaviourModeKey + 
				"\nTarget : " + CreatureControl.Creature.ActiveTargetName;
			Color _text_color = new Color( _gizmo_color.r,_gizmo_color.g,_gizmo_color.b, 1 );
			Vector3 _text_pos = _owner_pos;
			CustomGizmos.Text( _text , _text_pos , _text_color, 12, FontStyle.Italic );


			#endif
		}

		public void DrawMoveGizmos()
		{
			#if UNITY_EDITOR
			if( Owner == null || CreatureControl.Creature.Move.CurrentTarget == null )
				return;

			TargetObject _target = CreatureControl.Creature.Move.CurrentTarget;

			Vector3 _move_position = CreatureControl.Creature.Move.MovePosition;
			Vector3 _target_move_position = _target.TargetMovePosition;
			float _move_stopping_distance = CreatureControl.Creature.Move.DesiredStoppingDistance;
			float _move_segment_length = CreatureControl.Creature.Move.CurrentMove.SegmentLength;
			float _move_segment_variance = CreatureControl.Creature.Move.CurrentMove.SegmentVariance;
			float _move_deviation_length = CreatureControl.Creature.Move.CurrentMove.DeviationLength;
			float _move_deviation_variance = CreatureControl.Creature.Move.CurrentMove.DeviationVariance;
			float _move_deviation_length_max = _move_deviation_length * _move_deviation_variance;

			Vector3 _owner_pos = Owner.transform.position;
			//_owner_pos.y = GetLevel();
			//_move_position.y = GetLevel( _move_position );
			//_target_move_position.y = GetLevel( _target.TargetGameObject );

			Color _default_color = Gizmos.color;
			Gizmos.color = MoveProjectedPathColor;
			CustomGizmos.OffsetPath( _owner_pos, 2, _move_position, _move_stopping_distance ); // PATH FROM CREATURE TO NEXT MOVE POSITION
			CustomGizmos.Circle( _move_position, _move_stopping_distance, 5, false ); // STOP DISTANCE RANGE

			if( ShowText )
			{
				string _text = CreatureControl.name + "\n" +
					"Move Position : " + _move_position.ToString() + "\n" +
					"Move Stopping Distance : " + _move_stopping_distance;

				if( _move_segment_length > 0 )
				{
					_text += "\nMove Segment Length : " + _move_segment_length + "(" + _move_segment_variance + ")";
			
					if( _move_deviation_length > 0 )
						_text += "\nMove Deviation Length : " + _move_deviation_length + "*" + _move_deviation_variance + "(" + _move_deviation_length_max + ")";
				}


				Color _text_color = new Color( MoveProjectedPathColor.r,MoveProjectedPathColor.g,MoveProjectedPathColor.b, 1 );
				Vector3 _text_pos = _move_position + ( Vector3.forward * _move_stopping_distance );
				CustomGizmos.Text( _text , _text_pos , _text_color, 12, FontStyle.Italic );
			}


			if( CreatureControl.Creature.Move.CurrentMove.Type != MoveType.ESCAPE )
				CustomGizmos.OffsetPath( _move_position, _move_stopping_distance , _target_move_position , _target.Move.StoppingDistance );  // PATH NEXT MOVE POSITION TO TARGET MOVE POSITION


			if( _move_segment_length > 0 &&  _move_deviation_length_max > 0 )
			{
				Gizmos.color = new Color( MoveProjectedPathColor.r,MoveProjectedPathColor.g,MoveProjectedPathColor.b, Mathf.Max( MoveProjectedPathColor.a * 0.5f, 0.25f ) );

				float _max_range = _move_stopping_distance + _move_deviation_length_max;

				CustomGizmos.BeamCircle( _move_position, _move_deviation_length_max, 10, true, _move_deviation_length_max * 0.25f, "", false ); // RANDOM RANGE
				CustomGizmos.Circle( _move_position, _max_range, 5, true ); // MAX MOVE RANGE
			}

			Gizmos.color = _default_color;
			#endif
		}

		private void DrawTargetGizmos( TargetObject _target, Color _target_color )
		{ 
			Color _selection_color = ( _target.Active?ActiveTargetColor:InteractionColor);
			
			DrawTargetGizmos( _target, _target_color, _selection_color );
		}

		private float DrawTargetGizmos( TargetObject _target )
		{ 
			Color _target_color = ( _target.Active?ActiveTargetColor:TargetColor);
			Color _selection_color = ( _target.Active?ActiveTargetColor:InteractionColor);

			return DrawTargetGizmos( _target, _target_color, _selection_color );
		}

		/// <summary>
		/// Draws the target gizmos.
		/// </summary>
		/// <param name="_target">_target.</param>
		private float DrawTargetGizmos( TargetObject _target, Color _target_color, Color _selection_color )
		{ 
			if( _target == null || _target.IsValidAndReady == false )
				return GetLevel( _target.TargetGameObject );
			
			float _level = GetLevel( _target.TargetGameObject );

			#if UNITY_EDITOR

			//if( ! Application.isPlaying )
			//	_target.UpdateTargetMovePositionOffset( false );

			Vector3 _target_pos = _target.TargetTransformPosition;
			//Vector3 _target_direction = _target.TargetDirection;
			Vector3 _target_pos_offset = _target.TargetOffsetPosition;


			Vector3 _target_pos_move = _target.TargetMovePosition;
			float _target_selection_range = _target.Selectors.SelectionRange;
			float _target_selection_angle = _target.Selectors.SelectionAngle;

			Vector3 _target_pos_top = _target_pos;
			Vector3 _target_pos_bottom = _target_pos;


			_target_pos.y = _level;
			_target_pos_offset.y = _level;
			_target_pos_move.y = _level;
			_target_pos_top.y = _level + 3;
			_target_pos_bottom.y = _level;

			Color _previous_color = Gizmos.color;
			CustomGizmos.Color( _target_color );

			// SELECTION RANGE BEGIN
			if( _target.Selectors.UseSelectionRange && _target_selection_range > 0 )
			{
				if( _target_selection_angle == 0 )
					_target_selection_angle = 180;

				Vector3 _from = PositionTools.GetDirectionPosition( _target.TargetGameObject.transform, - _target_selection_angle, _target_selection_range ) - _target_pos;
				_from.y = 0;

				CustomGizmos.HandlesColor( new Color( TargetSelectionRangeColor.r,TargetSelectionRangeColor.g,TargetSelectionRangeColor.b, (_target.Active?TargetSelectionRangeColor.a + 0.25f:TargetSelectionRangeColor.a) ) );
				CustomGizmos.DrawSolidArc( _target_pos, Vector3.up, _from, _target_selection_angle * 2, _target_selection_range );

				CustomGizmos.GizmosColor( new Color( TargetSelectionRangeColor.r,TargetSelectionRangeColor.g,TargetSelectionRangeColor.b, 1 ) );
				CustomGizmos.ArrowArc( _target.TargetGameObject.transform, _target_selection_range, CustomGizmos.GetBestDegrees( _target_selection_range, _target_selection_angle ), - _target_selection_angle, _target_selection_angle, _level, true );
			
			
				if( _target_selection_angle < 180 )
				{
					Transform _target_transform = _target.TargetGameObject.transform;
					Vector3 _center = _target_transform.position;
					Vector3 _center_pos = PositionTools.GetDirectionPosition( _target_transform, 0, _target_selection_range + ( _target_selection_range / 10 ) );
					Vector3 _left_pos = PositionTools.GetDirectionPosition( _target_transform, - _target_selection_angle, _target_selection_range - 0.25f );
					Vector3 _right_pos = PositionTools.GetDirectionPosition( _target_transform, _target_selection_angle , _target_selection_range - 0.25f );
					
					_center.y = _level;
					_center_pos.y = _level;
					_left_pos.y = _level;
					_right_pos.y = _level;
					
					//Gizmos.DrawLine( _center, _center_pos );
					Gizmos.DrawLine( _center, _left_pos );
					Gizmos.DrawLine( _center, _right_pos ); /**/
				}
			}
			// SELECTION RANGE END

			CustomGizmos.Color( _target_color );
	

			// TARGET ICON BEGIN
			Gizmos.DrawLine( _target_pos_bottom, _target_pos_top );
			Gizmos.DrawIcon( _target_pos_top, "ice_waypoint.png");
			// TARGET ICON END

			// TARGET DIRECTION
			CustomGizmos.Arrow( 0, _target_pos, _target.TargetGameObject.transform.rotation, 1 );
			//Gizmos.DrawCube( _target_pos,  

			// TARGET STOP DISTANCE BEGIN
			CustomGizmos.HandlesColor( new Color( TargetStoppingDistanceColor.r,TargetStoppingDistanceColor.g,TargetStoppingDistanceColor.b, (_target.Active?TargetStoppingDistanceColor.a + 0.25f:TargetStoppingDistanceColor.a) ) );
			CustomGizmos.DrawSolidDisc( _target_pos_move , Vector3.up, _target.Move.StoppingDistance );

			if( ! _target.Move.IgnoreLevelDifference )
				CustomGizmos.Sphere( 0, _target_pos_move , Quaternion.identity, _target.Move.StoppingDistance * 2 );

			CustomGizmos.GizmosColor( new Color( TargetStoppingDistanceColor.r,TargetStoppingDistanceColor.g,TargetStoppingDistanceColor.b, 1 ) );
			CustomGizmos.Circle( _target_pos_move, _target.Move.StoppingDistance , 5, false ); 

			if( ShowText )
			{
				string _text = _target.TargetGameObject.name + "\n" +
					"Target Type : " + _target.EntityType.ToString() + ( _target.Active ? " (ACTIVE)" : " (" + _target.Selectors.Status.ToString() + ")" ) + "\n" +
					"Target Stopping Distance : " + _target.Move.StoppingDistance + "\n";

				if( _target.Active )
				{
					_text += "\nTarget Speed (Velocity) : " + _target.TargetSpeed + " " + _target.TargetVelocity.ToString(); 
				}

				CustomGizmos.Text( _text , _target_pos + ( Vector3.forward * ( _target.Move.StoppingDistance ) ), Gizmos.color, 12, FontStyle.Italic );
			}

			if( _target.Active )
				CustomGizmos.Cylinder( 0, _target_pos_move + (Vector3.up * _target.Move.StoppingDistance), Quaternion.Euler( 90,0,0) , _target.Move.StoppingDistance * 2 );
			// TARGET STOP DISTANCE END


			// MAX AND RANDOM RANGE BEGIN
			if( _target.Move.MovePositionType == TargetMovePositionType.Offset )
			{
				Color _color_handle = new Color( TargetRandomRangeColor.r,TargetRandomRangeColor.g,TargetRandomRangeColor.b, TargetRandomRangeColor.a );
				Color _color_gizmos = new Color( TargetRandomRangeColor.r,TargetRandomRangeColor.g,TargetRandomRangeColor.b, 1 );

				if( _target.Move.UseRandomRect )
				{
					Vector3 pos = _target_pos_offset;
					float _sd = _target.Move.StoppingDistance;

					float _x = _target.Move.RandomRangeRect.x * 0.5f;
					float _z = _target.Move.RandomRangeRect.z * 0.5f;

					//Gizmos.color = _color_gizmos;
					//Gizmos.DrawCube( pos + new Vector3( 0, _target.Move.RandomRangeRect.y * 0.5f, 0 ), _target.Move.RandomRangeRect );
			
					Vector3[] verts = new Vector3[] { 
						new Vector3( pos.x - _x , pos.y , pos.z - _z ),
						new Vector3( pos.x - _x , pos.y , pos.z + _z ),
						new Vector3( pos.x + _x , pos.y , pos.z + _z ),
						new Vector3( pos.x + _x , pos.y , pos.z - _z ) };

					Vector3[] _max_verts = (Vector3[])verts.Clone();
					_max_verts[0].x -= _sd;
					_max_verts[0].z -= _sd;
					_max_verts[1].x -= _sd;
					_max_verts[1].z += _sd;
					_max_verts[2].x += _sd;
					_max_verts[2].z += _sd;
					_max_verts[3].x += _sd;
					_max_verts[3].z -= _sd;

					_max_verts = PositionTools.RotatePointAroundPivot( pos, _max_verts, _target.TargetOffsetAngle );

					CustomGizmos.HandlesColor( Color.white );
					CustomGizmos.DrawSolidRectangleWithOutline( _max_verts, _color_handle, _color_gizmos );


					verts = PositionTools.RotatePointAroundPivot( pos, verts, _target.TargetOffsetAngle );


					float _dotsize = 5;
					CustomGizmos.HandlesColor( _color_gizmos );
					CustomGizmos.DrawDottedLine( verts[0], verts[1], _dotsize );
					CustomGizmos.DrawDottedLine( verts[1], verts[2], _dotsize );
					CustomGizmos.DrawDottedLine( verts[2], verts[3], _dotsize );
					CustomGizmos.DrawDottedLine( verts[3], verts[0], _dotsize );

					if( ShowText )
					{
						string _text = _target.TargetGameObject.name + "\n" +
							"Random Range (rectable): " + ( _target.Move.RandomRangeRect.x + "x" +_target.Move.RandomRangeRect.z + "+" + _target.Move.StoppingDistance );
						Color _text_color = new Color( TargetRandomRangeColor.r,TargetRandomRangeColor.g,TargetRandomRangeColor.b, 1 );
						Vector3 _text_pos = _target_pos + ( Vector3.forward * ( _target.Move.RandomRange + _target.Move.StoppingDistance ) );
						CustomGizmos.Text( _text , _text_pos , _text_color, 12, FontStyle.Italic );
					}

				}
				else
				{
					CustomGizmos.HandlesColor( _color_handle );
					CustomGizmos.DrawSolidDisc( _target_pos_offset , Vector3.up, _target.Move.RandomRange + _target.Move.StoppingDistance );
				
					CustomGizmos.GizmosColor( _color_gizmos );
					CustomGizmos.Circle( _target_pos_offset, _target.Move.RandomRange + _target.Move.StoppingDistance  , 5, false );// MAX RANGE
					CustomGizmos.Circle( _target_pos_offset, _target.Move.RandomRange, 5, true );  // RANDOM RANGE 

					if( ShowText )
					{
						string _text = _target.TargetGameObject.name + "\n" +
							"Random Range (circular): " + ( _target.Move.RandomRange + "+" + _target.Move.StoppingDistance );
						Color _text_color = new Color( TargetRandomRangeColor.r,TargetRandomRangeColor.g,TargetRandomRangeColor.b, 1 );
						Vector3 _text_pos = _target_pos + ( Vector3.forward * ( _target.Move.RandomRange + _target.Move.StoppingDistance ) );
						CustomGizmos.Text( _text , _text_pos , _text_color, 12, FontStyle.Italic );
					}
				}

				CustomGizmos.HandlesColor( _color_handle );
				CustomGizmos.GizmosColor( _color_gizmos );

				// SPECIAL CASES FOR INTERACTOR WHICH COULD HAVE SEVERAL SELECTION RANGES
				if( _target.Type != TargetType.INTERACTOR )
				{
					if( PositionTools.Distance( _target_pos_move, _target_pos_offset ) <= _target.Move.StoppingDistance )
						CustomGizmos.OffsetPath( _target_pos, 0.0f, _target_pos_offset, _target.Move.StoppingDistance );
					else
						Gizmos.DrawLine( _target_pos, _target_pos_offset );

					CustomGizmos.OffsetPath( _target_pos_offset, 0.0f, _target_pos_move, _target.Move.StoppingDistance );

					// DIRECTION
					//CustomGizmos.Arrow( _target_pos, _target_direction * _target.Move.StoppingDistance , 0.5f, 20 );
				}
			}
			// MAX AND RANDOM RANGE END

			BehaviourModeObject _mode = CreatureControl.Creature.Behaviour.GetBehaviourModeByKey( _target.Behaviour.BehaviourModeKey );			
			DrawBehaviourModeGizmos( _target, _mode );

			CustomGizmos.Text( _target.TargetGameObject.name, _target_pos, _target_color, 16, FontStyle.Bold  );

			// PREVIOUS COLOR
			CustomGizmos.Color( _previous_color );

			#endif

			return _level;
		}
	}

	/// <summary>
	/// Pointer object.
	/// </summary>
	[System.Serializable]
	public class PathObject : ICEObject
	{

		public void NavMeshPath( NavMeshAgent _agent )
		{/*
		GameObject _object = new GameObject();
		_object.transform.parent = _agent.gameObject.transform;
		LineRenderer _line = _object.AddComponent<LineRenderer>();
		_line.SetColors(Color.white,Color.white);
		_line.SetWidth(0.1f,0.1f);
		//Get def material
		
		_line.gameObject.renderer.material.color = Color.white;
		_line.gameObject.renderer.material.shader = Shader.Find("Sprites/Default");
		_line.gameObject.AddComponent<LineScript>();
		_line.SetVertexCount(_agent.path.corners.Length+1);
		int i = 0;
		foreach(Vector3 v in p.corners)
		{
			_line.SetPosition(i,v);
			//Debug.Log("position agent"+g.transform.position);
			//Debug.Log("position corner = "+v);
			i++;
		}
		_line.SetPosition(p.corners.Length,_agent.destination);
		_line.gameObject.tag = "ligne";*/
		}
	}
	/// <summary>
	/// Pointer object.
	/// </summary>
	[System.Serializable]
	public class PointerObject : ICEObject
	{
		public PointerObject(){}
		public PointerObject( Color _color )
		{
			PointerColor = _color;
		}
		public PointerObject( Color _color, string _name )
		{
			PointerColor = _color;
			PointerName = ( string.IsNullOrEmpty( _name ) ? "Pointer" : _name );
		}
		public PointerObject( Vector3 _size, Color _color, string _name )
		{
			PointerSize = _size;
			PointerColor = _color;
			PointerName = ( string.IsNullOrEmpty( _name ) ? "Pointer" : _name );
		}
		
		[SerializeField]
		private bool m_Enabled = false;
		public bool Enabled{
			set{
				if( ! value )
					DestroyPointer();
				
				m_Enabled = value;
			}
			get{ return m_Enabled; }
		}
		
		public Color PointerColor = new Vector4(1f, 0.0f, 0.0f, 0.20f);
		public string PointerName = "Pointer";
		public Vector3 PointerSize = new Vector3( 0.5f ,0.5f , 0.5f );
		
		[SerializeField]
		private PrimitiveType m_PointerType = PrimitiveType.Cylinder;
		public PrimitiveType PointerType{
			set{
				if( m_PointerType != value )
				{
					m_PointerType = value;
					DestroyPointer();
				}
			}
			get{ return m_PointerType; }
		}
		
		private GameObject m_Pointer = null;
		public GameObject Pointer{
			get{
				
				if( Enabled )
				{
					if( m_Pointer == null )
						m_Pointer = GameObject.CreatePrimitive( PointerType );
					
					m_Pointer.GetComponent<Collider>().enabled = false;
					m_Pointer.SetActive( Enabled );
					//m_Pointer.transform.position = m_TargetMovePosition;
					m_Pointer.transform.localScale = PointerSize;
					m_Pointer.GetComponent<Renderer>().material.color = PointerColor;
					Material _mat = m_Pointer.GetComponent<Renderer>().material;

					_mat.SetFloat("_Mode", 3);
					_mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
					_mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
					_mat.SetInt("_ZWrite", 0);
					_mat.DisableKeyword("_ALPHATEST_ON");
					_mat.DisableKeyword("_ALPHABLEND_ON");
					_mat.EnableKeyword("_ALPHAPREMULTIPLY_ON");
					_mat.renderQueue = 3000;

					m_Pointer.name = ( string.IsNullOrEmpty( PointerName ) ? "Pointer" : PointerName );
				}

				
				return m_Pointer;
			}
		}

		public Vector3 Position{
			get{ return ( Pointer != null ? Pointer.transform.position : Vector3.zero ); }
			set{ if( Pointer != null ) Pointer.transform.position = new Vector3( value.x, value.y + PointerSize.y, value.z ); }
		}
		
		private void DestroyPointer()
		{
			if( m_Pointer != null )
			{
				UnityEngine.Object.Destroy ( m_Pointer );
				m_Pointer = null;
			}
		}

	}


	[System.Serializable]
	public class DebugObject : ICEOwnerObject
	{
		public DebugObject(){}
		public DebugObject( ICEWorldBehaviour _component ) : base( _component ) {}

		private ICECreatureControl m_CreatureControl = null;
		public ICECreatureControl CreatureControl{
			get{ return m_CreatureControl = ( m_CreatureControl == null && Owner != null ? Owner.GetComponent<ICECreatureControl>():m_CreatureControl ); }
		}

		public override void Init( ICEWorldBehaviour _component )
		{
			base.Init( _component );
			Gizmos.Init( _component );
		}

		[SerializeField]
		private PointerObject m_MovePointer = new PointerObject( new Vector3( 0.5f, 0.25f, 0.5f ), new Vector4(1f, 0.0f, 0.0f, 0.20f), "MovePointer" );
		public PointerObject MovePointer{
			get{ return m_MovePointer = ( m_MovePointer == null ? new PointerObject( new Vector3( 0.5f, 0.25f, 0.5f ), new Vector4(1f, 0.0f, 0.0f, 0.20f), "MovePointer" ): m_MovePointer ); }
		}
		
		[SerializeField]
		private PointerObject m_TargetPositionPointer = new PointerObject( new Vector3( 0.25f, 1f, 0.25f ), new Vector4(0, 0.0f, 1f, 0.20f), "TargetMovePositionPointer" );
		public PointerObject TargetMovePositionPointer{
			get{ return m_TargetPositionPointer = ( m_TargetPositionPointer == null ? new PointerObject( new Vector3( 0.25f, 1f, 0.25f ), new Vector4(0, 0.0f, 1f, 0.20f), "TargetMovePositionPointer" ): m_TargetPositionPointer); }
		}

		[SerializeField]
		private PointerObject m_DesiredTargetMovePositionPointer = new PointerObject( new Vector3( 0.15f, 2f, 0.15f ), new Vector4(0, 1f, 0.0f, 0.20f), "DesiredTargetMovePositionPointer" );
		public PointerObject DesiredTargetMovePositionPointer{
			get{ return m_DesiredTargetMovePositionPointer = ( m_DesiredTargetMovePositionPointer == null ? new PointerObject( new Vector3( 0.15f, 2f, 0.15f ), new Vector4(0, 1f, 0.0f, 0.20f), "DesiredTargetMovePositionPointer" ): m_DesiredTargetMovePositionPointer ); }
		}

		[SerializeField]
		private GizmoObject m_Gizmos = null;
		public GizmoObject Gizmos{
			get{ return m_Gizmos = ( m_Gizmos == null ? new GizmoObject( OwnerComponent ): m_Gizmos); }
			set{ Gizmos.Copy( value ); }
		}
	}
}
