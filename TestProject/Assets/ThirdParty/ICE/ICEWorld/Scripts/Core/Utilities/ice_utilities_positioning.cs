// ##############################################################################
//
// ice_utilities_positioning.cs | ICE.World.Utilities.PositionTools
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

using ICE;
using ICE.World;
using ICE.World.EnumTypes;

namespace ICE.World.Utilities
{
	public static class PositionTools 
	{
		/* ToDo : cleanup member functions
		public static class Random 
		{
			
			/// <summary>
			/// Gets the random circle position within the specified radius.
			/// </summary>
			/// <returns>The random circle position.</returns>
			/// <param name="_origin">Origin.</param>
			/// <param name="_radius">Radius.</param>
			public static Vector3 CirclePosition( Vector3 _origin, float _radius ) 
			{ 
				Vector3 _position = _origin;
				Vector2 _point = UnityEngine.Random.insideUnitCircle * _radius;

				_position.x += _point.x;
				_position.z += _point.y;

				return _position;
			}
		}*/

		public static bool PositionReached ( Vector3 _pos_1, Vector3 _pos_2, float _distance, bool _ignore_level = false )
		{
			if( _ignore_level )
			{
				_pos_1.y = 0;
				_pos_2.y = 0;
			}

			return ( Distance( _pos_1, _pos_2 ) <= _distance ? true : false );
		}

		public static Vector3 OverGroundHeading( Vector3 _origin, Vector3 _target ){
			Vector3 _heading = _target - _origin;
			_heading.y = 0;

			return _heading;
		}

		public static Vector3 Heading( Vector3 _origin, Vector3 _target ){
			return _target - _origin;
		}

		public static Vector3 Direction( Vector3 _heading ){
			return (_heading != Vector3.zero ? _heading / _heading.magnitude : Vector3.zero );
		}

		public static float Speed( Vector3 _direction ){
			return Mathf.Clamp01( _direction.magnitude );
		}



		public static void ComputeSpeedDirection ( Transform root, Vector3 _target, ref float _speed, ref float _direction )
		{
			Vector3 worldDirection = Direction( OverGroundHeading( root.position, _target ) );

			_speed = Speed( worldDirection );
			if (_speed > 0.01f) { // dead zone
				Vector3 axis = Vector3.Cross (root.forward, worldDirection);
				_direction = Vector3.Angle (root.forward, worldDirection) / 180.0f * (axis.y < 0 ? -1 : 1);
			} else {
				_direction = 0.0f; 
			}
		}   

		/// <summary>
		/// Gets a clamped position by using the specified start and end vector.
		/// </summary>
		/// <returns>The clamped position.</returns>
		/// <param name="_start">Start.</param>
		/// <param name="_end">End.</param>
		/// <param name="_level">Level.</param>
		/// <param name="_precision">Step.</param>
		public static Vector3 GetClampedPosition( Vector3 _start, Vector3 _end, float _level, float _step_angle = 45 )
		{
			float _distance = Distance( _start, _end );
			Quaternion _rot = Quaternion.FromToRotation( Vector3.right, _end - _start );
			Vector3 _euler = _rot.eulerAngles;
			float _angle = _step_angle * (int)Mathf.Round( _euler.y / _step_angle );
			float _end_x = _start.x + _distance * Mathf.Cos(_angle * (Mathf.PI / 180f));
			float _end_z = _start.z + _distance * Mathf.Sin(-_angle * (Mathf.PI / 180f));
			return new Vector3( _end_x, _level, _end_z );
		}


		/// <summary>
		/// Transforms position from world space to local space unaffected by scale
		/// </summary>
		/// <returns>The inverse transform point.</returns>
		/// <param name="_transform">Transform.</param>
		/// <param name="_point">Point.</param>
		public static Vector3 FixInverseTransformPoint( Transform _transform, Vector3 _point )
		{ 
			if( _transform == null )
				return _point;

			_point = _transform.InverseTransformPoint( _point ); 

			_point.x = _point.x*_transform.lossyScale.x;
			_point.y = _point.y*_transform.lossyScale.y;
			_point.z = _point.z*_transform.lossyScale.z;	

			return _point;
		}

