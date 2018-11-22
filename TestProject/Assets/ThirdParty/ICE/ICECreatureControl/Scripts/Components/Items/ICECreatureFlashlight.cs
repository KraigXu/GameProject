// ##############################################################################
//
// ICECreatureFlashlight.cs
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

	public class ICECreatureFlashlight : ICECreatureItem {

		/// <summary>
		/// Gets the entity classification type of the object
		/// </summary>
		/// <value>The classification type of the entity.</value>
		/// <description>The EntityClassType will be used to quickly 
		/// identify the correct class type of a derived entity object 
		/// without casts
		/// </description>
		public override EntityClassType EntityType{
			get{ return EntityClassType.Flashlight; }
		}

		protected override void OnRegisterBehaviourEvents()
		{
			if( Flashlight.Enabled )
			{
				RegisterBehaviourEvent("FlashlightOn");
				RegisterBehaviourEvent("FlashlightOff");
				RegisterBehaviourEvent("FlashlightToggle");
			}
		}

		[SerializeField]
		private FlashlightObject m_Flashlight = null;
		public FlashlightObject Flashlight{
			get{ return m_Flashlight = ( m_Flashlight == null ? new FlashlightObject( this ):m_Flashlight ); }
			set{ Flashlight.Copy( value ); }
		}

		public override void Start () {

			base.Start();
			Flashlight.Init( this );
		}

		public override void Update() {

			base.Update();
			Flashlight.Update();
		}

		public virtual void FlashlightOn(){
			Flashlight.IsActive = true;
		}

		public virtual void FlashlightOff(){
			Flashlight.IsActive = false;
		}

		public virtual void FlashlightToggle(){
			Flashlight.IsActive = ! Flashlight.IsActive;
		}

	}
}