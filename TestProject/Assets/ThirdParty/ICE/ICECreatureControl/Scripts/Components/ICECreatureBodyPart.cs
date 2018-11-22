// ##############################################################################
//
// ICECreatureBodyPart.cs
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

	public class ICECreatureBodyPart : ICECreatureEntity {

		/// <summary>
		/// Gets the entity classification type of the object
		/// </summary>
		/// <value>The classification type of the entity.</value>
		/// <description>The EntityClassType will be used to quickly 
		/// identify the correct class type of a derived entity object 
		/// without casts</description>
		public override EntityClassType EntityType{
			get{ return EntityClassType.BodyPart; }
		}

		/// <summary>
		/// Register public methods. Override this method to register your own methods by using the RegisterPublicMethod();
		/// </summary>
		protected override void OnRegisterBehaviourEvents()
		{
			base.OnRegisterBehaviourEvents();
			//e.g.RegisterBehaviourEvent( "ApplyDamage", BehaviourEventParameterType.Float );
			RegisterBehaviourEvent( "EnableCollider" );
			RegisterBehaviourEvent( "DisableCollider" );
			RegisterBehaviourEvent( "ActivateColliders", BehaviourEventParameterType.Boolean );

			RegisterBehaviourEvent( "EnableImpact" );
			RegisterBehaviourEvent( "DisableImpact" );
			RegisterBehaviourEvent( "ActivateImpact", BehaviourEventParameterType.Boolean );
		}

		[SerializeField]
		private BasicImpactObject m_Impact = null;
		public BasicImpactObject Impact{
			get{ return m_Impact = ( m_Impact == null ? new BasicImpactObject(this) : m_Impact ); }
			set{ Impact.Copy( value ); }
		}

		[SerializeField]
		private BodyPartMotionObject m_Motion = null;
		public BodyPartMotionObject Motion{
			get{ return m_Motion = ( m_Motion == null ? new BodyPartMotionObject(this) : m_Motion ); }
			set{ Motion.Copy( value ); }
		}
			
		public Collider[] Colliders{
			get{ return this.gameObject.GetComponents<Collider>(); }
		}

		public override void Start () {
			base.Start();

			Impact.Init( this );
			Motion.Init( this );
		}

		public override void OnEnable() {
			base.OnEnable();

			Impact.Init( this );
			Motion.Init( this );
		}

		public override void OnDisable() {
			base.OnDisable();

			Impact.Reset();
			Motion.Reset();
		}

		public override void Update() {
			base.Update();

			Motion.Update();
		}

		/// <summary>
		/// Raises the trigger enter event.
		/// </summary>
		/// <param name="_other">Other.</param>
		public override void OnTriggerEnter( Collider _other ){
			base.OnTriggerEnter( _other );
			Hit( _other.gameObject, _other.ClosestPointOnBounds( transform.position ) );
		}

		/// <summary>
		/// Raises the collision enter event.
		/// </summary>
		/// <param name="_collision">Collision.</param>
		public override void OnCollisionEnter( Collision _collision ){
			base.OnCollisionEnter( _collision );
			Hit( _collision.gameObject, _collision.collider.ClosestPointOnBounds( transform.position ) );		
		}

		public void EnableImpact(){
			ActivateImpact( true );
		}

		public void DisableImpact(){
			ActivateImpact( false );
		}

		public void ActivateImpact( bool _enabled ){
			Impact.Enabled = _enabled;
		}

		public void EnableCollider(){
			ActivateColliders( true );
		}

		public void DisableCollider(){
			ActivateColliders( false );
		}

		public void ActivateColliders( bool _enabled ){

			Collider[] _colliders = this.gameObject.GetComponents<Collider>();
			if( _colliders == null )
				return;

			foreach( Collider _collider in _colliders )
				_collider.enabled = _enabled;
		}

		/// <summary>
		/// Hit the specified _object.
		/// </summary>
		/// <param name="_object">Object.</param>
		public virtual void Hit( GameObject  _target, Vector3 _point )
		{
			if( _target == null || ( ! Impact.ImpactBehaviour.AllowOwnImpacts && IsPartOfRoot( _target.transform ) ) )
				return;

			Impact.Hit( _target, _point );
		}
	}
}