		/// <summary>
		/// Transforms position from local space to world space unaffected by scale
		/// </summary>
		/// <returns>The transform point.</returns>
		/// <param name="_transform">Transform.</param>
		/// <param name="_point">Point.</param>
		public static Vector3 FixTransformPoint( Transform _transform, Vector3 _point )
		{ 
			if( _transform == null )
				return _point;

			_point.x = _point.x/_transform.lossyScale.x;
			_point.y = _point.y/_transform.lossyScale.y;
			_point.z = _point.z/_transform.lossyScale.z;

			return _transform.TransformPoint( _point ); 
		}

		/// <summary>
		/// Transforms position from world space to local space unaffected by scale
		/// </summary>
		/// <returns>The inverse transform point.</returns>
		/// <param name="_transform">Transform.</param>
		/// <param name="_point">Point.</param>
		public static Vector3 FixInverseTransformDirection( Transform _transform, Vector3 _direction )
		{ 
			if( _transform == null )
				return _direction;

			_direction = _transform.InverseTransformPoint( _direction ); 

			_direction.x = _direction.x*_transform.lossyScale.x;
			_direction.y = _direction.y*_transform.lossyScale.y;
			_direction.z = _direction.z*_transform.lossyScale.z;	

			return _direction;
		}

		/// <summary>
		/// Transforms position from local space to world space unaffected by scale
		/// </summary>
		/// <returns>The transform point.</returns>
		/// <param name="_transform">Transform.</param>
		/// <param name="_point">Point.</param>
		public static Vector3 FixTransformDirection( Transform _transform, Vector3 _direction )
		{ 
			if( _transform == null )
				return _direction;

			_direction.x = _direction.x/_transform.lossyScale.x;
			_direction.y = _direction.y/_transform.lossyScale.y;
			_direction.z = _direction.z/_transform.lossyScale.z;

			return _transform.TransformPoint( _direction ); 
		}

		/// <summary>
		/// Gets the random circle position within the specified radius.
		/// </summary>
		/// <returns>The random circle position.</returns>
		/// <param name="_origin">Origin.</param>
		/// <param name="_radius">Radius.</param>
		public static Vector3 GetRandomCirclePosition( Vector3 _origin, float _radius ) 
		{ 
			Vector3 _position = _origin;
			Vector2 _point = UnityEngine.Random.insideUnitCircle * _radius;

			_position.x += _point.x;
			_position.z += _point.y;

			return _position;
		}

		/// <summary>
		/// Gets the random position within the defined circle.
		/// </summary>
		/// <returns>The random circle position.</returns>
		/// <param name="_origin">Origin.</param>
		/// <param name="_min">Minimum.</param>
		/// <param name="_max">Max.</param>
		public static Vector3 GetRandomCirclePosition( Vector3 _origin, float _min, float _max ) 
		{ 
			Vector3 _position = _origin;
			float _distance = UnityEngine.Random.Range( _min, _max );
			float _angle = UnityEngine.Random.Range( 0, 360 );
			_position = GetPositionByAngleAndRadius( _position, _angle, _distance );

			return _position;
		}

		/// <summary>
		/// Gets a random position within the specified rect.
		/// </summary>
		/// <returns>The random rect position.</returns>
		/// <param name="x">The x coordinate.</param>
		/// <param name="z">The z coordinate.</param>
		/// <param name="centered">If set to <c>true</c> centered.</param>
		public static Vector3 GetRandomRectPosition( float x = 10.0f, float z = 10.0f, bool centered = false ){ 
			return GetRandomRectPosition( Vector3.zero, x,  z, centered ) ;
		}

		public static Vector3 GetRandomRectPosition( Transform _transform, Vector3 _size ){
			return FixTransformPoint( _transform, GetRandomRectPosition( Vector3.zero, _size, true ) );
		}

		/// <summary>
		/// Gets the random position within the defined rect.
		/// </summary>
		/// <returns>The random rect position.</returns>
		/// <param name="origin">Origin.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="z">The z coordinate.</param>
		/// <param name="centered">If set to <c>true</c> centered.</param>
		public static Vector3 GetRandomRectPosition( Vector3 _origin, float _x = 10.0f, float _z = 10.0f, bool _centered = false ) { 
			return GetRandomRectPosition( _origin, new Vector3( _x, 0, _z ), _centered ) ;
		}
			
