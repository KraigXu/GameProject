// ##############################################################################
//
// ICECreatureController.cs
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

namespace ICE.Creatures
{

	public abstract class ICECreatureCharacter : ICECreatureOrganism {

		#region Entity Values
		/// <summary>
		/// Gets the entity classification type of the object
		/// </summary>
		/// <value>The classification type of the entity.</value>
		/// <description>The EntityClassType will be used to quickly 
		/// identify the correct class type of a derived entity object 
		/// without casts
		/// </description>
		public override EntityClassType EntityType{
			get{ return EntityClassType.Creature; }
		}

		protected override void OnRegisterBehaviourEvents()
		{
			RegisterBehaviourEvent( "ApplyDamage", BehaviourEventParameterType.Float, "Damage Value", "Damage Value" );
			RegisterBehaviourEvent( "ApplyHunger", BehaviourEventParameterType.Float );
			RegisterBehaviourEvent( "ApplyThirst", BehaviourEventParameterType.Float );
			RegisterBehaviourEvent( "ApplyDebility", BehaviourEventParameterType.Float );
			RegisterBehaviourEvent( "ApplyStress", BehaviourEventParameterType.Float );
			RegisterBehaviourEvent( "ApplyAggressivity", BehaviourEventParameterType.Float );
			RegisterBehaviourEvent( "ApplyAnxiety", BehaviourEventParameterType.Float );
			RegisterBehaviourEvent( "ApplyExperience", BehaviourEventParameterType.Float );
			RegisterBehaviourEvent( "ApplyNosiness", BehaviourEventParameterType.Float );
			RegisterBehaviourEvent( "TriggerBehaviourRuleAudio" );
			RegisterBehaviourEvent( "TriggerBehaviourRuleEvent", BehaviourEventParameterType.Integer );
			RegisterBehaviourEvent( "TriggerTargetEvent", BehaviourEventParameterType.Integer );
		}

		#endregion
			
		public DisplayData Display = new DisplayData();

		private bool m_SenseCoroutineIsRunning = false;
		private bool m_ReactCoroutineIsRunning = false;

		public override bool Pause{
			get{ return m_Pause; }
			set{ m_Pause = value; }
		}

		/// <summary>
		/// Reset this instance.
		/// </summary>
		public override void Reset()
		{
			Creature.Reset();

			// RESET CUSTOM MISSIONS
			if( Missions == null )
				m_Missions = this.GetComponents<ICECreatureMission>();
				
			if( Missions != null )
			{
				foreach( ICECreatureMission _mission in Missions ){
					_mission.Reset();
				}
			}
		}

		[SerializeField]
		private CreatureObject m_Creature = null;
		public CreatureObject Creature{
			set{ m_Creature = value; }
			get{ return m_Creature = ( m_Creature == null ?new CreatureObject(this):m_Creature ); }
		}

		[SerializeField]
		private ICECreatureMission[] m_Missions = null;
		public ICECreatureMission[] Missions{
			get{ return m_Missions; }
		}

		public override ICE.World.Objects.EntityStatusObject Status {
			get { return Creature.Status; }
		}


		#region Creature Events

		public abstract void OnUpdateBegin();
		public abstract void OnSenseComplete();
		public abstract void OnReactComplete();
		public abstract void OnUpdateComplete();

		public delegate void OnTargetMoveCompleteEvent( GameObject _sender, TargetObject _target );
		public event OnTargetMoveCompleteEvent OnTargetMoveComplete;

		public delegate void OnCustomAnimationEvent();
		public event OnCustomAnimationEvent OnCustomAnimation;

		public delegate void OnCustomAnimationUpdateEvent();
		public event OnCustomAnimationUpdateEvent OnCustomAnimationUpdate;
	
		public delegate void OnBehaviourModeChangedEvent( GameObject _sender, BehaviourModeObject _new_mode, BehaviourModeObject _last_mode );
		public event OnBehaviourModeChangedEvent OnBehaviourModeChanged;

		public delegate void OnBehaviourModeRuleChangedEvent( GameObject _sender, BehaviourModeRuleObject _new_rule, BehaviourModeRuleObject _last_rule );
		public event OnBehaviourModeRuleChangedEvent OnBehaviourModeRuleChanged;

