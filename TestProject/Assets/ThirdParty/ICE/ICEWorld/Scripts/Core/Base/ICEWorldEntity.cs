// ##############################################################################
//
// ICE.World.ICEEntity.cs
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

namespace ICE.World
{

	/// <summary>
	/// Within the ICE World an entity is something that exists as itself, as a subject or as an object, actually 
	/// or potentially, concretely or abstractly, physically or not. On this note an ICE World Entity represents 
	/// an interactive GameObject within your scenes which could affect the gameplay in someways. ICEWorldEntity 
	/// is the lowermost base class for all interactive components within the ICE World (e.g. ICECreatureControl 
	/// or ICEPlayer, but also items, vehicles, construction elements etc.). 
	/// 
	/// ICEWorldEntity is derived from ICEWorldBehaviour and declared as abstract.
	/// 
	/// Use ICEWorldEntity or one of its child classes as base class for your own interaction components, such as your 
	/// own FPSController or your custom AI script, so your code will be automatically downwards compatible with the 
	/// rest of the ICE world.			
	/// </summary>
	public abstract class ICEWorldEntity : ICEWorldObject {

		public virtual EntityClassType EntityType{
			get{ return EntityClassType.Undefined; }
		}

		public bool IsLocal = true;

		public virtual bool CompareType( EntityClassType _type ){
			return ( EntityType == _type ? true : false );
		}

		[SerializeField]
		private EntityStatusObject m_Status = null;
		public virtual EntityStatusObject Status{
			get{ return m_Status = ( m_Status == null ? new EntityStatusObject(this):m_Status ); }
			set{ Status.Copy( value ); }
		}

		[SerializeField]
		protected EntityRuntimeBehaviourObject m_RuntimeOptions = null;
		public virtual EntityRuntimeBehaviourObject RuntimeBehaviour{
			get{ return m_RuntimeOptions = ( m_RuntimeOptions == null ? new EntityRuntimeBehaviourObject(this):m_RuntimeOptions ); }
			set{ RuntimeBehaviour.Copy( value ); }
		}

		public delegate void OnAddDamageEvent( float _damage, Vector3 _damage_direction, Vector3 _damage_position, Transform _attacker, float _force = 0  );
		public event OnAddDamageEvent OnAddDamage;

		protected bool m_Pause = false;
		public virtual bool Pause{
			get{ return m_Pause; }
			set{ m_Pause = value; }
		}

		/// <summary>
		/// The base offset can be used to adapt the correct ground level of 
		/// the object.
		/// </summary>
		public float BaseOffset = 0;
		public float BaseOffsetMaximum = 1;

		/// <summary>
		/// Gets the age or 0 if aging is not active
		/// </summary>
		/// <value>The age.</value>
		public virtual float Age{
			get{ return Status.Age; }
		}

		/// <summary>
		/// Gets a value indicating whether this entity is destroyed.
		/// </summary>
		/// <value><c>true</c> if this entity is destructible and destroyed; otherwise, <c>false</c>.</value>
		public virtual bool IsDestroyed{
			get{ return Status.IsDestroyed; }
		}

		/// <summary>
		/// Gets the durability.
		/// </summary>
		/// <value>The durability.</value>
		public virtual float Durability{
			get{ return Status.Durability; }
		}

		/// <summary>
		/// Gets the durability in percent.
		/// </summary>
		/// <value>The durability in percent.</value>
		public virtual float DurabilityInPercent{
			get{ return Status.DurabilityInPercent; }
		}
			
		/// <summary>
		/// OnRegisterPublicMethods is called within the GetPublicMethods() method to update the 
		/// m_PublicMethods list. Override this event to register your own methods by using the 
		/// RegisterPublicMethod(); while doing so you can use base.OnRegisterPublicMethods(); 
		/// to call the event in the base classes too.
		/// </summary>
		protected override void OnRegisterBehaviourEvents()
		{
			//base.OnRegisterBehaviourEvents(); 
			RegisterBehaviourEvent( "ApplyDamage", BehaviourEventParameterType.Float );
			RegisterBehaviourEvent( "Destroy" );
			RegisterBehaviourEvent( "Reset" );
		}
			
		private ICEWorldSingleton m_World = null;
		/// <summary>
		/// Gets the ICEWorld.
		/// </summary>
		/// <value>The current ICEWorld</value>
		public ICEWorldSingleton World{
			get{ return m_World = ( m_World == null?ICEWorldSingleton.Instance:m_World ); }
		}
			
