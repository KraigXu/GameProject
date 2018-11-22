// ##############################################################################
//
// ice_utilities_system.cs | ICE.World.Utilities.SystemTools
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
using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;

using ICE;
using ICE.World;
using ICE.World.EnumTypes;

namespace ICE.World.Utilities
{





	public static class SystemTools 
	{
		public static List<string> EnumToList<T>()
		{
			List<string> _list = new List<string>();

			foreach( T _expression in System.Enum.GetValues( typeof(T) ) )
				_list.Add( _expression.ToString() );
			
			return _list;
		}

		/// <summary>
		/// Removes the '(Clone)' suffix of a name.
		/// </summary>
		/// <returns>The clean name.</returns>
		/// <param name="_name">Name.</param>
		public static string CleanName( string _name )
		{
			if( string.IsNullOrEmpty( _name ) )
				return "";

			int _clone_index = _name.IndexOf( "(Clone)" );
			if( _clone_index > 0 )
				return _name.Substring( 0, _clone_index );
			else
				return _name.Replace("(Clone)", ""); 
		}

		public static int LevenshteinDistance( string s, string t )
		{
			if (string.IsNullOrEmpty(s))
			{
				if (string.IsNullOrEmpty(t))
					return 0;
				return t.Length;
			}

			if (string.IsNullOrEmpty(t))
			{
				return s.Length;
			}

			int n = s.Length;
			int m = t.Length;
			int[,] d = new int[n + 1, m + 1];

			// initialize the top and right of the table to 0, 1, 2, ...
			for (int i = 0; i <= n; d[i, 0] = i++);
			for (int j = 1; j <= m; d[0, j] = j++);

			for (int i = 1; i <= n; i++)
			{
				for (int j = 1; j <= m; j++)
				{
					int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
					int min1 = d[i - 1, j] + 1;
					int min2 = d[i, j - 1] + 1;
					int min3 = d[i - 1, j - 1] + cost;
					d[i, j] = Math.Min(Math.Min(min1, min2), min3);
				}
			}
			return d[n, m];
		}

		public static string RemoveIllegalCharacters( string _text, string _pattern, bool _regex )
		{
			if( _regex )
				_text = System.Text.RegularExpressions.Regex.Replace( _text , _pattern, string.Empty ).Trim();
			else
			{
				string illegal_chars = _pattern;
				for( int i = 0 ; i < illegal_chars.Length ; i++ )
					_text = _text.Replace( illegal_chars[i].ToString() , string.Empty );
			}

			return _text;
		}

		public static string[] AddArrayValue( string[] _array, string _value, ref int _index )
		{
			_index = _array.Length;
			string[] _new = new string[ _index + 1 ];
			for( int _i = 0 ; _i < _array.Length ; _i++ )
				_new[ _i  ] = _array[ _i ];

			_new[ _index ] = _value;

			return _new;
		}

		public static bool IsValid( string _value ){
			return ( ! string.IsNullOrEmpty( _value ) && _value.Trim() != "" ? true : false );
		}

		public static void UpdateRandomSeed()
		{
			#if UNITY_5_4 || UNITY_5_4_OR_NEWER
				UnityEngine.Random.InitState( (int)System.DateTime.Now.Millisecond );
			#else
				UnityEngine.Random.seed = (int)System.DateTime.Now.Millisecond;
			#endif
		}

		public static Vector3 FindCenterPoint( Vector3[] _points )
		{
			Vector3 _center = new Vector3( 0, 0, 0 );
			foreach( Vector3 _point in _points )
				_center += _point;

			return _center / _points.Length;
		}
			
		public static Vector3 ClosestPointOnSurface( Collider _collider, Vector3 _position )
		{
			if( _collider as BoxCollider != null )
				return ClosestPointOnSurface( _collider as BoxCollider, _position );
			else if( _collider as SphereCollider != null )
				return ClosestPointOnSurface( _collider as SphereCollider, _position );
			else if( _collider as CapsuleCollider != null )
				return ClosestPointOnSurface( _collider as CapsuleCollider, _position );
			else
				return _position;
		}

