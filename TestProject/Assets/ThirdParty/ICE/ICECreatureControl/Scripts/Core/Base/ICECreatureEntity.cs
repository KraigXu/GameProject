// ##############################################################################
//
// ICECreatureTarget.cs
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

namespace ICE.Creatures{
		
	/// <summary>
	/// ICE creature entity. ICECreatureEntity is a derivated class of the ICEWorldEntity and the 
	/// base class for all creature objects.
	/// </summary>
	public abstract class ICECreatureEntity : ICEWorldEntity {

		/// <summary>
		/// Gets the entity classification type of the object
		/// </summary>
		/// <value>The classification type of the entity.</value>
		/// <description>The EntityClassType will be used to quickly 
		/// identify the correct class type of a derived entity object 
		/// without casts
		/// </description>
		public override EntityClassType EntityType{
			get{ return EntityClassType.Entity; }
		}

		public override bool CompareType( EntityClassType _type ){
			return ( EntityType != _type || base.CompareType( _type ) ? true : false );
		}

		protected override void OnRegisterBehaviourEvents()
		{
			base.OnRegisterBehaviourEvents();
			//e.g. RegisterBehaviourEvent( "ApplyDamage", BehaviourEventParameterType.Float );
		}

		private List<ICECreatureEntity> m_ActiveCounterparts = null;
		public List<ICECreatureEntity> ActiveCounterparts{
			get{ return m_ActiveCounterparts = ( m_ActiveCounterparts == null ? new List<ICECreatureEntity>() : m_ActiveCounterparts ); }
		}


		/*
		private InventoryObject m_Inventory = null;
		public InventoryObject EntityInventory{
			get{ 


			}
		}*/

		public void AddActiveCounterpart( ICECreatureEntity _entity )
		{
			if( _entity == null )
				return;
			
			if( ! ActiveCounterpartExists( _entity ) )
				ActiveCounterparts.Add( _entity );

			//Debug.Log( this.name + " add target " + _entity.name + " (" + _entity.ObjectInstanceID + ") Count: " + FocusedEntities.Count  );
		}

		public void RemoveActiveCounterpart( ICECreatureEntity _entity )
		{
			if( _entity == null || ActiveCounterparts.Count == 0 )
				return;

			ActiveCounterparts.Remove( _entity );

			//Debug.Log( this.name + " remove target " + _entity.name + " (" + _entity.ObjectInstanceID + ") Count: " + FocusedEntities.Count );
		}

		public bool ActiveCounterpartExists( ICECreatureEntity _entity )
		{
			if( _entity == null || _entity == this )
				return true;

			if( ActiveCounterparts.IndexOf( _entity ) > -1 )
				return true;

			return false;
		}

		/// <summary>
		/// Gets the nearest focused entity.
		/// </summary>
		/// <returns>The nearest focused entity.</returns>
		/// <param name="_allow_child">If set to <c>true</c> allow child.</param>
		public ICECreatureEntity GetNearestActiveCounterparts( bool _allow_child = false )
		{

			ICECreatureEntity _best_entity = null;
			float _best_distance = Mathf.Infinity;

			// transform buffer
			Transform _transform = this.transform;

			for( int i = 0; i < ActiveCounterparts.Count; i++ )
			{
				ICECreatureEntity _entity = ActiveCounterparts[i];

				if( _entity != null )
				{
					// transform buffer
					Transform _entity_transform = _entity.transform;

					float _distance = PositionTools.Distance( _transform.position, _entity_transform.position );					
					if( _distance < _best_distance )
					{
						if( _allow_child || _entity_transform.IsChildOf( _transform ) == false )
						{
							_best_distance = _distance;	
							_best_entity = _entity;
						}
					}						
				}
			}

			return _best_entity;
		}

