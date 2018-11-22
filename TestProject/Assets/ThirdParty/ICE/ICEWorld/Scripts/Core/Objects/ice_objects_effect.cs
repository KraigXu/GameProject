// ##############################################################################
//
// ice_objects_effect.cs | ICE.World.Objects.EffectDataObject | EffectObject
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
using System.Xml;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;

namespace ICE.World.Objects
{
	[System.Serializable]
	public class DirectEffectDataObject : ICEOwnerObject
	{
		public DirectEffectDataObject(){}
		public DirectEffectDataObject( ICEWorldBehaviour _component ) : base( _component ){}
		public DirectEffectDataObject( DirectEffectDataObject _effect ) : base( _effect )
		{
			Init( _effect.OwnerComponent );

			ReferenceObject = _effect.ReferenceObject;
			Offset = _effect.Offset;
			Rotation = _effect.Rotation;
			OffsetType = _effect.OffsetType;
			OffsetRadius = _effect.OffsetRadius;
			Attached = _effect.Attached;
			MountPointName = _effect.MountPointName;

			Lifetime = _effect.Lifetime;
			LifetimeMaximum = _effect.LifetimeMaximum;
		}

		public override void Init (ICEWorldBehaviour _component)
		{
			base.Init( _component );
		}

		[XmlIgnore]
		public GameObject ReferenceObject = null;

		protected GameObject m_AttachedEffect = null;

		[XmlIgnore]
		public GameObject AttachedEffect{
			get{ return m_AttachedEffect; }
		}

		[XmlIgnore]
		public ParticleSystem AttachedParticleSystem{
			get{  return ( m_AttachedEffect != null ? m_AttachedEffect.GetComponent<ParticleSystem>() : null ); }
		}

		public Vector3 Offset = Vector3.zero;
		public Quaternion Rotation = Quaternion.identity;
		public RandomOffsetType OffsetType = RandomOffsetType.EXACT;
		public float OffsetRadius = 0;
		public float OffsetRadiusMaximum = 15;

		public bool Attached = false;

		public string MountPointName = "";
		protected Transform m_MountPointTransform = null;

		public float Lifetime = 0;
		public float LifetimeMaximum = 60;


	}

	[System.Serializable]
	public class DirectEffectObject : DirectEffectDataObject
	{
		public DirectEffectObject(){}
		public DirectEffectObject( ICEWorldBehaviour _component ) : base( _component ) {}
		public DirectEffectObject( DirectEffectObject _effect ) : base( _effect ){}

		public void Start( ICEWorldBehaviour _component ) 
		{
			if( _component == null || ! Enabled )
				return;
			
			base.Init( _component );	

			if( m_AttachedEffect != null && AttachedParticleSystem != null )
				AttachedParticleSystem.Play();
			else if( m_AttachedEffect == null )
				m_AttachedEffect = InstantiateEffect();
		}

		public void Update() 
		{
		}

		private GameObject InstantiateEffect()
		{
			if( Owner == null || ! Enabled || ReferenceObject == null )
				return null;

			if( ! string.IsNullOrEmpty( MountPointName.Trim() ) && ( m_MountPointTransform == null || m_MountPointTransform.name != MountPointName.Trim() ) )
				m_MountPointTransform = SystemTools.FindChildByName( MountPointName, Owner.transform ); 

			if( m_MountPointTransform == null )
				m_MountPointTransform = Owner.transform;

			Vector3 _position = m_MountPointTransform.position;
			Vector3 _offset = Vector3.zero;

			if( OffsetType == RandomOffsetType.EXACT )
			{
				_offset = Offset;
			}
			else if( OffsetRadius > 0 )
			{
				Vector2 _pos = Random.insideUnitCircle * OffsetRadius;

				_offset.x = _pos.x;
				_offset.z = _pos.y;

				if( OffsetType == RandomOffsetType.HEMISPHERE )
					_offset.y = Random.Range(0,OffsetRadius ); 
				else if( OffsetType == RandomOffsetType.SPHERE )
					_offset.y = Random.Range( - OffsetRadius , OffsetRadius ); 
			}

			_position = PositionTools.FixTransformPoint( m_MountPointTransform, _offset );

			GameObject _effect = WorldManager.Instantiate( ReferenceObject, _position, Rotation );
			if( _effect != null )
			{
				_effect.name = ReferenceObject.name;

				if( m_MountPointTransform != null )
					_effect.transform.rotation = m_MountPointTransform.rotation * Rotation;
				else
					_effect.transform.rotation = Rotation;

				_effect.SetActive( true );

				if( Attached == true && m_MountPointTransform != null && Owner.activeInHierarchy )
				{
					_effect.transform.SetParent( m_MountPointTransform, true );
				}
				else if( Attached == false && Lifetime > 0 )
				{
					WorldManager.Destroy( _effect, Lifetime );
					_effect = null;
				}
			}

			return _effect;
		}
	}