		public delegate void OnTargetMovePositionReachedEvent( GameObject _sender, TargetObject _target );
		public event OnTargetMovePositionReachedEvent OnTargetMovePositionReached;

		public delegate void OnMoveCompleteEvent( GameObject _sender, TargetObject _target );
		public event OnMoveCompleteEvent OnMoveComplete;

		public delegate void OnMoveUpdatePositionEvent( GameObject _sender, Vector3 _origin_position, ref Vector3 _new_position );
		public event OnMoveUpdatePositionEvent OnUpdateMovePosition;

		public delegate void OnCustomMoveEvent( GameObject _sender, ref Vector3 _new_position, ref Quaternion _new_rotation );
		public event OnCustomMoveEvent OnCustomMove;

		public delegate void OnMoveUpdateStepPositionEvent( GameObject _sender, Vector3 _origin_position, ref Vector3 _new_position );
		public event OnMoveUpdateStepPositionEvent OnUpdateStepPosition;

		public delegate void OnMoveUpdateStepRotationEvent( GameObject _sender, Quaternion _origin_rotation, ref Quaternion _new_rotation );
		public event OnMoveUpdateStepRotationEvent OnUpdateStepRotation;

		public delegate void OnUpdateDesiredMovePositionEvent( GameObject _sender, ref Vector3 _position );
		public event OnUpdateDesiredMovePositionEvent OnUpdateDesiredMovePosition;

		public delegate void OnDeadEvent( GameObject _sender );
		public event OnDeadEvent OnDead;

		public delegate void OnRemoveEvent( GameObject _sender );
		public event OnRemoveEvent OnRemove;

		public virtual void DoTargetMoveComplete( GameObject _sender, TargetObject _target )
		{
			if( DebugLogIsEnabled )
				PrintDebugLog( "DoTargetMoveComplete" );

			if( OnTargetMoveComplete != null )
				OnTargetMoveComplete( _sender, _target );
		}

		public virtual void DoCustomAnimation()
		{
			if( DebugLogIsEnabled )
				PrintDebugLog( "DoCustomAnimation" );

			if( OnCustomAnimation != null )
				OnCustomAnimation();
		}

		public virtual void DoCustomAnimationUpdate()
		{
			if( DebugLogIsEnabled )
				PrintDebugLog( "DoCustomAnimationUpdate" );

			if( OnCustomAnimationUpdate != null )
				OnCustomAnimationUpdate();
		}

		public virtual void DoBehaviourModeChanged( GameObject _sender, BehaviourModeObject _new_mode, BehaviourModeObject _last_mode )
		{
			if( DebugLogIsEnabled )
				PrintDebugLog( "DoBehaviourModeChanged - creature changed behaviour from " + ( _last_mode != null ? _last_mode.Key:"UNKNOWN" ) + " to " + ( _new_mode != null ? _new_mode.Key:"UNKNOWN" ) + "!" );

			if( OnBehaviourModeChanged != null )
				OnBehaviourModeChanged(_sender,_new_mode,_last_mode);
		}

		public virtual void DoBehaviourModeRuleChanged( GameObject _sender, BehaviourModeRuleObject _new_rule, BehaviourModeRuleObject _last_rule )
		{
			if( DebugLogIsEnabled )
			{
				if( _new_rule != null )
					PrintDebugLog( "DoBehaviourModeRuleChanged - '" + Creature.Behaviour.ActiveBehaviourModeKey + "' changed rule " + ( _last_rule != null ? _last_rule.Index + " to rule " : "" ) + _new_rule.Index );
				else
					PrintDebugLog( "DoBehaviourModeRuleChanged - '" + Creature.Behaviour.ActiveBehaviourModeKey + "' has empty rule!" );
			}
				
			if( OnBehaviourModeRuleChanged != null )
				OnBehaviourModeRuleChanged(_sender,_new_rule,_last_rule);
		}