		/// <summary>
		/// Gets the random position within the defined rect.
		/// </summary>
		/// <returns>The random rect position.</returns>
		/// <param name="origin">Origin.</param>
		/// <param name="_size">Size.</param>
		/// <param name="_centered">If set to <c>true</c> centered.</param>
		public static Vector3 GetRandomRectPosition( Vector3 _origin, Vector3 _size, bool _centered = false ) 
		{ 
			if( Mathf.Approximately( _size.magnitude, 0 ) )
				return _origin;
				
			Vector3 _position = _origin;
			Vector3 _min_size = Vector3.zero;
			Vector3 _max_size = _size;
			if( _centered )
			{
				_max_size = _size * 0.5f;
				_min_size = - _max_size; 
			}

			//SystemTools.UpdateRandomSeed();
			_position.x += ( _max_size.x > 0 ? UnityEngine.Random.Range( _min_size.x, _max_size.x ) : 0 );
			_position.z += ( _max_size.z > 0 ? UnityEngine.Random.Range( _min_size.z, _max_size.z ) : 0 );
			_position.y += ( _max_size.y > 0 ? UnityEngine.Random.Range( _min_size.y, _max_size.y ) : 0 );

			return _position;
		}


		/// <summary>
		/// Gets the random position.
		/// </summary>
		/// <returns>The random position.</returns>
		/// <param name="_position">Position.</param>
		/// <param name="_radius">Radius.</param>
		public static Vector3 GetRandomPosition( Vector3 _position, float _radius ) { 
			
			if( _radius == 0 )
				return _position;
			
			Vector2 _new_circle_point = UnityEngine.Random.insideUnitCircle * _radius;
			
			Vector3 _new_position = Vector3.zero;
			_new_position.x = _position.x + _new_circle_point.x;
			_new_position.z = _position.z + _new_circle_point.y;
			_new_position.y = GetGroundLevel( _position );

			return _new_position;
			
		}

		public static Vector3 GetDirectionPosition( Transform _transform, float _angle, float _distance )
		{
			if( _transform == null )
				return Vector3.zero;
			
			_angle += _transform.eulerAngles.y;
			
			if( _angle > 360 )
				_angle = _angle - 360;
			
			Vector3 _world_offset = GetPositionByAngleAndRadius( _transform.position, _angle, _distance );
			
			return _world_offset;
		}

		public static float GetGroundLevel( Vector3 _position, GroundCheckType _type, LayerMask _layerMask, float _min_offset = 0.5f, float _max_offset = 1000, float _base_offset = 0 )
		{
			if( _type == GroundCheckType.NONE )
				return _position.y;
			
			if( _type == GroundCheckType.RAYCAST )
			{
				RaycastHit hit;
				if( Physics.Raycast( new Vector3( _position.x, _position.y + _min_offset + ( _base_offset * -1 ), _position.z ), Vector3.down, out hit, Mathf.Infinity, _layerMask, WorldManager.TriggerInteraction ) )
					_position.y = hit.point.y;
				else if( Physics.Raycast( new Vector3( _position.x, _position.y + _max_offset + ( _base_offset * -1 ) , _position.z ), Vector3.down, out hit, Mathf.Infinity, _layerMask, WorldManager.TriggerInteraction ) )
					_position.y = hit.point.y;
				else if( Terrain.activeTerrain != null )
					_position.y = Terrain.activeTerrain.SampleHeight( _position ) + Terrain.activeTerrain.transform.position.y;
				
			}
			else if( Terrain.activeTerrain != null )
				_position.y = Terrain.activeTerrain.SampleHeight( _position ) + Terrain.activeTerrain.transform.position.y;

			return _position.y;
		}

		public static float GetGroundLevel( Vector3 _position, float _min_offset = 0.5f, float _max_offset = 1000, float _base_offset = 0 )
		{
			RaycastHit hit;
			LayerMask _mask = Physics.DefaultRaycastLayers;

			if( Physics.Raycast( new Vector3( _position.x, _position.y + _min_offset + ( _base_offset * -1 ), _position.z ), Vector3.down, out hit, Mathf.Infinity, _mask, WorldManager.TriggerInteraction ) )
				_position.y = hit.point.y;
			else if( Physics.Raycast( new Vector3( _position.x, _position.y + _max_offset + ( _base_offset * -1 ) , _position.z ), Vector3.down, out hit, Mathf.Infinity, _mask, WorldManager.TriggerInteraction ) )
				_position.y = hit.point.y;	
			else if( Terrain.activeTerrain != null )
				_position.y = Terrain.activeTerrain.SampleHeight( _position ) + Terrain.activeTerrain.transform.position.y;

			return _position.y;
		}





