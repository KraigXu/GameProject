// ##############################################################################
//
// ICECreatureBreadcrumb.cs
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

	public class ICECreatureBreadcrumb  : ICECreatureMarker  {

		public float DeactivationTime = 2;
		public float DeactivationTimeMaximum = 60;

		/// <summary>
		/// Gets the entity classification type of the object
		/// </summary>
		/// <value>The classification type of the entity.</value>
		/// <description>The EntityClassType will be used to quickly 
		/// identify the correct class type of a derived entity object 
		/// without casts
		/// </description>
		public override EntityClassType EntityType{
			get{ return EntityClassType.Breadcrumb; }
		}

		protected override void OnRegisterBehaviourEvents()
		{
			base.OnRegisterBehaviourEvents();
			//e.g. RegisterBehaviourEvent( "ApplyDamage", BehaviourEventParameterType.Float );
		}

		public override void Start () {
			base.Start(); 

			if( ObjectCollider != null )
				ObjectCollider.isTrigger = true;
		}

		public override void OnEnable () {
			base.OnEnable();
		}

		public override void OnDisable () {
			base.OnDisable();
		}
			
		/// <summary>
		/// Raises the trigger enter event.
		/// </summary>
		/// <param name="_other">Other.</param>
		public override void OnTriggerEnter( Collider _other ) 
		{
			// checks if the other object is a creature 
			if( _other.gameObject.GetComponent<ICECreatureControl>() != null )
			{
				// if the other is a creature run an invoke to reactivate the breadcrumb later ...
				Invoke( "Reactivate", DeactivationTime );

				// ... and deactivate the breadcrumb, so that your creature will lose this breadcrumb as target
				gameObject.SetActive( false );
				//Debug.Log( "Deactivate Breadcrumb" );
			}
		}

		/// <summary>
		/// Reactivate this instance.
		/// </summary>
		public void Reactivate()
		{
			// reactivate the breadcrumb so the next creature could detect it as target
			gameObject.SetActive( true );
			//Debug.Log( "Reactivate Breadcrumb" );
		}
	}
}