		public virtual void DoTargetMovePositionReached( GameObject _sender, TargetObject _target )
		{
			if( DebugLogIsEnabled )
			{
				if( _target != null )
					PrintDebugLog( "DoTargetMovePositionReached - " + _target.TargetMovePosition.ToString() + " was reached!" );
				else
					PrintDebugLog( "DoTargetMovePositionReached" );
			}
			
			if( OnTargetMovePositionReached != null )
				OnTargetMovePositionReached( _sender, _target );
		}

		public virtual void DoMoveComplete( GameObject _sender, TargetObject _target )
		{
			if( OnMoveComplete != null )
				OnMoveComplete( _sender, _target );
		}

		public virtual void DoUpdateDesiredMovePosition( GameObject _sender, ref Vector3 _position )
		{
			if( OnUpdateDesiredMovePosition != null )
				OnUpdateDesiredMovePosition( _sender, ref _position );
		}

		public virtual void DoMoveUpdatePosition( GameObject _sender, Vector3 _origin_position, ref Vector3 _new_position )
		{
			if( OnUpdateMovePosition != null )
				OnUpdateMovePosition( _sender, _origin_position, ref _new_position );
		}

		public virtual void DoMoveUpdateStepPosition( GameObject _sender, Vector3 _origin_position, ref Vector3 _new_position )
		{
			if( OnUpdateStepPosition != null )
				OnUpdateStepPosition( _sender, _origin_position, ref _new_position );
		}

		public virtual void DoMoveUpdateStepRotation( GameObject _sender, Quaternion _origin_rotation, ref Quaternion _new_rotation )
		{
			if( OnUpdateStepRotation != null )
				OnUpdateStepRotation( _sender, _origin_rotation, ref _new_rotation );
		}

		public virtual void DoCustomMove( GameObject _sender, ref Vector3 _new_position, ref Quaternion _new_rotation )
		{
			if( OnCustomMove != null )
				OnCustomMove( _sender, ref _new_position, ref _new_rotation );
		}

		public virtual void DoRemoveRequest( GameObject _sender )
		{
			if( OnDead != null )
				OnDead( _sender );
		}

		public virtual void DoRemove( GameObject _sender )
		{
			if( OnRemove != null )
				OnRemove( _sender );
		}

		#endregion

		#region Unity Events

		public override void Awake () {

			if( RuntimeBehaviour.UseDontDestroyOnLoad )
				DontDestroyOnLoad( this.gameObject );

			Message.Init( this );

			m_LastPosition = transform.position;
			m_LastRotation = transform.rotation;

			// wakes up the creature ...
			Creature.Init( this );

			Creature.OnTargetMoveComplete += DoTargetMoveComplete;

			Creature.Behaviour.OnBehaviourModeChanged += DoBehaviourModeChanged;
			Creature.Behaviour.OnBehaviourModeRuleChanged += DoBehaviourModeRuleChanged;

			Creature.Behaviour.BehaviourAnimation.OnCustomAnimation += DoCustomAnimation;
			Creature.Behaviour.BehaviourAnimation.OnCustomAnimationUpdate += DoCustomAnimationUpdate;

			Creature.Move.OnTargetMovePositionReached += DoTargetMovePositionReached;
			Creature.Move.OnMoveComplete += DoMoveComplete;
			Creature.Move.OnUpdateMovePosition += DoMoveUpdatePosition;
			Creature.Move.OnUpdateStepPosition += DoMoveUpdateStepPosition;
			Creature.Move.OnUpdateStepRotation += DoMoveUpdateStepRotation;
			Creature.Move.OnCustomMove += DoCustomMove;
			Creature.Move.OnUpdateDesiredMovePosition += DoUpdateDesiredMovePosition;

			Creature.Status.OnRemoveRequest += DoRemoveRequest;
			Creature.Status.OnRemove += DoRemove;

			RuntimeBehaviour.RuntimeRenaming( this );
			Register();
		}

		public override void Start () {

			if( IsRemoteClient )
				return;

			m_Missions = this.GetComponents<ICECreatureMission>();

			OnEnable();
		}