		public static Vector3 ClosestPointOnSurface( SphereCollider _collider, Vector3 _position )
		{
			if( _collider == null )
				return _position;
			
			Vector3 _point = Vector3.zero;

			_point = _position - _collider.transform.position;
			_point.Normalize();

			_point *= _collider.radius * _collider.transform.localScale.x;
			_point += _collider.transform.position;

			return _point;
		}

		public static Vector3 ClosestPointOnSurface( BoxCollider _collider, Vector3 _position )
		{
			if( _collider == null )
				return _position;
			
			Transform _transform = _collider.transform;

			Vector3 _local_position_point = _transform.InverseTransformPoint( _position );

			// Now, shift it to be in the center of the box
			_local_position_point -= _collider.center; 

			//Pre multiply to save operations.
			Vector3 _half_size = _collider.size * 0.5f; 

			// Clamp the points to the collider's extents
			Vector3 _local_normal_point = new Vector3(
				Mathf.Clamp( _local_position_point.x, -_half_size.x, _half_size.x ),
				Mathf.Clamp( _local_position_point.y, -_half_size.y, _half_size.y ),
				Mathf.Clamp( _local_position_point.z, -_half_size.z, _half_size.z )
			);

			//Calculate distances from each edge
			float _distance_x = Mathf.Min(Mathf.Abs(_half_size.x - _local_normal_point.x), Mathf.Abs(-_half_size.x - _local_normal_point.x));
			float _distance_y = Mathf.Min(Mathf.Abs(_half_size.y - _local_normal_point.y), Mathf.Abs(-_half_size.y - _local_normal_point.y));
			float _distance_z = Mathf.Min(Mathf.Abs(_half_size.z - _local_normal_point.z), Mathf.Abs(-_half_size.z - _local_normal_point.z));

			// Select a face to project on
			if( _distance_x < _distance_y && _distance_x < _distance_z )
				_local_normal_point.x = Mathf.Sign(_local_normal_point.x ) * _half_size.x;
			else if( _distance_y < _distance_x && _distance_y < _distance_z )
				_local_normal_point.y = Mathf.Sign(_local_normal_point.y ) * _half_size.y;
			else if( _distance_z < _distance_x && _distance_z < _distance_y )
				_local_normal_point.z = Mathf.Sign(_local_normal_point.z ) * _half_size.z;

			// Now we undo our transformations
			_local_normal_point += _collider.center; 
					
			// Return resulting point
			return _transform.TransformPoint( _local_normal_point ); 	
		}


		public static Vector3 ClosestPointOnSurface( CapsuleCollider _collider, Vector3 _position )
		{
			if( _collider == null )
				return _position;
			
			Transform _transform = _collider.transform;

			// The length of the line connecting the center of both sphere
			float _line_length = _collider.height - _collider.radius * 2;

			Vector3 _direction = Vector3.up;

			Vector3 _upper_sphere = _direction * _line_length * 0.5f + _collider.center; // The position of the radius of the upper sphere in local coordinates
			Vector3 _lower_sphere = -_direction * _line_length * 0.5f + _collider.center; // The position of the radius of the lower sphere in local coordinates

			Vector3 _local_point = _transform.InverseTransformPoint(_position); // The position of the controller in local coordinates

			Vector3 _contact_point = Vector3.zero; // Contact point
			Vector3 _direction_point = Vector3.zero; // The point we need to use to get a direction vector with the controller to calculate contact point

			if (_local_point.y < _line_length * 0.5f && _local_point.y > -_line_length * 0.5f) // Controller is contacting with cylinder, not spheres
				_direction_point = _direction * _local_point.y + _collider.center;
			else if (_local_point.y > _line_length * 0.5f) // Controller is contacting with the upper sphere 
				_direction_point = _upper_sphere;
			else if (_local_point.y < -_line_length * 0.5f) // Controller is contacting with lower sphere
				_direction_point = _lower_sphere;

			//Calculate contact point in local coordinates and return it in world coordinates
			_contact_point = _local_point - _direction_point;
			_contact_point.Normalize();
			_contact_point = _contact_point * _collider.radius + _direction_point;
			return _transform.TransformPoint( _contact_point );

		}

