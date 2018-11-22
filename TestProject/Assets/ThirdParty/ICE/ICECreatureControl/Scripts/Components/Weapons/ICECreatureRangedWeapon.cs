// ##############################################################################
//
// ICECreatureRangedWeapon.cs
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

namespace ICE.Creatures{

		/// <summary>
	/// ICE creature ranged weapon.
	/// </summary>
	public class ICECreatureRangedWeapon : ICECreatureWeapon {

		/// <summary>
		/// Gets the entity classification type of the object
		/// </summary>
		/// <value>The classification type of the entity.</value>
		/// <description>The EntityClassType will be used to quickly 
		/// identify the correct class type of a derived entity object 
		/// without casts
		/// </description>
		public override EntityClassType EntityType{
			get{ return EntityClassType.RangedWeapon; }
		}
			
		/// <summary>
		/// OnRegisterBehaviourEvents is called whithin the GetBehaviourEvents() method to update the 
		/// m_BehaviourEvents list. Override this event to register your own events by using the 
		/// RegisterBehaviourEvent method, while doing so you can use base.OnRegisterBehaviourEvents(); 
		/// to call the event in the base classes too.
		/// </summary>
		protected override void OnRegisterBehaviourEvents()
		{
			base.OnRegisterBehaviourEvents();
			RegisterBehaviourEvent("RangedWeaponAutomatic", BehaviourEventParameterType.Boolean, "Enable/disable Automatic", "" );
			RegisterBehaviourEvent("RangedWeaponFire");
			RegisterBehaviourEvent("RangedWeaponFireExtented", BehaviourEventParameterType.Boolean, "Reset Interval", "" );
			RegisterBehaviourEvent("RangedWeaponFireBurst", BehaviourEventParameterType.Integer, "Interval Limit", "" );
			RegisterBehaviourEvent("RangedWeaponStop");
			RegisterBehaviourEvent("RangedWeaponFireOneShot");
			RegisterBehaviourEvent("RangedWeaponDamage", BehaviourEventParameterType.Float );

			RegisterBehaviourEvent("RangedWeaponSecondaryAutomatic", BehaviourEventParameterType.Boolean, "Enable/disable Automatic", "" );
			RegisterBehaviourEvent("RangedWeaponSecondaryFire");
			RegisterBehaviourEvent("RangedWeaponSecondaryFireExtented", BehaviourEventParameterType.Boolean, "Reset Interval", "" );
			RegisterBehaviourEvent("RangedWeaponSecondaryFireBurst", BehaviourEventParameterType.Integer, "Interval Limit", "" );
			RegisterBehaviourEvent("RangedWeaponSecondaryStop");
			RegisterBehaviourEvent("RangedWeaponSecondaryFireOneShot");


			if( Laser.Enabled )
			{
				RegisterBehaviourEvent("RangedWeaponLaserOn");
				RegisterBehaviourEvent("RangedWeaponLaserOff");
				RegisterBehaviourEvent("RangedWeaponLaserToggle");
			}

			if( Flashlight.Enabled )
			{
				RegisterBehaviourEvent("RangedWeaponFlashlightOn");
				RegisterBehaviourEvent("RangedWeaponFlashlightOff");
				RegisterBehaviourEvent("RangedWeaponFlashlightToggle");
			}
		}

		[SerializeField]
		private RangedWeaponObject m_Weapon = null;
		public RangedWeaponObject Weapon{
			get{ return m_Weapon = ( m_Weapon == null ? new RangedWeaponObject(this) : m_Weapon ); }
		}

		[SerializeField]
		private RangedWeaponObject m_SecondaryWeapon = null;
		public RangedWeaponObject SecondaryWeapon{
			get{ return m_SecondaryWeapon = ( m_SecondaryWeapon == null ? new RangedWeaponObject(this) : m_SecondaryWeapon ); }
		}