		private ICEWorldEnvironment m_Environment = null;
		/// <summary>
		/// Gets the ICEWorldEnvironment.
		/// </summary>
		/// <value>The current ICEWorldEnvironment or null</value>
		public ICEWorldEnvironment Environment{
			get{ return m_Environment = ( m_Environment == null?ICEWorldEnvironment.Instance:m_Environment ); }
		}

		private ICEWorldRegister m_Registry = null;
		/// <summary>
		/// Gets the ICEWorldRegistry.
		/// </summary>
		/// <value>The current ICEWorldRegistry or null</value>
		public ICEWorldRegister Registry{
			get{ return m_Registry = ( m_Registry == null?ICEWorldRegister.Instance:m_Registry ); }
		}

		/// <summary>
		/// Gets the highest available root entity.
		/// </summary>
		/// <value>The parent entity.</value>
		public ICEWorldEntity RootEntity { 
			get{ return GetRootEntity(); }
		}

		public Transform RootEntityTransform { 
			get{ return RootEntity.gameObject.transform; }
		}

		/// <summary>
		/// Determines whether the specified transform is part of the root entity.
		/// </summary>
		/// <returns><c>true</c> if the specified transform is part of the root entity; otherwise, <c>false</c>.</returns>
		/// <param name="_object">Object.</param>
		public bool IsPartOfRoot( Transform _transform )
		{
			if( _transform == null )
				return false;
			
			return _transform.IsChildOf( RootEntityTransform );
		}

		public bool ContainsChild( ICEWorldEntity _object )
		{
			if( _object == null )
				return false;			
			
			ICEWorldEntity[] _entities = transform.GetComponentsInChildren<ICEWorldEntity>();
			foreach( ICEWorldEntity _entity in _entities )
			{
				if( _entity == _object )
					return true;
			}

			return false;
		}

		public bool UseHierarchyManagement { 
			get{ return ( RuntimeBehaviour.UseHierarchyManagement ? IsRootEntity : false ); } // as long as an entity is a child it shall use the HierarchyManagenent
		}

		public bool IsRootEntity { 
			get{ return ( RootEntity == this ? true : false ); }
		}

		public bool IsChildEntity { 
			get{ return ! IsRootEntity; }
		}

		/// <summary>
		/// Gets all child entities.
		/// </summary>
		/// <returns>The child entities.</returns>
		public ICEWorldEntity[] GetChildEntities(){
			return transform.GetComponentsInChildren<ICEWorldEntity>();
		}

		/// <summary>
		/// Gets all parent entities.
		/// </summary>
		/// <returns>The parent entities.</returns>
		public ICEWorldEntity[] GetParentEntities(){
			return transform.GetComponentsInParent<ICEWorldEntity>();
		}

		/// <summary>
		/// Determines whether this instance has parent entities.
		/// </summary>
		/// <returns><c>true</c> if this instance has parent entities; otherwise, <c>false</c>.</returns>
		public bool HasParentEntities(){
			
			ICEWorldEntity[] _parents = GetParentEntities();
			if( _parents == null || ( _parents.Length == 1 && _parents[0] == this ) )
				return false;
			else
				return true;
		}

		/// <summary>
		/// Gets the root entity or null in cases that this entity is the root entity
		/// </summary>
		/// <returns>The parent entity.</returns>
		public ICEWorldEntity GetRootEntity()
		{
			ICEWorldEntity _root = null;

			// if transform.root and transform are identic the entity is its own root
			if( transform.root == transform )
				_root = this;

			// if transform.root and transform.parent are identic and the transform.parent 
			// is an entity, the parent is the root, otherwise this entity is its own root
			else if( transform.root == transform.parent )
				_root = transform.parent.GetComponent<ICEWorldEntity>();

			// if both prior checks fails and _root stills empty we try to find the root entity within the hierarchy
			if( _root == null )
			{
				ICEWorldEntity[] _entities = GetParentEntities();
				if( _entities != null && _entities.Length > 1 )
				{
					foreach( ICEWorldEntity _entity in _entities )
					{
						if( _entity == this )
							continue;

						if( _entity.transform.root == _entity.transform || ! _entity.HasParentEntities() )
							return _entity;
					}
				}
			}

			// if m_RootEntity stills empty it seems that there are no higher entities within the hierarchy, so we assume that this 
			// entity is the root entity so we return this.
			if( _root == null )
				_root = this;

			return _root;
		}

		public override void Awake () {
			base.Awake();
		}

		public override void Start () {
			base.Start();

			Status.Init( this );
			RuntimeBehaviour.Init(this);
		}

		public override void OnEnable () {
			base.OnEnable();
			Status.Init( this );
			RuntimeBehaviour.Init(this);
		}

