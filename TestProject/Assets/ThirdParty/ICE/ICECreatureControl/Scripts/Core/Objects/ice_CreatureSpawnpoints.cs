// ##############################################################################
//
// ice_CreatureSpawnpoints.cs
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
using System.Text.RegularExpressions;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;
using ICE.Creatures.Attributes;

namespace ICE.Creatures.Objects
{
	[System.Serializable]
	public class SpawnPointDataObject : ICEDataObject
	{
		public SpawnPointDataObject(){}
		public SpawnPointDataObject( SpawnPointDataObject _point ){ Copy( _point ); }

		public void Copy( SpawnPointObject _point )
		{
			if( _point == null )
				return;

			base.Copy( _point );

			AccessType = _point.AccessType;
			IsPrefab = _point.IsPrefab;
			LevelDifference = _point.LevelDifference;
			LevelDifferenceMaximum = _point.LevelDifferenceMaximum;
			SpawningRangeMax = _point.SpawningRangeMax;
			SpawningRangeMin = _point.SpawningRangeMin;
			SpawningRangeMaximum = _point.SpawningRangeMaximum;
			SpawnPointGameObject = _point.SpawnPointGameObject;

			UseRandomRect = _point.UseRandomRect;
		}

		public TargetAccessType AccessType = TargetAccessType.OBJECT;
		public bool UseRandomRect = false;
		public Vector3 RandomRect = Vector3.one;
		public float SpawningRangeMin = 0;
		public float SpawningRangeMax = 5;
		public float SpawningRangeMaximum = 100;
		public float LevelDifference = 250;
		public float LevelDifferenceMaximum = 1000;
		public float LevelOffset = 0;
		public float LevelOffsetMaximum = 10;
		public bool IsPrefab = false;
		public GameObject SpawnPointGameObject = null;
	}

	[System.Serializable]
	public class SpawnPointObject : SpawnPointDataObject
	{
		public SpawnPointObject(){}
		public SpawnPointObject( SpawnPointObject _point ) : base( _point ) {}
		public SpawnPointObject( TargetObject _target )
		{
			Enabled = true;
			Foldout = true;

			AccessType = _target.AccessType;
			SpawnPointGameObject = _target.TargetGameObject;

			if( _target.Move.HasRandomRange )
			{
				SpawningRangeMin = _target.Move.StoppingDistance;
				SpawningRangeMax = _target.Move.RandomRange;
				SpawningRangeMaximum = _target.Move.RandomRangeMaximum;
			}
		}
		public SpawnPointObject( GameObject _object )
		{
			Enabled = true;
			Foldout = true;

			AccessType = TargetAccessType.OBJECT;
			SpawnPointGameObject = _object;

			SpawningRangeMin = 0;
			SpawningRangeMax = 5;
		}

		public SpawnPointObject( GameObject _object, float _min, float _max )
		{
			Enabled = true;
			Foldout = true;

			AccessType = TargetAccessType.OBJECT;
			SpawnPointGameObject = _object;

			SpawningRangeMin = Mathf.Min( _min, _max );
			SpawningRangeMax = Mathf.Max( _min, _max );
			SpawningRangeMaximum = Mathf.Max( SpawningRangeMax, SpawningRangeMaximum );
		}

		public SpawnPointObject( GameObject _object, Vector3 _size )
		{
			Enabled = true;
			Foldout = true;

			AccessType = TargetAccessType.OBJECT;
			SpawnPointGameObject = _object;

			UseRandomRect = true;
			RandomRect = _size;
			LevelDifference = _size.y;
		}

		public bool IsValid{
			get{
				if( SpawnPointGameObject != null && Enabled )
					return true;
				else
					return false;
			}
		}

		public string SpawnPointName{
			get{ return ( SpawnPointGameObject != null ? SpawnPointGameObject.name : "" ); }
		}

		public string SpawnPointTag{
			get{ return ( SpawnPointGameObject != null ? SpawnPointGameObject.tag : "Untagged" ); }
		}

		public void SetSpawnPointGameObjectByTag( string _tag )
		{
			if( ! string.IsNullOrEmpty( _tag ) )
				SpawnPointGameObject = CreatureRegister.GetReferenceGameObjectByTag( _tag );
			else
				SpawnPointGameObject = null;
		}

		public void SetSpawnPointGameObjectByName( string _name )
		{
			if( ! string.IsNullOrEmpty( _name ) )
				SpawnPointGameObject = CreatureRegister.GetReferenceGameObjectByName( _name );
			else
				SpawnPointGameObject = null;
		}


		public GameObject[] GetAllSpawnPointGameObjects()
		{
			if( AccessType == TargetAccessType.TAG )
				return GameObject.FindGameObjectsWithTag( SpawnPointTag );
			else
				return ObjectTools.GetObjectsByName( SpawnPointName );
		}

		/// <summary>
		/// Gets the best spawn point game object.
		/// </summary>
		/// <returns>The best spawn point game object.</returns>
		public GameObject GetBestSpawnPointGameObject()
		{
			GameObject _object = SpawnPointGameObject;

			if( AccessType == TargetAccessType.NAME )
				_object = CreatureRegister.GetRandomTargetByName( SpawnPointName );
			else if( AccessType == TargetAccessType.TAG )
				_object = CreatureRegister.GetRandomTargetByTag( SpawnPointTag );

			return _object;
		}

		/// <summary>
		/// Gets the spawn position.
		/// </summary>
		/// <returns>The spawn position.</returns>
		/// <param name="_base_offset">Base offset.</param>
		public Vector3 GetSpawnPosition( float _base_offset = 0 ) 
		{
			Vector3 _position = Vector3.zero;

			GameObject _object = GetBestSpawnPointGameObject();

			if( _object != null )
			{ 
				if( UseRandomRect )
					_position = PositionTools.GetRandomRectPosition( _object.transform.position, RandomRect, true );
				else if( SpawningRangeMax > 0 )
					_position = PositionTools.GetRandomCirclePosition( _object.transform.position, SpawningRangeMin, SpawningRangeMax );
				else
					_position = _object.transform.position;

				_position.y = PositionTools.GetGroundLevel( _position, CreatureRegister.GroundCheck , CreatureRegister.GroundLayerMask, 0.5f, LevelDifference, _base_offset );
				_position.y += _base_offset + LevelOffset;
			}

			return _position;
		}

	}

}
