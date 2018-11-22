// ##############################################################################
//
// ice_CreatureWeapon.cs
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

namespace ICE.Creatures.Objects
{
	[System.Serializable]
	public class RangedWeaponShellObject : ICEOwnerObject 
	{
		public RangedWeaponShellObject(){}
		public RangedWeaponShellObject( ICEWorldBehaviour _component ) : base( _component ){ Init( _component ); }
		public RangedWeaponShellObject( RangedWeaponShellObject _object ){ Copy( _object ); }

		public GameObject SpawnPoint = null;
		public GameObject ReferenceShell = null;
		public float EjectionSpeed = 100;
		public float EjectionSpeedMaximum = 1000;


		public override void Init( ICEWorldBehaviour _component ){

			base.Init( _component );

		}

		public void Copy( RangedWeaponShellObject _object )
		{
			base.Copy( _object );

		}

		public void Start()
		{
			if( ReferenceShell == null || SpawnPoint == null )
				return;

			GameObject _shell = CreatureRegister.Spawn( ReferenceShell, SpawnPoint.transform.position, SpawnPoint.transform.rotation );

			Rigidbody _shell_rigidbody = _shell.GetComponent<Rigidbody>();

			if( _shell_rigidbody != null )
			{
				_shell_rigidbody.AddForce( SpawnPoint.transform.forward * EjectionSpeed ); 
			}
		}

		public void Update()
		{
		}
	}

	[System.Serializable]
	public class RangedWeaponRecoilObject : ICEOwnerObject 
	{
		public RangedWeaponRecoilObject(){}
		public RangedWeaponRecoilObject( ICEWorldBehaviour _component ) : base( _component ){ Init( _component ); }
		public RangedWeaponRecoilObject( RangedWeaponRecoilObject _object ){ Copy( _object ); }

		public GameObject RecoilObject = null;

		public override void Init( ICEWorldBehaviour _component ){

			base.Init( _component );

		}

		public void Copy( RangedWeaponRecoilObject _object )
		{
			base.Copy( _object );

		}

		public void Start()
		{
		}

		public void Update()
		{
		}
	}


	[System.Serializable]
	public class RangedWeaponMuzzleFlashObject : ICEOwnerObject 
	{
		public RangedWeaponMuzzleFlashObject(){}
		public RangedWeaponMuzzleFlashObject( ICEWorldBehaviour _component ) : base( _component ){ Init( _component ); }
		public RangedWeaponMuzzleFlashObject( RangedWeaponMuzzleFlashObject _object ){ Copy( _object ); }

		public override void Init( ICEWorldBehaviour _component ){

			base.Init( _component );

		}

		public void Copy( RangedWeaponMuzzleFlashObject _object )
		{
			base.Copy( _object );

			MuzzleFlash = _object.MuzzleFlash;
		}


		public GameObject MuzzleFlash = null;

		public float MuzzleFlashScaleMaximum = 10;
		[SerializeField]
		private float m_MuzzleFlashScale = 1;
		public float MuzzleFlashScale{
			get{ return m_MuzzleFlashScale; }
			set{ 
				m_MuzzleFlashScale = value; 

				if( MuzzleFlash != null )
					MuzzleFlash.transform.localScale = new Vector3( m_MuzzleFlashScale, m_MuzzleFlashScale, m_MuzzleFlashScale );
			}
		}


		protected float m_FlashStartTime = 0;

		public void Start()
		{
			m_FlashStartTime = Time.time;

			if( MuzzleFlash != null )
				MuzzleFlash.SetActive( true );
		}

		public void Update()
		{
			if( MuzzleFlash != null && MuzzleFlash.activeInHierarchy && Time.time - m_FlashStartTime > 0.02f )
				MuzzleFlash.SetActive( false );
		}
	}

	[System.Serializable]
	public class RangedWeaponAmmunitionDataObject : ICEOwnerObject 
	{
		public RangedWeaponAmmunitionDataObject(){}
		public RangedWeaponAmmunitionDataObject( ICEWorldBehaviour _component ) : base( _component ){ Init( _component ); }
		public RangedWeaponAmmunitionDataObject( RangedWeaponAmmunitionDataObject _object ){ Copy( _object ); }

		public override void Init( ICEWorldBehaviour _component ){

			base.Init( _component );

		}

		public void Copy( RangedWeaponAmmunitionDataObject _object )
		{
			base.Copy( _object );
		}

		public RangedWeaponAmmunitionType Type = RangedWeaponAmmunitionType.Simulated;

		public DamageTransferType ImpactType = DamageTransferType.Direct;
		public string MethodName = "ApplyDamage";
		public float MethodDamage = 10;
		public float MethodDamageMaximum = 100;
		public DamageForceType ForceType = DamageForceType.None;
		public float ForceMin = 1000;
		public float ForceMax = 1000;
		public float ForceMaximum = 5000;
		public float ExplosionRadius = 5;
		public float ExplosionRadiusMaximum = 5;

		public GameObject Projectile = null;
		public float ProjectileScale = 1.0f;
		public float ProjectileMuzzleVelocity = 1000;
		public float ProjectileMuzzleVelocityMaximum = 1000;
		public GameObject ProjectileSpawnPoint = null;

	}

