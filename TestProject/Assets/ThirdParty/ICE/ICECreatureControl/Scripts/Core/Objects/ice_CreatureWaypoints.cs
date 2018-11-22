// ##############################################################################
//
// ice_CreatureWaypoints.cs
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
	/*
	[System.Serializable]
	public static class WaypointsRegister : System.Object
	{
		[SerializeField]
		private static List<WaypointList> m_WaypointLists;
		public static List<WaypointList> WaypointLists
		{
			get{ return m_WaypointLists; }
		}
	}*/

	[System.Serializable]
	public class WaypointObject : TargetObject
	{
		public WaypointObject() : base( TargetType.WAYPOINT ) {}
		public WaypointObject( GameObject _object ) : base( TargetType.WAYPOINT ) { OverrideTargetGameObject( _object ); }
		public WaypointObject( WaypointObject _object ) : base( TargetType.WAYPOINT ) { Copy( _object ); }

		public void Copy( WaypointObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			WaypointFoldout = _object.WaypointFoldout;
			BehaviourFoldout = _object.BehaviourFoldout;
			UseCustomBehaviour = _object.UseCustomBehaviour;
			DurationOfStay = _object.DurationOfStay;
			IsTransitPoint = _object.IsTransitPoint;

			BehaviourModeTravel = _object.BehaviourModeTravel;
			BehaviourModePatrol = _object.BehaviourModePatrol;
			BehaviourModeLeisure = _object.BehaviourModeLeisure;
			BehaviourModeRendezvous = _object.BehaviourModeRendezvous;
		}


		public bool UseCustomBehaviour = false;
		public bool BehaviourFoldout = false;
		public bool WaypointFoldout = false;

		public float DurationOfStay  = 0.0f;
		public bool IsTransitPoint = true;

		public string BehaviourModeTravel = "";
		public string BehaviourModePatrol = "";
		public string BehaviourModeLeisure = "";
		public string BehaviourModeRendezvous = "";
		
	}
	
	[System.Serializable]
	public class WaypointList : ICEObject
	{
		public WaypointList(){}
		public WaypointList( WaypointList _object ){ Copy( _object ); }

		public void Copy( WaypointList _object )
		{
			if( _object == null )
				return;

			Foldout = _object.Foldout;

			Identifier = _object.Identifier;
			Order = _object.Order;
			Ascending = _object.Ascending;

			Waypoints.Clear();
			foreach( WaypointObject _waypoint in _object.Waypoints )
				Waypoints.Add( new WaypointObject( _waypoint ) );
		}

		public bool Foldout = true;

		[SerializeField]
		private GameObject m_WaypointGroup = null;
		[XmlIgnore]
		public GameObject WaypointGroup
		{
			set{ 
				if( m_WaypointGroup != value )
				{
					m_WaypointGroup = value;

					UpdateWaypointGroup();
				}
			
			}
			get{ return m_WaypointGroup; }
		}

		public void UpdateWaypointGroup()
		{
			if( m_WaypointGroup == null || m_WaypointGroup.transform.childCount == 0 )
				return;
			
			Reset();
			
			for( int i = 0 ; i < m_WaypointGroup.transform.childCount ; i++ )
			{
				GameObject _object = m_WaypointGroup.transform.GetChild( i ).gameObject;
				
				if( _object )
				{
					Waypoints.Add( new WaypointObject( _object ) );
				}
			}
		}

		private WaypointObject m_LastWaypoint = null;
		public WaypointObject LastWaypoint{
			get{ return m_LastWaypoint; }
		}


		private int m_WaypointIndex = 0;

		public string Identifier = "";
		public WaypointOrderType Order;
		public bool Ascending = true;

		[SerializeField]
		private List<WaypointObject> m_Waypoints = new List<WaypointObject>();
		[XmlArray("Waypoints"),XmlArrayItem("WaypointObject")]
		public List<WaypointObject> Waypoints
		{
			get{ return m_Waypoints; }
		}

		private WaypointObject m_Waypoint = null;
		public WaypointObject Waypoint{
			get{ return m_Waypoint = ( m_Waypoint == null ? GetNextWaypoint() : m_Waypoint ); }
		}

		public List<WaypointObject> GetValidWaypoints()
		{
			List<WaypointObject> _waypoints = new List<WaypointObject>();

			foreach( WaypointObject _waypoint in m_Waypoints )
			{
				if( _waypoint.Enabled && _waypoint.IsValidAndReady ) 
					_waypoints.Add( _waypoint );
			}

			return _waypoints; 
		}

		public List<WaypointObject> GetEnabledWaypoints()
		{
			List<WaypointObject> _waypoints = new List<WaypointObject>();
			
			foreach( WaypointObject _waypoint in m_Waypoints )
			{
				if(  _waypoint.Enabled ) //_waypoint.TargetGameObject != null &&
					_waypoints.Add( _waypoint );
			}
			
			return _waypoints; 
		}

		public WaypointObject GetLastValidWaypoint()
		{
			WaypointObject _last_waypoint = null;

			foreach( WaypointObject _waypoint in m_Waypoints )
			{
				if( _waypoint.TargetGameObject != null && _waypoint.Enabled ) 
					_last_waypoint = _waypoint;
			}

			return _last_waypoint; 
		}

		public WaypointObject GetWaypointByName( string _name )
		{
			List<WaypointObject> _waypoints = GetEnabledWaypoints();

			foreach( WaypointObject _waypoint in _waypoints )
			{
				if( _waypoint.IsValidAndReady && _waypoint.TargetGameObject.name == _name )
					return _waypoint;
			}

			return null;
		}

		public WaypointObject GetWaypointByPosition( Vector3 _position )
		{
			WaypointObject new_waypoint = null;
			int new_waypoint_index = m_WaypointIndex;
			float distance = Mathf.Infinity;

			List<WaypointObject> _waypoints = GetEnabledWaypoints();

			for( int i = 0 ; i < _waypoints.Count ; i++ )
			{
				float tmp_distance = PositionTools.Distance( _position, _waypoints[ i ].TargetOffsetPosition );
				if( tmp_distance < distance )
				{
					new_waypoint_index = i;
					new_waypoint = _waypoints[ new_waypoint_index ];
					distance = tmp_distance;
				}
			}

			if( new_waypoint != null )
			{
				m_LastWaypoint = m_Waypoint;
				m_Waypoint = new_waypoint;
				m_WaypointIndex = new_waypoint_index;
			}

			return m_Waypoint;
		}

		public override void Reset()
		{
			m_Waypoints.Clear();
		}

		public WaypointObject GetNextWaypoint()
		{
			List<WaypointObject> _waypoints = GetEnabledWaypoints();

			if( _waypoints.Count == 0 )
				return null;

			WaypointObject new_waypoint = null;
			int new_waypoint_index = m_WaypointIndex;

			if( _waypoints.Count == 1 )
			{
				new_waypoint_index = 0;
				new_waypoint = _waypoints[ new_waypoint_index ];
			}
			else if( Order == WaypointOrderType.RANDOM )
			{
				new_waypoint_index = Random.Range(0,_waypoints.Count);
				new_waypoint = _waypoints[ new_waypoint_index ];
			}
			else 
			{
				if( Order == WaypointOrderType.PINGPONG )
				{
					if( Ascending && new_waypoint_index + 1 >= _waypoints.Count )
						Ascending = false;
					else if( ! Ascending && new_waypoint_index - 1 < 0 )
						Ascending = true;
				}
			
				if( Ascending )
				{
					new_waypoint_index++;

					if( new_waypoint_index >= _waypoints.Count )
						new_waypoint_index = 0;
				}
				else
				{
					new_waypoint_index--;
					
					if( new_waypoint_index < 0 )
						new_waypoint_index = _waypoints.Count - 1;
				}

				new_waypoint = _waypoints[ new_waypoint_index ];
			}


			if( new_waypoint != null )
			{
				m_LastWaypoint = m_Waypoint;
				m_Waypoint = new_waypoint;
				m_WaypointIndex = new_waypoint_index;
			}

			return m_Waypoint;
		
		}
	}


}
