// ##############################################################################
//
// ice_utilities_math.cs | ICE.World.Utilities.MathTools
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

namespace ICE.World.Utilities
{
	public static class MathTools 
	{

		public static bool ContainsPoint( Vector3[] _points, Vector3 _point )
		{ 
			int _j = _points.Length - 1; 
			bool inside = false; 
			for( int _i = 0; _i < _points.Length; _j = _i++) 
			{ 
				if( ( 
					( _points[_i].z <= _point.z && _point.z < _points[_j].z ) || 
					( _points[_j].z <= _point.z && _point.z < _points[_i].z ) ) && 
					( _point.x < ( _points[_j].x - _points[_i].x ) * ( _point.z - _points[_i].z ) / ( _points[_j].z - _points[_i].z ) + _points[_i].x ) )
				{
					inside = ! inside; 
				}
			} 
			return inside; 
		}

		public static bool ContainsPoint( Vector2[] _points, Vector2 _point )
		{ 
			int _j = _points.Length - 1; 
			bool inside = false; 
			for( int _i = 0; _i < _points.Length; _j = _i++) 
			{ 
				if( ( 
					( _points[_i].y <= _point.y && _point.y < _points[_j].y ) || 
					( _points[_j].y <= _point.y && _point.y < _points[_i].y ) ) && 
					( _point.x < ( _points[_j].x - _points[_i].x ) * ( _point.y - _points[_i].y ) / ( _points[_j].y - _points[_i].y ) + _points[_i].x ) )
				{
					inside = ! inside; 
				}
			} 
			return inside; 
		}

		public static bool CompareVectors( Vector3 _a , Vector3 _b ){
			return CompareVectors( _a , _b, 0 );
		}
		/// <summary>
		/// Compares the vectors.
		/// </summary>
		/// <returns><c>true</c>, if vectors was compared, <c>false</c> otherwise.</returns>
		/// <param name="_a">A.</param>
		/// <param name="_b">B.</param>
		/// <param name="_angle_error">Angle error.</param>
		public static bool CompareVectors( Vector3 _a , Vector3 _b, float _angle_error )
		{
			//if they aren't the same length, don't bother checking the rest.
			if( ! Mathf.Approximately( _a.magnitude, _b.magnitude ) )
				return false;

			//A value between -1 and 1 corresponding to the angle.
			float _cos_angle_error = Mathf.Cos( _angle_error * Mathf.Deg2Rad );

			//The dot product of normalized Vectors is equal to the cosine of the angle between them.
			//So the closer they are, the closer the value will be to 1.  Opposite Vectors will be -1
			//and orthogonal Vectors will be 0.
			float _cos_angle = Vector3.Dot( _a.normalized, _b.normalized );
		

			//If angle is greater, that means that the angle between the two vectors is less than the error allowed.
			return ( _cos_angle_error >= _cos_angle ? true : false );
		}

		public static Vector2 BezierCurve( float _t, Vector2 _p1, Vector2 _p2, Vector2 _p3, Vector2 _p4 ){
			return Mathf.Pow( 1 - _t, 3 ) * _p1 + 3 * Mathf.Pow ( 1 - _t, 2 ) * _t * _p2 + 3 * ( 1 - _t ) * _t * _t * _p3 + Mathf.Pow( _t, 3 ) * _p4;
		}

		//caclulate the rotational difference from A to B
		public static Quaternion SubtractRotation( Quaternion _rotation, Vector3 _euler ){
			return Quaternion.Inverse( Quaternion.Euler( _euler ) ) * _rotation;		
		}

		//caclulate the rotational difference from A to B
		public static Quaternion SubtractRotation( Quaternion _b, Quaternion _a ){
			return Quaternion.Inverse( _a ) * _b;		
		}

		//Add rotation b to rotation a.
		public static Quaternion AddRotation( Quaternion _rotation, Vector3 _euler ){
			return _rotation * Quaternion.Euler( _euler );		
		}

		//Add rotation b to rotation a.
		public static Quaternion AddRotation( Quaternion _a, Quaternion _b ){
			return _a * _b;		
		}

		//Same as the build in TransformDirection(), but using a rotation instead of a transform.
		public static Vector3 TransformDirection( Quaternion _rotation, Vector3 _vector ){
			return _rotation * _vector;
		}

		//Same as the build in InverseTransformDirection(), but using a rotation instead of a transform.
		public static Vector3 InverseTransformDirection( Quaternion _rotation, Vector3 _vector ){
			return Quaternion.Inverse( _rotation ) * _vector; 
		}