	[System.Serializable]
	public class RangedWeaponAmmunitionObject : RangedWeaponAmmunitionDataObject 
	{
		public RangedWeaponAmmunitionObject(){}
		public RangedWeaponAmmunitionObject( ICEWorldBehaviour _component ) : base( _component ){}
		public RangedWeaponAmmunitionObject( RangedWeaponAmmunitionObject _object ) : base( _object ){}

		public void Fire( Transform _target )
		{
			if( _target != null && DebugLogIsEnabled ) 
				PrintDebugLog( this, "Fire - " + _target.name + " (" + _target.position + ")" );

			if( Type == RangedWeaponAmmunitionType.Simulated && _target != null )
			{
				ICEWorldEntity.SendDamage( Owner, _target.gameObject, ImpactType, MethodDamage, MethodName, ForceType, UnityEngine.Random.Range( ForceMin, ForceMax ), ExplosionRadius );
			}
			else if( Type == RangedWeaponAmmunitionType.Projectile || Type == RangedWeaponAmmunitionType.BallisticProjectile )
			{
				Vector3 _position = Vector3.zero;
				Quaternion _rotation = Quaternion.identity;

					
				if( Type == RangedWeaponAmmunitionType.Projectile )
				{
					if( _target != null )
					{
						_position = _target.position;
					}

					if( ProjectileSpawnPoint != null )
					{
						Debug.DrawRay( ProjectileSpawnPoint.transform.position, ProjectileSpawnPoint.transform.forward * 100 );
						RaycastHit _hit;
						if( Physics.Raycast( ProjectileSpawnPoint.transform.position, ProjectileSpawnPoint.transform.forward, out _hit, Mathf.Infinity, -1, WorldManager.TriggerInteraction ) )
						{
							_position = _hit.point;
						}
					}

					GameObject _projectile = WorldManager.Instantiate( Projectile, _position, _rotation );
					if( _projectile != null )
					{
						_projectile.name = Projectile.name;
						_projectile.transform.localScale = new Vector3( ProjectileScale, ProjectileScale, ProjectileScale );

						SystemTools.EnableColliders( _projectile.transform, false );

						Rigidbody _rb = _projectile.GetComponent<Rigidbody>();
						if( _rb != null )
						{
							_rb.useGravity = false;
							_rb.isKinematic = true;
							_rb.constraints = RigidbodyConstraints.FreezeAll;
						}	

						_projectile.transform.SetParent( _target, true );

						ICECreatureProjectile _p = _projectile.GetComponent<ICECreatureProjectile>();
						if( _p != null )
						{
							_p.Hit( _target.gameObject, _position );
						}
					}
				}
				else if( Type == RangedWeaponAmmunitionType.BallisticProjectile )
				{
					if( ProjectileSpawnPoint != null )
					{
						_position = ProjectileSpawnPoint.transform.position;
						_rotation = ProjectileSpawnPoint.transform.rotation;
					}
					else
					{
						_position = Owner.transform.position;
						_rotation = Owner.transform.rotation;
					}

					GameObject _projectile = WorldManager.Instantiate( Projectile, _position, _rotation );
					if( _projectile != null )
					{
						_projectile.name = Projectile.name;
						_projectile.transform.localScale = new Vector3( ProjectileScale, ProjectileScale, ProjectileScale );

						Rigidbody _rb = _projectile.GetComponent<Rigidbody>();
						if( _rb != null )
						{
							_rb.AddForce( ProjectileSpawnPoint.transform.TransformDirection( new Vector3(0,0, ProjectileMuzzleVelocity ) ) );
						}
					}
				}
			}
		}
	}

	[System.Serializable]
	public class RangedWeaponDataObject : ICEOwnerObject 
	{
		public RangedWeaponDataObject(){}
		public RangedWeaponDataObject( ICEWorldBehaviour _component ) : base( _component ){ Init( _component ); }
		public RangedWeaponDataObject( RangedWeaponDataObject _object ){ Copy( _object ); }

		[SerializeField]
		private ICEDirectTimerObject m_Automatic = null;
		public ICEDirectTimerObject Automatic{
			get{ return m_Automatic = ( m_Automatic == null ? new ICEDirectTimerObject() : m_Automatic ); }
			set{ Automatic.Copy( value ); }
		}

		[SerializeField]
		private DirectAudioPlayerObject m_LaunchSound = null;
		public DirectAudioPlayerObject LaunchSound{
			get{ return m_LaunchSound = ( m_LaunchSound == null ? new DirectAudioPlayerObject( OwnerComponent ) : m_LaunchSound); }
			set{ LaunchSound.Copy( value ); }
		}

		[SerializeField]
		private RangedWeaponAmmunitionObject m_Ammunition = null;
		public RangedWeaponAmmunitionObject Ammunition{
			get{ return m_Ammunition = ( m_Ammunition == null ? new RangedWeaponAmmunitionObject( OwnerComponent ) : m_Ammunition); }
			set{ Ammunition.Copy( value ); }
		}

