// ##############################################################################
//
// ICECreatureWaypoint.cs
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
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;

namespace ICE.Creatures{


	[System.Serializable]
	public class WaypointLinkObject : ICEOwnerObject
	{
		public WaypointLinkObject(){Enabled = true;}
		public WaypointLinkObject( WaypointLinkObject _object ) : base( _object ){ Copy( _object ); }
		public WaypointLinkObject( ICEWorldBehaviour _component ) : base( _component ){}
		public WaypointLinkObject( ICEWorldBehaviour _component, GameObject _object ) : base( _component )
		{ 
			if( _object == null )
				return;

			base.Init( _component );

			Enabled = true;

			ICECreatureWaypoint _waypoint = _object.GetComponent<ICECreatureWaypoint>();
			if( _waypoint == null )
				_waypoint = _object.AddComponent<ICECreatureWaypoint>();

			Waypoint = _waypoint;

		}

		public void Copy( WaypointLinkObject _link )
		{
			if( _link == null )
				return;

			base.Copy( _link );

			Waypoint = _link.Waypoint;
		}
			
		public ICECreatureWaypoint Waypoint = null;
		public float Weight = 1;

		public bool IsTwoWay{
			get{ return Waypoint.Links.WaypointExists( Owner ); }
		}
	}

	[System.Serializable]
	public class WaypointLinksObject : ICEOwnerObject
	{
		public WaypointLinksObject(){ Enabled = true; }
		public WaypointLinksObject( WaypointLinksObject _object ) : base( _object ){ Copy( _object ); }
		public WaypointLinksObject( ICEWorldBehaviour _component ) : base( _component ){}

		public override void Init( ICEWorldBehaviour _component )
		{
			base.Init( _component );

			foreach( WaypointLinkObject _link in Links )
				_link.Init( _component );
		}

		public void Copy( WaypointLinksObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			Links = _object.Links;

		}

		[SerializeField]
		private List<WaypointLinkObject> m_Links = null;
		public List<WaypointLinkObject> Links{
			get{ 
				m_Links = ( m_Links == null ? new List<WaypointLinkObject>() : m_Links ); 

				for( int i = 0 ; i < m_Links.Count ; i++ )
				{
					if( m_Links[i] != null &&  m_Links[i].Waypoint == null )
					{
						m_Links.RemoveAt( i );
						--i;
					}					
				}

				return m_Links;
			}
			set{ 
				Links.Clear();
				if( value == null ) return;	
				foreach( WaypointLinkObject _link in value )
					Links.Add( new WaypointLinkObject( _link ) );
			}
		}
						
		public ICECreatureWaypoint SubdivideLink( WaypointLinkObject _link )
		{
			if( _link == null )
				return null;

			ICECreatureWaypoint _sub_waypoint = CreateWaypointByPosition( Vector3.Lerp( Owner.transform.position, _link.Waypoint.ObjectTransform.position,  0.5f ) );
			_sub_waypoint.Links.AddWaypointByObject( Owner );

			ICECreatureWaypoint _link_waypoint = _link.OwnerComponent as ICECreatureWaypoint;
			_sub_waypoint.Links.AddWaypointByObject( _link.Waypoint.gameObject );
			_link_waypoint.Links.AddWaypointByObject( _sub_waypoint.gameObject );

			_sub_waypoint.UseDebug = true;

			_link.Waypoint.Links.RemoveWaypointByObject( Owner );
			RemoveWaypointByObject( _link.Waypoint.gameObject );

			return _sub_waypoint;
		}

		public ICECreatureWaypoint CreateWaypointByPosition( Vector3 _posistion )
		{
			GameObject _object = new GameObject( "Waypoint (" + WaypointsCount() + ")" );
			_object.transform.position = _posistion;

			return AddWaypointByObject( _object );
		}

		public ICECreatureWaypoint AddWaypointByObject( GameObject _object )
		{
			if( _object == null || _object == Owner || WaypointExists( _object ) )
				return null;

			ICECreatureWaypoint _waypoint = _object.GetComponent<ICECreatureWaypoint>();

			if( _waypoint == null )
				_waypoint = _object.AddComponent<ICECreatureWaypoint>();

			if( _waypoint != null )
			{
				Links.Add( new WaypointLinkObject( OwnerComponent, _object ) );
			}

			return _waypoint;
		}

		public bool RemoveWaypointByObject( GameObject _object )
		{
			if( _object == null || _object == Owner )
				return false;

			WaypointLinkObject _waypoint = GetWaypoint( _object );

			if( _waypoint != null )
				return Links.Remove( _waypoint );

			return false;
		}

		public WaypointLinkObject GetWaypoint( GameObject _object )
		{
			if( _object == null )
				return null;
			
			foreach( WaypointLinkObject _link in Links )
			{
				if( _link.Waypoint != null && _link.Waypoint.gameObject == _object )
					return _link;
			}

			return null;
		}

		public bool WaypointExists( GameObject _object ){
			return ( GetWaypoint( _object ) != null ? true : false );
		}

		public static ICECreatureWaypoint[] Waypoints(){
			 return GameObject.FindObjectsOfType<ICECreatureWaypoint>();
		}

		public static int WaypointsCount(){
			return Waypoints().Length;
		}
	}

	public class ICECreatureWaypoint : ICECreatureMarker {

		/// <summary>
		/// Gets the entity classification type of the object
		/// </summary>
		/// <value>The classification type of the entity.</value>
		/// <description>The EntityClassType will be used to quickly 
		/// identify the correct class type of a derived entity object 
		/// without casts
		/// </description>
		public override EntityClassType EntityType{
			get{ return EntityClassType.Waypoint; }
		}

		[SerializeField]
		private WaypointLinksObject m_Links = null;
		public WaypointLinksObject Links{
			get{ return m_Links = ( m_Links == null ? new WaypointLinksObject() : m_Links ); }
			set{ Links = new WaypointLinksObject( value ); }
		}

		protected override void OnRegisterBehaviourEvents()
		{
			//e.g. RegisterBehaviourEvent( "ApplyDamage", BehaviourEventParameterType.Float );
		}
	}
}
