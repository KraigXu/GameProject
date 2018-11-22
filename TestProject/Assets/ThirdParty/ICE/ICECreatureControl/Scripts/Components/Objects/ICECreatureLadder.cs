// ##############################################################################
//
// ICECreatureLadder.cs
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

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;

namespace ICE.Creatures
{
	public class ICECreatureLadder : ICECreatureObject, INavigationLink
	{
		/// <summary>
		/// Gets the entity classification type of the object
		/// </summary>
		/// <value>The classification type of the entity.</value>
		/// <description>The EntityClassType will be used to quickly 
		/// identify the correct class type of a derived entity object 
		/// without casts
		/// </description>
		public override EntityClassType EntityType{
			get{ return EntityClassType.Ladder; }
		}

		protected override void OnRegisterBehaviourEvents()
		{
			base.OnRegisterBehaviourEvents();
			//e.g. RegisterBehaviourEvent( "ApplyDamage", BehaviourEventParameterType.Float );
		}

		public Vector3 ClimbingOffset = Vector3.zero;


		public override void OnTriggerEnter ( Collider _collider ) {

			if( transform.IsChildOf( _collider.transform ) )
				return;

			HandleLadder( true, _collider.transform.gameObject );


		}

		public override void OnTriggerStay ( Collider _collider ) {

			if( transform.IsChildOf( _collider.transform ) )
				return;

			HandleLadder( true, _collider.transform.gameObject );
		}

		public override void OnTriggerExit ( Collider _collider ) {

			if( transform.IsChildOf( _collider.transform ) )
				return;

			HandleLadder( false, _collider.transform.gameObject );
		}

		private void HandleLadder( bool _open, GameObject _object )
		{
	
			ICEWorldEntity _entity = ( _object != null ? _object.GetComponent<ICEWorldEntity>() : null );
			if( _entity == null )
				return;

			if( _entity.EntityType == EntityClassType.Creature )
			{
				ICECreatureControl _creature = _entity as ICECreatureControl;
				if( _creature == null )
					return;
				/*
				if( _creature.Creature.ActiveTargetVerticalDistance > 0 )
					Debug.Log( "CLIMB UP" );
				else
					Debug.Log( "CLIMB DOWN" );*/
			}
			else if( _entity.EntityType == EntityClassType.Player )
			{
				ICECreaturePlayer _player = _entity as ICECreaturePlayer;
				if( _player == null )
					return;
			}
		}

		private Vector3 _start_point;

		public Vector3 GetStartPosition( GameObject _object, Vector3 _offset )
		{
			if( _object == null )
				return Vector3.zero;

			if( AttachedCollider == null )
				AttachedCollider = GetComponent<BoxCollider>();

			Vector3 _point = AttachedCollider.bounds.ClosestPoint( _object.transform.position );

			Vector3 _point_offset = PositionTools.FixInverseTransformPoint( AttachedCollider.transform, _point );

			Vector3 _size = AttachedCollider.size * 0.5f;	

			_point_offset.z = ( _point_offset.z < 0 ? _point_offset.z + _size.z: _point_offset.z - _size.z );
			_point_offset.x = ( _point_offset.x < 0 ? _point_offset.x + _size.x : _point_offset.x - _size.x );

			_point_offset += ClimbingOffset + _offset;

			_point = PositionTools.FixTransformPoint( AttachedCollider.transform, _point_offset );

			_start_point= _point;

			return _point;
		}

		public Vector3 GetEndPosition( GameObject _object )
		{
			return Vector3.zero;
		}

		public Vector3 GetTopPosition{
			get{
				if( AttachedCollider == null )
					AttachedCollider = GetComponent<BoxCollider>();

				Vector3 _point = AttachedCollider.bounds.max;

				Vector3 _size = AttachedCollider.size * 0.5f;					

				_point.z = ( transform.up.z < 0 ? AttachedCollider.bounds.min.z + _size.z: AttachedCollider.bounds.max.z - _size.z );
				_point.x = ( transform.up.x < 0 ? AttachedCollider.bounds.min.x + _size.x : AttachedCollider.bounds.max.x - _size.x );

				return _point;
			}
		}

		public Vector3 GetBottomPosition{
			get{
				if( AttachedCollider == null )
					AttachedCollider = GetComponent<BoxCollider>();

				Vector3 _point = AttachedCollider.bounds.min;

				Vector3 _size = AttachedCollider.size * 0.5f;					

				_point.z = ( transform.up.z >= 0 ? AttachedCollider.bounds.min.z + _size.z: AttachedCollider.bounds.max.z - _size.z );
				_point.x = ( transform.up.x >= 0 ? AttachedCollider.bounds.min.x + _size.x : AttachedCollider.bounds.max.x - _size.x );

				return _point;
			}
		}

		public BoxCollider AttachedCollider;


		void OnDrawGizmosSelected() 
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere( _start_point, 0.25f );
		

			//Gizmos.color = Color.green;
			//Gizmos.DrawWireSphere( GetTopPosition, 0.25f );

			//Gizmos.color = Color.red;
			//Gizmos.DrawWireSphere( GetBottomPosition, 0.25f );
		}
	}
}
