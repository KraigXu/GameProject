// ##############################################################################
//
// ice_objects_status.cs | EntityStatusObject
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

namespace ICE.World.Objects
{
	[System.Serializable]
	public class EntityBodyPartObject : ICEOwnerObject {

		public EntityBodyPartObject(){}
		public EntityBodyPartObject( ICEWorldBehaviour _component ) : base( _component )
		{
			Init( _component );
		}

		public override void Init( ICEWorldBehaviour _component )
		{
			base.Init( _component );
		}

		public float DamageMultiplier = 1;
		public float DamageMultiplierMaximum = 100;
		public bool UseDamageTransfer = true;
		public bool DamageTransferRequired{
			get{
				if( OwnerComponent != null && OwnerComponent.transform.root != OwnerComponent.transform && UseDamageTransfer )
					return true;
				else
					return false;
			}
		}
	}

	[System.Serializable]
	public class EntityStatusObject : LifespanObject {

		public EntityStatusObject(){}
		public EntityStatusObject( EntityStatusObject _object ) : base( _object ){ Copy( _object ); }
		public EntityStatusObject( ICEWorldBehaviour _component ) : base( _component )
		{
			Init( _component );
		}

		public delegate void OnRemoveEvent( GameObject _sender );
		public event OnRemoveEvent OnRemove;

		public delegate void OnAddDamageEvent( GameObject _sender, float _durability, float _damage );
		public event OnAddDamageEvent OnAddDamage;

		/* TODO: UNDER CONSTRUCTION 
		[SerializeField]
		private DurabilityCompositionObject m_DurabilityComposition = null;
		public DurabilityCompositionObject DurabilityComposition{
			get{ return m_DurabilityComposition = ( m_DurabilityComposition == null ? new DurabilityCompositionObject() : m_DurabilityComposition ); }
			set{ m_DurabilityComposition = value; }
		}*/

		public float MassMin = 1;
		public float MassMax = 1;
		public float MassMaximum = 100;

		private float m_Mass = 0;
		public float Mass{
			get{ return m_Mass = Mathf.Clamp( m_Mass < 0.1f ? UnityEngine.Random.Range( MassMin, MassMax ) : m_Mass, 0.1f, Mathf.Max( 0.1f, MassMaximum ) ); }
		}

		[SerializeField]
		private CorpseObject m_Corpse = null;
		public CorpseObject Corpse{
			get{ return m_Corpse = ( m_Corpse == null ? new CorpseObject( OwnerComponent ) : m_Corpse ); }
			set{ Corpse.Copy( value ); }
		}

		[SerializeField]
		private OdourObject m_Odour = null;
		public OdourObject Odour{
			get{ return m_Odour = ( m_Odour == null ? new OdourObject( OwnerComponent ) : m_Odour ); }
			set{ Odour.Copy( value ); }
		}
			
		public bool UseDamageTransfer = true;
		public float DamageTransferMultiplier = 1;
		public float DamageTransferMultiplierMaximum = 100;

		[SerializeField]
		private bool m_IsDestructible = true;
		public bool IsDestructible{
			get{ return ( Enabled == true && m_IsDestructible == true ? true : false ); }
			set{ m_IsDestructible = value; }
		}

		protected float m_Durability = 100;
		public virtual float Durability{
			get{ 
				m_Durability = Mathf.Clamp( m_Durability, 0, m_InitialDurability );

				return m_Durability; 
			}
		}

		public virtual float DurabilityInPercent{
			get{ return FixedPercent( m_InitialDurability > 0 ? 100 / m_InitialDurability * Durability:100 ); }
		}

		public virtual float InitialDurabilityMultiplier{
			get{ return ( m_InitialDurability > 0?100/m_InitialDurability:1 ); }
		}

		protected float m_InitialDurability = 100;
		public virtual float InitialDurability{
			get{ return m_InitialDurability; }
		}
		public float InitialDurabilityMin = 100;
		public float InitialDurabilityMax = 100;
		public float InitialDurabilityMaximum = 100;

		public virtual void SetInitialDurability( float _value ){
			m_InitialDurability = Mathf.Clamp( _value, InitialDurabilityMin, InitialDurabilityMax );
		}

		public virtual void SetDurability( float _value ){
			m_Durability = Mathf.Clamp( _value, 0, m_InitialDurability );			
		}

		public virtual void UpdateDurabilityByPercent( float _percent ){

			_percent = FixedPercent( _percent );

			m_Durability = m_InitialDurability / 100 * _percent;	
		}

		/// <summary>
		/// Gets a value indicating whether this instance is destroyed.
		/// </summary>
		/// <value><c>true</c> if this instance is destroyed; otherwise, <c>false</c>.</value>
		public virtual bool IsDestroyed{
			get{ return ( IsDestructible && Durability <= 0 ? true:false ); }
		}
			
		public override void Init( ICEWorldBehaviour _component )
		{
			base.Init( _component );

			Corpse.Init( OwnerComponent );
			Odour.Init( OwnerComponent );

			m_InitialDurability = Random.Range( InitialDurabilityMin, InitialDurabilityMax );
			m_Durability = m_InitialDurability;

			PrintDebugLog( this, "Init" );
		}

		public void Copy( EntityStatusObject _object  )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			IsDestructible = _object.IsDestructible;
			InitialDurabilityMin = _object.InitialDurabilityMin;
			InitialDurabilityMax = _object.InitialDurabilityMax;
			InitialDurabilityMaximum = _object.InitialDurabilityMaximum;

			Corpse.Copy( _object.Corpse );
			Odour.Copy( _object.Odour );
		}

		public override void Reset()
		{
			base.Reset();

			m_InitialDurability = Random.Range( InitialDurabilityMin, InitialDurabilityMax );
			m_Durability = m_InitialDurability;

			PrintDebugLog( this, "Reset" );
		}

		public override void Update(){
			base.Update();

			Odour.HandleOdourMarker( Owner.transform );
		}

		public virtual void Remove()
		{
			if( DebugLogIsEnabled ) PrintDebugLog( this, "Remove - Durability :" + m_Durability );

			if( OnRemove != null )
				OnRemove( Owner );

			// Spawn a defined corpse if required
			Corpse.SpawnCorpse();

			// Removes the entity from the world
			WorldManager.Remove( Owner );
		}

		/// <summary>
		/// Processes the received damage.
		/// </summary>
		/// <returns>The damage.</returns>
		/// <param name="_damage">Damage.</param>
		public virtual void AddDamage( float _damage )
		{
			if( ! IsDestructible )
				return;
			
			m_Durability -= _damage;

			if( m_Durability < 0 )
				m_Durability = 0;

			if( OnAddDamage != null )
				OnAddDamage( Owner, m_Durability, _damage );

			if( DebugLogIsEnabled ) PrintDebugLog( this, "AddDamage - " + ( IsDestructible && m_InitialDurability > 0 ? "InitialDurability :" + m_InitialDurability +" - Durability :" + m_Durability  + " - Damage :" + _damage : "disabled" ) );
		}
	}
}