		[SerializeField]
		private RangedWeaponMuzzleFlashObject m_MuzzleFlash = null;
		public RangedWeaponMuzzleFlashObject MuzzleFlash{
			get{ return m_MuzzleFlash = ( m_MuzzleFlash == null ? new RangedWeaponMuzzleFlashObject( OwnerComponent ) : m_MuzzleFlash); }
			set{ MuzzleFlash.Copy( value ); }
		}

		[SerializeField]
		private RangedWeaponRecoilObject m_Recoil = null;
		public RangedWeaponRecoilObject Recoil{
			get{ return m_Recoil = ( m_Recoil == null ? new RangedWeaponRecoilObject( OwnerComponent ) : m_Recoil); }
			set{ Recoil.Copy( value ); }
		}

		[SerializeField]
		private RangedWeaponShellObject m_Shell = null;
		public RangedWeaponShellObject Shell{
			get{ return m_Shell = ( m_Shell == null ? new RangedWeaponShellObject( OwnerComponent ) : m_Shell); }
			set{ Shell.Copy( value ); }
		}

		[SerializeField]
		private DirectEffectObject m_Effect = null;
		public DirectEffectObject Effect{
			get{ return m_Effect = ( m_Effect == null ? new DirectEffectObject( OwnerComponent ) : m_Effect ); }
			set{ Effect.Copy( value ); }
		}

		protected Transform m_TargetTransform = null;
		public virtual Transform TargetTransform{
			get{ return m_TargetTransform; }
		}

		protected ICECreatureEntity m_TargetEntity = null;
		public virtual ICECreatureEntity TargetEntityComponent{
			get{ return m_TargetEntity = ( m_TargetEntity == null && m_TargetTransform != null ? m_TargetTransform.GetComponent<ICECreatureEntity>() : ( m_TargetTransform == null ? null : m_TargetEntity ) ); }
		}

		protected ICECreatureEntity m_Entity = null;
		public virtual ICECreatureEntity EntityComponent{
			get{ return m_Entity = ( m_Entity == null && OwnerComponent != null ? OwnerComponent.GetComponent<ICECreatureEntity>() : m_Entity ); }
		}


		public override void Init( ICEWorldBehaviour _component ){

			base.Init( _component );

			LaunchSound.Init( _component );
			Ammunition.Init( _component );
			MuzzleFlash.Init( _component );
			Recoil.Init( _component );
			Effect.Init( _component );
		}

		public void Copy( RangedWeaponDataObject _object )
		{
			base.Copy( _object );
		}
	}

	[System.Serializable]
	public class RangedWeaponObject : RangedWeaponDataObject 
	{
		public RangedWeaponObject(){}
		public RangedWeaponObject( ICEWorldBehaviour _component ) : base( _component ){}
		public RangedWeaponObject( RangedWeaponObject _object ) : base( _object ){}

		/// <summary>
		/// Fires a burst if
		/// </summary>
		/// <param name="_target">Target.</param>
		/// <param name="_limit">Limit.</param>
		public void FireBurst( Transform _target, int _limit )
		{
			m_TargetTransform = _target;

			if( TargetEntityComponent != null && EntityComponent != null )
				TargetEntityComponent.AddActiveCounterpart( EntityComponent.RootEntity as ICECreatureEntity );

			if( Automatic.Enabled )
				Automatic.StartWithImpulsLimit( _limit );
			else
				FireOneShot( _target );			
		}

		public void Fire( Transform _target ){
			Fire( _target, true );
		}

		public void Fire( Transform _target, bool _reset )
		{
			//Debug.Log( "TEST: Fire" );

			m_TargetTransform = _target;

			if( TargetEntityComponent != null && EntityComponent != null )
				TargetEntityComponent.AddActiveCounterpart( EntityComponent.RootEntity as ICECreatureEntity );

			if( Automatic.Enabled )
				Automatic.Start( _reset );
			else
				FireOneShot( _target );
		}

		public void Stop()
		{
			if( TargetEntityComponent != null && EntityComponent != null )
				TargetEntityComponent.RemoveActiveCounterpart( EntityComponent.RootEntity as ICECreatureEntity );
			
			if( Automatic.Stop() )
				FireOneShot( m_TargetTransform );
		}

		public void Update()
		{
			if( Automatic.Update() )
				FireOneShot( m_TargetTransform );
			
			MuzzleFlash.Update();
			Shell.Update();
			Effect.Update();
			Recoil.Update();
		}

		/// <summary>
		/// Fires the one shot.
		/// </summary>
		/// <param name="_target">Target.</param>
		public void FireOneShot( Transform _target )
		{
			if( Ammunition.Enabled )
				Ammunition.Fire( _target );

			if( Recoil.Enabled )
				Recoil.Start();			

			if( LaunchSound.Enabled )
				LaunchSound.Play();

			if( MuzzleFlash.Enabled )
				MuzzleFlash.Start();

			if( Shell.Enabled )
				Shell.Start();

			if( Effect.Enabled )
				Effect.Start( OwnerComponent );
		}
	}

}
