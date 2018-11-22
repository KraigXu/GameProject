// ##############################################################################
//
// ice_CreatureMissions.cs
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
	public class MissionsObject : ICEOwnerObject
	{
		public MissionsObject(){}
		public MissionsObject( MissionsObject _object ) : base( _object ){ Copy( _object ); }
		public MissionsObject( ICEWorldBehaviour _component ) : base( _component ){}

		public void Copy( MissionsObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			Outpost.Copy( _object.Outpost );
			Escort.Copy( _object.Escort );
			Patrol.Copy( _object.Patrol );
		}

		//--------------------------------------------------
		// HOME
		//--------------------------------------------------
		[SerializeField]
		private OutpostObject m_Outpost = null;
		public OutpostObject Outpost{
			get{return m_Outpost = ( m_Outpost == null ? new OutpostObject() : m_Outpost ); }
			set{ Outpost.Copy( value ); }
		}
		
		//--------------------------------------------------
		// ESCORT
		//--------------------------------------------------
		[SerializeField]
		private EscortObject m_Escort = null;
		public EscortObject Escort{
			get{return m_Escort = ( m_Escort == null ? new EscortObject() : m_Escort ); }
			set{ Escort.Copy( value ); }
		}
		
		//--------------------------------------------------
		// PATROL
		//--------------------------------------------------
		[SerializeField]
		private PatrolObject m_Patrol = null;
		public PatrolObject Patrol{
			get{return m_Patrol = ( m_Patrol == null ? new PatrolObject() : m_Patrol ); }
			set{ Patrol.Copy( value ); }
		}

		//--------------------------------------------------
		// SCOUT
		//--------------------------------------------------
		[SerializeField]
		private ScoutObject m_Scout = new ScoutObject();
		public ScoutObject Scout
		{
			get{ return m_Scout; }
		}

		//--------------------------------------------------
		// FORMATION
		//--------------------------------------------------
		[SerializeField]
		private FormationObject m_Formation = new FormationObject();
		public FormationObject Formation
		{
			get{ return m_Formation; }
		}

		//--------------------------------------------------
		// HORDE
		//--------------------------------------------------
		[SerializeField]
		private HordeObject m_Horde = new HordeObject();
		public HordeObject Horde
		{
			get{ return m_Horde; }
		}
	}



	[System.Serializable]
	public abstract class MissionObject : ICEOwnerObject
	{
		public MissionObject( TargetType _type ){ m_Target = new TargetObject( _type ); }
		public MissionObject( MissionObject _object ) : base( _object ){ Copy( _object ); }
		public MissionObject( ICEWorldBehaviour _component ) : base( _component ){}

		[SerializeField]
		private TargetObject m_Target = null;
		public virtual TargetObject Target{
			get{ return m_Target = ( m_Target == null ? new TargetObject() : m_Target ); }
			set{ Target.Copy( value ); }
		}

		public void Copy( MissionObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			BehaviourFoldout = _object.BehaviourFoldout;

			if( Target != null && _object.Target != null )
				Target.Copy( _object.Target );
		}

		public bool BehaviourFoldout = true;
	}


	[System.Serializable]
	public class OutpostObject : MissionObject
	{
		public OutpostObject() : base( TargetType.OUTPOST ) {}
		public OutpostObject( OutpostObject _object ) : base( _object ){ Copy( _object ); }
		public OutpostObject( ICEWorldBehaviour _component ) : base( _component ){}

		public void Copy( OutpostObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			BehaviourModeTravel = _object.BehaviourModeTravel;
			BehaviourModeLeisure = _object.BehaviourModeLeisure;
			BehaviourModeRendezvous = _object.BehaviourModeRendezvous;
		}

		//--------------------------------------------------
		// Mission Home Settings
		//--------------------------------------------------

		public string BehaviourModeTravel = "";
		public string BehaviourModeLeisure = "";
		public string BehaviourModeRendezvous = "";

		public bool TargetReady()
		{
			if( Enabled == true && Target.IsValidAndReady )
				return true;
			else
				return false;
		}
		/*
		private CreatureObject m_Creature = null;
		protected CreatureObject GetCreature()
		{
			if( m_Creature == null )
				m_Creature = Owner.GetComponent<ICECreatureControl>().Creature;
			
			return m_Creature;
		}*/

		public TargetObject PrepareTarget( ICEWorldBehaviour _component )
		{
			if( ! Enabled || ! OwnerIsReady( _component ) || Target.PrepareTargetGameObject( _component ) == null || ! Target.IsValidAndReady )
				return null;
				
			Target.Behaviour.SetDefault();

			// if the active target is not a OUTPOST or if the creature outside the max range it have to travel to reach its target
			//if( _creature.ActiveTarget == null || _creature.ActiveTarget.Type != TargetType.OUTPOST || ! Target.TargetInMaxRange( _owner.transform.position ))
			if( ! Target.TargetInMaxRange( Owner.transform.position ) )
				Target.Behaviour.CurrentBehaviourModeKey = BehaviourModeTravel;

			// if the creature reached the TargetMovePosition it should do the rendezvous behaviour
			else if( Target.TargetMoveComplete )
				Target.Behaviour.CurrentBehaviourModeKey = BehaviourModeRendezvous;

			// in all other case the creature should be standby and do some leisure activities
			else if( Target.Move.HasRandomRange )
				Target.Behaviour.CurrentBehaviourModeKey = BehaviourModeLeisure;

			return Target;
		}
	}

	[System.Serializable]
	[XmlRoot("EscortObject")]
	public class EscortObject : MissionObject
	{
		public EscortObject() : base( TargetType.ESCORT ) {}
		public EscortObject( EscortObject _object ) : base( _object ){ Copy( _object ); }
		public EscortObject( ICEWorldBehaviour _component ) : base( _component ){}

		public void Copy( EscortObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			DelayEscort = _object.DelayEscort;
			DelayFollow = _object.DelayFollow;
			DelayStandby = _object.DelayStandby;
			DelayIdle = _object.DelayIdle;

			DurationStandby = _object.DurationStandby;

			BehaviourModeEscort = _object.BehaviourModeEscort;
			BehaviourModeFollow = _object.BehaviourModeFollow;
			BehaviourModeStandby = _object.BehaviourModeStandby;
			BehaviourModeIdle = _object.BehaviourModeIdle;
		}

		//--------------------------------------------------
		// Mission Escort Settings
		//--------------------------------------------------

		public float DelayEscort = 0.0f;
		public float DelayFollow = 0.0f;
		public float DelayStandby = 1.0f;
		public float DelayIdle  = 2.0f;

		public float DurationStandby = 10.0f;

	
		public string BehaviourModeEscort = "";
		public string BehaviourModeFollow = "";
		public string BehaviourModeStandby = "";
		public string BehaviourModeIdle = "";

		public bool TargetReady()
		{
			if( Enabled == true && Target.IsValidAndReady )
				return true;
			else
				return false;
		}

		public TargetObject PrepareTarget( ICEWorldBehaviour _component, CreatureObject _creature )
		{
			if( ! Enabled || ! OwnerIsReady( _component ) || _creature == null || Target.PrepareTargetGameObject( _component ) == null || ! Target.IsValidAndReady )
				return null;

			Target.Behaviour.SetDefault();

			// if the creature has reached the target move position and the target doesn't move the creature should be standby or idle
			if ( Target.TargetMoveComplete && Target.TargetVelocity == Vector3.zero )
			{
				if( Target.Behaviour.CurrentBehaviourModeKey == BehaviourModeStandby && _creature.Behaviour.BehaviourTimer > DurationStandby )
					Target.Behaviour.CurrentBehaviourModeKey = BehaviourModeIdle;
					
				else if( Target.Behaviour.CurrentBehaviourModeKey != BehaviourModeIdle )
					Target.Behaviour.CurrentBehaviourModeKey = BehaviourModeStandby;
			}

			else if( ! Target.TargetInMaxRange( Owner.transform.position ) )
				Target.Behaviour.CurrentBehaviourModeKey = BehaviourModeFollow;

			else
				Target.Behaviour.CurrentBehaviourModeKey = BehaviourModeEscort;
			/*
			else if ( Target.TargetMoveComplete && Target.TargetVelocity > 0 )
				Target.BehaviourModeKey = BehaviourModeEscort;

			else if( Target.TargetVelocity > 0 || _creature.Behaviour.BehaviourModeKey != BehaviourModeIdle )
				Target.BehaviourModeKey = BehaviourModeFollow;
*/
			return Target;
		}
	}


	[System.Serializable]
	public class PatrolObject : MissionObject
	{
		public PatrolObject() : base( TargetType.WAYPOINT ) {}
		public PatrolObject( PatrolObject _object ) : base( _object ){ Copy( _object ); }
		public PatrolObject( ICEWorldBehaviour _component ) : base( _component ){}

		public void Copy( PatrolObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			Waypoints.Copy( _object.Waypoints ); 

			IsTransitPoint = _object.IsTransitPoint;
			DurationOfStay = _object.DurationOfStay;

			BehaviourModeTravel = _object.BehaviourModeTravel;
			BehaviourModePatrol = _object.BehaviourModePatrol;
			BehaviourModeLeisure = _object.BehaviourModeLeisure;
			BehaviourModeRendezvous = _object.BehaviourModeRendezvous;
		}

		public bool TargetReady()
		{
			if( Enabled == true && hasWaypoints == true && Target != null && Target.IsValidAndReady )
				return true;
			else
				return false;
		}

		[XmlIgnore]
		public override TargetObject Target{
			get{ return Waypoints.Waypoint as TargetObject; }
		}

		//--------------------------------------------------
		// Mission Patrol Settings
		//--------------------------------------------------
		[SerializeField]
		private WaypointList m_Waypoints = new WaypointList();
		public WaypointList Waypoints
		{
			get{ return m_Waypoints = ( m_Waypoints == null ? new WaypointList() : m_Waypoints ); }
			set{ Waypoints.Copy( value ); }
		}

		public bool hasWaypoints{
			get{
				if( Waypoints.Waypoints.Count > 0 )
					return true;
				else
					return false;
			}
		}

		public bool WaypointsFoldout = true;
		public bool IsTransitPoint = true;
		public float DurationOfStay = 0;
		public float GetDesiredDurationOfStay()
		{
			if( Waypoints.Waypoint != null && Waypoints.Waypoint.UseCustomBehaviour )
				return Waypoints.Waypoint.DurationOfStay;
			else
				return DurationOfStay;
		}

		public bool GetIsTransitPoint()
		{
			if( Waypoints.Waypoint != null && Waypoints.Waypoint.UseCustomBehaviour )
				return Waypoints.Waypoint.IsTransitPoint;
			else
				return IsTransitPoint;
		}

		public string BehaviourModeTravel = "";
		public string BehaviourModePatrol = "";
		public string BehaviourModeLeisure = "";
		public string BehaviourModeRendezvous = "";

		public string GetBehaviourModeTravel()
		{
			if( Waypoints.Waypoint != null && Waypoints.Waypoint.UseCustomBehaviour )
				return Waypoints.Waypoint.BehaviourModeTravel;
			else
				return BehaviourModeTravel;
		}

		public string GetBehaviourModeTravelByIndex( int _index )
		{
			if( _index > 0 && _index < Waypoints.Waypoints.Count && Waypoints.Waypoints[_index] != null && Waypoints.Waypoints[_index].UseCustomBehaviour )
				return Waypoints.Waypoints[_index].BehaviourModeTravel;
			else
				return BehaviourModeTravel;
		}

		public string GetBehaviourModePatrol()
		{
			if( Waypoints.Waypoint != null && Waypoints.Waypoint.UseCustomBehaviour )
				return Waypoints.Waypoint.BehaviourModePatrol;
			else
				return BehaviourModePatrol;
		}
		
		public string GetBehaviourModePatrolByIndex( int _index )
		{
			if( _index > 0 && _index < Waypoints.Waypoints.Count && Waypoints.Waypoints[_index] != null && Waypoints.Waypoints[_index].UseCustomBehaviour )
				return Waypoints.Waypoints[_index].BehaviourModePatrol;
			else
				return BehaviourModePatrol;
		}

		public string GetBehaviourModeLeisure()
		{
			if( Waypoints.Waypoint != null && Waypoints.Waypoint.UseCustomBehaviour )
				return Waypoints.Waypoint.BehaviourModeLeisure;
			else
				return BehaviourModeLeisure;
		}

		public string GetBehaviourModeLeisureByIndex( int _index )
		{
			if( _index > 0 && _index < Waypoints.Waypoints.Count && Waypoints.Waypoints[_index] != null && Waypoints.Waypoints[_index].UseCustomBehaviour )
			   return Waypoints.Waypoints[_index].BehaviourModeLeisure;
			else
				return BehaviourModeLeisure;
		}

		public string GetBehaviourModeRendezvous()
		{
			if( Waypoints.Waypoint != null && Waypoints.Waypoint.UseCustomBehaviour )
				return Waypoints.Waypoint.BehaviourModeRendezvous;
			else
				return BehaviourModeLeisure;
		}
		
		public string GetBehaviourModeRendezvousByIndex( int _index )
		{
			if( _index > 0 && _index < Waypoints.Waypoints.Count && Waypoints.Waypoints[_index] != null && Waypoints.Waypoints[_index].UseCustomBehaviour )
				return Waypoints.Waypoints[_index].BehaviourModeRendezvous;
			else
				return BehaviourModeRendezvous;
		}

		private float m_DurationOfStayTimer = 0;
		private float m_DurationOfStayUpdateTime = 0;


		/// <summary>
		/// Prepares a waypoint target.
		/// </summary>
		/// <returns>a waypoint target as potential target candidate</returns>
		/// <description>The UpdateTarget methods only prepares and/or preselect their targets to provide a potential 
		/// target candidates for the final selection.
		/// </description>
		public TargetObject PrepareTarget( ICEWorldBehaviour _component, CreatureObject _creature )
		{
			if( ! Enabled || ! OwnerIsReady( _component ) || _creature == null || Target.PrepareTargetGameObject( _component ) == null || ! Target.IsValidAndReady )
				return null;

			// as long as the creature is inside the max range, the duration of stay will measured, otherwise the timer will adjusted to zero.
			if( Target.TargetInMaxRange( Owner.transform.position ) )
			{
				if( m_DurationOfStayUpdateTime > 0 )
					m_DurationOfStayTimer += Time.time - m_DurationOfStayUpdateTime;

				m_DurationOfStayUpdateTime = Time.time;
			}
			else
			{
				m_DurationOfStayUpdateTime = 0;
				m_DurationOfStayTimer = 0;
			}

			Target.Behaviour.SetDefault();

			// if the active target is not a WAYPOINT we have to find the nearest waypoint and set the travel bahaviour 
			if( _creature.ActiveTarget == null || _creature.ActiveTarget.Type != TargetType.WAYPOINT )
			{
				// btw. GetWaypointByPosition() changes the target, so GetBehaviourModeTravel() returns the behaviour of the new waypoint,
				// which means, that the travel behavour always specifies the approach and not the departure.
				Waypoints.GetWaypointByPosition( Owner.transform.position );
				Target.Behaviour.CurrentBehaviourModeKey = GetBehaviourModeTravel();
			}
		
			// our creature have reached the max range of the given target waypoint - the max range is the random range plus target stop distance, 
			// or if the random range is zero just the target stop distance. 
			else if( Target.TargetMoveComplete )
			{
				if( GetIsTransitPoint() || GetDesiredDurationOfStay() == 0 || m_DurationOfStayTimer >= GetDesiredDurationOfStay() )
				{
					// btw. GetNextWaypoint() changes the target, so GetBehaviourModePatrol() returns the behaviour of the new waypoint,
					// which means, that a patrol behavour always specifies the approach and not the departure.
					Waypoints.GetNextWaypoint();
					Target.Behaviour.CurrentBehaviourModeKey = GetBehaviourModePatrol();
				}
				else
				{
					Target.Behaviour.CurrentBehaviourModeKey = GetBehaviourModeRendezvous();
				}
			}
			else if( ! GetIsTransitPoint() && Target.TargetInMaxRange( Owner.transform.position ) )
			{
				Target.Behaviour.CurrentBehaviourModeKey = GetBehaviourModeLeisure();
			}
			else
			{
				// before our creature can start with the patrol behaviour, it has to reach the first waypoint, so we check the 
				// previous target and if this is null or not a waypoint our creature is still travelling to the nearest waypoint
				// which we have found in the first rule.
				if( _creature.PreviousTarget != null && _creature.PreviousTarget.Type != TargetType.WAYPOINT )
					Target.Behaviour.CurrentBehaviourModeKey = GetBehaviourModeTravel();
				
				// now it looks that our creature is on the way between two waypoints and so it should use the patrol behaviour  
				else
					Target.Behaviour.CurrentBehaviourModeKey = GetBehaviourModePatrol();
			}

/*
			if( _creature.Move.CurrentTarget == Target && _creature.Move.HasDetour && _creature.Move.DetourPositionReached( _owner.transform.position ) )
			{
			}*/

			return Target;
		}

		public void LateUpdate( GameObject _owner, float _speed )
		{
			for( int _i = 0 ; _i < Waypoints.Waypoints.Count ; _i++ )
			{
				if( Waypoints.Waypoints[_i] != null && Waypoints.Waypoints[_i].Enabled )
					Waypoints.Waypoints[_i].LateUpdate( _owner, _speed );
			}
		}
	}

	[System.Serializable]
	public class CustomMissionObject : MissionObject
	{
		public CustomMissionObject() : base( TargetType.ESCORT ) {}
		public CustomMissionObject( EscortObject _object ) : base( _object ){ Copy( _object ); }
		public CustomMissionObject( ICEWorldBehaviour _component ) : base( _component ){}

		public void Copy( CustomMissionObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );
		}
	}


	[System.Serializable]
	public class ScoutObject : MissionObject
	{
		public ScoutObject() : base( TargetType.UNDEFINED ) {}
	}

	[System.Serializable]
	public class FormationObject : MissionObject
	{
		public FormationObject() : base( TargetType.UNDEFINED ) {}

	}

	[System.Serializable]
	public class HordeObject : MissionObject
	{
		public HordeObject() : base( TargetType.UNDEFINED ) {}
	}

}