		public override void OnEnable() {

			if( IsRemoteClient )
				return;

			if( Application.isPlaying && Creature.Status.IsDead )
				Creature.Reset();
			
			// if script or gameobject were disabled, we have to start the coroutine again ... 
			if( RuntimeBehaviour.UseCoroutine )
			{
				if( m_SenseCoroutineIsRunning == false )
					StartCoroutine( SenseCoroutine() );

				if( m_ReactCoroutineIsRunning == false )
					StartCoroutine( ReactCoroutine() );
			}
		}

		public override void OnDisable() {

			if( IsRemoteClient )
				return;

			Creature.SetActiveBehaviourModeByKey( Creature.Essentials.BehaviourModeIdle );
		
			// deactivating the gameobject will stopping the coroutine, so we capture the ondisable 
			// and stops the coroutine controlled ... btw. if only the script was disabled, the coroutine would be 
			// still running, but we don't need the coroutine if the rest of the script isn't running and so we 
			// capture all cases
			if( RuntimeBehaviour.UseCoroutine )
			{
				StopCoroutine( SenseCoroutine() );
				StopCoroutine( ReactCoroutine() );
			}

			m_SenseCoroutineIsRunning = false;
			m_ReactCoroutineIsRunning = false;
		}
			
		//********************************************************************************
		// Update
		//********************************************************************************
		public override void Update () 
		{
			if( IsRemoteClient )
				return;
	
			UpdateImpactForce();
		
			if( RuntimeBehaviour.IsLost == true || RuntimeBehaviour.CullingOptions.CheckCullingConditions( this ) == true )
				Remove();

			OnUpdateBegin();
			Creature.EarlyUpdate();

			if( ! RuntimeBehaviour.UseCoroutine )
			{
				Sense();
				React();
			}
			else if( gameObject.activeInHierarchy )
			{
				if( m_SenseCoroutineIsRunning == false )
					StartCoroutine( SenseCoroutine() );

				if( m_ReactCoroutineIsRunning == false )
					StartCoroutine( ReactCoroutine() );
			}

			Creature.UpdateMove();

			OnUpdateComplete();
		}


		public override void LateUpdate () {

			if( IsRemoteClient )
				return;
			
			Creature.LateUpdate();
			base.LateUpdate();

		}

		public override void OnCollisionEnter(Collision _collision) 
		{
			if( IsRemoteClient )
				return;
			
			if( _collision == null )
				return;

			//Creature.Move.EnterCollision( _collision );

			if( Creature.Environment.CollisionHandler.UseCollider )
			{
				string _name = "";
				if( _collision.contacts.Length > 0 )
					_name = _collision.contacts[0].thisCollider.name;

				Creature.HandleEnvironmentCollider( _collision.collider, ColliderEventType.ENTER, _name );
			}
		}

		public override void OnCollisionStay(Collision _collision) 
		{
			if( IsRemoteClient )
				return;
			
			if( _collision == null )
				return;

			//Creature.Move.HandleCollision( _collision );

			if( Creature.Environment.CollisionHandler.UseCollider )
			{
				string _name = "";
				if( _collision.contacts.Length > 0 )
					_name = _collision.contacts[0].thisCollider.name;

				Creature.HandleEnvironmentCollider( _collision.collider, ColliderEventType.STAY, _name );
			}
		}

		public override void OnCollisionExit(Collision _collision) 
		{
			if( IsRemoteClient )
				return;

			if( _collision == null )
				return;

			//Creature.Move.ExitCollision( _collision );

			if( Creature.Environment.CollisionHandler.UseCollider )
			{
				string _name = "";
				if( _collision.contacts.Length > 0 )
					_name = _collision.contacts[0].thisCollider.name;

				Creature.HandleEnvironmentCollider( _collision.collider, ColliderEventType.EXIT, _name );
			}
		}

		public override void OnTriggerEnter( Collider _collider ) 
		{
			base.OnTriggerEnter( _collider );
			if( IsRemoteClient )
				return;
			
			if( _collider == null )
				return;

			if( Creature.Status.UseShelter && _collider.gameObject.tag == Creature.Status.ShelterTag )
				Creature.Status.IsSheltered = true;
			if( Creature.Status.UseIndoor && _collider.gameObject.tag == Creature.Status.IndoorTag )
				Creature.Status.IsIndoor = true;

			if( Creature.Environment.CollisionHandler.UseTrigger )
				Creature.HandleEnvironmentCollider( _collider, ColliderEventType.ENTER );
		}
		