		/// <summary>
		/// Gets the nearest focused entity by the specified name.
		/// </summary>
		/// <returns>The nearest focused entity by name.</returns>
		/// <param name="_name">Name.</param>
		/// <param name="_allow_child">If set to <c>true</c> allow child.</param>
		public ICECreatureEntity GetBestCounterpartByName( string _name, int _max_counterparts, bool _allow_child = false  )
		{
			ICECreatureEntity _best_entity = null;
			float _best_distance = Mathf.Infinity;
			int _best_counterparts = _max_counterparts;

			// transform buffer
			Transform _transform = this.transform;

			for( int i = 0; i < ActiveCounterparts.Count; i++ )
			{
				ICECreatureEntity _entity = ActiveCounterparts[i];

				if( _entity != null && _entity.CompareName( _name ) )
				{
					Transform _entity_transform = _entity.transform;
					int _entity_counterparts = _entity.ActiveCounterparts.Count;
					float _entity_distance = PositionTools.Distance( _transform.position, _entity_transform.position );

					if( ( _entity_distance <= _best_distance ) &&
						( _allow_child || _entity_transform.IsChildOf( _transform ) == false ) &&
						( _best_counterparts == -1 || _entity_counterparts <= _best_counterparts ) )
					{
						_best_counterparts = _entity_counterparts;
						_best_distance = _entity_distance;	
						_best_entity = _entity;
					}					
				}
			}

			return _best_entity;
		}

		/// <summary>
		/// Gets the nearest focused entity by tag.
		/// </summary>
		/// <returns>The nearest focused entity by tag.</returns>
		/// <param name="_tag">Tag.</param>
		/// <param name="_allow_child">If set to <c>true</c> allow child.</param>
		public ICECreatureEntity GetBestCounterpartByTag( string _tag, int _max_counterparts, bool _allow_child = false  )
		{
			ICECreatureEntity _best_entity = null;
			float _best_distance = Mathf.Infinity;
			int _best_counterparts = _max_counterparts;

			// transform buffer
			Transform _transform = this.transform;

			for( int i = 0; i < ActiveCounterparts.Count; i++ )
			{
				ICECreatureEntity _entity = ActiveCounterparts[i];

				if( _entity != null && _entity.CompareTag( _tag ) )
				{
					Transform _entity_transform = _entity.transform;
					int _entity_counterparts = _entity.ActiveCounterparts.Count;
					float _entity_distance = PositionTools.Distance( _transform.position, _entity_transform.position );

					if( ( _entity_distance <= _best_distance ) &&
						( _allow_child || _entity_transform.IsChildOf( _transform ) == false ) &&
						( _best_counterparts == -1 || _entity_counterparts <= _best_counterparts ) )
					{
						_best_counterparts = _entity_counterparts;
						_best_distance = _entity_distance;	
						_best_entity = _entity;
					}						
				}
			}

			return _best_entity;
		}

		/// <summary>
		/// Gets the nearest focused entity by the specified type.
		/// </summary>
		/// <returns>The nearest focused entity by type.</returns>
		/// <param name="_type">Type.</param>
		/// <param name="_allow_child">If set to <c>true</c> allow child.</param>
		public ICECreatureEntity GetBestCounterpartByType( EntityClassType _type, int _max_counterparts, bool _allow_child = false  )
		{
			ICECreatureEntity _best_entity = null;
			float _best_distance = Mathf.Infinity;
			int _best_counterparts = _max_counterparts;

			// transform buffer
			Transform _transform = this.transform;

			for( int i = 0; i < ActiveCounterparts.Count; i++ )
			{
				ICECreatureEntity _entity = ActiveCounterparts[i];

				if( _entity != null && _entity.EntityType == _type )
				{
					Transform _entity_transform = _entity.transform;
					int _entity_counterparts = _entity.ActiveCounterparts.Count;
					float _entity_distance = PositionTools.Distance( _transform.position, _entity_transform.position );

					if( ( _entity_distance <= _best_distance ) &&
						( _allow_child || _entity_transform.IsChildOf( _transform ) == false ) &&
						( _best_counterparts == -1 || _entity_counterparts <= _best_counterparts ) )
					{
						_best_counterparts = _entity_counterparts;
						_best_distance = _entity_distance;	
						_best_entity = _entity;
					}
				}
			}

			return _best_entity;
		}

		public bool CompareEntity( ICECreatureEntity _entity_1, ICECreatureEntity _entity_2 ){
			return ( _entity_1 == null || _entity_2 == null || _entity_1 == this || _entity_2 == this || _entity_1 != _entity_2 || _entity_1.ObjectInstanceID != _entity_2.ObjectInstanceID ? false : true );
		}