	[System.Serializable]
	public class EffectDataObject : ICEImpulsTimerObject
	{
		public EffectDataObject(){}
		public EffectDataObject( ICEWorldBehaviour _component ) : base( _component ){}
		public EffectDataObject( EffectDataObject _effect ) : base( _effect as ICEImpulsTimerObject )
		{
			Init( _effect.OwnerComponent );

			ReferenceObject = _effect.ReferenceObject;
			Offset = _effect.Offset;
			Rotation = _effect.Rotation;
			OffsetType = _effect.OffsetType;
			OffsetRadius = _effect.OffsetRadius;
			Detach = _effect.Detach;
			MountPointName = _effect.MountPointName;
		}

		public override void Init (ICEWorldBehaviour _component)
		{
			base.Init( _component );
		}
			
		[XmlIgnore]
		public GameObject ReferenceObject = null;
		public Vector3 Offset = Vector3.zero;
		public Quaternion Rotation = Quaternion.identity;
		public RandomOffsetType OffsetType = RandomOffsetType.EXACT;
		public float OffsetRadius = 0;
		public float OffsetRadiusMaximum = 15;

		public bool Detach = false;

		public string MountPointName = "";
		protected Transform m_MountPointTransform = null;
	}

	[System.Serializable]
	public class EffectObject : EffectDataObject
	{
		public EffectObject(){}
		public EffectObject( ICEWorldBehaviour _component ) : base( _component ) {}
		public EffectObject( EffectObject _effect ) : base( _effect as EffectDataObject ){}

		[XmlIgnore]
		private GameObject m_CurrentEffect = null;
		public void Start( ICEWorldBehaviour _component ) 
		{
			base.Init( _component );		
			base.Start();

			/*
	


			if( Enabled == true && ReferenceObject != null )
			{

			}
			else if( Enabled == false && CurrentEffect != null )
			{
				GameObject.Destroy( CurrentEffect );
				CurrentEffect = null;
			}*/
		}

		public override void Stop() 
		{
			base.Stop();

			if( Detach == false && m_CurrentEffect != null )
				m_CurrentEffect.SetActive( false );
			else
				m_CurrentEffect = null;
		}

		protected override void Action()
		{
			InitializeEffect();
		}

		public void InitializeEffect()
		{
			if( Owner == null || ReferenceObject == null )
				return;

			if( m_CurrentEffect == null )
			{
				GameObject _effect = InstantiateNewEffect();

				if( Detach == false )
					m_CurrentEffect = _effect;
			}
			else
				m_CurrentEffect.SetActive( true );
		}

		private GameObject InstantiateNewEffect()
		{
			if( Owner == null )
				return null;

			if( ! string.IsNullOrEmpty( MountPointName.Trim() ) && ( m_MountPointTransform == null || m_MountPointTransform.name != MountPointName.Trim() ) )
				m_MountPointTransform = SystemTools.FindChildByName( MountPointName, Owner.transform ); 

			if( m_MountPointTransform == null )
				m_MountPointTransform = Owner.transform;

			Vector3 _position = m_MountPointTransform.position;
			Vector3 _offset = Vector3.zero;

			if( OffsetType == RandomOffsetType.EXACT )
			{
				_offset = Offset;
			}
			else if( OffsetRadius > 0 )
			{
				Vector2 _pos = Random.insideUnitCircle * OffsetRadius;

				_offset.x = _pos.x;
				_offset.z = _pos.y;

				if( OffsetType == RandomOffsetType.HEMISPHERE )
					_offset.y = Random.Range(0,OffsetRadius ); 
				else if( OffsetType == RandomOffsetType.SPHERE )
					_offset.y = Random.Range( - OffsetRadius , OffsetRadius ); 
			}

			_position = PositionTools.FixTransformPoint( m_MountPointTransform, _offset );

			GameObject _effect = (GameObject)Object.Instantiate( ReferenceObject, _position, Quaternion.identity );

			if( _effect != null )
			{
				_effect.name = ReferenceObject.name;

				if( m_MountPointTransform != null )
					_effect.transform.rotation = m_MountPointTransform.rotation * Rotation;
				else
					_effect.transform.rotation = Rotation;

				if( Detach == false )
					_effect.transform.SetParent( m_MountPointTransform, true );

				_effect.SetActive( true );
			}

			return _effect;
		}
	}
}