		public static Vector3 NearestVertexTo( GameObject _object, Vector3 _point )
		{
			// convert point to local space
			_point = _object.transform.InverseTransformPoint( _point );

			Mesh _mesh = _object.GetComponent<MeshFilter>().mesh;
			float _min_distance_sqr = Mathf.Infinity;
			Vector3 _nearest_vertex = Vector3.zero;

			// scan all vertices to find nearest
			foreach( Vector3 vertex in _mesh.vertices )
			{
				Vector3 _diff = _point-vertex;
				float _dist_sqr = _diff.sqrMagnitude;
				if( _dist_sqr < _min_distance_sqr )
				{
					_min_distance_sqr = _dist_sqr;
					_nearest_vertex = vertex;
				}
			}
			// convert nearest vertex back to world space
			return _object.transform.TransformPoint( _nearest_vertex );


		}

		public static Color ColorA( Color _color, float _alpha )
		{
			_color.a = _alpha;
			return _color;
		}

		/// <summary>
		/// Determines if is in layer mask the specified _object _layerMask.
		/// </summary>
		/// <returns><c>true</c> if is in layer mask the specified _object _layerMask; otherwise, <c>false</c>.</returns>
		/// <param name="_object">Object.</param>
		/// <param name="_layerMask">Layer mask.</param>
		public static bool IsInLayerMask( GameObject _object, LayerMask _mask ){
			return (( _mask.value & ( 1 << _object.layer )) > 0 );
		}

		/// <summary>
		/// Determines if is in layer mask the specified layer layerMask.
		/// </summary>
		/// <returns><c>true</c> if is in layer mask the specified layer layerMask; otherwise, <c>false</c>.</returns>
		/// <param name="layer">Layer.</param>
		/// <param name="layerMask">Layer mask.</param>
		public static bool IsInLayerMask(int _layer, LayerMask _mask ){
			return (( _mask.value & ( 1 << _layer )) > 0 );
		}

		/// <summary>
		/// Determines if is in layer mask the specified _layer layerMask.
		/// </summary>
		/// <returns><c>true</c> if is in layer mask the specified _layer layerMask; otherwise, <c>false</c>.</returns>
		/// <param name="_layer">Layer.</param>
		/// <param name="layerMask">Layer mask.</param>
		public static bool IsInLayerMask( string _name, LayerMask _mask ){
			return (( _mask.value & ( 1 << LayerMask.NameToLayer( _name ))) > 0 );
		}



		/// <summary>
		/// Layer names to mask.
		/// </summary>
		/// <returns>The to mask.</returns>
		/// <param name="layerNames">Layer names.</param>
		public static LayerMask NamesToMask( params string[] _names )
		{
			int _mask = 0;
			foreach( string _name in _names )
				_mask |= (1 << LayerMask.NameToLayer( _name ) );
	
			return _mask;
		}

		/// <summary>
		/// Gets the layer mask or the default mask.
		/// </summary>
		/// <returns>The layer mask.</returns>
		/// <param name="_layers">Layers.</param>
		/// <param name="_mask">Mask.</param>
		/// <param name="_default">Default.</param>
		public static LayerMask GetLayerMask( List<string> _layers, int _mask, int _default )
		{
			_mask = GetLayerMask( _layers, _mask );
			_mask = ( _mask == 0 ? _default : _mask );
			return _mask;
		}

		public static LayerMask GetLayerMask( List<string> _layers, int _mask )
		{
			if( _layers == null || _layers.Count == 0 )
				_mask = 0;			
			else if( _mask == 0 || ! Application.isPlaying )
			{
				if( _layers.Count > 0 )
					_mask = LayerMask.GetMask( _layers.ToArray() );
				else
					_mask = Physics.DefaultRaycastLayers;
			}

			return _mask;
		}

		/// <summary>
		/// Gets the layer mask.
		/// </summary>
		/// <returns>The layer mask.</returns>
		/// <param name="_layers">Layers.</param>
		/// <param name="_mask">Mask.</param>
		/// <param name="_water">If set to <c>true</c> water.</param>
		public static LayerMask GetLayerMask2( List<string> _layers, int _mask )
		{
			if( _layers == null || _layers.Count == 0 )
				_mask = 0;			
			else if( _mask == 0 || ! Application.isPlaying )
			{
				if( _layers.Count > 0 )
				{
					_mask = 0;
					foreach( string _layer in _layers )
					{
						int _index = LayerMask.NameToLayer( _layer );
						if( _index != -1 )
							_mask |= (1 << _index );
					}
				}
				else
					_mask = Physics.DefaultRaycastLayers;
			}

			return _mask;
		}