		//Rotate a vector as if it is attached to an object with rotation "from", which is then rotated to rotation "to".
		//Similar to TransformWithParent(), but rotating a vector instead of a transform.
		public static Vector3 RotateVectorFromTo( Quaternion _from_rotation, Quaternion _to_rotation, Vector3 _vector ){
			//Note: comments are in case all inputs are in World Space.
			Quaternion _rotation = SubtractRotation( _to_rotation, _from_rotation );			//Output is in object space.
			Vector3 _local_direction = InverseTransformDirection( _from_rotation, _vector );	//Output is in object space.
			Vector3 _local_rotated_direction = _rotation * _local_direction;					//Output is in local space.
			return TransformDirection( _from_rotation, _local_rotated_direction );				//Output is in world space.
		}

		public static float FixedPercent( float _value )
		{
			if( _value < 0 ) _value = 0;
			if( _value > 100 ) _value = 100;

			return (float)System.Math.Round( _value, 2 );
		}

		/// <summary>
		/// Normalize the specified _value by using _min and _max.
		/// </summary>
		/// <param name="_value">Value.</param>
		/// <param name="_min">Minimum.</param>
		/// <param name="_max">Max.</param>
		public static float Normalize( float _value, float _min,float _max) {
			return (Mathf.Clamp( _value, _min, _max ) - _min) / (_max - _min);
		}

		/// <summary>
		/// Denormalize the specified _normalized by using _min and _max.
		/// </summary>
		/// <param name="_normalized">Normalized.</param>
		/// <param name="_min">Minimum.</param>
		/// <param name="_max">Max.</param>
		public static float Denormalize( float _normalized, float _min, float _max) {
			return ( _normalized * ( _max - _min ) + _min );
		}

		public static float NormalizePeak( float _value, float _min,float _max, float _pow ) {
			return Mathf.Pow( NormalizePeak( _value, _min,_max), _pow );
		}

		public static float NormalizePeak( float _value, float _min,float _max) {
			float _peak = ( _max + _min ) * 0.5f;
			return Mathf.Clamp01( ( _value >= _min && _value < _peak ? MathTools.Normalize( _value - _min, 0, _peak - _min ) : ( _value <= _max && _value > _peak ? MathTools.Normalize( _max - _value, 0, _peak - _min ) : ( _value == _peak ? 1f : 0f ) ) ) );
		}

		public static float NormalizeRange( float _value, float _min, float _max, float _fade ) {
			float _peak_min = _min + _fade;
			float _peak_max = _max - _fade;
			return Mathf.Clamp01( ( _value >= _min && _value < _peak_min ? MathTools.Normalize( _value - _min, 0, _peak_min - _min ) : ( _value <= _max && _value > _peak_max ? MathTools.Normalize( _max - _value, 0, _peak_max - _min ) : ( _value >= _peak_min && _value <= _peak_max ? 1f : 0f ) ) ) );
		}


		/// <summary>
		/// Degrees to radian.
		/// </summary>
		/// <returns>The to radian.</returns>
		/// <param name="_angle">Angle.</param>
		public static float DegreeToRadian( float _angle ){
			return Mathf.PI * _angle / 180f;
		}

		/// <summary>
		/// Radians to degree.
		/// </summary>
		/// <returns>The to degree.</returns>
		/// <param name="_angle">Angle.</param>
		public static float RadianToDegree( float _angle ){
			return _angle * ( 180f / Mathf.PI );
		}

		/// <summary>
		/// Normalizes the angle.
		/// </summary>
		/// <returns>The angle.</returns>
		/// <param name="_angle">Angle.</param>
		public static float NormalizeAngle( float _angle ) 
		{
			while( _angle > 360 )
				_angle -= 360;
			while( _angle < 0 )
				_angle += 360;
			return _angle;
		}

		public static float NormalizeAngleInDegree( float _angle )
		{
			_angle = _angle % 360;
			if(_angle < 0) 
				_angle += 360;
			return _angle;
		}

		/// <summary>
		/// Clamps the angle.
		/// </summary>
		/// <returns>The angle.</returns>
		/// <param name="_angle">Angle.</param>
		/// <param name="_min">Minimum.</param>
		/// <param name="_max">Max.</param>
		public static float ClampAngle180( float _angle, float _min, float _max )
		{
			if( _angle < 90 || _angle > 270 ) 
			{       
				if( _angle > 180 ) _angle -= 360;  // convert all angles to -180..+180
				if( _max > 180 ) _max -= 360;
				if( _min > 180 ) _min -= 360;
			}    
			_angle = Mathf.Clamp( _angle, _min, _max );

			if( _angle < 0 ) _angle += 360;  // if angle negative, convert to 0..360
			return _angle;
		}


		public static float ClampAngle360( float _angle, float _min, float _max ){
			return Mathf.Clamp( NormalizeAngle( _angle ), _min, _max );
		}