		public override void OnDisable () {
			base.OnDisable();

			Status.Reset();
			RuntimeBehaviour.Reset();
		}

		public override void OnDestroy() {
			base.OnDestroy();
		}

		public override void Update(){

			UpdateImpactForce();

			Status.Update();
			DoUpdate();
		}

		public override void LateUpdate () {
			DoLateUpdate();
		}

		/// <summary>
		/// Register this instance.
		/// </summary>
		protected virtual void Register(){
			WorldManager.Register( transform.gameObject );
		}

		/// <summary>
		/// Deregister this instance.
		/// </summary>
		protected virtual void Deregister(){
			WorldManager.Deregister( transform.gameObject );
		}

		/// <summary>
		/// Removes this instance according to the defined reference group settings of the 
		/// WorldManager. In cases UseSoftRespawn is active the target will be dactivate, 
		/// stored and prepared for its next action, otherwise the object will be destroyed.
		/// </summary>
		protected virtual void Remove(){
			WorldManager.Remove( transform.gameObject );
		}

		public virtual void Reset()
		{
			Status.Reset();
		}

		public virtual void Destroy(){
			WorldManager.Remove( transform.gameObject );
		}

		/// <summary>
		/// Applies the specified damage.
		/// </summary>
		/// <param name="_damage">Damage.</param>
		public virtual void ApplyDamage( float _damage ){
			AddDamage( _damage, Vector3.zero, Vector3.zero, null, 0 );
		}

		/// <summary>
		/// Adds or forwards the received damage.
		/// </summary>
		/// <param name="_damage">Damage.</param>
		/// <param name="_damage_direction">Damage direction.</param>
		/// <param name="_attacker_position">Attacker position.</param>
		/// <param name="_attacker">Attacker.</param>
		/// <param name="_force">Force.</param>
		public virtual void AddDamage( float _damage, Vector3 _damage_direction, Vector3 _damage_position, Transform _attacker, float _force = 0  )
		{
			// use RootEntity instead of m_RootEntity to make sure that the root will be up-to-date
			if( IsChildEntity && Status.UseDamageTransfer ) 
				RootEntity.AddDamage( _damage * Status.DamageTransferMultiplier, _damage_direction, _damage_position, _attacker, _force );
			else
			{				
				Status.AddDamage( _damage * Status.DamageTransferMultiplier );	

				if( OnAddDamage != null )
					OnAddDamage( _damage * Status.DamageTransferMultiplier, _damage_direction, _damage_position, _attacker, _force );
			}
	
			//ApplyImpact( _damage_direction, _force );
		}

		private Vector3 m_ImpactForce = Vector3.zero;
		public Vector3 ImpactForce{
			get{ return m_ImpactForce; }
		}

		/// <summary>
		/// Gets the mass of the entity.
		/// </summary>
		/// <value>The mass.</value>
		public virtual float Mass{
			get{ return Status.Mass; }
		}

		/// <summary>
		/// Gets a value indicating whether this entity is currently effected by external forces.
		/// </summary>
		/// <value><c>true</c> if this instance has impact; otherwise, <c>false</c>.</value>
		public bool HasImpact{
			get{ return ( Status.Enabled && m_ImpactForce.magnitude > 0.2F ? true : false ); }
		}

		/// <summary>
		/// Applies the impact.
		/// </summary>
		/// <param name="_direction">Direction.</param>
		/// <param name="_force">Force.</param>
		public virtual void ApplyImpact( Vector3 _direction, float _force ){
			ApplyImpact( _direction, _force, Vector3.zero );
		}

		/// <summary>
		/// Applies the impact.
		/// </summary>
		/// <param name="_direction">Direction.</param>
		/// <param name="_force">Force.</param>
		/// <param name="_point">Point.</param>
		public virtual void ApplyImpact( Vector3 _direction, float _force, Vector3 _point ){

			if( IsChildEntity )
				RootEntity.ApplyImpact( _direction, _force );
			else if( ObjectRigidbody != null && ! ObjectRigidbody.isKinematic )
			{
				if( _point == Vector3.zero )
					ObjectRigidbody.AddForce( _direction.normalized * _force, ForceMode.Impulse );
				else
					ObjectRigidbody.AddForceAtPosition( _direction.normalized * _force, _point , ForceMode.Impulse );
			}
			else 
			{
				_direction.Normalize();
				if( _direction.y < 0) 
					_direction.y = -_direction.y; // reflect down force on the ground
				m_ImpactForce += _direction.normalized * _force / Mass;
			}
		}