		/// <summary>
		/// Gets the size of the object.
		/// </summary>
		/// <returns>The object size.</returns>
		/// <param name="_object">Object.</param>
		public static Vector3 GetObjectSize( GameObject _object )
		{
			Vector3 _total_size = Vector3.zero;

			Transform[] _transforms = _object.GetComponentsInChildren<Transform>();

			foreach( Transform _transform in _transforms )
			{
				Renderer _renderer = _transform.GetComponent<Renderer>();
				if( _renderer != null )
				{
					Vector3 _size = _renderer.bounds.size;

					if( _size.x > _total_size.x )
						_total_size.x = _size.x;
					if( _size.y > _total_size.y )
						_total_size.y = _size.y;
					if( _size.z > _total_size.z )
						_total_size.z = _size.z;
				}
			}

			//_total_size = Vector3.zero;

			if( _total_size == Vector3.zero )
				_total_size = GetObjectSizeByTransforms( _object );

			return _total_size;
		}

		/// <summary>
		/// Gets the object size by transform.
		/// </summary>
		/// <returns>The object size by transform.</returns>
		/// <param name="_object">Object.</param>
		public static Vector3 GetObjectSizeByTransform( GameObject _object )
		{
			Vector3 _size = Vector3.zero;

			Transform[] _transforms = _object.GetComponentsInChildren<Transform>();

			Vector3 _min = Vector3.zero;
			Vector3 _max = Vector3.zero;

			foreach( Transform _transform in _transforms )
			{
				if( _transform == _object.transform )
					continue;

				Vector3 _position = _object.transform.TransformPoint( _transform.localPosition );

				_position.x = _position.x/_object.transform.lossyScale.x;
				_position.y = _position.y/_object.transform.lossyScale.y;
				_position.z = _position.z/_object.transform.lossyScale.z;

				_position = _object.transform.InverseTransformPoint( _position );

				if( _position.x <= _min.x && _position.y <= _min.y )
				{
					_min.x = _position.x;
					_min.y = _position.y;
				}

				if( _position.x >= _max.x && _position.y <= _max.y )
				{
					_max.x = _position.x;
					_max.y = _position.y;
				}
				if( _position.z <= _min.z && _position.y <= _min.y )
				{
					_min.z = _position.z;
					_min.y = _position.y;
				}

				if( _position.z >= _max.z && _position.y <= _max.y )
				{
					_max.z = _position.z;
					_max.y = _position.y;
				}
			}

			_size.x = (_min.x * -1 ) + _max.x;
			_size.z = (_min.z * -1 ) + _max.z;
			_size.y = (_min.y * -1 ) + _max.y;

			return _size;
		}

		/// <summary>
		/// Gets the object size by transforms.
		/// </summary>
		/// <returns>The object size by transforms.</returns>
		/// <param name="_object">Object.</param>
		public static Vector3 GetObjectSizeByTransforms( GameObject _object )
		{
			Vector3 _size = Vector3.zero;

			Transform[] _transforms = _object.GetComponentsInChildren<Transform>();

			Vector3 _width_l = Vector3.zero;
			Vector3 _width_r = Vector3.zero;
			Vector3 _depth_f = Vector3.zero;
			Vector3 _depth_b = Vector3.zero;
			Vector3 _height_t = Vector3.zero;
			Vector3 _height_b = Vector3.zero;

			_width_l.y = Mathf.Infinity;
			_width_r.y = Mathf.Infinity;
			_depth_f.y = Mathf.Infinity;
			_depth_b.y = Mathf.Infinity;
			_height_t.y = Mathf.Infinity;
			_height_b.y = Mathf.Infinity;

			foreach( Transform _transform in _transforms )
			{
				if( _transform == _object.transform )
					continue;

				Vector3 _position = _object.transform.TransformPoint( _transform.localPosition );

				_position.x = _position.x/_object.transform.lossyScale.x;
				_position.y = _position.y/_object.transform.lossyScale.y;
				_position.z = _position.z/_object.transform.lossyScale.z;

				_position = _object.transform.InverseTransformPoint( _position );

				if( _position == Vector3.zero )
					continue;

				if( _position.x <= _width_l.x && _position.y <= _width_l.y )
				{
					_width_l.x = _position.x;
					_width_l.y = _position.y;
				}

				if( _position.x >= _width_r.x && _position.y <= _width_r.y )
				{
					_width_r.x = _position.x;
					_width_r.y = _position.y;
				}
				if( _position.z <= _depth_b.z && _position.y <= _depth_b.y )
				{
					_depth_b.z = _position.z;
					_depth_b.y = _position.y;
				}

				if( _position.z >= _depth_f.z && _position.y <= _depth_f.y )
				{
					_depth_f.z = _position.z;
					_depth_f.y = _position.y;
				}

				if( _position.y >= _height_t.y )
				{
					_height_t.z = _position.z;
					_height_t.y = _position.y;
				}

				if( _position.y <= _height_b.y )
				{
					_height_b.z = _position.z;
					_height_b.y = _position.y;
				}
			}

			_size.x = (_width_l.x * -1 ) + _width_r.x;
			_size.z = (_depth_b.z * -1 ) + _depth_f.z;
			_size.y = (_height_b.y * -1 ) + _height_t.y;

			return _size;
		}