		public override void OnTriggerStay( Collider _collider ) 
		{
			base.OnTriggerStay( _collider );
			if( IsRemoteClient )
				return;
			
			if( _collider == null )
				return;

			if( Creature.Environment.CollisionHandler.UseTrigger )
				Creature.HandleEnvironmentCollider( _collider, ColliderEventType.STAY );
		}

		public override void OnTriggerExit( Collider _collider ) 
		{
			base.OnTriggerExit( _collider );
			if( IsRemoteClient )
				return;
			
			if( _collider == null )
				return;
			
			if( Creature.Status.UseShelter && _collider.gameObject.tag == Creature.Status.ShelterTag )
				Creature.Status.IsSheltered = false;
			if( Creature.Status.UseIndoor && _collider.gameObject.tag == Creature.Status.IndoorTag )
				Creature.Status.IsIndoor = false;

			if( Creature.Environment.CollisionHandler.UseTrigger )
				Creature.HandleEnvironmentCollider( _collider, ColliderEventType.EXIT );
		}

		public override void OnControllerColliderHit( ControllerColliderHit hit ) 
		{
			if( IsRemoteClient )
				return;
			
			if( hit == null )
				return;

			if( Creature.Environment.CollisionHandler.UseCharacterController )
				Creature.HandleEnvironmentCollider( hit.collider, ColliderEventType.HIT );
			/*
			Rigidbody body = hit.collider.attachedRigidbody;
			if (body == null || body.isKinematic)
				return;
			
			if (hit.moveDirection.y < -0.3F)
				return;
			
			Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
			body.velocity = pushDir * 100;*/
		}

		#endregion 

		void OnBecameVisible()
		{
			Creature.HandleVisibility( true );
		}

		void OnBecameInvisible()
		{
			Creature.HandleVisibility( false );
		}

		#region Creature Behaviour Methods

		/// <summary>
		/// Applies damage is the standard method to apply damage to the creature ...
		/// </summary>
		/// <param name="_damage">Damage.</param>
		public override void ApplyDamage( float _damage ){

			if( Creature.Essentials.BehaviourModeImpactEnabled )
				Creature.SetActiveBehaviourModeByKey( Creature.Essentials.BehaviourModeImpact );
			
			AddDamage( _damage, Vector3.zero, Vector3.zero, null, 0 );
		}

		public virtual void ApplyHunger( float _value ){
			Creature.Status.AddHunger( _value );
		}

		public virtual void ApplyThirst( float _value ){
			Creature.Status.AddThirst( _value );
		}

		public virtual void ApplyDebility( float _value ){
			Creature.Status.AddDebility( _value );
		}

		public virtual void ApplyStress( float _value ){
			Creature.Status.AddStress( _value );
		}

		public virtual void ApplyAggressivity( float _value ){
			Creature.Status.AddAggressivity( _value );
		}

		public virtual void ApplyAnxiety( float _value ){
			Creature.Status.AddAnxiety( _value );
		}

		public virtual void ApplyExperience( float _value ){
			Creature.Status.AddExperience( _value );
		}

		public virtual void ApplyNosiness( float _value ){
			Creature.Status.AddNosiness( _value );
		}

		public override void ApplyImpact( Vector3 _direction, float _force ){

			if( Creature.Essentials.BehaviourModeImpactEnabled )
				Creature.SetActiveBehaviourModeByKey( Creature.Essentials.BehaviourModeImpact );

			base.ApplyImpact( _direction, _force );
		}

		/*
		public override void ApplyDamage( float _damage, Vector3 _damage_direction, Vector3 _attacker_position, Transform _attacker, float _force = 0  )
		{
			InfluenceDataObject _influence = new InfluenceDataObject();
			_influence.Enabled = true;
			_influence.Damage = _damage;
			ApplyInfluence( _influence, _damage_direction, _attacker_position, _attacker, (_force > 0?_force:_damage) );
		}*/

