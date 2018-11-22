// ##############################################################################
//
// ice_utilities_object.cs | ICE.World.Utilities.ObjectTools
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

namespace ICE.World.Utilities
{
	[System.Serializable]
	public struct ObjectRelations
	{
		public GameObject Origin;
		public GameObject Target;

		private Vector3 m_OriginPosition;
		private Vector3 m_TargetPosition;

		private Vector3 m_Heading;
		private float m_Distance;
		private Vector3 m_Direction;

		public ObjectRelations( GameObject _origin, GameObject _target )
		{
			this.Origin = _origin;
			this.Target = _target;

			this.m_OriginPosition = Vector3.zero;
			this.m_TargetPosition = Vector3.zero;

			this.m_Heading = Vector3.zero;
			this.m_Distance = 0;
			this.m_Direction = Vector3.zero;

			GetHeading();
			GetDistance();
			GetDirection();
		}

		public bool IsValid{
			get{ return ( GetOriginPosition() != Vector3.zero && GetTargetPosition() != Vector3.zero && OriginPosition != TargetPosition ? true:false ); }
		}
			
		public Vector3 GetOriginPosition(){
			return m_OriginPosition = ( Origin != null ? Origin.transform.position:Vector3.zero ); 
		}

		public Vector3 GetTargetPosition(){
			return m_TargetPosition = ( Target != null ? Target.transform.position:Vector3.zero );
		}

		public Vector3 OriginPosition{
			get{ return m_OriginPosition = ( m_OriginPosition == Vector3.zero ? GetOriginPosition():m_OriginPosition ); }
		}

		public Vector3 TargetPosition{
			get{ return m_TargetPosition = ( m_TargetPosition == Vector3.zero ? GetTargetPosition():m_TargetPosition ); }
		}

		public Vector3 Heading{
			get{ return m_Heading = ( m_Heading == Vector3.zero ? GetHeading():m_Heading ); }
		}

		public float Distance{
			get{ return m_Distance = ( m_Distance == 0 ? GetDistance():m_Distance ); }
		}

		public Vector3 Direction{
			get{ return m_Direction = ( m_Direction == Vector3.zero ? GetDirection():m_Direction ); }
		}

		public Vector3 GetHeading(){
			return m_Heading = ( IsValid ? TargetPosition - OriginPosition:Vector3.zero );
		}

		public float GetDistance(){
			return this.m_Distance = ( IsValid ? GetHeading().magnitude:0 );
		}

		public Vector3 GetDirection(){
			return this.m_Direction = ( IsValid ? GetHeading() / GetDistance():Vector3.zero );
		}

		public bool InRange( float _distance ){
			return ( IsValid && GetHeading().sqrMagnitude < _distance * _distance ? true:false );
		}
	}

	public static class ObjectTools 
	{
		public static bool CompareGameObject( GameObject _object_1, GameObject _object_2 ){

			//Debug.Log( System.Object.ReferenceEquals( _object_1, _object_2 ).ToString() + " - " + _object_1.GetInstanceID() + " == " + _object_2.GetInstanceID() );
			return ( ( _object_1 == null && _object_2 == null ) || ( _object_1 != null && _object_2 != null && _object_1.GetInstanceID() == _object_2.GetInstanceID() ) ? true : false );

			//return System.Object.ReferenceEquals( _object_1, _object_2 );
		} 

		/// <summary>
		/// Gets the random object by tag.
		/// </summary>
		/// <returns>The random object by tag.</returns>
		/// <param name="_tag">Tag.</param>
		public static GameObject GetRandomObjectByTag( string _tag )
		{
			if( _tag.Trim() == "" )
				return null;

			GameObject[] _objects = GameObject.FindGameObjectsWithTag( _tag );

			if( _objects != null && _objects.Length > 0 )
				return _objects[ (int)Random.Range( 0, _objects.Length ) ];
			else
				return null;
		}

		public static GameObject[] GetObjectsByName( string _name )
		{
			if( _name.Trim() == "" )
				return null;

			List<GameObject> _results = new List<GameObject>();
			GameObject[] _objects = GameObject.FindObjectsOfType<GameObject>();

			foreach( GameObject _object in _objects ){
				if( _object != null && _object.name == _name )
					_results.Add( _object );
			}

			return _results.ToArray();
		}

		/// <summary>
		/// Gets the random name of the object by.
		/// </summary>
		/// <returns>The random object by name.</returns>
		/// <param name="_name">Name.</param>
		public static GameObject GetRandomObjectByName( string _name )
		{
			if( _name.Trim() == "" )
				return null;

			List<GameObject> _results = new List<GameObject>();
			GameObject[] _objects = GameObject.FindObjectsOfType<GameObject>();

			foreach( GameObject _object in _objects ){
				if( _object != null && _object.name == _name )
					_results.Add( _object );
			}
				
			if( _results != null && _results.Count > 0 ) 
				return _results[ (int)Random.Range( 0, _results.Count ) ];
			else
				return GameObject.Find( _name );
		}

		/// <summary>
		/// Gets the nearest object by name, position and distance.
		/// </summary>
		/// <returns>The nearest object by name.</returns>
		/// <param name="_name">Name.</param>
		/// <param name="_position">Position.</param>
		/// <param name="_distance">Distance.</param>
		public static GameObject GetNearestObjectByName( string _name, Vector3 _position, float _distance )
		{
			if( _name.Trim() == "" )
				return null;

			if( _distance == 0 )
				_distance = Mathf.Infinity;

			GameObject _best_object = null;
			float _best_distance = _distance;

			GameObject[] _objects = GameObject.FindObjectsOfType<GameObject>();

			foreach( GameObject _tmp_object in _objects )
			{
				if( _tmp_object != null && _tmp_object.name == _name && _tmp_object.transform.position != _position && _tmp_object.activeInHierarchy  )
				{
					float _tmp_distance = PositionTools.Distance( _position, _tmp_object.transform.position );

					if( _tmp_distance < _best_distance )
					{
						_best_distance = _tmp_distance;					
						_best_object = _tmp_object;
					}
				}
			}

			return _best_object;
		}

		/// <summary>
		/// Gets the nearest object.
		/// </summary>
		/// <returns>The nearest object.</returns>
		/// <param name="_objects">Objects.</param>
		/// <param name="_position">Position.</param>
		/// <param name="_distance">Distance.</param>
		public static GameObject GetNearestObject( GameObject[] _objects, Vector3 _position, float _distance )
		{
			if( _objects == null || _objects.Length == 0 )
				return null;

			if( _distance == 0 )
				_distance = Mathf.Infinity;

			GameObject _best_object = null;
			float _best_distance = _distance;

			for( int i = 0; i < _objects.Length; i++ )
			{
				GameObject _object = _objects[i];

				if( _object != null && _object.activeInHierarchy && _object.transform.position != _position )
				{
					float _tmp_distance = PositionTools.Distance( _position, _object.transform.position );

					if( _tmp_distance < _best_distance )
					{
						_best_distance = _tmp_distance;					
						_best_object = _object;
					}
				}
			}

			return _best_object;
		}
	}
}