		/// <summary>
		/// Enables the colliders.
		/// </summary>
		/// <param name="_transform">Transform.</param>
		/// <param name="_enabled">If set to <c>true</c> enabled.</param>
		public static void EnableColliders( Transform _transform, bool _enabled )
		{
			if( _transform == null )
				return;
			
			Collider[] _colliders = _transform.GetComponentsInChildren<Collider>();

			foreach( Collider _collider in _colliders )
				_collider.enabled = _enabled;
		}

		/// <summary>
		/// Enables the renderer.
		/// </summary>
		/// <param name="_transform">Transform.</param>
		/// <param name="_enabled">If set to <c>true</c> enabled.</param>
		public static void EnableRenderer( Transform _transform, bool _enabled )
		{
			if( _transform == null )
				return;

			Renderer[] _renderers = _transform.GetComponentsInChildren<Renderer>();

			foreach( Renderer _renderer in _renderers )
				_renderer.enabled = _enabled;
		}

		/// <summary>
		/// Enables the colliders.
		/// </summary>
		/// <param name="_transform">Transform.</param>
		/// <param name="_enabled">If set to <c>true</c> enabled.</param>
		public static void EnableChildren( GameObject _object, bool _enabled )
		{
			if( _object == null )
				return;

			Transform[] _transforms = _object.GetComponentsInChildren<Transform>();
			foreach( Transform _transform in _transforms )
			{
				if( _transform != _object.transform )			
					_transform.gameObject.SetActive( _enabled );
			}
		}

		/// <summary>
		/// Destroy the specified _object.
		/// </summary>
		/// <param name="_object">Object.</param>
		public static bool Destroy( GameObject _object )
		{
			if( _object == null )
				return false;

			if( Application.isEditor )
				GameObject.DestroyImmediate( _object );
			else
				GameObject.Destroy( _object );

			return true;
		}

		/// <summary>
		/// Attachs to transform.
		/// </summary>
		/// <returns><c>true</c>, if to transform was attached, <c>false</c> otherwise.</returns>
		/// <param name="_object">Object.</param>
		/// <param name="_parent">Parent.</param>
		public static bool AttachToTransform( GameObject _object, Transform _parent )
		{
			if( _object == null ||  _parent == null )
				return false;

			_object.transform.parent = _parent;
			_object.transform.position = _parent.position;
			_object.transform.rotation = _parent.rotation;
			_object.SetActive( true );

			Rigidbody _rigidbody = _object.GetComponent<Rigidbody>();
			if( _rigidbody != null )
			{
				_rigidbody.useGravity = false;
				_rigidbody.isKinematic = true;
				_rigidbody.constraints = RigidbodyConstraints.FreezeAll;
			}

			return true;
		}

		/// <summary>
		/// Detachs from transform.
		/// </summary>
		/// <returns><c>true</c>, if from transform was detached, <c>false</c> otherwise.</returns>
		/// <param name="_object">Object.</param>
		public static bool DetachFromTransform( GameObject _object )
		{
			if( _object == null )
				return false;

			_object.transform.SetParent( null, true );
			_object.SetActive( true );

			Rigidbody _rigidbody = _object.GetComponent<Rigidbody>();
			if( _rigidbody != null )
			{
				_rigidbody.useGravity = true;
				_rigidbody.isKinematic = false;
				_rigidbody.constraints = RigidbodyConstraints.None;
			}

			return true;
		}