		[SerializeField]
		private LaserObject m_Laser = null;
		public LaserObject Laser{
			set{ m_Laser = value; }
			get{ return m_Laser = ( m_Laser == null ? new LaserObject( this ):m_Laser ); }
		}

		[SerializeField]
		private FlashlightObject m_Flashlight = null;
		public FlashlightObject Flashlight{
			set{ m_Flashlight = value; }
			get{ return m_Flashlight = ( m_Flashlight == null ? new FlashlightObject( this ):m_Flashlight ); }
		}
			
		public bool FireOnEnabled = false;

		private bool m_IsShooting = false;
		public bool IsShooting{
			get{ return m_IsShooting; }
		}
			
		private ICECreatureControl m_CreatureControl = null;
		public ICECreatureControl CreatureControl{
			get{ return m_CreatureControl = ( m_CreatureControl == null ? transform.GetComponentInParent<ICECreatureControl>() : m_CreatureControl ); }
		}

		private ICECreaturePlayer m_Player = null;
		public ICECreaturePlayer Player{
			get{ return m_Player = ( m_Player == null ? transform.GetComponentInParent<ICECreaturePlayer>() : m_Player ); }
		}

		public override void Start () {

			base.Start();

			Weapon.Init( this );
			SecondaryWeapon.Init( this );
			Laser.Init( this );
			Flashlight.Init( this );
		}
			
		public override void OnEnable()
		{
			if( FireOnEnabled )
				RangedWeaponFire();
		}

		public override void Update () 
		{
			base.Update();

			m_IsShooting = false;
				
			Laser.RenderLaser();
			Flashlight.Update();
			Weapon.Update();
			SecondaryWeapon.Update();
		}
			
		public virtual Transform GetTargetTransform()
		{
			if( CreatureControl )
			{
				TargetObject _target = CreatureControl.Creature.ActiveTarget;
				if( _target != null && _target.TargetGameObject != null )
				{
					Transform _transform = _target.TargetTransform;

					ICEWorldEntity[] _entities = _target.TargetGameObject.GetComponentsInChildren<ICEWorldEntity>();
					if( _entities != null && _entities.Length > 0 )
					{
						ICEWorldEntity _entity = _entities[ UnityEngine.Random.Range( 0, _entities.Length ) ];
						if( _entity != null )
							_transform = _entity.transform;	
					}
					else
					{
						Collider[] _colliders = _target.TargetGameObject.GetComponentsInChildren<Collider>();
						if( _colliders != null && _colliders.Length > 0  )
						{
							Collider _collider = _colliders[ UnityEngine.Random.Range( 0, _colliders.Length ) ];
							if( _collider != null )
								_transform = _collider.transform;
						}
						/*
						RaycastHit _hit;
						if( Physics.Raycast( transform.position, transform.forward, out _hit, Mathf.Infinity, -1, WorldManager.TriggerInteraction ) )
							return ( _hit.collider != null ? _hit.collider.transform : _hit.transform );
							*/
					}

					//transform.LookAt( _transform, Vector3.up );
					return _transform;
				}
			}
			else if( Player )
			{
				RaycastHit _hit;
				Ray _ray;

				if( Player.Player.UseMousePositionToAim )
					_ray = Camera.main.ScreenPointToRay( Input.mousePosition );
				else
					_ray = Camera.main.ViewportPointToRay( new Vector3( 0.5f, 0.5f, 0 ) );
				

				if( Physics.Raycast( _ray, out _hit ) )
					return ( _hit.collider != null ? _hit.collider.transform : _hit.transform );
			}
			else
			{
				RaycastHit _hit;
				if( Physics.Raycast( transform.position, transform.forward, out _hit, Mathf.Infinity, -1, WorldManager.TriggerInteraction ))
					return ( _hit.collider != null ? _hit.collider.transform : _hit.transform );
			}

			return null;
		}

		// PUBLIC METHODS

		public virtual void RangedWeaponAutomatic( bool _enabled ){
			Weapon.Automatic.Enabled = _enabled;
		}

