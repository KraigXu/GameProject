// ##############################################################################
//
// ice_Creature.cs
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
	[System.Serializable]
	public class CreatureObject : ICEOwnerObject
	{
		public CreatureObject(){}
		public CreatureObject( ICEWorldBehaviour _component ) : base( _component )
		{
			Init( _component );
		}

		private ReferenceGroupObject m_ReferenceGroup = null;
		public ReferenceGroupObject ReferenceGroup{
			get{ return m_ReferenceGroup = ( m_ReferenceGroup == null ? ((ICECreatureEntity)OwnerComponent != null ? ((ICECreatureEntity)OwnerComponent).Message.ReferenceGroup:null):m_ReferenceGroup ); }
		}

		public delegate void OnTargetMoveCompleteEvent( GameObject _sender, TargetObject _target );
		public event OnTargetMoveCompleteEvent OnTargetMoveComplete;

		public override void Init( ICEWorldBehaviour _component )
		{
			base.Init( _component );
	
			Move.Init( _component );
			Essentials.Init ( _component );
			Status.Init( _component );
			Behaviour.Init( _component );
			Missions.Init ( _component );
			Interaction.Init( _component );
			Environment.Init( _component );

			if( Essentials.BehaviourModeSpawnEnabled )
				SetActiveBehaviourModeByKey( Essentials.BehaviourModeSpawn );
			else
				SetActiveBehaviourModeByKey( Essentials.BehaviourModeIdle );
		}

		public override void Reset()
		{
			if( Application.isPlaying )
			{
				//Debug.Log( "Creature.Reset: " + OwnerName );

				Status.Reset();
				Behaviour.Reset();
				Move.Reset();

				m_ActiveTarget = null;

				Essentials.Target.Reset();
				Missions.Outpost.Target.Reset(); 
				Missions.Escort.Target.Reset();

				if( Missions.Patrol.Target != null )
					Missions.Patrol.Target.Reset();

				// PREDECISIONS INTERACTORS
				foreach( InteractorObject _interactor in Interaction.Interactors )
				{
					_interactor.Reset();
					foreach( TargetObject _target in _interactor.PreparedTargets )
						_target.Reset();
				}

			}
			else
			{
				m_Interaction = new InteractionObject(OwnerComponent);
				m_Essentials = new EssentialsObject(OwnerComponent);
				m_Status = new StatusObject(OwnerComponent);
				m_Behaviour = new BehaviourObject (OwnerComponent);
				m_Missions = new MissionsObject(OwnerComponent);
				m_Move = new MoveObject(OwnerComponent);
				m_Environment = new EnvironmentObject(OwnerComponent);
			}
		}

/*
		~CreatureObject()
		{
			ICECreatureRegister.Instance.Deregister( Owner );
		}*/


		public bool UseApplicationFocus = false;

		public void AutoDetectInteractors()
		{
			foreach( ReferenceGroupObject _group in CreatureRegister.ReferenceGroupObjects )
			{
				if( _group != null && _group.Enabled && ( 
					_group.EntityType == EntityClassType.Creature ||
					_group.EntityType == EntityClassType.Player ) )
				{
					Interaction.AddInteractor( _group.ReferenceGameObject, Status );
				}
			}
		}

		[SerializeField]
		private InteractionObject m_Interaction = null;
		public InteractionObject Interaction{
			get{ return m_Interaction = ( m_Interaction == null ?new InteractionObject(OwnerComponent):m_Interaction ); }
			set{ Interaction.Copy( value ); }
		}

		[SerializeField]
		private EssentialsObject m_Essentials = null;
		public EssentialsObject Essentials{
			get{ return m_Essentials = ( m_Essentials == null ?new EssentialsObject(OwnerComponent):m_Essentials );  }
			set{ Essentials.Copy( value ); }
		}

		[SerializeField]
		private MoveObject m_Move = null;
		public MoveObject Move{
			get{ return m_Move = ( m_Move == null ?new MoveObject(OwnerComponent):m_Move ); }
			set{ Move.Copy( value ); }
		}

		[SerializeField]
		private StatusObject m_Status = null;
		public StatusObject Status{
			get{ return m_Status = ( m_Status == null ?new StatusObject(OwnerComponent):m_Status ); }
			set{ Status.Copy( value ); }
		}

		[SerializeField]
		private BehaviourObject  m_Behaviour = null;
		public BehaviourObject Behaviour{
			get{ return m_Behaviour = ( m_Behaviour == null ?new BehaviourObject(OwnerComponent):m_Behaviour ); }
			set{ Behaviour.Copy( value ); }
		}

		[SerializeField]
		private MissionsObject m_Missions = null;
		public MissionsObject Missions{
			get{ return m_Missions = ( m_Missions == null ?new MissionsObject(OwnerComponent):m_Missions ); }
			set{ Missions.Copy( value ); }
		}
			
		[SerializeField]
		private EnvironmentObject m_Environment = null;
		public EnvironmentObject Environment{
			get{ return m_Environment = ( m_Environment == null ?new EnvironmentObject(OwnerComponent):m_Environment ); }
			set{ Environment.Copy( value ); }
		}

		private List<TargetObject> m_AvailableTargets = null;
		public List<TargetObject> AvailableTargets{
			get{ return m_AvailableTargets = ( m_AvailableTargets == null ? new List<TargetObject>() : m_AvailableTargets ); }
		}

		private TargetObject m_ActiveTarget = null;				
		public TargetObject ActiveTarget{
			get{ return m_ActiveTarget; }
		}

		private TargetObject m_PreviousTarget = null;				
		public TargetObject PreviousTarget{
			get{ return m_PreviousTarget; }
		}

		private GameObject m_ActiveTargetGameObject = null;				
		public GameObject ActiveTargetGameObject{
			get{ return m_ActiveTargetGameObject; }
		}

		private GameObject m_PreviousTargetGameObject = null;				
		public GameObject PreviousTargetGameObject{
			get{ return m_PreviousTargetGameObject; }
		}
			
		public Vector3 ActiveTargetDesiredMovePosition{
			get{ return ( m_ActiveTarget != null ? ActiveTarget.DesiredTargetMovePosition : Vector3.zero ); }
		}

		public Vector3 ActiveTargetMovePosition{
			get{ return ( m_ActiveTarget != null ? ActiveTarget.TargetMovePosition : Vector3.zero ); }
		}

		public Vector3 ActiveTargetTransformPosition{
			get{ return ( m_ActiveTarget != null ? ActiveTarget.TargetTransformPosition : Vector3.zero ); }
		}

		public float ActiveTargetMovePositionDistance{
			get{ return ( m_ActiveTarget != null && Owner != null ? ActiveTarget.TargetMovePositionDistanceTo( Owner.transform.position ):0 ); }
		}

		public float ActiveTargetOffsetPositionDistance{
			get{ return ( m_ActiveTarget != null && Owner != null ? ActiveTarget.TargetOffsetPositionDistanceTo( Owner.transform.position ):0 ); }
		}

		public float ActiveTargetDistance{
			get{ return ( m_ActiveTarget != null && Owner != null ? ActiveTarget.TargetDistanceTo( Owner.transform.position ) : 0 ); }
		}

		public float ActiveTargetVerticalDistance{
			get{ return ( m_ActiveTarget != null && Owner != null ? ActiveTarget.TargetVerticalDistanceTo( Owner.transform.position ) : 0 ); }
		}

		public Vector3 ActiveTargetDirection{
			get{ return ( m_ActiveTarget != null ? ActiveTarget.TargetDirection : Vector3.zero ); }
		}
			
		public float ActiveTargetActiveTime{
			get{ return ( m_ActiveTarget != null ? m_ActiveTarget.ActiveTime : 0 ); }
		}

		public float ActiveTargetActiveTimeTotal{
			get{ return ( m_ActiveTarget != null ? m_ActiveTarget.ActiveTimeTotal : 0 ); }
		}

		private string m_ActiveTargetKey = "";				
		public string ActiveTargetKey{
			get{ return m_ActiveTargetKey; }
		}

		private string m_PreviousTargetKey = "";				
		public string PreviousTargetKey{
			get{ return m_PreviousTargetKey; }
		}

		private string m_ActiveTargetName = "";				
		public string ActiveTargetName{
			get{ return m_ActiveTargetName; }
		}

		private int m_ActiveTargetID = 0;				
		public int ActiveTargetID{
			get{ return m_ActiveTargetID; }
		}

		private string m_PreviousTargetName = "";				
		public string PreviousTargetName{
			get{ return m_PreviousTargetName; }
		}

		private int m_PreviousTargetID = 0;				
		public int PreviousTargetID{
			get{ return m_PreviousTargetID; }
		}

		public Vector3 ActiveTargetVelocity{
			get{ return ( m_ActiveTarget != null ?  m_ActiveTarget.TargetVelocity : Vector3.zero ); }
		}

		public float ActiveTargetSpeed{
			get{ return ( m_ActiveTarget != null ?  m_ActiveTarget.TargetSpeed : 0 ); }
		}
			
		private bool m_TargetChanged = false;
		public bool TargetChanged{
			get{ return m_TargetChanged; }
		}
						
		public bool IsPerceptionTime()
		{
			if( ! m_HasStandardBehaviour && ( ActiveTarget == null || ! ActiveTarget.IsValidAndReady || Status.IsPerceptionTime( Behaviour.ActiveBehaviourModeRule ) ) )
				return true;
			else
				return false;			
		}

		public bool IsReactionTime()
		{
			if( Status.IsReactionTime( Behaviour.ActiveBehaviourModeRule ) )// || Move.TargetMovePositionReached || Move.MovePositionReached ) 
				return true;
			else
				return false;			
		}

		public void AddAvailableTarget( TargetObject _target )
		{
			if( _target == null || ! _target.IsValidAndReady )
				return;
				
			_target.Selectors.ResetStatus();
			AvailableTargets.Add ( _target );
		}

		private bool m_HasStandardBehaviour = false;
		public bool HasStandardBehaviour{
			get{ return m_HasStandardBehaviour; }
		}

		public bool SelectStandardBehaviour()
		{
			string _key = "";

			// DEAD
			if( Status.IsDead && Essentials.BehaviourModeDeadEnabled )
				_key = Essentials.BehaviourModeDead;

			// 
			else if( Move.IsFalling && Essentials.BehaviourModeFallEnabled )
				_key = Essentials.BehaviourModeFall;
						 
			// WOUNDED OR SOMETHINK LIKE THIS
			//else if( Status.IsWounded && Essentials.BehaviourModeWoundedEnabled )
			//	_key = Essentials.BehaviourModeWounded;
			else if( Status.ReposeRequired && Essentials.BehaviourModeReposeEnabled )
				_key = Essentials.BehaviourModeRepose;	

			// STANDARD MOVES
			else if( Move.IsClimbing && Essentials.BehaviourModeClimbEnabled )
			{
				if( Essentials.BehaviourModeClimbDownEnabled && Move.ClimbingDirection.y < 0 )
					_key = Essentials.BehaviourModeClimbDown;
				else
					_key = Essentials.BehaviourModeClimb;
			}
			else if( Move.IsCrossBelowRequired && Essentials.BehaviourModeSlideEnabled )
				_key = Essentials.BehaviourModeSlide;
			else if( Move.IsCrossOverRequired && Essentials.BehaviourModeVaultEnabled )
				_key = Essentials.BehaviourModeVault;
			else if( Move.IsJumping && Essentials.BehaviourModeJumpEnabled )
				_key = Essentials.BehaviourModeJump;
			else if( Move.IsBlocked )
			{
				if( Essentials.BehaviourModeWaitEnabled )
					_key = Essentials.BehaviourModeWait;
				else
					_key = Essentials.BehaviourModeIdle;
			}
			
			if( string.IsNullOrEmpty( _key ) )
				m_HasStandardBehaviour = false;
			else
				m_HasStandardBehaviour = ( SetActiveBehaviourModeByKey( _key ) || Behaviour.ActiveBehaviourModeKey == _key ? true : false );

			return m_HasStandardBehaviour;
		}

		public void SelectActiveTarget()
		{
			SelectorObject _selector = new SelectorObject( this );
			SetActiveTarget( _selector.SelectBestTarget( m_AvailableTargets ) );
			_selector = null;
		}

		public void ResetActiveTarget()
		{
			// handle previous target
			m_PreviousTarget = m_ActiveTarget;
			m_PreviousTargetGameObject = m_ActiveTargetGameObject;
			m_PreviousTargetID = m_ActiveTargetID;
			m_PreviousTargetName = m_ActiveTargetName;
			m_PreviousTargetKey = m_ActiveTargetName + m_ActiveTargetID.ToString();

			if( m_PreviousTarget != null )
			{
				m_PreviousTarget.SetActive( false );

				if( m_PreviousTarget.EntityComponent != null )
					m_PreviousTarget.EntityComponent.RemoveActiveCounterpart( OwnerComponent as ICECreatureEntity );

				if( m_PreviousTarget.GroupMessage.Type != BroadcastMessageType.NONE )
				{
					BroadcastMessageDataObject _data = new BroadcastMessageDataObject();
					_data.Type = m_PreviousTarget.GroupMessage.Type;
					_data.TargetGameObject = m_PreviousTargetGameObject;
					_data.Command = "";

					ICECreatureEntity _entity = OwnerComponent as ICECreatureEntity;
					if( _entity != null )
						_entity.Message.SendGroupMessage( _data );
				}
			}

			m_ActiveTarget = null;
			m_ActiveTargetGameObject = null;
			m_ActiveTargetID = 0;
			m_ActiveTargetName = "";
			m_ActiveTargetKey = "";
		}

		/// <summary>
		/// Sets the active target.
		/// </summary>
		/// <param name="_target">_target.</param>
		public void SetActiveTarget( TargetObject _target )
		{
			if( Status.IsDead || Status.IsSpawning )
				return;

			if( Move.Deadlock.Deadlocked )
			{
				Move.Deadlock.Reset( Owner.transform );
				if( Move.Deadlock.Action == DeadlockActionType.BEHAVIOUR )
				{
					SetActiveBehaviourModeByKey( Move.Deadlock.Behaviour );
					return;
				}
				else if( Move.Deadlock.Action == DeadlockActionType.UPDATE && m_ActiveTarget != null )
					m_ActiveTarget.Move.UpdateRandomOffset();
				else
				{
					Status.Kill();
					return;
				}
			}
				
			if( _target == null || ! _target.IsValidAndReady || Status.RecreationRequired )
				_target = Essentials.PrepareTarget( OwnerComponent );

			if( _target == null || ! _target.IsValidAndReady )
			{
				if( DebugLogIsEnabled ) PrintDebugLog( this, "Sorry, the creature have no valid target!" );
				return;
			}

			m_TargetChanged = false;
			if( IsTargetUpdatePermitted( _target ) )
			{
				// update target 
				if( ! _target.CompareTarget( m_ActiveTarget, m_ActiveTargetID ) )
				{
					// handle previous target
					ResetActiveTarget();
			
					// handle new target
					m_ActiveTarget = _target;
					m_ActiveTargetGameObject = _target.TargetGameObject;
					m_ActiveTargetID = _target.TargetID;
					m_ActiveTargetName = _target.TargetName;
					m_ActiveTargetKey = _target.TargetName + _target.TargetID.ToString();
			
					if( m_ActiveTarget != null )
					{						
						m_ActiveTarget.SetActive( OwnerComponent );

						if( m_ActiveTarget.EntityComponent != null  )
							m_ActiveTarget.EntityComponent.AddActiveCounterpart( OwnerComponent as ICECreatureEntity );

						if( m_ActiveTarget.GroupMessage.Type != BroadcastMessageType.NONE )
						{
							/*
							BroadcastMessageDataObject _data = new BroadcastMessageDataObject();

							_data.Type = m_ActiveTarget.GroupMessage.Type;
							_data.TargetGameObject = m_ActiveTarget.TargetGameObject;
							_data.Command = m_ActiveTarget.GroupMessage.Command;*/

							ICECreatureEntity _entity = OwnerComponent as ICECreatureEntity;
							if( _entity != null )
								_entity.Message.SendGroupMessage( new BroadcastMessageDataObject( m_ActiveTarget.GroupMessage.Type, m_ActiveTarget.TargetGameObject, m_ActiveTarget.GroupMessage.Command ) );
						}
					}

					if( DebugLogIsEnabled ) 
					{
						string _previus = ( m_PreviousTargetName != "" ? m_PreviousTargetName + " (" + m_PreviousTargetID + ")": "unknown" );
						string _active = ( m_ActiveTargetName != "" ? m_ActiveTargetName + " (" + m_ActiveTargetID + ")": "unknown" );

						PrintDebugLog( this, "SetActiveTarget - creature changed active target from " + _previus + " to " + _active + "." );
					}

					Move.UpdateTargets( m_ActiveTarget, Essentials.Target );

					m_TargetChanged = true;
				}
			
			
				// update target behaviour
				if( m_ActiveTarget != null )
				{
					string _key = ActiveTarget.Behaviour.CurrentBehaviourModeKey;

					if( string.IsNullOrEmpty( _key ) )
					{
						// if the active target is not a HOME or if the creature outside the max range it have to travel to reach its target
						if( ! ActiveTarget.TargetInMaxRange( Owner.transform.position ))
							_key = Essentials.BehaviourModeRun;

						// if the creature reached the TargetMovePosition it should do the rendezvous behaviour
						else if( ActiveTarget.TargetMoveComplete )
							_key = Essentials.BehaviourModeIdle;

						// in all other case the creature should be standby and do some leisure activities
						else //if( Target.TargetRandomRange > 0 )
							_key = Essentials.BehaviourModeWalk;
					}

					SetActiveBehaviourModeByKey( _key );
				}
			}
		}

		public bool SetActiveBehaviourModeByKey( string _key )
		{
			if( Behaviour == null )
				return false;

			return Behaviour.SetBehaviourModeByKey( _key );
		}

		private bool IsTargetUpdatePermitted( TargetObject _target )
		{
			if( _target == null )
				return false;
			
			if( m_ActiveTarget == null || Behaviour.ActiveBehaviourMode == null || Behaviour.ActiveBehaviourMode.Favoured.Enabled == false)
				return true;
			
			bool _permitted = true;
			
			if( ( Behaviour.ActiveBehaviourMode.Favoured.Enabled == true ) && (
				( Behaviour.ActiveBehaviourMode.Favoured.Runtime > 0 && Behaviour.BehaviourTimer < Behaviour.ActiveBehaviourMode.Favoured.Runtime ) ||
				( Behaviour.ActiveBehaviourMode.Favoured.FavouredUntilNextMovePositionReached && ! Move.MovePositionReached ) ||
				( Behaviour.ActiveBehaviourMode.Favoured.FavouredUntilTargetMovePositionReached && ! Move.TargetMovePositionReached ) ||
				( Behaviour.ActiveBehaviourMode.Favoured.FavouredUntilNewTargetInRange( _target, PositionTools.Distance( _target.TargetGameObject.transform.position, Owner.transform.position ) ) ) ||
				( Behaviour.ActiveBehaviourMode.HasActiveDetourRule && Behaviour.ActiveBehaviourMode.Favoured.FavouredUntilDetourPositionReached && ! Move.DetourComplete ) ) )
				_permitted = false;
			else
				_permitted = true;
			
			//mode check - the new mode could be also forced, so we have to check this here 
			if( _permitted == false )
			{
				BehaviourModeObject _mode = Behaviour.GetBehaviourModeByKey( _target.Behaviour.CurrentBehaviourModeKey );
				
				if( _mode != null && _mode.Favoured.Enabled == true )
				{
					if( Behaviour.ActiveBehaviourMode.Favoured.FavouredPriority > _mode.Favoured.FavouredPriority )
						_permitted = false;
					else if( Behaviour.ActiveBehaviourMode.Favoured.FavouredPriority < _mode.Favoured.FavouredPriority ) 
						_permitted = true;
					else 
						_permitted = (Random.Range(0,1) == 0?false:true);
				}
			}
			
			
			return _permitted;
		}


		public void UpdateMove()
		{
			if( Status.IsDead )
				Move.Stop();
			else
				Move.Move();


		}

		//--------------------------------------------------

		public void EarlyUpdate()
		{
			Status.EarlyUpdate( Move.DesiredVelocity );
			Behaviour.EarlyUpdate( Status );
						
			if( ActiveTarget != null && ActiveTarget.IsValidAndReady )
			{
				ActiveTarget.EarlyUpdate( OwnerComponent );	

				if( ! Status.IsPerceptionForced && ActiveTarget.TargetMoveComplete )
					Status.IsPerceptionForced = true;
				
				if( ! Status.IsReactionForced && ActiveTarget.TargetMoveComplete )
					Status.IsReactionForced = true;

				if( ActiveTarget.TargetMoveComplete )
				{
					if( OnTargetMoveComplete != null )
						OnTargetMoveComplete( Owner, ActiveTarget );
				}
			}

			SelectStandardBehaviour();
		}

		public void LateUpdate()
		{
			if( ! Status.LateUpdate() )
				return;

			Essentials.Target.LateUpdate( Owner, Move.MoveSpeed );
			Missions.Outpost.Target.LateUpdate( Owner, Move.MoveSpeed );
			Missions.Escort.Target.LateUpdate( Owner, Move.MoveSpeed );
			Missions.Patrol.LateUpdate( Owner, Move.MoveSpeed );
			Interaction.LateUpdate( Owner, Move.MoveSpeed );

			if( ActiveTarget != null )
			{
				ActiveTarget.LateUpdate( Owner, Move.MoveSpeed );	

				if( ActiveTarget.Influences.Update() )
					UpdateStatusInfluences( ActiveTarget.Influences );
			}

			Environment.SurfaceHandler.Update( this );

			if( Environment.SurfaceHandler.ActiveSurface != null )
			{
				if( Environment.SurfaceHandler.ActiveSurface.Influences.Update() )
					UpdateStatusInfluences( Environment.SurfaceHandler.ActiveSurface.Influences );

				if( Environment.SurfaceHandler.ActiveSurface.BehaviourModeKey != "" )
					SetActiveBehaviourModeByKey( Environment.SurfaceHandler.ActiveSurface.BehaviourModeKey );
			}

			if( Behaviour.LateUpdate( Status ) && Behaviour.ActiveBehaviourModeRule.Influences.Update() )
				UpdateStatusInfluences( Behaviour.ActiveBehaviourModeRule.Influences );
		}

		//--------------------------------------------------

		/// <summary>
		/// Updates the status influences.
		/// </summary>
		/// <param name="_influences">Influences.</param>
		public void UpdateStatusInfluences( InfluenceDataObject _influences ){
			UpdateStatusInfluences( new InfluenceObject( _influences ) );
		}

		/// <summary>
		/// Updates influences.
		/// </summary>
		/// <param name="_influences">Influences.</param>
		public void UpdateStatusInfluences( InfluenceObject _influences )
		{
			if( _influences == null || _influences.Enabled == false )
				return;
			
			if( _influences.UseDamageInPercent ) 
				Status.AddDamageInPercent( _influences.GetDamage() );
			else
				Status.AddDamage( _influences.GetDamage() );

			if( _influences.UseStressInPercent ) 
				Status.AddStressInPercent( _influences.GetStress() );
			else
				Status.AddStress( _influences.GetStress() );

			if( _influences.UseDebilityInPercent ) 
				Status.AddDebilityInPercent( _influences.GetDebility() );
			else
				Status.AddDebility( _influences.GetDebility() );

			if( _influences.UseHungerInPercent ) 
				Status.AddHungerInPercent( _influences.GetHunger() );
			else
				Status.AddHunger( _influences.GetHunger() );

			if( _influences.UseThirstInPercent ) 
				Status.AddThirstInPercent( _influences.GetThirst() );
			else
				Status.AddThirst( _influences.GetThirst() );


			Status.AddAggressivity( _influences.Aggressivity );
			Status.AddAnxiety( _influences.Anxiety );
			Status.AddExperience( _influences.Experience );
			Status.AddNosiness( _influences.Nosiness );
		}

		public bool m_IsVisible = false;
		public void HandleVisibility( bool _visible )
		{	/*
				m_IsVisible = _visible;
				if( m_IsVisible )
					Debug.Log( Owner.name + " is visible!" );
				else
					Debug.Log( Owner.name + " is invisible!" );
		
			*/
		}

		public void HandleEnvironmentCollider( Collider _collider, ColliderEventType _type, string _contact = "" )
		{
			if( Status.IsDead )
				return;

			if( Environment.CollisionHandler.Enabled )
			{
				List<CollisionDataObject> _collisions = Environment.CollisionHandler.CheckCollider( _collider, _contact );
				foreach( CollisionDataObject _data in _collisions )
				{

					if( _data != null )
					{
						if( _type == ColliderEventType.ENTER )
							_data.Influences.Start();
						else if( _type == ColliderEventType.EXIT )
							_data.Influences.Stop( OwnerComponent );
						else if( _type == ColliderEventType.HIT )
							UpdateStatusInfluences( _data.Influences );
						else if( _data.Influences.Update() )
							UpdateStatusInfluences( _data.Influences );
				
	
						if( _data.BehaviourModeKey != "" )
							SetActiveBehaviourModeByKey( _data.BehaviourModeKey );
					}
				}
			}
		}
	}
}