		/// <summary>
		/// Processes the child.
		/// </summary>
		/// <param name="aObj">A object.</param>
		/// <param name="aList">A list.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		private static void ProcessChild<T>(Transform aObj, ref List<T> aList) where T : Component
		{
			T c = aObj.GetComponent<T>();
			if (c != null)
				aList.Add(c);
			foreach(Transform child in aObj)
				ProcessChild<T>(child,ref aList);
		}

		/// <summary>
		/// Gets the specified root component.
		/// </summary>
		/// <returns>The root.</returns>
		/// <param name="_transform">Transform.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T GetRoot<T>( Transform _transform ) where T : Component
		{
			if( _transform == null || _transform.root == _transform )
				return null;
			
			T[] _components = _transform.GetComponentsInParent<T>();
			foreach(  T _component in _components )
				if( _component.transform.root == _component.transform )
					return _component;

			return null;
		}

		/// <summary>
		/// Gets all children.
		/// </summary>
		/// <returns>The all children.</returns>
		/// <param name="aObj">A object.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T[] GetAllChildren<T>(this Transform aObj) where T : Component
		{
			List<T> result = new List<T>();
			ProcessChild<T>(aObj, ref result);
			return result.ToArray();
		}

		/// <summary>
		/// Gets all children.
		/// </summary>
		/// <returns>The all children.</returns>
		/// <param name="aObj">A object.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T[] GetAllChildren<T>(this GameObject aObj) where T : Component
		{
			List<T> result = new List<T>();
			ProcessChild<T>(aObj.transform, ref result);
			return result.ToArray();
		}

		/// <summary>
		/// Gets the children.
		/// </summary>
		/// <param name="_list">List.</param>
		/// <param name="_transform">Transform.</param>
		public static void GetChildren( List<Transform> _list, Transform _transform )
		{
			if( _list == null || _transform == null )
				return;

			_list.Add( _transform );
			
			foreach (Transform _child in _transform )
				GetChildren( _list, _child );
		}

		public static T FindChildOfTypeByName<T>( string _name, Transform _parent )
		{
			Transform _transform = FindChildByName( _name, _parent );

			if( _transform == null )
				return default(T);
			
			return _transform.GetComponent<T>();
		}

		/// <summary>
		/// Finds the name of the child by.
		/// </summary>
		/// <returns>The child by name.</returns>
		/// <param name="_name">Name.</param>
		/// <param name="_parent">Parent.</param>
		public static Transform FindChildByName( string _name, Transform _parent )
		{
			if( string.IsNullOrEmpty( _name ) || _parent == null )
				return null;

			if( _parent.name == _name )
				return _parent;

			foreach( Transform _child in _parent )
			{
				Transform _result = FindChildByName( _name, _child );

				if( _result != null && _result.name == _name )
					return _result;
			}

			return null;
		}

		/// <summary>
		/// Finds the game objects by layer.
		/// </summary>
		/// <returns>The game objects by layer.</returns>
		/// <param name="_layer">Layer.</param>
		public static GameObject[] FindGameObjectsByLayer( int _layer ) 
		{
			GameObject[] _objects = GameObject.FindObjectsOfType<GameObject>();

			List<GameObject> _result_objects = new List<GameObject>();

			for( int i = 0; i < _objects.Length; i++) 
			{
			 	if( _objects[i].layer == _layer )
			    	_result_objects.Add(_objects[i]);
		    }
		     
			if( _result_objects.Count == 0 )
				return null;
		     
		     return _result_objects.ToArray();
		 }

		/// <summary>
		/// Copies the transforms.
		/// </summary>
		/// <param name="_source_transform">Source transform.</param>
		/// <param name="_target_transform">Target transform.</param>
		public static void CopyTransforms( Transform _source_transform , Transform _target_transform )
		{
			if( _source_transform == null || _target_transform == null )
				return;

			_target_transform.position = _source_transform.position;
			_target_transform.rotation = _source_transform.rotation;
			//_target_transform.localScale = _source_transform.localScale; // nice locking effect but not the right way :)
			
			foreach( Transform _child in _target_transform) 
			{
				Transform _source = _source_transform.Find( _child.name );
				if( _source )
					CopyTransforms( _source, _child );
			}
		}
			
