// ##############################################################################
//
// ICECreatureZone.cs
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

	public class ICECreatureZone : ICECreatureLocation {

		/// <summary>
		/// Gets the entity classification type of the object
		/// </summary>
		/// <value>The classification type of the entity.</value>
		/// <description>The EntityClassType will be used to quickly 
		/// identify the correct class type of a derived entity object 
		/// without casts
		/// </description>
		public override EntityClassType EntityType{
			get{ return EntityClassType.Zone; }
		}

		protected override void OnRegisterBehaviourEvents()
		{
			//e.g. RegisterBehaviourEvent( "ApplyDamage", BehaviourEventParameterType.Float );
		}

		[SerializeField]
		protected InfluenceDataObject m_Influences = null;
		public virtual InfluenceDataObject Influences{
			get{ return m_Influences = ( m_Influences == null ? new InfluenceDataObject():m_Influences ); }
			set{ Influences.Copy( value ); }
		}

		// Use this for initialization
		public override void Start () {
			base.Start(); 

			if( ObjectCollider != null )
				ObjectCollider.isTrigger = true;
		}

		// Update is called once per frame
		public override void Update() {

		}
			
		public override void OnTriggerEnter( Collider _collider ) 
		{
			if( IsRemoteClient )
				return;

			if( _collider == null )
				return;

			ICECreatureEntity _entity = _collider.gameObject.GetComponent<ICECreatureEntity>();
			if( _entity != null )
			{
				if( DebugLogIsEnabled )
					PrintDebugLog( "OnTriggerEnter - " + _entity.name + " enter zone." );

				ICECreatureZone _zone = _entity as ICECreatureZone;
				if( _zone != null )
					EnterZone( _zone.name );

				ICECreatureControl _creature = _entity as ICECreatureControl;
				if( _creature != null )
					_creature.Creature.UpdateStatusInfluences( Influences );
			}
		}

		public override void OnTriggerStay( Collider _collider ) 
		{
			if( IsRemoteClient )
				return;

			if( _collider == null )
				return;

			ICECreatureEntity _entity = _collider.gameObject.GetComponent<ICECreatureEntity>();
			if( _entity != null )
			{
				if( DebugLogIsEnabled )
					PrintDebugLog( "OnTriggerStay - " + _entity.name + " stays in zone." );

				ICECreatureZone _zone = _entity as ICECreatureZone;
				if( _zone != null )
					EnterZone( _zone.name );

				ICECreatureControl _creature = _entity as ICECreatureControl;
				if( _creature != null )
					_creature.Creature.UpdateStatusInfluences( Influences );					
			}
		}

		public override void OnTriggerExit( Collider _collider ) 
		{
			if( IsRemoteClient )
				return;

			if( _collider == null )
				return;

			ICECreatureEntity _entity = _collider.gameObject.GetComponent<ICECreatureEntity>();
			if( _entity != null )
			{
				if( DebugLogIsEnabled )
					PrintDebugLog( "OnTriggerExit - " + _entity.name + " leaves zone." );

				ICECreatureZone _zone = _entity as ICECreatureZone;
				if( _zone != null )
					EnterZone( _zone.name );

				ICECreatureControl _creature = _entity as ICECreatureControl;
				if( _creature != null )
					_creature.Creature.UpdateStatusInfluences( Influences );
			}
		}
	}
}