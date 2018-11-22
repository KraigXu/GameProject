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

	public class ICECreatureTorch : ICECreatureItem {

		/// <summary>
		/// Gets the entity classification type of the object
		/// </summary>
		/// <value>The classification type of the entity.</value>
		/// <description>The EntityClassType will be used to quickly 
		/// identify the correct class type of a derived entity object 
		/// without casts
		/// </description>
		public override EntityClassType EntityType{
			get{ return EntityClassType.Torch; }
		}

		protected override void OnRegisterBehaviourEvents()
		{
			if( Fire.Enabled )
			{
				RegisterBehaviourEvent("FireOn");
				RegisterBehaviourEvent("FireOff");
				RegisterBehaviourEvent("FireToggle");
			}
		}

		[SerializeField]
		private FireObject m_Fire = null;
		public FireObject Fire{
			get{ return m_Fire = ( m_Fire == null ? new FireObject( this ):m_Fire ); }
			set{ Fire.Copy( value ); }
		}

		public override void Start () {

			base.Start();
			Fire.Init( this );
		}

		public override void Update () {

			base.Update();
			Fire.Update();
		}

		public virtual void FireOn(){
			Fire.IsActive = true;
		}

		public virtual void FireOff(){
			Fire.IsActive = false;
		}

		public virtual void FireToggle(){
			Fire.IsActive = ! Fire.IsActive;
		}

	}
}