		public bool CompareName( string _name ){
			return ( SystemTools.CleanName( this.name ) == SystemTools.CleanName( _name ) ? true : false );
		}

		[SerializeField]
		private MessageObject m_Message = null;
		public MessageObject Message{
			get{ return m_Message = ( m_Message == null ? new MessageObject() :m_Message); }
			set{ Message.Copy( value ); }
		}

		protected Vector3 m_LastPosition = Vector3.zero;
		public Vector3 LastPosition{
			get{ return m_LastPosition; }
		}

		protected Quaternion m_LastRotation = Quaternion.identity;
		public Quaternion LastRotation{
			get{ return m_LastRotation; }
		}

		private Vector3 m_Velocity = Vector3.zero;
		public Vector3 Velocity{
			get{ return m_Velocity; }
		}

		private Vector3 m_FrameVelocity = Vector3.zero;
		public Vector3 FrameVelocity{
			get{ return m_FrameVelocity; }
		}

		/// <summary>
		/// Awake this instance and runs the registration process. If you override 
		/// this method please make sure to call base.Register or register the target
		/// by your own code.
		/// </summary>
		public override void Awake () {

			base.Awake();

			Message.Init( this );
			RuntimeBehaviour.RuntimeRenaming( this );
			Register();

			m_LastPosition = transform.position;
			m_LastRotation = transform.rotation;
		}

		public override void OnEnable () {
			base.OnEnable();

		}

		public override void OnDisable () {
			base.OnDisable();
		}

		/// <summary>
		/// Raises the destroy event and runs the deregistration process. If you override 
		/// this method please make sure to call base.Deregister or deregister the target
		/// by your own code.
		/// </summary>
		public override void OnDestroy() {

			Deregister();
			base.OnDestroy();
		}

		protected override void Register()
		{
			Message.SetReferenceGroup( CreatureRegister.Register( transform.gameObject ) );
		}

		protected override void Deregister()
		{
			if( CreatureRegister.Deregister( transform.gameObject ) )
				Message.SetReferenceGroup( null );
		}

		/// <summary>
		/// Removes this instance according to the defined reference group settings of the 
		/// CreatureRegister. In cases UseSoftRespawn is active the target will be dactivate, 
		/// stored and prepared for its next action, otherwise the object will be destroyed.
		/// </summary>
		protected override void Remove()
		{
			CreatureRegister.Remove( transform.gameObject );
		}

		public override void Update()
		{
			if( RuntimeBehaviour.IsLost == true || RuntimeBehaviour.CullingOptions.CheckCullingConditions( this ) == true )
				Remove();
			
			base.Update();
			DoUpdate();
		}

		public override void LateUpdate()
		{
			// calulates the current velocity of the entity
			m_FrameVelocity = ( ObjectTransform.position - m_LastPosition ) / Time.deltaTime;
			m_Velocity = Vector3.Lerp( m_Velocity, m_FrameVelocity, 0.1f );
			m_LastPosition = ObjectTransform.position;

			// calulates the current angular velocity of the entity
			m_LastRotation = transform.rotation;

			DoLateUpdate();
		}

		private List<string> m_Zones = new List<string>();

		public bool IsInZone( string _name )
		{
			foreach( string _zone in m_Zones )
				if( _zone == _name ) return true;

			return false;
		}

		public void EnterZone( string _name ){
			if( ! IsInZone( _name ) ) 
				m_Zones.Add( _name );
		}

		public void ExitZone( string _name ){
			m_Zones.Remove( _name );
		}

		public override void OnTriggerEnter( Collider _collider ) 
		{
			if( IsRemoteClient )
				return;

			if( _collider == null )
				return;

			ICECreatureZone _zone = _collider.gameObject.GetComponent<ICECreatureZone>();
			if( _zone != null )
				EnterZone( _zone.name );
		}

		public override void OnTriggerStay( Collider _collider ) 
		{
			if( IsRemoteClient )
				return;

			if( _collider == null )
				return;

		}

		public override void OnTriggerExit( Collider _collider ) 
		{
			if( IsRemoteClient )
				return;

			if( _collider == null )
				return;

			ICECreatureZone _zone = _collider.gameObject.GetComponent<ICECreatureZone>();
			if( _zone != null )
				ExitZone( _zone.name );
		}
	}
}