		public static float LevelDeviation( Transform _transform, Vector3 _position )
		{
			Vector3 _heading = new Vector3( 0, _position.y - _transform.position.y , 0 );
			//Vector3 _forward = _transform.TransformDirection(Vector3.forward) - _heading.normalized;

			return DirectionAngle( Vector3.forward, Vector3.right, _heading );
		}

		public static float CourseDeviation( Quaternion _origin, Quaternion _target ){
				Quaternion _diff = Quaternion.Inverse( _origin ) * _target;
				return Mathf.Abs( MathTools.EulerToSigned( _diff.eulerAngles ).y );
		}

		public static float CourseDeviation( Transform _transform, Vector3 _position )
		{
			Vector3 _heading =  _position - _transform.position;
			Vector3 _forward = _transform.TransformDirection(Vector3.forward) - _heading.normalized;
			//Vector3 _forward_2 = _transform.TransformDirection( _heading );
			return DirectionAngle( _transform.forward, _transform.up, _forward );
		}

		public static float GetDirectionAngleByPosition( Transform _transform, Vector3 _target )
		{
			return DirectionAngle( _transform.forward, Direction( OverGroundHeading( _transform.position, _target ) ) );
		}

		public static float DirectionAngle( Vector3 _forward, Vector3 _direction )
		{
			Vector3 _axis = Vector3.Cross(_forward , _direction );
			return Vector3.Angle( _forward, _direction ) / 180.0f * (_axis.y < 0 ? -1 : 1);
		}

		public static float DirectionAngle( Vector3 _forward, Vector3 _up, Vector3 _heading )
		{
			Vector3 _perpendicular = Vector3.Cross( _forward, _heading );
			return Vector3.Dot( _perpendicular , _up );
		}   

		public static Vector3[] RotatePointAroundPivot( Vector3 _pivot, Vector3[] _points, float _angle ){

			if( _points == null || _points.Length < 1 ) 
				return _points;

			for( int _i = 0 ; _i < _points.Length ; _i++ )
				_points[_i] = RotatePointAroundPivot( _pivot, _points[_i], new Vector3( 0, _angle , 0 ) );
			
			return _points;
		}

		public static Vector3 RotatePointAroundPivot( Vector3 _pivot, Vector3 _point, float _angle ){
			return RotatePointAroundPivot( _pivot, _point, new Vector3( 0, _angle , 0 ) );
		}

		public static Vector3 RotatePointAroundPivot( Vector3 _pivot, Vector3 _point, Vector3 _angles ){

			Vector3 dir = _point - _pivot; // get point direction relative to pivot
			dir = Quaternion.Euler( _angles ) * dir; // rotate it
			_point = dir + _pivot; // calculate rotated point

			return _point;
		}

		//returns negative value when left, positive when right, and 0 for forward/backward
		public static float DirectionAngleExt( Vector3 _forward, Vector3 _up, Vector3 _heading )
		{
			float _angle = DirectionAngle( _forward, _up, _heading );

			if( _angle > 1f )
				_angle = 1;
			else if( _angle < -1f )
				_angle = -1;
			else
				_angle = 0;
			
			return _angle;
		}  

		public static Vector3 GetPositionByDirectionAndRadius( Vector3 _origin, Vector3 _direction, float _radius )
		{ 
			float _rad = GetVectorAngleInDegree( _direction ) * Mathf.Deg2Rad;			
			return _origin + new Vector3( Mathf.Sin( _rad ) * _radius, _direction.y, Mathf.Cos( _rad ) * _radius );
		}

		public static Vector3 GetPositionByAngleAndRadius( Vector3 _origin, float _angle, float _radius )
		{ 
			float _rad = _angle * Mathf.Deg2Rad;			
			return _origin + new Vector3( Mathf.Sin( _rad ) * _radius, 0, Mathf.Cos( _rad ) * _radius );
		}