		/// <summary>
		/// Updates the impact based movements and consumes the impact energy each cycle:
		/// </summary>
		protected virtual void UpdateImpactForce(){

			// apply the impact force:
			if( HasImpact )
			{
				transform.position += m_ImpactForce * Time.deltaTime;
			}

			// consumes the impact energy each cycle:
			m_ImpactForce = Vector3.Lerp( m_ImpactForce, Vector3.zero, Mass * Time.deltaTime );

		}

		public virtual void OnCollisionEnter(Collision _collision) {}
		public virtual void OnCollisionStay(Collision _collision) {}
		public virtual void OnCollisionExit( Collision _collision )  {}
		public virtual void OnTriggerEnter( Collider _collider ) {}
		public virtual void OnTriggerStay( Collider _collider ) {}
		public virtual void OnTriggerExit( Collider _collider ) {}
		public virtual void OnControllerColliderHit( ControllerColliderHit hit ) {}


		/// <summary>
		/// Gets the ICEWorldEntity of the specified object.
		/// </summary>
		/// <returns>The ICEWorldEntity.</returns>
		/// <param name="_object">Object.</param>
		public static ICEWorldEntity GetWorldEntity( GameObject _object )
		{
			if( _object == null )
				return null;

			// First we try to get an entity from the specified object ...
			ICEWorldEntity _entity = _object.GetComponent<ICEWorldEntity>();

			// ... if this failed we try to get one from its parents ...
			if( _entity == null )
				_entity = _object.GetComponentInParent<ICEWorldEntity>();

			// ... and finaly we try to get one from its children ...
			if( _entity == null )
				_entity = _object.GetComponentInChildren<ICEWorldEntity>();

			return _entity;
		}

		/// <summary>
		/// SendDamage handles damage and impact forces for the specified target object. You can use this static method to 
		/// affect each entity object within your scene. Please note that _target can be adjusted to null, in 
		/// such a case the _force_type will be automatically changed to DamageForceType.Explosion and the origin 
		/// of the detonation will be the _sender.transform.position.
		/// </summary>
		/// <param name="_sender">Sender.</param>
		/// <param name="_target">Target.</param>
		/// <param name="_impact_type">Impact type.</param>
		/// <param name="_damage">Damage.</param>
		/// <param name="_damage_method">Damage method.</param>
		/// <param name="_force_type">Force type.</param>
		/// <param name="_force">Force.</param>
		/// <param name="_radius">Radius.</param>
		public static void SendDamage( GameObject _sender, GameObject _target, DamageTransferType _impact_type, float _damage, string _damage_method, DamageForceType _force_type, float _force, float _radius ){

			Vector3 _hit_point = ( _target != null ? _target.transform.position : _sender.transform.position );
			RaycastHit _hit;
			if( Physics.Raycast( _sender.transform.position, _sender.transform.forward, out _hit, Mathf.Infinity, -1, WorldManager.TriggerInteraction ) )
				_hit_point = _hit.point;

			SendDamage( _sender, _target, _impact_type, _damage, _damage_method, _hit_point, _force_type, _force, _radius );
		}