		public virtual void ApplyInfluence( InfluenceDataObject _influence, Vector3 influence_direction, Vector3 _attacker_position, Transform _attacker, float _force = 0  )
		{
			Creature.UpdateStatusInfluences( _influence );

			Rigidbody _body = transform.GetComponent<Rigidbody>();
			if( _body != null )
				_body.AddForce( ( transform.position - _attacker_position ).normalized * _force, ForceMode.Impulse);
		}
			
		public void TriggerBehaviourRuleAudio()
		{
			if( Creature.Behaviour.ActiveBehaviourModeRule != null )
				Creature.Behaviour.BehaviourAudioPlayer.Play( Creature.Behaviour.ActiveBehaviourModeRule.Audio );
		}

		public void TriggerBehaviourRuleEvent( int _value )
		{
			if( Creature.Behaviour.ActiveBehaviourModeRule != null )
				Creature.Behaviour.ActiveBehaviourModeRule.Events.TriggerAction( this, null, _value );
		}

		public void TriggerTargetEvent( int _value )
		{
			if( Creature.ActiveTarget != null && Creature.ActiveTarget.IsValidAndReady )
				Creature.ActiveTarget.Events.TriggerAction( this, Creature.ActiveTarget.TargetGameObject, _value );
		}

		#endregion

		#region Creature Sense

		public virtual void Sense()
		{
			if( ! Creature.IsPerceptionTime() )
				return;

			DoSense();
		}

			
		IEnumerator SenseCoroutine()
		{
			while( RuntimeBehaviour.UseCoroutine )
			{
				// coroutine is alive ... 
				m_SenseCoroutineIsRunning = true;

				DoSense();

				yield return new WaitWhile( () => ! Creature.IsPerceptionTime() );
			}		

			m_SenseCoroutineIsRunning = false;
			yield break;
		}

		protected virtual void DoSense()
		{
			Creature.AvailableTargets.Clear();

			// PREDECISIONS HOME
			Creature.AddAvailableTarget( Creature.Essentials.PrepareTarget( this ) );

			// PREDECISIONS MISSION OUTPOST
			Creature.AddAvailableTarget( Creature.Missions.Outpost.PrepareTarget( this ) );

			// PREDECISIONS MISSION ESCORT
			Creature.AddAvailableTarget( Creature.Missions.Escort.PrepareTarget( this, Creature ) );

			// PREDECISIONS MISSION PATROL
			Creature.AddAvailableTarget( Creature.Missions.Patrol.PrepareTarget( this, Creature ) );

			// PREDECISIONS INTERACTORS
			foreach( InteractorObject _interactor in Creature.Interaction.Interactors )
			{
				_interactor.PrepareTargets( this );
				foreach( TargetObject _target in _interactor.PreparedTargets )
					Creature.AddAvailableTarget( _target );
			}

			// PREDECISIONS CUSTOM MISSIONS
			foreach( ICECreatureMission _mission in Missions ){
				Creature.AddAvailableTarget( _mission.PrepareTarget() );
			}

			Creature.SelectActiveTarget();

		
			OnSenseComplete();
		}

		#endregion

		#region Creature React

		public virtual void React()
		{
			if( ! Creature.IsReactionTime() )
				return;

			DoReact();
		}

		IEnumerator ReactCoroutine()
		{
			while( RuntimeBehaviour.UseCoroutine )
			{
				// coroutine is alive ... 
				m_ReactCoroutineIsRunning = true;

				DoReact();


				//Debug.Log( Time.fixedUnscaledTime - _time );
				//_time = Time.fixedUnscaledTime;

				//Debug.Log( Creature.IsReactionTime() );

				yield return new WaitWhile( () => ! Creature.IsReactionTime() );
				//Debug.Log( "NEXT" );
			}		

			m_ReactCoroutineIsRunning = false;
			yield break;
		}

		protected virtual void DoReact()
		{
			Creature.Move.Update( Creature.Behaviour.ActiveBehaviourModeRule );
		}

		#endregion


			
	}
}