		/// <summary>
		/// Gets the relative position of the specified world position.
		/// </summary>
		/// <returns>The relative position.</returns>
		/// <param name="_transform">Transform.</param>
		/// <param name="_position">Position.</param>
		public static Vector3 GetRelativePosition( Transform _transform, Vector3 _position ){ 
			return FixInverseTransformPoint( _transform, _position );
		}

		/// <summary>
		/// Gets the world position of the specified local offset position.
		/// </summary>
		/// <returns>The world position.</returns>
		/// <param name="_transform">Transform.</param>
		/// <param name="_offset">Offset.</param>
		public static Vector3 GetWorldPosition( Transform _transform, Vector3 _offset ){
			return FixTransformPoint( _transform, _offset );
		}

		/// <summary>
		/// Gets the angle in degree.
		/// </summary>
		/// <returns>The angle in degree.</returns>
		/// <param name="_vector">Vector.</param>
		public static float GetVectorAngleInDegree( Vector3 _vector ){ 
			return Mathf.Atan2( _vector.x, _vector.z ) * Mathf.Rad2Deg;
		}

		/// <summary>
		/// Gets the normalized angle in degree.
		/// </summary>
		/// <returns>The normalized angle.</returns>
		/// <param name="_vector">Vector.</param>
		public static float GetNormalizedVectorAngle( Vector3 _vector ){ 
			return MathTools.NormalizeAngle( GetVectorAngleInDegree( _vector ) );
		}

		/// <summary>
		/// Gets the direction angle.
		/// </summary>
		/// <returns>The direction angle.</returns>
		/// <param name="_transform">Transform.</param>
		/// <param name="_position">Position.</param>
		public static float GetSignedDirectionAngle( Transform _transform, Vector3 _position )
		{
			if( _transform == null )
				return 0;

			float _angle = MathTools.NormalizeAngle( PositionTools.GetNormalizedVectorAngle( _position - _transform.position ) - _transform.eulerAngles.y );

			if( _angle > 180 )
				_angle -= 360; 

			return _angle;
		}

		/// <summary>
		/// Gets the position angle.
		/// </summary>
		/// <returns>The position angle.</returns>
		/// <param name="_transform">Transform.</param>
		/// <param name="_position">Position.</param>
		public static float GetPositionAngle( Transform _transform, Vector3 _position ){ 
			return GetNormalizedVectorAngle( GetRelativePosition( _transform, _position ) );
		}

		/// <summary>
		/// Gets the distance.
		/// </summary>
		/// <returns>The distance.</returns>
		/// <param name="_pos1">Pos1.</param>
		/// <param name="_pos2">Pos2.</param>
		/// <param name="_ignore_level">If set to <c>true</c> ignore level.</param>
		public static float GetDistance( Vector3 _pos1, Vector3 _pos2, bool _ignore_level ){
			return ( _ignore_level ? GetOverGroundDistance( _pos1, _pos2 ) : Distance( _pos1, _pos2 ) );
		}

		/// <summary>
		/// Gets the vertical distance.
		/// </summary>
		/// <returns>The vertical distance.</returns>
		/// <param name="_pos1">Pos1.</param>
		/// <param name="_pos2">Pos2.</param>
		public static float GetVerticalDistance( Vector3 _pos1, Vector3 _pos2 ){
			return ( _pos1.y - _pos2.y );
		}

		/// <summary>
		/// Gets the over ground distance (ignoring the level difference)
		/// </summary>
		/// <returns>The over ground distance.</returns>
		/// <param name="pos1">Pos1.</param>
		/// <param name="pos2">Pos2.</param>
		public static float GetOverGroundDistance( Vector3 _pos1, Vector3 _pos2 )
		{
			_pos1.y = 0;
			_pos2.y = 0;
			return Distance( _pos1, _pos2 );
		}

		/// <summary>
		/// Distance the specified _pos1 and _pos2.
		/// </summary>
		/// <param name="_pos1">Pos1.</param>
		/// <param name="_pos2">Pos2.</param>
		public static float Distance( Vector3 _pos1, Vector3 _pos2 ){
			return Vector3.Distance( _pos1, _pos2 );
		}
	}

}
