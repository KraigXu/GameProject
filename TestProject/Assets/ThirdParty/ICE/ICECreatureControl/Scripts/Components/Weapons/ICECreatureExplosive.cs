// ##############################################################################
//
// ICECreatureExplosive.cs
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

	public class ICECreatureExplosive : ICECreatureWeapon {

		/// <summary>
		/// Gets the entity classification type of the object
		/// </summary>
		/// <value>The classification type of the entity.</value>
		/// <description>The EntityClassType will be used to quickly 
		/// identify the correct class type of a derived entity object 
		/// without casts
		/// </description>
		public override EntityClassType EntityType{
			get{ return EntityClassType.Explosive; }
		}

		protected override void OnRegisterBehaviourEvents()
		{
			base.OnRegisterBehaviourEvents();
			//e.g.RegisterBehaviourEvent( "ApplyDamage", BehaviourEventParameterType.Float );
		}

		[SerializeField]
		private ICE.World.Objects.ExplosiveObject m_Explosive = null;
		public ICE.World.Objects.ExplosiveObject Explosive{
			get{ return m_Explosive = ( m_Explosive == null ? new ICE.World.Objects.ExplosiveObject(this) : m_Explosive ); }
			set{ Explosive.Copy( value ); }
		}

		public override void OnEnable () {
			base.OnEnable();

			Explosive.Init( this );
		}

		public override void OnDisable()
		{
			if( Status.IsDestroyed && Explosive.DetonateOnDestroyed )
				Impact.Hit( null, Vector3.zero );

			base.OnDisable();
			Explosive.Reset();
		}

		public override void OnDestroy(){
			base.OnDestroy();

			if( Status.IsDestroyed && Explosive.DetonateOnDestroyed )
				Impact.Hit( null, Vector3.zero );
		}
			
		/// <summary>
		/// Hit the specified _object.
		/// </summary>
		/// <param name="_object">Object.</param>
		public override void Hit( GameObject  _target, Vector3 _point )
		{
			if( _target == null || IsPartOfRoot( _target.transform ) || ! Explosive.DetonateOnContact )
				return;

			Impact.Hit( _target, _point );
		}
	}
}