		/// <summary>
		/// Gets the name of the ground texture.
		/// </summary>
		/// <returns>The ground texture name.</returns>
		/// <param name="_position">Position.</param>
		/// <param name="_mask">Mask.</param>
		/// <param name="_terrain">Terrain.</param>
		/// <param name="_terrain_data">Terrain data.</param>
		/// <param name="_offset">Offset.</param>
		public static string GetGroundTextureName( Vector3 _position, LayerMask _mask, ref Terrain _terrain, ref TerrainData _terrain_data, float _offset )
		{
			if( _terrain == null )
			{
				_terrain = Terrain.activeTerrain;

				if( _terrain != null )
					_terrain_data = _terrain.terrainData;
			}
				
			string _name = "";
			RaycastHit _hit;

			_position.y += _offset;	
			if( Physics.Raycast( _position, Vector3.down, out _hit, Mathf.Infinity, _mask, WorldManager.TriggerInteraction ) )
			{
				Terrain _hit_terrain = _hit.transform.GetComponent<Terrain>();
				if( _hit_terrain != null  )
				{
					int _texture_id = SystemTools.GetMainTerrainTexture( _position, _hit_terrain );
					if( _terrain_data.splatPrototypes.Length > 0 )
						_name = _terrain_data.splatPrototypes[_texture_id].texture.name;
				}
				else 
				{
					Renderer _hit_renderer = _hit.collider.gameObject.GetComponent<Renderer>();
					if( _hit_renderer != null && _hit_renderer.material != null && _hit_renderer.material.mainTexture != null  )
						_name = _hit_renderer.material.mainTexture.name;
				}

			}

			return _name;
		}

		/// <summary>
		/// Gets the main texture of the terrain.
		/// </summary>
		/// <returns>The main terrain texture index</returns>
		/// <param name="_world_pos">_world_pos.</param>
		/// <param name="_terrain">_terrain.</param>
		/// <description>
		/// http://answers.unity3d.com/questions/34328/terrain-with-multiple-splat-textures-how-can-i-det.html
		/// </description>
		public static int GetMainTerrainTexture( Vector3 _world_pos, Terrain _terrain )
		{
			TerrainData _terrain_data = _terrain.terrainData;
			Vector3 _terrain_position = _terrain.transform.position;

			// evaluate the splat map cell 
			int _map_x = (int)((( _world_pos.x - _terrain_position.x ) / _terrain_data.size.x ) * _terrain_data.alphamapWidth );
			int _map_z = (int)((( _world_pos.z - _terrain_position.z ) / _terrain_data.size.z ) * _terrain_data.alphamapHeight );

			// get the splat map data 
			float[,,] _map_data = _terrain_data.GetAlphamaps(_map_x,_map_z,1,1);			

			// extracting the _map_data array data to the 1d mix array:
			float[] _mix = new float[_map_data.GetUpperBound(2)+1];
			for( int i=0; i<_mix.Length; ++i )
				_mix[i] = _map_data[0,0,i];    

			float _max_mix = 0;
			int _max_index = 0;

			// find the maximum by looping through the mix values 
			for( int _i = 0; _i < _mix.Length; ++_i)
			{
				if( _mix[_i] > _max_mix )
				{
					_max_index = _i;
					_max_mix = _mix[_i];
				}
			}

			return _max_index;

		}
	}

	#if ICE_EXPERIMENTAL

	public static BehaviourEventParameterType GetMethodParameterType( ICEWorldBehaviour _control, string _method )
	{
		System.Type _type = _control.GetType();

		if( _type == null )
			return BehaviourEventParameterType.None;

		if( _type.GetMethod( _method, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance, null, new System.Type[] { typeof(bool) }, null) != null )
			return BehaviourEventParameterType.Boolean;
		else if( _type.GetMethod( _method, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance, null, new System.Type[] { typeof(string) }, null) != null )
			return BehaviourEventParameterType.String;
		else if( _type.GetMethod( _method, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance, null, new System.Type[] { typeof(float) }, null) != null )
			return BehaviourEventParameterType.Float;
		else if( _type.GetMethod( _method, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance, null, new System.Type[] { typeof(int) }, null) != null )
			return BehaviourEventParameterType.Integer;
		else
			return BehaviourEventParameterType.None;
	}