		/// <summary>
		/// RangedWeapon will fire a serial of shots according to the given interval settings
		/// </summary>
		public virtual void RangedWeaponFire(){
			Weapon.Fire( GetTargetTransform() );
		}

		/// <summary>
		/// RangedWeapon will fire a serial of shots according to the specified reset value and the given interval settings
		/// </summary>
		/// <param name="_reset">If set to <c>true</c> reset.</param>
		public virtual void RangedWeaponFireExtented( bool _reset ){
			Weapon.Fire( GetTargetTransform(), _reset );
		}

		public virtual void RangedWeaponFireBurst( int _shots ){
			Weapon.FireBurst( GetTargetTransform(), _shots );
		}

		/// <summary>
		/// RangedWeapon will stop fireing
		/// </summary>
		public virtual void RangedWeaponStop(){
			Weapon.Stop();
		}

		/// <summary>
		/// RangedWeapon will fire one shot.
		/// </summary>
		public virtual void RangedWeaponFireOneShot(){
			Weapon.FireOneShot( GetTargetTransform() );
		}

		public virtual void RangedWeaponSecondaryAutomatic( bool _enabled ){
			SecondaryWeapon.Automatic.Enabled = _enabled;
		}

		/// <summary>
		/// SecondaryRangedWeapon will fire a serial of shots according to the given interval settings
		/// </summary>
		public virtual void RangedWeaponSecondaryFire(){
			SecondaryWeapon.Fire( GetTargetTransform() );
		}

		/// <summary>
		/// SecondaryRangedWeapon will fire a serial of shots according to the specified reset value and the given interval settings
		/// </summary>
		/// <param name="_reset">If set to <c>true</c> reset.</param>
		public virtual void RangedWeaponSecondaryFireExtented( bool _reset ){
			Weapon.Fire( GetTargetTransform(), _reset );
		}

		public virtual void RangedWeaponSecondaryFireBurst( int _shots ){
			SecondaryWeapon.FireBurst( GetTargetTransform(), _shots );
		}

		/// <summary>
		/// SecondaryRangedWeapon will stop fireing
		/// </summary>
		public virtual void RangedWeaponSecondaryStop(){
			SecondaryWeapon.Stop();
		}

		/// <summary>
		/// SecondaryRangedWeapon will fire one shot.
		/// </summary>
		public virtual void RangedWeaponSecondaryFireOneShot(){
			SecondaryWeapon.FireOneShot( GetTargetTransform() );
		}

		/// <summary>
		/// Switched the lase of the RangedWeapon on.
		/// </summary>
		public virtual void RangedWeaponLaserOn(){
			Laser.IsActive = true;
		}

		/// <summary>
		/// Switched the lase of the RangedWeapon off.
		/// </summary>
		public virtual void RangedRangedWeaponLaserOff(){
			Laser.IsActive = false;
		}

		/// <summary>
		/// Rangeds the weapon laser toggle.
		/// </summary>
		public virtual void RangedWeaponLaserToggle(){
			Laser.IsActive = ! Laser.IsActive;
		}

		/// <summary>
		/// Switched the flashlight of the RangedWeapon on.
		/// </summary>
		public virtual void RangedWeaponFlashlightOn(){
			Flashlight.IsActive = true;
		}

		/// <summary>
		/// Switched the flashlight of the RangedWeapon off.
		/// </summary>
		public virtual void RangedWeaponFlashlightOff(){
			Flashlight.IsActive = false;
		}

		/// <summary>
		/// Toggles the flashlight of the RangedWeapon on/off.
		/// </summary>
		public virtual void RangedWeaponFlashlightToggle(){
			Flashlight.IsActive = ! Flashlight.IsActive;
		}

		/// <summary>
		/// Applies damage to the RangedWeapon.
		/// </summary>
		/// <param name="_damage">Damage.</param>
		public virtual void RangedWeaponApplyDamage( float _damage ){
			ApplyDamage( _damage );
		}

	}
}