		/// <summary>
		/// SendDamage handles damage and impact forces for the specified target object. You can use this static method to 
		/// affect each entity object within your scene. Please note that _target can be adjusted to null, in 
		/// such a case the _force_type will be automatically changed to DamageForceType.Explosion and the origin 
		/// of the detonation will be the _sender.transform.position.
		/// </summary>
		/// <param name="_sender">Sender.</param>
		/// <param name="_target">Target.</param>
		/// <param name="_impact_type">Impact type.</param>
		/// <param name="_damage">Damage.</param>
		/// <param name="_damage_method">Damage method.</param>
		/// <param name="_damage_point">Damage point.</param>
		/// <param name="_force_type">Force type.</param>
		/// <param name="_force">Force.</param>
		/// <param name="_radius">Radius.</param>
		public static void SendDamage( GameObject _sender, GameObject _target, DamageTransferType _impact_type, float _damage, string _damage_method, Vector3 _damage_point, DamageForceType _force_type, float _force, float _radius )
		{
			if( _sender == null )
				return;

			if( _target == null )
				_force_type = DamageForceType.Explosion;

			// If the force type is an explosion will will handle first the explosion impact to all objects around the specified target 
			// in cases the target will be NULL (e.g. remote or timed detonation of an explosive etc.) the sender will be the origin of 
			// the explosion.
			if( _force_type == DamageForceType.Explosion )
			{
				_damage_point = ( _damage_point == Vector3.zero ? ( _target != null ? _target.transform.position : _sender.transform.position ) : _damage_point );

				Collider[] _colliders = Physics.OverlapSphere( _damage_point, _radius );
				if( _colliders != null )
				{
					foreach( Collider _collider in _colliders )
					{
						if( _collider == null || _collider.gameObject == _target || _collider.gameObject == _sender )
							continue;
						
						float _distance = PositionTools.Distance( _damage_point, _collider.gameObject.transform.position );
						float _multiplier = Mathf.Clamp01( 1 - MathTools.Normalize( _distance, 0, _radius ) );

						// If a explosion radius is given we will try to apply a suitable force to the colliders gamesobject
						if(  _radius > 0 )
						{
							if( _collider.attachedRigidbody != null && ! _collider.attachedRigidbody.isKinematic )
								_collider.attachedRigidbody.AddExplosionForce( _force * _multiplier, _damage_point, _radius );
							else
							{
								ICEWorldEntity _entity = ICEWorldEntity.GetWorldEntity( _collider.gameObject );
								if( _entity != null )
								{
									Vector3 _direction = _collider.transform.position - _damage_point;
									_entity.ApplyImpact( _direction, _force * _multiplier );
								}
							}
						}

						// SendTargetDamage will try now to damage the colliders gameobject according to the given distance and multiplier
						ICEWorldEntity.SendTargetDamage( _sender, _collider.gameObject, _impact_type, _damage * _multiplier, _damage_method, _damage_point, _force_type, _force * _multiplier );
					}
				}
			}


			if( _target != null )
			{
				// whenever a target is specified and the defined force type isn't NONE we try to apply also a force to the target
				if( _force_type != DamageForceType.None )
				{
					Vector3 _direction = _target.transform.position - _sender.transform.position;
					_direction.Normalize();
					// Handle Target Rigidbody and forces
					Rigidbody _target_rigidbody = _target.GetComponent<Rigidbody>();
					if( _target_rigidbody != null  && ! _target_rigidbody.isKinematic )
						_target_rigidbody.AddForce( _direction.normalized * _force, ForceMode.Force );
					else
					{
						ICEWorldEntity _entity = ICEWorldEntity.GetWorldEntity( _target );
						if( _entity != null )
							_entity.ApplyImpact( _direction, _force );
					}
				}

				// Finally we try to damage the specified target
				ICEWorldEntity.SendTargetDamage( _sender, _target, _impact_type, _damage, _damage_method, _damage_point, _force_type, _force );
			}
		}

		/// <summary>
		/// Sends the specified damage to an external target object.
		/// </summary>
		/// <param name="_target">Target.</param>
		/// <param name="_attacker">Attacker.</param>
		/// <param name="_impact_type">Impact type.</param>
		/// <param name="_damage">Damage.</param>
		/// <param name="_damage_method">Damage method.</param>
		/// <param name="_force">Force.</param>
		private static void SendTargetDamage( GameObject _sender, GameObject _target, DamageTransferType _impact_type, float _damage, string _damage_method, Vector3 _damage_point, DamageForceType _force_type, float _force )
		{
			if( _target == null || _sender == null || _target == _sender )
				return;

			bool _handled = false;

			if( _impact_type == DamageTransferType.Direct || _impact_type == DamageTransferType.DirectOrMessage || _impact_type == DamageTransferType.DirectAndMessage )
			{
				_handled = EntityDamageConverter.HandleDamage( _sender, _target, _impact_type, _damage, _damage_method, _damage_point, _force_type, _force );

				if( _handled == false )
				{
					ICEWorldEntity _entity = ICEWorldEntity.GetWorldEntity( _target );
					if( _entity != null )
					{
						Vector3 _position = ( _damage_point == Vector3.zero ? _entity.transform.position : _damage_point ); 
						Vector3 _direction = _sender.transform.position - _target.transform.position;

						_entity.AddDamage( _damage, _direction, _position, _sender.transform, _force );
						_handled = true;
					}
				}
			}

			if( _impact_type == DamageTransferType.Message || _impact_type == DamageTransferType.DirectAndMessage || ( _impact_type == DamageTransferType.DirectOrMessage && _handled == false ) )
				_target.SendMessageUpwards( _damage_method, _damage, SendMessageOptions.DontRequireReceiver );
		}
	}
		
	public delegate bool HandleDamageEvent( GameObject _sender, GameObject _target, DamageTransferType _impact_type, float _damage, string _damage_method, Vector3 _damage_point, DamageForceType _force_type, float _force );

	public class EntityDamageConverter{

		public static HandleDamageEvent HandleDamage = DoHandleDamage;
		public static bool DoHandleDamage( GameObject _sender, GameObject _target, DamageTransferType _impact_type, float _damage, string _damage_method, Vector3 _damage_point, DamageForceType _force_type, float _force ){
			return false;
		}
	}
}
