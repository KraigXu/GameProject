// ##############################################################################
//
// ice_objects_impact.cs | ICE.World.Objects.DamageImpactDataObject | ICE.World.Objects.DamageImpactObject
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
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;

namespace ICE.World.Objects
{
	[System.Serializable]
	public class ImpactBehaviourObject : ICEOwnerObject{

		public ImpactBehaviourObject(){}
		public ImpactBehaviourObject( ImpactBehaviourObject _object ) : base( _object ){ Copy( _object ); }
		public ImpactBehaviourObject( ICEWorldBehaviour _component ) : base( _component ){ Init( _component ); }

		public void Copy( ImpactBehaviourObject _object  )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			/*UseImpactDelay = _object.UseImpactDelay;
			ImpactDelayMin = _object.ImpactDelayMin;
			ImpactDelayMax = _object.ImpactDelayMax;
			ImpactDelayMaximum = _object.ImpactDelayMaximum; */

			UseAttachOnHit = _object.UseAttachOnHit;
			UseHideOnHit = _object.UseHideOnHit;
			UseDestroyOnHit = _object.UseDestroyOnHit;
			DestroyingDelayMin = _object.DestroyingDelayMin;
			DestroyingDelayMax = _object.DestroyingDelayMax;
			DestroyingDelayMaximum = _object.DestroyingDelayMaximum;
			HitCountMin = _object.HitCountMin;
			HitCountMax = _object.HitCountMax;
			HitCountMaximum = _object.HitCountMaximum;
			AllowOwnImpacts = _object.AllowOwnImpacts;

			//UseImpactOnDamage = _object.UseImpactOnDamage;
		}

		[SerializeField]
		private LayerObject m_ImpactLayer = null;
		public LayerObject ImpactLayer{
			get{ return m_ImpactLayer = ( m_ImpactLayer == null ? new LayerObject() : m_ImpactLayer); }
			set{ ImpactLayer.Copy( value ); }
		}

		private int m_HitCounter = 0;
		private int m_HitCount = 0;
		public int IgnoreHitsCount{
			get{ return m_HitCount = ( m_HitCount == 0 ? UnityEngine.Random.Range( HitCountMin, HitCountMax ) : m_HitCount ); }
		}

		public bool UseIgnoreHits = false;
		public int HitCountMin = 0;
		public int HitCountMax = 0;
		public int HitCountMaximum = 10;
		/*
		public bool UseImpactDelay = false;
		public float ImpactDelayMin = 2f;
		public float ImpactDelayMax = 2f;
		public float ImpactDelayMaximum = 30f;*/


		public bool UseAttachOnHit = false;
		public bool UseHideOnHit = false;
		public bool UseDestroyOnHit = false;
		public float DestroyingDelayMin = 2f;
		public float DestroyingDelayMax = 2f;
		public float DestroyingDelayMaximum = 30f;
		public bool AllowOwnImpacts = false;
		//public bool UseImpactOnDamage = true;

		public bool UseDestroyOnDurabilityLimit = true;

		public float DestroyDelay{
			get{ return UnityEngine.Random.Range( DestroyingDelayMin, IgnoreHitsCount ); }
		}

		public bool RefusedByLayer( int _layer ){
			return ( ImpactLayer.Enabled && ImpactLayer.Layers.Count > 0 && ! ImpactLayer.Contains( _layer ) ? true : false );		
		}