		public static float ClampAngle( float _angle, float _min, float _max )
		{
			_angle = Mathf.Repeat( _angle, 360 );
			_min = Mathf.Repeat( _min, 360 );
			_max = Mathf.Repeat( _max, 360 );

			bool _inverse = false;
			float _tmp_min = _min;
			float _tmp_angle = _angle;
			if( _min > 180 )
			{
				_inverse = !_inverse;
				_tmp_min -= 180;
			}

			if( _angle > 180 )
			{
				_inverse = !_inverse;
				_tmp_angle -= 180;
			}

			var _result = ( ! _inverse ? _tmp_angle > _tmp_min : _tmp_angle < _tmp_min );
			if( ! _result )
				_angle = _min;

			_inverse = false;
			_tmp_angle = _angle;
			float _tmp_max = _max;
			if( _angle > 180 )
			{
				_inverse = !_inverse;
				_tmp_angle -= 180;
			}

			if(_max > 180)
			{
				_inverse = !_inverse;
				_tmp_max -= 180;
			}

			_result = ( ! _inverse ? _tmp_angle < _tmp_max : _tmp_angle > _tmp_max );
			if( ! _result )
				_angle = _max;
			
			return _angle;
		}

		public static float SignedVectorAngle( Vector3 _v1, Vector3 _v2  )
		{
			float _angle = Vector3.Angle( _v1, _v2 );
			Vector3 _cross = Vector3.Cross( _v1, _v2 );
			return ( _cross.y < 0 ? -_angle : _angle );
		}

		public static float SignedVectorAngle( Vector3 _v1, Vector3 _v2, Vector3 _normal ){
			// angle in [0,180]
			float _angle = Vector3.Angle( _v1, _v2 );
			float _sign = Mathf.Sign( Vector3.Dot( _normal, Vector3.Cross( _v1, _v2 ) ) );

			return _angle * _sign;
		}

		public static float VectorAngle( Vector3 _v1, Vector3 _v2, Vector3 _normal ){
			return (  SignedVectorAngle( _v1, _v2, _normal ) + 360 ) % 360;
		}

		public static float CalculateAngularVelocity( Vector3 _velocity, Vector3 _velocity_max, Vector3 _velocity_default  )
		{
			if( _velocity.z == 0 )
				return _velocity_default.y;
				
			float _multiplier = Mathf.Clamp( 1.0f - MathTools.Normalize( Mathf.Abs( _velocity.z ), 0, _velocity_max.z ), 0.1f, 1f );
			return _velocity_default.y * Mathf.Pow( _multiplier, 2 );

		}

		public static Vector3 EulerToSigned( Vector3 _euler )
		{
			Vector3 _signed = Vector3.zero;

			_signed.x = ( _euler.x > 180 ? _euler.x - 360 : _euler.x );
			_signed.y = ( _euler.y > 180 ? _euler.y - 360 : _euler.y );
			_signed.z = ( _euler.z > 180 ? _euler.z - 360 : _euler.z );

			return _signed;
		}

		public static Vector3 SignedToEuler( Vector3 _signed )
		{
			Vector3 _euler = Vector3.zero;

			_euler.x = ( _signed.x < 0 ? _signed.x + 360 : _signed.x );
			_euler.y = ( _signed.y < 0 ? _signed.y + 360 : _signed.y );
			_euler.z = ( _signed.z < 0 ? _signed.z + 360 : _signed.z );

			return _euler;
		}

		public static float UnsignedAngle( float _signed ){
			return ( _signed < 0 ? _signed + 360 : _signed );
		}

		public static float SignedAngle( float _unsigned ){
			return ( _unsigned > 180 ? _unsigned - 360 : _unsigned );
		}

		public static Vector3 ClampEuler( Vector3 _euler, Vector3 _min, Vector3 _max ){
			return SignedToEuler( ClampSignedEuler( EulerToSigned( _euler ), EulerToSigned( _min ), EulerToSigned( _max ) ) );
		}

		public static Vector3 ClampSignedEuler( Vector3 _euler, Vector3 _min, Vector3 _max )
		{
			float _min_x = Mathf.Min( _min.x, _max.x );
			float _max_x = Mathf.Max( _min.x, _max.x );
			float _min_y = Mathf.Min( _min.y, _max.y );
			float _max_y = Mathf.Max( _min.y, _max.y );
			float _min_z = Mathf.Min( _min.z, _max.z );
			float _max_z = Mathf.Max( _min.z, _max.z );
			
			Vector3 _clamped = _euler;

			if( _euler.x < _min_x || _euler.x > _max_x )
				_clamped.x = Mathf.Clamp( _euler.x, _min_x, _max_x );
			if( _euler.y < _min_y || _euler.y > _max_y )
				_clamped.y = Mathf.Clamp( _euler.y, _min_y, _max_y );
			if( _euler.z < _min_z || _euler.z > _max_z )
				_clamped.z = Mathf.Clamp( _euler.z, _min_z, _max_z );

			return _clamped;
		}


	}
}