	/// <summary>
	/// Copies the component.
	/// </summary>
	/// <returns>The component.</returns>
	/// <param name="_original">_original.</param>
	/// <param name="_destination">_destination.</param>
	public static Component CopyComponent( Component _original, GameObject _destination )
	{
		System.Type _type = _original.GetType();
		Component _copy = _destination.AddComponent(_type);
		// Copied fields can be restricted with BindingFlags
		System.Reflection.FieldInfo[] _fields = _type.GetFields(); 
		foreach (System.Reflection.FieldInfo _field in _fields)
			_field.SetValue( _copy, _field.GetValue(_original));
		return _copy;
	}

	/// <summary>
	/// Copies the component (generic).
	/// </summary>
	/// <returns>The component.</returns>
	/// <param name="_original">_original.</param>
	/// <param name="_destination">_destination.</param>
	public static T CopyComponent<T>(T _original, GameObject _destination) where T : Component
	{
		System.Type _type = _original.GetType();
		Component _copy = _destination.AddComponent(_type);
		System.Reflection.FieldInfo[] _fields = _type.GetFields();
		foreach( System.Reflection.FieldInfo _field in _fields )
			_field.SetValue( _copy, _field.GetValue(_original) );
		return _copy as T;
	}


	/// <summary>
	/// Lists the assemblies.
	/// </summary>
	/// <param name="_types">If set to <c>true</c> types.</param>
	public static void ListAssemblies( bool _types = false )
	{
		System.Reflection.Assembly[] _assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
		foreach( System.Reflection.Assembly _assembly in _assemblies )
		{
			Debug.Log( "Name : " + _assembly.FullName );

			if( _types == true )
				ListAssemblyTypes( _assembly );
		}
	}

	/// <summary>
	/// Lists the assembly types.
	/// </summary>
	/// <param name="_assembly">Assembly.</param>
	public static void ListAssemblyTypes( System.Reflection.Assembly _assembly )
	{
		if( _assembly == null )
		return;

		foreach( System.Type _type in _assembly.GetTypes() )
		{
			Debug.Log( "    Type : " + _type.Name );
		}
	}

	/// <summary>
	/// Gets the name of the assembly by type.
	/// </summary>
	/// <returns>The assembly by type name.</returns>
	/// <param name="_name">Name.</param>
	public static System.Reflection.Assembly GetAssemblyByTypeName( string _name )
	{
		System.Reflection.Assembly[] _assemblies = GetAssemblies();

		foreach( System.Reflection.Assembly _assembly in _assemblies )
		{
			foreach( System.Type _type in _assembly.GetTypes() )
			{
				if ( _type.Name == _name )
					return _assembly;
			}
		}

		return null;
	}

	/// <summary>
	/// Gets the assemblies.
	/// </summary>
	/// <returns>The assemblies.</returns>
	public static System.Reflection.Assembly[] GetAssemblies()
	{
		return System.AppDomain.CurrentDomain.GetAssemblies();
	}

	/// <summary>
	/// Doeses the type exist.
	/// </summary>
	/// <returns><c>true</c>, if type exist was doesed, <c>false</c> otherwise.</returns>
	/// <param name="_name">Name.</param>
	public static bool DoesTypeExist( string _name )
	{
		/*
		SystemTools.ListAssemblies();

		Debug.Log( "test 1 " + SystemTools.DoesTypeExist("ICECreatureControl").ToString() );
		Debug.Log( "test 2 " + SystemTools.DoesTypeExist("PHOTON").ToString() );

		System.Reflection.Assembly _asm = SystemTools.GetAssemblyByTypeName( "ICECreatureControl" );

		if( _asm != null )
		Debug.Log( "test 3 " + _asm.FullName );
		*/
		System.Reflection.Assembly[] _assemblies = GetAssemblies();

		foreach( System.Reflection.Assembly _assembly in _assemblies )
		{
			foreach( System.Type _type in _assembly.GetTypes() )
			{
				if ( _type.Name == _name )
					return true;
			}
		}

		return false;
	}

	#endif
}