		public bool RefusedByHitCounter{
			get{
				if( UseIgnoreHits && IgnoreHitsCount > 0 )
				{
					m_HitCounter++;
					if( m_HitCounter >= IgnoreHitsCount )
					{
						m_HitCount = 0;
						m_HitCounter = 0;
					}
					else
						return true;
				}	

				return false;
			}
		}
	}

	[System.Serializable]
	public class ImpactDataObject : ICEOwnerObject 
	{
		public ImpactDataObject(){}
		public ImpactDataObject( ICEWorldBehaviour _component ) : base( _component ){ Init( _component ); }
		public ImpactDataObject( ImpactDataObject _object ){ Copy( _object ); }

		public override void Init( ICEWorldBehaviour _component ){

			base.Init( _component );

			ImpactSound.Init( _component );
			ImpactEffect.Init( _component );
			ImpactBehaviour.Init( _component );
		}

		public override void Reset()
		{
			ImpactSound.Reset();
			ImpactEffect.Reset();
			ImpactBehaviour.Reset();
		}

		public void Copy( ImpactDataObject _object )
		{
			base.Copy( _object );

			ImpactType = _object.ImpactType;

			ImpactSound = _object.ImpactSound;
			ImpactEffect = _object.ImpactEffect;
			ImpactBehaviour = _object.ImpactBehaviour;

			DamageMethodName = _object.DamageMethodName;
			DamageMethodValue = _object.DamageMethodValue;
			DamageMethodValueMaximum = _object.DamageMethodValueMaximum;

			ForceType = _object.ForceType;
			ForceMin = _object.ForceMin;
			ForceMax = _object.ForceMax;
			ForceMaximum = _object.ForceMaximum;
			ExplosionRadius = _object.ExplosionRadius;
			ExplosionRadiusMaximum = _object.ExplosionRadiusMaximum;

		}

		public DamageTransferType ImpactType = DamageTransferType.Direct;

		public string DamageMethodName = "ApplyDamage";
		public float DamageMethodValue = 10;
		public float DamageMethodValueMaximum = 100;
		public DamageForceType ForceType = DamageForceType.None;
		public float ForceMin = 100;
		public float ForceMax = 100;
		public float ForceMaximum = 1000;
		public float ExplosionRadius = 5;
		public float ExplosionRadiusMaximum = 5000;

			
		[SerializeField]
		private DirectAudioPlayerObject m_ImpactSound = null;
		public DirectAudioPlayerObject ImpactSound{
			get{ return m_ImpactSound = ( m_ImpactSound == null ? new DirectAudioPlayerObject( OwnerComponent ) : m_ImpactSound); }
			set{ ImpactSound.Copy( value ); }
		}

		[SerializeField]
		private DirectEffectObject m_ImpactEffect = null;
		public DirectEffectObject ImpactEffect{
			get{ return m_ImpactEffect = ( m_ImpactEffect == null ? new DirectEffectObject( OwnerComponent ) : m_ImpactEffect); }
			set{ ImpactEffect.Copy( value ); }
		}

		[SerializeField]
		private ImpactBehaviourObject m_ImpactBehaviour = null;
		public ImpactBehaviourObject ImpactBehaviour{
			get{ return m_ImpactBehaviour = ( m_ImpactBehaviour == null ? new ImpactBehaviourObject() : m_ImpactBehaviour ); }
			set{ ImpactBehaviour.Copy( value ); }
		}
	}

	[System.Serializable]
	public class BasicImpactObject : ImpactDataObject 
	{
		public BasicImpactObject(){}
		public BasicImpactObject( ICEWorldBehaviour _component ) : base( _component ){ Init( _component ); }
		public BasicImpactObject( BasicImpactObject _object ){ Copy( _object ); }

		public virtual void Hit( GameObject _target, Vector3 _hit_point )
		{
			if( Owner == null || Enabled == false )
				return;

			if( _target != null && ImpactBehaviour.RefusedByLayer( _target.layer ) )
				return;

			ICEWorldEntity.SendDamage( Owner, _target, ImpactType, DamageMethodValue, DamageMethodName, _hit_point, ForceType, UnityEngine.Random.Range( ForceMin, ForceMax ), ExplosionRadius );
			ImpactSound.Play();
			ImpactEffect.Start( OwnerComponent );
		}
	}

	[System.Serializable]
	public class ImpactObject : BasicImpactObject 
	{
		public ImpactObject(){}
		public ImpactObject( ICEWorldBehaviour _component ) : base( _component ){ Init( _component ); }
		public ImpactObject( ImpactObject _object ){ Copy( _object ); }

		public override void Hit( GameObject _target, Vector3 _hit_point )
		{
			if( Owner == null || Enabled == false )
				return;

			if( ( _target != null && ImpactBehaviour.RefusedByLayer( _target.layer ) ) || ImpactBehaviour.RefusedByHitCounter )
				return;

			ICEWorldEntity.SendDamage( Owner, _target, ImpactType, DamageMethodValue, DamageMethodName, _hit_point, ForceType, UnityEngine.Random.Range( ForceMin, ForceMax ), ExplosionRadius );
			ImpactSound.Play();
			ImpactEffect.Start( OwnerComponent );

			if( ImpactBehaviour.Enabled )
			{
				// Handle Projectile Rigidbody 
				Rigidbody _rb = Owner.GetComponent<Rigidbody>();
				if( _rb != null )
				{
					_rb.useGravity = false;
					_rb.isKinematic = true;
					_rb.constraints = RigidbodyConstraints.FreezeAll;
				}

				if( ImpactBehaviour.UseAttachOnHit )
					WorldManager.AttachToTransform( Owner, _target.transform, false );

				if( ImpactBehaviour.UseHideOnHit )
					SystemTools.EnableRenderer( Owner.transform, false );

				if( ImpactBehaviour.UseDestroyOnHit )
					WorldManager.Destroy( Owner, ImpactBehaviour.DestroyDelay );
			}
		}
	}
}

